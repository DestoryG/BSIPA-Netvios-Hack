using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002BD RID: 701
	internal struct IpAdapterUnicastAddress
	{
		// Token: 0x04001948 RID: 6472
		internal uint length;

		// Token: 0x04001949 RID: 6473
		internal AdapterAddressFlags flags;

		// Token: 0x0400194A RID: 6474
		internal IntPtr next;

		// Token: 0x0400194B RID: 6475
		internal IpSocketAddress address;

		// Token: 0x0400194C RID: 6476
		internal PrefixOrigin prefixOrigin;

		// Token: 0x0400194D RID: 6477
		internal SuffixOrigin suffixOrigin;

		// Token: 0x0400194E RID: 6478
		internal DuplicateAddressDetectionState dadState;

		// Token: 0x0400194F RID: 6479
		internal uint validLifetime;

		// Token: 0x04001950 RID: 6480
		internal uint preferredLifetime;

		// Token: 0x04001951 RID: 6481
		internal uint leaseLifetime;

		// Token: 0x04001952 RID: 6482
		internal byte prefixLength;
	}
}
