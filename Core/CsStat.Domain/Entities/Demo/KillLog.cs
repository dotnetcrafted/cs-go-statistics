﻿using CsStat.Domain.Definitions;

namespace CsStat.Domain.Entities.Demo
{
    public class KillLog
    {
        public long? Killer { get; set; }
        public string KillerName { get; set; }
        public long? Victim { get; set; }
        public string VictimName { get; set; }
        public bool IsHeadshot { get; set; }
        public Weapons Weapon { get; set; }
        public long? Assister { get; set; }
        public string AssisterName { get; set; }
        public bool IsSuicide { get; set; }
        public int RoundNumber { get; set; }
        public int Time { get; set; }
        public int PenetratedObjects { get; set; }
        public bool IsPenetrated => PenetratedObjects != 0;
        public bool IsFlashed { get; set; }
    }
}