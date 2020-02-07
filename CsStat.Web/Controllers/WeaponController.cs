using System.Linq;
using System.Web.Mvc;
using BusinessFacade.Repositories;
using CsStat.LogApi;
using CsStat.LogApi.Interfaces;

namespace CsStat.Web.Controllers
{
    public class WeaponController : BaseController
    {
        private static IPlayerRepository _playerRepository;
        private static ISteamApi _steamApi;

        public WeaponController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
            _steamApi = new SteamApi();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetWeaponsStat()
        {
            var weaponStats = _playerRepository.GetWeaponStat();
            var ids = weaponStats.SelectMany(x => x.Players.Select(z => z.SteamId))
                .Distinct()
                .ToList();

            var steamIds = string.Join(",", ids.Select(x => x).ToList());
            var avatars = _steamApi.GetAvatarUrlBySteamId(steamIds);

            var players = ids.Select(x => new
            {
                SteamId = x,
                ImagePath = avatars.FirstOrDefault(a => a.Key == x).Value
            }).ToDictionary(x => x.SteamId, z => z.ImagePath);

            return Json(new
            {
                Players = players,
                WeaponStats = weaponStats
            }, JsonRequestBehavior.AllowGet);
        }
    }
}