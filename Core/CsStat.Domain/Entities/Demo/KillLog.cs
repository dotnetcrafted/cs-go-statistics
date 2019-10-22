namespace CsStat.Domain.Entities.Demo
{
    public class KillLog
    {
        public long? Killer { get; set; }
        public long? Victim { get; set; }
        public bool IsHeadshot { get; set; }
        public string Weapon { get; set; }
        public long? Assister { get; set; }
        public int RoundNumber { get; set; }
    }
}