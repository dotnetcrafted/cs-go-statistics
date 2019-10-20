using MongoDB.Bson;

namespace CsStat.Domain.Entities.Demo
{
    public class KillLog
    {
        public KillLog(ObjectId killer, ObjectId victim, bool isHeadshot, string weapon)
        {
            Killer = killer;
            Victim = victim;
            IsHeadshot = isHeadshot;
            Weapon = weapon;
        }

        public ObjectId Killer { get; set; }
        public ObjectId Victim { get; set; }
        public bool IsHeadshot { get; set; }
        public string Weapon { get; set; }

        public ObjectId Assister { get; set; }
    }
}