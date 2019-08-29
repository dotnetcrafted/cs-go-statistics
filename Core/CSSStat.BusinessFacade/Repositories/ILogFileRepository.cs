using System.Collections.Generic;
using CsStat.Domain.Entities;
using DataService.Interfaces;


namespace BusinessFacade.Repositories
{
    public interface ILogFileRepository
    {
        IEnumerable<LogFile> GetFiles();
        void AddFile(LogFile logFile);
    }

    public class LogFileRepository : ILogFileRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public LogFileRepository(IMongoRepositoryFactory mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public IEnumerable<LogFile> GetFiles()
        {
            return _mongoRepository.GetRepository<LogFile>().Collection.FindAll();
        }

        public void AddFile(LogFile logFile)
        {
            _mongoRepository.GetRepository<LogFile>().Collection.Insert(logFile);
        }
    }
}