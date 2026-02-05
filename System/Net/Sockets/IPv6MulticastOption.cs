using System;

namespace System.Net.Sockets
{
	// Token: 0x0200036F RID: 879
	public class IPv6MulticastOption
	{
		// Token: 0x06001FDA RID: 8154 RVA: 0x00094E8D File Offset: 0x0009308D
		public IPv6MulticastOption(IPAddress group, long ifindex)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			if (ifindex < 0L || ifindex > (long)((ulong)(-1)))
			{
				throw new ArgumentOutOfRangeException("ifindex");
			}
			this.Group = group;
			this.InterfaceIndex = ifindex;
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x00094EC6 File Offset: 0x000930C6
		public IPv6MulticastOption(IPAddress group)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			this.Group = group;
			this.InterfaceIndex = 0L;
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06001FDC RID: 8156 RVA: 0x00094EEB File Offset: 0x000930EB
		// (set) Token: 0x06001FDD RID: 8157 RVA: 0x00094EF3 File Offset: 0x000930F3
		public IPAddress Group
		{
			get
			{
				return this.m_Group;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_Group = value;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06001FDE RID: 8158 RVA: 0x00094F0A File Offset: 0x0009310A
		// (set) Token: 0x06001FDF RID: 8159 RVA: 0x00094F12 File Offset: 0x00093112
		public long InterfaceIndex
		{
			get
			{
				return this.m_Interface;
			}
			set
			{
				if (value < 0L || value > (long)((ulong)(-1)))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_Interface = value;
			}
		}

		// Token: 0x04001DDC RID: 7644
		private IPAddress m_Group;

		// Token: 0x04001DDD RID: 7645
		private long m_Interface;
	}
}
