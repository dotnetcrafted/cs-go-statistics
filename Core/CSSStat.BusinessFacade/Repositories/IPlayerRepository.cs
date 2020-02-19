using System.Collections.Generic;
using CsStat.Domain.Entities;

namespace BusinessFacade.Repositories
{
    public interface IPlayerRepository
    {
        IEnumerable<PlayerStatsModel> GetStatsForAllPlayers(string dateFrom="", string dateTo="");
        PlayerStatsModel GetStatsForPlayer(string playerName, string dateFrom = "", string dateTo = "");
        IEnumerable<Player> GetAllPlayers();
        Player GetPlayerByNickName(string nickName);
        Player GetPlayerById(string id);
        string AddPlayer(Player player);
        void AddPlayers(List<Player> players);
        void UpdatePlayer(string id, string firstName = null, string secondName = null, string imagePath = null);
    }
}