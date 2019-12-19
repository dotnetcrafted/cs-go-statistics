using System;
using System.Collections.Generic;
using CsStat.Domain.Definitions;
using MongoRepository;

namespace CsStat.Domain.Entities
{
    public class UsefulInfo : Entity
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public DateTime PublishDate { get; set; }
        public string Tags { get; set; }
    }
}