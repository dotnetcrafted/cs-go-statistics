using System;
using System.Collections.Generic;
using System.Linq;
using CsStat.Domain.Entities;
using CsStat.LogApi.Enums;
using DataService.Interfaces;
using MongoDB.Driver.Builders;

namespace BusinessFacade.Repositories.Implementations
{
    public class LogsRepository : BaseRepository, ILogsRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public IEnumerable<Log> GetAllLogs()
        {
           return base.GetAll<Log>();
        }

        public IEnumerable<Log> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo)
        {
            var query = new QueryBuilder<Log>();
            return _mongoRepository.GetRepository<Log>().Collection.Find(query.And(query.GTE(x => x.DateTime, timeFrom), query.LTE(x => x.DateTime, timeTo))).ToList();
        }

        public IEnumerable<Log> GetPlayerLogs(Player player)
        {
            var query = new QueryBuilder<Log>();
            return _mongoRepository.GetRepository<Log>().Collection.Find(query.Or(query.EQ(x=>x.Player.SteamId, player.SteamId), query.EQ(x=>x.Victim.SteamId, player.SteamId), query.EQ(x=>x.Action, Actions.TargetBombed))).ToList();
        }

        public LogsRepository(IMongoRepositoryFactory mongoRepository) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
    }
}