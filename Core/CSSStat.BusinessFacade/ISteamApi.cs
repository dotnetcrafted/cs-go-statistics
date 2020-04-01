using System.Collections.Generic;

namespace CsStat.LogApi.Interfaces
{
    public interface ISteamApi
    {
        Dictionary<string, string> GetAvatarUrlBySteamId(string steamId);
    }
}