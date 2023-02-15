using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace WebBlazorN7
{
	public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
	{
		public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation)
			: base(provider, navigation)
		{
			ConfigureHandler(
				authorizedUrls: new[] { "http://localhost:55772" },
				scopes: new[] { "companyApi" });
		}
	}
}
