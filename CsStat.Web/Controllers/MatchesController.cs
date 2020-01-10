using System.Web.Mvc;
using BusinessFacade.Repositories;
using CsStat.LogApi;
using CsStat.LogApi.Interfaces;

namespace CsStat.Web.Controllers
{
    public class MatchesController : BaseController
    {
        private static IPlayerRepository _playerRepository;
        private static IDemoRepository _demoRepository;
        private static ISteamApi _steamApi;

        public MatchesController(IPlayerRepository playerRepository, IDemoRepository demoRepository)
        {
            _playerRepository = playerRepository;
            _demoRepository = demoRepository;
            _steamApi = new SteamApi();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Matches()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetMatchesData()
        {
            return new JsonResult
            {
                Data = new
                {
                    Matches = _demoRepository.GetAllLogs()
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}