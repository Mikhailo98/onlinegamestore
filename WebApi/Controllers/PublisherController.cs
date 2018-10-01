using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models.Dtos.PublisherDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Infrastucture;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/publishers")]
    public class PublisherController : Controller
    {

        private readonly IPublisherService publisherService;
        private readonly IMapper mapper;


        public PublisherController(IPublisherService gameService, IMapper mapper)
        {
            this.publisherService = gameService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<PublisherDto> dto = await publisherService.GetAll();
            return Ok(dto);

        }


        [HttpGet("{id:int:min(1)}/games")]
        public async Task<IActionResult> GetAllGamesOfPublisher(int id)
        {
            List<GameDto> commentDtos = await publisherService.GetGamesOfPublisher(id);
            return StatusCode(200, commentDtos);

        }


        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> Get(int id)
        {
            PublisherDto genre = await publisherService.GetInfo(id);
            return StatusCode(200, genre);

        }

        [HttpPost]
        [CustomValidation]
        public async Task<IActionResult> CreatePublisherAsync([FromBody]PublisherCreateModel publisher)
        {
            await publisherService.CreatePublisher(new CreatePublisherDto() { Name = publisher.Name });
            return StatusCode(201, "Publisher was added");
        }



        [HttpPut("{id:int:min(1)}")]
        [CustomValidation]
        public async Task<IActionResult> Put(int id, [FromBody]PublisherCreateModel value)
        {
            EditPublisherDto editedPubliisher = new EditPublisherDto()
            {
                Id = id,
                Name = value.Name,
            };

            await publisherService.EditPublisher(editedPubliisher);
            return StatusCode(200, "Publisher was updated");
        }

    }
}