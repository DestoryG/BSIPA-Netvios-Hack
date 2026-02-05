using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002A6 RID: 678
	[global::__DynamicallyInvokable]
	public abstract class IPv4InterfaceStatistics
	{
		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001956 RID: 6486
		[global::__DynamicallyInvokable]
		public abstract long BytesReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001957 RID: 6487
		[global::__DynamicallyInvokable]
		public abstract long BytesSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001958 RID: 6488
		[global::__DynamicallyInvokable]
		public abstract long IncomingPacketsDiscarded
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001959 RID: 6489
		[global::__DynamicallyInvokable]
		public abstract long IncomingPacketsWithErrors
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x0600195A RID: 6490
		[global::__DynamicallyInvokable]
		public abstract long IncomingUnknownProtocolPackets
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x0600195B RID: 6491
		[global::__DynamicallyInvokable]
		public abstract long NonUnicastPacketsReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x0600195C RID: 6492
		[global::__DynamicallyInvokable]
		public abstract long NonUnicastPacketsSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x0600195D RID: 6493
		[global::__DynamicallyInvokable]
		public abstract long OutgoingPacketsDiscarded
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x0600195E RID: 6494
		[global::__DynamicallyInvokable]
		public abstract long OutgoingPacketsWithErrors
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x0600195F RID: 6495
		[global::__DynamicallyInvokable]
		public abstract long OutputQueueLength
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001960 RID: 6496
		[global::__DynamicallyInvokable]
		public abstract long UnicastPacketsReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001961 RID: 6497
		[global::__DynamicallyInvokable]
		public abstract long UnicastPacketsSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0007DCEA File Offset: 0x0007BEEA
		[global::__DynamicallyInvokable]
		protected IPv4InterfaceStatistics()
		{
		}
	}
}
