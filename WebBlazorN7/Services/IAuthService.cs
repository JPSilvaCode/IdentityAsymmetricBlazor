using WebBlazorN7.ViewModel;

namespace WebBlazorN7.Services
{
    public interface IAuthService
    {

        Task<Tuple<LoginResponse, Erro>> Login(LoginRequest loginModel);
        Task<Tuple<LoginResponse, Erro>> RefreshToken();
		Task Logout();
    }
}
