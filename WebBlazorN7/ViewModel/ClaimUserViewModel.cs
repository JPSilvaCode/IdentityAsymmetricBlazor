using System.ComponentModel.DataAnnotations;

namespace WebBlazorN7.ViewModel
{
	public class ClaimUserViewModel
	{		
		public string Type { get; set; }
		
		public string Value { get; set; }
		public Usuario UserResponse { get; set; }
	}
}
