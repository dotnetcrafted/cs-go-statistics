using System;
using System.IO;
using DemoInfo;
using ReadFile.ReadDemo.Model;

namespace ReadFile.ReadDemo
{
    internal static class Program
    {
        private static DemoParser _parser;
        private static Result _results = new Result();

        private static bool _matchStarted;
        private static Round _currentRound;
        private static int _currentRoundNumber = 0;
        private static int _roundEndedCount = 0;

        private static int _lastTScore = 0;
        private static int _lastCTScore = 0;

        private static void Main(string[] args)
        {
            var file = File.OpenRead(
                @"d:\MSExam\SALo\Latest\Utils\ReadFile.ReadDemo\bin\Debug\Demos\auto0-20191009-125008-1572099890-de_mirage-fuse8delete.dem");

            _parser = new DemoParser(file);
            _parser.ParseHeader();
            _parser.MatchStarted += Parser_MatchStarted;
            _parser.RoundStart += Parser_RoundStart;
            _parser.PlayerKilled += Parser_PlayerKilled;
            _parser.RoundEnd += Parser_RoundEnd;
            _parser.TickDone += Parser_TickDone;
            _parser.BombPlanted += Parser_BombPlanted;
            _parser.BombDefused += Parser_BombDefused;

            _parser.ParseToEnd();

            Console.WriteLine($"{nameof(_results.HighestFragDeathRatio)}: {_results.HighestFragDeathRatio.Name}");
            Console.WriteLine($"{nameof(_results.LeastDeaths)}: {_results.LeastDeaths.Name}");
            Console.WriteLine($"{nameof(_results.LeastKills)}: {_results.LeastKills.Name}");

            Console.ReadKey();
        }

        private static void Parser_MatchStarted(object sender, MatchStartedEventArgs e)
        {
            _matchStarted = true;

            foreach (var player in _parser.PlayingParticipants)
            {
                _results.Players.Add(player.SteamID, new Model.Player(player.Name, player.SteamID));
            }

            _currentRound = new Round {RoundNumber = _currentRoundNumber};
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
            _results.Players[e.Player.SteamID].BombDefuses.Add(_currentRound);
            _currentRound.BombDefuser = _results.Players[e.Player.SteamID];
        }

        private static void Parser_BombPlanted(object sender, BombEventArgs e)
        {
            _results.Players[e.Player.SteamID].BombPlants.Add(_currentRound);
            _currentRound.BombPlanter = _results.Players[e.Player.SteamID];
        }
    }
}