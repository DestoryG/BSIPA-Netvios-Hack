using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002F9 RID: 761
	internal class SystemIPInterfaceStatistics : IPInterfaceStatistics
	{
		// Token: 0x06001AD2 RID: 6866 RVA: 0x00081068 File Offset: 0x0007F268
		internal SystemIPInterfaceStatistics(long index)
		{
			this.ifRow = SystemIPInterfaceStatistics.GetIfEntry2(index);
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x0008107C File Offset: 0x0007F27C
		public override long OutputQueueLength
		{
			get
			{
				return (long)this.ifRow.outQLen;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001AD4 RID: 6868 RVA: 0x00081089 File Offset: 0x0007F289
		public override long BytesSent
		{
			get
			{
				return (long)this.ifRow.outOctets;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001AD5 RID: 6869 RVA: 0x00081096 File Offset: 0x0007F296
		public override long BytesReceived
		{
			get
			{
				return (long)this.ifRow.inOctets;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x000810A3 File Offset: 0x0007F2A3
		public override long UnicastPacketsSent
		{
			get
			{
				return (long)this.ifRow.outUcastPkts;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001AD7 RID: 6871 RVA: 0x000810B0 File Offset: 0x0007F2B0
		public override long UnicastPacketsReceived
		{
			get
			{
				return (long)this.ifRow.inUcastPkts;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001AD8 RID: 6872 RVA: 0x000810BD File Offset: 0x0007F2BD
		public override long NonUnicastPacketsSent
		{
			get
			{
				return (long)this.ifRow.outNUcastPkts;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001AD9 RID: 6873 RVA: 0x000810CA File Offset: 0x0007F2CA
		public override long NonUnicastPacketsReceived
		{
			get
			{
				return (long)this.ifRow.inNUcastPkts;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001ADA RID: 6874 RVA: 0x000810D7 File Offset: 0x0007F2D7
		public override long IncomingPacketsDiscarded
		{
			get
			{
				return (long)this.ifRow.inDiscards;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001ADB RID: 6875 RVA: 0x000810E4 File Offset: 0x0007F2E4
		public override long OutgoingPacketsDiscarded
		{
			get
			{
				return (long)this.ifRow.outDiscards;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001ADC RID: 6876 RVA: 0x000810F1 File Offset: 0x0007F2F1
		public override long IncomingPacketsWithErrors
		{
			get
			{
				return (long)this.ifRow.inErrors;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001ADD RID: 6877 RVA: 0x000810FE File Offset: 0x0007F2FE
		public override long OutgoingPacketsWithErrors
		{
			get
			{
				return (long)this.ifRow.outErrors;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001ADE RID: 6878 RVA: 0x0008110B File Offset: 0x0007F30B
		public override long IncomingUnknownProtocolPackets
		{
			get
			{
				return (long)this.ifRow.inUnknownProtos;
			}
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x00081118 File Offset: 0x0007F318
		internal static MibIfRow2 GetIfEntry2(long index)
		{
			MibIfRow2 mibIfRow = default(MibIfRow2);
			if (index == 0L)
			{
				return mibIfRow;
			}
			mibIfRow.interfaceIndex = (uint)index;
			uint ifEntry = UnsafeNetInfoNativeMethods.GetIfEntry2(ref mibIfRow);
			if (ifEntry != 0U)
			{
				throw new NetworkInformationException((int)ifEntry);
			}
			return mibIfRow;
		}

		// Token: 0x04001AB9 RID: 6841
		private MibIfRow2 ifRow;
	}
}
