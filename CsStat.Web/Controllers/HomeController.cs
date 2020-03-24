using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessFacade.Repositories;
using CsStat.LogApi;
using CsStat.LogApi.Interfaces;
using CsStat.SystemFacade.Extensions;
using CsStat.Web.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;

namespace CsStat.Web.Controllers
{
    public class HomeController : BaseController
    {
        private static IMapper _mapper;
        private static IPlayerRepository _playerRepository;
        private static ISteamApi _steamApi;

        public HomeController(IPlayerRepository playerRepository, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _steamApi = new SteamApi();
            _mapper = mapper;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ResponseCache(Duration = 600, VaryByQueryKeys = new string[] { "dateFrom", "dateTo" })]
        public ActionResult GetRepository(string dateFrom = "", string dateTo = "")
        {
            if (dateFrom.IsEmpty() && dateTo.IsEmpty())
            {
                var day = DateTime.Now.Hour < 12 ? DateTime.Now.AddDays(-1) : DateTime.Now;

                dateTo = day.ToShortFormat();
                dateFrom = day.ToShortFormat();
            }

            var playersStat = GetPlayersStat(dateFrom, dateTo)
                .Where(x => x.TotalGames != 0)
                .OrderByDescending(x => x.KdRatio)
                .ThenByDescending(x => x.Kills)
                .ThenByDescending(x => x.TotalGames)
                .ToList();


            return new JsonResult(
                new SaloModel
                {
                    Players = playersStat,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                }
            );
        }

        private static List<PlayerStatsViewModel> GetPlayersStat(string dateFrom = "", string dateTo = "")
        {
            var players = _playerRepository.GetStatsForAllPlayers(dateFrom, dateTo).OrderByDescending(x=>x.KdRatio).ToList();
            var steamIds = string.Join(",", players.Select(x => x.Player.SteamId).ToList());
            var avatars = _steamApi.GetAvatarUrlBySteamId(steamIds);

            foreach (var player in players)
            {
                player.Player.ImagePath = avatars.FirstOrDefault(x => x.Key == player.Player.SteamId).Value;
                foreach (var victim in player.Victims)
                {
                    victim.ImagePath = avatars.FirstOrDefault(x => x.Key == victim.SteamId).Value;
                }

                foreach (var killer in player.Killers)
                {
                    killer.ImagePath = avatars.FirstOrDefault(x => x.Key == killer.SteamId).Value;
                }
            }

            return _mapper.Map<List<PlayerStatsViewModel>>(players);
        }


    }
}
