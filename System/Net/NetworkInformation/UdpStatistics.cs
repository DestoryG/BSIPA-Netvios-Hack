using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000308 RID: 776
	[global::__DynamicallyInvokable]
	public abstract class UdpStatistics
	{
		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06001B74 RID: 7028
		[global::__DynamicallyInvokable]
		public abstract long DatagramsReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06001B75 RID: 7029
		[global::__DynamicallyInvokable]
		public abstract long DatagramsSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06001B76 RID: 7030
		[global::__DynamicallyInvokable]
		public abstract long IncomingDatagramsDiscarded
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06001B77 RID: 7031
		[global::__DynamicallyInvokable]
		public abstract long IncomingDatagramsWithErrors
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06001B78 RID: 7032
		[global::__DynamicallyInvokable]
		public abstract int UdpListeners
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x00081F74 File Offset: 0x00080174
		[global::__DynamicallyInvokable]
		protected UdpStatistics()
		{
		}
	}
}
