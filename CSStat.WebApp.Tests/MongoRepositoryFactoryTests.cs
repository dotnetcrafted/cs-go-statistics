﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CSStat.CsLogsApi.Extensions;
using CsStat.Domain.Definitions;
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
        private readonly IPlayerRepository _playerRepository;
        private readonly IBaseRepository _baseRepository;
        public  MongoRepositoryFactoryTests()
        {
            _connectionString = new ConnectionStringFactory();
            _mongoRepository = new MongoRepositoryFactory(_connectionString);
            _logRepository = new LogsRepository(_mongoRepository);
            _playerRepository = new PlayerRepository(_mongoRepository);
            _baseRepository = new BaseRepository(_mongoRepository);
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
            var logsToInsert = new List<Log>();

            foreach (var logLine in splitLogs)
            {
                logsToInsert.Add(api.ParseLine(logLine));
            }

            _baseRepository.InsertBatch(logsToInsert);


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
            var players = _playerRepository.GetStatsForAllPlayers();

            foreach (var player in players)
            {
                PrintPlayerStat(player);
                Console.WriteLine(Environment.NewLine);
            }
        }
        [Test]
        [TestCase(@"\Images\test1.jpg")]
        public void UpdatePlayer(string imagePath)
        {
            var player = _playerRepository.GetPlayerByNickName("Radik F.");
            _playerRepository.UpdatePlayer(player.Id, "Radik", "Faiskhanov", imagePath);
        }
        [Test]
        public void GetPlayer()
        {
            PrintPlayer(_playerRepository.GetPlayerByNickName("Radik F."));
        }

        [Test]
        public void GetLogs()
        {
            var logs = _logRepository.GetAllLogs().Where(x => x.Gun == Guns.Unknown).ToList();
            logs.ForEach(PrintLog);
        }

        private static byte[] ReadImage(string path)
        {
          using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
          {
              using (var br = new BinaryReader(fs))
              {
                  var fInfo = new FileInfo(path);
                  return br.ReadBytes((int) fInfo.Length);
              }
          }
        }

        private static void PrintPlayer(Player player)
        {
            Console.WriteLine($"First Name: {player.FirstName},Second Name: {player.SecondName},Nick Name: {player.NickName},Image: {player.ImagePath}".Replace(',', '\n'));
        }

        private static void PrintPlayerStat(PlayerStatsModel log)
        {
            var gun = string.IsNullOrEmpty(log.FavoriteGun.GetDescription())
                ? log.FavoriteGun.ToString()
                : log.FavoriteGun.GetDescription();

            Console.WriteLine(
                ($"PlayerName: {log.Player.NickName},Kills: {log.Kills},Deaths: {log.Death},Assists: {log.Assists}," +
                $"K/D ratio: {log.KdRatio},Total Games: {log.TotalGames},Kills Per Game: {log.KillsPerGame}," +
                $"Death Per Game: {log.DeathPerGame},Favorite Gun: {gun},Head shot: {log.HeadShot}%,Defused bombs: {log.Defuse},Explode Bombs: {log.Explode}").Replace(',', '\n'));
        }

        private static void PrintLog(Log log)
        {
            var action = string.IsNullOrEmpty(log.Action.GetDescription())
                ? log.Action.ToString()
                : log.Action.GetDescription();

            Console.WriteLine(
                ($"PlayerName: {log.Player.NickName},PlayerTeam: {log.PlayerTeam.GetDescription()},Action: {action},VictimName: {log.Victim.NickName},VictimTeam: {log.VictimTeam.GetDescription()}," +
                $"Gun: {log.Gun.GetDescription()},IsHeadshot: {log.IsHeadShot},DateTime: {log.DateTime.ToString(new CultureInfo("ru-RU", false).DateTimeFormat)}")
                    .Replace(',', '\n'));
            Console.WriteLine(Environment.NewLine);
        }
    }
}