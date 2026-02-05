using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002C4 RID: 708
	internal struct MibTcpStats
	{
		// Token: 0x040019BA RID: 6586
		internal uint reTransmissionAlgorithm;

		// Token: 0x040019BB RID: 6587
		internal uint minimumRetransmissionTimeOut;

		// Token: 0x040019BC RID: 6588
		internal uint maximumRetransmissionTimeOut;

		// Token: 0x040019BD RID: 6589
		internal uint maximumConnections;

		// Token: 0x040019BE RID: 6590
		internal uint activeOpens;

		// Token: 0x040019BF RID: 6591
		internal uint passiveOpens;

		// Token: 0x040019C0 RID: 6592
		internal uint failedConnectionAttempts;

		// Token: 0x040019C1 RID: 6593
		internal uint resetConnections;

		// Token: 0x040019C2 RID: 6594
		internal uint currentConnections;

		// Token: 0x040019C3 RID: 6595
		internal uint segmentsReceived;

		// Token: 0x040019C4 RID: 6596
		internal uint segmentsSent;

		// Token: 0x040019C5 RID: 6597
		internal uint segmentsResent;

		// Token: 0x040019C6 RID: 6598
		internal uint errorsReceived;

		// Token: 0x040019C7 RID: 6599
		internal uint segmentsSentWithReset;

		// Token: 0x040019C8 RID: 6600
		internal uint cumulativeConnections;
	}
}
