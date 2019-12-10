using System.Collections.Generic;
using CsStat.Domain.Entities;
using DataService.Interfaces;

namespace BusinessFacade.Repositories.Implementations
{
    public class UsefulLinkRepository :BaseRepository, IUsefulLinkRepository
    {
        private IMongoRepositoryFactory _mongoRepository;
        public UsefulLinkRepository(IMongoRepositoryFactory mongoRepository) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public void AddInfo(UsefulInfo info)
        {
            base.Insert(info);
        }

        public bool UpdateInfo(string id)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteInfo(string id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<UsefulInfo> GetAll()
        {
           return base.GetAll<UsefulInfo>();
        }

        public UsefulInfo GetInfo(string id)
        {
            return base.GetOne<UsefulInfo>(id);
        }
    }
}
