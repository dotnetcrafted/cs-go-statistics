using System.ComponentModel;
using System.Security.Cryptography;
using CsStat.SystemFacade.Attributes;

namespace CsStat.Domain.Definitions
{
    public enum AchievementsEnum
    {
        [Caption("MVP")]
        [Description("Player with most points")]
        Mvp,

        [Caption("First by k/d")]
        [Description("First player by k/d")]
        First,

        [Caption("Second by k/d ")]
        [Description("Second blayer by k/d")]
        Second,

        [Caption("Third by k/d")]
        [Description("Third player by k/d")]
        Third,

        [Caption("Killer")]
        [Description("Player with most kills")]
        Killer,

        [Caption("Team player")]
        [Description("Player with most assists")]
        TeamPLayer,

        [Caption("Head Hunter")]
        [Description("Player with most head shots")]
        HeadHunter,

        [Caption("Sniper")]
        [Description("Player with most kills with sniper rifles")]
        Sniper,

        [Caption("Kenny")]
        [Description("They Killed Kenny! You Bastards!\nPlayer with most deaths")]
        Kenny
    }
}