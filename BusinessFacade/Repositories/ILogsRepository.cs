using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSStat.CsLogsApi.Models;

namespace BusinessFacade.Repositories
{
    public interface ILogsRepository
    {
        void InsertLog(LogModel log);
        void InsertBatch(List<LogModel> logs);
        IEnumerable<LogModel> GetAllLogs();
        IEnumerable<LogModel> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo);
    }
}