namespace WebBlazorN7.ViewModel
{
	public class LoginResponse
	{
		public string accessToken { get; set; }
		public int expiresIn { get; set; }
		public string refreshToken { get; set; }
		public UsuarioToken usuarioToken { get; set; }
	}

	public class Claim
	{
		public string value { get; set; }
		public string type { get; set; }
	}

	public class UsuarioToken
	{
		public int id { get; set; }
		public string email { get; set; }
		public List<Claim> claims { get; set; }
	}
}
