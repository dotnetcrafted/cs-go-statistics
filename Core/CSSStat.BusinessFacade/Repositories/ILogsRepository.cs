using System;
using System.Collections.Generic;
using CsStat.Domain.Entities;

namespace BusinessFacade.Repositories
{
    public interface ILogsRepository
    {
        IEnumerable<Log> GetAllLogs();
        IEnumerable<Log> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo);
        IEnumerable<Log> GetPlayerLogs(Player player);
    }
}