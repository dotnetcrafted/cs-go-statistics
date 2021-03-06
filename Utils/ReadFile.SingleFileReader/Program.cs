﻿using System;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CsStat.Domain;
using CsStat.LogApi;
using DataService;

namespace ReadFile.SingleFileReader
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Start");

            var parser = new CsLogsApi();
            var logRepository = new BaseRepository(new MongoRepositoryFactory(new ConnectionStringFactory()));
            var fileRepository = new LogFileRepository(new MongoRepositoryFactory(new ConnectionStringFactory()));

            Console.WriteLine($"Read logs from \"{Settings.ConsoleLogsPath}\"");

            var watcher = new Reader(Settings.ConsoleLogsPath, parser, logRepository, fileRepository);

            watcher.Start();

            while (Console.ReadKey().Key != ConsoleKey.Escape) { }

            watcher.Stop();

            Console.WriteLine("Finish");
            Console.ReadLine();
        }
    }
}
