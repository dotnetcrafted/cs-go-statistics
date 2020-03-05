using System;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using BusinessFacade.Repositories;
using CsStat.Domain;
using CsStat.Web.Models;
using Microsoft.Ajax.Utilities;
using ServerQueries.Source;

namespace CsStat.Web.Controllers
{
    public class HangoutBotController : Controller
    {
        private static IPlayerRepository _playerRepository;
        private IQueryConnection _queryConnection;
        public HangoutBotController(IPlayerRepository playerRepository, IQueryConnection queryConnection)
        {
            _playerRepository = playerRepository;
            _queryConnection = queryConnection;
        }
        // GET
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Server, VaryByParam = "playerName;intervalAlias")]
        public JsonResult GetPlayerStat(string playerName = "", string intervalAlias = "")
        {
            if (playerName.IsNullOrWhiteSpace())
            {
                return new JsonResult();
            }

            var dateFrom = string.Empty;
            var dateTo = string.Empty;

            if (intervalAlias.ToLower() != "all")
            {
                dateFrom = DateTime.Now.Hour < 12 ? DateTime.Now.AddDays(-1).ToShortDateString() : DateTime.Now.ToShortDateString();
            }

            return new JsonResult
            {
                Data = GetStatForOnePlayer(playerName, dateFrom, dateTo),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetPlayerList()
        {
            return new JsonResult
            {
                Data = _playerRepository.GetAllPlayers(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static PlayerStatsViewModel GetStatForOnePlayer(string playerName, string dateFrom, string dateTo)
        {
            var stat = _playerRepository.GetStatsForPlayer(playerName, dateFrom, dateTo);

            return stat == null ? new PlayerStatsViewModel() : Mapper.Map<PlayerStatsViewModel>(stat);
        }
    }
}