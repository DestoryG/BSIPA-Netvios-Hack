using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002F4 RID: 756
	internal class SystemIcmpV6Statistics : IcmpV6Statistics
	{
		// Token: 0x06001A7E RID: 6782 RVA: 0x0008005C File Offset: 0x0007E25C
		internal SystemIcmpV6Statistics()
		{
			uint icmpStatisticsEx = UnsafeNetInfoNativeMethods.GetIcmpStatisticsEx(out this.stats, AddressFamily.InterNetworkV6);
			if (icmpStatisticsEx != 0U)
			{
				throw new NetworkInformationException((int)icmpStatisticsEx);
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001A7F RID: 6783 RVA: 0x00080087 File Offset: 0x0007E287
		public override long MessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.dwMsgs);
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001A80 RID: 6784 RVA: 0x0008009A File Offset: 0x0007E29A
		public override long MessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.dwMsgs);
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001A81 RID: 6785 RVA: 0x000800AD File Offset: 0x0007E2AD
		public override long ErrorsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.dwErrors);
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001A82 RID: 6786 RVA: 0x000800C0 File Offset: 0x0007E2C0
		public override long ErrorsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.dwErrors);
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001A83 RID: 6787 RVA: 0x000800D3 File Offset: 0x0007E2D3
		public override long DestinationUnreachableMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)1L))]);
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001A84 RID: 6788 RVA: 0x000800EA File Offset: 0x0007E2EA
		public override long DestinationUnreachableMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)1L))]);
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001A85 RID: 6789 RVA: 0x00080101 File Offset: 0x0007E301
		public override long PacketTooBigMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)2L))]);
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001A86 RID: 6790 RVA: 0x00080118 File Offset: 0x0007E318
		public override long PacketTooBigMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)2L))]);
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001A87 RID: 6791 RVA: 0x0008012F File Offset: 0x0007E32F
		public override long TimeExceededMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)3L))]);
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001A88 RID: 6792 RVA: 0x00080146 File Offset: 0x0007E346
		public override long TimeExceededMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)3L))]);
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001A89 RID: 6793 RVA: 0x0008015D File Offset: 0x0007E35D
		public override long ParameterProblemsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)4L))]);
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001A8A RID: 6794 RVA: 0x00080174 File Offset: 0x0007E374
		public override long ParameterProblemsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)4L))]);
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001A8B RID: 6795 RVA: 0x0008018B File Offset: 0x0007E38B
		public override long EchoRequestsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)128L))]);
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001A8C RID: 6796 RVA: 0x000801A6 File Offset: 0x0007E3A6
		public override long EchoRequestsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)128L))]);
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001A8D RID: 6797 RVA: 0x000801C1 File Offset: 0x0007E3C1
		public override long EchoRepliesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)129L))]);
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001A8E RID: 6798 RVA: 0x000801DC File Offset: 0x0007E3DC
		public override long EchoRepliesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)129L))]);
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001A8F RID: 6799 RVA: 0x000801F7 File Offset: 0x0007E3F7
		public override long MembershipQueriesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)130L))]);
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001A90 RID: 6800 RVA: 0x00080212 File Offset: 0x0007E412
		public override long MembershipQueriesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)130L))]);
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x0008022D File Offset: 0x0007E42D
		public override long MembershipReportsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)131L))]);
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001A92 RID: 6802 RVA: 0x00080248 File Offset: 0x0007E448
		public override long MembershipReportsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)131L))]);
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001A93 RID: 6803 RVA: 0x00080263 File Offset: 0x0007E463
		public override long MembershipReductionsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)132L))]);
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001A94 RID: 6804 RVA: 0x0008027E File Offset: 0x0007E47E
		public override long MembershipReductionsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)132L))]);
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001A95 RID: 6805 RVA: 0x00080299 File Offset: 0x0007E499
		public override long RouterAdvertisementsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)134L))]);
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001A96 RID: 6806 RVA: 0x000802B4 File Offset: 0x0007E4B4
		public override long RouterAdvertisementsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)134L))]);
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001A97 RID: 6807 RVA: 0x000802CF File Offset: 0x0007E4CF
		public override long RouterSolicitsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)133L))]);
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001A98 RID: 6808 RVA: 0x000802EA File Offset: 0x0007E4EA
		public override long RouterSolicitsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)133L))]);
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001A99 RID: 6809 RVA: 0x00080305 File Offset: 0x0007E505
		public override long NeighborAdvertisementsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)136L))]);
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001A9A RID: 6810 RVA: 0x00080320 File Offset: 0x0007E520
		public override long NeighborAdvertisementsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)136L))]);
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001A9B RID: 6811 RVA: 0x0008033B File Offset: 0x0007E53B
		public override long NeighborSolicitsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)135L))]);
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001A9C RID: 6812 RVA: 0x00080356 File Offset: 0x0007E556
		public override long NeighborSolicitsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)135L))]);
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001A9D RID: 6813 RVA: 0x00080371 File Offset: 0x0007E571
		public override long RedirectsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)137L))]);
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001A9E RID: 6814 RVA: 0x0008038C File Offset: 0x0007E58C
		public override long RedirectsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)137L))]);
			}
		}

		// Token: 0x04001AA2 RID: 6818
		private MibIcmpInfoEx stats;
	}
}
