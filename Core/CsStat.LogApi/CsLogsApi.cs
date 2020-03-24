using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BusinessFacade.Repositories;
using CSStat.CsLogsApi.Extensions;
using CsStat.Domain;
using CsStat.Domain.Definitions;
using CsStat.Domain.Entities;
using CsStat.LogApi.Enums;
using CsStat.LogApi.Interfaces;
using ErrorLogger;

namespace CsStat.LogApi
{
    public class CsLogsApi : ICsLogsApi
    {
        private static IEnumerable<EnumExtensions.AttributeModel> _attributeList;
        private static IPlayerRepository _playerRepository;
        private static ILogger _logger;
        private static readonly string _dateTimeTemplate = "MM/dd/yyyy - HH:mm:ss";
        public CsLogsApi(IPlayerRepository playerRepository, ILogger logger)
        {
            _attributeList = Actions.Unknown.GetAttributeList().Where(x => !string.IsNullOrEmpty(x.Value));
            _playerRepository = playerRepository;
            _logger = logger;
        }
        public List<Log> ParseLogs(List<string> logs)
        {
            var list = new List<Log>();

            foreach (var logLine in logs)
            {
                if (!IsClearString(logLine))
                {
                    continue;
                }

                try
                {
                    var parsed = ParseLine(logLine);

                    if (parsed != null)
                    {
                        list.Add(parsed);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(logLine, ex, "Bad log string format");
                }
            }

            return list;

        }

        private static bool IsClearString(string logLine)
        {
            return !logLine.Contains("_killed") 
                   && !logLine.Contains("GOTV") 
                   && !logLine.Contains("><BOT><")
                   && !logLine.Contains("chickenskilled")
                   && _attributeList.Any(attribute => logLine.Contains(attribute.Value));
        }
        private static Log ParseLine(string logLine)
        {
            var splitLine = logLine.Split('"');

            var action = string.Equals(splitLine[2].Trim(), "triggered")
                ? GetAction(splitLine[3].Trim())
                : GetAction(splitLine[2].Trim());

            var result = new Log();

            switch (action)
            {
                case Actions.Other:
                    result.Action = Actions.Other;
                    break;

                case Actions.Kill:
                    result = new Log
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        Player = GetPlayer(splitLine[1]),
                        PlayerTeam = GetTeam(splitLine[1].Trim()),
                        Action = action,
                        Victim = GetPlayer(splitLine[3]),
                        VictimTeam = GetTeam(splitLine[3].Trim()),
                        IsHeadShot = logLine.Contains("headshot"),
                        Gun = GetGun(splitLine[5].Trim())
                    };

                    break;
                case Actions.TargetBombed:
                    result = new Log
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        Player = null,
                        PlayerTeam = Teams.T,
                        Action = action,
                        Victim = null,
                        VictimTeam = Teams.Null,
                        IsHeadShot = false,
                        Gun = Guns.Null
                    };
                    break;
                case Actions.KilledByBomb:
                    result = new Log
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        Player = null,
                        PlayerTeam = Teams.Null,
                        Action = action,
                        Victim = GetPlayer(splitLine[1]),
                        VictimTeam = GetTeam(splitLine[1].Trim()),
                        IsHeadShot = false,
                        Gun = Guns.Bomb
                    };
                    break;
                default:
                    result = new Log
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        Player = GetPlayer(splitLine[1]),
                        PlayerTeam = GetTeam(splitLine[1].Trim()),
                        Action = action,
                        Victim = null,
                        VictimTeam = Teams.Null,
                        IsHeadShot = false,
                        Gun = Guns.Null
                    };
                    break;
            }

            if (result.Action==Actions.Kill && result.PlayerTeam == result.VictimTeam)
            {
                result.Action = Actions.FriendlyKill;
            }
            else if(result.Action == Actions.Other)

            {
                return null;
            }

            return result;
        }

        private static Player GetPlayer(string playerString)
        {
            var nickName = GetClearName(playerString.Trim());

            var player = _playerRepository.GetPlayerByNickName(nickName);

            if (player != null)
            {
                return player;
            }

            var steamId = GetSteamId(playerString);

            player = new Player
            {
                NickName = nickName,
                SteamId = steamId,

            };

            var playerId = _playerRepository.AddPlayer(player);

            return string.IsNullOrEmpty(playerId)
                ?  null
                : _playerRepository.GetPlayerById(playerId);
        }

        private static string GetClearName(string name)
        {
            return name.Split('<')[0];
        }

        private static string GetSteamId(string steamString)
        {
            var idFromLog = steamString.Split('<')[2].TrimEnd('>');
            var firstId = Settings.FirstSteamId;
            var idNumber = int.Parse(idFromLog.Split(':').Last());
            var universe = int.Parse(idFromLog.Split(':')[1]);
            var steamId = idNumber * 2 + firstId + universe;
            return steamId.ToString();
        }

        private static Teams GetTeam(string line)
        {
            return line.Contains("<CT>")
                ? Teams.Ct
                : Teams.T;
        }

        private static DateTime GetDateTime(string dateTime)
        {
            return dateTime.Length < _dateTimeTemplate.Length
                ? DateTime.MinValue
                : DateTime.TryParseExact(dateTime.Substring(2, 21), _dateTimeTemplate, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var result)
                    ? result
                    : DateTime.MinValue;
        }

        private static Actions GetAction(string action)
        {
            foreach (var attribute in _attributeList)
            {
                if (!action.Contains(attribute.Value)) continue;
                var actionIndex = attribute.Key;
                var actions = (Actions)actionIndex;
                return actions;
            }

            return Actions.Unknown;
        }

        private static Guns GetGun(string gun)
        {
            var attributeList = Guns.Null.GetAttributeList();

            var gunIndex = 0;

            foreach (var attribute in attributeList)
            {
                if (gun.Contains(attribute.Value))
                {
                    gunIndex = attribute.Key;
                }
            }

            return (Guns) gunIndex;
        }

    }

}