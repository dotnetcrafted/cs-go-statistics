using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessFacade.Repositories;
using CsStat.Domain;
using CsStat.Domain.Entities;
using CsStat.LogApi.Interfaces;
using CsStat.SystemFacade;
using CsStat.SystemFacade.DummyCache;
using CsStat.SystemFacade.DummyCacheFactories;
using DataService;
using ErrorLogger;
using UpdateCacheService;

namespace ReadFile.SingleFileReader
{
    public class Reader : BaseWatcher
    {
        private readonly string path;
        private readonly ICsLogsApi parsers;
        private readonly IBaseRepository logRepository;
        private readonly ILogFileRepository logFileRepository;
        private readonly IProgress<string> _progress;

        private Timer _timer;
        private static readonly long _timerInterval = Settings.TimerInterval;
        private static readonly object _locker = new object();

        private readonly IPlayersCacheService _cacheService;
        private readonly ILogger _logger;

        public Reader(string path, ICsLogsApi parsers, IBaseRepository logRepository,
            ILogFileRepository logFileRepository, IProgress<string> progress)
        {
            this.path = path;
            this.parsers = parsers;
            this.logRepository = logRepository;
            this.logFileRepository = logFileRepository;
            _progress = progress;
            _cacheService = new PlayersCacheService();
            var connectionString = new ConnectionStringFactory();
            var mongoRepository = new MongoRepositoryFactory(connectionString);
            _logger = new Logger(mongoRepository);
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
                    _progress.Report(e.Message);
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

        protected override void ReadFile()
        {
            _progress.Report($"{DateTime.Now} | Checking file");
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

                try
                {
                    ReadLines(logFile);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Read file exception");
                }

                logFileRepository.UpdateFile(logFile);
            }
            else
            {
                _progress.Report("new file");
                logFile = new LogFile
                {
                    Path = path,
                    Created = File.GetCreationTime(path),
                    ReadBytes = 0
                };
                try
                {
                    ReadLines(logFile);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Read file exception");
                }
                logFileRepository.AddFile(logFile);
            }
        }

        private async void ReadLines(LogFile logFile)
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

            if (lines.Count <= 0) 
                return;

            var logs = parsers.ParseLogs(lines);
            if (!logs.Any()) 
                return;

            _progress.Report($"{logs.Count} logs will be added");
            logRepository.InsertBatch(logs);
            await _cacheService.ClearPlayersCache();
            _progress.Report($"Last read line is \"{lines.Last()}\"");
        }
    }
}