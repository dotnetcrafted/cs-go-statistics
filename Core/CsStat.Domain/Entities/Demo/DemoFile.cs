using System;
using MongoRepository;

namespace CsStat.Domain.Entities.Demo
{
    public class DemoFile : Entity
    {
        public bool IsSuccessfully { get; set; }
        public string Message { get; set; }
        public string Runner { get; set; }
        public string Path { get; set; }
        public DateTime Created { get; set; }
    }
}