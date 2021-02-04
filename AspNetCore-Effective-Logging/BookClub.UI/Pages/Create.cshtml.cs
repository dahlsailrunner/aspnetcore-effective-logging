using BookClub.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BookClub.UI.Pages
{
	public class CreateModel : PageModel
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<CreateModel> _logger;

		public CreateModel( IHttpClientFactory httpClientFactory, ILogger<CreateModel> logger )
		{
			_httpClientFactory = httpClientFactory;
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
			if ( !ModelState.IsValid )
			{
				return Page();
			}
			_logger.LogInformation( "Submitting new book: {Book}", Book );

			await _httpClientFactory.CreateClient( "API" ).PostAsJsonAsync( "https://localhost:44322/apiERROR/Book", Book );

			return RedirectToPage( "BookList" );
		}
	}
}