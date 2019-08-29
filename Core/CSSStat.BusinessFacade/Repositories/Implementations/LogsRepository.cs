using System;
using System.Collections.Generic;
using System.Linq;
using CsStat.Domain.Entities;
using DataService.Interfaces;
using MongoDB.Driver.Builders;

namespace BusinessFacade.Repositories.Implementations
{
    public class LogsRepository : ILogsRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public LogsRepository(IMongoRepositoryFactory mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
  
        public IEnumerable<Log> GetAllLogs()
        {
            return _mongoRepository.GetRepository<Log>().Collection.FindAll();
        }

        public IEnumerable<Log> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo)
        {
            var query = new QueryBuilder<Log>();
            return _mongoRepository.GetRepository<Log>().Collection.Find(query.And(query.GTE(x => x.DateTime, timeFrom), query.LTE(x => x.DateTime, timeTo))).ToList();
        }
    }
}