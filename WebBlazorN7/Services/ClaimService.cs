using System.Net.Http.Json;
using System.Xml.Linq;
using WebBlazorN7.ViewModel;

namespace WebBlazorN7.Services
{
	public class ClaimService : IClaimService
	{
		public HttpClient _httpClient;

		public ClaimService(HttpClient client)
		{
			_httpClient = client;
		}

		public async Task<Tuple<ClaimViewModel, Erro>> Add(ClaimViewModel claimViewModel)
		{
			try
			{
				using var response = await _httpClient.PostAsJsonAsync($"/api/v3/Claims/claim", claimViewModel);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var claim = await response.Content.ReadFromJsonAsync<ClaimViewModel>();
					if (claim is not null)
						return new Tuple<ClaimViewModel, Erro>(claim, null);
				}
				else
				{
					var erro = await response.Content.ReadFromJsonAsync<Erro>();
					if (erro is not null)
						return new Tuple<ClaimViewModel, Erro>(null, erro);
				}

				return new Tuple<ClaimViewModel, Erro>(null, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<ClaimViewModel, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}

		public async Task<Erro> PutType(string oldType, string newType)
		{
			try
			{
				var newClaim = new { oldType, newType };
				var response = await _httpClient.PutAsJsonAsync($"/api/v3/Claims/claimType", newClaim);

				if (!response.IsSuccessStatusCode)
				{
					var erro = await response.Content.ReadFromJsonAsync<Erro>();
					if (erro is not null)
						return erro;
				}

				return null;
			}
			catch (HttpRequestException ex)
			{
				return new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } };
			}
		}

		public async Task<Erro> PutValue(int userId, string type, string value)
		{
			try
			{
				var newClaim = new { value };
				var response = await _httpClient.PutAsJsonAsync($"/api/v3/Claims/claim/{type}/user/{userId}", newClaim);

				if (!response.IsSuccessStatusCode)
				{
					var erro = await response.Content.ReadFromJsonAsync<Erro>();
					if (erro is not null)
						return erro;
				}

				return null;
			}
			catch (HttpRequestException ex)
			{
				return new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } };
			}
		}

		public async Task<Erro> DeleteType(string type)
		{
			try
			{
				var response = await _httpClient.DeleteAsync($"/api/v3/Claims/claim/{type}");

				if (!response.IsSuccessStatusCode)
				{
					var erro = await response.Content.ReadFromJsonAsync<Erro>();
					if (erro is not null)
						return erro;
				}

				return null;
			}
			catch (HttpRequestException ex)
			{
				return new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } };
			}
		}

		public async Task<Erro> DeleteTypeUser(string type, int userId)
		{
			try
			{
				var response = await _httpClient.DeleteAsync($"/api/v3/Claims/claim/{type}/user/{userId}");

				if (!response.IsSuccessStatusCode)
				{
					var erro = await response.Content.ReadFromJsonAsync<Erro>();
					if (erro is not null)
						return erro;
				}

				return null;
			}
			catch (HttpRequestException ex)
			{
				return new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } };
			}
		}

		public async Task<Tuple<List<ClaimViewModel>, Erro>> Get()
		{
			try
			{
				var claims = await _httpClient.GetFromJsonAsync<List<ClaimViewModel>>("/api/v3/Claims/claims");
				return new Tuple<List<ClaimViewModel>, Erro>(claims, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<List<ClaimViewModel>, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}

		public async Task<Tuple<List<ClaimUserViewModel>, Erro>> GetUsersByClaim(string type)
		{
			try
			{
				var caimUsers = await _httpClient.GetFromJsonAsync<List<ClaimUserViewModel>>($"/api/v3/Claims/claim/{type}/users");
				return new Tuple<List<ClaimUserViewModel>, Erro>(caimUsers, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<List<ClaimUserViewModel>, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}
	}
}
