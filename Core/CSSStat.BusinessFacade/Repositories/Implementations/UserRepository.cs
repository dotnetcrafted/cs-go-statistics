using System.Collections.Generic;
using System.Linq;
using CsStat.Domain.Entities;
using DataService.Interfaces;

namespace BusinessFacade.Repositories.Implementations
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private IMongoRepositoryFactory _mongoRepository;
        public UserRepository(IMongoRepositoryFactory mongoRepository) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public void Add(User user)
        {
           base.Insert(user);
        }

        public User GetByName(string name)
        {
            return _mongoRepository.GetRepository<User>().GetAll(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public IEnumerable<User> GetAll()
        {
           return base.GetAll<User>();
        }
    }
}