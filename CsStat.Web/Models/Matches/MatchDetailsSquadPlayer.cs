namespace CsStat.Web.Models.Matches
{
    public class MatchDetailsSquadPlayer
    {
        public string Id { get; set; } // steamId
        public string Name { get; set; } // djoony
        public string SteamImage { get; set; } // url to steam profile image

        public int Kills { get; set; }
        public int Assists { get; set; }
        public int Deaths { get; set; }

        public string Kad => $"{Kills}/{Assists}/{Deaths}"; // 10/2/5
        public double KdDiff => Kills - Deaths; // -2
        public double Kd => Deaths > 0 ? (double) Kills / (double) Deaths : 0; // 1.24
        public double Adr { get; set; } // 118 ??
        public double Ud { get; set; } // 24 ??
    }
}