using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002F3 RID: 755
	internal enum IcmpV6StatType
	{
		// Token: 0x04001A94 RID: 6804
		DestinationUnreachable = 1,
		// Token: 0x04001A95 RID: 6805
		PacketTooBig,
		// Token: 0x04001A96 RID: 6806
		TimeExceeded,
		// Token: 0x04001A97 RID: 6807
		ParameterProblem,
		// Token: 0x04001A98 RID: 6808
		EchoRequest = 128,
		// Token: 0x04001A99 RID: 6809
		EchoReply,
		// Token: 0x04001A9A RID: 6810
		MembershipQuery,
		// Token: 0x04001A9B RID: 6811
		MembershipReport,
		// Token: 0x04001A9C RID: 6812
		MembershipReduction,
		// Token: 0x04001A9D RID: 6813
		RouterSolicit,
		// Token: 0x04001A9E RID: 6814
		RouterAdvertisement,
		// Token: 0x04001A9F RID: 6815
		NeighborSolict,
		// Token: 0x04001AA0 RID: 6816
		NeighborAdvertisement,
		// Token: 0x04001AA1 RID: 6817
		Redirect
	}
}
