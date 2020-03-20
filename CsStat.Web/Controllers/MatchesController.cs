using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BusinessFacade;
using BusinessFacade.Repositories;
using CsStat.Domain.Definitions;
using CsStat.Domain.Models;
using CsStat.LogApi;
using CsStat.LogApi.Interfaces;
using CsStat.StrapiApi;
using CsStat.SystemFacade.Extensions;
using CsStat.Web.Models.Matches;

namespace CsStat.Web.Controllers
{
    public class MatchesController : BaseController
    {
        private static IPlayerRepository _playerRepository;
        private static IDemoRepository _demoRepository;
        private static ISteamApi _steamApi;
        private static IStrapiApi _strapiApi;
        private static List<MapInfoModel> _mapInfos;
        private static List<WeaponModel> _weapons;

        public MatchesController(IPlayerRepository playerRepository, IDemoRepository demoRepository,
            IStrapiApi strapiApi)
        {
            _playerRepository = playerRepository;
            _demoRepository = demoRepository;
            _steamApi = new SteamApi();
            _strapiApi = strapiApi;
            _mapInfos = _strapiApi.GetAllMapInfos();
            _weapons = strapiApi.GetAllWeapons();
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

            if (!matches.Any())
            {
                return Json("An error occurred getting matches");
            }

            return Json
            (
                matches.Select(x => new BaseMatch
                {
                    Id = x.Id,
                    Map = x.Map,
                    Date = x.MatchDate,
                    TScore = x.TotalSquadAScore,
                    CTScore = x.TotalSquadBScore,
                    MapImage = GetMapImage(x.Map),
                    Duration = x.Duration
                })
            );
        }

        [HttpGet]
        public ActionResult GetMatch(string matchId)
        {
            if (matchId.IsNotEmpty())
            {
                var match = _demoRepository.GetMatch(matchId);

                if (match == null)
                    return Json("Match not found");

                var steamIds = string.Join(",", match.Players.Select(x => x.SteamID).ToList());
                var avatars = _steamApi.GetAvatarUrlBySteamId(steamIds);
                var matchDetails = new MatchDetails
                {
                    Id = match.Id,
                    Map = match.Map,
                    MapImage = GetMapImage(match.Map),
                    Date = match.MatchDate,
                    TScore = match.TotalSquadAScore,
                    CTScore = match.TotalSquadBScore,
                    Duration = match.Duration,
                    Rounds = match.Rounds.Select(round => new MatchRound
                    {
                        Id = round.RoundNumber,
                        CTScore = round.CTScore,
                        TScore = round.TScore,
                        Reason = (int) round.Reason,
                        ReasonTitle = round.ReasonTitle,
                        Duration = round.Duration,
                        Kills = round.Squads
                            .SelectMany(squad => squad.Players
                                .SelectMany(player => player.Kills
                                    .Select((kill, index) => new MatchDetailsKill
                                    {
                                        Id = index,
                                        FormattedTime = "0",
                                        Killer = player.SteamID,
                                        Victim = kill.Victim,
                                        Assister = kill.Assister,
                                        Weapon = new WeaponViewModel
                                        {
                                            Id = (int)kill.Weapon,
                                            IconUrl = _weapons.FirstOrDefault(x=>x.Id == (int)kill.Weapon)?.Icon.FullUrl,
                                            ImageUrl = _weapons.FirstOrDefault(x=>x.Id == (int)kill.Weapon)?.Image.FullUrl,
                                            Name = _weapons.FirstOrDefault(x=>x.Id == (int)kill.Weapon)?.Name,
                                            Type = _weapons.FirstOrDefault(x=>x.Id == (int)kill.Weapon)?.Type.Name,
                                        },
                                        IsSuicide = kill.IsSuicide,
                                        IsHeadshot = kill.IsHeadshot,
                                        Time = kill.Time,
                                        IsPenetrated = kill.IsPenetrated
                                    })
                                )).ToList(),
                        Squads = round.Squads.Select((squad, index) => new MatchDetailsSquad
                        {
                            Id = index,
                            Title = squad.SquadTitle,
                            Players = squad.Players.Select(player => new MatchDetailsSquadPlayer
                            {
                                Id = player.SteamID.ToString(),
                                Name = player.Name,
                                SteamImage = avatars.FirstOrDefault(x => x.Key == player.SteamID.ToString()).Value,
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

        private static string GetMapImage(string mapName)
        {
            return _mapInfos.FirstOrDefault(y => y.MapName == mapName)?.Image.FullUrl
                   ?? _strapiApi.GetImage(Constants.ImagesName.DefaultImage)?.Image.FullUrl
                   ?? "";
        }
    }
}
