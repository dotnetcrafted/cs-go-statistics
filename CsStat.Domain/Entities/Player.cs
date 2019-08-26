using DataService;
using MongoRepository;

namespace CSStat.CsLogsApi.Models
{
    public class PlayerModel : Entity
    {
        public string NickName { get; set; }
        public MongoReference<PlayerStat> Stat { get; set; }
    }

    public class PlayerStat : Entity
    {
        public int Kills { get; set; }
        public int Death { get; set; }
        public int Assist { get; set; }
    }
}