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

            var player = Mapper.Map<List<PlayerModelDto>>(_playerRepository.GetAllPlayers()).Select(x=>x.NickName);
            return View(player);

        }

        public PartialViewResult PlayerList()
        {
            var player = Mapper.Map<List<PlayerModelDto>>(_playerRepository.GetAllPlayers()).Select(x => x.NickName);
            return PartialView(player);
        }
        public PartialViewResult PlayerInfo(string nickName)
        {
            return PartialView();
        }
    }
}