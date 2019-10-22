using System.Collections.Generic;
using DemoInfo;

namespace ReadFile.ReadDemo.Model
{
    public class Round
    {
        public int RoundNumber { get; set; }
        public Team Winner = Team.Spectate;

        public Player BombPlanter = null;
        public Player BombDefuser = null;

        public Dictionary<Team, List<Player>> Teams;
    }
}