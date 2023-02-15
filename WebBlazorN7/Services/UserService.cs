using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WebBlazorN7.Pages;
using WebBlazorN7.ViewModel;
using static System.Net.WebRequestMethods;

namespace WebBlazorN7.Services
{
	public class UserService : IUserService
	{
		public HttpClient _httpClient;
		private readonly ILocalStorageService _localStorage;

		public UserService(HttpClient client, ILocalStorageService localStorage)
		{
			_httpClient = client;
			_localStorage = localStorage;
		}

		public async Task<Tuple<RegistroResponse, Erro>> Register(RegistroRequest registro)
		{
			try
			{
				using var response = await _httpClient.PostAsJsonAsync("/api/v3/User/register", registro);

				if (response.StatusCode == System.Net.HttpStatusCode.Created)
				{
					var usuario = await response.Content.ReadFromJsonAsync<RegistroResponse>();
					if (usuario is not null)
						return new Tuple<RegistroResponse, Erro>(usuario, null);
				}
				else
				{
					var erro = await response.Content.ReadFromJsonAsync<Erro>();
					if (erro is not null)
						return new Tuple<RegistroResponse, Erro>(null, erro);
				}

				return new Tuple<RegistroResponse, Erro>(null, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<RegistroResponse, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}

		public async Task<Tuple<List<Usuario>, Erro>> Get()
		{
			try
			{
				//var token = await _localStorage.GetItemAsync<string>("authToken");
				//_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var usuarios = await _httpClient.GetFromJsonAsync<List<Usuario>>("/api/v3/User/users");
				return new Tuple<List<Usuario>, Erro>(usuarios, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<List<Usuario>, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}

		public async Task<Tuple<Usuario, Erro>> Get(int id)
		{
			try
			{
				var usuario = await _httpClient.GetFromJsonAsync<Usuario>($"/api/v3/User/user/{id}");
				return new Tuple<Usuario, Erro>(usuario, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<Usuario, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}

		public async Task<Erro> Put(string email, Usuario usuario)
		{
			try
			{
				var response = await _httpClient.PutAsJsonAsync<Usuario>($"/api/v3/User/update/{email}", usuario);

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

		public async Task<Erro> Delete(string email)
		{
			try
			{
				var response = await _httpClient.DeleteAsync($"/api/v3/User/userbyemail/{email}");

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

		public async Task<Tuple<List<string>, Erro>> GetRoles(int usuarioId)
		{
			try
			{
				var roles = await _httpClient.GetFromJsonAsync<List<string>>($"/api/v3/User/user/{usuarioId}/roles");
				return new Tuple<List<string>, Erro>(roles, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<List<string>, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}

		public async Task<Erro> AddRole(int usuarioId, int roleId)
		{
			try
			{
				var roles = await _httpClient.PostAsync($"/api/v3/User/user/{usuarioId}/role/{roleId}", null);
				return null;
			}
			catch (HttpRequestException ex)
			{
				return new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } };
			}
		}

		public async Task<Erro> DeleteRole(int usuarioId, int roleId)
		{
			try
			{
				var roles = await _httpClient.DeleteAsync($"/api/v3/User/user/{usuarioId}/role/{roleId}");
				return null;
			}
			catch (HttpRequestException ex)
			{
				return new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } };
			}
		}

		public async Task<Tuple<List<ClaimViewModel>, Erro>> GetClaims(int usuarioId)
		{
			try
			{
				var roles = await _httpClient.GetFromJsonAsync<List<ClaimViewModel>>($"/api/v3/User/user/{usuarioId}/claims");
				return new Tuple<List<ClaimViewModel>, Erro>(roles, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<List<ClaimViewModel>, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}

		public async Task<Erro> AddClaim(int usuarioId, string type, string value)
		{
			try
			{
				var roles = await _httpClient.PostAsync($"/api/v3/User/user/{usuarioId}/claim/{type}/{value}", null);
				return null;
			}
			catch (HttpRequestException ex)
			{
				return new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } };
			}
		}

		public async Task<Erro> PutValueClaim(int usuarioId, string type, string value)
		{
			try
			{
				var roles = await _httpClient.PutAsync($"/api/v3/User/user/{usuarioId}/claim/{type}/{value}", null);
				return null;
			}
			catch (HttpRequestException ex)
			{
				return new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } };
			}
		}

		public async Task<Erro> DeleteClaim(int usuarioId, string type)
		{
			try
			{
				var roles = await _httpClient.DeleteAsync($"/api/v3/User/user/{usuarioId}/claim/{type}");
				return null;
			}
			catch (HttpRequestException ex)
			{
				return new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } };
			}
		}
	}
}
