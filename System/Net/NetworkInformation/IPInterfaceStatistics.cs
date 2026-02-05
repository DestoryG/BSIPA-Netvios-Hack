using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002A5 RID: 677
	[global::__DynamicallyInvokable]
	public abstract class IPInterfaceStatistics
	{
		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001949 RID: 6473
		[global::__DynamicallyInvokable]
		public abstract long BytesReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x0600194A RID: 6474
		[global::__DynamicallyInvokable]
		public abstract long BytesSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x0600194B RID: 6475
		[global::__DynamicallyInvokable]
		public abstract long IncomingPacketsDiscarded
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x0600194C RID: 6476
		[global::__DynamicallyInvokable]
		public abstract long IncomingPacketsWithErrors
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x0600194D RID: 6477
		[global::__DynamicallyInvokable]
		public abstract long IncomingUnknownProtocolPackets
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x0600194E RID: 6478
		[global::__DynamicallyInvokable]
		public abstract long NonUnicastPacketsReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x0600194F RID: 6479
		[global::__DynamicallyInvokable]
		public abstract long NonUnicastPacketsSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001950 RID: 6480
		[global::__DynamicallyInvokable]
		public abstract long OutgoingPacketsDiscarded
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001951 RID: 6481
		[global::__DynamicallyInvokable]
		public abstract long OutgoingPacketsWithErrors
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001952 RID: 6482
		[global::__DynamicallyInvokable]
		public abstract long OutputQueueLength
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001953 RID: 6483
		[global::__DynamicallyInvokable]
		public abstract long UnicastPacketsReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001954 RID: 6484
		[global::__DynamicallyInvokable]
		public abstract long UnicastPacketsSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x0007DCE2 File Offset: 0x0007BEE2
		[global::__DynamicallyInvokable]
		protected IPInterfaceStatistics()
		{
		}
	}
}
