using System.ComponentModel;

namespace CsStat.Domain.Definitions
{
    public enum AchievementsEnum
    {
        [Description("MVP")]
        Mvp,

        [Description("First by k/d")]
        First,

        [Description("Second by k/d ")]
        Second,

        [Description("Third by k/d")]
        Third,

        [Description("Killer")]
        Killer,

        [Description("Team player")]
        TeamPLayer,

        [Description("Head Hunter")]
        HeadHunter,

        [Description("Sniper")]
        Sniper,

        [Description("Kenny")]
        Kenny
    }
}