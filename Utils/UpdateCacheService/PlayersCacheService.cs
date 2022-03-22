using System.Net;
using System.Threading.Tasks;
using CsStat.Domain;
using CsStat.Domain.Entities;

namespace UpdateCacheService
{
    public interface IPlayersCacheService
    {
        Task ClearPlayersCache();
    }
    public class PlayersCacheService : BaseService, IPlayersCacheService
    {
        public async Task ClearPlayersCache()
        {
            var request = await GetAsync(Settings.ClearPlayerCacheEndpoint);

            if (request.Code != HttpStatusCode.OK)
            {
                Logger.Error($"Clear cache error. Request content: {request.Content}");
            }
        }
    }
}
