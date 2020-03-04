using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessFacade.Repositories;
using CsStat.Domain.Entities.Demo;
using CsStat.LogApi;
using CsStat.LogApi.Interfaces;
using CsStat.SystemFacade.Extensions;

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


        [HttpGet]
        public ActionResult GetFullData()
        {
            var matches = _demoRepository.GetAllLogs().ToList();

            var players = matches.SelectMany(x => x.Players.Select(z => z.SteamID)).Distinct();
            var steamIds = string.Join(",", players);
            var avatars = _steamApi.GetAvatarUrlBySteamId(steamIds);

            foreach (var match in matches)
            {
                foreach (var player in match.Players)
                {
                    player.ProfileImageUrl = avatars.FirstOrDefault(x => x.Key == player.SteamID.ToString()).Value;
                }
            }

            return new JsonResult
            {
                Data = new {matches},
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [HttpGet]
        public ActionResult GetMatchesData()
        {
            var matches = _demoRepository.GetMatches().ToList();

            return new JsonResult
            {
                Data = matches.Select(x => new MatchViewData()
                {
                    Id = x.Id,
                    Map = x.Map,
                    Date = x.MatchDate
                }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetMatch(string matchId)
        {
            if (matchId.IsNotEmpty())
            {
                var match = _demoRepository.GetMatch(matchId);

                return new JsonResult
                {
                    Data = new { match },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            return new JsonResult
            {
                Data = "missing match id",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

    public class MatchViewData
    {
        public string Id { get; set; }
        public string Map { get; set; }
        public DateTime? Date { get; set; }
    }
}