using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<DemoLog> GetMatches()
        {
            return _mongoRepository.GetRepository<DemoLog>()
                .Select(x => new DemoLog { 
                    Id = x.Id,
                    ParsedDate = x.ParsedDate,
                    MatchDate = x.MatchDate,
                    Map= x.Map,
                    TotalSquadAScore = x.TotalSquadAScore,
                    TotalSquadBScore = x.TotalSquadBScore,
                    Duration = x.Duration
                })
                .OrderByDescending(x => x.MatchDate)
                .ToList();
        }

        public IEnumerable<DemoLog> GetAllLogs()
        {
            return _mongoRepository.GetRepository<DemoLog>();
        }

        public DemoLog GetMatch(string matchId)
        {
            return _mongoRepository.GetRepository<DemoLog>().FirstOrDefault(x => x.Id == matchId);
        }

        public IEnumerable<DemoLog> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo)
        {
            throw new NotImplementedException();
        }
    }
}