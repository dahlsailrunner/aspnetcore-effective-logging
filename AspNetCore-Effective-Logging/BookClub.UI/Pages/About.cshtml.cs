using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace BookClub.UI.Pages
{
    public class AboutModel :PageModel
    {
        public void OnGet()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value;
            Log.Information("UI ENTRY: {UserName} - ({EntryUserId}) is about to call the book api " +
                "to get all books. {Claims}", User?.Identity?.Name, userId, User.Claims);

            Log.Information("Just a sample log message with an authenticated user.");
           
        }
    }
}