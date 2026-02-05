using System;

namespace System.Net.WebSockets
{
	// Token: 0x02000235 RID: 565
	public enum WebSocketError
	{
		// Token: 0x04001696 RID: 5782
		Success,
		// Token: 0x04001697 RID: 5783
		InvalidMessageType,
		// Token: 0x04001698 RID: 5784
		Faulted,
		// Token: 0x04001699 RID: 5785
		NativeError,
		// Token: 0x0400169A RID: 5786
		NotAWebSocket,
		// Token: 0x0400169B RID: 5787
		UnsupportedVersion,
		// Token: 0x0400169C RID: 5788
		UnsupportedProtocol,
		// Token: 0x0400169D RID: 5789
		HeaderError,
		// Token: 0x0400169E RID: 5790
		ConnectionClosedPrematurely,
		// Token: 0x0400169F RID: 5791
		InvalidState
	}
}
