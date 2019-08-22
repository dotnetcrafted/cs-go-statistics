using CsStat.LogApi.Enums;

namespace CSStat.CsLogsApi.Models
{
    public class LogModel
    {
        public string DateTime { get; set; }
        public string PlayerName { get; set; }
        public string VictimName { get; set; }
        public Actions Action { get; set; }
        public bool IsHeadShot { get; set; }
        public Guns Gun { get; set; }
    }
}