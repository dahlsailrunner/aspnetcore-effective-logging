using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BookClub.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace BookClub.UI.Pages
{
    public class BookListModel : PageModel
    {
        public List<Book> Books;

        public async Task OnGetAsync()
        {
            using (var http = new HttpClient(new StandardHttpMessageHandler(HttpContext)))
            {
                var response = await http.GetAsync("https://localhost:44322/api/Book");
                Books = JsonConvert.DeserializeObject<List<Book>>(await response.Content.ReadAsStringAsync());
            }
        }
    }
}