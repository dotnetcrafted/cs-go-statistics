using CsStat.Domain.Entities;

namespace BusinessFacade.Repositories
{
    public interface IErrorLogRepository
    {
        void Log(LoggerEntity error);
    }
}