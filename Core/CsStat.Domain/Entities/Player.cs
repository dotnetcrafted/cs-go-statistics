using MongoRepository.DAL;

namespace CsStat.Domain.Entities
{
    public class Player : Entity, IBaseEntity
    {
        public string NickName { get; set; }
        public string SteamId { get; set; }
        public string ImagePath { get; set; }
        public bool IsRetired { get; set; }
    }
}