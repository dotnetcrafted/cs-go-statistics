using System.ComponentModel;

namespace CsStat.LogApi.Enums
{
    public enum Guns
    {
        Null,
        [Description("AK-47")]
        Ak,
        [Description("M-16")]
        M16,
        [Description("USP")]
        Usp,
        [Description("Glock")]
        Glock
    }
}