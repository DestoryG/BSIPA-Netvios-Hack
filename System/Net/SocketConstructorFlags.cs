using System;

namespace System.Net
{
	// Token: 0x020001DB RID: 475
	[Flags]
	internal enum SocketConstructorFlags
	{
		// Token: 0x040014F6 RID: 5366
		WSA_FLAG_OVERLAPPED = 1,
		// Token: 0x040014F7 RID: 5367
		WSA_FLAG_MULTIPOINT_C_ROOT = 2,
		// Token: 0x040014F8 RID: 5368
		WSA_FLAG_MULTIPOINT_C_LEAF = 4,
		// Token: 0x040014F9 RID: 5369
		WSA_FLAG_MULTIPOINT_D_ROOT = 8,
		// Token: 0x040014FA RID: 5370
		WSA_FLAG_MULTIPOINT_D_LEAF = 16
	}
}
