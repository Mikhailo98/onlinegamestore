using BusinessLogicLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICommentService
    {
        Task AddComment(CreateCommentDto comment);
        Task DeleteComment(int id);
        Task EditComment(int id, CommentDto editedComment);
        Task<CommentDto> GetInfo(int id);
    }
}
