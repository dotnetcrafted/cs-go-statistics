using System;
using System.Collections.Generic;
using CsStat.Domain.Entities.Demo;
using DataService.Interfaces;

namespace BusinessFacade.Repositories.Implementations
{
    public class DemoRepository : BaseRepository, IDemoRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public DemoRepository(IMongoRepositoryFactory mongoRepository) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public IEnumerable<DemoLog> GetAllLogs()
        {
            return _mongoRepository.GetRepository<DemoLog>().Collection.FindAll();
        }

        public IEnumerable<DemoLog> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo)
        {
            throw new NotImplementedException();
        }
    }
}