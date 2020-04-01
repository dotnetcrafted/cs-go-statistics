using System;
using System.Security.Authentication;
using DataService.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
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
            BsonDefaults.MaxSerializationDepth = 500;
            return new MongoRepository<T>(_connectionString);
        }

        public MongoRepository<T> GetRepositoryFromUrl<T>() where T : Entity
        {
            var set = MongoClientSettings.FromUrl(new MongoUrl(_connectionString));
            set.SslSettings = new SslSettings{EnabledSslProtocols = SslProtocols.Tls12};
            
            return new MongoRepository<T>();
        }
    }
}