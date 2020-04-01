using System;
using System.Web.Mvc;
using BusinessFacade;
using CsStat.Domain;
using CsStat.StrapiApi;
using CsStat.SystemFacade.Extensions;
using CsStat.Web.Models;
using ServerQueries.Source;

namespace CsStat.Web.Controllers
{
    public class ServerInfoController : BaseController
    {
        private readonly IQueryConnection _queryConnection;
        private static IStrapiApi _strapiApi;

        private readonly string[] _maps =
            {"de_dust2", "de_mirage", "de_inferno", "de_cache", "de_nuke", "de_overpass", "de_train", "de_vertigo"};
        
        public ServerInfoController(IQueryConnection queryConnection, IStrapiApi strapiApi)
        {
            _queryConnection = queryConnection;
            _strapiApi = strapiApi;
        }

        public JsonResult ServerInfo()
        {
            var serverInfo = GetServerInfo();

            if (serverInfo.IsAlive)
            {
                var mapInfo = _strapiApi.GetMapInfo(serverInfo.Map);
                serverInfo.ImageUrl = mapInfo.Image.FullUrl;
                serverInfo.Description = mapInfo.Description;
            }
            else
            {
                serverInfo.ImageUrl = _strapiApi.GetImage(Constants.ImagesIds.DefaultImage)?.Image.FullUrl;
                serverInfo.Map = "Server is down";
            }

            return Json(serverInfo);
        }

        public JsonResult ServerInfoMock(bool? isAlive, string map = "")
        {
            if (isAlive ?? new Random().Next(0, 2) == 1)
            {
                var mapInfo = _strapiApi.GetMapInfo(map.OrDefault(_maps[new Random().Next(0, 8)]));
                
                return new JsonResult
                {
                    Data = new ServerInfoModel
                    {
                        IsAlive = true,
                        PlayersCount = new Random().Next(0, 20),
                        Map = mapInfo.MapName,
                        ImageUrl = mapInfo.Image?.FullUrl
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            return Json
            (
                new ServerInfoModel
                {
                    IsAlive = false,
                    PlayersCount = 0,
                    Map = string.Empty,
                    ImageUrl = "https://admin.csfuse8.site/uploads/7203cded792e4d2ca72e3b47d248db6c.jpg"
                }
            );
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
