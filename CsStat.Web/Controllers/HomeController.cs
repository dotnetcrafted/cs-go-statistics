using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BusinessFacade.Repositories;
using CsStat.LogApi;
using CsStat.LogApi.Interfaces;
using CsStat.Web.Models;
using Newtonsoft.Json;

namespace CsStat.Web.Controllers
{
    public class HomeController : Controller
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
        //[OutputCache(Duration = 600)]
        public ActionResult GetRepository(string dateFrom = "", string dateTo = "")
        {
            if (string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
            {
                dateTo = DateTime.Now.ToUniversalTime().ToString("MM/dd/yyyy");
                dateFrom = DateTime.Now.AddDays(-(int)(DateTime.Now.DayOfWeek - 1)).ToString("MM/dd/yyyy");
            }

            var playersStat = GetPlayers(dateFrom, dateTo)?.OrderByDescending(x => x.KdRatio).ThenByDescending(x => x.Kills).ToList();

            var model = new SaloModel
            {
                Players = playersStat,
                DateFrom = dateFrom,
                DateTo = dateTo

            };

            var json = JsonConvert.SerializeObject(model);

            var result = new JsonResult
            {
                Data = json,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return result;
        }

        private static IEnumerable<PlayerStatsViewModel> GetPlayers(string dateFrom = "", string dateTo = "")
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