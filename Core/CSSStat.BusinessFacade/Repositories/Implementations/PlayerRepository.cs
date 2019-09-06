using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Pipes;
using System.Linq;
using CSStat.CsLogsApi.Extensions;
using CsStat.Domain.Definitions;
using CsStat.Domain.Entities;
using CsStat.Domain.Models;
using CsStat.LogApi.Enums;
using CsStat.SystemFacade.Attributes;
using DataService.Interfaces;
using MongoDB.Driver.Builders;

namespace BusinessFacade.Repositories.Implementations
{
    public class PlayerRepository : IPlayerRepository
    {
        private static ILogsRepository _logsRepository;
        private static IMongoRepositoryFactory _mongoRepository;
        private static List<Log> _logs;

        public PlayerRepository(IMongoRepositoryFactory mongoRepository)
        {
            _mongoRepository = mongoRepository;
            _logsRepository = new LogsRepository(_mongoRepository);
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _mongoRepository.GetRepository<Player>().Collection.FindAll();
        }

        public Player GetPlayerByNickName(string nickName)
        {
            var query = new QueryBuilder<Player>();
            return _mongoRepository.GetRepository<Player>().Collection.Find(query.EQ(x => x.NickName, nickName)).FirstOrDefault();
        }

        public Player GetPlayerById(string id)
        {
            var query = new QueryBuilder<Player>();
            return _mongoRepository.GetRepository<Player>().Collection.Find(query.EQ(x => x.Id, id)).FirstOrDefault();
        }

        public string AddPlayer(Player player)
        {
            return _mongoRepository.GetRepository<Player>().Collection.Insert(player).Ok
                ? GetPlayerByNickName(player.NickName).Id
                : string.Empty;
        }

        public void AddPlayers(List<Player> players)
        {
            _mongoRepository.GetRepository<Player>().Collection.InsertBatch(players);
        }

        public void UpdatePlayer(string id, string firstName = null, string secondName = null, string imagePath = null)
        {
            var player = GetPlayerById(id);

            if (player == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                player.FirstName = firstName;
            }

            if (!string.IsNullOrEmpty(secondName))
            {
                player.SecondName = secondName;
            }

            if (!string.IsNullOrEmpty(imagePath))
            {
                player.ImagePath = imagePath;
            }

            _mongoRepository.GetRepository<Player>().Collection.Save(player);
        }

        public IEnumerable<PlayerStatsModel> GetStatsForAllPlayers(string dateFrom = "", string dateTo = "")
        {
            _logs = GetLogs(dateFrom, dateTo);

            if (_logs==null || !_logs.Any())
            {
                return null;
            }

            var players = GetAllPlayers().ToList();

            if(!players.Any())
                return null;

            var playersStats = new List<PlayerStatsModel>();

            foreach (var player in players)
            {
                if (player.NickName.Contains("chicken"))
                {
                    continue;
                }

                var guns = GetGuns(_logs.Where(x => x.Player?.Id == player.Id && x.Action == Actions.Kill).ToList());
                var sniperRifle = guns?.Where(x => x.Gun.GetAttribute<IsSniperRifleAttribute>().Value);
                var explodeBombs = GetExplodeBombs(_logs.Where(x => x.Player?.Id == player.Id && x.Action == Actions.Plant).ToList());
                var defuse = _logs.Count(x => x.Player?.Id == player.Id && x.Action == Actions.Defuse);
                var friendlyKills = _logs.Count(x => x.Player?.Id == player.Id && x.Action == Actions.FriendlyKill);
                var assists = _logs.Count(x => x.Player?.Id == player.Id && x.Action == Actions.Assist);
                var kills = _logs.Count(x => x.Player?.Id == player.Id && x.Action == Actions.Kill);
                var death = _logs.Count(x => x.Victim?.Id == player.Id);
                var totalGames = _logs.Count(x => x.Player?.Id == player.Id && x.Action == Actions.EnteredTheGame);
                playersStats.Add(new PlayerStatsModel
                    {
                        Player = player,
                        Kills = kills,
                        Death = death,
                        Assists = assists,
                        FriendlyKills = friendlyKills,
                        TotalGames = totalGames,
                        HeadShot = kills==0 ? 0 : Math.Round(_logs.Count(x => x.Player?.Id == player.Id && x.IsHeadShot && x.Action == Actions.Kill) /(double) kills * 100, 2) ,
                        Guns = guns,
                        Defuse = defuse,
                        Explode = explodeBombs,
                        Points = kills + assists + (defuse + explodeBombs)*2 - friendlyKills * 2 - kills/2,
                        SniperRifleKills = sniperRifle?.Select(x => x.Kills).Sum() ?? 0
                    });
            }

            var achievements = GetAchievements(playersStats);

            foreach (var playerStats in playersStats)
            {
                playerStats.Achievements = achievements.Where(x => x.PlayerId == playerStats.Player.Id).ToList();
            }

            return playersStats;
        }

        private static List<Log> GetLogs(string dateFrom = "", string dateTo = "")
        {
            var logs = new List<Log>();

            if (!string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
            {
                if (DateTime.TryParse(dateFrom, CultureInfo.InvariantCulture, DateTimeStyles.None, out var from))
                {
                    logs = _logsRepository.GetLogsForPeriod(from, DateTime.Today).ToList();
                }
            }
            else if (!string.IsNullOrEmpty(dateTo) && string.IsNullOrEmpty(dateFrom))
            {
                if (DateTime.TryParse(dateTo, CultureInfo.InvariantCulture, DateTimeStyles.None, out var to))
                {
                    logs = _logsRepository.GetLogsForPeriod(DateTime.Today, to).ToList();
                }
            }
            else if (!string.IsNullOrEmpty(dateTo) && !string.IsNullOrEmpty(dateFrom))
            {
                if (DateTime.TryParse(dateTo, CultureInfo.InvariantCulture, DateTimeStyles.None, out var to)
                    && DateTime.TryParse(dateFrom, CultureInfo.InvariantCulture, DateTimeStyles.None, out var from))
                {
                    logs = _logsRepository.GetLogsForPeriod(from, to).ToList();
                }
            }
            else
            {
                logs = _logsRepository.GetAllLogs().ToList();
            }

            return !logs.Any() 
                ? null 
                : logs;
        }

        private static List<GunModel> GetGuns(IReadOnlyCollection<Log> logs)
        {
            return !logs.Any()
                ? null
                : logs.Where(x=>x.Action==Actions.Kill).GroupBy(x => x.Gun).Select(r => new GunModel
                       {
                           Gun = r.Key,
                           Kills = r.Count()
                       }).OrderByDescending(x=>x.Kills).ToList();
        }

        private static int GetExplodeBombs(IReadOnlyCollection<Log> logs)
        {
            return !logs.Any()
                ? 0
                : logs.Select(bomb => _logsRepository.GetLogsForPeriod(bomb.DateTime, bomb.DateTime.AddMinutes(1)).ToList())
                    .Count(intervalLogs => intervalLogs.Count(x => x.Action == Actions.TargetBombed) > 0);
        }

        private static List<AchieveModel> GetAchievements(IReadOnlyCollection<PlayerStatsModel> playersStats)
        {
            var achievements = new List<AchieveModel>
            {
                new AchieveModel
                {
                    Achieve = AchievementsEnum.First,
                    PlayerId = playersStats.OrderByDescending(x => x.KdRatio).FirstOrDefault()?.Player.Id
                },

                new AchieveModel
                {
                    Achieve = AchievementsEnum.Second,
                    PlayerId = playersStats.OrderByDescending(x => x.KdRatio).Skip(1).Take(1).First().Player.Id
                },

                new AchieveModel
                {
                    Achieve =  AchievementsEnum.Third,
                    PlayerId = playersStats.OrderByDescending(x => x.KdRatio).Skip(2).Take(1).First().Player.Id
                },

                new AchieveModel
                {
                Achieve =  AchievementsEnum.Killer,
                PlayerId = playersStats.OrderByDescending(x => x.Kills).FirstOrDefault()?.Player.Id
                },

                new AchieveModel
                {
                    Achieve =  AchievementsEnum.TeamPLayer,
                    PlayerId = playersStats.OrderByDescending(x => x.Assists).FirstOrDefault()?.Player.Id
                },

                new AchieveModel
                {
                    Achieve =  AchievementsEnum.HeadHunter,
                    PlayerId = playersStats.OrderByDescending(x => x.HeadShot).FirstOrDefault()?.Player.Id
                },

                new AchieveModel
                {
                    Achieve =  AchievementsEnum.Kenny,
                    PlayerId = playersStats.OrderByDescending(x => x.Death).FirstOrDefault()?.Player.Id
                },

                new AchieveModel
                {
                    Achieve =  AchievementsEnum.Mvp,
                    PlayerId = playersStats.OrderByDescending(x=>x.Points).FirstOrDefault()?.Player.Id
                },

                new AchieveModel
                {
                    Achieve = AchievementsEnum.Sniper,
                    PlayerId = playersStats.OrderByDescending(x=>x.SniperRifleKills).FirstOrDefault()?.Player.Id
                }
            };

            return achievements;
        }
    }
}