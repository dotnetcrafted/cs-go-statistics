using System;
using AutoMapper;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CsStat.Domain;
using CsStat.Domain.Entities.Demo;
using CsStat.LogApi;
using CsStat.LogApi.Interfaces;
using CsStat.StrapiApi;
using DataService;
using DataService.Interfaces;
using ErrorLogger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReadFile.ReadDemo.Profiles;

namespace ReadFile.ReadDemo
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Start");

            var builder = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder => {
                    configurationBuilder
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables();
                })
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddTransient<Reader>();
                   services.AddSingleton<ILogger, Logger>();
                   services.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
                   services.AddSingleton<IStrapiApi, StrapiApi>();
                   services.AddSingleton<IMongoRepositoryFactory, MongoRepositoryFactory>();
                   services.AddSingleton<IPlayerRepository, PlayerRepository>();
                   services.AddSingleton<ICsLogsApi, CsLogsApi>();
                   services.AddSingleton<IBaseRepository, BaseRepository>();
                   services.AddSingleton<ILogFileRepository, LogFileRepository>();
               });
            var host = builder.Build();
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var reader = services.GetRequiredService<Reader>();
                    reader.Run();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:");
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
        }
    }

    internal class Reader
    {
        private static MapperConfiguration Config =>
            new MapperConfiguration(cfg => { cfg.AddProfile<DemoProfile>(); });

        private readonly IBaseRepository _baseRepository;
        private readonly IMongoRepositoryFactory _mongoRepositoryFactory;

        public Reader(IMongoRepositoryFactory mongoRepositoryFactory, IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
            _mongoRepositoryFactory = mongoRepositoryFactory;
        }

        internal void Run()
        {
            Console.WriteLine($"Reading demo files from \"{Settings.DemosFolderPath}\" folder");

            var demoReader = new DemoReader(Settings.DemosFolderPath,
                new BaseFileRepository<DemoFile>(_mongoRepositoryFactory),
                _baseRepository,
                Config.CreateMapper()
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