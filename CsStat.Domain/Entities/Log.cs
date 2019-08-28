using System;
using CsStat.Domain.Definitions;
using CsStat.LogApi.Enums;
using MongoRepository;

namespace CsStat.Domain.Entities
{
    public class LogModel : Entity, IBaseEntity
    {
        public DateTime DateTime { get; set; }
        public string PlayerName { get; set; }
        public Teams PlayerTeam { get; set; }
        public string VictimName { get; set; }
        public Teams VictimTeam { get; set; }
        public Actions Action { get; set; }
        public bool IsHeadShot { get; set; }
        public Guns Gun { get; set; }
    }
}