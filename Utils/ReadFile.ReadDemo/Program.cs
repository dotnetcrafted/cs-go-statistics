﻿using System;
using AutoMapper;
using BusinessFacade.Repositories.Implementations;
using CsStat.Domain;
using DataService;
using ReadFile.ReadDemo.Profiles;

namespace ReadFile.ReadDemo
{
    internal static class Program
    {
        private static MapperConfiguration Config =>
            new MapperConfiguration(cfg => { cfg.AddProfile<DemoProfile>(); });

        private static void Main()
        {
            Console.WriteLine("Start");
            Console.WriteLine($"Reading demo files from \"{Settings.DemosFolderPath}\" folder");
            var progress = new Progress<string>(Console.WriteLine);

            var demoReader = new DemoReader(Settings.DemosFolderPath,
                new DemoFileRepository(new MongoRepositoryFactory(new ConnectionStringFactory())), 
                new BaseRepository(new MongoRepositoryFactory(new ConnectionStringFactory())),
                Config.CreateMapper(), progress
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