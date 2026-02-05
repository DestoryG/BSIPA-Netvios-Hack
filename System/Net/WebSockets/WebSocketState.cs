using System;

namespace System.Net.WebSockets
{
	// Token: 0x0200023C RID: 572
	public enum WebSocketState
	{
		// Token: 0x040016CE RID: 5838
		None,
		// Token: 0x040016CF RID: 5839
		Connecting,
		// Token: 0x040016D0 RID: 5840
		Open,
		// Token: 0x040016D1 RID: 5841
		CloseSent,
		// Token: 0x040016D2 RID: 5842
		CloseReceived,
		// Token: 0x040016D3 RID: 5843
		Closed,
		// Token: 0x040016D4 RID: 5844
		Aborted
	}
}
