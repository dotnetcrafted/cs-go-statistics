using System;

namespace CsStat.SystemFacade.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToShortFormat(this DateTime dateTime)
        {
            return dateTime.ToString("MM/dd/yyyy");
        }
    }
}