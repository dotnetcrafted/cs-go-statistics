using System.Collections.Generic;

namespace CsStat.Web.Models.Matches
{
    public class MatchDetails : BaseMatch
    {
        public List<MatchRound> Rounds { get; set; }
    }
}