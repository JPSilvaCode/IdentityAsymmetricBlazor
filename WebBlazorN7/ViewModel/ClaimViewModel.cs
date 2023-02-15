using System.ComponentModel.DataAnnotations;

namespace WebBlazorN7.ViewModel
{
	public class ClaimViewModel
	{
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[MaxLength(10, ErrorMessage = "Tamanho máximo {1}.")]
		[MinLength(2, ErrorMessage = "Tamanho minímo {1}.")]
		public string Type { get; set; }

		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		public string Value { get; set; }

		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[Range(1,9999, ErrorMessage = "O campo {0} é obrigatório.")]
		public int UserId { get; set; }

		public bool Selecao { get; set; }
	}
}
