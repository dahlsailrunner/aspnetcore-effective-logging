using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BookClub.API
{
    public class SwaggerConfig : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IConfiguration _config;

        public SwaggerConfig(IConfiguration config)
        {
            _config = config;
        }
        public void Configure(SwaggerGenOptions options)
        {
            var disco = GetDiscoveryDocument();
            var oauthScopeDic = new Dictionary<string, string> { { "api", "Access to the Book Club API" } };

            //options.OperationFilter<AuthorizeOperationFilter>();
            options.DescribeAllParametersInCamelCase();
            options.CustomSchemaIds(x => x.FullName);
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Book Club API", Version = "v1" });

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(disco.AuthorizeEndpoint),
                        TokenUrl = new Uri(disco.TokenEndpoint),
                        Scopes = oauthScopeDic
                    }
                }
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
                    },
                    oauthScopeDic.Keys.ToArray()
                }
            });
        }

        private DiscoveryDocumentResponse GetDiscoveryDocument()
        {
            var client = new HttpClient();
            var authority = _config.GetValue<string>("Security:Authority");
            return client.GetDiscoveryDocumentAsync(authority)
                .GetAwaiter()
                .GetResult();
        }
    }
}
