using System.ComponentModel;

namespace CsStat.Domain.Definitions
{
    // extracted from DemoInfo
    public enum RoundEndReason
    {
        [Description("TargetBombed")]
        TargetBombed = 1,
        [Description("VIPEscaped")]
        VIPEscaped = 2,
        [Description("VIPKilled")]
        VIPKilled = 3,
        [Description("TerroristsEscaped")]
        TerroristsEscaped = 4,
        [Description("CTStoppedEscape")]
        CTStoppedEscape = 5,
        [Description("TerroristsStopped")]
        TerroristsStopped = 6,
        [Description("BombDefused")]
        BombDefused = 7,
        [Description("CTWin")]
        CTWin = 8,
        [Description("TerroristWin")]
        TerroristWin = 9,
        [Description("Draw")]
        Draw = 10, // 0x0000000A
        [Description("HostagesRescued")]
        HostagesRescued = 11, // 0x0000000B
        [Description("TargetSaved")]
        TargetSaved = 12, // 0x0000000C
        [Description("HostagesNotRescued")]
        HostagesNotRescued = 13, // 0x0000000D
        [Description("TerroristsNotEscaped")]
        TerroristsNotEscaped = 14, // 0x0000000E
        [Description("VIPNotEscaped")]
        VIPNotEscaped = 15, // 0x0000000F
        [Description("GameStart")]
        GameStart = 16, // 0x00000010
        [Description("TerroristsSurrender")]
        TerroristsSurrender = 17, // 0x00000011
        [Description("CTSurrender")]
        CTSurrender = 18, // 0x00000012
    }
    
}