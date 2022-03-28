using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TelegramBot.Extensions;
using TelegramBot.Services;

namespace TelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var notificationSender = new NotificationSender();
            notificationSender.Start();

            while(Console.ReadKey().Key != ConsoleKey.Escape) { }
            
            notificationSender.Stop();

            Console.ReadLine();
        }
    }
}
