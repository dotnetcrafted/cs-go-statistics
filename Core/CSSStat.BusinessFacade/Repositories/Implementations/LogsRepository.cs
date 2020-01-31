using System;
using System.Collections.Generic;
using System.Linq;
using CsStat.Domain.Entities;
using CsStat.LogApi.Enums;
using DataService.Interfaces;
using MongoDB.Driver;
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

        public IEnumerable<Log> GetPlayerLogs(Player player, DateTime timeFrom, DateTime timeTo)
        {
            var andList = new List<IMongoQuery>();

            andList.Add(Query.Or(Query.EQ("Player.SteamId", player.SteamId), Query.EQ("Victim.SteamId", player.SteamId), Query.EQ("Action", Actions.TargetBombed)));
            andList.Add(Query.And(Query.GTE("DateTime", timeFrom), Query.LTE("DateTime",timeTo)));

            var query = new QueryBuilder<Log>();

            var mongoQuery = query.And(andList);
            return _mongoRepository.GetRepository<Log>().Collection.Find(mongoQuery).ToList();
        }

        public LogsRepository(IMongoRepositoryFactory mongoRepository) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
    }
}