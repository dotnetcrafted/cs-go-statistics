namespace CSStat.CsLogsApi.Models
{
    public class PlayerModel
    {
        public string NickName { get; set; }

        public PlayerStat Stat { get; set; }
    }

    public class PlayerStat
    {
        public int Kills { get; set; }
        public int Death { get; set; }
        public int Assist { get; set; }
    }
}