using System;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CsStat.Domain.Entities;
using DataService.Interfaces;

namespace ErrorLogger
{
    public interface ILogger
    {
        void Error(string latestString, Exception exception, string message);
        void Error(Exception exception, string message);
        void Error(string message);
        void Info(string message);

    }
    public class Logger : ILogger
    {
        private readonly IErrorLogRepository _errorLogRepository;
        private readonly IMongoRepositoryFactory _mongoRepository;

        public Logger(IMongoRepositoryFactory mongoRepository)
        {
            _mongoRepository = mongoRepository;
            _errorLogRepository = new ErrorLogRepository(_mongoRepository);
        }

        public void Error(string latestString, Exception exception, string message)
        {
            Log(message, latestString, LogType.Error, exception);
        }

        public void Error(Exception exception, string caption)
        {
            Error("", exception, caption);
        }

        public void Error(string message)
        {
            Error("", null, message);
        }

        public void Info(string message)
        {
            Log(message, "", LogType.Info, null);
        }

        private void Log(string message, string latestString, LogType type, Exception exception)
        {
            var error = new LoggerEntity
            {
                Exception = exception?.Message,
                LogString = latestString,
                Message = message,
                Time = $"{DateTime.Now:HH-mm:dd-MM-yyyy}",
                Type = type.ToString()
            };

            _errorLogRepository.Log(error);
        }
    }
}
