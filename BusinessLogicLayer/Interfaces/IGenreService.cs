using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Models.Dtos.GenreDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IGenreService
    {
        Task AddGenre(GenreCreateDto genre);
        Task DeleteGenre(int id);
        Task EditGenre(EditGenreDto editedGenre);
        Task<GenreDto> GetInfo(int id);
        Task<List<GenreDto>> GetAll();
        Task<List<GameDto>> GetGamesOfGenre(int id);
    }
}
