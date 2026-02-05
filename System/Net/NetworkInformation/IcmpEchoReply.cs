using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002D5 RID: 725
	internal struct IcmpEchoReply
	{
		// Token: 0x04001A30 RID: 6704
		internal uint address;

		// Token: 0x04001A31 RID: 6705
		internal uint status;

		// Token: 0x04001A32 RID: 6706
		internal uint roundTripTime;

		// Token: 0x04001A33 RID: 6707
		internal ushort dataSize;

		// Token: 0x04001A34 RID: 6708
		internal ushort reserved;

		// Token: 0x04001A35 RID: 6709
		internal IntPtr data;

		// Token: 0x04001A36 RID: 6710
		internal IPOptions options;
	}
}
