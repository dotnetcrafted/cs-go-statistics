using CsStat.Domain.Entities;
using DataService.Interfaces;

namespace BusinessFacade.Repositories
{
    public interface ILogFileRepository : IFileRepository<LogFile>
    {
        void UpdateFile(LogFile logFile);
    }

    public class LogFileRepository : BaseFileRepository<LogFile>, ILogFileRepository
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
    }
}