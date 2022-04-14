using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CsStat.Domain.Entities;
using CsStat.LogApi.Enums;
using DataService.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace BusinessFacade.Repositories.Implementations
{
    public class LogsRepository : BaseRepository, ILogsRepository
    {
        private static IMongoRepositoryFactory _mongoRepository;

        public IEnumerable<Log> GetAllLogs()
        {
           return base.GetAll<Log>();
        }

        public IEnumerable<Log> GetLogsForPeriod(DateTime timeFrom, DateTime timeTo, Expression<Func<Log, bool>> сondition = null)
        {
            var builder = new QueryBuilder<Log>();

            var queries = new List<IMongoQuery> {
                Query.And(Query.GTE("DateTime", timeFrom), Query.LTE("DateTime", timeTo))
            };
                                   
            var query = builder.And(queries);

            var logs = _mongoRepository.GetRepository<Log>().Collection.Find(query).AsQueryable();

            if (сondition != null)
            {
                logs = logs.Where(сondition);
            }
                
            return logs.ToList();
        }

        public IEnumerable<Log> GetPlayerLogs(Player player, DateTime timeFrom, DateTime timeTo)
        {
            var andList = new List<IMongoQuery>
            {
                Query.Or(Query.EQ("Player.SteamId", player.SteamId), Query.EQ("Victim.SteamId", player.SteamId), Query.EQ("Action", Actions.TargetBombed)),
                Query.And(Query.GTE("DateTime", timeFrom), Query.LTE("DateTime", timeTo))
            };

            var query = new QueryBuilder<Log>();

            var mongoQuery = query.And(andList);
            return _mongoRepository.GetRepository<Log>().Collection.Find(mongoQuery).ToList();
        }

        public LogsRepository(IMongoRepositoryFactory mongoRepository) : base(mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
    }
}