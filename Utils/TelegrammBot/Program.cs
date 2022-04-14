using System;

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
