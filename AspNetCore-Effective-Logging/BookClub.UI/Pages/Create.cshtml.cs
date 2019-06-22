using BookClub.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookClub.UI.Pages
{
    public class CreateModel : PageModel
    {
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
            using (var http = new HttpClient(new StandardHttpMessageHandler(HttpContext)))
            {
                await http.PostAsJsonAsync("https://localhost:44322/api/Book", Book);
            }
            return RedirectToPage("BookList");
        }
    }
}