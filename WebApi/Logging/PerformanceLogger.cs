using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;


namespace WebApi.Logging
{
    public class PerformanceLogger : IDisposable
    {

        private readonly ILogger logger;
        private readonly string message;
        private readonly Stopwatch timer;

        public PerformanceLogger(ILogger logger, string message)
        {
            this.message = message;
            this.logger = logger;
            timer = new Stopwatch();
            timer.Start();
        }


        public void Dispose()
        {
            timer.Stop();
            var ms = timer.ElapsedMilliseconds;

            logger.LogTrace(
                string.Format("{0} - Elapsed Milliseconds: {1}", this.message, ms)
            );
        }
    }
}
