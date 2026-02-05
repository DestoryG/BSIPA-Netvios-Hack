using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002FA RID: 762
	internal class SystemIPv4InterfaceStatistics : IPv4InterfaceStatistics
	{
		// Token: 0x06001AE0 RID: 6880 RVA: 0x0008114E File Offset: 0x0007F34E
		internal SystemIPv4InterfaceStatistics(long index)
		{
			this.ifRow = SystemIPInterfaceStatistics.GetIfEntry2(index);
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001AE1 RID: 6881 RVA: 0x00081162 File Offset: 0x0007F362
		public override long OutputQueueLength
		{
			get
			{
				return (long)this.ifRow.outQLen;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001AE2 RID: 6882 RVA: 0x0008116F File Offset: 0x0007F36F
		public override long BytesSent
		{
			get
			{
				return (long)this.ifRow.outOctets;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x0008117C File Offset: 0x0007F37C
		public override long BytesReceived
		{
			get
			{
				return (long)this.ifRow.inOctets;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001AE4 RID: 6884 RVA: 0x00081189 File Offset: 0x0007F389
		public override long UnicastPacketsSent
		{
			get
			{
				return (long)this.ifRow.outUcastPkts;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001AE5 RID: 6885 RVA: 0x00081196 File Offset: 0x0007F396
		public override long UnicastPacketsReceived
		{
			get
			{
				return (long)this.ifRow.inUcastPkts;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06001AE6 RID: 6886 RVA: 0x000811A3 File Offset: 0x0007F3A3
		public override long NonUnicastPacketsSent
		{
			get
			{
				return (long)this.ifRow.outNUcastPkts;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x000811B0 File Offset: 0x0007F3B0
		public override long NonUnicastPacketsReceived
		{
			get
			{
				return (long)this.ifRow.inNUcastPkts;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06001AE8 RID: 6888 RVA: 0x000811BD File Offset: 0x0007F3BD
		public override long IncomingPacketsDiscarded
		{
			get
			{
				return (long)this.ifRow.inDiscards;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x000811CA File Offset: 0x0007F3CA
		public override long OutgoingPacketsDiscarded
		{
			get
			{
				return (long)this.ifRow.outDiscards;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06001AEA RID: 6890 RVA: 0x000811D7 File Offset: 0x0007F3D7
		public override long IncomingPacketsWithErrors
		{
			get
			{
				return (long)this.ifRow.inErrors;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06001AEB RID: 6891 RVA: 0x000811E4 File Offset: 0x0007F3E4
		public override long OutgoingPacketsWithErrors
		{
			get
			{
				return (long)this.ifRow.outErrors;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06001AEC RID: 6892 RVA: 0x000811F1 File Offset: 0x0007F3F1
		public override long IncomingUnknownProtocolPackets
		{
			get
			{
				return (long)this.ifRow.inUnknownProtos;
			}
		}

		// Token: 0x04001ABA RID: 6842
		private MibIfRow2 ifRow;
	}
}
