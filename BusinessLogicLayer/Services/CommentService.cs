using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("BusinessLogicLayer.Test")]
namespace BusinessLogicLayer.Services
{
    internal class CommentService : ICommentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        public async Task AnswerOnComment(CreateAnswerCommentDto comment)
        {
            using (unitOfWork)
            {
                var commentOnAnswer = unitOfWork.CommentRepository.GetSingleAsync(c => c.Id == comment.ParentCommentId);
                var game = unitOfWork.GameRepository.GetSingleAsync(c => c.Id == comment.GameId);

                if (await commentOnAnswer == null)
                {
                    throw new ArgumentException("Invalid comment id");
                }

                if (await game == null)
                {
                    throw new ArgumentException("Invalid game id");
                }


                unitOfWork.CommentRepository.Create(new Comment()
                {
                    Name = commentOnAnswer.Result.
                    Body = $"{commentOnAnswer.Result.Name}, " + comment.Body,
                    ParentComment = commentOnAnswer.Result,
                    Game = game.Result,
                });

                await unitOfWork.CommitAsync();
            }
        }
    }
}
