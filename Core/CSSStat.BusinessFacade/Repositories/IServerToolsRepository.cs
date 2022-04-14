using CsStat.Domain.Entities.ServerTools;

namespace BusinessFacade.Repositories
{
    public interface IServerToolsRepository
    {
        void SaveSettings(ServerToolsSettings settings);
        ServerToolsSettings GetSettings();
    }
}