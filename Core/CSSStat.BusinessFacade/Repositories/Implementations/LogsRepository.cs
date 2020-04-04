using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CsStat.Domain.Entities;
using CsStat.LogApi.Enums;
using DataService.Interfaces;
using MongoDB.Driver;

namespace BusinessFacade.Repositories.Implementations
{
    public class LogsRepository : BaseRepository, ILogsRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public IEnumerable<Log> GetAllLogs()
        {
           return base.GetAll<Log>();
        }

        public IEnumerable<Log> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo, Expression<Func<Log, bool>> сondition = null)
        {
            var logs = _mongoRepository.GetRepository<Log>().Where(x => timeFrom >= x.DateTime && x.DateTime <= timeTo);

            if (сondition != null)
            {
                logs = logs.Where(сondition);
            }
                
            return logs.ToList();
        }

        public IEnumerable<Log> GetPlayerLogs(Player player, DateTime timeFrom, DateTime timeTo)
        {
            var logs = _mongoRepository.GetRepository<Log>().Where( x =>
                    (x.Player.SteamId == player.SteamId || x.Victim.SteamId == player.SteamId || x.Action == Actions.TargetBombed)
                    &&
                    (timeFrom <= x.DateTime && x.DateTime <= timeTo)
                ).ToList();
            return logs;
        }

        public LogsRepository(IMongoRepositoryFactory mongoRepository) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
    }
}