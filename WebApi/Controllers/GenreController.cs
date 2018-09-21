using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/genre")]
    public class GenreController : Controller
    {

        private readonly IGenreService genreService;

        public GenreController(IGenreService genreService)
        {
            this.genreService = genreService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await genreService.GetAll());

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            if (id <= 0)
            {
                return BadRequest("Invalid Genre Id");
            }

            try
            {
                var genre = await genreService.GetInfo(id);
                return Ok(genre);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateGanreModel value)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenreCreateDto genre = new GenreCreateDto()
            {
                Name = value.Name,
                HeadGenreId = value.HeadGenre
            };
            try
            {
                await genreService.AddGenre(genre);
                return Ok("Genre was added!");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

     

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await genreService.DeleteGenre(id);
                return Ok("Genre was deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
