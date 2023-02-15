using System.ComponentModel.DataAnnotations;

namespace WebBlazorN7.ViewModel
{
	public class RoleViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[MaxLength(20, ErrorMessage = "Tamanho máximo {1}.")]
		[MinLength(2, ErrorMessage = "Tamanho minímo {1}.")]
		public string Name { get; set; }

		public List<Usuario> UsersResponse { get; set; }

		public bool Selecao { get; set; }
	}
}
