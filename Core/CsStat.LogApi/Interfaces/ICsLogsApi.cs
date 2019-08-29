using System.Collections.Generic;
using CsStat.Domain.Entities;

namespace CSStat.CsLogsApi.Interfaces
{
    public interface ICsLogsApi
    {
        Log ParseLine(string logLine);
        List<Log> ParseLogs(string logs);
    }
}