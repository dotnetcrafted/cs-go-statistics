﻿using System.Collections.Generic;
using CsStat.Domain.Models;

namespace CsStat.Web.Models
{
    public class PlayerStatsViewModel
    {
        public string Id { get; set; }
        public string SteamId { get; set; }
        public int Points { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int FriendlyKills { get; set; }
        public double KillsPerGame { get; set; }
        public double AssistsPerGame { get; set; }
        public double DeathsPerGame { get; set; }
        public int DefusedBombs { get; set; }
        public int ExplodedBombs { get; set; }
        public int TotalGames { get; set; }
        public int HeadShot { get; set; }
        public int KdDif => Kills - Deaths;
        public string Kad => $"{Kills}/{Assists}/{Deaths}";
        public double KdRatio { get; set; }
        public List<VictimKillerModel> Victims { get; set; }
        public List<VictimKillerModel> Killers { get; set; }
        public List<AchievementViewModel> Achievements { get; set; }
        public List<WeaponViewModel> Guns { get; set; }
    }
}
