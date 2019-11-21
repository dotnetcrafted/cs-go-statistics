using System;
using CsStat.Domain.Entities;

namespace BusinessFacade.Repositories
{
    public interface IErrorLogRepository
    {
        void Error(Error error);
    }
}