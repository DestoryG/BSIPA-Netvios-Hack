using System;

namespace System.Net.Sockets
{
	// Token: 0x02000374 RID: 884
	[Flags]
	public enum SocketInformationOptions
	{
		// Token: 0x04001E25 RID: 7717
		NonBlocking = 1,
		// Token: 0x04001E26 RID: 7718
		Connected = 2,
		// Token: 0x04001E27 RID: 7719
		Listening = 4,
		// Token: 0x04001E28 RID: 7720
		UseOnlyOverlappedIO = 8
	}
}
