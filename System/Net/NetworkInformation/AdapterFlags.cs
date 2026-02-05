using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002B5 RID: 693
	[Flags]
	internal enum AdapterFlags
	{
		// Token: 0x04001912 RID: 6418
		DnsEnabled = 1,
		// Token: 0x04001913 RID: 6419
		RegisterAdapterSuffix = 2,
		// Token: 0x04001914 RID: 6420
		DhcpEnabled = 4,
		// Token: 0x04001915 RID: 6421
		ReceiveOnly = 8,
		// Token: 0x04001916 RID: 6422
		NoMulticast = 16,
		// Token: 0x04001917 RID: 6423
		Ipv6OtherStatefulConfig = 32,
		// Token: 0x04001918 RID: 6424
		NetBiosOverTcp = 64,
		// Token: 0x04001919 RID: 6425
		IPv4Enabled = 128,
		// Token: 0x0400191A RID: 6426
		IPv6Enabled = 256,
		// Token: 0x0400191B RID: 6427
		IPv6ManagedAddressConfigurationSupported = 512
	}
}
