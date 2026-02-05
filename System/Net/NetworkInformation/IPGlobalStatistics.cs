using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002A2 RID: 674
	[global::__DynamicallyInvokable]
	public abstract class IPGlobalStatistics
	{
		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001925 RID: 6437
		[global::__DynamicallyInvokable]
		public abstract int DefaultTtl
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001926 RID: 6438
		[global::__DynamicallyInvokable]
		public abstract bool ForwardingEnabled
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001927 RID: 6439
		[global::__DynamicallyInvokable]
		public abstract int NumberOfInterfaces
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001928 RID: 6440
		[global::__DynamicallyInvokable]
		public abstract int NumberOfIPAddresses
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001929 RID: 6441
		[global::__DynamicallyInvokable]
		public abstract long OutputPacketRequests
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x0600192A RID: 6442
		[global::__DynamicallyInvokable]
		public abstract long OutputPacketRoutingDiscards
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x0600192B RID: 6443
		[global::__DynamicallyInvokable]
		public abstract long OutputPacketsDiscarded
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x0600192C RID: 6444
		[global::__DynamicallyInvokable]
		public abstract long OutputPacketsWithNoRoute
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x0600192D RID: 6445
		[global::__DynamicallyInvokable]
		public abstract long PacketFragmentFailures
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x0600192E RID: 6446
		[global::__DynamicallyInvokable]
		public abstract long PacketReassembliesRequired
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x0600192F RID: 6447
		[global::__DynamicallyInvokable]
		public abstract long PacketReassemblyFailures
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001930 RID: 6448
		[global::__DynamicallyInvokable]
		public abstract long PacketReassemblyTimeout
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001931 RID: 6449
		[global::__DynamicallyInvokable]
		public abstract long PacketsFragmented
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001932 RID: 6450
		[global::__DynamicallyInvokable]
		public abstract long PacketsReassembled
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001933 RID: 6451
		[global::__DynamicallyInvokable]
		public abstract long ReceivedPackets
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001934 RID: 6452
		[global::__DynamicallyInvokable]
		public abstract long ReceivedPacketsDelivered
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001935 RID: 6453
		[global::__DynamicallyInvokable]
		public abstract long ReceivedPacketsDiscarded
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001936 RID: 6454
		[global::__DynamicallyInvokable]
		public abstract long ReceivedPacketsForwarded
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001937 RID: 6455
		[global::__DynamicallyInvokable]
		public abstract long ReceivedPacketsWithAddressErrors
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001938 RID: 6456
		[global::__DynamicallyInvokable]
		public abstract long ReceivedPacketsWithHeadersErrors
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001939 RID: 6457
		[global::__DynamicallyInvokable]
		public abstract long ReceivedPacketsWithUnknownProtocol
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x0600193A RID: 6458
		[global::__DynamicallyInvokable]
		public abstract int NumberOfRoutes
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x0007DCD2 File Offset: 0x0007BED2
		[global::__DynamicallyInvokable]
		protected IPGlobalStatistics()
		{
		}
	}
}
