using System.Linq;
using System.Web.Mvc;
using BusinessFacade.Repositories;
using CsStat.LogApi;
using CsStat.LogApi.Interfaces;
using CsStat.SystemFacade.Extensions;
using CsStat.Web.Models.Matches;

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

            return Json(matches);
        }


        [HttpGet]
        public ActionResult GetMatchesData()
        {
            var matches = _demoRepository.GetMatches().ToList();

            return Json
            (
                matches.Select(x => new BaseMatch
                    {
                        Id = x.Id,
                        Map = x.Map,
                        Date = x.MatchDate,
                        TScore = x.TotalSquadAScore,
                        CTScore = x.TotalSquadBScore
                    })
            );
        }

        [HttpGet]
        public ActionResult GetMatch(string matchId)
        {
            if (matchId.IsNotEmpty())
            {
                var match = _demoRepository.GetMatch(matchId);

                var matchDetails = new MatchDetails
                {
                    Id = match.Id,
                    Map = match.Map,
                    Date = match.MatchDate,
                    TScore = match.TotalSquadAScore,
                    CTScore = match.TotalSquadBScore,
                    Rounds = match.Rounds.Select(round => new MatchRound
                    {
                        Id = "",
                        CTScore = round.CTScore,
                        TScore = round.TScore,
                        Reason = (int) round.Reason,
                        ReasonTitle = round.ReasonTitle,
                        Kills = round.Squads
                            .SelectMany(squad => squad.Players
                                .SelectMany(player => player.Kills
                                    .Select(kill => new MatchDetailsKill
                                    {
                                        Id = "",
                                        FormattedTime = "",
                                        Killer = player.SteamID,
                                        Victim = kill.Victim,
                                        Assister = kill.Assister,
                                        Weapon = kill.Weapon,
                                        IsSuicide = kill.IsSuicide,
                                        IsHeadshot = kill.IsHeadshot
                                    })
                                )).ToList(),
                        Squads = round.Squads.Select(squad => new MatchDetailsSquad
                        {
                            Id = "",
                            Title = squad.SquadTitle,
                            Players = squad.Players.Select(player => new MatchDetailsSquadPlayer
                            {
                                Id = "",
                                Name = player.Name,
                                SteamImage = player.ProfileImageUrl,
                                Kills = player.Kills.Count,
                                Deaths = player.Deaths.Count,
                                Assists = player.Assists.Count,
                                Adr = 0.0,
                                Ud = 0.0
                            }).ToList()
                        }).ToList()
                    }).ToList()
                };
                
                return Json(matchDetails);
            }

            return Json("missing match id");
        }
    }
}
