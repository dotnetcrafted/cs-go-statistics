namespace TelegramBot.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotEmpty(this string str)
        {
            return !str.IsEmpty();
        }


        public static long ParseOrDefault(this string value, long defaultValue)
        {
            if (value.IsEmpty())
            {
                return defaultValue;
            }

            return long.TryParse(value, out var parsedValue)
                ? parsedValue
                : defaultValue;
        }

        public static int ParseOrDefault(this string value, int defaultValue)
        {
            if (value.IsEmpty())
            {
                return defaultValue;
            }

            return int.TryParse(value, out var parsedValue)
                ? parsedValue
                : defaultValue;
        }

        public static Time ParseOrDefault(this string value, Time defaultValue)
        {
            if (value.IsEmpty())
            {
                return defaultValue;
            }

            return Time.TryParse(value, out var result)
                ? result
                : defaultValue;
        }

        public static int ToIntSafety(this string value)
        {
            return value.ParseOrDefault(-1);
        }
    }
}