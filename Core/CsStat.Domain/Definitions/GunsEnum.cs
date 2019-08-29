using System.ComponentModel;
using CsStat.SystemFacade.Attributes;

namespace CsStat.Domain.Definitions
{
    public enum Guns
    {
        Unknown,

        Null,

        Bomb,

        [Description("Glock-18")]
        [LogValue("glock")]
        Glock,

        [Description("Dual Berettas")]
        [LogValue("elite")]
        Elite,

        [Description("P250")]
        [LogValue("p250")]
        P250,

        [Description("Tec-9")]
        [LogValue("tec9")]
        Tec9,

        [Description("CZ75-Auto")]
        [LogValue("cz75a")]
        CZ75,

        [Description("Desert Eagle")]
        [LogValue("deagle")]
        Deagle,

        [Description("R8 Revolver")]
        [LogValue("revolver")]
        Revolver,

        [Description("USP-S")]
        [LogValue("usp_silencer")]
        Usps,

        [Description("P2000")]
        [LogValue("hkp2000")]
        P2000,


        [Description("Five-Seven")]
        [LogValue("fiveseven")]
        FiveSeven,

        [Description("Nova")]
        [LogValue("nova")]
        Nova,

        [Description("XM1014")]
        [LogValue("xm1014")]
        Xml,

        [Description("Sawed-Off")]
        [LogValue("sawedoff")]
        Sawedoff,

        [Description("M249")]
        [LogValue("m249")]
        M249,

        [Description("Negev")]
        [LogValue("negev")]
        Negev,

        [Description("MAG-7")]
        [LogValue("mag7")]
        Mag,

        [Description("MAC-10")]
        [LogValue("mac10")]
        Mac,

        [Description("MP7")]
        [LogValue("mp7")]
        Mp7,

        [Description("UMP-45")]
        [LogValue("ump45")]
        Ump,

        [Description("P90")]
        [LogValue("p90")]
        P90,

        [Description("PP-Bizon")]
        [LogValue("bizon")]
        Bizon,

        [Description("MP9")]
        [LogValue("mp9")]
        Mp9,

        [Description("Galil AR")]
        [LogValue("galilar")]
        Galil,

        [Description("AK-47")]
        [LogValue("ak47")]
        Ak,

        [Description("SSG 08")]
        [LogValue("ssg08")]
        Ssg,

        [Description("SG 553")]
        [LogValue("sg556")]
        Sg,

        [Description("AWP")]
        [LogValue("awp")]
        Awp,

        [Description("G3SG1")]
        [LogValue("g3sg1")]
        G3sgl,

        [Description("FAMAS")]
        [LogValue("famas")]
        Famas,

        [Description("M4A4")]
        [LogValue("m4a4")]
        M4,

        [Description("M4A1-S")]
        [LogValue("m4a1_silencer")]
        M4s,

        [Description("AUG")]
        [LogValue("aug")]
        Aug,

        [Description("Zeus x27")]
        [LogValue("taser")]
        Taser,

        [Description("Molotov")]
        [LogValue("molotov")]
        Molotov,

        [Description("Decoy Grenade")]
        [LogValue("decoy")]
        Decoy,

        [Description("Flashbang")]
        [LogValue("flashbang")]
        Flash,

        [Description("High Explosive Grenade")]
        [LogValue("hegrenade")]
        He,

        [Description("Smoke Grenade")]
        [LogValue("smokegrenade")]
        Smoke,

        [Description("Incendiary Grenade")]
        [LogValue("incgrenade")]
        Inc,

        [Description("Knife")]
        [LogValue("knife_t")]
        KnifeT,

        [Description("Knife")]
        [LogValue("knife_ct")]
        KnifeCT



    }
}

