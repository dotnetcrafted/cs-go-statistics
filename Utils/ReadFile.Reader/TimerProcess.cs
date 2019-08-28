using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessFacade.Repositories;
using CsStat.Domain.Entities;
using CSStat.CsLogsApi.Interfaces;

namespace ReadFile.Reader
{
    public class TimerProcess
    {
        /*просматриваемая директория и файл для мониторинга*/
        private readonly ICsLogsApi parsers;
        private readonly IBaseRepository logRepository;
        private readonly ILogFileRepository logFileRepository;

        private readonly string logsDirectory;
        private static string lastLogFileName = "";


        /*чтение строк из файла*/
        private static int _skip = 0;
        private static int _take = 10;
        private static int fileReadNewLinesInterval = 1000;


        /*услоия для отмены или выполения задачи*/
        private static bool _isThereFilesInQueue = false;
        private static bool _isTaskFinish = true;
        private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private static CancellationToken _cancellationToken = _cancellationTokenSource.Token;


        /*общзие настройки таймера. По таймеру выполянется проверка наличия новых файлов в папке*/
        private const long TimerInterval = 5000;
        private static Timer _timer;
        private static readonly object _locker = new object();

        public TimerProcess(string logsDirectory, ICsLogsApi parsers, IBaseRepository logRepository, ILogFileRepository logFileRepository)
        {
            this.logsDirectory = logsDirectory;
            this.parsers = parsers;
            this.logRepository = logRepository;
            this.logFileRepository = logFileRepository;
        }

        public void Start()
        {
            _timer = new Timer(Callback, null, 0, TimerInterval);
        }

        public static void Stop()
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
            Console.WriteLine("Watch directory");

            var logFiles = Directory.GetFiles(logsDirectory);
            // взять только те файлы, которых нет в базе(все файлы которые есть в базе - уже обработаны)
            var newFiles = logFiles.Except(logFileRepository.GetFiles().Select(x => x.Name)).ToArray();

            if (!newFiles.Any() && !_isThereFilesInQueue)
                return;

            Console.WriteLine("There are new files or there is file in queue:");
            foreach (var file in newFiles)
            {
                var lines = File.ReadAllLines(file);
                _skip = lines.Length; // на данном шаге мы прочитаем все строки из файла и дальше нужно читать только новые строки
                foreach (var line in lines)
                {
                    // "скормить" все парсеру
                    var log = parsers.ParseLine(line);
                    logRepository.InsertLog(log);
                    Console.WriteLine(line);
                }

                // запомнить файл в базе, чтобы повторно его не обрабатывать
                logFileRepository.AddFile(new LogFile { Name = file});
                _isThereFilesInQueue = true; // запоминаем файл в очереди на обработку
            }

            // есть новые файлы, либо есть файла, которые находится в очереди на обработку
            if (newFiles.Length != 0 && lastLogFileName != newFiles.Last() || _isThereFilesInQueue)
            {
                lastLogFileName = newFiles.Length > 0 ? newFiles.Last() : lastLogFileName;
                
                //отменить предыдущую задачу. (задача будет отменена, но иссключение будет выброшено, когда файл прочитан до конца)
                _cancellationTokenSource.Cancel();

                if (_isTaskFinish)
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    _cancellationToken = _cancellationTokenSource.Token;

                    Task.Factory.StartNew(x =>
                    {
                        _isThereFilesInQueue = false; // очистили очередь
                        _isTaskFinish = false; // началась новая задача

                        var logFileName = (string) x;

                        while (true)
                        {
                            var fileStream = File.Open(logFileName, FileMode.Open);
                            var totalLines = FileHelper.CountLinesMaybe(fileStream);
                            fileStream.Close();

                            //задача была отменена, но надо убедиться, что файл прочитан до конца
                            if (_cancellationToken.IsCancellationRequested && totalLines == _skip/*дочитали файл до конца*/)
                            {
                                _skip = 0; // обнуляем, чтобы новый файл начать читать с нуля
                                _isTaskFinish = true; // текущая задача закончилась
                                _cancellationToken.ThrowIfCancellationRequested(); // прерываем выполнение текущей задачи
                            }

                            // читаем из файла по n-строк
                            var lines = File.ReadLines(logFileName).Skip(_skip).Take(_take).ToList();
                            if (lines.Count > 0)
                            {
                                _skip += lines.Count;
                                foreach (var line in lines)
                                {
                                    // "скормить" все парсеру
                                    var log = parsers.ParseLine(line);
                                    logRepository.InsertLog(log);
                                    Console.WriteLine(line);
                                }
                            }

                            Thread.Sleep(fileReadNewLinesInterval); // немного подождаем пока появятся новые записи в логе
                        }
                    }, lastLogFileName, _cancellationToken);
                }
            }
        }
    }
}