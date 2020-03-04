using System;
using System.Collections.Generic;
using System.Linq;
using CsStat.Domain.Entities.Demo;
using DataService.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace BusinessFacade.Repositories.Implementations
{
    public class DemoRepository : BaseRepository, IDemoRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public DemoRepository(IMongoRepositoryFactory mongoRepository) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public IEnumerable<DemoLog> GetMatches()
        {
            return _mongoRepository.GetRepository<DemoLog>().Collection
                .FindAllAs<DemoLog>()
                .SetFields(Fields.Include(nameof(DemoLog.Id), nameof(DemoLog.MatchDate), nameof(DemoLog.Map)))
                .OrderByDescending(x => x.MatchDate)
                .ToList();
        }

        public IEnumerable<DemoLog> GetAllLogs()
        {
            return _mongoRepository.GetRepository<DemoLog>().Collection.FindAll();
        }

        public DemoLog GetMatch(string matchId)
        {
            var query = new QueryBuilder<DemoLog>();
            return _mongoRepository.GetRepository<DemoLog>().Collection.Find(query.EQ(x => x.Id, matchId))
                .FirstOrDefault();
        }

        public IEnumerable<DemoLog> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo)
        {
            throw new NotImplementedException();
        }
    }
}