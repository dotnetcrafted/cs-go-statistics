using System.Collections.Generic;
using CsStat.Domain.Entities;

namespace CsStat.LogApi.Interfaces
{
    public interface ICsLogsApi
    {
        List<Log> ParseLogs(List<string> logs);
    }
}