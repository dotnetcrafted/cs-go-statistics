using System;
using System.Configuration;
using System.IO;
using CsStat.SystemFacade.Extensions;

namespace ReadFile.Reader
{
    public static class Settings
    {
        public static string LogsPath => GetSetting(nameof(LogsPath), Defaults.LogsPath);

        public static int FileReadNewLinesInterval => GetSetting(nameof(FileReadNewLinesInterval), Defaults.FileReadNewLinesInterval);
        public static int TimerInterval => GetSetting(nameof(TimerInterval), Defaults.TimerInterval);
        public static int TakeLines => GetSetting(nameof(TakeLines), Defaults.TakeLines);


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

        private static class Defaults
        {
            public static string LogsPath = Path.Combine(Environment.CurrentDirectory, "logs");
            public const int FileReadNewLinesInterval = 10000;
            public const int TimerInterval = 10000;
            public const int TakeLines = 50;
        }
    }
}