using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BookClub.Infrastructure.Filters
{
    public class TrackPagePerformanceFilter : IPageFilter
    {
        private readonly ILogger<TrackPagePerformanceFilter> _logger;
        private Stopwatch _timer;

        public TrackPagePerformanceFilter(ILogger<TrackPagePerformanceFilter> logger)
        {
            _logger = logger;
        }
        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            _timer = new Stopwatch();
            _timer.Start();
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            _timer.Stop();
            if (context.Exception == null)
            {
                _logger.LogRoutePerformance(context.ActionDescriptor.RelativePath,
                    context.HttpContext.Request.Method,
                    _timer.ElapsedMilliseconds);
            }            
            //_logger.LogInformation("{PageName} {Method} model code took {ElapsedMilliseconds}.",
            //    context.ActionDescriptor.RelativePath, 
            //    context.HttpContext.Request.Method, 
            //    _timer.ElapsedMilliseconds);
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            // not needed
        }
    }
}
