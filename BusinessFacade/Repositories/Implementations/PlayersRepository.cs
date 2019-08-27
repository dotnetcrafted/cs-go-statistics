using System;
using System.Collections.Generic;
using System.Linq;
using CSStat.CsLogsApi.Extensions;
using CSStat.CsLogsApi.Models;
using CsStat.Domain.Definitions;
using CsStat.Domain.Entities;
using CsStat.LogApi.Enums;
using DataService.Interfaces;

namespace BusinessFacade.Repositories.Implementations
{
    public class PlayersRepository : IPlayersRepository
    {
        private readonly IEnumerable<LogModel> _logs;

        public PlayersRepository(IMongoRepositoryFactory mongoRepository)
        {
            _logs = mongoRepository.GetRepository<LogModel>().Collection.FindAll().SetSortOrder("PlayerName");
        }
        public IEnumerable<PlayerModel> GetAllPlayers()
        {
            var playersNameList = _logs.DistinctBy(x => x.PlayerName).Select(x => x.PlayerName);

            return (from playerName in playersNameList
                    let kills = _logs.Count(x => string.Equals(x.PlayerName, playerName) && x.Action == Actions.Kill)
                    select new PlayerModel
                    {
                        PlayerName = playerName,
                        Kills = kills,
                        Death = _logs.Count(x => string.Equals(x.VictimName, playerName)),
                        Assists = _logs.Count(x => string.Equals(x.PlayerName, playerName) && x.Action == Actions.Assist),
                        TotalGames = 1,
                        HeadShot = Math.Round(_logs.Count(x => string.Equals(x.PlayerName, playerName) && x.IsHeadShot) / (double) kills * 100,2),
                        FavoriteGun = GetFavoriteGun(_logs.Where(x => x.PlayerName == playerName && x.Action == Actions.Kill).ToList()),
                        Defuse = _logs.Count(x=>string.Equals(x.PlayerName, playerName) && x.Action == Actions.Defuse)
                    }).ToList();
        }

        private static Guns GetFavoriteGun(IReadOnlyCollection<LogModel> logs)
        {
            return !logs.Any()
                ? Guns.Null
                : logs.GroupBy(x => x.Gun).Select(r => new
                       {
                           Gun = r.Key,
                           Count = r.Count()
                       }).ToList()
                      .FirstOrDefault()?.Gun 
                    ?? Guns.Unknown;
        }
    }
}