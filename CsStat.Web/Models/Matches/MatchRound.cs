using System.Collections.Generic;

namespace CsStat.Web.Models.Matches
{
    public class MatchRound
    {
        public int Id { get; set; }
        public int TScore { get; set; } // 2
        public int CTScore { get; set; } // 0
        public int Reason { get; set; } // 1
        public string ReasonTitle { get; set; } // bomb exploded

        public List<MatchDetailsSquad> Squads { get; set; }
        public List<MatchDetailsKill> Kills { get; set; }
    }
}