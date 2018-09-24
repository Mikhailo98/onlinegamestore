using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models.Dtos.PublisherDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


        [HttpGet("{id}/games")]
        public async Task<IActionResult> GetAllGamesOfPublisher(int id)
        {
            List<GameDto> commentDtos = await publisherService.GetGamesOfPublisher(id);

            return Ok(commentDtos);

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            if (id <= 0)
            {
                return StatusCode(400, "Invalid Publisher Id");
            }

            try
            {
                PublisherDto genre = await publisherService.GetInfo(id);
                return Ok(genre);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreatePublisherAsync([FromBody]PublisherCreateModel publisher)
        {

            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            try
            {
                await publisherService.CreatePublisher(new CreatePublisherDto() { Name = publisher.Name });
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return StatusCode(201, "Publisher was added");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]PublisherCreateModel value)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            EditPublisherDto editedPubliisher = new EditPublisherDto()
            {
                Id = id,
                Name = value.Name,
            };

            await publisherService.EditPublisher(editedPubliisher);

            return StatusCode(200,  "Publisher was updated");
        }

    }
}