namespace CsStat.Web.Models.Matches
{
    public class MatchDetailsKill
    {
        public int Id { get; set; }
        public string FormattedTime { get; set; } // 0:28
        public long Killer { get; set; } // MatchDetailsSquadPlayer id
        public long? Victim { get; set; } // MatchDetailsSquadPlayer id,
        public long? Assister { get; set; } // MatchDetailsSquadPlayer id
        public string Weapon { get; set; } // "USP-S"
        public bool IsHeadshot { get; set; }
        public bool IsSuicide { get; set; }
        public int Time { get; set; }
        public bool IsPenetrated { get; set; }
    }
}