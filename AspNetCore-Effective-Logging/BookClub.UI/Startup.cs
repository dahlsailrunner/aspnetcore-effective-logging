using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookClub.Infrastructure;
using CoreFlogger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BookClub.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IScopeInformation, ScopeInformation>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = "https://demo.identityserver.io";

                    options.ClientId = "interactive.confidential";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.Scope.Add("email");
                    options.Scope.Add("api");
                    options.Scope.Add("offline_access");

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                    options.ClaimActions.MapAllExcept("nbf", "exp", "aud", "nonce", "iat", "c_hash");
                    options.Events.OnTicketReceived = e =>
                    {
                        e.Principal = TransformClaims(e.Principal);
                        return Task.CompletedTask;
                    };
                });

            services.AddRazorPages();
            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                //config.Filters.Add(typeof(TrackPagePerformanceFilter));
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            //app.UseExceptionHandler("/Error");

            app.UseExceptionHandler(eApp =>
            {
                eApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorCtx = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorCtx != null)
                    {
                        var ex = errorCtx.Error;
                        WebHelper.LogWebError("ToDos", "Core API", ex, context);

                        var errorId = Activity.Current?.Id ?? context.TraceIdentifier;
                        var jsonResponse = JsonConvert.SerializeObject(new CustomErrorResponse
                        {
                            ErrorId = errorId,
                            Message = "Some kind of error happened in the API."
                        });
                        await context.Response.WriteAsync(jsonResponse, Encoding.UTF8);
                    }
                });
            });
            app.UseHsts();
            
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        private ClaimsPrincipal TransformClaims(ClaimsPrincipal principal)
        {
            var claims = new List<Claim>();
            claims.AddRange(principal.Claims);  // retain any claims from originally authenticated user
            
            var newIdentity = new ClaimsIdentity(claims, principal.Identity.AuthenticationType, "name", "role");
            return new ClaimsPrincipal(newIdentity);
        }
    }
}
