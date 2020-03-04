namespace CsStat.Web.Models
{
    public class ServerInfoModel
    {
        public bool IsAlive { get; set; }
        public int PlayersCount { get; set; }
        public string Map { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }
}