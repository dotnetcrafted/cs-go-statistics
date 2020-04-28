using System;
using System.Globalization;

namespace CsStat.SystemFacade.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static string OrDefault(this string value, string defaultValue)
        {
            return value.IsNotEmpty() ? value : defaultValue;
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

        public static long ParseOrDefault(this string value, long defaultValue)
        {
            if (value.IsEmpty())
            {
                return defaultValue;
            }

            return int.TryParse(value, out var parsedValue)
                ? parsedValue
                : defaultValue;
        }

        public static DateTime ToDate(this string str, DateTime defaultValue)
        {
            if (str.IsEmpty())
                return defaultValue;

            return DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                ? date
                : defaultValue;
        }

        public static string ToCamelCase(this string value)
        {
            var firstChar = value[0].ToString().ToLower();
            value = value.Substring(1, value.Length - 1);
            return firstChar + value;
        }

        public static int GetMinutes(this string str)
        {
            if (!str.Contains(":"))
            {
                return 0;
            }
            var digits = str.Split(':');
            return digits[0].ParseOrDefault(0) * 60 + digits[1].ParseOrDefault(0);
        }
    }
}