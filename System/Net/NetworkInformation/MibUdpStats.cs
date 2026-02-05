using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002C3 RID: 707
	internal struct MibUdpStats
	{
		// Token: 0x040019B5 RID: 6581
		internal uint datagramsReceived;

		// Token: 0x040019B6 RID: 6582
		internal uint incomingDatagramsDiscarded;

		// Token: 0x040019B7 RID: 6583
		internal uint incomingDatagramsWithErrors;

		// Token: 0x040019B8 RID: 6584
		internal uint datagramsSent;

		// Token: 0x040019B9 RID: 6585
		internal uint udpListeners;
	}
}
