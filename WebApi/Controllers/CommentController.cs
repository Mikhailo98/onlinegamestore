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
               

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Comment
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }


        [HttpPost("{id}/comments")]
        public async Task<IActionResult> CreateAnswerForAnotherComment(int id, [FromBody]CreateCommentModel comment)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid comment id");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Mapping 
            //var commentDto = mapper.Map<CreateAnswerCommentDto>(comment);
            //commentDto.AnswerId = id;

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
                return BadRequest(ex.Message);
            }

            return Ok("Answer was added");
        }

    }
}
