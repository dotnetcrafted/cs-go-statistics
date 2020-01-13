using System.Collections.Generic;
using CsStat.Domain.Definitions;

namespace CsStat.Domain.Entities.Demo
{
    public class RoundLog
    {
        public int RoundNumber { get; set; }

        public Teams Winner { get; set; }
        public string WinnerTitle { get; set; }

        public RoundEndReason Reason { get; set; }
        public string ReasonTitle { get; set; }

        public long? BombPlanter { get; set; }
        public string BombPlanterName { get; set; }

        public long? BombDefuser { get; set; }
        public string BombDefuserName { get; set; }

        public bool IsBombExploded { get; set; }

        public Dictionary<string, List<PlayerLog>> Teams { get; set; }
    }
}