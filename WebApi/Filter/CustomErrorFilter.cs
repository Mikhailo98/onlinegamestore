using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Filter
{
    public class CustomErrorFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _log;

        public CustomErrorFilter(ILogger log)
        {
            _log = log;
        }

        public bool AllowMultiple => true;

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                WrapAndLogException(context);

            });
        }
                    

        private void WrapAndLogException(ExceptionContext context)
        {
            var exception = context.Exception;
            HttpStatusCode code;
            string message;
            dynamic details = null;

            if (exception is CustomApiException customException)
            {
                code = customException.Code;
                message = customException.Message;
                details = customException.Fields;
            }

            else
            {
                code = HttpStatusCode.InternalServerError;
                var r = context.Result;
                var headers = string.Join(Environment.NewLine, context.HttpContext.Request.Headers.Select(x => $"{x.Key}:{x.Value}"));
                _log.Error(exception,
                    LogMessageComposer.Compose(
                    new
                    {
                        details = "Http request failed",
                        user = "Anonimous",
                        url = context.RouteData.Values,
                        //TODO: Log here other usefull information which can be retrieved.
                    }));

                message = "An error occurred. Please try again.";
            }

            context.Result = new ContentResult();

        }
    }
}
