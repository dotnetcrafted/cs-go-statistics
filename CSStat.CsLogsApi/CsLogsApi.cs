using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSStat.CsLogsApi.Extensions;
using CSStat.CsLogsApi.Interfaces;
using CSStat.CsLogsApi.Models;

namespace CSStat.CsLogsApi
{
    public class CsLogsApi : ICsLogsApi
    {
        private const string ExamplePath = @"D:\Logs\CS\qconsole.log";
        private string _path;
        private List<LogModel> _logs;

        public CsLogsApi(string path)
        {
            _path = path;

            if (string.IsNullOrEmpty(_path))
            {
                _path = ExamplePath;
            }

            _logs = GetLogs(_path);
        }

        private static List<LogModel> GetLogs(string path)
        {
            List<string> stringLogs;

            using (var streamReader = new StreamReader(path))
            {
                stringLogs = streamReader.ReadToEnd().Split('\n').Skip(4).ToList().Filter("Server cvar");
            }

            var result = new List<LogModel>();

            foreach (var log in stringLogs)
            {
                if (log.ToCharArray().Count(x => x == ':') < 4)
                {
                    continue;
                }

                var dateAndTime = string.Empty;
                var count = 0;

                foreach (var symbol in log)
                {
                    dateAndTime += symbol;

                    if (symbol == ':')
                    {
                        count++;
                    }

                    if (count < 3) continue;
                    break;
                }

                dateAndTime = dateAndTime.Substring(2, dateAndTime.Length - 3);

                result.Add(new LogModel
                {
                    Date = dateAndTime.Split('-')[0].Trim(),
                    Time = dateAndTime.Split('-')[1].Trim(),
                    Message = log.Substring(dateAndTime.Length + 3, log.Length - dateAndTime.Length - 3)
                });
            }

            return result;
        }

        public List<PlayerModel> GetPlayers()
        {
            var logsWithPlayers = _logs.Where(x => x.Message.Contains("entered"));

            var players = logsWithPlayers.Select(log => new PlayerModel {NickName = GetNickName(log.Message)}).ToList().DistinctBy(x=>x.NickName).ToList();

            if (!players.Any())
            {
                return null;
            }

            foreach (var player in players)
            {
                player.Stat = GetPlayerStat(player.NickName);
            }

            return players;
        }

        private string GetNickName(string logString)
        {
            return logString.Substring(2, Array.IndexOf(logString.ToCharArray(), '<') - 2);
        }

        private PlayerStat GetPlayerStat(string nickName)
        {
            var logsForPlayer = _logs.Where(x => x.Message.Contains(nickName)).ToList().Where(x=>x.Message.Contains("killed")).ToList();

            var stat = new PlayerStat();

            foreach (var log in logsForPlayer)
            {
                if (GetNickName(log.Message) == nickName)
                {
                    stat.Kills++;
                }
                else
                {
                    stat.Death++;
                }
            }

            return stat;
        }
    }
}