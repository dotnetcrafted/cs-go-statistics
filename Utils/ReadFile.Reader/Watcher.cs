using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessFacade.Repositories;
using CsStat.Domain;
using CsStat.Domain.Entities;
using CsStat.LogApi.Interfaces;

namespace ReadFile.Reader
{
    public class Watcher
    {
        private readonly string logsPath;
        private static string _lastFile = "";


        private static int _skip = 0;
        private static readonly int _take = Settings.TakeLines;
        private static readonly int _fileReadNewLinesInterval = Settings.FileReadNewLinesInterval;


        private static bool _isTaskFinish = true;
        private static bool _isThereFileInQueue = false;
        private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private static CancellationToken _cancellationToken = _cancellationTokenSource.Token;


        private Timer _timer;
        private static readonly long _timerInterval = Settings.TimerInterval;
        private static readonly object _locker = new object();


        private readonly ICsLogsApi parsers;
        private readonly IBaseRepository logRepository;
        private readonly ILogFileRepository logFileRepository;

        public Watcher(string logsPath, ICsLogsApi parsers, IBaseRepository logRepository, ILogFileRepository logFileRepository)
        {
            this.logsPath = logsPath;
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

                WatchDirectory();
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

        private void WatchDirectory()
        {
            Console.WriteLine("Watch directory");

            var allFiles = Directory.GetFiles(logsPath);
            var newFiles = allFiles.Except(logFileRepository.GetFiles().Select(x => x.Name)).ToArray();

            if (!newFiles.Any() && !_isThereFileInQueue)
                return;

            Console.WriteLine("There are new files or there is file in queue");
            foreach (var file in newFiles)
            {
                var lines = File.ReadAllLines(file);
                _skip = lines.Length;
                
                logRepository.InsertBatch(parsers.ParseLogs(lines.ToList()));

                logFileRepository.AddFile(new LogFile { Name = file});
                _isThereFileInQueue = true;
            }

            if (newFiles.Length != 0 && _lastFile != newFiles.Last() || _isThereFileInQueue)
            {
                _lastFile = newFiles.Length > 0 ? newFiles.Last() : _lastFile;

                _cancellationTokenSource.Cancel();

                if (_isTaskFinish)
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    _cancellationToken = _cancellationTokenSource.Token;

                    Task.Factory.StartNew(x =>
                    {
                        _isThereFileInQueue = false;
                        _isTaskFinish = false;

                        var path = (string) x;

                        while (true)
                        {
                            var fileStream = File.Open(path, FileMode.Open);
                            var totalLines = FileHelper.CountLines(fileStream);
                            fileStream.Close();

                            if (_cancellationToken.IsCancellationRequested && totalLines <= _skip)
                            {
                                _skip = 0;
                                _isTaskFinish = true;
                                _cancellationToken.ThrowIfCancellationRequested();
                            }

                            var lines = File.ReadLines(path).Skip(_skip).Take(_take).ToList();
                            if (lines.Count > 0)
                            {
                                _skip += lines.Count;
                                logRepository.InsertBatch(parsers.ParseLogs(lines));
                            }

                            Thread.Sleep(_fileReadNewLinesInterval);
                        }
                    }, _lastFile, _cancellationToken);
                }
            }
        }
    }
}