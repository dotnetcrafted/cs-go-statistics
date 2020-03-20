using CsStat.Domain.Definitions;
using DemoInfo;

namespace ReadFile.ReadDemo
{
    public static class EquipmentMapper
    {
        public static Weapons Map(EquipmentElement weapon)
        {
            switch (weapon)
            {
                case EquipmentElement.AK47:
                    return Weapons.Ak;
                case EquipmentElement.AUG:
                    return Weapons.Aug;
                case EquipmentElement.AWP:
                    return Weapons.Awp;
                case EquipmentElement.Bizon:
                    return Weapons.Bizon;
                case EquipmentElement.Bomb:
                    return Weapons.Bomb;
                case EquipmentElement.CZ:
                    return Weapons.CZ75;
                case EquipmentElement.Deagle:
                    return Weapons.Deagle;
                case EquipmentElement.Decoy:
                    return Weapons.Decoy;
                case EquipmentElement.DualBarettas:
                    return Weapons.Elite;
                case EquipmentElement.Famas:
                    return Weapons.Famas;
                case EquipmentElement.FiveSeven:
                    return Weapons.FiveSeven;
                case EquipmentElement.Flash:
                    return Weapons.Flash;
                case EquipmentElement.G3SG1:
                    return Weapons.G3sgl;
                case EquipmentElement.Gallil:
                    return Weapons.Galil;
                case EquipmentElement.Glock:
                    return Weapons.Glock;
                case EquipmentElement.HE:
                    return Weapons.He;
                case EquipmentElement.P2000:
                    return Weapons.P2000;
                case EquipmentElement.Incendiary:
                    return Weapons.Inc;
                case EquipmentElement.Knife:
                    return Weapons.Knife;
                case EquipmentElement.M249:
                    return Weapons.M249;
                case EquipmentElement.M4A4:
                    return Weapons.M4;
                case EquipmentElement.M4A1:
                    return Weapons.M4s;
                case EquipmentElement.Mac10:
                    return Weapons.Mac;
                case EquipmentElement.Swag7:
                    return Weapons.Mag;
                case EquipmentElement.Molotov:
                    return Weapons.Molotov;
                case EquipmentElement.MP5SD:
                    return Weapons.Mp5;
                case EquipmentElement.MP7:
                    return Weapons.Mp7;
                case EquipmentElement.MP9:
                    return Weapons.Mp9;
                case EquipmentElement.Negev:
                    return Weapons.Negev;
                case EquipmentElement.Nova:
                    return Weapons.Nova;
                case EquipmentElement.P250:
                    return Weapons.P250;
                case EquipmentElement.P90:
                    return Weapons.P90;
                case EquipmentElement.Revolver:
                    return Weapons.Revolver;
                case EquipmentElement.SawedOff:
                    return Weapons.Sawedoff;
                case EquipmentElement.Scar20:
                    return Weapons.Scar;
                case EquipmentElement.SG556:
                    return Weapons.Sg;
                case EquipmentElement.Smoke:
                    return Weapons.Smoke;
                case EquipmentElement.Scout:
                    return Weapons.Scout;
                case EquipmentElement.Zeus:
                    return Weapons.Taser;
                case EquipmentElement.Tec9:
                    return Weapons.Tec9;
                case EquipmentElement.UMP:
                    return Weapons.Ump;
                case EquipmentElement.USP:
                    return Weapons.Usps;
                case EquipmentElement.XM1014:
                    return Weapons.Xml;
                case EquipmentElement.World:
                    return Weapons.World;
                default:
                    return Weapons.Unknown;
            }
        }
    }
}