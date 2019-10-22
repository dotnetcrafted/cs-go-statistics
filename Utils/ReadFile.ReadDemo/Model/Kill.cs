namespace ReadFile.ReadDemo.Model
{
    public class Kill
    {
        public Kill(Player killer, Player victim, bool isHeadshot, string weapon, int roundNumber)
        {
            Killer = killer;
            Victim = victim;
            IsHeadshot = isHeadshot;
            Weapon = weapon;
        }

        public Player Killer { get; set; }
        public Player Victim { get; set; }
        public bool IsHeadshot { get; set; }
        public string Weapon { get; set; }
        public int RoundNumber { get; set; }
        
        public Player Assister { get; set; }
    }
}