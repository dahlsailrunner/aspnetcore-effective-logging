using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BookClub.Infrastructure.Middleware
{
	public class ApiExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ApiExceptionMiddleware> _logger;
		private readonly ApiExceptionOptions _options;

		public ApiExceptionMiddleware( ApiExceptionOptions options, RequestDelegate next,
			ILogger<ApiExceptionMiddleware> logger )
		{
			_next = next;
			_logger = logger;
			_options = options;
		}

		public async Task Invoke( HttpContext context /* other dependencies */)
		{
			try
			{
				await _next( context );
			}
			catch ( Exception ex )
			{
				HandleExceptionAsync( context, ex );
				throw; // Just let exception be thrown so caller can process it
			}
		}

		private void HandleExceptionAsync( HttpContext context, Exception exception )
		{
			var error = new ApiError
			{
				Id = Guid.NewGuid().ToString(),
				Status = (short)HttpStatusCode.InternalServerError,
				Title = "Some kind of error occurred in the API.  Please use the id and contact our " +
						"support team if the problem persists."
			};

			_options.AddResponseDetails?.Invoke( context, exception, error );

			var innerExMessage = GetInnermostExceptionMessage( exception );

			var level = _options.DetermineLogLevel?.Invoke( exception ) ?? LogLevel.Error;
			_logger.Log( level, exception, "BADNESS!!! " + innerExMessage + " -- {ErrorId}.", error.Id );

			// No longer returning 'text' to caller.  Will let exception throw.
			/*
            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
			*/
		}

		private string GetInnermostExceptionMessage( Exception exception )
		{
			if ( exception.InnerException != null )
				return GetInnermostExceptionMessage( exception.InnerException );

			return exception.Message;
		}
	}
}
