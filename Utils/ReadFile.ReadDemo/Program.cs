using System;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CsStat.Domain;
using CsStat.Domain.Entities.Demo;
using DataService;

namespace ReadFile.ReadDemo
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Start");
            Console.WriteLine($"Reading demo files from \"{Settings.DemosFolderPath}\"");

            var demoReader = new DemoReader(Settings.DemosFolderPath,
                new BaseFileRepository<DemoFile>(new MongoRepositoryFactory(new ConnectionStringFactory())),
                new BaseRepository(new MongoRepositoryFactory(new ConnectionStringFactory()))
            );

            demoReader.Start(Settings.TimerInterval);

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
            }

            demoReader.Stop();

            Console.WriteLine("Finish");
            Console.ReadLine();
        }
    }
}
