using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BookClub.Infrastructure.Attributes
{
    public class TrackPerformance : ActionFilterAttribute
    {
        private readonly ILogger<TrackPerformance> _logger;
        private readonly Stopwatch _timer;

        public TrackPerformance(ILogger<TrackPerformance> logger)
        {
            _logger = logger;
            _timer = new Stopwatch();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {            
            _timer.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _timer.Stop();
            if (context.Exception == null)
            {
                _logger.LogRoutePerformance(context.HttpContext.Request.Path,
                    context.HttpContext.Request.Method,
                    _timer.ElapsedMilliseconds);
            }
        }
    }
}
