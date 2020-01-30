using System.Collections.Generic;

namespace ServerQueries.Common
{
    public static class Constants
    {
        /// <summary>
        /// Retrieves information about the server including, but not limited to: its name, the map currently being played, and the number of players.
        /// </summary>
        public static byte[] A2S_INFO_REQUEST = new byte[]
        {
                0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45, 0x6E, 0x67, 0x69,
                0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00
        };

        /// <summary>
        /// The middle byte of the challenge response.
        /// </summary>
        public static byte CHALLENGE_RESPONE = 0x41;

        /// <summary>
        /// This query retrieves information about the players currently on the server. It needs an initial step to acquire a challenge number.
        /// </summary>
        public static byte[] A2S_PLAYER_CHALLENGE_REQUEST = new byte[] {
            0xFF, 0xFF, 0xFF, 0xFF, 0x55, 0xFF, 0xFF, 0xFF, 0xFF
        };

        /// <summary>
        /// Returns the server rules, or configuration variables in name/value pairs. This query requires an initial challenge step.
        /// </summary>
        public static byte[] A2S_RULES_CHALLENGE_REQUEST = new byte[] {
            0xFF, 0xFF, 0xFF, 0xFF, 0x56, 0xFF, 0xFF, 0xFF, 0xFF
        };

        /// <summary>
        /// Ping the server to see if it exists, this can be used to calculate the latency to the server.
        /// </summary>
        public static byte[] A2A_PING_REQUEST = new byte[] {
            0xFF, 0xFF, 0xFF, 0xFF, 0x69
        };

        /// <summary>
        /// A2S_PLAYER and A2S_RULES queries both require a challenge number. Formerly, this number could be obtained via an A2S_SERVERQUERY_GETCHALLENGE request.
        /// </summary>
        public static byte[] A2S_SERVERQUERY_GETCHALLENGE_REQUEST = new byte[]
        {
            0xFF, 0xFF, 0xFF, 0xFF, 0x57
        };

        /// <summary>
        /// Mapping for Byte to ServerType conversion
        /// </summary>
        public static Dictionary<byte, Enums.ServerType> ByteServerTypeMapping =
            new Dictionary<byte, Enums.ServerType>()
            {
                {0x64, Enums.ServerType.Dedicated},
                {0x6c, Enums.ServerType.NonDedicated},
                {0x70, Enums.ServerType.SourceTvRelay},
                {0x00, Enums.ServerType.NonDedicated} //Rag Doll Kung Fu
            };
        /// <summary>
        /// Mapping for Byte to Environment conversion
        /// </summary>
        public static Dictionary<byte, Enums.Environment> ByteEnvironmentMapping = new Dictionary<byte, Enums.Environment>()
        {
            { 0x6c, Enums.Environment.Linux },
            { 0x77, Enums.Environment.Windows },
            { 0x6d, Enums.Environment.Mac },
            { 0x6f, Enums.Environment.Mac }
        };

        /// <summary>
        /// Mapping for Byte to TheShipMode conversion
        /// </summary>
        public static Dictionary<byte, Enums.TheShipMode> ByteTheShipModeMapping = new Dictionary<byte, Enums.TheShipMode>()
        {
            { 0x00, Enums.TheShipMode.Hunt },
            { 0x01, Enums.TheShipMode.Elimination },
            { 0x02, Enums.TheShipMode.Duel },
            { 0x03, Enums.TheShipMode.Deathmatch },
            { 0x04, Enums.TheShipMode.VipTeam },
            { 0x05, Enums.TheShipMode.TeamElimination }
        };

        public static Dictionary<byte, Enums.Visibility> ByteVisibilityMapping = new Dictionary<byte, Enums.Visibility>()
        {
            { 0x00, Enums.Visibility.Public },
            { 0x01, Enums.Visibility.Private }
        };

        public static uint SimpleResponseHeader = 0xFFFFFFFF;

        public static uint MultiPacketResponseHeader = 0xFFFFFFFE;

        public static short TheShipGameId = 2400;
    }
}