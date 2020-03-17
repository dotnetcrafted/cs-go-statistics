using System.Collections.Generic;

namespace CsStat.Web.Models.Matches
{
    public class MatchDetailsSquad
    {
        public string Id { get; set; } // t
        public string Title { get; set; } // Team A
        public List<MatchDetailsSquadPlayer> Players { get; set; }
    }
}