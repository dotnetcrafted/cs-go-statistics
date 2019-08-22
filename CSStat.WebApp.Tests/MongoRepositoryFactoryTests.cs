using System;
using System.Configuration;
using System.Linq;
using CSStat.WebApp.Tests.Entity;
using DataService;
using MongoDB.Driver;
using MongoRepository;
using NUnit.Framework;

namespace CSStat.WebApp.Tests
{
    public class MongoRepositoryFactoryTests
    {
        [Test]
        public void ReturnRepositoryOfType()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            var mongoRepositoryFactory = new MongoRepositoryFactory(connectionString);
            var repository = mongoRepositoryFactory.GetRepository<TestEntity>();
            Assert.True(repository.GetType() == typeof(MongoRepository<TestEntity>));
        }

        [Test]
        public void ConnectToDataBase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            server.GetDatabaseNames().ToList().ForEach(Console.WriteLine);
        }
    }
}