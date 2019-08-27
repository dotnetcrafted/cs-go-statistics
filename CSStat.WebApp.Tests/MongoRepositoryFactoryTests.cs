using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CSStat.CsLogsApi.Extensions;
using CSStat.CsLogsApi.Models;
using CsStat.Domain.Entities;
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

        private readonly IMongoRepositoryFactory _mongoRepository;
        private readonly IConnectionStringFactory _connectionString;
        private readonly ILogsRepository _logRepository;
        private readonly IPlayersRepository _playerRepository;
        public  MongoRepositoryFactoryTests()
        {
            _connectionString = new ConnectionStringFactory();
            _mongoRepository = new MongoRepositoryFactory(_connectionString);
            _logRepository = new LogsRepository(_mongoRepository);
            _playerRepository = new PlayersRepository(_logRepository);
        }
        [Test]
        public void ReturnRepositoryOfType()
        {
            var repository = _mongoRepository.GetRepository<TestEntity>();
            Assert.True(repository.GetType() == typeof(MongoRepository<TestEntity>));
        }

        [Test]
        public void ConnectToDataBase()
        {
            var connectionString = _connectionString.GetConnectionString();
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            server.GetDatabaseNames().ToList().ForEach(Console.WriteLine);
        }

        [Test]
        [TestCase(@"d:\Projects\counterstrikestat\Latest\CSStat.WebApp.Tests\TestParse\testLogs.txt")]
        public void SaveLog(string file)
        {
            var logs = string.Empty;

            using (var sr = new StreamReader(file))
            {
                logs = sr.ReadToEnd();
            }

            var splitLogs = logs.Split('\n').ToList();

            var api = new CsStat.LogApi.CsLogsApi();
            var logsToInsert = new List<LogModel>();

            foreach (var logLine in splitLogs)
            {
                logsToInsert.Add(api.ParseLine(logLine));
            }

            _logRepository.InsertBatch(logsToInsert);


        }

        [Test]
        public void GetLogsForPeriod()
        {
            var startDate = DateTime.ParseExact(@"08/20/2019 - 12:17:29", "MM/dd/yyyy - HH:mm:ss", CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(@"08/20/2019 - 12:20:32", "MM/dd/yyyy - HH:mm:ss", CultureInfo.InvariantCulture);
            var logs =_logRepository.GetLogsForPeriod(startDate, endDate);
            foreach (var logModel in logs)
            {
                Console.WriteLine(logModel.DateTime);
            }
        }

        [Test]
        public void GetPlayers()
        {
            var players = _playerRepository.GetAllPlayers();

            foreach (var player in players)
            {
                PrintLog(player);
                Console.WriteLine(Environment.NewLine);
            }
        }

        private static void PrintLog(PlayerModel log)
        {
            var gun = string.IsNullOrEmpty(log.FavoriteGun.GetDescription())
                ? log.FavoriteGun.ToString()
                : log.FavoriteGun.GetDescription();

            Console.WriteLine(
                ($"PlayerName: {log.PlayerName},Kills: {log.Kills},Deaths: {log.Death},Assists: {log.Assists}," +
                $"K/D ratio: {log.KdRatio},Total Games: {log.TotalGames},Kills Per Game: {log.KillsPerGame}," +
                $"Death Per Game: {log.DeathPerGame},Favorite Gun: {gun},Head shot: {log.HeadShot}%,Defused bombs: {log.Defuse},ExplodeBombs: {log.Explode}").Replace(',', '\n'));
        }
    }
}