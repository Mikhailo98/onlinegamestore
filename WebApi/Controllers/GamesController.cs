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
using BusinessLogicLayer.Models.Dtos.GameDto;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using BusinessLogicLayer.Models.Dtos.CommentDto;

namespace WebApi.Controllers
{
    [Route("api/games")]
    public class GamesController : Controller
    {

        private readonly IGameService gameService;
        private readonly IMapper mapper;
        private readonly IHostingEnvironment _appEnvironment;

        public GamesController(IGameService gameService,
            IMapper mapper, IHostingEnvironment appEnvironment)
        {
            this.gameService = gameService;
            this.mapper = mapper;
            _appEnvironment = appEnvironment;
        }




        [HttpGet("{id}/genres")]
        public async Task<IActionResult> GetGenresByGameKey(int id)
        {
            List<GenreDto> commentDtos = await gameService.GetGenres(id);

            return Ok(commentDtos);

        }


        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(int id)
        {

            string j = await gameService.GetGameLocalPath(id);

            string filePath = Path.Combine(_appEnvironment.ContentRootPath, j);

            string fileType = "application/pdf";

            string fileName = "book.pdf";

            return PhysicalFile(filePath, fileType, fileName);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> EditGame(int id, [FromBody]GameCreateModel game)
        {

            EditGameDto dto = new EditGameDto()
            {
                Id = id,
                Description = game.Description,
                Genres = game.Genres,
                Name = game.Name,
                Platforms = game.Platforms,
                PublisherId = game.PublisherId
            };

            await gameService.EditGame(dto);

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody]GameCreateModel game)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateGameDto createdGame = new CreateGameDto()
            {
                Name = game.Name,
                Description = game.Description,
                PublisherId = game.PublisherId,
                Genres = game.Genres,
                Platforms = game.Platforms,
            };


            try
            {
                await gameService.AddGame(createdGame);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Game was added");
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<GameDto> games = await gameService.GetAll();
                return Ok(games);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var game = await gameService.GetInfo(id);
                return Ok(game);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteById(int id)
        {

            if (id <= 0)
            {
                return BadRequest("Invalid image id");
            }

            try
            {
                await gameService.DeleteGame(id);
                return Ok("Game was successfully deleted ");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("{id}/comments")]
        public async Task<IActionResult> MakeComment(int id, [FromBody]string comment)
        {

            if (id <= 0)
            {
                return BadRequest("Invalid game id");
            }


            try
            {
                await gameService.CommentGame(new CreateCommentDto() { Body = comment, GameId = id });
                return Ok("Comment was added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetAllComment(int id)
        {

            if (id <= 0)
            {
                return BadRequest("Invalid game id");
            }

            try
            {
                var comments = await gameService.GetAllComments(id);
                return Ok(comments);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}