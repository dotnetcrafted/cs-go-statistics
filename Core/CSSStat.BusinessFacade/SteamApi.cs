using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using CsStat.Domain;
using CsStat.Domain.Models;
using CsStat.LogApi.Interfaces;
using Newtonsoft.Json;

namespace BusinessFacade
{
    public class SteamApi : ISteamApi
    {
        private string _steamApiPath;
        private string _apiKey;

        public SteamApi()
        {
            _steamApiPath = Settings.PlayersDataSteamApiPath;
            _apiKey = Settings.ApiKey;
        }
        
        public Dictionary<string,string> GetAvatarUrlBySteamId(string steamId)
        {
            var url = new UriBuilder(_steamApiPath);
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString["key"] = _apiKey;
            queryString["steamIds"] = steamId;

            url.Query = queryString.ToString();

            var client = new WebClient();
            client.Headers.Add("Accepts", "application/json");

            Response players;

            try
            {
                players = JsonConvert.DeserializeObject<SteamPlayer>(client.DownloadString(url.ToString())).Data;
            }
            catch (Exception ex)
            {
                return null;
            }

            var result = new Dictionary<string,string>();

            if (players == null)
            {
                return null;
            }

            foreach (var player in players.PlayersInfo)
            {
                result.Add(player.SteamId,player.AvatarFull);
            }

            return result;
        }
    }
}