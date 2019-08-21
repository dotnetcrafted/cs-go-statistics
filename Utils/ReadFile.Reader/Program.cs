using System;
using System.Configuration;
using System.IO;

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

            var timer = new TimerProcess(Path.Combine(currentDirectory, logsDirectory));

            timer.Start();

            while (Console.ReadKey().Key != ConsoleKey.Escape) {}

            timer.Stop();

            Console.ForegroundColor = defaultForegroundColor;
            Console.WriteLine("Finish");
            Console.ReadLine();
        }
    }
}