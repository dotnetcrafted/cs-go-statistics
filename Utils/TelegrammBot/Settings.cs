using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TelegramBot.Extensions;

namespace TelegramBot
{
    public static class Settings
    {
        public static string BotToken => GetSetting(nameof(BotToken), "");
        public static long ChatId => GetSetting(nameof(ChatId), 0);
        public static Time BeforeGameNotificationTime => GetSetting(nameof(BeforeGameNotificationTime), new Time(0, 0));
        public static Time AfterGameNotificationTime => GetSetting(nameof(AfterGameNotificationTime), new Time(0, 0));
        public static string NotificationsApiUrl => GetSetting(nameof(NotificationsApiUrl), "");
        public static string StickersApiUrl => GetSetting(nameof(StickersApiUrl), "");
        public static long TimerInterval => GetSetting(nameof(TimerInterval), 0);
        public static List<int> ExcludeDays => GetSetting(nameof(ExcludeDays), new List<int>{0,6});
        public static string BestPlayerApiUrl => GetSetting(nameof(BestPlayerApiUrl), "");

        private static string GetSetting(string settingName, string defaultValue = null)
        {
            var settingValue = ConfigurationManager.AppSettings[settingName];

            return settingValue.IsEmpty()
                ? defaultValue
                : settingValue;
        }

        private static Time GetSetting(string settingName, Time defaultValue = null)
        {
            var settingValue = ConfigurationManager.AppSettings[settingName];
            return settingValue.ParseOrDefault(defaultValue);
        }

        private static long GetSetting(string settingName, long defaultValue)
        {
            var settingValue = ConfigurationManager.AppSettings[settingName];
            return settingValue.ParseOrDefault(defaultValue);
        }

        private static List<int> GetSetting(string settingName, List<int> defaultValue)
        {
            var settingValue = ConfigurationManager.AppSettings[settingName];
            return settingValue.IsEmpty() ? defaultValue : settingValue.Split(',').Select(x => x.Trim().ToIntSafety()).ToList();
        }
    }
}