using System;
using MongoRepository.DAL;

namespace CsStat.Domain.Entities
{
    public class LogFile : Entity, IBaseEntity
    {
        public string Path { get; set; }
        public long ReadBytes { get; set; }
        public DateTime Created { get; set; }
    }
}