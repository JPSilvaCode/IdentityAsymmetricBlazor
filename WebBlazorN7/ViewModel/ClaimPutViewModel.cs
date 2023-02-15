using System.ComponentModel.DataAnnotations;

namespace WebBlazorN7.ViewModel
{
	public class ClaimPutViewModel
	{
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[MaxLength(10, ErrorMessage = "Tamanho máximo {1}.")]
		[MinLength(2, ErrorMessage = "Tamanho minímo {1}.")]
		public string Type { get; set; }
	}
}
