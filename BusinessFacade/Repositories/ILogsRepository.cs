using System.Collections.Generic;
using System.Threading.Tasks;
using CSStat.CsLogsApi.Models;

namespace BusinessFacade.Repositories
{
    public interface ILogsRepository
    {
        void InsertLog(LogModel log);
        IEnumerable<LogModel> GetAllLogs();
    }
}