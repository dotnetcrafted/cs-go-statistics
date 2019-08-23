using System.ComponentModel;

namespace CsStat.Domain.Definitions
{
    public enum Teams
    {
        Null,
        [Description("Counter-Terrorists")]
        Ct,
        [Description("Terrorists")]
        T
    }
}