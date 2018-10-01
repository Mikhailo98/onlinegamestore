using AutoMapper;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Models;

namespace WebApi.Test.CommentControllerTest
{
    [TestFixture]
    public class CommentControllerTest
    {
        private Mock<ICommentService> commentControllerMoq;
        private Mock<IMapper> mapperMoq;
        private CommentController commentController;


        public CommentControllerTest()
        {

        }

        [SetUp]
        public void SetUp()
        {
            commentControllerMoq = new Mock<ICommentService>();
            mapperMoq = new Mock<IMapper>();
            commentController = new CommentController
                (commentControllerMoq.Object, mapperMoq.Object);

        }

        [Test]
        public async Task CreateAnswerForAnotherComment_ReceiveNegativeId_ReturnStatusCode400()
        {
            CreateAnswerModel model = new CreateAnswerModel()
            {
                GameId = 1,
                Name = "Mike",
                Body = "Body"
            };

            IActionResult actionResult = await commentController.CreateAnswerForAnotherComment(-1, model);
            var okResult = actionResult as ObjectResult;


            Assert.IsNotNull(okResult);
            Assert.AreEqual(400, okResult.StatusCode);
        }


        [Test]
        public async Task CreateAnswerForAnotherComment_ReceiveValidIDandInvalidComment_ReturnInvalidModelState()
        {
            CreateAnswerModel model = new CreateAnswerModel()
            {
                Body = "Body",
                Name = null,
                GameId = 0,
                
            };

            IActionResult actionResult = await commentController.CreateAnswerForAnotherComment(1, model);
            var okResult = actionResult as ObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(400, okResult.StatusCode);
        }


    }
}
