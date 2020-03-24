using System;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CsStat.Domain;
using CsStat.LogApi;
using CsStat.LogApi.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ReadFile.SingleFileReader
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Start");

            var builder = new HostBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddTransient<Reader>();
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
                    var reader = services.GetRequiredService<SingleFileReader>();
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

    internal class SingleFileReader
    {
        private readonly ICsLogsApi _parser;
        private readonly IBaseRepository _baseRepository;
        private readonly ILogFileRepository _logFileRepository;

        public SingleFileReader(ICsLogsApi csLogsApi, IBaseRepository baseRepository, ILogFileRepository logFileRepository)
        {
            _parser = csLogsApi;
            _baseRepository = baseRepository;
            _logFileRepository = logFileRepository;
        }

        internal void Run()
        {
            Console.WriteLine($"Read logs from \"{Settings.ConsoleLogsPath}\"");

            var watcher = new Reader(Settings.ConsoleLogsPath, _parser, _baseRepository, _logFileRepository);

            watcher.Start();

            while (Console.ReadKey().Key != ConsoleKey.Escape) { }

            watcher.Stop();

            Console.WriteLine("Finish");
            Console.ReadLine();
        }
    }
}
