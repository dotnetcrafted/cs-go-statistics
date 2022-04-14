using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CSStat.CsLogsApi.Extensions;
using CsStat.Domain;
using CsStat.Domain.Definitions;
using CsStat.Domain.Entities;
using CsStat.LogApi.Enums;
using CsStat.LogApi.Interfaces;
using DataService;
using ErrorLogger;

namespace CsStat.LogApi
{
    public class CsLogsApi : ICsLogsApi
    {
        private static IEnumerable<EnumExtensions.AttributeModel> _actions;
        private static IPlayerRepository _playerRepository;
        private static ILogger _logger;
        private static readonly string _dateTimeTemplate = "MM/dd/yyyy - HH:mm:ss";
        public CsLogsApi()
        {
            var connectionString = new ConnectionStringFactory();
            var mongoRepository = new MongoRepositoryFactory(connectionString);
            var strapi = new StrapiApi.StrapiApi();
            _actions = Actions.Unknown.GetAttributeList().Where(x => !string.IsNullOrEmpty(x.Value));
            _playerRepository = new PlayerRepository(mongoRepository, strapi);
            _logger = new Logger(mongoRepository);
        }
        public List<Log> ParseLogs(List<string> logs)
        {
            var list = new List<Log>();

            foreach (var logLine in logs.Where(IsClearString))
            {
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
                   && _actions.Any(attribute => logLine.Contains(attribute.Value));
        }
        private static Log ParseLine(string logLine)
        {
            var splitLine = logLine.Split('"');

            var action = string.Equals(splitLine[2].Trim(), "triggered")
                ? Actions.Unknown.GetEnumByAttribute(splitLine[3].Trim())
                : Actions.Unknown.GetEnumByAttribute(splitLine[2].Trim());

            switch (action)
            {
                case Actions.Other:
                    return null;

                case Actions.Kill:
                    var playerTeam = GetTeam(splitLine[1].Trim());
                    var victimTeam = GetTeam(splitLine[3].Trim());
                    return new Log
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        Player = GetPlayer(splitLine[1]),
                        PlayerTeam = playerTeam,
                        Action = playerTeam == victimTeam ? Actions.FriendlyKill : Actions.Kill,
                        Victim = GetPlayer(splitLine[3]),
                        VictimTeam = victimTeam,
                        IsHeadShot = logLine.Contains("headshot"),
                        Gun = Weapons.Unknown.GetEnumByAttribute(splitLine[5].Trim())
                    };

                case Actions.TargetBombed:
                   return new Log
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        Player = null,
                        PlayerTeam = Teams.T,
                        Action = action,
                        Victim = null,
                        VictimTeam = Teams.Null,
                        IsHeadShot = false,
                        Gun = Weapons.Null
                    };

                case Actions.KilledByBomb:
                    return new Log
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        Player = null,
                        PlayerTeam = Teams.Null,
                        Action = action,
                        Victim = GetPlayer(splitLine[1]),
                        VictimTeam = GetTeam(splitLine[1].Trim()),
                        IsHeadShot = false,
                        Gun = Weapons.Bomb
                    };

                default:
                    return new Log
                    {
                        DateTime = GetDateTime(splitLine[0].Trim()),
                        Player = GetPlayer(splitLine[1]),
                        PlayerTeam = GetTeam(splitLine[1].Trim()),
                        Action = action,
                        Victim = null,
                        VictimTeam = Teams.Null,
                        IsHeadShot = false,
                        Gun = Weapons.Null
                    };
            }
        }

        private static Player GetPlayer(string playerString)
        {
            var nickName = GetClearName(playerString.Trim());

            var player = _playerRepository.GetPlayerByNickName(nickName);

            if (player != null)
            {
                return player;
            }

            player = new Player
            {
                NickName = nickName,
                SteamId = GetSteamId(playerString)

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
    }

}
