using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Middleware
{
    public class HttpStatusCodeExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpStatusCodeExceptionMiddleware> _logger;



        public HttpStatusCodeExceptionMiddleware(RequestDelegate next, ILogger<HttpStatusCodeExceptionMiddleware> loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            HttpStatusCode code;
            string message;
            dynamic details;


            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                var headers = string.Join(Environment.NewLine, context.Request.Headers.Select(x => $"{x.Key}:{x.Value}"));

                code = HttpStatusCode.BadRequest;

                _logger.LogWarning(ex, LogMessageComposer.Compose(
                    new
                    {
                        user = context.User.Identity?.Name ?? "anonymous",
                        message = ex.Message,
                        parameter = ex.ParamName,
                        method = ex.TargetSite.ReflectedType.FullName,
                    }));


                await context.Response.WriteAsync(new UserErrorDetails()
                {
                    StatusCode = (int)code,
                    Message = $"{ex.Message}"
                }.ToString());

            }
            catch (Exception ex)
            {
                code = HttpStatusCode.InternalServerError;


                _logger.LogCritical(ex,
                         LogMessageComposer.Compose(
                    new
                    {
                        user = context.User.Identity.Name,
                        message = ex.Message,
                        targetsite = ex.TargetSite,
                        exception = ex,
                        uri = context.Request.Path,
                        method = context.Request.Method,
                  
                    }));

                await context.Response.WriteAsync(new UserErrorDetails()
                {
                    StatusCode = (int)code,
                    Message = "An error occurred. Please try again."
                }.ToString());
            }
        }

    }

}
