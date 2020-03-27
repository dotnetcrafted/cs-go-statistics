using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using BusinessFacade;
using BusinessFacade.Repositories;
using CsStat.Domain;
using CsStat.LogApi.Interfaces;
using CsStat.StrapiApi;
using CsStat.SystemFacade.Extensions;
using CsStat.Web.Models;
using Newtonsoft.Json;


namespace CsStat.Web.Controllers
{
    public class HomeController : BaseController
    {
        private static IPlayerRepository _playerRepository;
        private static IStrapiApi _strapiApi;

        public HomeController(IPlayerRepository playerRepository, IStrapiApi strapiApi)
        {
            _playerRepository = playerRepository;
            _strapiApi = strapiApi;
        }
        public ActionResult Index()
        {
            var allPlayers = _playerRepository.GetAllPlayers().ToList().Select(player => new PlayerViewModel
            {
                SteamId = player.SteamId,
                NickName = player.NickName,
                SteamImage = player.ImagePath,
                Rang = player.Rang
            });
            var model = new AppStateModel
            {
                ApiPaths = Settings.ToJson(),
                Icons = _strapiApi.GetAllImages()?
                    .Select(icon => new ImageViewModel
                    {
                        Name = icon.CodeName,
                        Image = icon.Image.FullUrl
                    }).ToJson(),
                Weapons = _strapiApi.GetAllWeapons()?
                    .Select(weapon => new WeaponViewModel
                    {
                        Id = weapon.Id,
                        Name = weapon.Name,
                        IconImage = weapon.Icon.FullUrl,
                        PhotoImage = weapon.Image.FullUrl,
                        Type = weapon.Type.Name
                    }).ToJson(false, true),
                Players = allPlayers.ToJson()
            };

            return View(model);
        }

        [HttpGet]
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Server, VaryByParam = "dateFrom;dateTo")]
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


            return Json
            (
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
            return Mapper.Map<List<PlayerStatsViewModel>>(players);
        }


    }
}
