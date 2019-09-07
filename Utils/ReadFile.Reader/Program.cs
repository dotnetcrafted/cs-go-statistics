using System;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CsStat.Domain;
using CsStat.LogApi;
using DataService;

namespace ReadFile.Reader
{
    internal static class Program
    {
        private static void Main()
        {
            ColorConsole.Default("Start");

            var parser = new CsLogsApi();
            var logRepository = new BaseRepository(new MongoRepositoryFactory(new ConnectionStringFactory()));
            var fileRepository = new LogFileRepository(new MongoRepositoryFactory(new ConnectionStringFactory()));

            ColorConsole.Default($"Read logs from \"{Settings.LogsPath}\"");

            var watcher = new Watcher(Settings.LogsPath, parser, logRepository, fileRepository);
            
            watcher.Start();

            while (Console.ReadKey().Key != ConsoleKey.Escape) { }

            watcher.Stop();

            ColorConsole.Default("Finish");
            Console.ReadLine();
        }
    }
}