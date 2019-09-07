using MongoRepository;

namespace CsStat.Domain.Entities
{
    public class LogFile : Entity, IBaseEntity
    {
        public string Name { get; set; }
        public long Lenght { get; set; }
    }
}