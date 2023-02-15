using System.Net.Http.Json;
using WebBlazorN7.ViewModel;

namespace WebBlazorN7.Services
{
	public class RoleService : IRoleService
	{
		public HttpClient _httpClient;

		public RoleService(HttpClient client)
		{
			_httpClient = client;
		}

		public async Task<Tuple<List<RoleViewModel>, Erro>> Get()
		{
			try
			{
				var roles = await _httpClient.GetFromJsonAsync<List<RoleViewModel>>("/api/v3/Roles/roles");
				return new Tuple<List<RoleViewModel>, Erro>(roles, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<List<RoleViewModel>, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}

		public async Task<Tuple<RoleViewModel, Erro>> Get(int id)
		{
			try
			{
				var role = await _httpClient.GetFromJsonAsync<RoleViewModel>($"/api/v3/Roles/{id}");
				return new Tuple<RoleViewModel, Erro>(role, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<RoleViewModel, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}

		public async Task<Tuple<RoleViewModel, Erro>> Add(string name)
		{
			try
			{
				using var response = await _httpClient.PostAsync($"/api/v3/Roles/role/{name}", null);

				if (response.StatusCode == System.Net.HttpStatusCode.Created)
				{
					var role = await response.Content.ReadFromJsonAsync<RoleViewModel>();
					if (role is not null)
						return new Tuple<RoleViewModel, Erro>(role, null);
				}
				else
				{
					var erro = await response.Content.ReadFromJsonAsync<Erro>();
					if (erro is not null)
						return new Tuple<RoleViewModel, Erro>(null, erro);
				}

				return new Tuple<RoleViewModel, Erro>(null, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<RoleViewModel, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}

		public async Task<Erro> Put(string oldName, string newName)
		{
			try
			{
				var newRole = new { oldName, newName };
				var response = await _httpClient.PutAsJsonAsync($"/api/v3/Roles/role", newRole);

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

		public async Task<Erro> Delete(string name)
		{
			try
			{
				var response = await _httpClient.DeleteAsync($"/api/v3/Roles/{name}");

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

		public async Task<Erro> AddUser(int roleId, int userId)
		{
			try
			{
				using var response = await _httpClient.PostAsync($"/api/v3/Roles/Role/{roleId}/User/{userId}", null);

				if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
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

		public async Task<Erro> AddUsers(int roleId, List<int> usersId)
		{
			try
			{
				using var response = await _httpClient.PostAsJsonAsync($"/api/v3/Roles/Role/{roleId}/UsersById", usersId);

				if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
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

		public async Task<Tuple<RoleViewModel, Erro>> GetUsers(int roleId)
		{
			try
			{
				var role = await _httpClient.GetFromJsonAsync<RoleViewModel>($"/api/v3/Roles/Role/{roleId}/Users");
				return new Tuple<RoleViewModel, Erro>(role, null);
			}
			catch (HttpRequestException ex)
			{
				return new Tuple<RoleViewModel, Erro>(null, new Erro { errors = new Errors { Messages = new List<string> { $"Request failed. {(ex.StatusCode.HasValue ? $"Error status code: {(int)ex.StatusCode}" : $"{ex.Message}")}" } } });
			}
		}

		public async Task<Erro> DeleteUser(int roleId, int userId)
		{
			try
			{
				using var response = await _httpClient.DeleteAsync($"/api/v3/Roles/Role/{roleId}/User/{userId}");

				if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
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
	}
}
