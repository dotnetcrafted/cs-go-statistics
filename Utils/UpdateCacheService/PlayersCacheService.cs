using CsStat.SystemFacade.DummyCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using CsStat.Domain;

namespace UpdateCacheService
{
    public interface IPlayersCacheService
    {
       void ClearPlayersCache();
    }
    public class PlayersCacheService : BaseService, IPlayersCacheService
    {
        public async void ClearPlayersCache()
        { 
            var request = await GetAsync(Settings.ClearPlayerCacheEndpoint);
            
            if (request.Code != HttpStatusCode.OK)
            {
                Logger.Error(request.Content);
            }
        }
    }
}
