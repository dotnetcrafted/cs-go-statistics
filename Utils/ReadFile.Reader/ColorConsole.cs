using System;

namespace ReadFile.Reader
{
    public static class ColorConsole
    {
        private static readonly object _lockObject = new object();

        public static void Red(string msg)
        {
            lock (_lockObject)
            {
                var foregroundColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg);

                Console.ForegroundColor = foregroundColor;
            }
        }

        public static void Green(string msg)
        {
            lock (_lockObject)
            {
                var foregroundColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(msg);

                Console.ForegroundColor = foregroundColor;
            }
        }

        public static void Yellow(string msg)
        {
            lock (_lockObject)
            {
                var foregroundColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(msg);

                Console.ForegroundColor = foregroundColor;
            }
        }

        public static void Default(string msg)
        {
            lock (_lockObject)
            {
                var foregroundColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(msg);

                Console.ForegroundColor = foregroundColor;
            }
        }
    }
}