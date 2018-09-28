using BusinessLogicLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICommentService
    {
        Task AnswerOnComment(CreateAnswerCommentDto comment);
    }
}
