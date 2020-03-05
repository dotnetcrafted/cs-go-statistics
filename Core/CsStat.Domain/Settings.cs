using System;
using System.Configuration;
using System.IO;
using CsStat.SystemFacade.Extensions;

namespace CsStat.Domain
{
    public static class Settings
    {
        public static string LogsPath => GetSetting(nameof(LogsPath), Defaults.LogsPath);
        public static string ConsoleLogsPath => GetSetting(nameof(ConsoleLogsPath), Defaults.ConsoleLogsPath);

        public static int FileReadNewLinesInterval =>
            GetSetting(nameof(FileReadNewLinesInterval), Defaults.FileReadNewLinesInterval);

        public static int TimerInterval => GetSetting(nameof(TimerInterval), Defaults.TimerInterval);
        public static int TakeLines => GetSetting(nameof(TakeLines), Defaults.TakeLines);
        public static string PlayersDataApiPath => GetSetting(nameof(PlayersDataApiPath), Defaults.PlayersDataApiPath);
        public static string PlayerStatApiPath => GetSetting(nameof(PlayerStatApiPath), Defaults.PlayerStatApiPath);
        public static string WikiDataApiPath => GetSetting(nameof(WikiDataApiPath), Defaults.WikiDataApiPath);
        public static string ServerInfoDataApiPath => GetSetting(nameof(ServerInfoDataApiPath), Defaults.ServerInfoDataApiPath);
        public static string ServerInfoDataMockApiPath => GetSetting(nameof(ServerInfoDataMockApiPath), Defaults.ServerInfoDataMockApiPath);
        public static string WikiPagePath => GetSetting(nameof(WikiPagePath), Defaults.WikiPagePath);
        public static string PlayersDataSteamApiPath =>GetSetting(nameof(PlayersDataSteamApiPath), Defaults.PlayersDataSteamApiPath);
        public static long FirstSteamId => GetSetting(nameof(FirstSteamId), Defaults.FirstSteamId);
        public static string ApiKey => GetSetting(nameof(ApiKey), Defaults.ApiKey);
        public static string ArticlesPath => GetSetting(nameof(ArticlesPath), Defaults.ArticlesPath);
        public static string AchievementsPath => GetSetting(nameof(AchievementsPath), Defaults.AchievementsPath);
        public static string MapInfoPath => GetSetting(nameof(MapInfoPath), Defaults.MapInfoPath);
        public static string ImagesPath => GetSetting(nameof(ImagesPath), Defaults.ImagesPath);
        public static string CmsAdminPath => GetSetting(nameof(CmsAdminPath), Defaults.CmsAdminPath);
        public static string AdminPath => GetSetting(nameof(AdminPath), Defaults.AdminPath);
        public static string CsServerIp => GetSetting(nameof(CsServerIp), Defaults.CsServerIp);
        public static string PlayersListApiPath => GetSetting(nameof(PlayersListApiPath), Defaults.PlayersListApiPath);
        public static int ShowNullPlayers => GetSetting(nameof(ShowNullPlayers), Defaults.ShowNullPlayers);
        public static int CsServerPort => GetSetting(nameof(CsServerPort), Defaults.CsServerPort);


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
            public static string LogsPath = Path.Combine(Environment.CurrentDirectory, "logs");
            public static string ConsoleLogsPath = Path.Combine(Environment.CurrentDirectory, "console.log");
            public const int FileReadNewLinesInterval = 10000;
            public const int TimerInterval = 10000;
            public const int TakeLines = 50;
            public const string PlayersDataApiPath = "api/playersdata";
            public const string PlayerStatApiPath = "api/bot/playerstat";
            public const string PlayersListApiPath = "api/bot/playerslist";
            public const string ServerInfoDataMockApiPath = "api/serverinfomock";
            public const string WikiDataApiPath = "api/articles";
            public const string ServerInfoDataApiPath = "api/bot/serverinfo";
            public const string WikiPagePath = "wiki";
            public const string PlayersDataSteamApiPath =@"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/";
            public const long FirstSteamId = 76561197960265728;
            public const string ApiKey = "C03F2D79CF2FE20B64A85531031F3983";
            public const string ArticlesPath = "https://admin.csfuse8.site/articles?_sort=createdAt:desc";
            public const string AchievementsPath = "https://admin.csfuse8.site/Achievements";
            public const string MapInfoPath = "https://admin.csfuse8.site/map-infos";
            public const string ImagesPath = "https://admin.csfuse8.site/images";
            public const string CmsAdminPath = "https://admin.csfuse8.site/shell";
            public const string AdminPath = "https://admin.csfuse8.site";
            public const string CsServerIp = "192.168.100.241";
            public const int ShowNullPlayers = 1;
            public const int CsServerPort = 27015;
        }
    }
}
