using System;
using System.Collections.Generic;
using System.Linq;
using CSStat.CsLogsApi.Extensions;
using CsStat.Domain.Definitions;
using CsStat.Domain.Entities;
using CsStat.Domain.Extensions;
using CsStat.Domain.Models;
using CsStat.LogApi;
using CsStat.LogApi.Enums;
using CsStat.StrapiApi;
using CsStat.SystemFacade.Extensions;
using DataService.Interfaces;

namespace BusinessFacade.Repositories.Implementations
{
    public class PlayerRepository : BaseRepository, IPlayerRepository
    {
        private static ILogsRepository _logsRepository;
        private static IMongoRepositoryFactory _mongoRepository;
        private static IStrapiApi _strapiApi;
        private static List<WeaponModel> _weapons;

        public PlayerRepository(IMongoRepositoryFactory mongoRepository, IStrapiApi strapiApi) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
            _logsRepository = new LogsRepository(_mongoRepository);
            _strapiApi = strapiApi;
            _weapons = _strapiApi.GetAllWeapons();
        }

        #region Mongo

        public IEnumerable<Player> GetAllPlayers()
        {
            var query = new QueryBuilder<Player>();
            var players = _mongoRepository.GetRepository<Player>().Collection.Find(query.Where(x => !x.IsRetired)).DistinctBy(x => x.SteamId).ToList();
            var steamIds = string.Join(",", players.Select(x => x.SteamId).ToList());
            var avatars = new SteamApi().GetAvatarUrlBySteamId(steamIds);
            
            foreach (var player in players)
            {
                player.ImagePath = avatars.FirstOrDefault(x => x.Key == player.SteamId).Value;
            }

            return players;
        }

        public Player GetPlayerByNickName(string nickName)
        {
            return _mongoRepository.GetRepository<Player>().GetAll(x => x.NickName == nickName).FirstOrDefault();
        }

        public Player GetPlayerById(string id)
        {
            return base.GetOne<Player>(id);
        }

        public string AddPlayer(Player player)
        {
            return _mongoRepository.GetRepository<Player>().Add(player) != null
                ? GetPlayerByNickName(player.NickName).Id
                : string.Empty;
        }

        public void AddPlayers(List<Player> players)
        {
           base.InsertBatch(players);
        }

        #endregion

        public IEnumerable<PlayerStatsModel> GetStatsForAllPlayers(string dateFrom, string dateTo, Constants.PeriodDay? periodDay = Constants.PeriodDay.All)
        {
            var logs = GetLogs(dateFrom, dateTo, periodDay);
            var playersStats = new List<PlayerStatsModel>();

            if (!logs.Any())
                return playersStats;

            var players = GetAllPlayers().ToList();

            if(!players.Any())
                return playersStats;

            foreach (var player in players)
            {
                var playerLogs = logs.Where(x => x.Player?.SteamId == player.SteamId || x.Victim?.SteamId == player.SteamId || x.Action == Actions.TargetBombed).ToList();

                if (!playerLogs.Any())
                {
                    continue;
                }
                playersStats.Add(CountStats(playerLogs, player));
            }

            SetAchievementsToPLayers(playersStats.OrderByDescending(x=>x.KdRatio).ToList());

            return playersStats;
        }


        public PlayerStatsModel GetStatsForPlayer(string playerName, string from ="", string to="")
        {
            var player = GetPlayerByNickName(playerName);
            
            if (player == null)
            {
                return null;
            }

            var logs = _logsRepository.GetPlayerLogs(player, from.ToDate(DateTime.MinValue), to.ToDate(DateTime.Today).AddDays(1)).ToList();
            
            return !logs.Any() 
                ? new PlayerStatsModel() 
                : CountStats(logs, player);
        }

        private static PlayerStatsModel CountStats(List<Log> logs, Player player)
        {
            var gunLogs = logs.Where(x => x.Action == Actions.Kill && x.Player.SteamId == player.SteamId).OrderBy(x=>x.Gun).ToList();
            var guns = GetGuns(gunLogs);
            var sniperRifle = guns?.Where(x => x.Weapon.Type.Type == WeaponTypes.SniperRifle);
            var grenade = guns?.Where(x => x.Weapon.Id == (int)Weapons.He).Sum(x => x.Kills) ?? 0;
            var knife = guns?.Where(x => x.Weapon.Id == (int)Weapons.Knife).Sum(x => x.Kills) ?? 0;
            var molotov = guns?.Where(x => x.Weapon.Id == (int)Weapons.Molotov || x.Weapon.Id == (int)Weapons.Inferno || x.Weapon.Id == (int)Weapons.Inc).Sum(x => x.Kills) ?? 0;
            var explodeBombs = GetExplodeBombs(logs.Where(x => x.Action == Actions.Plant).ToList(), logs.Where(x => x.Action == Actions.TargetBombed).ToList());
            var defuse = logs.Count(x => x.Action == Actions.Defuse);
            var friendlyKills = logs.Count(x => x.Action == Actions.FriendlyKill && x.Player.SteamId == player.SteamId);
            var assists = logs.Count(x => x.Action == Actions.Assist && x.Player.SteamId == player.SteamId);
            var kills = logs.Count(x => x.Action == Actions.Kill && x.Player.SteamId == player.SteamId);
            var deaths = logs.Count(x => x.Action == Actions.Kill && x.Victim?.SteamId == player.SteamId);
            var totalGames = logs.Count(x => x.Action == Actions.EnteredTheGame);
            var headShotCount = logs.Count(x => x.IsHeadShot && x.Action == Actions.Kill && x.Player.SteamId == player.SteamId);
            var victimList = logs.Where(x => x.Action == Actions.Kill && x.Player.SteamId == player.SteamId).Select(x => x.Victim).ToList();
            var killerList = logs.Where(x => x.Action == Actions.Kill && x.Victim?.SteamId == player.SteamId).Select(x => x.Player).ToList();
            var friendlyVictimList = logs.Where(x => x.Action == Actions.FriendlyKill && x.Player.SteamId == player.SteamId).Select(x => x.Victim).ToList();
            var friendlyKillerList = logs.Where(x => x.Action == Actions.FriendlyKill && x.Victim?.SteamId == player.SteamId).Select(x => x.Player).ToList();

            return new PlayerStatsModel
            {
                Player = player,
                Kills = kills,
                Deaths = deaths,
                Assists = assists,
                FriendlyKills = friendlyKills,
                TotalGames = totalGames,
                HeadShotsCount = headShotCount,
                HeadShotsPercent = kills == 0 ? 0 : (double)headShotCount/kills * 100,
                Guns = guns,
                Defuse = defuse,
                Explode = explodeBombs,
                Points = kills + assists + (defuse + explodeBombs) * 2 - friendlyKills * 2 - deaths / 2,
                SniperRifleKills = sniperRifle?.Select(x => x.Kills).Sum() ?? 0,
                Victims = GetPlayers(victimList).OrderByDescending(x => x.Count).ToList(),
                Killers = GetPlayers(killerList).OrderByDescending(x => x.Count).ToList(),
                FriendKillers = GetPlayers(friendlyKillerList).OrderByDescending(x => x.Count).ToList(),
                FriendVictims = GetPlayers(friendlyVictimList).OrderByDescending(x => x.Count).ToList(),
                GrenadeKills = grenade,
                MolotovKills = molotov,
                KnifeKills = knife

            };
        }
        
        private static List<VictimKillerModel> GetPlayers(List<Player> players)
        {
            var victimModel = new List<VictimKillerModel>();
            foreach (var victim in players.DistinctBy(x => x.SteamId))
            {
                victimModel.Add(new VictimKillerModel
                {
                    SteamId = victim.SteamId,
                    Count = players.Count(x => x.SteamId == victim.SteamId)
                });
            }

            return victimModel;
        }

        private static List<Log> GetLogs(string from = "", string to = "", Constants.PeriodDay? periodDay = Constants.PeriodDay.All)
        {
            var periodDayCondition = PeriodsDayCondition.Get(periodDay);
            return _logsRepository
                .GetLogsForPeriod(from.ToDate(DateTime.MinValue), to.ToDate(DateTime.Today).AddDays(1), periodDayCondition)
                .ToList();
        }

        private static List<WeaponStatModel> GetGuns(IReadOnlyCollection<Log> logs)
        {
            return !logs.Any()
                ? new List<WeaponStatModel>()
                : logs.Where(x=>x.Action==Actions.Kill).GroupBy(x => x.Gun).Select(r => new WeaponStatModel
                       {
                           Weapon = _weapons.FirstOrDefault(x=>x.Id == (int)r.Key),
                           Kills = r.Count()
                       }).OrderByDescending(x=>x.Kills).ToList();
        }

        private static int GetExplodeBombs(IReadOnlyCollection<Log> playersLogs, IReadOnlyCollection<Log> logs)
        {

            return playersLogs.Select(bomb => logs.Where(x => x.DateTime > bomb.DateTime && x.DateTime < bomb.DateTime.AddMinutes(1)).ToList())
                    .Count(intervalLogs => intervalLogs.Count(x => x.Action == Actions.TargetBombed) > 0);
        }

        private static void SetAchievementsToPLayers(List<PlayerStatsModel> playersStats)
        {
            var achievements = _strapiApi.GetAchieves();
            
            if (!achievements.Any())
                return;

            playersStats.Where(x => x.KdRatio > 0).OrderByDescending(x => x.KdRatio).ThenByDescending(x => x.Kills)
                .FirstOrDefault()?
                .Achievements
                .Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.FirstKd));

            playersStats.Where(x => x.KdRatio > 0).OrderByDescending(x => x.KdRatio).ThenByDescending(x => x.Kills)
                .Skip(1).Take(1).FirstOrDefault()?.Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.SecondKd));

            playersStats.Where(x => x.KdRatio > 0).OrderByDescending(x => x.KdRatio).ThenByDescending(x => x.Kills)
                .Skip(2).Take(1).FirstOrDefault()?.Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.ThirdKd));

            playersStats.Where(x => x.Kills > 0).OrderByDescending(x => x.Kills).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.Killer));

            playersStats.Where(x => x.Assists > 0).OrderByDescending(x => x.Assists).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.TeamPlayer));

            playersStats.Where(x => x.HeadShotsCount > 0 && x.Kills > 7).OrderByDescending(x => Math.Round(x.HeadShotsPercent)).ThenByDescending(x=>x.KdRatio)
                .FirstOrDefault()?.Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.HeadHunter));

            playersStats.Where(x => x.Deaths > 0).OrderByDescending(x => x.Deaths).ThenBy(x => x.KdRatio)
                .FirstOrDefault()?.Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.Kenny));

            playersStats.Where(x => x.Points > 0).OrderByDescending(x => x.Points).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.Mvp));

            var playerStats = playersStats.Where(x => x.SniperRifleKills > 0).OrderByDescending(x => x.SniperRifleKills).FirstOrDefault();
            playerStats?.Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.Sniper).ChangeDescription(playerStats.SniperRifleKills));

            playersStats.Where(x => x.FriendlyKills > 0).OrderByDescending(x => x.FriendlyKills).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.Brutus));

            playerStats = playersStats.Where(x => x.GrenadeKills > 0).OrderByDescending(x => x.GrenadeKills).FirstOrDefault();
            playerStats?.Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.Pitcher).ChangeDescription(playerStats.GrenadeKills));

            playerStats = playersStats.Where(x => x.MolotovKills > 0).OrderByDescending(x => x.MolotovKills).FirstOrDefault();
            playerStats?.Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.Firebug).ChangeDescription(playerStats.MolotovKills));

            playersStats.Where(x => x.Explode > 0).OrderByDescending(x => x.Explode).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.Bomberman));

            playersStats.Where(x => x.Defuse > 0).OrderByDescending(x => x.Defuse).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.Sapper));

            playersStats.Where(x => x.Kills > 0).OrderBy(x => x.Kills).ThenByDescending(x=>x.Deaths).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.Pacifist));

            playerStats = playersStats.Where(x => x.KnifeKills > 0).OrderByDescending(x => x.KnifeKills).FirstOrDefault();
            playerStats?.Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.Samurai).ChangeDescription(playerStats.KnifeKills));
        }
    }
}
