using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace CoreFlogger
{
    public static class WebHelper
    {
        public static void LogWebUsage(string product, string layer, string activityName,
            HttpContext context, Dictionary<string, object> additionalInfo = null)
        {
            var details = GetWebFlogDetail(product, layer, activityName, context, additionalInfo);            
            Flogger.WriteUsage(details);            
        }

        public static void LogWebDiagnostic(string product, string layer, string message,
            HttpContext context, Dictionary<string, object> diagnosticInfo = null)
        {
            var details = GetWebFlogDetail(product, layer, message, context, diagnosticInfo);
            Flogger.WriteDiagnostic(details);            
        }
        public static void LogWebError(string product, string layer, Exception ex, 
            HttpContext context)
        {
            var details = GetWebFlogDetail(product, layer, null, context, null);
            details.Exception = ex;

            Flogger.WriteError(details);
        }

        public static FlogDetail GetWebFlogDetail(string product, string layer, 
            string activityName, HttpContext context, 
            Dictionary<string, object> additionalInfo = null)
        {
            var detail = new FlogDetail
            {
                Product = product,
                Layer = layer,
                Message = activityName,
                Hostname = Environment.MachineName,
                CorrelationId = Activity.Current?.Id ?? context.TraceIdentifier,
                AdditionalInfo = additionalInfo ?? new Dictionary<string, object>()
            };

            GetUserData(detail, context);
            GetRequestData(detail, context);
            // Session data??
            // Cookie data??

            return detail;
        }

        private static void GetRequestData(FlogDetail detail, HttpContext context)
        {
            var request = context.Request;
            if (request != null)
            {
                detail.Location = request.Path;
                                      
                detail.AdditionalInfo.Add("UserAgent", request.Headers["User-Agent"]);
                // non en-US preferences here??
                detail.AdditionalInfo.Add("Languages", request.Headers["Accept-Language"]);  

                var qdict = Microsoft.AspNetCore.WebUtilities
                    .QueryHelpers.ParseQuery(request.QueryString.ToString());
                foreach (var key in qdict.Keys)
                {
                    detail.AdditionalInfo.Add($"QueryString-{key}", qdict[key]);
                }
                
            }
        }        

        private static void GetUserData(FlogDetail detail, HttpContext context)
        {
            var userId = "";
            var userName = "";            
            var user = context.User;  // ClaimsPrincipal.Current is not sufficient
            if (user != null)
            {
                var i = 1; // i included in dictionary key to ensure uniqueness
                foreach (var claim in user.Claims)
                {
                    if (claim.Type == ClaimTypes.NameIdentifier)
                        userId = claim.Value;
                    else if (claim.Type == "name")
                        userName = claim.Value;
                    else
                        // example dictionary key: UserClaim-4-role 
                        detail.AdditionalInfo.Add($"UserClaim-{i++}-{claim.Type}", claim.Value);
                }
            }
            detail.UserId = userId;
            detail.UserName = userName;
        }
    }
}
