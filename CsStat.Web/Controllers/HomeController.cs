﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessFacade;
using BusinessFacade.Repositories;
using CsStat.Domain;
using CsStat.LogApi.Interfaces;
using CsStat.StrapiApi;
using CsStat.SystemFacade.Extensions;
using CsStat.Web.Models;
using MongoDB.Driver;
using static BusinessFacade.Constants;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace CsStat.Web.Controllers
{
    public class HomeController : BaseController
    {
        private static IMapper _mapper;
        private static IPlayerRepository _playerRepository;
        private static IStrapiApi _strapiApi;

        public HomeController(IPlayerRepository playerRepository, IStrapiApi strapiApi, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _strapiApi = strapiApi;
            _mapper = mapper;
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
        [ResponseCache(Duration = 600, VaryByQueryKeys = new string[] { "dateFrom", "dateTo", "periodDay" })]
        public ActionResult GetRepository(string dateFrom = "", string dateTo = "", PeriodDay? periodDay = null)
        {
            if (dateFrom.IsEmpty() && dateTo.IsEmpty())
            {
                var day = DateTime.Now.Hour < 12 ? DateTime.Now.AddDays(-1) : DateTime.Now;

                dateTo = day.ToShortFormat();
                dateFrom = day.ToShortFormat();
            }

            var playersStat = GetPlayersStat(dateFrom, dateTo, periodDay)
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

        private static List<PlayerStatsViewModel> GetPlayersStat(string from = "", string to = "", PeriodDay? periodDay = null)
        {
            var players = _playerRepository.GetStatsForAllPlayers(from, to, periodDay).OrderByDescending(x=>x.KdRatio).ToList();
            return _mapper.Map<List<PlayerStatsViewModel>>(players);
        }
    }
}
