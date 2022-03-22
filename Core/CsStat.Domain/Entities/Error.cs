using MongoRepository;

namespace CsStat.Domain.Entities
{
    public class LoggerEntity : Entity, IBaseEntity
    {
        public string Message { get; set; }
        public string LogString { get; set; }
        public string Exception { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
    }
}