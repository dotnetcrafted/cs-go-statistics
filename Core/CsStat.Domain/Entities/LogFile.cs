using System;
using MongoRepository;

namespace CsStat.Domain.Entities
{
    public class LogFile : Entity
    {
        public long ReadBytes { get; set; }
        public string Path { get; set; }
        public DateTime Created { get; set; }
    }
}