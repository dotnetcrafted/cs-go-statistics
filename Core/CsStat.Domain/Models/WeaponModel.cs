using CsStat.Domain.Definitions;
using Newtonsoft.Json;

namespace CsStat.Domain.Models
{
    public class WeaponModel
    {
        [JsonProperty("WeaponId")]
        public int Id { get; set; }
        public string Name { get; set; }
        public Image Image { get; set; }
        public Image Icon { get; set; }
        
        [JsonProperty("Type")]
        public WeaponType Type { get; set; }
    }
    public class WeaponType
    {
        [JsonProperty("TypeId")]
        public WeaponTypes Type { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

    }
}