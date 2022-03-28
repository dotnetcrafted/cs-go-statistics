using System.Collections.Generic;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using TelegramBot.Models;

namespace TelegramBot.Services
{
    public interface IPlayerStatService
    {
        public Task<PlayerStats> GetBestPlayerStat();
    }

    public class PlayerStatService : BaseRequestService, IPlayerStatService
    {
        public async Task<PlayerStats> GetBestPlayerStat()
        {
            var statRequest = await GetAsync<PlayerStats>(Settings.BestPlayerApiUrl);

            return statRequest.Code != HttpStatusCode.OK
                ? new PlayerStats()
                : statRequest.Content;
        }
    }
}