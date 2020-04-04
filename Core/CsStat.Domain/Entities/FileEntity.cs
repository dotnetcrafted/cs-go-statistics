using System;
using MongoRepository.DAL;

namespace CsStat.Domain.Entities
{
    public class FileEntity : Entity
    {
        public string Path { get; set; }
        public DateTime Created { get; set; }
    }
}