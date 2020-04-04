using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CsStat.Domain.Entities;

namespace BusinessFacade.Repositories
{
    public interface ILogsRepository
    {
        IEnumerable<Log> GetAllLogs();
        IEnumerable<Log> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo, Expression<Func<Log, bool>> periodDay = null);
        IEnumerable<Log> GetPlayerLogs(Player player, DateTime timeFrom, DateTime timeTo);
    }
}