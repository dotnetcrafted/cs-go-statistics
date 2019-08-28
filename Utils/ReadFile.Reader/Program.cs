using System;
using System.Configuration;
using System.IO;
using BusinessFacade.Repositories.Implementations;
using CsStat.LogApi;
using DataService;

namespace ReadFile.Reader
{
    internal static class Program
    {
        private static void Main()
        {
            var defaultForegroundColor = Console.ForegroundColor;
            Console.WriteLine("Start");

            var currentDirectory = Environment.CurrentDirectory;
            var logsDirectory = ConfigurationManager.AppSettings["logsDirectory"];

            var parser = new CsLogsApi();
            var logRepository = new BaseRepository(new MongoRepositoryFactory(new ConnectionStringFactory()));
            var timer = new TimerProcess(Path.Combine(currentDirectory, logsDirectory), parser, logRepository, logFileRepository);

            timer.Start();

            while (Console.ReadKey().Key != ConsoleKey.Escape) {}

            TimerProcess.Stop();

            Console.ForegroundColor = defaultForegroundColor;
            Console.WriteLine("Finish");
            Console.ReadLine();
        }
    }
}