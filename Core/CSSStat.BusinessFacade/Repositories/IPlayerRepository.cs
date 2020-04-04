using System;
using System.Collections.Generic;
using BusinessFacade.Repositories.Implementations;
using CsStat.Domain.Entities;

namespace BusinessFacade.Repositories
{
    public interface IPlayerRepository
    {
        IEnumerable<PlayerStatsModel> GetStatsForAllPlayers(string dateFrom="", string dateTo="", Constants.PeriodDay? periodDay = Constants.PeriodDay.All);
        PlayerStatsModel GetStatsForPlayer(string playerName, string dateFrom = "", string dateTo = "");
        IEnumerable<Player> GetAllPlayers();
        Player GetPlayerByNickName(string nickName);
        Player GetPlayerById(string id);
        string AddPlayer(Player player);
        void AddPlayers(List<Player> players);
    }
}