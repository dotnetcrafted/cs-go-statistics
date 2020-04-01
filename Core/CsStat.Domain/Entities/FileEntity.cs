using System;
using MongoRepository;

namespace CsStat.Domain.Entities
{
    public class FileEntity : Entity
    {
        public string Path { get; set; }
        public DateTime Created { get; set; }
    }
}