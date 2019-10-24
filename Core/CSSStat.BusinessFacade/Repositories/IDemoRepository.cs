using System;
using System.Collections.Generic;
using CsStat.Domain.Entities.Demo;

namespace BusinessFacade.Repositories
{
    public interface IDemoRepository : IBaseRepository
    {
        IEnumerable<DemoLog> GetAllLogs();
        IEnumerable<DemoLog> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo);
    }
}