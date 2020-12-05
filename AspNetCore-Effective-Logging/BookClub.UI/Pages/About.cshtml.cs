using CoreFlogger;
using CoreFlogger.BaseClasses;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace BookClub.UI.Pages
{
    public class AboutModel : BasePageModel
    {
        private readonly ILogger<AboutModel> _logger;

        public AboutModel(ILogger<AboutModel> logger, IScopeInformation scope) : base(logger, scope)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value;
            _logger.LogInformation("UI ENTRY: {UserName} - ({EntryUserId}) is about to call the book api " +
                "to get all books. {Claims}", User.Identity.Name, userId, User.Claims);
           
        }
    }
}