using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BookClub.Infrastructure.Middleware;
using BookClub.Data;
using BookClub.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using BookClub.Infrastructure.Filters;
using BookClub.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

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

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://demo.identityserver.io";
                    options.ApiName = "api";                    
                });

            services.AddAuthorization();

            services.AddSwaggerGen(c =>
            {
                var oauthScopeDic = new Dictionary<string, string> { {"api", "Access to the Book Club API"} };
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Book Club API", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://demo.identityserver.io/connect/authorize"),
                            Scopes = oauthScopeDic
                        }
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
                        },
                        oauthScopeDic.Keys.ToArray()
                    }
                });
            });

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
                options.OAuthClientId("implicit");  // should represent the swagger UI
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
