using System.ComponentModel.DataAnnotations;

namespace WebBlazorN7.ViewModel
{
    public class LoginRequest
    {
        [Required]
		[EmailAddress]
		public string Email { get; set; }

        [Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
