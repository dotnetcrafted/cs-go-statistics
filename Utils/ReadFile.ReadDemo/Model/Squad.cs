using System.Collections.Generic;
using DemoInfo;

namespace ReadFile.ReadDemo.Model
{
    public class Squad
    {
        public Squad()
        {
            Players = new List<Player>();
        }

        public Team Team { get; set; }
        public string Title { get; set; }
        public List<Player> Players { get; set; }
    }
}