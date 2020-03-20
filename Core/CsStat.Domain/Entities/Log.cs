using System;
using CsStat.Domain.Definitions;
using CsStat.LogApi.Enums;
using DataService;
using MongoRepository;

namespace CsStat.Domain.Entities
{
    public class Log : Entity, IBaseEntity
    {
        public DateTime DateTime { get; set; }
        public Player Player { get; set; }
        public Teams PlayerTeam { get; set; }
        public Player Victim { get; set; }
        public Teams VictimTeam { get; set; }
        public Actions Action { get; set; }
        public bool IsHeadShot { get; set; }
        public Weapons Gun { get; set; }
    }
}