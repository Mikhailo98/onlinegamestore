using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/publisher")]
    public class PublisherController : Controller
    {

        private readonly IPublisherService gameService;

        public PublisherController(IPublisherService gameService)
        {
            this.gameService = gameService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePublisherAsync([FromBody]PublisherCreateModel publisher)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await gameService.CreatePublisher(new BusinessLogicLayer.Dtos.PublisherDto() { Name = publisher.Name });
            }
            catch (Exception ex )
            {
                return BadRequest(ex.Message);
            }

            return Ok("Publisher was added");
        }
    }
}