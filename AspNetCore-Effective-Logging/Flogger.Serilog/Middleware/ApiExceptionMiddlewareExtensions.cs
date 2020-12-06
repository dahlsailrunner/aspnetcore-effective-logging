using System;
using Microsoft.AspNetCore.Builder;

namespace Flogger.Serilog.Middleware
{
    public static class ApiExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomApiExceptionHandler(this IApplicationBuilder builder, 
            Action<ApiExceptionOptions> configureOptions)
        {
            var options = new ApiExceptionOptions();
            configureOptions(options);
            
            return builder.UseMiddleware<ApiExceptionMiddleware>(options);
        }
        public static IApplicationBuilder UseCustomApiExceptionHandler(this IApplicationBuilder builder)
        {
            var options = new ApiExceptionOptions();            
            return builder.UseMiddleware<ApiExceptionMiddleware>(options);
        }
    }
}
