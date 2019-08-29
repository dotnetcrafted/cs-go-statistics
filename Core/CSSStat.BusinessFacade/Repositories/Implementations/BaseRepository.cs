using System.Collections.Generic;
using DataService.Interfaces;
using MongoRepository;

namespace BusinessFacade.Repositories.Implementations
{
    public class BaseRepository : IBaseRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public BaseRepository(IMongoRepositoryFactory mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public void InsertLog<T>(T entity) where T: Entity
        {
            _mongoRepository.GetRepository<T>().Collection.Insert(entity);
        }

        public void InsertBatch<T>(IEnumerable<T> entities) where T: Entity
        {
            _mongoRepository.GetRepository<T>().Collection.InsertBatch(entities);
        }
    }
}