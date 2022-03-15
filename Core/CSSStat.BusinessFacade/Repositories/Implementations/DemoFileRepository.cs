using System.Collections.Generic;
using CsStat.Domain.Entities.Demo;
using DataService.Interfaces;

namespace BusinessFacade.Repositories.Implementations
{
    public class DemoFileRepository : BaseRepository, IDemoFileRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;
        public DemoFileRepository(IMongoRepositoryFactory mongoRepository) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public IEnumerable<DemoFile> GetFiles()
        {
            return GetAll<DemoFile>();
        }

        public void AddFile(DemoFile file)
        {
            Insert(file);
        }
    }
}