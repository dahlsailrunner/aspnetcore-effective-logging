using System;
using System.Data;
using System.Data.SqlClient;
//using BookClub.Infrastructure.Middleware;
using BookClub.Data;
using BookClub.Infrastructure.Filters;
using BookClub.Infrastructure.Middleware;
using BookClub.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using BookClub.Infrastructure.Filters;
//using BookClub.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using CoreFlogger;

namespace BookClub.API
{
    public class Startup
    {
        //private readonly ILoggerFactory _loggerFactory;

        //public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //_loggerFactory = loggerFactory;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IScopeInformation, ScopeInformation>();

            services.AddScoped<IDbConnection, SqlConnection>(p =>
                new SqlConnection(Configuration.GetConnectionString("BookClubDb")));
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBookLogic, BookLogic>();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfig>();

            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration.GetValue<string>("Security:Authority");
                    options.Audience = Configuration.GetValue<string>("Security:Audience");
                });

            services.AddAuthorization();

            services.AddSwaggerGen();  // configured in SwaggerConfig by transient dependency above

            services.AddMvc(options =>
            {
                var builder = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser();
                options.Filters.Add(new AuthorizeFilter(builder.Build()));
                options.Filters.Add(typeof(TrackActionPerformanceFilter));
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseApiExceptionHandler(options =>
            {
                options.AddResponseDetails = UpdateApiErrorResponse;
                options.DetermineLogLevel = DetermineLogLevel;
            });
            app.UseHsts();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Club API");
                options.OAuthClientId(Configuration.GetValue<string>("Security:ClientId"));  
                options.OAuthClientSecret(Configuration.GetValue<string>("Security:ClientSecret"));
                options.OAuthAppName("Book Club API");
                options.OAuthUsePkce();
            });
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private LogLevel DetermineLogLevel(Exception ex)
        {
            if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) ||
                ex.Message.StartsWith("a network-related", StringComparison.InvariantCultureIgnoreCase))
            {
                return LogLevel.Critical;
            }
            return LogLevel.Error;
        }

        private void UpdateApiErrorResponse(HttpContext context, Exception ex, ApiError error)
        {
            if (ex.GetType().Name == nameof(SqlException))
            {
                error.Detail = "Exception was a database exception!";
            }
            //error.Links = "https://gethelpformyerror.com/";
        }
    }
}
