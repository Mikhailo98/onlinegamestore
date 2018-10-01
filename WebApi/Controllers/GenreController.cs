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
using WebApi.Infrastucture;
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

        [HttpGet("{id:int:min(1)}/games")]
        public async Task<IActionResult> GetAllCommentsbyGameKey(int id)
        {
            List<GameDto> commentDtos = await genreService.GetGamesOfGenre(id);
            return StatusCode(200, commentDtos);

        }



        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> Get(int id)
        {
            var genre = await genreService.GetInfo(id);
            return StatusCode(200, genre);
        }


        [HttpPut("{id:int:min(1)}")]
        [CustomValidation]
        public async Task<IActionResult> Put(int id, [FromBody]EditGenreModel value)
        {

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
        [CustomValidation]
        public async Task<IActionResult> Post([FromBody]CreateGanreModel value)
        {

            GenreCreateDto genre = new GenreCreateDto()
            {
                Name = value.Name,
                HeadGenreId = value.HeadGenre
            };

            await genreService.AddGenre(genre);
            return StatusCode(201, "Genre was added!");
        }



        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> Delete(int id)
        {
            await genreService.DeleteGenre(id);
            return StatusCode(204, "Genre was deleted");
        }
    }
}
