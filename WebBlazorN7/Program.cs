using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using WebBlazorN7;
using WebBlazorN7.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services
	.AddBlazorise(options =>
	{
		options.Immediate = true;
	})
	.AddBootstrapProviders()
	.AddFontAwesomeIcons();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IClaimService, ClaimService>();

//builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
//{
//	client.BaseAddress = new Uri("http://localhost:55772");
//});

//builder.Services.AddHttpClient<IUserService, UserService>(client =>
//{
//	client.BaseAddress = new Uri("http://localhost:55772");
//});

//builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<TokenHandler>();

//builder.Services.AddHttpClient("IAAPI", client => client.BaseAddress = new Uri("http://localhost:55772"));//.AddHttpMessageHandler<TokenHandler>();

//builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("IAAPI"));

builder.Services.AddScoped(sp => new HttpClient
{
	BaseAddress = new Uri("http://localhost:55772")
}
.EnableIntercept(sp));

builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddHttpClientInterceptor();

builder.Services.AddScoped<HttpInterceptorService>();

//builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

//builder.Services.AddHttpClient("IAAPI", cl =>
//{
//	cl.BaseAddress = new Uri("http://localhost:55772");
//}).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

//builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("IAAPI"));

await builder.Build().RunAsync();
