using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace BookClub.UI.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            // no authenticated user is required here -- but they MIGHT be auth'd.
            Log.Information("Hello from the Home page.");
        }
    }
}
