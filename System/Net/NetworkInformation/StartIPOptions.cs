using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002DA RID: 730
	[Flags]
	internal enum StartIPOptions
	{
		// Token: 0x04001A40 RID: 6720
		Both = 3,
		// Token: 0x04001A41 RID: 6721
		None = 0,
		// Token: 0x04001A42 RID: 6722
		StartIPv4 = 1,
		// Token: 0x04001A43 RID: 6723
		StartIPv6 = 2
	}
}
