using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using CsStat.Domain.Definitions;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;

namespace CsStat.Domain.Entities.Demo
{
    [DataContract]
    public class DemoLog : Entity, IBaseEntity
    {
        public DemoLog()
        {
            Players = new List<PlayerLog>();
            Rounds = new List<RoundLog>();
        }

        [DataMember]
        public string Map { get; set; }
        [DataMember]
        public long Size { get; set; }
        [DataMember]
        public DateTime? MatchDate { get; set; }

        [DataMember]
        public string DemoFileName { get; set; }

        [DataMember]
        public DateTime? ParsedDate { get; set; }

        [DataMember]
        public List<PlayerLog> Players { get; set; }
        [DataMember]
        public List<RoundLog> Rounds { get; set; }

        [BsonIgnore]
        public int TotalRound => Rounds.Count;

        public int TotalSquadAScore { get; set; }
        
        public int TotalSquadBScore { get; set; }

        public float MatchDuration { get; set; }
    }
}