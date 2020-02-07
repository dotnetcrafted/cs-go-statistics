using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CSStat.CsLogsApi.Extensions;
using CsStat.Domain.Definitions;
using CsStat.Domain.Entities;
using CsStat.Domain.Models;
using CsStat.LogApi.Enums;
using CsStat.StrapiApi;
using CsStat.SystemFacade.Attributes;
using CsStat.SystemFacade.Extensions;
using DataService.Interfaces;
using MongoDB.Driver.Builders;

namespace BusinessFacade.Repositories.Implementations
{
    public class PlayerRepository : BaseRepository, IPlayerRepository
    {
        private static ILogsRepository _logsRepository;
        private static IMongoRepositoryFactory _mongoRepository;
        private static IStrapiApi _strapiApi;
        
        public PlayerRepository(IMongoRepositoryFactory mongoRepository, IStrapiApi strapiApi) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
            _logsRepository = new LogsRepository(_mongoRepository);
            _strapiApi = strapiApi;
        }

        #region Mongo

        public IEnumerable<Player> GetAllPlayers()
        {
            return base.GetAll<Player>().OrderByDescending(x=>x.Id).DistinctBy(x=>x.SteamId);
        }

        public Player GetPlayerByNickName(string nickName)
        {
            var query = new QueryBuilder<Player>();
            return _mongoRepository.GetRepository<Player>().Collection.Find(query.EQ(x => x.NickName, nickName)).FirstOrDefault();
        }

        public Player GetPlayerById(string id)
        {
            return base.GetOne<Player>(id);
        }

        public string AddPlayer(Player player)
        {
            return _mongoRepository.GetRepository<Player>().Collection.Insert(player).Ok
                ? GetPlayerByNickName(player.NickName).Id
                : string.Empty;
        }

        public void AddPlayers(List<Player> players)
        {
           base.InsertBatch(players);
        }

        public void UpdatePlayer(string id, string firstName = null, string secondName = null, string imagePath = null)
        {
            var player = GetPlayerById(id);

            if (player == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                player.FirstName = firstName;
            }

            if (!string.IsNullOrEmpty(secondName))
            {
                player.SecondName = secondName;
            }

            if (!string.IsNullOrEmpty(imagePath))
            {
                player.ImagePath = imagePath;
            }

            _mongoRepository.GetRepository<Player>().Collection.Save(player);
        }

        #endregion

        public IEnumerable<PlayerStatsModel> GetStatsForAllPlayers(string dateFrom, string dateTo)
        {
            var logs = GetLogs(dateFrom, dateTo);
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
            var guns = GetGuns(logs.Where(x => x.Action == Actions.Kill && x.Player.SteamId == player.SteamId).ToList());
            var sniperRifle = guns?.Where(x => x.Gun.GetAttribute<IsSniperRifleAttribute>().Value);
            var grenade = guns?.Where(x => x.Gun == Guns.He).Sum(x => x.Kills);
            var molotov = guns?.Where(x => x.Gun == Guns.Molotov || x.Gun == Guns.Inferno || x.Gun == Guns.Inc).Sum(x => x.Kills);
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
                HeadShot = headShotCount,
                Guns = guns,
                Defuse = defuse,
                Explode = explodeBombs,
                Points = kills + assists + (defuse + explodeBombs) * 2 - friendlyKills * 2 - deaths / 2,
                SniperRifleKills = sniperRifle?.Select(x => x.Kills).Sum() ?? 0,
                Victims = GetPlayers(victimList).OrderByDescending(x => x.Count).ToList(),
                Killers = GetPlayers(killerList).OrderByDescending(x => x.Count).ToList(),
                FriendKillers = GetPlayers(friendlyKillerList).OrderByDescending(x => x.Count).ToList(),
                FriendVictims = GetPlayers(friendlyVictimList).OrderByDescending(x => x.Count).ToList(),
                GrenadeKills = grenade ?? 0,
                MolotovKills = molotov ?? 0

            };
        }
        
        private static List<PlayerModel> GetPlayers(List<Player> players)
        {
            var victimModel = new List<PlayerModel>();
            foreach (var victim in players.DistinctBy(x => x.SteamId))
            {
                victimModel.Add(new PlayerModel
                {
                    Name = victim.NickName,
                    SteamId = victim.SteamId,
                    Count = players.Count(x => x.SteamId == victim.SteamId)
                });
            }

            return victimModel;
        }

        private static List<Log> GetLogs(string from = "", string to = "")
        {
            return _logsRepository
                .GetLogsForPeriod(from.ToDate(DateTime.MinValue), to.ToDate(DateTime.Today).AddDays(1))
                .ToList();
        }

        private static List<GunModel> GetGuns(IReadOnlyCollection<Log> logs)
        {
            return !logs.Any()
                ? null
                : logs.Where(x => x.Action == Actions.Kill)
                    .GroupBy(x => x.Gun)
                    .Select(r => new GunModel
                    {
                        Gun = r.Key,
                        Kills = r.Count()
                    }).OrderByDescending(x => x.Kills).ToList();
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
            
            playersStats.Where(x=>x.KdRatio > 0).OrderByDescending(x => x.KdRatio).ThenByDescending(x=>x.Kills).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.FirstKd)); 

            playersStats.Where(x=>x.KdRatio > 0).OrderByDescending(x => x.KdRatio).ThenByDescending(x=>x.Kills).Skip(1).Take(1).First()
                .Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.SecondKd));
                
            playersStats.Where(x=>x.KdRatio > 0).OrderByDescending(x => x.KdRatio).ThenByDescending(x => x.Kills).Skip(2).Take(1).First()
                .Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.ThirdKd));

            playersStats.Where(x=>x.Kills > 0).OrderByDescending(x => x.Kills).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x=>x.AchievementId == Constants.AchievementsIds.Killer));

            playersStats.Where(x=>x.Assists > 0).OrderByDescending(x => x.Assists).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x=>x.AchievementId == Constants.AchievementsIds.TeamPlayer));

            playersStats.Where(x=>x.HeadShot > 0 && x.Kills > 7).OrderByDescending(x => x.HeadShot).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x=>x.AchievementId == Constants.AchievementsIds.HeadHunter));
                
            playersStats.Where(x=>x.Deaths > 0).OrderByDescending(x => x.Deaths).ThenBy(x=>x.KdRatio).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x=>x.AchievementId == Constants.AchievementsIds.Kenny));
                
            playersStats.Where(x=>x.Points > 0).OrderByDescending(x=>x.Points).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x=>x.AchievementId == Constants.AchievementsIds.Mvp));              

            playersStats.Where(x=>x.SniperRifleKills > 0).OrderByDescending(x=>x.SniperRifleKills).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x=>x.AchievementId == Constants.AchievementsIds.Sniper));

            playersStats.Where(x => x.FriendlyKills > 0).OrderByDescending(x => x.FriendlyKills).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x => x.AchievementId == Constants.AchievementsIds.Brutus));
               
            playersStats.Where(x=>x.GrenadeKills > 0).OrderByDescending(x=>x.GrenadeKills).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x=>x.AchievementId == Constants.AchievementsIds.Pitcher));

            playersStats.Where(x=>x.MolotovKills > 0).OrderByDescending(x=>x.MolotovKills).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x=>x.AchievementId == Constants.AchievementsIds.Firebug));

            playersStats.Where(x=>x.Explode > 0).OrderByDescending(x=>x.Explode).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x=>x.AchievementId == Constants.AchievementsIds.Bomberman));
                
            playersStats.Where(x=>x.Defuse > 0).OrderByDescending(x=>x.Defuse).FirstOrDefault()?
                .Achievements.Add(achievements.FirstOrDefault(x=>x.AchievementId == Constants.AchievementsIds.Sapper));
        }

        public List<WeaponStat> GetWeaponStat()
        {
            return GetLogs()
                .Where(l => l.Action == Actions.Kill)
                .Where(l => l.Gun != Guns.Null &&
                            l.Gun != Guns.Unknown &&
                            l.Gun != Guns.Bomb)
                .GroupBy(gg => gg.Gun)
                .Select(g => new WeaponStat
                {
                    Title = g.Key.GetDescription(),
                    Total = g.Count(),
                    Players = g.GroupBy(pg => new
                        {
                            pg.Player.NickName,
                            pg.Player.SteamId,
                        })
                        .Select(p => new WeaponPlayerStat
                        {
                            NickName = p.Key.NickName,
                            SteamId = p.Key.SteamId,
                            Kills = p.Count()
                        })
                        .OrderByDescending(z => z.Kills)
                        .ToList()
                }).ToList();
        }
    }
}
