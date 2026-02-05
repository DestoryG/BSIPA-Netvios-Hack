using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002AA RID: 682
	[global::__DynamicallyInvokable]
	public abstract class UnicastIPAddressInformation : IPAddressInformation
	{
		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001963 RID: 6499
		[global::__DynamicallyInvokable]
		public abstract long AddressPreferredLifetime
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001964 RID: 6500
		[global::__DynamicallyInvokable]
		public abstract long AddressValidLifetime
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001965 RID: 6501
		[global::__DynamicallyInvokable]
		public abstract long DhcpLeaseLifetime
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001966 RID: 6502
		[global::__DynamicallyInvokable]
		public abstract DuplicateAddressDetectionState DuplicateAddressDetectionState
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001967 RID: 6503
		[global::__DynamicallyInvokable]
		public abstract PrefixOrigin PrefixOrigin
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001968 RID: 6504
		[global::__DynamicallyInvokable]
		public abstract SuffixOrigin SuffixOrigin
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001969 RID: 6505
		[global::__DynamicallyInvokable]
		public abstract IPAddress IPv4Mask
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x0600196A RID: 6506 RVA: 0x0007DCF2 File Offset: 0x0007BEF2
		[global::__DynamicallyInvokable]
		public virtual int PrefixLength
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0007DCF9 File Offset: 0x0007BEF9
		[global::__DynamicallyInvokable]
		protected UnicastIPAddressInformation()
		{
		}
	}
}
