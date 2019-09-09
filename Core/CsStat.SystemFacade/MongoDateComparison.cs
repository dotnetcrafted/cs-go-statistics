using System;

namespace CsStat.SystemFacade
{
    public static class MongoDateComparison
    {
        private static readonly int precisionInMilliseconds = 1000;

        public static bool MongoEquals(this DateTime dateTime, DateTime mongoDateTime)
        {
            return Math.Abs((dateTime - mongoDateTime).TotalMilliseconds) < precisionInMilliseconds;
        }
    }
}