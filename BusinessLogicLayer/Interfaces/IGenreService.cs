using BusinessLogicLayer.Dtos;
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
        Task EditGenre(int id, GenreDto editedGenre);
        Task<GenreDto> GetInfo(int id);
    }
}
