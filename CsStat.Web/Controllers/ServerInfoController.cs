﻿using System;
using System.Web.Helpers;
using System.Web.Mvc;
using CsStat.Domain;
using CsStat.StrapiApi;
using CsStat.SystemFacade.Extensions;
using CsStat.Web.Models;
using ServerQueries.Source;

namespace CsStat.Web.Controllers
{
    public class ServerInfoController : Controller
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
                        ImageUrl = "https://admin.csfuse8.site/uploads/15989dfab7bd4ee2abd457829e22b987.png"
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
                    ImageUrl = "server_is_off.jpg"
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult ServerInfoWidget()
        {
            var serverInfo = GetServerInfo();
            var mapInfo = _strapiApi.GetMapInfo(serverInfo.Map);
            serverInfo.ImageUrl = $"{Settings.AdminPath}{mapInfo.Image.Url}";
            serverInfo.Description = mapInfo.Description;
            
            return new JsonResult
            {
                Data = serverInfo,
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
