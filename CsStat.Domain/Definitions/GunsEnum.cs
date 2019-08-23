using System.ComponentModel;
using CsStat.SystemFacade.Attributes;

namespace CsStat.Domain.Definitions
{
    public enum Guns
    {
        Null,
        [LogValue("hkp2000")]
        Hkp,

        [LogValue("m4a1_silencer")]
        [Description("Эмка с глушитилем")]
        M4Silencer,

        [LogValue("usp_silencer")]
        [Description("USP с глушителем")]
        UspSilencer,

        [LogValue("m4a1")]
        [Description("Эмка")]
        M4,

        [LogValue("ak47")]
        [Description("Калаш")]
        Ak,

        [LogValue("aug")]
        Aug,

        [LogValue("hegrenade")]
        [Description("Грена")]
        He,

        [LogValue("glock")]
        [Description("Глок")]
        Glock,

        [LogValue("sg556")]
        Sg,

        [LogValue("mag7")]
        Mag,

        [LogValue("galilar")]
        Galilar,

        [LogValue("cz75a")]
        Cz,

        [LogValue("ump45")]
        Ump,

        [LogValue("mp5sd")]
        [Description("Бомжа")]
        Mp5,

        [LogValue("famas")]
        [Description("Фамас")]
        Famas,

        [LogValue("fiveseven")]
        Fiveseven,

        [LogValue("mp7")]
        Mp7,

        [LogValue("bizon")]
        [Description("Бизон")]
        Bizon,

        [LogValue("mp9")]
        Mp9,

        [LogValue("p90")]
        P90,

        [LogValue("awp")]
        [Description("Слон")]
        Awp
    }
}