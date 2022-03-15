using System.Collections.Generic;
using System.Linq;
using BusinessFacade.Repositories.Implementations;
using CsStat.Domain.Entities;
using DataService.Interfaces;
using MongoDB.Driver.Builders;

namespace BusinessFacade.Repositories
{
    public interface ILogFileRepository 
    {
        void UpdateFile(LogFile logFile);
        LogFile GetFileByName(string path);
        void AddFile(LogFile file);
        IEnumerable<LogFile> GetFiles();
    }

    public class LogFileRepository : BaseRepository, ILogFileRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public LogFileRepository(IMongoRepositoryFactory mongoRepository)
            : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public void UpdateFile(LogFile logFile)
        {
            if (string.IsNullOrEmpty(logFile?.Path))
                return;

            var file = GetFileByName(logFile.Path);

            if (file == null)
                return;

            file.ReadBytes = logFile.ReadBytes;

            _mongoRepository.GetRepository<LogFile>().Collection.Save(file);
        }

        public LogFile GetFileByName(string path)
        {
            return _mongoRepository.GetRepository<LogFile>()
                .Collection.Find(new QueryBuilder<LogFile>().EQ(x => x.Path, path))
                .FirstOrDefault();
        }

        public void AddFile(LogFile file)
        {
            Insert(file);
        }

        public IEnumerable<LogFile> GetFiles()
        {
            return GetAll<LogFile>();
        }
    }
}