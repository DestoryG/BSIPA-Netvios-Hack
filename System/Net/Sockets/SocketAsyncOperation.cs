using System;

namespace System.Net.Sockets
{
	// Token: 0x02000379 RID: 889
	public enum SocketAsyncOperation
	{
		// Token: 0x04001E5B RID: 7771
		None,
		// Token: 0x04001E5C RID: 7772
		Accept,
		// Token: 0x04001E5D RID: 7773
		Connect,
		// Token: 0x04001E5E RID: 7774
		Disconnect,
		// Token: 0x04001E5F RID: 7775
		Receive,
		// Token: 0x04001E60 RID: 7776
		ReceiveFrom,
		// Token: 0x04001E61 RID: 7777
		ReceiveMessageFrom,
		// Token: 0x04001E62 RID: 7778
		Send,
		// Token: 0x04001E63 RID: 7779
		SendPackets,
		// Token: 0x04001E64 RID: 7780
		SendTo
	}
}
