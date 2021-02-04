using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BookClub.Infrastructure.Services
{
	public class EnsureSuccessStatusCodeHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
		{
			var response = await base.SendAsync( request, cancellationToken );

			response.EnsureSuccessStatusCode();

			return response;
		}
	}
}
