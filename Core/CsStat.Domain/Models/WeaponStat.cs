using System;
using System.Collections.Generic;

namespace CsStat.Domain.Models
{
    public class WeaponStat
    {
        public WeaponStat()
        {
            Players = new List<WeaponPlayerStat>();
        }

        public string Title { get; set; }
        public int Kills { get; set; }
        public int Headshots { get; set; }
        public double Ratio => Kills != 0 ? Math.Round((double) Headshots / Kills, 2) * 100 : 0;

        public List<WeaponPlayerStat> Players { get; set; }
    }
}