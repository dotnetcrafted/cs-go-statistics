using System.Collections.Generic;
using CsStat.Domain.Entities;

namespace BusinessFacade.Repositories
{
    public interface IUsefulLinkRepository
    {
        void AddInfo(UsefulInfo info);
        bool UpdateInfo(string id);
        bool DeleteInfo(string id);
        IEnumerable<UsefulInfo> GetAll();
        UsefulInfo GetInfo(string id);
    }
}