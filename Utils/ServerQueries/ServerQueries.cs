using System;
using CsStat.Domain;
using ServerQueries.Models;
using ServerQueries.Source;

namespace ServerQueries
{
    public interface IServerQueries
    {
        ServerInfoModel GetServerInfo();
    }
    public class ServerQueries : IServerQueries
    {
        public ServerInfoModel GetServerInfo()
        {
            var queryConnection = new QueryConnection
            {
                Host = Settings.CsServerIp,
                Port = Settings.CsServerPort
            };

            try
            {
                queryConnection.Connect();
                var info = queryConnection.GetInfo();

                return new ServerInfoModel
                {
                    IsAlive = true,
                    PlayersCount = info.Players,
                    Map = info.Map
                };
            }
            catch (Exception e)
            {
                return new ServerInfoModel
                {
                    IsAlive = false,
                    PlayersCount = 0
                };
            }
        }
    }
}