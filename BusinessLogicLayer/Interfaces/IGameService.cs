using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Dtos.GameDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IGameService
    {
        Task AddGame(CreateGameDto game);
        Task DeleteGame(int id);
        Task EditGame( EditGameDto editedGame);
        Task<DetailedGameModel> GetInfo(int id);
        Task<List<GameDto>> GetAll();
    }
}
