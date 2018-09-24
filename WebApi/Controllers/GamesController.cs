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
        private readonly IHostingEnvironment appEnvironment;

        public GamesController(IGameService gameService,
            IMapper mapper, IHostingEnvironment appEnvironment)
        {
            this.gameService = gameService;
            this.mapper = mapper;
            this.appEnvironment = appEnvironment;
        }




        [HttpGet("{id}/genres")]
        public async Task<IActionResult> GetGenresByGameKey(int id)
        {
            try
            {
                List<GenreDto> commentDtos = await gameService.GetGenres(id);
                return StatusCode(200, commentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }


        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(int id)
        {

            string localpath = await gameService.GetGameLocalPath(id);

            string filePath = Path.Combine(appEnvironment.ContentRootPath, localpath);
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

            return StatusCode(204);
        }


        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody]GameCreateModel game)
        {

            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
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
                return StatusCode(400, ex.Message);
            }

            return StatusCode(201, "Game was added");
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<GameDto> games = await gameService.GetAll();
                return StatusCode(200, games);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var game = await gameService.GetInfo(id);
                return StatusCode(200, game);

            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);

            }

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteById(int id)
        {

            if (id <= 0)
            {
                return StatusCode(400, "Invalid image id");
            }

            try
            {
                await gameService.DeleteGame(id);
                return StatusCode(204, "Game was successfully deleted ");

            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }



        [HttpPost("{id}/comments")]
        public async Task<IActionResult> MakeComment(int id, [FromBody]string comment)
        {

            if (id <= 0)
            {
                return StatusCode(400, "Invalid game id");
            }


            try
            {
                await gameService.CommentGame(new CreateCommentDto() { Body = comment, GameId = id });
                return StatusCode(201, "Comment was added");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

        }


        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetAllComment(int id)
        {

            if (id <= 0)
            {
                return StatusCode(400, "Invalid game id");
            }

            try
            {
                var comments = await gameService.GetAllComments(id);
                return Ok(comments);

            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }


    }
}