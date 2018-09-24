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
                return StatusCode(400, ex.Message);
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
                return StatusCode(400, ex.Message);
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            if (id <= 0)
            {
                return StatusCode(400, "Invalid Genre Id");
            }

            try
            {
                var genre = await genreService.GetInfo(id);
                return StatusCode(200, genre);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]EditGenreModel value)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);

            }

            EditGenreDto genreDto = new EditGenreDto()
            {
                Id = value.Id,
                HeadGenreId = value.HeadGenreId,
                Name = value.Name
            };

            await genreService.EditGenre(genreDto);


            return StatusCode(204);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateGanreModel value)
        {

            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            GenreCreateDto genre = new GenreCreateDto()
            {
                Name = value.Name,
                HeadGenreId = value.HeadGenre
            };
            try
            {
                await genreService.AddGenre(genre);
                return StatusCode(201, "Genre was added!");
            }
            catch (Exception ex)
            {

                return StatusCode(400, ex.Message);
            }

        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await genreService.DeleteGenre(id);
                return StatusCode(204, "Genre was deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

        }
    }
}
