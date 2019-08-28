using System.Collections.Generic;
using CSStat.CsLogsApi.Models;

namespace CSStat.CsLogsApi.Interfaces
{
    public interface ICsLogsApi
    {
        LogModel ParseLine(string logLine);
        List<LogModel> ParseLogs(string logs);
    }
}