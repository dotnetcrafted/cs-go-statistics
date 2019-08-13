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
        private const string Path = @"D:\Logs\CS\qconsole.log";

        public List<LogModel> GetLogs()
        {
            return GetLogs(Path);
        }

        public List<LogModel> GetLogs(string path)
        {
            List<string> logs;

            using (var streamReader = new StreamReader(path))
            {
                logs = streamReader.ReadToEnd().Split('\n').Skip(4).ToList().Filter("Server cvar");
            }

            var result = new List<LogModel>();

            foreach (var log in logs)
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

        public List<PlayerModel> GetPlayers(List<LogModel> logs)
        {
            var logsWithPlayers = logs.Where(x => x.Message.Contains("entered"));

            var players = logsWithPlayers.Select(logsWithPlayer => new PlayerModel
            {
                Death = 0,
                Kills = 0,
                NickName = logsWithPlayer.Message.Substring(2, Array.IndexOf(logsWithPlayer.Message.ToCharArray(), '<') - 2)
            }).ToList();

            return players.DistinctBy(x => x.NickName).ToList();
        }
    }
}
}