using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using BusinessFacade;
using BusinessFacade.Repositories;
using CsStat.Domain;
using CsStat.Domain.Entities.Demo;
using CsStat.Domain.Models;
using CsStat.LogApi;
using CsStat.LogApi.Interfaces;
using CsStat.StrapiApi;
using CsStat.SystemFacade.Extensions;
using CsStat.Web.Models;
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
        public ActionResult GetMatchesData(string page)
        {
            var matches = _demoRepository.GetMatches()
                .Where(x => x.TotalSquadAScore + x.TotalSquadBScore > Settings.RoundsLimit)
                .ToList();

            if (!matches.Any())
            {
                return Json("An error occurred getting matches");
            }

            var pageNum = ParsePageNumber(page);

            var model = new MatchesViewModel
            {
                Matches = matches
                    .OrderByDescending(x => x.MatchDate)
                    .ThenByDescending(x => x.ParsedDate)
                    .Skip((pageNum - 1) * Settings.MatchesLimit)
                    .Take(Settings.MatchesLimit)
                    .Select(x => new BaseMatch
                    {
                        Id = x.Id,
                        Map = x.Map,
                        Date = x.MatchDate,
                        AScore = x.TotalSquadAScore,
                        BScore = x.TotalSquadBScore,
                        MapImage = GetMapImage(x.Map),
                        Duration = x.Duration
                    }),
                Pagination = new Pagination 
                { 
                    PageSize = Settings.MatchesLimit,
                    TotalPages = (int)Math.Ceiling((decimal)matches.Count()/Settings.MatchesLimit),
                    CurrentPage = pageNum,
                    TotalItems = matches.Count
                }
            };

            return Json(model);
        }

        [HttpGet]
        public ActionResult GetMatch(string matchId)
        {
            if (matchId.IsNotEmpty())
            {
                var match = _demoRepository.GetMatch(matchId);
                var images = _strapiApi.GetAllImages();

                if (match == null)
                    return Json("Match not found");

                var playerStatByRounds = match.Rounds.SelectMany(round => round.Squads.SelectMany(squad =>
                    squad.Players.Select(player =>
                        new PlayerStatByRound
                        {
                            RoundNumber = round.RoundNumber,
                            SteamId = player.SteamID,
                            Kills = player.Kills.Where(x => !x.IsSuicide).ToList().Count -
                                    player.Kills.Where(x => x.IsSuicide).ToList().Count,
                            Assists = player.Assists.Count,
                            Death = player.Deaths.Count,
                            Damage = player.Damage.Sum(x => x.HealthDamage),
                            UtilityDamage = player.UtilityDamage.Sum(x => x.HealthDamage),
                            Score = GetScore(player)
                        }))).ToList();

                var matchDetails = new MatchDetails
                {
                    Id = match.Id,
                    Map = match.Map,
                    MapImage = GetMapImage(match.Map),
                    Date = match.MatchDate,
                    AScore = match.TotalSquadAScore,
                    BScore = match.TotalSquadBScore,
                    Duration = match.Duration,
                    Rounds = match.Rounds.Select(round => new MatchRound
                    {
                        Id = round.RoundNumber,
                        CTScore = round.CTScore,
                        TScore = round.TScore,
                        Reason = (int) round.Reason,
                        ReasonTitle = round.ReasonTitle,
                        Duration = round.Duration,
                        ReasonIconUrl = images.FirstOrDefault(x => string.Equals(x.CodeName, round.ReasonTitle, StringComparison.InvariantCultureIgnoreCase))?.Image.FullUrl,
                        Kills = round.Squads
                            .SelectMany(squad => squad.Players
                                .SelectMany(player => player.Kills
                                    .Select((kill, index) => new MatchDetailsKill
                                    {
                                        Id = index,
                                        Killer = player.SteamID.ToString(),
                                        Victim = kill.Victim?.ToString(),
                                        Assister = kill.Assister?.ToString(),
                                        Weapon = kill.Weapon,
                                        IsSuicide = kill.IsSuicide,
                                        IsHeadshot = kill.IsHeadshot,
                                        Time = kill.Time,
                                        IsPenetrated = kill.IsPenetrated
                                    })
                                )).OrderBy(x => x.Time).ToList(),
                        Squads = round.Squads.Select((squad, index) => new MatchDetailsSquad
                        {
                            Id = index,
                            Title = squad.SquadTitle,
                            Players = squad.Players.Select(player => new MatchDetailsSquadPlayer
                            {
                                Id = player.SteamID.ToString(),
                                Team = squad.Team,
                                Kills = playerStatByRounds
                                    .Where(x => x.SteamId == player.SteamID && x.RoundNumber <= round.RoundNumber)
                                    .Sum(t => t.Kills),
                                Deaths = playerStatByRounds
                                    .Where(x => x.SteamId == player.SteamID && x.RoundNumber <= round.RoundNumber)
                                    .Sum(t => t.Death),
                                Assists = playerStatByRounds
                                    .Where(x => x.SteamId == player.SteamID && x.RoundNumber <= round.RoundNumber)
                                    .Sum(t => t.Assists),
                                Adr = Convert.ToInt32(Math.Round(
                                    playerStatByRounds.Where(x => x.SteamId == player.SteamID &&
                                                                  x.RoundNumber <= round.RoundNumber)
                                        .Sum(t => t.Damage) / (double) round.RoundNumber)),
                                Ud = playerStatByRounds.Where(x => x.SteamId == player.SteamID &&
                                                                   x.RoundNumber <= round.RoundNumber)
                                    .Sum(t => t.UtilityDamage),
                                Score = playerStatByRounds
                                    .Where(x => x.SteamId == player.SteamID && x.RoundNumber <= round.RoundNumber)
                                    .Sum(t => t.Score),
                                IsDied = playerStatByRounds
                                    .First(x => x.SteamId == player.SteamID && x.RoundNumber == round.RoundNumber)
                                    .Death > 0
                            }).OrderByDescending(player => player.Score).ToList()
                        }).OrderByDescending(x => x.Title).ToList()
                    }).ToList()
                };

                return Json(matchDetails);
            }

            return Json("missing match id");
        }

        private static int GetScore(PlayerLog player)
        {
            var bombPlanted = player.BombPlants.Count;
            var bombExploded = player.BombExplosions.Count;

            var bombDefuse = player.BombDefuses.Count;

            var kills = player.Kills.Count(x => !x.IsSuicide);
            var assist = player.Assists.Count;

            var killTeammate = player.Teamkills.Count;
            var suicide = player.Kills.Count(x => x.IsSuicide);

            return (bombPlanted * 2) +
                   (bombExploded * 2) +
                   (bombDefuse * 2) +
                   (kills * 2) +
                   assist +
                   (killTeammate * -1) +
                   (suicide * -2);
        }

        private static string GetMapImage(string mapName)
        {
            return _mapInfos.FirstOrDefault(y => y.MapName == mapName)?.Image.FullUrl
                   ?? _strapiApi.GetImage(BusinessFacade.Constants.ImagesIds.DefaultImage)?.Image.FullUrl
                   ?? "";
        }

        private static int ParsePageNumber(string page)
        {
            var pageNum = page.ParseOrDefault(1);
            return pageNum < 1 ? 1 : pageNum;

        }
    }
}