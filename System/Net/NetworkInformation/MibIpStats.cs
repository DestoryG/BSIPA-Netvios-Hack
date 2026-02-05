using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002C5 RID: 709
	internal struct MibIpStats
	{
		// Token: 0x040019C9 RID: 6601
		internal bool forwardingEnabled;

		// Token: 0x040019CA RID: 6602
		internal uint defaultTtl;

		// Token: 0x040019CB RID: 6603
		internal uint packetsReceived;

		// Token: 0x040019CC RID: 6604
		internal uint receivedPacketsWithHeaderErrors;

		// Token: 0x040019CD RID: 6605
		internal uint receivedPacketsWithAddressErrors;

		// Token: 0x040019CE RID: 6606
		internal uint packetsForwarded;

		// Token: 0x040019CF RID: 6607
		internal uint receivedPacketsWithUnknownProtocols;

		// Token: 0x040019D0 RID: 6608
		internal uint receivedPacketsDiscarded;

		// Token: 0x040019D1 RID: 6609
		internal uint receivedPacketsDelivered;

		// Token: 0x040019D2 RID: 6610
		internal uint packetOutputRequests;

		// Token: 0x040019D3 RID: 6611
		internal uint outputPacketRoutingDiscards;

		// Token: 0x040019D4 RID: 6612
		internal uint outputPacketsDiscarded;

		// Token: 0x040019D5 RID: 6613
		internal uint outputPacketsWithNoRoute;

		// Token: 0x040019D6 RID: 6614
		internal uint packetReassemblyTimeout;

		// Token: 0x040019D7 RID: 6615
		internal uint packetsReassemblyRequired;

		// Token: 0x040019D8 RID: 6616
		internal uint packetsReassembled;

		// Token: 0x040019D9 RID: 6617
		internal uint packetsReassemblyFailed;

		// Token: 0x040019DA RID: 6618
		internal uint packetsFragmented;

		// Token: 0x040019DB RID: 6619
		internal uint packetsFragmentFailed;

		// Token: 0x040019DC RID: 6620
		internal uint packetsFragmentCreated;

		// Token: 0x040019DD RID: 6621
		internal uint interfaces;

		// Token: 0x040019DE RID: 6622
		internal uint ipAddresses;

		// Token: 0x040019DF RID: 6623
		internal uint routes;
	}
}
