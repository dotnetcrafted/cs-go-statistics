using System;
using System.Xml;
using CsStat.Domain.Definitions;
using CsStat.LogApi.Enums;
using MongoRepository;

namespace CSStat.CsLogsApi.Models
{
    public class LogModel : Entity
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