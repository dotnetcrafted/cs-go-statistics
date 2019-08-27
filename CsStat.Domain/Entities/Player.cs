﻿using System;
using System.Runtime.Remoting.Messaging;
using CsStat.Domain.Definitions;
using MongoRepository;

namespace CsStat.Domain.Entities
{
    public class PlayerModel : Entity
    {
        public string PlayerName { get; set; }
        public int Kills { get; set; }
        public int Death { get; set; }
        public int Assists { get; set; }
        public double HeadShot { get; set; }
        public int TotalGames { get; set; }
        public int Defuse { get; set; }
        public Guns FavoriteGun { get; set; }
        public double KdRatio
        {
            get
            {
                if (Death > 0)
                {
                    return Math.Round((double)Kills / Death,2);
                }

                return Kills;
            }
        }
        public double KillsPerGame
        {
            get
            {
                if (TotalGames == 0)
                {
                    return 0;
                }

                return Math.Round((double)Kills / TotalGames,2);
            }
        }
        public double AssistsPerGame
        {
            get
            {
                if (TotalGames == 0)
                {
                    return 0;
                }

                return Math.Round((double)Assists / TotalGames,2);
            }
        }
        public double DeathPerGame
        {
            get
            {
                if (TotalGames == 0)
                {
                    return 0;
                }

                return Math.Round((double)Death / TotalGames,2);
            }
        }
    }
}