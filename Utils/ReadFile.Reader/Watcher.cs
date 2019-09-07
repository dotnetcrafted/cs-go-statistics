using System.Collections.Generic;
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

        private static string _lastReadLine = "";

        private static long _skip = 0;
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
            ColorConsole.Yellow("Watch directory");

            var allFiles = Directory.GetFiles(logsPath);
            var newFiles = allFiles.Except(logFileRepository.GetFiles().Select(x => x.Name)).ToArray();

            if (!newFiles.Any() && !_isThereFileInQueue)
            {
                if (allFiles.Length != 0 && allFiles.Last() != _lastFile)
                {
                    _lastFile = allFiles.Length > 0 ? allFiles.Last() : _lastFile;

                    var logFile = logFileRepository.GetFileByName(_lastFile);
                    _skip = logFile.Lenght;

                    ReadLinesFromFile();
                }
                return;
            }

            ColorConsole.Yellow("There are new files or there is file in queue");
            foreach (var file in newFiles)
            {
                var lines = new List<string>();
                long position;
                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                        while (streamReader.Peek() >= 0)
                        {
                            lines.Add(streamReader.ReadLine());
                        }
                        position = streamReader.BaseStream.Length;
                    }
                }

                _skip = position;

                var logs = parsers.ParseLogs(lines.ToList());
                if (logs != null)
                {
                    ColorConsole.Yellow($"{logs.Count} logs will be added");
                    ColorConsole.Yellow($"Last read lines is {lines.Last()}");
                    logRepository.InsertBatch(logs);
                }

                logFileRepository.AddFile(new LogFile { Name = file, Lenght = position });
                _isThereFileInQueue = true;
            }

            if (newFiles.Length != 0 && _lastFile != newFiles.Last() || _isThereFileInQueue)
            {
                _lastFile = newFiles.Length > 0 ? newFiles.Last() : _lastFile;

                ReadLinesFromFile();
            }
        }

        private void ReadLinesFromFile()
        {
            _cancellationTokenSource.Cancel();

            ColorConsole.Yellow($"Last read file is \"{_lastFile}\"");
            if (_isTaskFinish)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _cancellationToken = _cancellationTokenSource.Token;

                var taskOptions = new TaskOptions
                {
                    Path = _lastFile,
                    Skip = _skip
                };

                Task.Factory.StartNew(x =>
                {
                    ColorConsole.Green("Check new lines in the file");

                    _isThereFileInQueue = false;
                    _isTaskFinish = false;

                    var options = (TaskOptions)x;

                    while (true)
                    {
                        if (_cancellationToken.IsCancellationRequested && FileHelper.FileLengthInBytes(options.Path) <= options.Skip)
                        {
                            options.Skip = 0;
                            _isTaskFinish = true;
                            _cancellationToken.ThrowIfCancellationRequested();
                        }

                        var lines = new List<string>();
                        using (var fileStream = new FileStream(options.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (var streamReader = new StreamReader(fileStream))
                            {
                                streamReader.BaseStream.Seek(options.Skip, SeekOrigin.Begin);
                                while (streamReader.Peek() >= 0)
                                {
                                    var readLine = streamReader.ReadLine();
                                    if (!string.IsNullOrEmpty(readLine))
                                    {
                                        lines.Add(readLine);
                                        options.Skip = streamReader.BaseStream.Length;
                                        logFileRepository.UpdateFile(new LogFile
                                        {
                                            Name = options.Path,
                                            Lenght = options.Skip
                                        });
                                    }
                                }
                            }
                        }

                        if (lines.Count > 0)
                        {
                            var logs = parsers.ParseLogs(lines);
                            if (logs != null)
                            {
                                ColorConsole.Yellow($"{logs.Count} logs will be added");

                                logRepository.InsertBatch(logs);

                                _lastReadLine = lines.Last();
                            }
                        }

                        ColorConsole.Green($"New Lines were read {lines.Count}");
                        ColorConsole.Green($"Last read line is \"{_lastReadLine}\"");

                        Thread.Sleep(_fileReadNewLinesInterval);
                    }
                }, taskOptions, _cancellationToken);
            }
        }
    }
}