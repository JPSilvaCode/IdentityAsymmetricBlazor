using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using WebBlazorN7.Services;
using WebBlazorN7.ViewModel;

namespace WebBlazorN7
{
	public class ApiAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly HttpClient _httpClient;
		private readonly ILocalStorageService _localStorage;

		public ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
		{
			_httpClient = httpClient;
			_localStorage = localStorage;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var savedToken = await _localStorage.GetItemAsync<string>("authToken");

			if (string.IsNullOrWhiteSpace(savedToken))
				return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

			var claims = ParseClaimsFromJwt(savedToken);
			var email = string.Empty;
			if (claims.Count(c => c.Type.Equals("email")).Equals(1))
			{
				email = claims.SingleOrDefault(c => c.Type.Equals("email"))?.Value;
				if (string.IsNullOrEmpty(email))
					return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
			}

			//if (!AuthenticationService.CheckTokenIsValid(savedToken))
			//{
			//	return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

			//var retorno = await _authService.RefreshToken(savedToken);
			//LoginResponse usuario = retorno.Item1;
			//if (usuario is not null)
			//{
			//	await _localStorage.SetItemAsync("authToken", usuario.accessToken);
			//	savedToken = await _localStorage.GetItemAsync<string>("authToken");
			//}
			//}

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

			var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new System.Security.Claims.Claim(ClaimTypes.Name, email) }, "apiauth"));
			var authState = new AuthenticationState(authenticatedUser);

			return authState;
		}

		public void MarkUserAsAuthenticated(string email)
		{
			var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new System.Security.Claims.Claim(ClaimTypes.Name, email) }, "apiauth"));
			var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
			NotifyAuthenticationStateChanged(authState);
		}

		public void MarkUserAsLoggedOut()
		{
			var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
			var authState = Task.FromResult(new AuthenticationState(anonymousUser));
			NotifyAuthenticationStateChanged(authState);
		}

		private IEnumerable<System.Security.Claims.Claim> ParseClaimsFromJwt(string jwt)
		{
			var claims = new List<System.Security.Claims.Claim>();
			var payload = jwt.Split('.')[1];
			var jsonBytes = ParseBase64WithoutPadding(payload);
			var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

			keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

			if (roles != null)
			{
				if (roles.ToString().Trim().StartsWith("["))
				{
					var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

					foreach (var parsedRole in parsedRoles)
					{
						claims.Add(new System.Security.Claims.Claim(ClaimTypes.Role, parsedRole));
					}
				}
				else
				{
					claims.Add(new System.Security.Claims.Claim(ClaimTypes.Role, roles.ToString()));
				}

				keyValuePairs.Remove(ClaimTypes.Role);
			}

			claims.AddRange(keyValuePairs.Select(kvp => new System.Security.Claims.Claim(kvp.Key, kvp.Value.ToString())));

			return claims;
		}

		private byte[] ParseBase64WithoutPadding(string base64)
		{
			switch (base64.Length % 4)
			{
				case 2: base64 += "=="; break;
				case 3: base64 += "="; break;
			}
			return Convert.FromBase64String(base64);
		}
	}
}
