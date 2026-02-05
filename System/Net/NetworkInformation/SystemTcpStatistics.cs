using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000302 RID: 770
	internal class SystemTcpStatistics : TcpStatistics
	{
		// Token: 0x06001B44 RID: 6980 RVA: 0x00081BAF File Offset: 0x0007FDAF
		private SystemTcpStatistics()
		{
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x00081BB8 File Offset: 0x0007FDB8
		internal SystemTcpStatistics(AddressFamily family)
		{
			uint tcpStatisticsEx = UnsafeNetInfoNativeMethods.GetTcpStatisticsEx(out this.stats, family);
			if (tcpStatisticsEx != 0U)
			{
				throw new NetworkInformationException((int)tcpStatisticsEx);
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001B46 RID: 6982 RVA: 0x00081BE2 File Offset: 0x0007FDE2
		public override long MinimumTransmissionTimeout
		{
			get
			{
				return (long)((ulong)this.stats.minimumRetransmissionTimeOut);
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001B47 RID: 6983 RVA: 0x00081BF0 File Offset: 0x0007FDF0
		public override long MaximumTransmissionTimeout
		{
			get
			{
				return (long)((ulong)this.stats.maximumRetransmissionTimeOut);
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001B48 RID: 6984 RVA: 0x00081BFE File Offset: 0x0007FDFE
		public override long MaximumConnections
		{
			get
			{
				return (long)((ulong)this.stats.maximumConnections);
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001B49 RID: 6985 RVA: 0x00081C0C File Offset: 0x0007FE0C
		public override long ConnectionsInitiated
		{
			get
			{
				return (long)((ulong)this.stats.activeOpens);
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001B4A RID: 6986 RVA: 0x00081C1A File Offset: 0x0007FE1A
		public override long ConnectionsAccepted
		{
			get
			{
				return (long)((ulong)this.stats.passiveOpens);
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001B4B RID: 6987 RVA: 0x00081C28 File Offset: 0x0007FE28
		public override long FailedConnectionAttempts
		{
			get
			{
				return (long)((ulong)this.stats.failedConnectionAttempts);
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001B4C RID: 6988 RVA: 0x00081C36 File Offset: 0x0007FE36
		public override long ResetConnections
		{
			get
			{
				return (long)((ulong)this.stats.resetConnections);
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001B4D RID: 6989 RVA: 0x00081C44 File Offset: 0x0007FE44
		public override long CurrentConnections
		{
			get
			{
				return (long)((ulong)this.stats.currentConnections);
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06001B4E RID: 6990 RVA: 0x00081C52 File Offset: 0x0007FE52
		public override long SegmentsReceived
		{
			get
			{
				return (long)((ulong)this.stats.segmentsReceived);
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06001B4F RID: 6991 RVA: 0x00081C60 File Offset: 0x0007FE60
		public override long SegmentsSent
		{
			get
			{
				return (long)((ulong)this.stats.segmentsSent);
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06001B50 RID: 6992 RVA: 0x00081C6E File Offset: 0x0007FE6E
		public override long SegmentsResent
		{
			get
			{
				return (long)((ulong)this.stats.segmentsResent);
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06001B51 RID: 6993 RVA: 0x00081C7C File Offset: 0x0007FE7C
		public override long ErrorsReceived
		{
			get
			{
				return (long)((ulong)this.stats.errorsReceived);
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06001B52 RID: 6994 RVA: 0x00081C8A File Offset: 0x0007FE8A
		public override long ResetsSent
		{
			get
			{
				return (long)((ulong)this.stats.segmentsSentWithReset);
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06001B53 RID: 6995 RVA: 0x00081C98 File Offset: 0x0007FE98
		public override long CumulativeConnections
		{
			get
			{
				return (long)((ulong)this.stats.cumulativeConnections);
			}
		}

		// Token: 0x04001ADF RID: 6879
		private MibTcpStats stats;
	}
}
