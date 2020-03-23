﻿namespace ReadFile.ReadDemo.Model
{
    public class Kill
    {
        public Kill(Player killer, Player victim, bool isHeadshot, string weapon, int roundNumber,
            bool isSuicide, float killTime, int penetratedObjects)
        {
            Killer = killer;
            Victim = victim;
            IsHeadshot = isHeadshot;
            Weapon = weapon;
            RoundNumber = roundNumber;
            IsSuicide = isSuicide;
            KillTime = killTime;
            PenetratedObjects = penetratedObjects;
        }

        public Player Killer { get; set; }
        public Player Victim { get; set; }
        public bool IsHeadshot { get; set; }
        public string Weapon { get; set; }
        public int RoundNumber { get; set; }

        public float KillTime { get; set; }

        public bool IsSuicide { get; set; }

        public Player Assister { get; set; }

        public int PenetratedObjects { get; set; }
    }
}