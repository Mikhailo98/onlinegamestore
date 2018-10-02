using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace WebApi.Logging
{
    public class PerformanceLogging : Attribute, IActionFilter
    {

        private readonly ILogger<PerformanceLogging> logger;
        private readonly Stopwatch timer;

        public PerformanceLogging(ILogger<PerformanceLogging> logger)
        {
            timer = new Stopwatch();
            this.logger = logger;
        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            timer.Start();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            timer.Stop();
            var ms = timer.ElapsedMilliseconds;

            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];

            logger.LogTrace(
                string.Format($"controller: {controllerName}, action: {actionName}" +
                $"- Elapsed Milliseconds: {ms}"));
        }
    }
}
