using BusinessLogicLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IGameService
    {
        Task AddGame(GameDto game);
        Task DeleteGame(int id);
        Task EditGame(int id, GameDto editedGame);
        Task<GameDto> GetInfo(int id);


    }
}
