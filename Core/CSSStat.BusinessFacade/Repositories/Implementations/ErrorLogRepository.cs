using System;
using CsStat.Domain.Entities;
using DataService.Interfaces;

namespace BusinessFacade.Repositories.Implementations
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public ErrorLogRepository(IMongoRepositoryFactory mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
        public void Error(Error error)
        {
            _mongoRepository.GetRepository<Error>().Collection.Insert(error);
        }
    }
}