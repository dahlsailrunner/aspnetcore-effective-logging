using System;
using Microsoft.AspNetCore.Http;

namespace Flogger.Serilog.Middleware
{
    public class ApiExceptionOptions
    {       
        public Action<HttpContext, Exception, ApiError> AddResponseDetails { get; set; }        
    }
}
