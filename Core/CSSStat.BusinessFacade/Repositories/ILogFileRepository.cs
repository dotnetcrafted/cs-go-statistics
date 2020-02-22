using System.Collections.Generic;
using System.Linq;
using CsStat.Domain.Entities;
using DataService.Interfaces;
using MongoDB.Driver;


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
            return _mongoRepository.GetRepository<LogFile>();
        }

        public void AddFile(LogFile logFile)
        {
            _mongoRepository.GetRepository<LogFile>().Add(logFile);
        }

        public LogFile GetFileByName(string name)
        {
            return _mongoRepository.GetRepository<LogFile>().GetAll(x => x.Path == name).FirstOrDefault();
        }

        public void UpdateFile(LogFile logFile)
        {
            if(string.IsNullOrEmpty(logFile?.Path))
                return;

            var file = GetFileByName(logFile.Path);

            if(file == null)
                return;

            file.ReadBytes = logFile.ReadBytes;

            _mongoRepository.GetRepository<LogFile>().Update(file);
        }
    }
}