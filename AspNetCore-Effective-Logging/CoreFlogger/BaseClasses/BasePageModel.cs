using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using BookClub.Infrastructure;

namespace CoreFlogger.BaseClasses
{
    public class BasePageModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IScopeInformation _scopeInfo;
        private readonly Stopwatch _timer;
        private IDisposable _userScope;
        private IDisposable _hostScope;

        public BasePageModel(ILogger logger, IScopeInformation scopeInfo)
        {
            _logger = logger;                
            _scopeInfo = scopeInfo;           
            _timer = new Stopwatch();
        }
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var userDict = new Dictionary<string, string>
            {
                {"UserId", context.HttpContext.User.FindFirst("sub")?.Value },
                {"GivenName", context.HttpContext.User.FindFirst("given_name")?.Value }
            };
            userDict.Add("Email", MaskEmailAddress(context.HttpContext.User.FindFirst("email")?.Value));

            _userScope = _logger.BeginScope(userDict);
            _hostScope = _logger.BeginScope(_scopeInfo.HostScopeInfo);

            _timer.Start();
        }

        private string MaskEmailAddress(string emailAddress)
        {
            var atIndex = emailAddress?.IndexOf('@');
            if (atIndex > 1)
            {
                return string.Format("{0}{1}***{2}", emailAddress[0], emailAddress[1],
                    emailAddress.Substring(atIndex.Value));
            }
            return emailAddress;            
        }

        public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            _timer.Stop();
            _logger.LogRoutePerformance(context.ActionDescriptor.RelativePath,
                context.HttpContext.Request.Method,
                _timer.ElapsedMilliseconds);

            _userScope?.Dispose();
            _hostScope?.Dispose();
        }
    }
}
