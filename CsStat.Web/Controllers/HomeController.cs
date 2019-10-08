using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using BusinessFacade.Repositories;
using CsStat.LogApi;
using CsStat.LogApi.Interfaces;
using CsStat.SystemFacade.Extensions;
using CsStat.Web.Models;

namespace CsStat.Web.Controllers
{
    public class HomeController : BaseController
    {
        private static IPlayerRepository _playerRepository;
        private static ISteamApi _steamApi;

        public HomeController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
            _steamApi = new SteamApi();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Server)]
        public ActionResult GetRepository(string dateFrom = "", string dateTo = "")
        {
            if (dateFrom.IsEmpty() && dateTo.IsEmpty())
            {
                dateTo = DateTime.Now.ToShortFormat();
                dateFrom = DateTime.Now.AddDays(-(int) (DateTime.Now.DayOfWeek - 1)).ToShortFormat();
            }

            var playersStat = GetPlayersStat(dateFrom, dateTo)
                ?.OrderByDescending(x => x.KdRatio)
                .ThenByDescending(x => x.Kills)
                .ToList();

            return new JsonResult
            {
                Data = new SaloModel
                {
                    Players = playersStat,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static IEnumerable<PlayerStatsViewModel> GetPlayersStat(string dateFrom = "", string dateTo = "")
        {
            var players = _playerRepository.GetStatsForAllPlayers(dateFrom, dateTo).ToList();
            var steamIds = string.Join(",", players.Select(x => x.Player.SteamId).ToList());
            var avatars = _steamApi.GetAvatarUrlBySteamId(steamIds);

            foreach (var player in players)
            {
                player.Player.ImagePath = avatars.FirstOrDefault(x => x.Key == player.Player.SteamId).Value;
            }

            return Mapper.Map<List<PlayerStatsViewModel>>(players);
        }
    }
}
