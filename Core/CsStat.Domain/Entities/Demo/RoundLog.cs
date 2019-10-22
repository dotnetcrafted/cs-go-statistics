using System.Collections.Generic;
using CsStat.Domain.Definitions;

namespace CsStat.Domain.Entities.Demo
{
    public class RoundLog
    {
        public int RoundNumber { get; set; }
        public Teams Winner { get; set; }

        public long? BombPlanter { get; set; }
        public long? BombDefuser { get; set; }

        public Dictionary<Teams, List<long>> Teams { get; set; }
    }
}