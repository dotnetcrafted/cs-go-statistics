using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using AutoMapper;
using BusinessFacade.Repositories;
using CsStat.Web.Models;

namespace CsStat.Web.Controllers
{
    public class AdminController : Controller
    {
        private IPlayerRepository _playerRepository;
        public AdminController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public ActionResult Index()
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Login");

            var players = Mapper.Map<List<PlayerModelDto>>(_playerRepository.GetAllPlayers());

            return View(players);
        }

        [HttpPost]
        public ActionResult Update(List<PlayerModelDto> model)
        {
            foreach (var player in model)
            {
                _playerRepository.UpdatePlayer(null,player.NickName, player.FirstName, player.SecondName, player.ImagePath);    
            }

            return RedirectToAction("Index");
        }
    }
}