using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002C2 RID: 706
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct MibIfRow2
	{
		// Token: 0x04001989 RID: 6537
		private const int GuidLength = 16;

		// Token: 0x0400198A RID: 6538
		private const int IfMaxStringSize = 256;

		// Token: 0x0400198B RID: 6539
		private const int IfMaxPhysAddressLength = 32;

		// Token: 0x0400198C RID: 6540
		internal ulong interfaceLuid;

		// Token: 0x0400198D RID: 6541
		internal uint interfaceIndex;

		// Token: 0x0400198E RID: 6542
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		internal byte[] interfaceGuid;

		// Token: 0x0400198F RID: 6543
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 257)]
		internal char[] alias;

		// Token: 0x04001990 RID: 6544
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 257)]
		internal char[] description;

		// Token: 0x04001991 RID: 6545
		internal uint physicalAddressLength;

		// Token: 0x04001992 RID: 6546
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		internal byte[] physicalAddress;

		// Token: 0x04001993 RID: 6547
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		internal byte[] permanentPhysicalAddress;

		// Token: 0x04001994 RID: 6548
		internal uint mtu;

		// Token: 0x04001995 RID: 6549
		internal NetworkInterfaceType type;

		// Token: 0x04001996 RID: 6550
		internal InterfaceTunnelType tunnelType;

		// Token: 0x04001997 RID: 6551
		internal uint mediaType;

		// Token: 0x04001998 RID: 6552
		internal uint physicalMediumType;

		// Token: 0x04001999 RID: 6553
		internal uint accessType;

		// Token: 0x0400199A RID: 6554
		internal uint directionType;

		// Token: 0x0400199B RID: 6555
		internal byte interfaceAndOperStatusFlags;

		// Token: 0x0400199C RID: 6556
		internal OperationalStatus operStatus;

		// Token: 0x0400199D RID: 6557
		internal uint adminStatus;

		// Token: 0x0400199E RID: 6558
		internal uint mediaConnectState;

		// Token: 0x0400199F RID: 6559
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		internal byte[] networkGuid;

		// Token: 0x040019A0 RID: 6560
		internal InterfaceConnectionType connectionType;

		// Token: 0x040019A1 RID: 6561
		internal ulong transmitLinkSpeed;

		// Token: 0x040019A2 RID: 6562
		internal ulong receiveLinkSpeed;

		// Token: 0x040019A3 RID: 6563
		internal ulong inOctets;

		// Token: 0x040019A4 RID: 6564
		internal ulong inUcastPkts;

		// Token: 0x040019A5 RID: 6565
		internal ulong inNUcastPkts;

		// Token: 0x040019A6 RID: 6566
		internal ulong inDiscards;

		// Token: 0x040019A7 RID: 6567
		internal ulong inErrors;

		// Token: 0x040019A8 RID: 6568
		internal ulong inUnknownProtos;

		// Token: 0x040019A9 RID: 6569
		internal ulong inUcastOctets;

		// Token: 0x040019AA RID: 6570
		internal ulong inMulticastOctets;

		// Token: 0x040019AB RID: 6571
		internal ulong inBroadcastOctets;

		// Token: 0x040019AC RID: 6572
		internal ulong outOctets;

		// Token: 0x040019AD RID: 6573
		internal ulong outUcastPkts;

		// Token: 0x040019AE RID: 6574
		internal ulong outNUcastPkts;

		// Token: 0x040019AF RID: 6575
		internal ulong outDiscards;

		// Token: 0x040019B0 RID: 6576
		internal ulong outErrors;

		// Token: 0x040019B1 RID: 6577
		internal ulong outUcastOctets;

		// Token: 0x040019B2 RID: 6578
		internal ulong outMulticastOctets;

		// Token: 0x040019B3 RID: 6579
		internal ulong outBroadcastOctets;

		// Token: 0x040019B4 RID: 6580
		internal ulong outQLen;
	}
}
