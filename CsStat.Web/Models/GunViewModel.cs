namespace CsStat.Web.Models
{
    public class WeaponViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Kills { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }
        public string IconUrl { get; set; }
    }
}