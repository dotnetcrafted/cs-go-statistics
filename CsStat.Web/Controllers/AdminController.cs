using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            var player = Mapper.Map<List<PlayerModelDto>>(_playerRepository.GetAllPlayers());
            return View(player);

        }

        [ChildActionOnly]
        public PartialViewResult ModifyPlayerPartial(string nickName)
        {
            return PartialView(Mapper.Map<PlayerModelDto>(_playerRepository.GetPlayerByNickName(nickName)));
        }
    }
}