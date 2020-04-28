using System.Collections.Generic;
using System.Linq;
using CsStat.Domain.Entities;
using DataService.Interfaces;
using MongoDB.Driver.Builders;

namespace BusinessFacade.Repositories
{
    public interface IFileRepository<T> where T : FileEntity
    {
        IEnumerable<T> GetFiles();
        void AddFile(T demoFile);
        T GetFileByName(string name);
    }

    public class BaseFileRepository<T> : IFileRepository<T> where T : FileEntity
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public BaseFileRepository(IMongoRepositoryFactory mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public IEnumerable<T> GetFiles()
        {
            return _mongoRepository.GetRepository<T>().Collection.FindAll();
        }

        public void AddFile(T demoFile)
        {
            _mongoRepository.GetRepository<T>().Collection.Insert(demoFile);
        }

        public T GetFileByName(string name)
        {
            return _mongoRepository.GetRepository<T>()
                .Collection.Find(new QueryBuilder<T>().EQ(x => x.Path, name)).FirstOrDefault();
        }

    }
}