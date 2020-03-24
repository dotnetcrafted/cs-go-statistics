namespace CsStat.Web.Models.Matches
{
    public class MatchDetailsKill
    {
        public int Id { get; set; }
        public string Killer { get; set; } // MatchDetailsSquadPlayer id
        public string Victim { get; set; } // MatchDetailsSquadPlayer id,
        public string Assister { get; set; } // MatchDetailsSquadPlayer id
        public WeaponViewModel Weapon { get; set; } // "USP-S"
        public bool IsHeadshot { get; set; }
        public bool IsSuicide { get; set; }
        public int Time { get; set; }
        public bool IsPenetrated { get; set; }
    }
}