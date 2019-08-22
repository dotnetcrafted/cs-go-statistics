using System;
using DataService.Interfaces;
using MongoRepository;

namespace DataService
{
    public class MongoRepositoryFactory : IMongoRepositoryFactory
    {
        private readonly string _connectionString;
        public MongoRepositoryFactory(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty", "connectionString");
            }

            _connectionString = connectionString;
        }
        public MongoRepository<T> GetRepository<T>() where T : Entity
        {
            return new MongoRepository<T>(_connectionString);
        }
    }
}