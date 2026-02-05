using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000303 RID: 771
	internal class SystemUdpStatistics : UdpStatistics
	{
		// Token: 0x06001B54 RID: 6996 RVA: 0x00081CA6 File Offset: 0x0007FEA6
		private SystemUdpStatistics()
		{
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x00081CB0 File Offset: 0x0007FEB0
		internal SystemUdpStatistics(AddressFamily family)
		{
			uint udpStatisticsEx = UnsafeNetInfoNativeMethods.GetUdpStatisticsEx(out this.stats, family);
			if (udpStatisticsEx != 0U)
			{
				throw new NetworkInformationException((int)udpStatisticsEx);
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06001B56 RID: 6998 RVA: 0x00081CDA File Offset: 0x0007FEDA
		public override long DatagramsReceived
		{
			get
			{
				return (long)((ulong)this.stats.datagramsReceived);
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06001B57 RID: 6999 RVA: 0x00081CE8 File Offset: 0x0007FEE8
		public override long IncomingDatagramsDiscarded
		{
			get
			{
				return (long)((ulong)this.stats.incomingDatagramsDiscarded);
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06001B58 RID: 7000 RVA: 0x00081CF6 File Offset: 0x0007FEF6
		public override long IncomingDatagramsWithErrors
		{
			get
			{
				return (long)((ulong)this.stats.incomingDatagramsWithErrors);
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06001B59 RID: 7001 RVA: 0x00081D04 File Offset: 0x0007FF04
		public override long DatagramsSent
		{
			get
			{
				return (long)((ulong)this.stats.datagramsSent);
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06001B5A RID: 7002 RVA: 0x00081D12 File Offset: 0x0007FF12
		public override int UdpListeners
		{
			get
			{
				return (int)this.stats.udpListeners;
			}
		}

		// Token: 0x04001AE0 RID: 6880
		private MibUdpStats stats;
	}
}
