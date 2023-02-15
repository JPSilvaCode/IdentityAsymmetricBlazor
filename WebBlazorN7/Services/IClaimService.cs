using WebBlazorN7.ViewModel;

namespace WebBlazorN7.Services
{
	public interface IClaimService
	{
		Task<Tuple<ClaimViewModel, Erro>> Add(ClaimViewModel claimViewModel);
		Task<Erro> PutType(string oldName, string newName);
		Task<Erro> PutValue(int userId, string type, string value);
		Task<Erro> DeleteType(string type);
		Task<Erro> DeleteTypeUser(string type, int userId);
		Task<Tuple<List<ClaimViewModel>, Erro>> Get();
		Task<Tuple<List<ClaimUserViewModel>, Erro>> GetUsersByClaim(string type);
	}
}
