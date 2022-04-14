using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TelegramBot.Models;

namespace TelegramBot.Services
{
    public interface IPlayerStatService
    {
        public Task<List<PlayerStats>> GetBestPlayerStat();
    }

    public class PlayerStatService : BaseRequestService, IPlayerStatService
    {
        public async Task<List<PlayerStats>> GetBestPlayerStat()
        {
            var statRequest = await GetAsync<List<PlayerStats>>(Settings.BestPlayerApiUrl);

            return statRequest.Code != HttpStatusCode.OK
                ? new List<PlayerStats>()
                : statRequest.Content;
        }
    }
}