using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

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

            BombDefuses = new List<int>();
            BombExplosions = new List<int>();
            BombPlants = new List<int>();
        }

        public string Name { get; set; }
        public long SteamID { get; set; }
        [BsonIgnore]
        public string ProfileImageUrl { get; set; }

        public List<KillLog> Kills { get; set; }
        public List<KillLog> Assists { get; set; }

        public List<KillLog> Deaths { get; set; }
        public List<KillLog> Teamkills { get; set; }

        public List<DamageLog> Damage { get; set; }
        public List<DamageLog> UtilityDamage { get; set; }

        public List<int> BombDefuses { get; set; }
        public List<int> BombExplosions { get; set; }
        public List<int> BombPlants { get; set; }
    }
}