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
using WebApi.Infrastucture;


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


        [CustomValidation]
        [HttpPost("{id:int:min(1)}/comments")]
        public async Task<IActionResult> CreateAnswerForAnotherComment(int id, [FromBody]CreateAnswerModel comment)
        {                       
            var commentDto = new CreateAnswerCommentDto()
            {
                ParentCommentId = id,
                Body = comment.Body,
                GameId = comment.GameId,
                Name = comment.Name
            };

            await commnetService.AnswerOnComment(commentDto);
            return StatusCode(201, "Comment was created");
        }

    }
}
