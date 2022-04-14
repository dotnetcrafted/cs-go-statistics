namespace CsStat.Web.Models
{
    public class WeaponsStatsViewModel
    {
        public int Id { get; set; }
        public int Kills { get; set; }
        public double KillsRatio { get; set; }
        public int HeadShots { get; set; }
        public int HeadShotsRatio { get; set; }
    }
}