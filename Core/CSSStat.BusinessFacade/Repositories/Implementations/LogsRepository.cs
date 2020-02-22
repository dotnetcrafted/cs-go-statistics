using System;
using System.Collections.Generic;
using System.Linq;
using CsStat.Domain.Entities;
using CsStat.LogApi.Enums;
using DataService.Interfaces;
using MongoDB.Driver;

namespace BusinessFacade.Repositories.Implementations
{
    public class LogsRepository : BaseRepository, ILogsRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public IEnumerable<Log> GetAllLogs()
        {
           return base.GetAll<Log>();
        }

        public IEnumerable<Log> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo)
        {
            return _mongoRepository.GetRepository<Log>().GetAll(x => x.DateTime >= timeFrom && x.DateTime <= timeTo).ToList();
        }

        public IEnumerable<Log> GetPlayerLogs(Player player, DateTime timeFrom, DateTime timeTo)
        {                                          
            return _mongoRepository.GetRepository<Log>().GetAll(x=> 
            (x.Player.SteamId == player.SteamId || x.Victim.SteamId == player.SteamId || x.Action == Actions.TargetBombed) 
            && (x.DateTime >= timeFrom && x.DateTime <= timeTo)).ToList();
        }

        public LogsRepository(IMongoRepositoryFactory mongoRepository) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
    }
}