using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using BusinessFacade.Repositories;
using CsStat.Domain.Entities.Demo;
using CsStat.SystemFacade;
using CsStat.SystemFacade.Extensions;
using DemoInfo;
using ReadFile.ReadDemo.Model;
using Player = DemoInfo.Player;

namespace ReadFile.ReadDemo
{
    public class DemoReader : BaseWatcher
    {
        private const int FullHP = 100;

        private const string SuqadA = "Team A";
        private const string SuqadB = "Team B";
        private const int SwapRoundNumber = 15;

        private static DemoParser _parser;
        private static Result _results;

        private static string _demoFileName;
        private static string _fullDemoFileName;

        private static bool _matchStarted;
        private static Round _currentRound;
        private static int _currentRoundNumber;
        private static int _roundEndedCount;

        private static int _matchTickTimeStart;
        private static int _rountTickTimeStart;

        private static int _lastTScore;
        private static int _lastCTScore;

        private static int _squadAScore;
        private static int _squadBScore;

        private static List<Player> _participants = new List<Player>();
        private static List<Damage> _playerDamageStat = new List<Damage>();

        #region inint

        private readonly string path;
        private readonly IDemoFileRepository demoFileRepository;
        private readonly IBaseRepository demoRepository;
        private readonly IMapper mapper;
        private static IProgress<string> _progress;

        public DemoReader(string path, IDemoFileRepository demoFileRepository, IBaseRepository demoRepository,
            IMapper mapper, IProgress<string> progress)
        {
            this.path = path;
            this.demoFileRepository = demoFileRepository;
            this.demoRepository = demoRepository;
            this.mapper = mapper;
            _progress = progress;
        }

        #endregion

        protected override void ReadFile()
        {
            _progress.Report($"{DateTime.Now} | Checking file");
            var allFiles = Directory.GetFiles(path, "*.dem");

            var demoFiles = demoFileRepository.GetFiles().ToList();
            var newFiles = allFiles.Except(demoFiles.Select(x => x.Path)).ToArray();
            _progress.Report($"All Files: {allFiles.Length}");
            _progress.Report($"New Files: {newFiles.Length}");
            _progress.Report($"Demo Files: {demoFiles.Count}");
            

            foreach (var file in newFiles)
            {
                var isSuccessfully = true;
                var error = string.Empty;
                try
                {
                    var fileStream = File.OpenRead(file);

                    _fullDemoFileName = fileStream.Name;
                    _demoFileName = Path.GetFileName(fileStream.Name);

                    if (_demoFileName.IsEmpty())
                        return;

                    Reset();
                    ParseDemo(fileStream);
                }
                catch (Exception e)
                {
                    isSuccessfully = false;
                    _progress.Report(e.Message);
                    error = e.ToString();
                }
                finally
                {
                    demoFileRepository.AddFile(new DemoFile
                    {
                        Created = File.GetCreationTime(file),
                        Path = file,
                        IsSuccessfully = isSuccessfully,
                        Message = error,
                        Runner = Environment.MachineName
                    });
                }
                _progress.Report($"Demo Files: {demoFileRepository.GetFiles().Count()}");
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
            _currentRoundNumber = 1;
            _roundEndedCount = default;

            _matchTickTimeStart = 0;
            _rountTickTimeStart = 0;

            _lastCTScore = default;
            _lastTScore = default;

            _squadAScore = default;
            _squadBScore = default;

            _participants = new List<Player>();
            _playerDamageStat = new List<Damage>();
        }

        private void ParseDemo(FileStream file)
        {
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
            _parser.FreezetimeEnded += Parser_FreezetimeEnded;
            _parser.PlayerHurt += Parser_PlayerHurt;

            _progress.Report($"Parse file: \"{_demoFileName}\" Size: {new FileInfo(file.Name).Length.ToSize(LongExtension.SizeUnits.MB)}Mb");

            var sw = new Stopwatch();
            sw.Start();
            _parser.ParseToEnd();
            sw.Stop();

            MatchFinish();

            _progress.Report($"It took: {sw.Elapsed:mm':'ss':'fff}");
        }

        private static void Parser_PlayerHurt(object sender, PlayerHurtEventArgs e)
        {
            if (!_matchStarted || e.Attacker == null || e.Player == null)
                return;

            var attakersTeam = GetParticipants(_participants, _parser.Participants)
                .FirstOrDefault(x => x.SteamID == e.Attacker.SteamID)?.Team;
            var victimsTeam = GetParticipants(_participants, _parser.Participants)
                .FirstOrDefault(x => x.SteamID == e.Player.SteamID)?.Team;

            if (attakersTeam == victimsTeam) // attacked teammate
                return;

            var query = _playerDamageStat.Where(damage => damage.RoundNumber == _currentRoundNumber &&
                                                          damage.Attacker == e.Attacker.SteamID &&
                                                          damage.Victim == e.Player.SteamID)
                .ToList();

            var givenDamage = query.Sum(x => x.HealthDamage);
            var lastHealth = query.Any() ? query.Min(x => x.Health) : FullHP;

            if (givenDamage >= FullHP)
                return;

            _playerDamageStat.Add(new Damage
            {
                RoundNumber = _currentRoundNumber,
                Weapon = EquipmentMapper.Map(e.Weapon.Weapon),
                Attacker = e.Attacker.SteamID,
                Victim = e.Player.SteamID,
                Health = e.Health,
                HealthDamage = e.HealthDamage + givenDamage <= FullHP ? e.HealthDamage : lastHealth,
                ArmorDamage = e.ArmorDamage
            });
        }

        private static DateTime? GetDemoDate(string fileName)
        {
            var sections = fileName?.Split('-');
            if (sections?.Length == 6)
            {
                if (DateTime.TryParseExact(string.Join(" ", sections.Skip(1).Take(2)),
                    "yyyyMMdd HHmmss",
                    null,
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
                TotalSquadAScore = _squadAScore,
                TotalSquadBScore = _squadBScore,
                Duration = (int) Math.Round(GetMatchDuration()),
                Players = _results.Players.Select(x => new PlayerLog
                {
                    Name = x.Value.Name,
                    SteamID = x.Value.SteamID,
                    Assists = x.Value.Assists?.Select(z => mapper.Map<KillLog>(z)).ToList(),
                    Deaths = x.Value.Deaths?.Select(z => mapper.Map<KillLog>(z)).ToList(),
                    Kills = x.Value.Kills?.Select(z => mapper.Map<KillLog>(z)).ToList(),
                    Teamkills = x.Value.Teamkills?.Select(z => mapper.Map<KillLog>(z)).ToList(),
                    BombDefuses = x.Value.BombDefuses?.Select(z => z.RoundNumber).ToList(),
                    BombExplosions = x.Value.BombExplosions?.Select(z => z.RoundNumber).ToList(),
                    BombPlants = x.Value.BombPlants?.Select(z => z.RoundNumber).ToList(),
                    Damage = GetDamage(_playerDamageStat.Where(stat => !stat.IsNadeDamage), x.Value.SteamID),
                    UtilityDamage = GetDamage(_playerDamageStat.Where(stat => stat.IsNadeDamage), x.Value.SteamID)
                }).ToList(),
                Rounds = _results.Rounds?.Select(x => new RoundLog
                {
                    BombPlanter = x.Value.BombPlanter?.SteamID,
                    BombPlanterName = x.Value.BombPlanter?.Name,
                    BombPlantedTime = x.Value.BombPlantedTime,
                    BombDefuser = x.Value.BombDefuser?.SteamID,
                    BombDefuserName = x.Value.BombDefuser?.Name,
                    BombDefusedTime = x.Value.BombDefusedTime,
                    IsBombExploded = x.Value.IsBombExploded,
                    Reason = (CsStat.Domain.Definitions.RoundEndReason) (int) x.Value.Reason,
                    ReasonTitle = x.Value.Reason.ToString(),
                    RoundNumber = x.Value.RoundNumber,
                    Duration = x.Value.Duration,
                    Winner = x.Value.Winner == Team.CounterTerrorist
                        ? CsStat.Domain.Definitions.Teams.Ct
                        : CsStat.Domain.Definitions.Teams.T,
                    WinnerTitle = x.Value.Winner.ToString(),
                    TScore = x.Value.TScore,
                    CTScore = x.Value.CTScore,
                    Squads = x.Value.Squads.Select(z => new SquadLog
                    {
                        Team = z.Team.ToString(),
                        SquadTitle = z.Title,
                        Players = z.Players.Select(k => new PlayerLog
                        {
                            Name = k.Name,
                            SteamID = k.SteamID,
                            Kills = k.Kills.Where(p => p.RoundNumber == x.Value.RoundNumber)
                                .Select(v => mapper.Map<KillLog>(v)).ToList(),
                            Assists = k.Assists.Where(p => p.RoundNumber == x.Value.RoundNumber)
                                .Select(v => mapper.Map<KillLog>(v)).ToList(),
                            Deaths = k.Deaths.Where(p => p.RoundNumber == x.Value.RoundNumber)
                                .Select(v => mapper.Map<KillLog>(v)).ToList(),
                            Teamkills = k.Teamkills.Where(p => p.RoundNumber == x.Value.RoundNumber)
                                .Select(v => mapper.Map<KillLog>(v)).ToList(),
                            Damage = GetDamage(_playerDamageStat.Where(stat => !stat.IsNadeDamage), k.SteamID,
                                x.Value.RoundNumber),
                            UtilityDamage = GetDamage(_playerDamageStat.Where(stat => stat.IsNadeDamage), k.SteamID,
                                x.Value.RoundNumber)
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
            ;

            demoRepository.Insert(demoLog);

            var time = TimeSpan.FromSeconds(demoLog.Duration);
            _progress.Report($"Match duration: {time:hh\\:mm\\:ss\\:fff}");

            _progress.Report($"{SuqadA}: {_squadAScore}");
            _progress.Report($"{SuqadB}: {_squadBScore}");
        }

        private static List<DamageLog> GetDamage(IEnumerable<Damage> stat, long steamId, int roundNumber = 0)
        {
            var query = stat.Where(x => x.Attacker == steamId);
            if (roundNumber != 0)
                query = query.Where(x => x.RoundNumber == roundNumber);

            return query.GroupBy(x => x.RoundNumber)
                .Select(x => new DamageLog
                {
                    RoundNumber = x.Key,
                    HealthDamage = x.Sum(z => z.HealthDamage),
                    ArmorDamage = x.Sum(z => z.ArmorDamage)
                }).ToList();
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
            _matchTickTimeStart = _parser.CurrentTick;

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

        private static void Parser_FreezetimeEnded(object sender, FreezetimeEndedEventArgs e)
        {
            _rountTickTimeStart = _parser.CurrentTick;
        }

        private static void Parser_RoundStart(object sender, RoundStartedEventArgs e)
        {
            if (!_matchStarted)
                return;

            _results = _results ?? new Result(_demoFileName);

            _currentRound = new Round();

            _currentRoundNumber++;
            _currentRound.RoundNumber = _currentRoundNumber;

            _participants = _parser.Participants.ToList();
        }

        private static IEnumerable<Player> GetParticipants(IEnumerable<Player> participantsAtStart,
            IEnumerable<Player> participantsAtEnd)
        {
            var diff = participantsAtStart.Where(x => participantsAtEnd.All(z => z.SteamID != x.SteamID));
            return participantsAtEnd.Union(diff);
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
            //It happens if there is only one player on the server and he switches between times
            if (_lastCTScore == _parser.CTScore && _lastTScore == _parser.TScore)
            {
                return;
            }

            Team winningTeam;

            if (_currentRound.Reason == RoundEndReason.BombDefused ||
                _currentRound.Reason == RoundEndReason.TargetSaved ||
                _currentRound.Reason == RoundEndReason.CTWin)
            {
                winningTeam = Team.CounterTerrorist;
                if (_currentRoundNumber <= SwapRoundNumber)
                    _squadAScore++;
                else
                    _squadBScore++;
            }
            else
            {
                winningTeam = Team.Terrorist;
                if (_currentRoundNumber <= SwapRoundNumber)
                    _squadBScore++;
                else
                    _squadAScore++;
            }

            _lastTScore = _parser.TScore;
            _lastCTScore = _parser.CTScore;

            _currentRound.Winner = winningTeam;

            _currentRound.TScore = _parser.TScore;
            _currentRound.CTScore = _parser.CTScore;

            _currentRound.Duration = (int) Math.Round(GetRoundDuration());

            _currentRound.Squads = GetParticipants(_participants, _parser.Participants)
                .Where(x => x.SteamID != 0 && x.Team != Team.Spectate) // skip spectators
                .GroupBy(x => new {x.Team})
                .Select(x => new Squad
                {
                    Team = x.Key.Team,
                    Title = GetSquadName(x.Key.Team),
                    Players = x.Select(z => _results.Players[z.SteamID]).ToList()
                })
                .ToList();

            _results.Rounds.Add(_currentRoundNumber, _currentRound);

            var foregroundColor = Console.ForegroundColor;
            if (winningTeam == Team.CounterTerrorist)
                Console.ForegroundColor = ConsoleColor.Cyan;

            if (winningTeam == Team.Terrorist)
                Console.ForegroundColor = ConsoleColor.DarkYellow;

            var squadScore = _currentRoundNumber <= SwapRoundNumber
                ? $"{SuqadA}(CT): {_squadAScore,-2} - {SuqadB}: {_squadBScore,-2} (T)"
                : $"{SuqadB}(CT): {_squadBScore,-2} - {SuqadA}: {_squadAScore,-2} (T)";

            var duration = TimeSpan.FromSeconds(_currentRound.Duration);

            _progress.Report($"Round number: {_currentRound.RoundNumber,-2} | {squadScore} | reason: {_currentRound.Reason,-12} | duration: {duration:hh\\:mm\\:ss\\:fff} | winning team: {winningTeam}");

            Console.ForegroundColor = foregroundColor;
        }

        private static string GetSquadName(Team team)
        {
            if (_currentRoundNumber <= SwapRoundNumber)
            {
                return team == Team.CounterTerrorist ? SuqadA : SuqadB;
            }
            else
            {
                return team == Team.CounterTerrorist ? SuqadB : SuqadA;
            }
        }

        private static void Parser_PlayerKilled(object sender, PlayerKilledEventArgs e)
        {
            if (!_matchStarted)
                return;

            var killTime = GetRoundDuration();

            if (e.Killer != null && e.Killer.SteamID != 0 && e.Victim != null && e.Victim.SteamID != 0)
            {
                var kill = new Kill(_results.Players[e.Killer.SteamID], _results.Players[e.Victim.SteamID],
                    e.Headshot, EquipmentMapper.Map(e.Weapon.Weapon), _currentRoundNumber,
                    e.Killer.SteamID == e.Victim.SteamID, killTime, e.PenetratedObjects, e.AssistedFlash);

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

            // suicide
            if (e.Killer == null && e.Victim != null && e.Victim.SteamID != 0)
            {
                var kill = new Kill(null, _results.Players[e.Victim.SteamID],
                    e.Headshot, EquipmentMapper.Map(e.Weapon.Weapon),
                    _currentRoundNumber, true, killTime, e.PenetratedObjects, e.AssistedFlash);

                if (e.Assister != null)
                {
                    kill.Assister = _results.Players[e.Assister.SteamID];
                    kill.Assister.Assists.Add(kill);
                }

                _results.Players[e.Victim.SteamID].Deaths.Add(kill);
            }
        }

        private static void Parser_BombDefused(object sender, BombEventArgs e)
        {
            _results.Players[e.Player.SteamID].BombDefuses.Add(_currentRound);
            _currentRound.BombDefuser = _results.Players[e.Player.SteamID];
            _currentRound.BombDefusedTime = GetRoundDuration();
        }

        private static void Parser_BombPlanted(object sender, BombEventArgs e)
        {
            _results.Players[e.Player.SteamID].BombPlants.Add(_currentRound);
            _currentRound.BombPlanter = _results.Players[e.Player.SteamID];
            _currentRound.BombPlantedTime = GetRoundDuration();
        }

        private static void Parser_BombExploded(object sender, BombEventArgs e)
        {
            _results.Players[_currentRound.BombPlanter.SteamID].BombExplosions.Add(_currentRound);
            _currentRound.IsBombExploded = true;
        }

        private static float GetRoundDuration()
        {
            return (_parser.CurrentTick - _rountTickTimeStart) / _parser.TickRate;
        }
        private static float GetMatchDuration()
        {
            return (_parser.CurrentTick - _matchTickTimeStart) / _parser.TickRate;
        }
    }
}