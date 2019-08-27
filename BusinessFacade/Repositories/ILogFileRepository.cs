using System.Collections.Generic;

namespace BusinessFacade.Repositories
{
    public interface ILogFileRepository
    {
        IEnumerable<string> GetFiles();
    }

    class LogFileRepository : ILogFileRepository
    {
        public IEnumerable<string> GetFiles()
        {
            throw new System.NotImplementedException();
        }
    }
}