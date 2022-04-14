using System;
using System.Configuration;
using System.IO;
using System.Linq;
using CsStat.SystemFacade.Attributes;
using CsStat.SystemFacade.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CsStat.Domain
{
    public static class Settings
    {
        public static int ShowNullPlayers => GetSetting(nameof(ShowNullPlayers), Defaults.ShowNullPlayers);

        ///<filters>
        /// dateFrom?: string; dateTo?: string
        /// /api/playersdata?dateFrom=03/12/2020&dateTo=03/13/2020
        /// </filters>
        [IncludePropertyToJson]
        public static string PlayersDataApiPath => GetSetting(nameof(PlayersDataApiPath), Defaults.PlayersDataApiPath);
        public static string PlayerStatApiPath => GetSetting(nameof(PlayerStatApiPath), Defaults.PlayerStatApiPath);
        public static string BestPlayerApiPath => GetSetting(nameof(BestPlayerApiPath), Defaults.BestPlayerApiPath);

        public static string LogsPath => GetSetting(nameof(LogsPath), Defaults.LogsPath);
        public static string CsPath => GetSetting(nameof(LogsPath), Defaults.CsPath);
        public static string ConsoleLogsPath => GetSetting(nameof(ConsoleLogsPath), Defaults.ConsoleLogsPath);
        public static string DemosFolderPath => GetSetting(nameof(DemosFolderPath), Defaults.DemosFolderPath);
        public static string FullMatchesDataApiPath => GetSetting(nameof(FullMatchesDataApiPath), Defaults.FullMatchesDataApiPath);

        // /api/matchesdata
        [IncludePropertyToJson]
        public static string MatchesDataApiPath => GetSetting(nameof(MatchesDataApiPath), Defaults.MatchesDataApiPath);

        ///<filters>
        /// matchId: string
        /// /api/matchdata?matchId=5e71c0c858daf2008805cb57
        /// </filters>
        [IncludePropertyToJson]
        public static string MatchDataApiPath => GetSetting(nameof(MatchDataApiPath), Defaults.MatchDataApiPath);
        
        [IncludePropertyToJson]
        public static string WikiDataApiPath => GetSetting(nameof(WikiDataApiPath), Defaults.WikiDataApiPath);

        [IncludePropertyToJson]
        public static string ServerInfoDataApiPath =>GetSetting(nameof(ServerInfoDataApiPath), Defaults.ServerInfoDataApiPath);
        public static string ServerInfoDataMockApiPath =>GetSetting(nameof(ServerInfoDataMockApiPath), Defaults.ServerInfoDataMockApiPath);
        public static string PlayersListApiPath => GetSetting(nameof(PlayersListApiPath), Defaults.PlayersListApiPath);
        public static int FileReadNewLinesInterval =>GetSetting(nameof(FileReadNewLinesInterval), Defaults.FileReadNewLinesInterval);
        public static int TimerInterval => GetSetting(nameof(TimerInterval), Defaults.TimerInterval);
        public static int TakeLines => GetSetting(nameof(TakeLines), Defaults.TakeLines);
        public static string WikiPagePath => GetSetting(nameof(WikiPagePath), Defaults.WikiPagePath);
        public static string MatchesPagePath => GetSetting(nameof(MatchesPagePath), Defaults.MatchesPagePath);
        public static string DemoReaderPagePath => GetSetting(nameof(DemoReaderPagePath), Defaults.DemoReaderPagePath);
        public static string PlayersDataSteamApiPath => GetSetting(nameof(PlayersDataSteamApiPath), Defaults.PlayersDataSteamApiPath);
        public static string ApiKey => GetSetting(nameof(ApiKey), Defaults.ApiKey);
        public static string ArticlesPath => GetSetting(nameof(ArticlesPath), Defaults.ArticlesPath);
        public static string AchievementsPath => GetSetting(nameof(AchievementsPath), Defaults.AchievementsPath);
        public static string MapInfoPath => GetSetting(nameof(MapInfoPath), Defaults.MapInfoPath);
        public static string ImagesPath => GetSetting(nameof(ImagesPath), Defaults.ImagesPath);
        public static string WeaponsPath => GetSetting(nameof(WeaponsPath), Defaults.WeaponsPath);
        public static string CmsAdminPath => GetSetting(nameof(CmsAdminPath), Defaults.CmsAdminPath);
        public static string AdminPath => GetSetting(nameof(AdminPath), Defaults.AdminPath);
        public static long FirstSteamId => GetSetting(nameof(FirstSteamId), Defaults.FirstSteamId);
        public static string CsServerIp => GetSetting(nameof(CsServerIp), Defaults.CsServerIp);
        public static int CsServerPort => GetSetting(nameof(CsServerPort), Defaults.CsServerPort);
        public static int MatchesLimit => GetSetting(nameof(MatchesLimit), Defaults.MatchesLimit);
        public static int RoundsLimit => GetSetting(nameof(RoundsLimit), Defaults.RoundsLimit);
        public static string ClearPlayerCacheEndpoint => GetSetting(nameof(ClearPlayerCacheEndpoint), Defaults.ClearPlayerCacheEndpoint);
        public static string ClearCacheApi => GetSetting(nameof(ClearCacheApi), Defaults.ClearCacheApi);

        ///<filters>
        /// dateFrom?: string; dateTo?: string
        /// api/weaponsdata?dateFrom=04/13/2022&dateTo=04/13/2022
        /// </filters>
        [IncludePropertyToJson]
        public static string WeaponsDataApiPath => GetSetting(nameof(WeaponsDataApiPath), Defaults.WeaponsDataApiPath);
        
        public static string ToJson()
        {
            var settings = typeof(Settings);
            var propertyInfo = settings.GetFilteredProperties();
            
            var model = propertyInfo.Select(info => new SettingsModel
            {
                Name = info.Name.ToCamelCase(),
                Value = info.GetValue(null).ToString()
            }).ToList();

            return JsonConvert.SerializeObject(model);
        }

        private static string GetSetting(string settingName, string defaultValue = null)
        {
            var settingValue = ConfigurationManager.AppSettings[settingName];

            if (string.IsNullOrEmpty(settingValue))
            {
                settingValue = defaultValue;
            }

            return settingValue;
        }

        private static int GetSetting(string settingName, int defaultValue)
        {
            var settingValue = ConfigurationManager.AppSettings[settingName];

            return settingValue.ParseOrDefault(defaultValue);
        }

        private static long GetSetting(string settingName, long defaultValue)
        {
            var settingValue = ConfigurationManager.AppSettings[settingName];

            return settingValue.ParseOrDefault(defaultValue);
        }

        private static class Defaults
        {
            public static string Domain = "https://fuse8csgo.ru";
            public static string LogsPath = Path.Combine(Environment.CurrentDirectory, "logs");
            public static string ConsoleLogsPath = Path.Combine(Environment.CurrentDirectory, "console.log");
            public static string DemosFolderPath = Path.Combine(Environment.CurrentDirectory, "Demos");
            public const int FileReadNewLinesInterval = 10000;
            public const int TimerInterval = 10000;
            public const int TakeLines = 50;
            public const string PlayersDataApiPath = "api/playersdata";
            public const string PlayerStatApiPath = "api/bot/playerstat";
            public const string BestPlayerApiPath = "api/bot/bestplayerstat";
            public const string PlayersListApiPath = "api/bot/playerslist";
            public const string WeaponsDataApiPath = "api/weaponsdata";
            public const string ServerInfoDataMockApiPath = "api/serverinfomock";
            public const string FullMatchesDataApiPath = "api/fullmatchesdata";
            public const string MatchesDataApiPath = "api/matchesdata";
            public const string MatchDataApiPath = "api/matchdata";
            public const string WikiDataApiPath = "api/articles";
            public const string ClearCacheApi = "api/cache/clear";
            public const string ServerInfoDataApiPath = "api/bot/serverinfo";
            public const string WikiPagePath = "wiki";
            public const string DemoReaderPagePath = "demo-reader";
            public const string PlayersDataSteamApiPath =@"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/";
            public const long FirstSteamId = 76561197960265728;
            public const string ApiKey = "C03F2D79CF2FE20B64A85531031F3983";
            public const string AdminPath = "http://csstat.westeurope.cloudapp.azure.com:1337";
            public const string CsServerIp = "192.168.100.241";
            public const int ShowNullPlayers = 1;
            public const int CsServerPort = 27015;
            public const int MatchesLimit = 12;
            public const int RoundsLimit = 5;
            public const string MatchesPagePath = "/matches";
            public const string CsPath = @"D:\Games\CS_GO_DS\";
            public static string ArticlesPath => $"{AdminPath}/articles?_sort=createdAt:desc";
            public static string AchievementsPath => $"{AdminPath}/Achievements";
            public static string MapInfoPath => $"{AdminPath}/map-infos";
            public static string ImagesPath => $"{AdminPath}/images";
            public static string CmsAdminPath => $"{AdminPath}/shell";
            public static string WeaponsPath => $"{AdminPath}/weapons";
            public static string ClearPlayerCacheEndpoint => $"{Domain}/{ClearCacheApi}";

        }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SettingsModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
