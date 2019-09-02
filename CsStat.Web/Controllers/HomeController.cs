using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using BusinessFacade.Repositories;
using CsStat.Domain.Entities;
using Newtonsoft.Json;

namespace CSStat.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private static IPlayerRepository _playerRepository;

        public HomeController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;

    }
        public ActionResult Index(string dateFrom, string dateTo)
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetReposutory()
        {

            var playersStat = _playerRepository.GetStatsForAllPlayers().OrderByDescending(x => x.KdRatio);
            var json = JsonConvert.SerializeObject(playersStat);
            var result = new JsonResult
            {
                Data = playersStat,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return result;
        }

        private IEnumerable<PlayerStatsModel> GetStat(DateTime dateFrom, DateTime dateTo)
        {
            return _playerRepository.GetStatsForAllPlayers(dateFrom.ToString(CultureInfo.InvariantCulture), dateTo.ToString(CultureInfo.InvariantCulture));
        }
    }
}