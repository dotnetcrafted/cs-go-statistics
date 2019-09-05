using System.Collections.Generic;
using CsStat.Domain.Entities;

namespace BusinessFacade.Repositories
{
    public interface IPlayerRepository
    {
        IEnumerable<PlayerStatsModel> GetStatsForAllPlayers();
        IEnumerable<Player> GetAllPlayers();
        Player GetPlayerByNickName(string nickName);
        Player GetPlayerById(string id);
        string AddPlayer(Player player);
        void AddPlayers(List<Player> players);
        void UpdatePlayer(string id=null, string nickName = null,string firstName = null, string secondName = null, string imagePath = null);
    }
}