using System;
using System.Collections.Generic;
using MongoRepository;

namespace CsStat.Domain.Entities.Demo
{
    public class DemoLog : Entity, IBaseEntity
    {
        public DemoLog()
        {
            Players = new List<PlayerLog>();
            Rounds = new List<RoundLog>();
        }

        public string Map { get; set; }
        public long Size { get; set; }
        public DateTime? Date { get; set; }

        public string DemoFileName { get; set; }

        public List<PlayerLog> Players { get; set; }
        public List<RoundLog> Rounds { get; set; }
    }
}