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

            var action = string.Equals(splitLine[2].Trim(), "triggered")
                ? GetAction(splitLine[3].Trim())
                : GetAction(splitLine[2].Trim());
            
            var result = action != Actions.Kill
                ? new LogModel
                {
                    DateTime = GetClearDateTime(splitLine[0].Trim()),
                    PlayerName = GetClearName(splitLine[1].Trim()),
                    PlayerTeam = GetTeam(splitLine[1].Trim()),
                    Action = action,
                    VictimName = string.Empty,
                    VictimTeam = Teams.Null,
                    IsHeadShot = false,
                    Gun = Guns.Null
                }
                : new LogModel
                {
                    DateTime = GetClearDateTime(splitLine[0].Trim()),
                    PlayerName = GetClearName(splitLine[1].Trim()),
                    PlayerTeam = GetTeam(splitLine[1].Trim()),
                    Action = action,
                    VictimName = GetClearName(splitLine[3].Trim()),
                    VictimTeam = GetTeam(splitLine[3].Trim()),
                    IsHeadShot = logLine.Contains("headshot"),
                    Gun = GetGun(splitLine[5].Trim())
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
            return dateTime.Substring(2, dateTime.Length - 3);
        }

        private static Actions GetAction(string action)
        {
            var attributeList = Actions.Unknown.GetAttributeList().Where(x=>!string.IsNullOrEmpty(x.Value));

            return (from attribute in attributeList
                    where action.Contains(attribute.Value)
                    select attribute.Key into actionIndex
                        select (Actions) actionIndex).FirstOrDefault();
        }

        private static Guns GetGun(string gun)
        {
            var attributeList = Guns.Null.GetAttributeList();
            var gunIndex = attributeList.FirstOrDefault(x => x.Value.Contains(gun))?.Key ?? 0;
            return (Guns) gunIndex;
        }

    }

}