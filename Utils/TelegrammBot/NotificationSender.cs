using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Telegram.Bot.Types.Enums;
using TelegramBot.Enums;
using TelegramBot.Extensions;
using TelegramBot.Services;
using TelegramBot.Utilities;

namespace TelegramBot
{
    public class NotificationSender
    {
        private Timer _timer;
        private static readonly object _locker = new object();
        private static readonly long _timerInterval = Settings.TimerInterval;
        private static INotificationService _notificationService;
        private static IPlayerStatService _playerStatService;
        private static Bot _botClient;

        public NotificationSender()
        {
            _notificationService = new NotificationsService();
            _botClient = new Bot(Settings.BotToken, Settings.ChatId);
            _playerStatService = new PlayerStatService();
        }

        public async void Start()
        {
            var me = await _botClient.GetInfo();
            LogToConsole($"Bot {me.FirstName} has been initialized.", LogTypes.Info);
            _timer = new Timer(Callback, null, 0, _timerInterval);
        }

        public void Stop()
        {
            _timer.Dispose();
        }

        private void Callback(object state)
        {
            var hasLock = false;
            try
            {
                Monitor.TryEnter(_locker, ref hasLock);
                if (!hasLock)
                {
                    return;
                }

                _timer.Change(Timeout.Infinite, Timeout.Infinite);

                try
                {
                   SendNotification();
                }
                catch (Exception e)
                {
                    LogToConsole(e.Message, LogTypes.Error);
                }
            }
            finally
            {
                if (hasLock)
                {
                    Monitor.Exit(_locker);
                    _timer.Change(_timerInterval, _timerInterval);
                }
            }
        }
        private static async void SendNotification()
        {
            var dateTime = DateTime.Now;

            if (Settings.ExcludeDays.Contains((int) dateTime.DayOfWeek))
            {
                return;
            }

            var nowTime = new Time(dateTime.Hour, dateTime.Minute);
            
            if (nowTime == Settings.BeforeGameNotificationTime)
            {
                var notifications = await _notificationService.GetAllBeforeGameNotifications();
                var stickers = await _notificationService.GetAllStickers();

                var text = notifications.RandomElement().Text;
                await _botClient.SendMessage(text, ParseMode.MarkdownV2);
                await _botClient.SendSticker(stickers.RandomElement().StickerId);

                LogToConsole($"Message: {text} has been sent", LogTypes.Info);
            }

            if (nowTime == Settings.AfterGameNotificationTime)
            {
                var notifications = await _notificationService.GetAllAfterGameNotifications();
                var playerStat = await _playerStatService.GetBestPlayerStat();
                var notification = notifications.FirstOrDefault();
                
                if (notification == null || notification.Text.IsEmpty() || playerStat == null)
                {
                    return;
                }

                var text = HandlebarsEngine.ProcessTemplate(notification.Text, new
                {
                    userName = playerStat.Player.NickName,
                    kdStat = playerStat.KdRatio.ToString(CultureInfo.InvariantCulture).Replace(".", "\\."),
                    kills = playerStat.Kills,
                    assists = playerStat.Assists,
                    headshot = Math.Round(playerStat.HeadShotsPercent, 0),
                    achivements = string.Join(", ", playerStat.Achievements.Select(x => x.Name).ToArray())
                });

                LogToConsole($"Message: {text} has been sent", LogTypes.Info);

                await _botClient.SendMessage(text, ParseMode.MarkdownV2);
            }
        }

        private static void LogToConsole(string text, LogTypes type)
        {
            var log = $"{type.ToString()}: {DateTime.Now} - {text}";
            Console.WriteLine(log);
        }
    }
}