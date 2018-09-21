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

namespace WebApi.Controllers
{
    [Route("api/games")]
    public class GamesController : Controller
    {

        private readonly IGameService gameService;

        public GamesController(IGameService gameService)
        {
            this.gameService = gameService;
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

    }
}