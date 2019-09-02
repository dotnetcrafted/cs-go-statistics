using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CsStat.Domain.Definitions;
using CsStat.Domain.Entities;
using CsStat.LogApi.Enums;
using DataService.Interfaces;
using MongoDB.Driver.Builders;

namespace BusinessFacade.Repositories.Implementations
{
    public class PlayerRepository : IPlayerRepository
    {
        private static ILogsRepository _logsRepository;
        private static IMongoRepositoryFactory _mongoRepository;

        public PlayerRepository(IMongoRepositoryFactory mongoRepository)
        {
            _mongoRepository = mongoRepository;
            _logsRepository = new LogsRepository(_mongoRepository);
        }
        public IEnumerable<PlayerStatsModel> GetStatsForAllPlayers(string dateFrom = "", string dateTo = "")
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

            if (!logs.Any())
            {
                return null;
            }

            var players = GetAllPlayers().ToList();

            return !players.Any()
                ? null
                : (from player in players
                   let kills = logs.Count(x => x.Player?.Id == player.Id && x.Action == Actions.Kill)
                    select new PlayerStatsModel
                    {
                        Player = player,
                        Kills = kills,
                        Death = logs.Count(x => x.Victim?.Id == player.Id),
                        Assists = logs.Count(x => x.Player?.Id == player.Id && x.Action == Actions.Assist),
                        TotalGames = logs.Count(x =>x.Player?.Id == player.Id && x.Action == Actions.EnteredTheGame),
                        HeadShot = Math.Round(logs.Count(x => x.Player?.Id == player.Id && x.IsHeadShot) / (double) kills *100, 2),
                        FavoriteGun = GetFavoriteGun(logs.Where(x =>x.Player?.Id == player.Id && x.Action == Actions.Kill).ToList()),
                        Defuse = logs.Count(x =>x.Player?.Id== player.Id && x.Action == Actions.Defuse),
                        Explode = GetExplodeBombs(logs.Where(x =>x.Player?.Id == player.Id && x.Action == Actions.Plant).ToList())
                    }).ToList();
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _mongoRepository.GetRepository<Player>().Collection.FindAll();
        }

        public Player GetPlayerByNickName(string nickName)
        {
            var query = new QueryBuilder<Player>();
            return _mongoRepository.GetRepository<Player>().Collection.Find(query.EQ(x=>x.NickName,nickName)).FirstOrDefault();
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


        public void UpdatePlayer(string id,string firstName = null, string secondName = null, string imagePath = null)
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

        private static Guns GetFavoriteGun(IReadOnlyCollection<Log> logs)
        {
            return !logs.Any()
                ? Guns.Null
                : logs.GroupBy(x => x.Gun).Select(r => new
                       {
                           Gun = r.Key,
                           Count = r.Count()
                       }).ToList().OrderByDescending(x=>x.Count)
                      .FirstOrDefault()?.Gun ?? Guns.Unknown;
        }

        private static int GetExplodeBombs(IReadOnlyCollection<Log> logs)
        {
            return !logs.Any()
                ? 0
                : logs.Select(bomb => _logsRepository.GetLogsForPeriod(bomb.DateTime, bomb.DateTime.AddMinutes(1)).ToList())
                    .Count(intervalLogs => intervalLogs.Count(x => x.Action == Actions.TargetBombed) > 0);
        }
    }
}