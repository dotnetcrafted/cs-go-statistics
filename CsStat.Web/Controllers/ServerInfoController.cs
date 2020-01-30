using System;
using System.Web.Mvc;
using CsStat.Domain;
using CsStat.Web.Models;
using ServerQueries.Source;

namespace CsStat.Web.Controllers
{
    public class ServerInfoController : Controller
    {
        // GET
        private IQueryConnection _queryConnection;
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