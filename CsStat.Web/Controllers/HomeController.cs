using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using BusinessFacade.Repositories;
using CsStat.Domain;
using CsStat.StrapiApi;
using CsStat.SystemFacade;
using CsStat.SystemFacade.DummyCache;
using CsStat.SystemFacade.DummyCacheFactories;
using CsStat.SystemFacade.Extensions;
using CsStat.Web.Models;
using static BusinessFacade.Constants;


namespace CsStat.Web.Controllers
{
    public class HomeController : BaseController
    {
        private static IPlayerRepository _playerRepository;
        private static IStrapiApi _strapiApi;
        private static ILogFileRepository _logFileRepository;
        private readonly IDummyCacheManager _statDummyCacheManager;

        public HomeController(IPlayerRepository playerRepository, IStrapiApi strapiApi, ILogFileRepository logFileRepository)
        {
            _playerRepository = playerRepository;
            _strapiApi = strapiApi;
            _statDummyCacheManager = new DummyCacheManager(new StatDummyCacheFactory());
            _logFileRepository = logFileRepository;
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

        [OutputCache(Duration = 1200, Location = OutputCacheLocation.Client, VaryByParam = "dateFrom;dateTo;periodDay")]
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

            _statDummyCacheManager.AddDependency(BuildKey(dateFrom, dateFrom, periodDay));

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

        public ActionResult ClearCache()
        {
            var prevReadBytes = Session[SystemFacade.Constants.Session.ReadBytes] is long ? (long)Session[SystemFacade.Constants.Session.ReadBytes] : 0;
            var logFile = _logFileRepository.GetFileByName(Settings.ConsoleLogsPath);

            if (logFile != null)
            {
                if (prevReadBytes != logFile.ReadBytes)
                {
                    _statDummyCacheManager.CacheCleanByDependency(SystemFacade.Constants.Cache.DependencyKeys.AllPlayers);
                    Session[SystemFacade.Constants.Session.ReadBytes] = logFile.ReadBytes;
                }
            }

            return Json("Ok");
        }

        private static List<PlayerStatsViewModel> GetPlayersStat(string from = "", string to = "", PeriodDay? periodDay = null)
        {
            var players = _playerRepository.GetStatsForAllPlayers(from, to, periodDay).OrderByDescending(x=>x.KdRatio).ToList();
            return Mapper.Map<List<PlayerStatsViewModel>>(players);
        }

        private static string BuildKey(string dateFrom = "", string dateTo = "", PeriodDay? periodDay = null)
        {
            var key = $"{SystemFacade.Constants.Cache.DependencyKeys.AllPlayers}|";
            
            if (dateFrom.IsNotEmpty())
            {
                key += $"dateFrom:{dateFrom}|";
            }

            if (dateTo.IsNotEmpty())
            {
                key += $"dateTo:{dateTo}|";
            }

            if (periodDay != null)
            {
                key += $"periodDay:{periodDay}";
            }

            return key;
        }
    }
}
