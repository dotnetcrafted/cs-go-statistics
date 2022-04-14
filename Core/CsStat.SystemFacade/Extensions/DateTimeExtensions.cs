using System;
using System.Globalization;

namespace CsStat.SystemFacade.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToShortFormat(this DateTime dateTime)
        {
            return dateTime.ToString("MM/dd/yyyy");
        }
        public static string ToShortTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToString("t", CultureInfo.InvariantCulture);
        }

        public static DateTime AddDaysExcludeWeekends(this DateTime dateTime, int days)
        {
            var nextDay = dateTime;
            
            for (var i = 0; i < days;)
            {
                nextDay = nextDay.AddDays(1);
                if (nextDay.DayOfWeek != DayOfWeek.Saturday && nextDay.DayOfWeek != DayOfWeek.Sunday)
                {
                    i++;
                }
            }

            return nextDay;
        }
    }
}