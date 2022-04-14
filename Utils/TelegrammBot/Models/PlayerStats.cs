using System.Collections.Generic;

namespace TelegramBot.Models
{
    public class PlayerStats
    {
        public Player Player { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public double HeadShotsPercent { get; set; }
        public List<Achievement> Achievements { get; set; }
        public double KdRatio { get; set; }
        public object Id { get; set; }
        public string Kad => $"{Kills}/{Assists}/{Deaths}";
    }

    public class Player
    {
        public string NickName { get; set; }
        public string SteamId { get; set; }
        public string ImagePath { get; set; }
        public bool IsRetired { get; set; }
        public int Rang { get; set; }
        public string Id { get; set; }
    }

    public class Achievement
    {
        public string AchievementId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}