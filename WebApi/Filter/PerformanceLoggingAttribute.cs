using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace WebApi.Filter
{

    public class PerformanceLoggingAttribute : IActionFilter
    {

        private readonly ILogger<PerformanceLoggingAttribute> logger;
        private readonly Stopwatch timer;

        public PerformanceLoggingAttribute(ILogger<PerformanceLoggingAttribute> logger)
        {
            timer = new Stopwatch();
            this.logger = logger;
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


        public void OnActionExecuting(ActionExecutingContext context)
        {
            timer.Start();
        }
    }
}
