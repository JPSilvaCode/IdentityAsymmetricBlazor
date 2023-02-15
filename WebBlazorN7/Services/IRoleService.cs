using WebBlazorN7.ViewModel;

namespace WebBlazorN7.Services
{
	public interface IRoleService
	{
		Task<Tuple<RoleViewModel, Erro>> Add(string name);
		Task<Erro> Put(string oldName, string newName);
		Task<Erro> Delete(string name);
		Task<Tuple<List<RoleViewModel>, Erro>> Get();
		Task<Tuple<RoleViewModel, Erro>> Get(int id);
		Task<Erro> AddUser(int roleId, int userId);
		Task<Erro> AddUsers(int roleId, List<int> usersId);
		Task<Tuple<RoleViewModel, Erro>> GetUsers(int roleId);
		Task<Erro> DeleteUser(int roleId, int userId);
	}
}
