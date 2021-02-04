using BookClub.Infrastructure;
using BookClub.Infrastructure.BaseClasses;
using BookClub.Logic.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookClub.UI.Pages
{
	public class BookListBadModel : BasePageModel
	{
		private readonly IHttpClientFactory _httpClientFactory;
		public List<BookModel> Books;

		public BookListBadModel( IHttpClientFactory httpClientFactory, ILogger<BookListModel> logger, IScopeInformation scope ) : base( logger, scope )
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task OnGetAsync()
		{
			var response = await _httpClientFactory.CreateClient( "API" ).GetAsync( "https://localhost:44322/api/Book?throwException=true" );
			Books = JsonConvert.DeserializeObject<List<BookModel>>(
				await response.Content.ReadAsStringAsync() ).OrderByDescending( a => a.Id ).ToList();
		}
	}
}