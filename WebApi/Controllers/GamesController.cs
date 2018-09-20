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

            GameDto g = new GameDto()
            {
                Name = game.Name,
                Description = game.Description,
                PublisherId = game.PublisherId
            };


            try
            {
                await gameService.AddGame(g);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Game was added");
        }

        [HttpPut]
        public async Task<IActionResult> EditGame([FromBody]EditGameModel game)
        {
            return null;
        }

        [HttpGet]
        public HttpResponseMessage GetAll()
        {

            return null;
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

        [HttpDelete("{id}")]
        public HttpResponseMessage DeleteById(int id)
        {
            return null;
        }



    }
}