using System;

namespace CsStat.Web.Models.Matches
{
    public class BaseMatch
    {
        public string Id { get; set; }
        public DateTime? Date { get; set; } // ISO
        public string Map { get; set; } // de_inferno
        public string MapImage { get; set; } // soruce from CMS
        public int TScore { get; set; } // 16
        public int CTScore { get; set; } // 12
        public int? Duration { get; set; } // 129 - total in seconds
    }
}