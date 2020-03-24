namespace CsStat.Web.Models.Matches
{
    public class PlayerStatByRound
    {
        public long SteamId { get; set; }
        public int RoundNumber { get; set; }
        public int Kills { get; set; }
        public int Assists { get; set; }
        public int Death { get; set; }
        public int Damage { get; set; }
        public int UtilityDamage { get; set; }
    }
}