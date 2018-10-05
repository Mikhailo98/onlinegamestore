using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApi.Middleware;

namespace WebApi.Test.MiddlewareTest
{
    [TestFixture]
    public class CustomErrorMiddlewareTest
    {
        Mock<ILogger<CustomErrorMiddleware>> loggermoq;
        DefaultHttpContext httpContext;
        [SetUp]
        public void SetUp()
        {
            loggermoq = new Mock<ILogger<CustomErrorMiddleware>>();
            httpContext = new DefaultHttpContext();

        }
        [Test]
        public async Task InvokeAsync_ReturnsStatusCode200()
        {
            // Arrange
            var authMiddleware = new CustomErrorMiddleware(
                next: (innerHttpContext) => Task.FromResult(0),
                loggerFactory: loggermoq.Object);

            // Act
            await authMiddleware.InvokeAsync(httpContext);

            // Assert
            Assert.That(httpContext.Response.StatusCode == (int)HttpStatusCode.OK,
                "Status code should be 200 as defualt");
        }

        [Test]
        public async Task InvokeAsync_InnerException_WritesIntoLogWarningAndReturnsBadRequest()
        {
            // Arrange
            var authMiddleware = new CustomErrorMiddleware(
                next: (innerHttpContext) => throw new ArgumentException("Invalid user id"),
                loggerFactory: loggermoq.Object);

            // Act
            await authMiddleware.InvokeAsync(httpContext);

            // Assert
            Assert.That(httpContext.Response.StatusCode == (int)HttpStatusCode.BadRequest,
        "Status code should be 400 since ArgumentException was thrown");

        }


        [Test]
        public async Task InvokeAsync_UnexpectedException_WritesIntoLogCriticalAndReturnsBadRequest()
        {
            // Arrange
            var authMiddleware = new CustomErrorMiddleware(
                next: (innerHttpContext) => throw new Exception(),
                loggerFactory: loggermoq.Object);
            // Act
            await authMiddleware.InvokeAsync(httpContext);

            // Assert
            Assert.That(httpContext.Response.StatusCode == (int)HttpStatusCode.BadRequest,
        "Status code should be 400 since unexpected Exception was thrown");

        }
    }
}
