using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookClub.UI.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }
        public string ApiRoute { get; set; }
        public string ApiStatus { get; set; }
        public string ApiErrorId { get; set; }
        public string ApiTitle { get; set; }
        public string ApiDetail { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var exceptionPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var ex = exceptionPathFeature?.Error;
            if (ex.Data.Contains("API Route"))
            {
                ApiRoute = ex.Data["API Route"]?.ToString();
                ApiStatus = ex.Data["API Status"]?.ToString();
                ApiErrorId = ex.Data["API ErrorId"]?.ToString();
                ApiTitle = ex.Data["API Title"]?.ToString();
                ApiDetail = ex.Data["API Detail"]?.ToString();                
            }
        }
    }
}
