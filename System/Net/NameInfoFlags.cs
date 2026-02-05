using System;

namespace System.Net
{
	// Token: 0x020001D9 RID: 473
	[Flags]
	internal enum NameInfoFlags
	{
		// Token: 0x040014ED RID: 5357
		NI_NOFQDN = 1,
		// Token: 0x040014EE RID: 5358
		NI_NUMERICHOST = 2,
		// Token: 0x040014EF RID: 5359
		NI_NAMEREQD = 4,
		// Token: 0x040014F0 RID: 5360
		NI_NUMERICSERV = 8,
		// Token: 0x040014F1 RID: 5361
		NI_DGRAM = 16
	}
}
