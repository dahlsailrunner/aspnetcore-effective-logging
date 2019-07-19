using BookClub.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookClub.UI.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ILogger<CreateModel> logger)
        {
            _logger = logger;
        }
        [BindProperty]
        public Book Book { get; set; }
        
        public void OnGet()
        {
            Book = new Book();
        }
        
        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            _logger.LogInformation("Submitting new book: {Book}", Book);
            using (var http = new HttpClient(new StandardHttpMessageHandler(HttpContext, _logger)))
            {
                await http.PostAsJsonAsync("https://localhost:44322/apiERROR/Book", Book);
            }
            return RedirectToPage("BookList");
        }
    }
}