using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Controllers;
using Xunit;

namespace WebApi.Test
{
    public class BasicTests :
        IClassFixture<WebApplicationFactory<Startup>>
    {


        private readonly WebApplicationFactory<Startup> _factory;
        private Mock<IGameService> gamesServiceMoq;
        private Mock<IMapper> mapperMoq;
        private GamesController gamesController;
        private Mock<ILogger<GamesController>> moqLogger;
        private HttpClient client;



        private TestServer _server;
        private HttpClient _client;


        public BasicTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;



            gamesServiceMoq = new Mock<IGameService>();
            mapperMoq = new Mock<IMapper>();
            var m = gamesServiceMoq.Object;

            _server = new TestServer(new WebHostBuilder()
                   .UseStartup<Startup>().ConfigureTestServices(services =>
                   services.AddTransient<IGameService>(p => m)
                   ));
            _client = _server.CreateClient();

        }


        public void SetUp()
        {

        }



        [Theory]
        [InlineData("/api/games")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {

            gamesController = new GamesController
        (gamesServiceMoq.Object, mapperMoq.Object, null, null);

            gamesServiceMoq.Setup(p => p.GetAll()).ReturnsAsync(new List<GameDto>());

            var response = await _client.GetAsync("/api/games");
            response.EnsureSuccessStatusCode();

            // Arrange


            // Act
            //  var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            //Assert.Equal("text/html; charset=utf-8",
            //    response.Content.Headers.ContentType.ToString());
        }
    }
}