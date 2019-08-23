using System;
using System.Linq;
using CSStat.CsLogsApi.Extensions;
using CSStat.CsLogsApi.Interfaces;
using CSStat.CsLogsApi.Models;
using CsStat.Domain.Definitions;
using CsStat.LogApi.Enums;

namespace CsStat.LogApi
{
    public class CsLogsApi : ICsLogsApi
    {
        public LogModel ParseLine(string logLine)
        {
            var splitLine = logLine.Split('"');

            var action = GetAction(logLine);

            var result = action == Actions.Assist
                ? new LogModel
                {
                    DateTime = GetClearDateTime(splitLine[0]),
                    PlayerName = GetClearName(splitLine[1]),
                    PlayerTeam = GetTeam(splitLine[1]),
                    Action = action,
                    VictimName = string.Empty,
                    VictimTeam = Teams.Null,
                    IsHeadShot = logLine.Contains("headshot"),
                    Gun = Guns.Null
                }
                : new LogModel
                {
                    DateTime = GetClearDateTime(splitLine[0]),
                    PlayerName = GetClearName(splitLine[1]),
                    PlayerTeam = GetTeam(splitLine[1]),
                    Action = action,
                    VictimName = GetClearName(splitLine[3]),
                    VictimTeam = GetTeam(splitLine[3]),
                    IsHeadShot = logLine.Contains("headshot"),
                    Gun = GetGun(splitLine[5])
                };

            if (result.PlayerTeam == result.VictimTeam)
            {
                result.Action = Actions.FriendlyKill;
            }

            return result;
        }

        private static string GetClearName(string name)
        {
            return name.Split('<')[0];
        }

        private static Teams GetTeam(string line)
        {
            return line.Contains("CT")
                ? Teams.Ct
                : Teams.T;
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

            return Actions.Unknown;
        }

        private static Guns GetGun(string gun)
        {
            var attributeList = Guns.Null.GetAttributeList();
            var gunIndex = attributeList.FirstOrDefault(x => x.Value == gun)?.Key ?? 0;
            return (Guns) gunIndex;
        }

    }

}