using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002B9 RID: 697
	internal struct IpAddrString
	{
		// Token: 0x04001932 RID: 6450
		internal IntPtr Next;

		// Token: 0x04001933 RID: 6451
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string IpAddress;

		// Token: 0x04001934 RID: 6452
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string IpMask;

		// Token: 0x04001935 RID: 6453
		internal uint Context;
	}
}
