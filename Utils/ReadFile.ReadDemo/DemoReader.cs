using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BusinessFacade.Repositories;
using CsStat.Domain.Definitions;
using CsStat.Domain.Entities.Demo;
using CsStat.SystemFacade;
using CsStat.SystemFacade.Extensions;
using DemoInfo;
using ReadFile.ReadDemo.Model;

namespace ReadFile.ReadDemo
{
    public class DemoReader : BaseWatcher
    {
        private static DemoParser _parser;
        private static Result _results;

        private static string _currentDemoFileName;

        private static bool _matchStarted;
        private static Round _currentRound;
        private static int _currentRoundNumber;
        private static int _roundEndedCount;

        private static int _lastTScore;
        private static int _lastCTScore;

        #region inint
        private readonly string path;
        private readonly IFileRepository<DemoFile> demoFileRepository;
        private readonly IBaseRepository demoRepository;

        public DemoReader(string path, IFileRepository<DemoFile> demoFileRepository, IBaseRepository demoRepository)
        {
            this.path = path;
            this.demoFileRepository = demoFileRepository;
            this.demoRepository = demoRepository;
        }
        #endregion

        protected override void ReadFile()
        {
            var allFiles = Directory.GetFiles(path);
            var newFiles = allFiles.Except(demoFileRepository.GetFiles().Select(x => x.Path)).ToArray();

            foreach (var file in newFiles)
            {
                ParseDemo(file);

                demoFileRepository.AddFile(new DemoFile
                {
                    Created = File.GetCreationTime(file),
                    Path = file
                });
            }
        }

        private void ParseDemo(string filePath)
        {
            var file = File.OpenRead(filePath);
            _currentDemoFileName = Path.GetFileName(file.Name);

            _parser = new DemoParser(file);
            _parser.ParseHeader();
            _parser.MatchStarted += Parser_MatchStarted;
            _parser.RoundStart += Parser_RoundStart;
            _parser.PlayerKilled += Parser_PlayerKilled;
            _parser.RoundEnd += Parser_RoundEnd;
            _parser.TickDone += Parser_TickDone;
            _parser.BombPlanted += Parser_BombPlanted;
            _parser.BombDefused += Parser_BombDefused;

            Console.WriteLine($"Parse file: \"{_currentDemoFileName}\" Size: {new FileInfo(file.Name).Length.ToSize(LongExtension.SizeUnits.MB)}Mb");
            
            var sw = new Stopwatch();
            sw.Start();
            _parser.ParseToEnd();
            sw.Stop();

            //demoRepository.InsertLog(new DemoLog
            //{
            //    DemoFileName = _results.DemoFileName,
            //    Players = _results.Players.Select(x => new PlayerLog
            //    {
            //        Name = x.Value.Name,
            //        SteamID = x.Value.SteamID,
            //        Assists = x.Value.Assists.Select(x=>x.I)

            //    }).ToList(),
            //    Rounds = _results.Rounds.Select(x => new RoundLog()).ToList(),
            //});

            
            demoRepository.InsertLog(_results);

            Console.WriteLine($"It took: {sw.Elapsed:mm':'ss':'fff}");
        }

        private static void UpdatePlayers()
        {
            _results = _results ?? new Result(_currentDemoFileName);

            foreach (var player in _parser.PlayingParticipants.Where(x => !_results.Players.ContainsKey(x.SteamID)))
            {
                _results.Players.Add(player.SteamID, new Model.Player(player.Name, player.SteamID));
            }
        }

        private static void Parser_MatchStarted(object sender, MatchStartedEventArgs e)
        {
            _matchStarted = true;

            _results = _results ?? new Result(_currentDemoFileName);

            UpdatePlayers();

            _currentRound = new Round { RoundNumber = _currentRoundNumber };
        }

        private static void Parser_RoundEnd(object sender, RoundEndedEventArgs e)
        {
            if (_matchStarted)
            {
                _roundEndedCount = 1;
            }
        }

        private static void Parser_RoundStart(object sender, RoundStartedEventArgs e)
        {
            if (!_matchStarted)
                return;

            _results = _results ?? new Result(_currentDemoFileName);

            _currentRound = new Round();
            _currentRoundNumber++;
            _currentRound.RoundNumber = _currentRoundNumber;
        }

        private static void Parser_TickDone(object sender, TickDoneEventArgs e)
        {
            if (_roundEndedCount == 0)
            {
                return;
            }

            _roundEndedCount++;

            if (_roundEndedCount < _parser.TickRate * 2)
            {
                return;
            }

            _roundEndedCount = 0;
            RoundEnd();
        }

        private static void RoundEnd()
        {
            var winningTeam = Team.Spectate;
            if (_lastTScore != _parser.TScore)
            {
                winningTeam = Team.Terrorist;
            }
            else if (_lastCTScore != _parser.CTScore)
            {
                winningTeam = Team.CounterTerrorist;
            }

            _currentRound.Winner = winningTeam;


            _lastTScore = _parser.TScore;
            _lastCTScore = _parser.CTScore;

            _results.Rounds.Add(_currentRoundNumber, _currentRound);
        }

        private static void Parser_PlayerKilled(object sender, PlayerKilledEventArgs e)
        {
            UpdatePlayers();

            if (_matchStarted && e.Killer != null && e.Killer.SteamID != 0 && e.Victim != null && e.Victim.SteamID != 0)
            {
                var kill = new Kill(_results.Players[e.Killer.SteamID], _results.Players[e.Victim.SteamID],
                    e.Headshot, e.Weapon.Weapon.ToString());

                if (e.Assister != null)
                {
                    kill.Assister = _results.Players[e.Assister.SteamID];
                    kill.Assister.Assists.Add(kill);
                }

                _results.Players[e.Killer.SteamID].Kills.Add(kill);

                _results.Players[e.Victim.SteamID].Deaths.Add(kill);

                if (e.Killer.Team == e.Victim.Team)
                {
                    _results.Players[e.Killer.SteamID].Teamkills.Add(kill);
                }
            }
        }

        private static void Parser_BombDefused(object sender, BombEventArgs e)
        {
            UpdatePlayers();

            _results.Players[e.Player.SteamID].BombDefuses.Add(_currentRound);
            _currentRound.BombDefuser = _results.Players[e.Player.SteamID];
        }

        private static void Parser_BombPlanted(object sender, BombEventArgs e)
        {
            UpdatePlayers();

            _results.Players[e.Player.SteamID].BombPlants.Add(_currentRound);
            _currentRound.BombPlanter = _results.Players[e.Player.SteamID];
        }
    }
}