using System;
using System.Collections.Generic;
using CsStat.Domain.Entities.Demo;
using DataService.Interfaces;

namespace BusinessFacade.Repositories.Implementations
{
    public class DemoRepository : IDemoRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public DemoRepository(IMongoRepositoryFactory mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public IEnumerable<DemoLog> GetAllLogs()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DemoLog> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo)
        {
            throw new NotImplementedException();
        }
    }
}