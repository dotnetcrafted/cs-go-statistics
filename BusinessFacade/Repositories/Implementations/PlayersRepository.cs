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
        private static ILogsRepository _logsRepository;

        public PlayersRepository(ILogsRepository logsRepository)
        {
            _logsRepository = logsRepository;
        }
        public IEnumerable<PlayerModel> GetAllPlayers()
        {
            var logs = _logsRepository.GetAllLogs().ToList();
            if (!logs.Any())
            {
                return null;
            }
            var playersNameList = logs.DistinctBy(x => x.PlayerName).Select(x => x.PlayerName);

            return (from playerName in playersNameList
                    let kills = logs.Count(x => string.Equals(x.PlayerName, playerName) && x.Action == Actions.Kill)
                    select new PlayerModel
                    {
                        PlayerName = playerName,
                        Kills = kills,
                        Death = logs.Count(x => string.Equals(x.VictimName, playerName)),
                        Assists = logs.Count(x => string.Equals(x.PlayerName, playerName) && x.Action == Actions.Assist),
                        TotalGames = 1,
                        HeadShot = Math.Round(logs.Count(x => string.Equals(x.PlayerName, playerName) && x.IsHeadShot) / (double) kills * 100,2),
                        FavoriteGun = GetFavoriteGun(logs.Where(x => string.Equals(x.PlayerName, playerName) && x.Action == Actions.Kill).ToList()),
                        Defuse = logs.Count(x=>string.Equals(x.PlayerName, playerName) && x.Action == Actions.Defuse),
                        Explode = GetExplodeBombs(logs.Where(x=>string.Equals(x.PlayerName, playerName) && x.Action == Actions.Plant).ToList())
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
                       }).ToList().OrderByDescending(x=>x.Count)
                      .FirstOrDefault()?.Gun ?? Guns.Unknown;
        }

        private static int GetExplodeBombs(IReadOnlyCollection<LogModel> logs)
        {
            var enumerable = logs.Select(bomb => _logsRepository.GetLogsForPeriod(bomb.DateTime, bomb.DateTime.AddMinutes(1)).ToList());
            var explodeBombs = enumerable.Count(intervalLogs => intervalLogs.Count(x => x.Action == Actions.TargetBombed) > 0);
            return !logs.Any()
                ? 0
                : explodeBombs;
        }
    }
}