using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020001D6 RID: 470
	internal struct WSAData
	{
		// Token: 0x040014D8 RID: 5336
		internal short wVersion;

		// Token: 0x040014D9 RID: 5337
		internal short wHighVersion;

		// Token: 0x040014DA RID: 5338
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 257)]
		internal string szDescription;

		// Token: 0x040014DB RID: 5339
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
		internal string szSystemStatus;

		// Token: 0x040014DC RID: 5340
		internal short iMaxSockets;

		// Token: 0x040014DD RID: 5341
		internal short iMaxUdpDg;

		// Token: 0x040014DE RID: 5342
		internal IntPtr lpVendorInfo;
	}
}
