using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using BusinessLogicLayer.Interfaces;
using WebApi.Models;
using BusinessLogicLayer.Dtos;
using AutoMapper;

namespace WebApi.Controllers
{
    [Route("api/comments")]
    public class CommentController : Controller
    {
        private readonly ICommentService commnetService;
        private readonly IMapper mapper;

        public CommentController(ICommentService gameService, IMapper mapper)
        {
            this.commnetService = gameService;
            this.mapper = mapper;
        }

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> CreateAnswerForAnotherComment(int id, [FromBody]CreateCommentModel comment)
        {
            if (id <= 0)
            {
                return StatusCode(400, "Invalid comment id");
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }


            var y = mapper.Map<CreateAnswerCommentDto>(comment);

            var commentDto = new CreateAnswerCommentDto()
            {
                AnswerId = id,
                Body = comment.Body,
                GameId = comment.GameId
            };

            try
            {
                await commnetService.AddComment(commentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return StatusCode(201, "Answer was added");
        }

    }
}
