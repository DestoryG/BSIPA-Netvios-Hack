using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002FB RID: 763
	internal class SystemIPGlobalStatistics : IPGlobalStatistics
	{
		// Token: 0x06001AED RID: 6893 RVA: 0x000811FE File Offset: 0x0007F3FE
		private SystemIPGlobalStatistics()
		{
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x00081208 File Offset: 0x0007F408
		internal SystemIPGlobalStatistics(AddressFamily family)
		{
			uint ipStatisticsEx = UnsafeNetInfoNativeMethods.GetIpStatisticsEx(out this.stats, family);
			if (ipStatisticsEx != 0U)
			{
				throw new NetworkInformationException((int)ipStatisticsEx);
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06001AEF RID: 6895 RVA: 0x00081232 File Offset: 0x0007F432
		public override bool ForwardingEnabled
		{
			get
			{
				return this.stats.forwardingEnabled;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x0008123F File Offset: 0x0007F43F
		public override int DefaultTtl
		{
			get
			{
				return (int)this.stats.defaultTtl;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x0008124C File Offset: 0x0007F44C
		public override long ReceivedPackets
		{
			get
			{
				return (long)((ulong)this.stats.packetsReceived);
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x0008125A File Offset: 0x0007F45A
		public override long ReceivedPacketsWithHeadersErrors
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsWithHeaderErrors);
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x00081268 File Offset: 0x0007F468
		public override long ReceivedPacketsWithAddressErrors
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsWithAddressErrors);
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001AF4 RID: 6900 RVA: 0x00081276 File Offset: 0x0007F476
		public override long ReceivedPacketsForwarded
		{
			get
			{
				return (long)((ulong)this.stats.packetsForwarded);
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001AF5 RID: 6901 RVA: 0x00081284 File Offset: 0x0007F484
		public override long ReceivedPacketsWithUnknownProtocol
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsWithUnknownProtocols);
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x00081292 File Offset: 0x0007F492
		public override long ReceivedPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsDiscarded);
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x000812A0 File Offset: 0x0007F4A0
		public override long ReceivedPacketsDelivered
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsDelivered);
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x000812AE File Offset: 0x0007F4AE
		public override long OutputPacketRequests
		{
			get
			{
				return (long)((ulong)this.stats.packetOutputRequests);
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x000812BC File Offset: 0x0007F4BC
		public override long OutputPacketRoutingDiscards
		{
			get
			{
				return (long)((ulong)this.stats.outputPacketRoutingDiscards);
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001AFA RID: 6906 RVA: 0x000812CA File Offset: 0x0007F4CA
		public override long OutputPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.stats.outputPacketsDiscarded);
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06001AFB RID: 6907 RVA: 0x000812D8 File Offset: 0x0007F4D8
		public override long OutputPacketsWithNoRoute
		{
			get
			{
				return (long)((ulong)this.stats.outputPacketsWithNoRoute);
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001AFC RID: 6908 RVA: 0x000812E6 File Offset: 0x0007F4E6
		public override long PacketReassemblyTimeout
		{
			get
			{
				return (long)((ulong)this.stats.packetReassemblyTimeout);
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001AFD RID: 6909 RVA: 0x000812F4 File Offset: 0x0007F4F4
		public override long PacketReassembliesRequired
		{
			get
			{
				return (long)((ulong)this.stats.packetsReassemblyRequired);
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001AFE RID: 6910 RVA: 0x00081302 File Offset: 0x0007F502
		public override long PacketsReassembled
		{
			get
			{
				return (long)((ulong)this.stats.packetsReassembled);
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001AFF RID: 6911 RVA: 0x00081310 File Offset: 0x0007F510
		public override long PacketReassemblyFailures
		{
			get
			{
				return (long)((ulong)this.stats.packetsReassemblyFailed);
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001B00 RID: 6912 RVA: 0x0008131E File Offset: 0x0007F51E
		public override long PacketsFragmented
		{
			get
			{
				return (long)((ulong)this.stats.packetsFragmented);
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06001B01 RID: 6913 RVA: 0x0008132C File Offset: 0x0007F52C
		public override long PacketFragmentFailures
		{
			get
			{
				return (long)((ulong)this.stats.packetsFragmentFailed);
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001B02 RID: 6914 RVA: 0x0008133A File Offset: 0x0007F53A
		public override int NumberOfInterfaces
		{
			get
			{
				return (int)this.stats.interfaces;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001B03 RID: 6915 RVA: 0x00081347 File Offset: 0x0007F547
		public override int NumberOfIPAddresses
		{
			get
			{
				return (int)this.stats.ipAddresses;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001B04 RID: 6916 RVA: 0x00081354 File Offset: 0x0007F554
		public override int NumberOfRoutes
		{
			get
			{
				return (int)this.stats.routes;
			}
		}

		// Token: 0x04001ABB RID: 6843
		private MibIpStats stats;
	}
}
