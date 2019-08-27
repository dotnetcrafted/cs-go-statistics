using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReadFile.Reader
{
    public class TimerProcess
    {
        private readonly string logsDirectory;
        private readonly TmpFiles tmpFiles = new TmpFiles();
        private static string lastLogFileName = "";

        private static int skip = 0;
        private static int take = 10;


        private static bool isNewFileInQueue = false;
        private static bool isTaskFinish = true;
        private static CancellationTokenSource cts = new CancellationTokenSource();
        private static CancellationToken ct = cts.Token;


        private const long TimerInterval = 5000;
        private static Timer _timer;
        private static object _locker = new object();

        public TimerProcess(string logsDirectory)
        {
            this.logsDirectory = logsDirectory;
        }

        public void Start()
        {
            _timer = new Timer(Callback, null, 0, TimerInterval);
        }

        public void Stop()
        {
            _timer.Dispose();
        }

        private void Callback(object state)
        {
            var hasLock = false;
            try
            {
                Monitor.TryEnter(_locker, ref hasLock);
                if (!hasLock)
                {
                    return;
                }

                _timer.Change(Timeout.Infinite, Timeout.Infinite);

                WatchDirectory();
            }
            finally
            {
                if (hasLock)
                {
                    Monitor.Exit(_locker);
                    _timer.Change(TimerInterval, TimerInterval);
                }
            }
        }

        private void WatchDirectory()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("Watch directory");

            var logFiles = Directory.GetFiles(logsDirectory);
            var newFiles = logFiles.Except(tmpFiles.Files).ToArray();

            if (!newFiles.Any() && !isNewFileInQueue)
                return;

            Console.WriteLine("New files:");
            foreach (var file in newFiles)
            {
                // "скормить" все парсеру
                Console.WriteLine(file);
                tmpFiles.Files.Add(file);
                isNewFileInQueue = true;
            }

            if (newFiles.Length != 0 && lastLogFileName != newFiles.Last() || isNewFileInQueue)
            {
                lastLogFileName = newFiles.Length > 0 ? newFiles.Last() : lastLogFileName;

                cts.Cancel();

                if (isTaskFinish)
                {
                    cts = new CancellationTokenSource();
                    ct = cts.Token;
                    Task.Factory.StartNew((x) =>
                    {
                        isNewFileInQueue = false; // очистили очередь
                        isTaskFinish = false;
                        var logFileName = (string) x;

                        while (true)
                        {
                            var fileStream = File.Open(logFileName, FileMode.Open);
                            if (ct.IsCancellationRequested && FileHelper.CountLinesMaybe(fileStream) == skip)
                            {
                                fileStream.Close();
                                isTaskFinish = true;
                                skip = 0;
                                ct.ThrowIfCancellationRequested();
                            }

                            fileStream.Close();

                            var lines = File.ReadLines(logFileName).Skip(skip).Take(take).ToList();
                            if (lines.Count > 0)
                            {
                                skip += lines.Count;
                            }

                            foreach (var line in lines)
                            {
                                Console.WriteLine(line);
                            }

                            Thread.Sleep(1000);
                        }
                    }, lastLogFileName, ct);
                }
            }
        }
    }

    public class TmpFiles
    {
        public TmpFiles()
        {
            Files = new List<string>();
        }

        public List<string> Files { get; set; }
    }
}