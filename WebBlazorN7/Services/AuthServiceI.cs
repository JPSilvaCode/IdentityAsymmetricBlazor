using System.Net.Http.Json;
using WebBlazorN7.ViewModel;

namespace WebBlazorN7.Services
{
	public class AuthServiceI
	{
		public HttpClient _httpClient;

		public AuthServiceI(HttpClient client)
		{
			_httpClient = client;
		}

		public async Task<Tuple<LoginResponse, Erro>> Login(string email, string password)
		{
			var postBody = new { email, password };
			using var response = await _httpClient.PostAsJsonAsync("/api/v3/Auth/login", postBody);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				var usuario = await response.Content.ReadFromJsonAsync<LoginResponse>();
				if (usuario is not null)
					return new Tuple<LoginResponse, Erro>(usuario, null);
			}
			else
			{
				var erro = await response.Content.ReadFromJsonAsync<Erro>();
				if (erro is not null)
					return new Tuple<LoginResponse, Erro>(null, erro);
			}

			return new Tuple<LoginResponse, Erro>(null, null);
		}		
	}
}
