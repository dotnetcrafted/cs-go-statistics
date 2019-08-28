using System.Collections.Generic;
using CsStat.Domain.Entities;

namespace CSStat.CsLogsApi.Interfaces
{
    public interface ICsLogsApi
    {
        LogModel ParseLine(string logLine);
        List<LogModel> ParseLogs(string logs);
    }
}