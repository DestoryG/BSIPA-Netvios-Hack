using System;

namespace System.Net.Sockets
{
	// Token: 0x0200037E RID: 894
	[Flags]
	public enum SocketFlags
	{
		// Token: 0x04001EE6 RID: 7910
		None = 0,
		// Token: 0x04001EE7 RID: 7911
		OutOfBand = 1,
		// Token: 0x04001EE8 RID: 7912
		Peek = 2,
		// Token: 0x04001EE9 RID: 7913
		DontRoute = 4,
		// Token: 0x04001EEA RID: 7914
		MaxIOVectorLength = 16,
		// Token: 0x04001EEB RID: 7915
		Truncated = 256,
		// Token: 0x04001EEC RID: 7916
		ControlDataTruncated = 512,
		// Token: 0x04001EED RID: 7917
		Broadcast = 1024,
		// Token: 0x04001EEE RID: 7918
		Multicast = 2048,
		// Token: 0x04001EEF RID: 7919
		Partial = 32768
	}
}
