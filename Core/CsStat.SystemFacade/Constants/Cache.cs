using System;

namespace CsStat.SystemFacade.Constants
{
    public struct Cache
    {
        public static DateTime NoAbsoluteExpiration => DateTime.MaxValue;
        public struct DependencyKeys
        {
            public const string AllPlayers = "allPlayers";
            public const string AllWeapons = "allWeapons";
        }
    }

    public struct Session
    {
        public static string ReadBytes = "readBytes";
    }
}