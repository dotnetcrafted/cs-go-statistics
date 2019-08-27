using System.Collections.Generic;
using System.Threading.Tasks;
using CSStat.CsLogsApi.Models;
using DataService.Interfaces;

namespace BusinessFacade.Repositories.Implementations
{
    public class LogsRepository : ILogsRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public LogsRepository(IMongoRepositoryFactory mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
        public void InsertLog(LogModel log)
        {
            _mongoRepository.GetRepository<LogModel>().Collection.Insert(log);
        }

        public void InsertBatch(List<LogModel> logs)
        {
            _mongoRepository.GetRepository<LogModel>().Collection.InsertBatch(logs);
        }

        public IEnumerable<LogModel> GetAllLogs()
        {
            return _mongoRepository.GetRepository<LogModel>().Collection.FindAll();
        }
    }
}