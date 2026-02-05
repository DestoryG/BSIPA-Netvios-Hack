using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020001D2 RID: 466
	internal struct IPMulticastRequest
	{
		// Token: 0x040014CD RID: 5325
		internal int MulticastAddress;

		// Token: 0x040014CE RID: 5326
		internal int InterfaceAddress;

		// Token: 0x040014CF RID: 5327
		internal static readonly int Size = Marshal.SizeOf(typeof(IPMulticastRequest));
	}
}
