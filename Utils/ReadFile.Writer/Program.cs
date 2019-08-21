using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ReadFile.Writer
{
    internal static class Program
    {
        private static readonly CancellationTokenSource _tokenSource2 = new CancellationTokenSource();
        private static CancellationToken _ct = _tokenSource2.Token;

        private static async Task Main()
        {
            var currentDirectory = Environment.CurrentDirectory;
            var logsPath = ConfigurationManager.AppSettings["logsPath"];
            var logFileName = ConfigurationManager.AppSettings["logFileName"];
            
            await WriteInFile(Path.Combine(currentDirectory, logsPath), logFileName);

            Console.ReadLine();

            _tokenSource2.Cancel();
        }

        private static async Task WriteInFile(string path, string logFileName)
        {
            Console.WriteLine("WriteInFile()");

            try
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("Task");

                    if (Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    if (!File.Exists(Path.Combine(path, logFileName)))
                    {
                        using (var sw = File.CreateText(Path.Combine(path, logFileName)))
                        {
                            Console.WriteLine("create");

                            while (true)
                            {
                                Console.WriteLine("While create");
                                Thread.Sleep(2000);
                                sw.WriteLine("This");

                                if (_ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    _ct.ThrowIfCancellationRequested();
                                }
                            }
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            using (var sw = File.AppendText(Path.Combine(path, logFileName)))
                            {
                                Console.WriteLine("append");


                                Console.WriteLine("While append");
                                Thread.Sleep(2000);
                                sw.WriteLine("This");

                                if (_ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    _ct.ThrowIfCancellationRequested();
                                }
                            }
                        }
                    }
                }, _tokenSource2.Token);
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
            }
            finally
            {
                _tokenSource2.Dispose();
            }
        }
    }
}