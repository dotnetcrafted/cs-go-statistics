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
        public int Total { get; set; }
        public List<WeaponPlayerStat> Players { get; set; }
    }
}