namespace ReadFile.ReadDemo.Model
{
    public class Round
    {
        public int RoundNumber { get; set; }
        public DemoInfo.Team Winner = DemoInfo.Team.Spectate;

        public Player BombPlanter = null;
        public Player BombDefuser = null;


    }
}