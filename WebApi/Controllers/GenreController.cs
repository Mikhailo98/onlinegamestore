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

            return StatusCode(200, await genreService.GetAll());
            
        }

        [HttpGet("{id}/games")]
        public async Task<IActionResult> GetAllCommentsbyGameKey(int id)
        {

            List<GameDto> commentDtos = await genreService.GetGamesOfGenre(id);
            return StatusCode(200, commentDtos);

        }



        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            if (id <= 0)
            {
                return StatusCode(400, "Invalid Genre Id");
            }
            var genre = await genreService.GetInfo(id);
            return StatusCode(200, genre);


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

            await genreService.AddGenre(genre);
            return StatusCode(201, "Genre was added!");
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await genreService.DeleteGenre(id);
            return StatusCode(204, "Genre was deleted");
        }
    }
}
