using System;

namespace System.Net.Sockets
{
	// Token: 0x02000380 RID: 896
	public enum SocketOptionName
	{
		// Token: 0x04001EF7 RID: 7927
		Debug = 1,
		// Token: 0x04001EF8 RID: 7928
		AcceptConnection,
		// Token: 0x04001EF9 RID: 7929
		ReuseAddress = 4,
		// Token: 0x04001EFA RID: 7930
		KeepAlive = 8,
		// Token: 0x04001EFB RID: 7931
		DontRoute = 16,
		// Token: 0x04001EFC RID: 7932
		Broadcast = 32,
		// Token: 0x04001EFD RID: 7933
		UseLoopback = 64,
		// Token: 0x04001EFE RID: 7934
		Linger = 128,
		// Token: 0x04001EFF RID: 7935
		OutOfBandInline = 256,
		// Token: 0x04001F00 RID: 7936
		DontLinger = -129,
		// Token: 0x04001F01 RID: 7937
		ExclusiveAddressUse = -5,
		// Token: 0x04001F02 RID: 7938
		SendBuffer = 4097,
		// Token: 0x04001F03 RID: 7939
		ReceiveBuffer,
		// Token: 0x04001F04 RID: 7940
		SendLowWater,
		// Token: 0x04001F05 RID: 7941
		ReceiveLowWater,
		// Token: 0x04001F06 RID: 7942
		SendTimeout,
		// Token: 0x04001F07 RID: 7943
		ReceiveTimeout,
		// Token: 0x04001F08 RID: 7944
		Error,
		// Token: 0x04001F09 RID: 7945
		Type,
		// Token: 0x04001F0A RID: 7946
		ReuseUnicastPort = 12295,
		// Token: 0x04001F0B RID: 7947
		MaxConnections = 2147483647,
		// Token: 0x04001F0C RID: 7948
		IPOptions = 1,
		// Token: 0x04001F0D RID: 7949
		HeaderIncluded,
		// Token: 0x04001F0E RID: 7950
		TypeOfService,
		// Token: 0x04001F0F RID: 7951
		IpTimeToLive,
		// Token: 0x04001F10 RID: 7952
		MulticastInterface = 9,
		// Token: 0x04001F11 RID: 7953
		MulticastTimeToLive,
		// Token: 0x04001F12 RID: 7954
		MulticastLoopback,
		// Token: 0x04001F13 RID: 7955
		AddMembership,
		// Token: 0x04001F14 RID: 7956
		DropMembership,
		// Token: 0x04001F15 RID: 7957
		DontFragment,
		// Token: 0x04001F16 RID: 7958
		AddSourceMembership,
		// Token: 0x04001F17 RID: 7959
		DropSourceMembership,
		// Token: 0x04001F18 RID: 7960
		BlockSource,
		// Token: 0x04001F19 RID: 7961
		UnblockSource,
		// Token: 0x04001F1A RID: 7962
		PacketInformation,
		// Token: 0x04001F1B RID: 7963
		HopLimit = 21,
		// Token: 0x04001F1C RID: 7964
		IPProtectionLevel = 23,
		// Token: 0x04001F1D RID: 7965
		IPv6Only = 27,
		// Token: 0x04001F1E RID: 7966
		NoDelay = 1,
		// Token: 0x04001F1F RID: 7967
		BsdUrgent,
		// Token: 0x04001F20 RID: 7968
		Expedited = 2,
		// Token: 0x04001F21 RID: 7969
		NoChecksum = 1,
		// Token: 0x04001F22 RID: 7970
		ChecksumCoverage = 20,
		// Token: 0x04001F23 RID: 7971
		UpdateAcceptContext = 28683,
		// Token: 0x04001F24 RID: 7972
		UpdateConnectContext = 28688
	}
}
