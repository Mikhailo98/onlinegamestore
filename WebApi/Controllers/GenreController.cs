using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models.Dtos.GenreDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/genres")]
    public class GenreController : Controller
    {

        private readonly IGenreService genreService;
        private readonly IMapper mapper;

        public GenreController(IGenreService genreService, IMapper mapper)
        {
            this.genreService = genreService;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await genreService.GetAll());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/games")]
        public async Task<IActionResult> GetAllCommentsbyGameKey(int id)
        {
            try
            {
                List<GameDto> commentDtos = await genreService.GetGamesOfGenre(id);
                return Ok(commentDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]EditGenreModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            EditGenreDto genreDto = new EditGenreDto()
            {
                Id = value.Id,
                HeadGenreId = value.HeadGenreId,
                Name = value.Name
            };

            await genreService.EditGenre(genreDto);


            return Ok();
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
