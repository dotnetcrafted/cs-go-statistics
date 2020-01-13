using System.Collections.Generic;
using CsStat.Domain.Entities;
using CsStat.Domain.Entities.Demo;
using MongoRepository;

namespace ReadFile.ReadDemo.Model
{
    public class Result : Entity, IBaseEntity
    {
        public string DemoFileName { get; }

        public Dictionary<long, Player> Players { get; set; }
        public Dictionary<int, Round> Rounds { get; set; }

        public Result(string demoFileName)
        {
            DemoFileName = demoFileName;

            Players = new Dictionary<long, Player>();
            Rounds = new Dictionary<int, Round>();
        }
    }
}
