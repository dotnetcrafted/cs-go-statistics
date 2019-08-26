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
        FriendlyKill
    }
}