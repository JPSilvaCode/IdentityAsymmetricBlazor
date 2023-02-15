using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WebBlazorN7.Services;
using WebBlazorN7.ViewModel;

namespace WebBlazorN7
{
	public class RefreshTokenService
	{
		private readonly AuthenticationStateProvider _authProvider;
		private readonly IAuthService _authService;
		private readonly ILocalStorageService _localStorage;

		public RefreshTokenService(AuthenticationStateProvider authProvider, IAuthService authService, ILocalStorageService localStorage)
		{
			_authProvider = authProvider;
			_authService = authService;
			_localStorage = localStorage;
		}

		public async Task<string> TryRefreshToken()
		{
			var authState = await _authProvider.GetAuthenticationStateAsync();
			var user = authState.User;

			if (user == null) return string.Empty;

			var savedToken = await _localStorage.GetItemAsync<string>("authToken");
			if (string.IsNullOrEmpty(savedToken)) return string.Empty;

			var exp = AuthenticationService.GetTokenExpirationTime(savedToken);
			var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));

			var timeUTC = DateTime.UtcNow;

			var diff = expTime - timeUTC;
			if (diff.TotalMinutes <= 2)
			{
				var retorno = await _authService.RefreshToken();
				LoginResponse usuario = retorno.Item1;
				if (usuario != null)
					return usuario.accessToken;
			}

			return string.Empty;
		}
	}
}
