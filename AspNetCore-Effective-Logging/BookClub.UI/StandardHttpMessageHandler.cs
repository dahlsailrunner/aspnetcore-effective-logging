﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Serilog;

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
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            var token = await _httpContext.GetTokenAsync("access_token");

            request.Headers.Add("Authorization", $"Bearer {token}");
            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var error = JObject.Parse(jsonContent);
                string errorId = null, errorTitle = null, errorDetail = null;
                if (error != null)
                {
                    errorId = error["Id"]?.ToString();
                    errorTitle = error["Title"]?.ToString();
                    errorDetail = error["Detail"]?.ToString();
                }
                var ex = new Exception("API Failure");

                ex.Data.Add("API Route", $"GET {request.RequestUri}");
                ex.Data.Add("API Status", (int)response.StatusCode);
                ex.Data.Add("API ErrorId", errorId);
                ex.Data.Add("API Title", errorTitle);
                ex.Data.Add("API Detail", errorDetail);

                //_logger.LogWarning("API Error when calling {APIRoute}: {APIStatus},", $"GET {request.RequestUri}",
                //    (int)response.StatusCode);
                Log.Warning("API Error when calling {APIRoute}: {APIStatus}," +
                    " {ApiErrorId} - {Title} - {Detail}", 
                    $"GET {request.RequestUri}", (int)response.StatusCode,
                    errorId, errorTitle, errorDetail);
                throw ex;
            }
            return response;
        }
    }
}
