namespace WebBlazorN7.ViewModel
{
	public class Erro
	{
		public string title { get; set; }
		public int status { get; set; }
		public Errors errors { get; set; }
	}

	public class Errors
	{
		public List<string> Messages { get; set; }
		public List<string> ConfirmPassword { get; set; }

		public override string ToString()
		{
			string erro = string.Empty;

			if (Messages != null)
			{
				var count = 1;
				foreach (var message in Messages)
				{
					erro += $"{message} {(count.Equals(Messages.Count()) ? "" : "\r\n")}";
					count++;
				}
			}

			if (ConfirmPassword != null)
			{
				var count = 1;
				foreach (var message in ConfirmPassword)
				{
					erro += $"{message} {(count.Equals(ConfirmPassword.Count()) ? "" : "\r\n")}";
					count++;
				}
			}

			return erro;
		}
	}
}
