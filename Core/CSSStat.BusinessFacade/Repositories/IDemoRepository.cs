using System;
using System.Collections.Generic;
using CsStat.Domain.Entities.Demo;

namespace BusinessFacade.Repositories
{
    public interface IDemoRepository : IBaseRepository
    {
        IEnumerable<DemoLog> GetMatches();
        IEnumerable<DemoLog> GetAllLogs();
        DemoLog GetMatch(string matchId);
        IEnumerable<DemoLog> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo);
    }
}