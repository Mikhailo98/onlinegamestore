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
using Microsoft.Extensions.Logging;
using WebApi.Infrastucture;

namespace WebApi.Controllers
{
    [Route("api/games")]
    public class GamesController : Controller
    {

        private readonly IGameService gameService;
        private readonly IMapper mapper;
        private readonly IHostingEnvironment appEnvironment;
        private readonly ILogger<GamesController> _logger;



        public GamesController(IGameService gameService,
            IMapper mapper, IHostingEnvironment appEnvironment, ILogger<GamesController> logger)
        {
            this.gameService = gameService;
            this.mapper = mapper;
            this.appEnvironment = appEnvironment;
            _logger = logger;


        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<GameDto> games = await gameService.GetAll();

            return StatusCode(200, games);

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var game = await gameService.GetInfo(id);
            return StatusCode(200, game);


        }

        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetAllComment(int id)
        {

            if (id <= 0)
            {
                return StatusCode(400, "Invalid game id");
            }

            var comments = await gameService.GetAllComments(id);
            return Ok(comments);


        }


        [HttpGet("{id}/genres")]
        public async Task<IActionResult> GetGenresByGameKey(int id)
        {
            _logger.LogWarning("Index page says hello");


            List<GenreDto> commentDtos = await gameService.GetGenres(id);
            return StatusCode(200, commentDtos);

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
        [CustomValidation]
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



            await gameService.AddGame(createdGame);



            return StatusCode(201, "Game was added");
        }







        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteById(int id)
        {

            if (id <= 0)
            {
                return StatusCode(400, "Invalid image id");
            }


            await gameService.DeleteGame(id);
            return StatusCode(204, "Game was successfully deleted ");


        }



        [HttpPost("{id}/comments")]
        public async Task<IActionResult> MakeComment(int id, [FromBody]string comment)
        {

            if (id <= 0)
            {
                return StatusCode(400, "Invalid game id");
            }


            await gameService.CommentGame(new CreateCommentDto() { Body = comment, GameId = id });
            return StatusCode(201, "Comment was added");


        }





    }
}