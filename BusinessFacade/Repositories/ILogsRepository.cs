using System;
using System.Collections.Generic;
using CsStat.Domain.Entities;

namespace BusinessFacade.Repositories
{
    public interface ILogsRepository
    {
        IEnumerable<LogModel> GetAllLogs();
        IEnumerable<LogModel> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo);
    }
}