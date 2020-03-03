using System;
using System.Web.Helpers;
using System.Web.Mvc;
using CsStat.Domain;
using CsStat.SystemFacade.Extensions;
using CsStat.Web.Models;
using ServerQueries.Source;

namespace CsStat.Web.Controllers
{
    public class ServerInfoController : Controller
    {
        private readonly IQueryConnection _queryConnection;

        private readonly string[] _maps =
            {"de_dust2", "de_mirage", "de_inferno", "de_cache", "de_nuke", "de_overpass", "de_train", "de_vertigo"};
        
        public ServerInfoController(IQueryConnection queryConnection)
        {
            _queryConnection = queryConnection;
        }

        public JsonResult ServerInfo()
        {
            return new JsonResult
            {
                Data = GetServerInfo(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult ServerInfoMock(bool? isAlive, string map = "")
        {
            if (isAlive ?? new Random().Next(0, 2) == 1)
            {
                return new JsonResult
                {
                    Data = new ServerInfoModel
                    {
                        IsAlive = true,
                        PlayersCount = new Random().Next(0, 20),
                        Map = map.OrDefault(_maps[new Random().Next(0, 8)]),
                        Image = $"imagePath\\{map.OrDefault(_maps[new Random().Next(0, 8)])}.jpg"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            return new JsonResult
            {
                Data = new ServerInfoModel
                {
                    IsAlive = false,
                    PlayersCount = 0,
                    Map = string.Empty,
                    Image = "server_is_off.jpg"
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private ServerInfoModel GetServerInfo()
        {
            _queryConnection.Host = Settings.CsServerIp;
            _queryConnection.Port = Settings.CsServerPort;

            try
            {
                _queryConnection.Connect();
                var info = _queryConnection.GetInfo();

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
