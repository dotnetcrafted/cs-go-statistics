using System.Collections.Generic;

namespace CsStat.Domain.Entities.Demo
{
    public class PlayerLog
    {
        public PlayerLog()
        {
            Assists = new List<KillLog>();

            Kills = new List<KillLog>();
            Assists = new List<KillLog>();

            Deaths = new List<KillLog>();
            Teamkills = new List<KillLog>();

            BombPlants = new List<RoundLog>();
            BombExplosions = new List<RoundLog>();
            BombDefuses = new List<RoundLog>();
        }

        public string Name { get; set; }
        public long SteamID { get; set; }

        public List<KillLog> Kills { get; set; }
        public List<KillLog> Assists { get; set; }

        public List<KillLog> Deaths { get; set; }
        public List<KillLog> Teamkills { get; set; }

        public List<RoundLog> BombPlants = new List<RoundLog>();
        public List<RoundLog> BombExplosions = new List<RoundLog>();
        public List<RoundLog> BombDefuses = new List<RoundLog>();
    }
}