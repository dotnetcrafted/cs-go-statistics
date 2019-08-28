using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using DataService;
using DataService.Interfaces;

namespace CSStat.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private static IPlayersRepository _playerRepository;

        public HomeController(IPlayersRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }
        public ActionResult Index()
        {
            var playersStat = _playerRepository.GetStatsForAllPlayers();
            return View(playersStat);
        }
    }
}