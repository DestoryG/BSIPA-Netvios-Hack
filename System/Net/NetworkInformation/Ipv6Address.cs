using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002D6 RID: 726
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct Ipv6Address
	{
		// Token: 0x04001A37 RID: 6711
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		internal byte[] Goo;

		// Token: 0x04001A38 RID: 6712
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		internal byte[] Address;

		// Token: 0x04001A39 RID: 6713
		internal uint ScopeID;
	}
}
