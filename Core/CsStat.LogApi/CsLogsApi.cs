using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CSStat.CsLogsApi.Extensions;
using CsStat.Domain.Definitions;
using CsStat.Domain.Entities;
using CsStat.LogApi.Enums;
using CsStat.LogApi.Interfaces;
using DataService;
using DataService.Interfaces;

namespace CsStat.LogApi
{
    public class CsLogsApi : ICsLogsApi
    {
        private static IEnumerable<EnumExtensions.AttributeModel> _attributeList;
        private static IPlayerRepository _playerRepository;
        private static readonly string _dateTimeTemplate = "MM/dd/yyyy - HH:mm:ss";
        public CsLogsApi()
        {
            var connectionString = new ConnectionStringFactory();
            _attributeList = Actions.Unknown.GetAttributeList().Where(x => !string.IsNullOrEmpty(x.Value));
            _playerRepository = new PlayerRepository(new MongoRepositoryFactory(connectionString));
        }
        public List<Log> ParseLogs(List<string> logs)
        {
            return !logs.Any()
                ? null 
                : (from logLine in logs from attribute in _attributeList where logLine.Contains(attribute.Value) select ParseLine(logLine)).ToList();
        }
        private Log ParseLine(string logLine)
        {
            var splitLine = logLine.Split('"');

            var action = string.Equals(splitLine[2].Trim(), "triggered")
                ? GetAction(splitLine[3].Trim())
                : GetAction(splitLine[2].Trim());

            Log result;

            switch (action)
            {
                case Actions.Kill:
                    result = new Log
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        Player = GetPlayer(GetClearName(splitLine[1].Trim())),
                        PlayerTeam = GetTeam(splitLine[1].Trim()),
                        Action = action,
                        Victim = GetPlayer(GetClearName(splitLine[3].Trim())),
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
                        Victim = GetPlayer(GetClearName(splitLine[1].Trim())),
                        VictimTeam = GetTeam(splitLine[1].Trim()),
                        IsHeadShot = false,
                        Gun = Guns.Bomb
                    };
                    break;
                default:
                    result = new Log
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        Player = GetPlayer(GetClearName(splitLine[1].Trim())),
                        PlayerTeam = GetTeam(splitLine[1].Trim()),
                        Action = action,
                        Victim = null,
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

        private static Player GetPlayer(string nickName)
        {
            var player = _playerRepository.GetPlayerByNickName(nickName);

            if (player != null)
            {
                return player;
            }

            player = new Player
            {
                NickName = nickName
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