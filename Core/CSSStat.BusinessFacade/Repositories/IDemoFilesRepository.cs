using System.Collections.Generic;
using CsStat.Domain.Entities.Demo;

namespace BusinessFacade.Repositories
{
    public interface IDemoFileRepository
    {
        IEnumerable<DemoFile> GetFiles();
        void AddFile(DemoFile file);
    }
}