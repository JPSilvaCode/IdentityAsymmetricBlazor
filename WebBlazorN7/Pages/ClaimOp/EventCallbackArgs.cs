using WebBlazorN7.ViewModel;

namespace WebBlazorN7.Pages.ClaimOp
{
	public class EventCallbackArgs
	{
		public EventCallbackArgs(bool isOk, ClaimViewModel? claimViewModel)
		{
			IsOk = isOk;
			ClaimViewModel = claimViewModel;
		}

		public bool IsOk { get; private set; }
		public ClaimViewModel? ClaimViewModel { get; private set; }
	}
}
