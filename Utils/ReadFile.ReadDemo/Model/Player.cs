using System.Collections.Generic;

namespace ReadFile.ReadDemo.Model
{
    public class Player
    {
        public Player(string name, long steamId)
        {
            Name = name;
            SteamID = steamId;
            Assists = new List<Kill>();

            Kills = new List<Kill>();
            Assists = new List<Kill>();

            Deaths = new List<Kill>();
            Teamkills = new List<Kill>();

            Damage = new List<Damage>();

            BombPlants = new List<Round>();
            BombExplosions = new List<Round>();
            BombDefuses = new List<Round>();
        }

        public string Name { get; set; }
        public long SteamID { get; set; }

        public List<Kill> Kills { get; set; }
        public List<Kill> Assists { get; set; }

        public List<Kill> Deaths { get; set; }
        public List<Kill> Teamkills { get; set; }

        public List<Damage> Damage { get; set; }
        
        public List<Round> BombPlants = new List<Round>();
        public List<Round> BombExplosions = new List<Round>();
        public List<Round> BombDefuses = new List<Round>();

        public int HeadshotCount
        {
            get
            {
                int hsCount = 0;
                foreach (Kill k in this.Kills)
                {
                    if (k.IsHeadshot)
                        hsCount++;
                }

                return hsCount;
            }
        }
    }
}