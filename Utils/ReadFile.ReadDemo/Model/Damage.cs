using System.Collections.Generic;
using CsStat.Domain.Definitions;

namespace ReadFile.ReadDemo.Model
{
    public class Damage
    {
        private static readonly List<Weapons> _weapons = new List<Weapons>
        {
            Weapons.Decoy,
            Weapons.Flash,
            Weapons.He,
            Weapons.Inferno,
            Weapons.Inc,
            Weapons.Molotov,
            Weapons.Smoke
        };

        public int RoundNumber { get; set; }
        public long SteamId { get; set; }
        public int HealthDamage { get; set; }
        public int ArmorDamage { get; set; }
        public Weapons Weapon { get; set; }
        public bool IsNadeDamage => _weapons.Contains(Weapon);
    }
}