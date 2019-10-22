namespace CsStat.Domain.Definitions
{
    // extracted from DemoInfo
    public enum RoundEndReason
    {
        TargetBombed = 1,
        VIPEscaped = 2,
        VIPKilled = 3,
        TerroristsEscaped = 4,
        CTStoppedEscape = 5,
        TerroristsStopped = 6,
        BombDefused = 7,
        CTWin = 8,
        TerroristWin = 9,
        Draw = 10, // 0x0000000A
        HostagesRescued = 11, // 0x0000000B
        TargetSaved = 12, // 0x0000000C
        HostagesNotRescued = 13, // 0x0000000D
        TerroristsNotEscaped = 14, // 0x0000000E
        VIPNotEscaped = 15, // 0x0000000F
        GameStart = 16, // 0x00000010
        TerroristsSurrender = 17, // 0x00000011
        CTSurrender = 18, // 0x00000012
    }
    
}