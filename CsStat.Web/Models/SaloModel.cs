using System;
using System.Collections.Generic;

namespace CsStat.Web.Models
{
    public class SaloModel
    {
        public List<PlayerStatsViewModel> Players { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}