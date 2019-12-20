using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using BusinessFacade.Repositories;
using CSStat.CsLogsApi.Extensions;
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

        private static string _demoFileName;
        private static string _fullDemoFileName;
        
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
                var isSuccessfully = true;
                var error = string.Empty;
                try
                {
                    Reset();
                    ParseDemo(file);
                }
                catch (Exception e)
                {
                    isSuccessfully = false;
                    Console.WriteLine(e);
                    error = e.ToString();
                }
                finally
                {
                    demoFileRepository.AddFile(new DemoFile
                    {
                        Created = File.GetCreationTime(file),
                        Path = file,
                        IsSuccessfully = isSuccessfully,
                        Message = error
                    });
                }
            }
        }

        private static void Reset()
        {
            var players = new Dictionary<long, Model.Player>();
            if (_results != null && _results.Players.Any())
            {
                foreach (var player in _results.Players)
                {
                    players.Add(player.Key, new Model.Player(player.Value.Name, player.Value.SteamID));
                }
            }
            _results = new Result(_demoFileName) {Players = players};

            _matchStarted = default;
            _currentRound = null;
            _currentRoundNumber = default;
            _roundEndedCount = default;

            _lastTScore = default;
            _lastCTScore = default;
        }

        private void ParseDemo(string filePath)
        {
            var file = File.OpenRead(filePath);
            _fullDemoFileName = file.Name;
            _demoFileName = Path.GetFileName(file.Name);

            if (_demoFileName.IsEmpty())
                return;

            _parser = new DemoParser(file);
            _parser.ParseHeader();
            _parser.MatchStarted += Parser_MatchStarted;
            _parser.RoundStart += Parser_RoundStart;
            _parser.PlayerKilled += Parser_PlayerKilled;
            _parser.RoundEnd += Parser_RoundEnd;
            _parser.TickDone += Parser_TickDone;
            _parser.BombPlanted += Parser_BombPlanted;
            _parser.BombDefused += Parser_BombDefused;
            _parser.BombExploded += Parser_BombExploded;
            _parser.PlayerBind += Parser_PlayerBind;

            Console.WriteLine(
                $"Parse file: \"{_demoFileName}\" Size: {new FileInfo(file.Name).Length.ToSize(LongExtension.SizeUnits.MB)}Mb");

            var sw = new Stopwatch();
            sw.Start();
            _parser.ParseToEnd();
            sw.Stop();

            MatchFinish();

            Console.WriteLine($"It took: {sw.Elapsed:mm':'ss':'fff}");
        }

        private static DateTime? GetDemoDate(string fileName)
        {
            var sections = fileName?.Split('-');
            if (sections?.Length == 6)
            {
                if (DateTime.TryParseExact(string.Join(" ", sections.Skip(1).Take(2)),
                    "yyyyMMdd hhmmss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var date))
                    return date;
            }

            return null;
        }

        private void MatchFinish()
        {
            var demoLog = new DemoLog
            {
                Map = _parser.Map,
                MatchDate = GetDemoDate(_demoFileName),
                Size = new FileInfo(_fullDemoFileName).Length,
                DemoFileName = _results.DemoFileName,
                ParsedDate = DateTime.Now,
                Players = _results.Players.Select(x => new PlayerLog
                {
                    Name = x.Value.Name,
                    SteamID = x.Value.SteamID,
                    Assists = x.Value.Assists?.Select(z => new KillLog
                    {
                        Killer = z.Killer?.SteamID,
                        Victim = z.Victim?.SteamID,
                        Assister = z.Assister?.SteamID,
                        Weapon = z.Weapon,
                        IsHeadshot = z.IsHeadshot,
                        RoundNumber = z.RoundNumber
                    }).ToList(),
                    Deaths = x.Value.Deaths?.Select(z => new KillLog
                    {
                        Killer = z.Killer?.SteamID,
                        Victim = z.Victim?.SteamID,
                        Assister = z.Assister?.SteamID,
                        Weapon = z.Weapon,
                        IsHeadshot = z.IsHeadshot,
                        RoundNumber = z.RoundNumber
                    }).ToList(),
                    Kills = x.Value.Kills?.Select(z => new KillLog
                    {
                        Killer = z.Killer?.SteamID,
                        Victim = z.Victim?.SteamID,
                        Assister = z.Assister?.SteamID,
                        Weapon = z.Weapon,
                        IsHeadshot = z.IsHeadshot,
                        RoundNumber = z.RoundNumber
                    }).ToList(),
                    Teamkills = x.Value.Teamkills?.Select(z => new KillLog
                    {
                        Killer = z.Killer?.SteamID,
                        Victim = z.Victim?.SteamID,
                        Assister = z.Assister?.SteamID,
                        Weapon = z.Weapon,
                        IsHeadshot = z.IsHeadshot,
                        RoundNumber = z.RoundNumber
                    }).ToList(),
                    BombDefuses = x.Value.BombDefuses?.Select(z => new RoundLog
                    {
                        Winner = z.Winner == Team.CounterTerrorist
                            ? CsStat.Domain.Definitions.Teams.Ct
                            : CsStat.Domain.Definitions.Teams.T,
                        BombDefuser = z.BombDefuser?.SteamID,
                        BombPlanter = z.BombPlanter?.SteamID,
                        RoundNumber = z.RoundNumber
                    }).ToList(),
                    BombExplosions = x.Value.BombExplosions?.Select(z => new RoundLog
                    {
                        Winner = z.Winner == Team.CounterTerrorist
                            ? CsStat.Domain.Definitions.Teams.Ct
                            : CsStat.Domain.Definitions.Teams.T,
                        BombDefuser = z.BombDefuser?.SteamID,
                        BombPlanter = z.BombPlanter?.SteamID,
                        RoundNumber = z.RoundNumber
                    }).ToList(),
                    BombPlants = x.Value.BombPlants?.Select(z => new RoundLog
                    {
                        Winner = z.Winner == Team.CounterTerrorist
                            ? CsStat.Domain.Definitions.Teams.Ct
                            : CsStat.Domain.Definitions.Teams.T,
                        BombDefuser = z.BombDefuser?.SteamID,
                        BombPlanter = z.BombPlanter?.SteamID,
                        RoundNumber = z.RoundNumber
                    }).ToList()
                }).ToList(),
                Rounds = _results.Rounds?.Select(x => new RoundLog
                {
                    BombPlanter = x.Value.BombPlanter?.SteamID,
                    BombDefuser = x.Value.BombDefuser?.SteamID,
                    IsBombExploded = x.Value.IsBombExploded,
                    Reason = (CsStat.Domain.Definitions.RoundEndReason)(int)x.Value.Reason,
                    RoundNumber = x.Value.RoundNumber,
                    Winner = x.Value.Winner == Team.CounterTerrorist
                        ? CsStat.Domain.Definitions.Teams.Ct
                        : CsStat.Domain.Definitions.Teams.T,
                    Teams = x.Value.Teams.ToDictionary(
                        z => z.Key == Team.CounterTerrorist
                            ? CsStat.Domain.Definitions.Teams.Ct.GetDescription()
                            : CsStat.Domain.Definitions.Teams.T.GetDescription(),
                        z => z.Value.Select(k => k.SteamID).ToList()
                    )
                }).ToList()
            };

            demoRepository.InsertLog(demoLog);
        }

        private static void Parser_PlayerBind(object sender, PlayerBindEventArgs e)
        {
            _results = _results ?? new Result(_demoFileName);

            if (e.Player == null || e.Player.SteamID == 0 || _results.Players.ContainsKey(e.Player.SteamID))
            {
                return;
            }

            _results.Players.Add(e.Player.SteamID, new Model.Player(e.Player.Name, e.Player.SteamID));
        }

        private void Parser_MatchStarted(object sender, MatchStartedEventArgs e)
        {
            if (_matchStarted) // if we here again it means that the demo file contains two or more matches
            {
                MatchFinish();
                Reset();
            }

            _matchStarted = true;

            _results = _results ?? new Result(_demoFileName);

            _currentRound = new Round {RoundNumber = _currentRoundNumber};
        }

        private static void Parser_RoundEnd(object sender, RoundEndedEventArgs e)
        {
            if (!_matchStarted) 
                return;

            _currentRound.Reason = e.Reason;
            _roundEndedCount = 1;
        }

        private static void Parser_RoundStart(object sender, RoundStartedEventArgs e)
        {
            if (!_matchStarted)
                return;

            _results = _results ?? new Result(_demoFileName);

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
            Console.WriteLine($"Round number: {_currentRound.RoundNumber}");

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

            _currentRound.Teams = _parser.Participants
                .Where(x => x.SteamID != 0 && x.Team != Team.Spectate) // skip spectators
                .GroupBy(x => new {x.Team}).ToDictionary(x => x.Key.Team,
                    x => x.Select(z => _results.Players[z.SteamID]).ToList());

            _results.Rounds.Add(_currentRoundNumber, _currentRound);
        }

        private static void Parser_PlayerKilled(object sender, PlayerKilledEventArgs e)
        {
            if (_matchStarted && e.Killer != null && e.Killer.SteamID != 0 && e.Victim != null && e.Victim.SteamID != 0)
            {
                var kill = new Kill(_results.Players[e.Killer.SteamID], _results.Players[e.Victim.SteamID],
                    e.Headshot, e.Weapon.Weapon.ToString(), _currentRoundNumber);

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
            _results.Players[e.Player.SteamID].BombDefuses.Add(_currentRound);
            _currentRound.BombDefuser = _results.Players[e.Player.SteamID];
        }

        private static void Parser_BombPlanted(object sender, BombEventArgs e)
        {
            _results.Players[e.Player.SteamID].BombPlants.Add(_currentRound);
            _currentRound.BombPlanter = _results.Players[e.Player.SteamID];
        }

        private static void Parser_BombExploded(object sender, BombEventArgs e)
        {
            _results.Players[e.Player.SteamID].BombExplosions.Add(_currentRound);
            _currentRound.IsBombExploded = true;
        }
    }
}