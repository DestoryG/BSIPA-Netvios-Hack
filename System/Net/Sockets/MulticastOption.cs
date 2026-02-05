using System;

namespace System.Net.Sockets
{
	// Token: 0x0200036E RID: 878
	public class MulticastOption
	{
		// Token: 0x06001FD1 RID: 8145 RVA: 0x00094DA0 File Offset: 0x00092FA0
		public MulticastOption(IPAddress group, IPAddress mcint)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			if (mcint == null)
			{
				throw new ArgumentNullException("mcint");
			}
			this.Group = group;
			this.LocalAddress = mcint;
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x00094DD2 File Offset: 0x00092FD2
		public MulticastOption(IPAddress group, int interfaceIndex)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			if (interfaceIndex < 0 || interfaceIndex > 16777215)
			{
				throw new ArgumentOutOfRangeException("interfaceIndex");
			}
			this.Group = group;
			this.ifIndex = interfaceIndex;
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x00094E0D File Offset: 0x0009300D
		public MulticastOption(IPAddress group)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			this.Group = group;
			this.LocalAddress = IPAddress.Any;
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x00094E35 File Offset: 0x00093035
		// (set) Token: 0x06001FD5 RID: 8149 RVA: 0x00094E3D File Offset: 0x0009303D
		public IPAddress Group
		{
			get
			{
				return this.group;
			}
			set
			{
				this.group = value;
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x00094E46 File Offset: 0x00093046
		// (set) Token: 0x06001FD7 RID: 8151 RVA: 0x00094E4E File Offset: 0x0009304E
		public IPAddress LocalAddress
		{
			get
			{
				return this.localAddress;
			}
			set
			{
				this.ifIndex = 0;
				this.localAddress = value;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x00094E5E File Offset: 0x0009305E
		// (set) Token: 0x06001FD9 RID: 8153 RVA: 0x00094E66 File Offset: 0x00093066
		public int InterfaceIndex
		{
			get
			{
				return this.ifIndex;
			}
			set
			{
				if (value < 0 || value > 16777215)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.localAddress = null;
				this.ifIndex = value;
			}
		}

		// Token: 0x04001DD9 RID: 7641
		private IPAddress group;

		// Token: 0x04001DDA RID: 7642
		private IPAddress localAddress;

		// Token: 0x04001DDB RID: 7643
		private int ifIndex;
	}
}
