using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookClub.Logic.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace BookClub.UI.Pages
{
    public class BookListBadModel : PageModel
    {
        public List<BookModel> Books;

        public async Task OnGetAsync()
        {            
            using (var http = new HttpClient(new StandardHttpMessageHandler(HttpContext)))
            {
                var response = await http.GetAsync("https://localhost:44322/api/Book?throwException=true");
                Books = JsonConvert.DeserializeObject<List<BookModel>>(
                    await response.Content.ReadAsStringAsync()).OrderByDescending(a=> a.Id).ToList();                
            }
        }       
    }
}