using System.Collections.Generic;
using CsStat.Domain.Entities;
using CsStat.Domain.Models;

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

        List<WeaponStatDto> GetWeaponStat(string from="", string to="");
    }
}
