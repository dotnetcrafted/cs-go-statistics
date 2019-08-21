using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace ReadFile.Reader
{
    public class TimerProcess
    {
        private readonly string logsDirectory;
        
        private static string lastLogFileName = "";


        private const long TimerInterval = 1000;
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

            Console.WriteLine("Watch directory: ");

            var logFiles = Directory.GetFiles(logsDirectory);
            if (logFiles.Any() && (lastLogFileName == null || lastLogFileName != logFiles.Last()))
            {
                lastLogFileName = logFiles.Last();
                Console.WriteLine($"Detected new log file: {lastLogFileName}");
            }
            else
            {
                Console.WriteLine($"there are not any new file");

            }
        }
    }
}