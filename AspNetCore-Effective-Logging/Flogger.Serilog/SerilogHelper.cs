using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Flogger.Serilog
{
    public static class SerilogHelper
    {
        public static void WithCustomConfiguration(this LoggerConfiguration loggerConfig,
            IConfiguration config, string appName)
        {
            loggerConfig
                .ReadFrom.Configuration(config) // minimum levels defined per project in json files 
                .Enrich.WithProperty("ApplicationName", appName)
                .Enrich.FromLogContext();

            ConfigureSinkOptions(loggerConfig, config);
        }

        public static IApplicationBuilder UseCustomSerilogRequestLogging(this IApplicationBuilder app, AssemblyName nameInfo)
        {
            return app.UseSerilogRequestLogging(options =>
            {
                options.GetLevel = ExcludeHealthChecks;
                options.EnrichDiagnosticContext = (diagCtx, httpContext) =>
                {
                    diagCtx.Set("RequestHost", httpContext.Request.Host.Value);
                    diagCtx.Set("RequestScheme", httpContext.Request.Scheme);
                    diagCtx.Set("Assembly", nameInfo.Name);
                    diagCtx.Set("Version", nameInfo.Version);
                    diagCtx.Set("Machine", Environment.MachineName);
                    var user = httpContext.User.Identity;
                    if (user != null && user.IsAuthenticated)
                    {
                        // include any / all claims that you want to log here....
                        var ci = user as ClaimsIdentity;
                        var email = ci?.Claims.FirstOrDefault(a => a.Type == "email")?.Value;
                        diagCtx.Set("UserName", email);
                        diagCtx.Set("UserId", ci?.Claims.FirstOrDefault(a => a.Type == "sub")?.Value);
                    }
                };
            });
        }


        private static void ConfigureSinkOptions(LoggerConfiguration loggerConfig, IConfiguration config)
        {
            var seqUri = config.GetValue<string>("Logging:SeqUri");
            var usingCustomSink = config.GetValue<bool>("Logging:UsingCustomSink");

            if (string.IsNullOrEmpty(seqUri) && !usingCustomSink)
            {
                throw new Exception("Logging:SeqUri or a WriteTo sink must be set in the appSettings.json file.");
            }

            if (!string.IsNullOrEmpty(seqUri))
            {
                loggerConfig.WriteTo.Seq(seqUri);
            }
        }

        private static LogEventLevel ExcludeHealthChecks(HttpContext ctx, double _, Exception ex) =>
            ex != null
                ? LogEventLevel.Error
                : ctx.Response.StatusCode > 499
                    ? LogEventLevel.Error
                    : IsHealthCheckEndpoint(ctx) // Not an error, check if it was a health check
                        ? LogEventLevel.Verbose // Was a health check, use Verbose
                        : LogEventLevel.Information;

        private static bool IsHealthCheckEndpoint(HttpContext ctx)
        {
            return false;
        }
    }
}
