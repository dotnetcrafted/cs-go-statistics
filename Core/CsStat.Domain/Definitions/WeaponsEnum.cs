using System.ComponentModel;
using CsStat.SystemFacade.Attributes;

namespace CsStat.Domain.Definitions
{
    public enum Weapons
    {
        Unknown,

        Null,

        Bomb,

        [LogValue("glock")]
        Glock,

        [LogValue("elite")]
        Elite,

        [LogValue("p250")]
        P250,

        [LogValue("tec9")]
        Tec9,

        [LogValue("cz75a")]
        CZ75,

        [LogValue("deagle")]
        Deagle,

        [LogValue("revolver")]
        Revolver,

        [LogValue("usp_silencer")]
        Usps,

        [LogValue("hkp2000")]
        P2000,

        [LogValue("fiveseven")]
        FiveSeven,

        [LogValue("nova")]
        Nova,

        [LogValue("xm1014")]
        Xml,

        [LogValue("sawedoff")]
        Sawedoff,

        [LogValue("m249")]
        M249,

        [LogValue("negev")]
        Negev,

        [LogValue("mag7")]
        Mag,

        [LogValue("mac10")]
        Mac,

        [LogValue("mp7")]
        Mp7,

        [LogValue("mp5sd")]
        Mp5,

        [LogValue("inferno")]
        Inferno,

        [LogValue("ump45")]
        Ump,

        [LogValue("p90")]
        P90,

        [LogValue("bizon")]
        Bizon,

        [LogValue("mp9")]
        Mp9,

        [LogValue("galilar")]
        Galil,

        [LogValue("ak47")]
        Ak,

        [LogValue("ssg08")]
        Ssg, // sniper

        [LogValue("sg556")]
        Sg,

        [LogValue("awp")]
        Awp, //sniper

        [LogValue("scar20")]
        Scar, //sniper

        [LogValue("scout")]
        Scout, //sniper

        [LogValue("g3sg1")]
        G3sgl, //sniper

        [LogValue("famas")]
        Famas,

        [LogValue("m4a1")]
        M4,

        [LogValue("m4a1_silencer")]
        M4s,

        [LogValue("aug")]
        Aug,

        [LogValue("taser")]
        Taser,

        [LogValue("molotov")]
        Molotov,

        [LogValue("decoy")]
        Decoy,

        [LogValue("flashbang")]
        Flash,

        [LogValue("hegrenade")]
        He,

        [LogValue("smokegrenade")]
        Smoke,

        [LogValue("incgrenade")]
        Inc,

        [LogValue("knife")]
        Knife,

        World
    }
}

