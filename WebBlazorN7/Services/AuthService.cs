using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WebBlazorN7.ViewModel;

namespace WebBlazorN7.Services
{
	public class AuthService : IAuthService
	{
		private readonly HttpClient _httpClient;
		private readonly AuthenticationStateProvider _authenticationStateProvider;
		private readonly ILocalStorageService _localStorage;

		public AuthService(HttpClient httpClient,
						   AuthenticationStateProvider authenticationStateProvider,
						   ILocalStorageService localStorage)
		{
			_httpClient = httpClient;
			_authenticationStateProvider = authenticationStateProvider;
			_localStorage = localStorage;
		}

		public async Task<Tuple<LoginResponse, Erro>> Login(LoginRequest loginModel)
		{
			try
			{
				var loginAsJson = JsonSerializer.Serialize(loginModel);
				var response = await _httpClient.PostAsync("/api/v3/Auth/login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));

				if (!response.IsSuccessStatusCode)
				{
					var erro = await response.Content.ReadFromJsonAsync<Erro>();
					if (erro is not null)
						return new Tuple<LoginResponse, Erro>(null, erro);
				}

				var loginResult = JsonSerializer.Deserialize<LoginResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				if (loginResult == null)
					return new Tuple<LoginResponse, Erro>(null, null);

				await _localStorage.SetItemAsync("authToken", loginResult.accessToken);
				await _localStorage.SetItemAsync("refreshToken", loginResult.refreshToken);

				((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email);
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.accessToken);

				return new Tuple<LoginResponse, Erro>(loginResult, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<LoginResponse, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}

		public async Task<Tuple<LoginResponse, Erro>> RefreshToken()
		{
			var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

			try
			{
				var response = await _httpClient.PostAsync($"/api/v3/Auth/refresh-token-guid/{refreshToken}", null);

				if (!response.IsSuccessStatusCode)
				{
					((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();

					var erro = await response.Content.ReadFromJsonAsync<Erro>();
					if (erro is not null)
						return new Tuple<LoginResponse, Erro>(null, erro);
				}

				var loginResult = JsonSerializer.Deserialize<LoginResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				if (loginResult == null)
					return new Tuple<LoginResponse, Erro>(null, null);

				await _localStorage.SetItemAsync("authToken", loginResult.accessToken);
				await _localStorage.SetItemAsync("refreshToken", loginResult.refreshToken);

				((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginResult.usuarioToken.email);
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.accessToken);

				return new Tuple<LoginResponse, Erro>(loginResult, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<LoginResponse, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}

		public async Task Logout()
		{
			await _localStorage.RemoveItemAsync("authToken");
			await _localStorage.RemoveItemAsync("refreshToken");

			((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
			_httpClient.DefaultRequestHeaders.Authorization = null;
		}
	}
}
