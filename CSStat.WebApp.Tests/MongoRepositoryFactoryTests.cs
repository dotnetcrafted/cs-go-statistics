using System;
using System.Configuration;
using System.IO;
using System.Linq;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CSStat.CsLogsApi.Extensions;
using CSStat.WebApp.Tests.Entity;
using DataService;
using DataService.Interfaces;
using MongoDB.Driver;
using MongoRepository;
using NUnit.Framework;

namespace CSStat.WebApp.Tests
{
    public class MongoRepositoryFactoryTests
    {

        private IMongoRepositoryFactory _mongoRepositoryFactory;
        private IConnectionStringFactory _connectionStringFactory;
        private ILogsRepository _logRepository;
        public  MongoRepositoryFactoryTests()
        {
            _connectionStringFactory = new ConnectionStringFactory();
            _mongoRepositoryFactory = new MongoRepositoryFactory(_connectionStringFactory);
            _logRepository = new LogsRepository(_mongoRepositoryFactory);
        }
        [Test]
        public void ReturnRepositoryOfType()
        {
            var repository = _mongoRepositoryFactory.GetRepository<TestEntity>();
            Assert.True(repository.GetType() == typeof(MongoRepository<TestEntity>));
        }

        [Test]
        public void ConnectToDataBase()
        {
            var connectionString = _connectionStringFactory.GetConnectionString();
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            server.GetDatabaseNames().ToList().ForEach(Console.WriteLine);
        }

        [Test]
        [TestCase(@"d:\Projects\counterstrikestat\Latest\CSStat.WebApp.Tests\TestParse\testString.txt")]
        public void SaveLog(string file)
        {
            var logLine = string.Empty;

            using (var sr = new StreamReader(file))
            {
                logLine = sr.ReadToEnd();
            }

            var splitLine = logLine.Split('"').ToList();

            var api = new CsStat.LogApi.CsLogsApi();

            var log = api.ParseLine(logLine);

            _logRepository.InsertLog(log);

        }

        [Test]
        public void GetLog()
        {
            Console.WriteLine(_logRepository.GetAllLogs().Count());
        }
    }
}