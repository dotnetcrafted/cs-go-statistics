using System;
using DataService.Interfaces;
using MongoRepository;

namespace DataService
{
    public class MongoRepositoryFactory : IMongoRepositoryFactory
    {
        private readonly IConnectionStringFactory _connectionStringFactory;
        private readonly string _connectionString;
        public MongoRepositoryFactory(IConnectionStringFactory connectionStringFactory)
        {
            _connectionStringFactory = connectionStringFactory;

            var connectionString = _connectionStringFactory.GetConnectionString();

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