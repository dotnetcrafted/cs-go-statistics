﻿using System;
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
    }
}