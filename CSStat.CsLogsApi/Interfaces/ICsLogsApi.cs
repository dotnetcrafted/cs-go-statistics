using System.Collections.Generic;
using CSStat.CsLogsApi.Models;

namespace CSStat.CsLogsApi.Interfaces
{
    public interface ICsLogsApi
    {
        List<LogModel> GetLogs(string path);

        List<PlayerModel> GetPlayers(List<LogModel> logs);
    }
}