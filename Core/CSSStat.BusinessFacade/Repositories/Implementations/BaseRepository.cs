using System.Collections.Generic;
using System.Linq;
using DataService.Interfaces;
using MongoRepository.DAL;

namespace BusinessFacade.Repositories.Implementations
{
    public class BaseRepository : IBaseRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public BaseRepository(IMongoRepositoryFactory mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public void Insert<T>(T entity) where T : Entity
        {
            _mongoRepository.GetRepository<T>().Add(entity);
        }
        
        public void InsertBatch<T>(IEnumerable<T> entities) where T : Entity
        {
            if (entities != null && entities.Any())
                _mongoRepository.GetRepository<T>().Add(entities);
        }

        public IEnumerable<T> GetAll<T>() where T : Entity
        {
            return _mongoRepository.GetRepository<T>().GetAll(x=> true);
        }

        public T GetOne<T>(string id) where T : Entity
        {
            return _mongoRepository.GetRepository<T>().GetById(id);
        }
    }
}