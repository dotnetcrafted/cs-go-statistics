using System.Collections.Generic;
using CsStat.Domain.Entities;

namespace BusinessFacade.Repositories
{
    public interface IPlayersRepository
    {
        IEnumerable<PlayerModel> GetAllPlayers();
    }
}