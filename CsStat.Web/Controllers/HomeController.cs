using System.Linq;
using System.Web.Mvc;
using BusinessFacade.Repositories;
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
        public ActionResult Index()
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
    }
}