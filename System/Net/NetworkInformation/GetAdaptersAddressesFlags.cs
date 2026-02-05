using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002B8 RID: 696
	[Flags]
	internal enum GetAdaptersAddressesFlags
	{
		// Token: 0x04001927 RID: 6439
		SkipUnicast = 1,
		// Token: 0x04001928 RID: 6440
		SkipAnycast = 2,
		// Token: 0x04001929 RID: 6441
		SkipMulticast = 4,
		// Token: 0x0400192A RID: 6442
		SkipDnsServer = 8,
		// Token: 0x0400192B RID: 6443
		IncludePrefix = 16,
		// Token: 0x0400192C RID: 6444
		SkipFriendlyName = 32,
		// Token: 0x0400192D RID: 6445
		IncludeWins = 64,
		// Token: 0x0400192E RID: 6446
		IncludeGateways = 128,
		// Token: 0x0400192F RID: 6447
		IncludeAllInterfaces = 256,
		// Token: 0x04001930 RID: 6448
		IncludeAllCompartments = 512,
		// Token: 0x04001931 RID: 6449
		IncludeTunnelBindingOrder = 1024
	}
}
