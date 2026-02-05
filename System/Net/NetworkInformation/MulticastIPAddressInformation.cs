using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002AC RID: 684
	[global::__DynamicallyInvokable]
	public abstract class MulticastIPAddressInformation : IPAddressInformation
	{
		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001978 RID: 6520
		[global::__DynamicallyInvokable]
		public abstract long AddressPreferredLifetime
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001979 RID: 6521
		[global::__DynamicallyInvokable]
		public abstract long AddressValidLifetime
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x0600197A RID: 6522
		[global::__DynamicallyInvokable]
		public abstract long DhcpLeaseLifetime
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x0600197B RID: 6523
		[global::__DynamicallyInvokable]
		public abstract DuplicateAddressDetectionState DuplicateAddressDetectionState
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x0600197C RID: 6524
		[global::__DynamicallyInvokable]
		public abstract PrefixOrigin PrefixOrigin
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x0600197D RID: 6525
		[global::__DynamicallyInvokable]
		public abstract SuffixOrigin SuffixOrigin
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x0007DDA5 File Offset: 0x0007BFA5
		[global::__DynamicallyInvokable]
		protected MulticastIPAddressInformation()
		{
		}
	}
}
