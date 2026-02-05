using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020001DA RID: 474
	internal struct IPv6MulticastRequest
	{
		// Token: 0x040014F2 RID: 5362
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		internal byte[] MulticastAddress;

		// Token: 0x040014F3 RID: 5363
		internal int InterfaceIndex;

		// Token: 0x040014F4 RID: 5364
		internal static readonly int Size = Marshal.SizeOf(typeof(IPv6MulticastRequest));
	}
}
