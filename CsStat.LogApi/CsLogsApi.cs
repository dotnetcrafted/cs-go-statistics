using CSStat.CsLogsApi.Interfaces;
using CSStat.CsLogsApi.Models;
using CsStat.LogApi.Enums;

namespace CsStat.LogApi
{
    public class CsLogsApi : ICsLogsApi
    {
        public LogModel ParseLine(string logLine)
        {
            var splitLine = logLine.Split('"');

            var action = GetAction(splitLine[2]);

            return action == Actions.Assist
                ? new LogModel
                {
                    DateTime = GetClearDateTime(splitLine[0]),
                    PlayerName = GetClearName(splitLine[1]),
                    Action = action,
                    VictimName = string.Empty,
                    IsHeadShot = logLine.Contains("headshot"),
                    Gun = Guns.Null
                }
                : new LogModel
                {
                    DateTime = GetClearDateTime(splitLine[0]),
                    PlayerName = GetClearName(splitLine[1]),
                    Action = action,
                    VictimName = GetClearName(splitLine[3]),
                    IsHeadShot = logLine.Contains("headshot"),
                    Gun = Guns.Ak //TODO: add get gun logic
                };
        }

        private static string GetClearName(string name)
        {
            return name.Split('<')[0];
        }

        private static string GetClearDateTime(string dateTime)
        {
            return dateTime.Substring(2, dateTime.Length - 4);
        }

        private static Actions GetAction(string action)
        {
            if (action.Contains("killed"))
            {
                return Actions.Kill;
            }

            if (action.Contains("assisted"))
            {
                return Actions.Assist;
            }

            //TODO: Friendly kill logic

            return Actions.Unknown;
        }

    }

}