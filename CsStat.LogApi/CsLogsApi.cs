using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
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
        private static IEnumerable<EnumExtensions.AttributeModel> _attributeList;
        public CsLogsApi()
        {
            _attributeList = Actions.Unknown.GetAttributeList().Where(x => !string.IsNullOrEmpty(x.Value));
        }
        public List<LogModel> ParseLogs(string logs)
        {
            return string.IsNullOrWhiteSpace(logs) 
                ? null 
                : (from logLine in logs.Split('\n') from attribute in _attributeList where logLine.Contains(attribute.Value) select ParseLine(logLine)).ToList();
        }
        public LogModel ParseLine(string logLine)
        {
            var splitLine = logLine.Split('"');

            var action = string.Equals(splitLine[2].Trim(), "triggered")
                ? GetAction(splitLine[3].Trim())
                : GetAction(splitLine[2].Trim());

            LogModel result;

            switch (action)
            {
                case Actions.Kill:
                    result = new LogModel
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        PlayerName = GetClearName(splitLine[1].Trim()),
                        PlayerTeam = GetTeam(splitLine[1].Trim()),
                        Action = action,
                        VictimName = GetClearName(splitLine[3].Trim()),
                        VictimTeam = GetTeam(splitLine[3].Trim()),
                        IsHeadShot = logLine.Contains("headshot"),
                        Gun = GetGun(splitLine[5].Trim())
                    };
                    break;
                case Actions.TargetBombed:
                    result = new LogModel
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        PlayerName = string.Empty,
                        PlayerTeam = Teams.T,
                        Action = action,
                        VictimName = string.Empty,
                        VictimTeam = Teams.Null,
                        IsHeadShot = false,
                        Gun = Guns.Null
                    };
                    break;
                case Actions.KilledByBomb:
                    result = new LogModel
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        PlayerName = string.Empty,
                        PlayerTeam = Teams.Null,
                        Action = action,
                        VictimName = GetClearName(splitLine[1].Trim()),
                        VictimTeam = GetTeam(splitLine[1].Trim()),
                        IsHeadShot = false,
                        Gun = Guns.Bomb
                    };
                    break;
                default:
                    result = new LogModel
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        PlayerName = GetClearName(splitLine[1].Trim()),
                        PlayerTeam = GetTeam(splitLine[1].Trim()),
                        Action = action,
                        VictimName = string.Empty,
                        VictimTeam = Teams.Null,
                        IsHeadShot = false,
                        Gun = Guns.Null
                    };
                    break;
            }

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

        private static DateTime GetDateTime(string dateTime)
        {
            return DateTime.ParseExact(dateTime.Substring(2, 21), "MM/dd/yyyy - HH:mm:ss", CultureInfo.InvariantCulture);
        }

        private static Actions GetAction(string action)
        {
            return (from attribute in _attributeList
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