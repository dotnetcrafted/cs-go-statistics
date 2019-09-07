using System.Collections.Generic;
using System.Linq;
using CsStat.Domain.Entities;
using DataService.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;


namespace BusinessFacade.Repositories
{
    public interface ILogFileRepository
    {
        IEnumerable<LogFile> GetFiles();
        void AddFile(LogFile logFile);
        LogFile GetFileByName(string name);
        void UpdateFile(LogFile logFile);
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

        public LogFile GetFileByName(string name)
        {
            return _mongoRepository.GetRepository<LogFile>().Collection.Find(new QueryBuilder<LogFile>().EQ(x => x.Name, name)).FirstOrDefault();
        }

        public void UpdateFile(LogFile logFile)
        {
            if(string.IsNullOrEmpty(logFile?.Name))
                return;

            var file = GetFileByName(logFile.Name);

            if(file == null)
                return;

            file.Lenght = logFile.Lenght;

            _mongoRepository.GetRepository<LogFile>().Collection.Save(file);
        }
    }
}