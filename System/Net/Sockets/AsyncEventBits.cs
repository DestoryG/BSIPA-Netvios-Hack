using System;

namespace System.Net.Sockets
{
	// Token: 0x02000367 RID: 871
	[Flags]
	internal enum AsyncEventBits
	{
		// Token: 0x04001D93 RID: 7571
		FdNone = 0,
		// Token: 0x04001D94 RID: 7572
		FdRead = 1,
		// Token: 0x04001D95 RID: 7573
		FdWrite = 2,
		// Token: 0x04001D96 RID: 7574
		FdOob = 4,
		// Token: 0x04001D97 RID: 7575
		FdAccept = 8,
		// Token: 0x04001D98 RID: 7576
		FdConnect = 16,
		// Token: 0x04001D99 RID: 7577
		FdClose = 32,
		// Token: 0x04001D9A RID: 7578
		FdQos = 64,
		// Token: 0x04001D9B RID: 7579
		FdGroupQos = 128,
		// Token: 0x04001D9C RID: 7580
		FdRoutingInterfaceChange = 256,
		// Token: 0x04001D9D RID: 7581
		FdAddressListChange = 512,
		// Token: 0x04001D9E RID: 7582
		FdAllEvents = 1023
	}
}
