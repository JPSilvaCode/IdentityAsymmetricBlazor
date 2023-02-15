using WebBlazorN7.ViewModel;

namespace WebBlazorN7.Services
{
	public interface IUserService
	{
		Task<Tuple<RegistroResponse, Erro>> Register(RegistroRequest registro);
		Task<Tuple<List<Usuario>, Erro>> Get();
		Task<Tuple<Usuario, Erro>> Get(int id);
		Task<Erro> Put(string email, Usuario usuario);
		Task<Erro> Delete(string email);
		Task<Tuple<List<string>, Erro>> GetRoles(int usuarioId);
		Task<Erro> AddRole(int usuarioId, int roleId);
		Task<Erro> DeleteRole(int usuarioId, int roleId);
		Task<Tuple<List<ClaimViewModel>, Erro>> GetClaims(int usuarioId);
		Task<Erro> AddClaim(int usuarioId, string type, string value);
		Task<Erro> PutValueClaim(int usuarioId, string type, string value);
		Task<Erro> DeleteClaim(int usuarioId, string type);
	}
}
