using System;
using System.Threading.Tasks;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CsStat.Domain;
using CsStat.LogApi;
using CsStat.LogApi.Interfaces;
using CsStat.StrapiApi;
using DataService;
using DataService.Interfaces;
using ErrorLogger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ReadFile.Reader
{
    internal static class Program
    {
        private static void Main()
        {
            ColorConsole.Default("Start");

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
                   services.AddSingleton<IPlayerRepository, PlayerRepository> ();
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
                    ColorConsole.Red("Error:");
                    ColorConsole.Red(ex.Message);
                    Console.ReadLine();
                }
            }
        }
    }

    internal class Reader
    {
        private readonly ICsLogsApi _parser;
        private readonly IBaseRepository _baseRepository;
        private readonly ILogFileRepository _logFileRepository;

        public Reader(ICsLogsApi csLogsApi, IBaseRepository baseRepository, ILogFileRepository logFileRepository)
        {
            _parser = csLogsApi;
            _baseRepository = baseRepository;
            _logFileRepository = logFileRepository;
        }

        internal void Run()
        {
            ColorConsole.Default($"Read logs from \"{Settings.LogsPath}\"");

            var watcher = new Watcher(Settings.LogsPath, _parser, _baseRepository, _logFileRepository);

            watcher.Start();

            while (Console.ReadKey().Key != ConsoleKey.Escape) { }

            watcher.Stop();

            ColorConsole.Default("Finish");
            Console.ReadLine();
        }
    }
}