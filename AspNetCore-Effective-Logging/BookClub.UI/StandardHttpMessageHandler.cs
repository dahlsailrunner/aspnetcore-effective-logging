using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace BookClub.UI
{
    public class StandardHttpMessageHandler : DelegatingHandler
    {
        private readonly HttpContext _httpContext;
        public StandardHttpMessageHandler(HttpContext httpContext)
        {
            _httpContext = httpContext;
            InnerHandler = new SocketsHttpHandler();
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _httpContext.GetTokenAsync("access_token");

            request.Headers.Add("Authorization", $"Bearer {token}");
            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = "";
                var id = "";

                if (response.Content.Headers.ContentLength > 0)
                {
                    var j = JObject.Parse(await response.Content.ReadAsStringAsync());
                    error = (string)j["Title"];
                    id = (string)j["Id"];
                }

                var ex = new Exception("API Failure");

                ex.Data.Add("API Route", $"GET {request.RequestUri}");
                ex.Data.Add("API Status", (int)response.StatusCode);
                if (!string.IsNullOrEmpty(error))
                {
                    ex.Data.Add("API Error", error);
                    ex.Data.Add("API ErrorId", id);
                }
                throw ex;
            }

            return response;
        }
    }
}
