using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002F2 RID: 754
	internal class SystemIcmpV4Statistics : IcmpV4Statistics
	{
		// Token: 0x06001A63 RID: 6755 RVA: 0x0007FE44 File Offset: 0x0007E044
		internal SystemIcmpV4Statistics()
		{
			uint icmpStatistics = UnsafeNetInfoNativeMethods.GetIcmpStatistics(out this.stats);
			if (icmpStatistics != 0U)
			{
				throw new NetworkInformationException((int)icmpStatistics);
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001A64 RID: 6756 RVA: 0x0007FE6D File Offset: 0x0007E06D
		public override long MessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.messages);
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001A65 RID: 6757 RVA: 0x0007FE80 File Offset: 0x0007E080
		public override long MessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.messages);
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001A66 RID: 6758 RVA: 0x0007FE93 File Offset: 0x0007E093
		public override long ErrorsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.errors);
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001A67 RID: 6759 RVA: 0x0007FEA6 File Offset: 0x0007E0A6
		public override long ErrorsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.errors);
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001A68 RID: 6760 RVA: 0x0007FEB9 File Offset: 0x0007E0B9
		public override long DestinationUnreachableMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.destinationUnreachables);
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001A69 RID: 6761 RVA: 0x0007FECC File Offset: 0x0007E0CC
		public override long DestinationUnreachableMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.destinationUnreachables);
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001A6A RID: 6762 RVA: 0x0007FEDF File Offset: 0x0007E0DF
		public override long TimeExceededMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.timeExceeds);
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001A6B RID: 6763 RVA: 0x0007FEF2 File Offset: 0x0007E0F2
		public override long TimeExceededMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.timeExceeds);
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001A6C RID: 6764 RVA: 0x0007FF05 File Offset: 0x0007E105
		public override long ParameterProblemsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.parameterProblems);
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001A6D RID: 6765 RVA: 0x0007FF18 File Offset: 0x0007E118
		public override long ParameterProblemsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.parameterProblems);
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001A6E RID: 6766 RVA: 0x0007FF2B File Offset: 0x0007E12B
		public override long SourceQuenchesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.sourceQuenches);
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001A6F RID: 6767 RVA: 0x0007FF3E File Offset: 0x0007E13E
		public override long SourceQuenchesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.sourceQuenches);
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001A70 RID: 6768 RVA: 0x0007FF51 File Offset: 0x0007E151
		public override long RedirectsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.redirects);
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001A71 RID: 6769 RVA: 0x0007FF64 File Offset: 0x0007E164
		public override long RedirectsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.redirects);
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001A72 RID: 6770 RVA: 0x0007FF77 File Offset: 0x0007E177
		public override long EchoRequestsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.echoRequests);
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001A73 RID: 6771 RVA: 0x0007FF8A File Offset: 0x0007E18A
		public override long EchoRequestsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.echoRequests);
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001A74 RID: 6772 RVA: 0x0007FF9D File Offset: 0x0007E19D
		public override long EchoRepliesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.echoReplies);
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001A75 RID: 6773 RVA: 0x0007FFB0 File Offset: 0x0007E1B0
		public override long EchoRepliesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.echoReplies);
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001A76 RID: 6774 RVA: 0x0007FFC3 File Offset: 0x0007E1C3
		public override long TimestampRequestsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.timestampRequests);
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001A77 RID: 6775 RVA: 0x0007FFD6 File Offset: 0x0007E1D6
		public override long TimestampRequestsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.timestampRequests);
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001A78 RID: 6776 RVA: 0x0007FFE9 File Offset: 0x0007E1E9
		public override long TimestampRepliesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.timestampReplies);
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001A79 RID: 6777 RVA: 0x0007FFFC File Offset: 0x0007E1FC
		public override long TimestampRepliesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.timestampReplies);
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001A7A RID: 6778 RVA: 0x0008000F File Offset: 0x0007E20F
		public override long AddressMaskRequestsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.addressMaskRequests);
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001A7B RID: 6779 RVA: 0x00080022 File Offset: 0x0007E222
		public override long AddressMaskRequestsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.addressMaskRequests);
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001A7C RID: 6780 RVA: 0x00080035 File Offset: 0x0007E235
		public override long AddressMaskRepliesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.addressMaskReplies);
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001A7D RID: 6781 RVA: 0x00080048 File Offset: 0x0007E248
		public override long AddressMaskRepliesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.addressMaskReplies);
			}
		}

		// Token: 0x04001A92 RID: 6802
		private MibIcmpInfo stats;
	}
}
