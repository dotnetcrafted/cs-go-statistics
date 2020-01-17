using System.Collections.Generic;
using DemoInfo;

namespace ReadFile.ReadDemo.Model
{
    public class Round
    {
        public int RoundNumber { get; set; }

        public Team Winner = Team.Spectate;
        public RoundEndReason Reason { get; set; }

        public int TScore { get; set; }
        public int CTScore { get; set; }

        public Player BombPlanter = null;
        public Player BombDefuser = null;

        public bool IsBombExploded { get; set; }
        
        public Dictionary<Team, List<Player>> Teams;

        public List<Squad> Squads;
    }
}
