using System.ComponentModel;
using CsStat.SystemFacade.Attributes;

namespace CsStat.LogApi.Enums
{
    public enum Actions
    {
        [Description("Unknown")]
        Unknown,

        [LogValue("killed")]
        [Description("Kill")]
        Kill,

        [LogValue("assisted killing")]
        [Description("Assist")]
        Assist,

        [Description("Friendly kill")]
        FriendlyKill,

        [LogValue("Defused_The_Bomb")]
        [Description("Defuse the bomb")]
        Defuse,

        [LogValue("Planted_The_Bomb")]
        [Description("Explode bomb")]
        Plant,

        [LogValue("SFUI_Notice_Target_Bombed")]
        TargetBombed

    }
}