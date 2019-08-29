using MongoRepository;

namespace DataService.Interfaces
{
    public interface IMongoRepositoryFactory
    {
        MongoRepository<T> GetRepository<T>() where T : Entity;
    }
}