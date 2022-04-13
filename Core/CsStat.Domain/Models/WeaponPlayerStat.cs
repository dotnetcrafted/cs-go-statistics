using System;

namespace CsStat.Domain.Models
{
    public class WeaponPlayerStat
    {
        public string NickName { get; set; }
        public string SteamId { get; set; }
        public int Kills { get; set; }
        public int Headshots { get; set; }
        public double Ratio => Kills != 0 ? Math.Round((double) Headshots / Kills, 2) * 100 : 0;
    }
}