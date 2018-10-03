using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Dtos.CommentDto;
using BusinessLogicLayer.Models.Dtos.GameDto;
using BusinessLogicLayer.Pagination;
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
        Task EditGame(EditGameDto editedGame);
        Task<GameDto> GetInfo(int id);
        Task<List<GameDto>> GetAll();
        Task<List<CommentDto>> GetAllComments(int id);
        Task<List<GenreDto>> GetGenres(int id);
        Task CommentGame(CreateCommentDto comment);
        Task<string> GetGameLocalPath(int id);

        Task<List<GameDto>> OrderedBy(PagingParamsBll paging);

    }


}
