using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002D7 RID: 727
	internal struct Icmp6EchoReply
	{
		// Token: 0x04001A3A RID: 6714
		internal Ipv6Address Address;

		// Token: 0x04001A3B RID: 6715
		internal uint Status;

		// Token: 0x04001A3C RID: 6716
		internal uint RoundTripTime;

		// Token: 0x04001A3D RID: 6717
		internal IntPtr data;
	}
}
