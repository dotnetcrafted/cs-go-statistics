using System.Collections.Generic;

namespace CsStat.Domain.Entities.Demo
{
    public class SquadLog
    {
        public string Team { get; set; }
        public string SquadTitle { get; set; }
        public List<PlayerLog> Players { get; set; }
    }
}