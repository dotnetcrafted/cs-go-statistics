using System.Collections.Generic;
using System.Linq;

namespace ReadFile.ReadDemo.Model
{
    public class Result
    {
        public Result()
        {
            Players = new Dictionary<long, Player>();
            Rounds = new Dictionary<int, Round>();
        }

        public Dictionary<long, Player> Players { get; set; }
        public Dictionary<int, Round> Rounds { get; set; }


        public Player MostHeadshots
        {
            get
            {
                return Players.Aggregate((a, b) => a.Value.HeadshotCount > b.Value.HeadshotCount ? a : b).Value;
            }
        }

        public Player MostKills
        {
            get
            {
                return Players.Aggregate((a, b) => a.Value.Kills.Count > b.Value.Kills.Count ? a : b).Value;
            }
        }

        public Player LeastKills
        {
            get
            {
                return Players.Aggregate((a, b) => a.Value.Kills.Count < b.Value.Kills.Count ? a : b).Value;
            }
        }

        public Player HighestFragDeathRatio
        {
            get
            {
                return Players.Aggregate((a, b) => ((float)a.Value.Kills.Count / (float)a.Value.Deaths.Count) > ((float)b.Value.Kills.Count / (float)b.Value.Deaths.Count) ? a : b).Value;
            }
        }

        public Player LeastDeaths
        {
            get
            {
                return Players.Aggregate((a, b) => a.Value.Deaths.Count < b.Value.Deaths.Count ? a : b).Value;
            }
        }

        public Player MostDeaths
        {
            get
            {
                return Players.Aggregate((a, b) => a.Value.Deaths.Count > b.Value.Deaths.Count ? a : b).Value;
            }
        }

        public Player LeastTKs
        {
            get
            {
                return Players.Aggregate((a, b) => a.Value.Teamkills.Count < b.Value.Teamkills.Count ? a : b).Value;
            }
        }

        public Player MostTKs
        {
            get
            {
                return Players.Aggregate((a, b) => a.Value.Teamkills.Count > b.Value.Teamkills.Count ? a : b).Value;
            }
        }

        public Player MostCowardly
        {
            get
            {
                return Players.Aggregate((a, b) => (a.Value.Kills.Count + a.Value.Deaths.Count) < (b.Value.Kills.Count + b.Value.Deaths.Count) ? a : b).Value;
            }
        }

        public Player MostBombPlants
        {
            get
            {
                return Players.Aggregate((a, b) => a.Value.BombPlants.Count > b.Value.BombPlants.Count ? a : b).Value;
            }
        }

        public Player MostBombDefuses
        {
            get
            {
                return Players.Aggregate((a, b) => a.Value.BombDefuses.Count > b.Value.BombDefuses.Count ? a : b).Value;
            }
        }
    }
}