using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using BusinessFacade.Repositories;
using CsStat.SystemFacade.DummyCache;
using CsStat.SystemFacade.DummyCacheFactories;
using CsStat.SystemFacade.Extensions;
using CsStat.Web.Models;

namespace CsStat.Web.Controllers
{
    public class WeaponController : BaseController
    {
        private static IPlayerRepository _playerRepository;
        private readonly IDummyCacheManager _dummyCacheManager;

        public WeaponController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
            _dummyCacheManager = new DummyCacheManager(new WeaponsStatDummyCacheFactory());
        }


        [HttpGet]
        [OutputCache(Duration = Constants.OutputCache.Duration, Location = OutputCacheLocation.Server, VaryByParam = "dateFrom;dateTo")]
        public JsonResult GetWeaponsStat(string dateFrom = "", string dateTo = "")
        {
            var weaponStats = Mapper.Map<List<WeaponsStatsViewModel>>(_playerRepository.GetWeaponStat(dateFrom, dateTo));
            var totalKills = weaponStats.Sum(x => x.Kills);

            foreach (var stat in weaponStats)
            {
                stat.KillsRatio = Math.Round((double) stat.Kills / totalKills * 100, 2);
            }

            _dummyCacheManager.AddDependency(BuildKey(dateFrom, dateTo));

            return Json(weaponStats.OrderByDescending(x=>x.Kills));
        }

        private string BuildKey(string dateFrom, string dateTo)
        {
            var key = $"{SystemFacade.Constants.Cache.DependencyKeys.AllWeapons}|";

            if (dateFrom.IsNotEmpty())
            {
                key += $"dateFrom:{dateFrom}|";
            }

            if (dateTo.IsNotEmpty())
            {
                key += $"dateTo:{dateTo}|";
            }
            
            return key;
        }
    }
}