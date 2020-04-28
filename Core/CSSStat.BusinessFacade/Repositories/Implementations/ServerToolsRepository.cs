using System.Linq;
using CsStat.Domain;
using CsStat.Domain.Entities.ServerTools;
using DataService.Interfaces;

namespace BusinessFacade.Repositories.Implementations
{
    public class ServerToolsRepository : BaseRepository, IServerToolsRepository
    {
        public void SaveSettings(ServerToolsSettings settings)
        {
            UpdateById(settings);
        }

        public ServerToolsSettings GetSettings()
        {
            return GetAll<ServerToolsSettings>().FirstOrDefault() ?? new ServerToolsSettings();
        }

        public ServerToolsRepository(IMongoRepositoryFactory mongoRepository) : base(mongoRepository)
        {
        }
    }
}