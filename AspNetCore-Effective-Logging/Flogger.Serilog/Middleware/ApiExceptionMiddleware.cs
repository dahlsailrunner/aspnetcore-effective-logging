using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace Flogger.Serilog.Middleware
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiExceptionOptions _options;

        public ApiExceptionMiddleware(ApiExceptionOptions options, RequestDelegate next)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _options);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ApiExceptionOptions opts)
        {            
            var error = new ApiError
            {
                Id = Guid.NewGuid().ToString(),
                Status = (short) HttpStatusCode.InternalServerError,
                Title = "Some kind of error occurred in the API.  Please use the id and contact our support team if the problem persists."
            };

            opts.AddResponseDetails?.Invoke(context, exception, error);
            
            Log.ForContext("ErrorId", error.Id)
               .Error(exception, "An exception was caught in the API request pipeline");

            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }       
    }
}
