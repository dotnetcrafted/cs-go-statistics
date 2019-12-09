using System.Collections.Generic;
using CsStat.Domain.Entities;
using DataService.Interfaces;

namespace BusinessFacade.Repositories.Implementations
{
    public class UsefulLinkRepository : IUsefulLinkRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public UsefulLinkRepository(IMongoRepositoryFactory mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
        public void AddInfo(UsefulInfo info)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateInfo(string id)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteInfo(string id)
        {
            throw new System.NotImplementedException();
        }

        public List<UsefulInfo> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public UsefulInfo GetInfo(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}