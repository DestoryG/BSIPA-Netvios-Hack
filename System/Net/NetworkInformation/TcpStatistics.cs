using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000307 RID: 775
	[global::__DynamicallyInvokable]
	public abstract class TcpStatistics
	{
		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06001B65 RID: 7013
		[global::__DynamicallyInvokable]
		public abstract long ConnectionsAccepted
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06001B66 RID: 7014
		[global::__DynamicallyInvokable]
		public abstract long ConnectionsInitiated
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06001B67 RID: 7015
		[global::__DynamicallyInvokable]
		public abstract long CumulativeConnections
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06001B68 RID: 7016
		[global::__DynamicallyInvokable]
		public abstract long CurrentConnections
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06001B69 RID: 7017
		[global::__DynamicallyInvokable]
		public abstract long ErrorsReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06001B6A RID: 7018
		[global::__DynamicallyInvokable]
		public abstract long FailedConnectionAttempts
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06001B6B RID: 7019
		[global::__DynamicallyInvokable]
		public abstract long MaximumConnections
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06001B6C RID: 7020
		[global::__DynamicallyInvokable]
		public abstract long MaximumTransmissionTimeout
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06001B6D RID: 7021
		[global::__DynamicallyInvokable]
		public abstract long MinimumTransmissionTimeout
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06001B6E RID: 7022
		[global::__DynamicallyInvokable]
		public abstract long ResetConnections
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06001B6F RID: 7023
		[global::__DynamicallyInvokable]
		public abstract long SegmentsReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06001B70 RID: 7024
		[global::__DynamicallyInvokable]
		public abstract long SegmentsResent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06001B71 RID: 7025
		[global::__DynamicallyInvokable]
		public abstract long SegmentsSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06001B72 RID: 7026
		[global::__DynamicallyInvokable]
		public abstract long ResetsSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x00081F6C File Offset: 0x0008016C
		[global::__DynamicallyInvokable]
		protected TcpStatistics()
		{
		}
	}
}
