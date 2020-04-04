using System;
using AutoMapper;
using BusinessFacade.Repositories;
using CsStat.Web.Models;
using Microsoft.AspNetCore.Mvc;
using ServerQueries.Source;

namespace CsStat.Web.Controllers
{
    public class HangoutBotController : Controller
    {
        private static IMapper _mapper;
        private static IPlayerRepository _playerRepository;
        private static IQueryConnection _queryConnection;
        public HangoutBotController(IPlayerRepository playerRepository, IQueryConnection queryConnection, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _queryConnection = queryConnection;
            _mapper = mapper;
        }
        // GET
        [ResponseCache(Duration = 600, VaryByQueryKeys = new string[] { "playerName", "intervalAlias" })]
        public JsonResult GetPlayerStat(string playerName = "", string intervalAlias = "")
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                return new JsonResult(null);
            }

            var dateFrom = string.Empty;
            var dateTo = string.Empty;

            if (intervalAlias.ToLower() != "all")
            {
                dateFrom = DateTime.Now.Hour < 12 ? DateTime.Now.AddDays(-1).ToShortDateString() : DateTime.Now.ToShortDateString();
            }

            return new JsonResult(GetStatForOnePlayer(playerName, dateFrom, dateTo));
        }

        public JsonResult GetPlayerList()
        {
            return new JsonResult(_playerRepository.GetAllPlayers());
        }

        private static PlayerStatsViewModel GetStatForOnePlayer(string playerName, string dateFrom, string dateTo)
        {
            var stat = _playerRepository.GetStatsForPlayer(playerName, dateFrom, dateTo);

            return stat == null ? new PlayerStatsViewModel() : _mapper.Map<PlayerStatsViewModel>(stat);
        }
    }
}