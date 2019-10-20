using CsStat.Domain.Definitions;
using MongoDB.Bson;

namespace CsStat.Domain.Entities.Demo
{
    public class RoundLog
    {
        public int RoundNumber { get; set; }
        public Teams Winner = Teams.Null;

        public ObjectId BombPlanter;
        public ObjectId BombDefuser;
    }
}