using System;

namespace System.Net.Sockets
{
	// Token: 0x02000372 RID: 882
	public enum ProtocolType
	{
		// Token: 0x04001E07 RID: 7687
		IP,
		// Token: 0x04001E08 RID: 7688
		IPv6HopByHopOptions = 0,
		// Token: 0x04001E09 RID: 7689
		Icmp,
		// Token: 0x04001E0A RID: 7690
		Igmp,
		// Token: 0x04001E0B RID: 7691
		Ggp,
		// Token: 0x04001E0C RID: 7692
		IPv4,
		// Token: 0x04001E0D RID: 7693
		Tcp = 6,
		// Token: 0x04001E0E RID: 7694
		Pup = 12,
		// Token: 0x04001E0F RID: 7695
		Udp = 17,
		// Token: 0x04001E10 RID: 7696
		Idp = 22,
		// Token: 0x04001E11 RID: 7697
		IPv6 = 41,
		// Token: 0x04001E12 RID: 7698
		IPv6RoutingHeader = 43,
		// Token: 0x04001E13 RID: 7699
		IPv6FragmentHeader,
		// Token: 0x04001E14 RID: 7700
		IPSecEncapsulatingSecurityPayload = 50,
		// Token: 0x04001E15 RID: 7701
		IPSecAuthenticationHeader,
		// Token: 0x04001E16 RID: 7702
		IcmpV6 = 58,
		// Token: 0x04001E17 RID: 7703
		IPv6NoNextHeader,
		// Token: 0x04001E18 RID: 7704
		IPv6DestinationOptions,
		// Token: 0x04001E19 RID: 7705
		ND = 77,
		// Token: 0x04001E1A RID: 7706
		Raw = 255,
		// Token: 0x04001E1B RID: 7707
		Unspecified = 0,
		// Token: 0x04001E1C RID: 7708
		Ipx = 1000,
		// Token: 0x04001E1D RID: 7709
		Spx = 1256,
		// Token: 0x04001E1E RID: 7710
		SpxII,
		// Token: 0x04001E1F RID: 7711
		Unknown = -1
	}
}
