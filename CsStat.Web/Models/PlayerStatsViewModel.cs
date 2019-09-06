using System.Collections.Generic;

namespace CsStat.Web.Models
{
    public class PlayerStatsViewModel
    {
        public string Id { get; set; }
        public string SteamId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int Points { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int FriendlyKills { get; set; }
        public int KillsPerGame { get; set; }
        public int AssistsPerGame { get; set; }
        public int DeathsPerGame { get; set; }
        public int DefusedBombs { get; set; }
        public int ExplodedBombs { get; set; }
        public int TotalGames { get; set; }
        public double HeadShot { get; set; }
        public double KdRatio { get; set; }
        public List<AchievementViewModel> Achievements { get; set; }
        public List<GunViewModel> Guns { get; set; }
    }
}