using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BusinessFacade.Repositories;
using CsStat.Domain;
using CsStat.Domain.Entities;
using CsStat.LogApi.Interfaces;
using CsStat.SystemFacade;

namespace ReadFile.SingleFileReader
{
    public class Reader
    {
        private readonly string path;
        private readonly ICsLogsApi parsers;
        private readonly IBaseRepository logRepository;
        private readonly ILogFileRepository logFileRepository;

        private Timer _timer;
        private static readonly long _timerInterval = Settings.TimerInterval;
        private static readonly object _locker = new object();

        public Reader(string path, ICsLogsApi parsers, IBaseRepository logRepository,
            ILogFileRepository logFileRepository)
        {
            this.path = path;
            this.parsers = parsers;
            this.logRepository = logRepository;
            this.logFileRepository = logFileRepository;
        }

        public void Start()
        {
            _timer = new Timer(Callback, null, 0, _timerInterval);
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

                try
                {
                    ReadFile();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            finally
            {
                if (hasLock)
                {
                    Monitor.Exit(_locker);
                    _timer.Change(_timerInterval, _timerInterval);
                }
            }
        }

        private void ReadFile()
        {
            Console.WriteLine($"{DateTime.Now} | Checking file");

            if (!File.Exists(path))
                return;

            var createdDate = File.GetCreationTime(path);

            var logFile = logFileRepository.GetFileByName(path);
            if (logFile != null)
            {
                if (createdDate.MongoEquals(logFile.Created))
                {
                    logFile.ReadBytes = 0;
                }

                ReadLines(logFile);
                logFileRepository.UpdateFile(logFile);
            }
            else
            {
                logFile = new LogFile
                {
                    Path = path,
                    Created = File.GetCreationTime(path),
                    ReadBytes = 0
                };

                ReadLines(logFile);
                logFileRepository.AddFile(logFile);
            }
        }

        private void ReadLines(LogFile logFile)
        {
            var lines = new List<string>();
            using (var fileStream = new FileStream(logFile.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    streamReader.BaseStream.Seek(logFile.ReadBytes, SeekOrigin.Begin);
                    while (streamReader.Peek() >= 0)
                    {
                        var readLine = streamReader.ReadLine();
                        if (!string.IsNullOrEmpty(readLine))
                        {
                            lines.Add(readLine);
                        }
                    }

                    logFile.ReadBytes = streamReader.BaseStream.Length;
                }
            }

            if (lines.Count > 0)
            {
                var logs = parsers.ParseLogs(lines);
                if (logs != null)
                {
                    Console.WriteLine($"{logs.Count} logs will be added");

                    logRepository.InsertBatch(logs);

                    Console.WriteLine($"Last read line is \"{lines.Last()}\"");
                }
            }
        }
    }
}