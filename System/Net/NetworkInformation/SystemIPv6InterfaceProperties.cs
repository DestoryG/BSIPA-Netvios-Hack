using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002FF RID: 767
	internal class SystemIPv6InterfaceProperties : IPv6InterfaceProperties
	{
		// Token: 0x06001B28 RID: 6952 RVA: 0x00081710 File Offset: 0x0007F910
		internal SystemIPv6InterfaceProperties(uint index, uint mtu, uint[] zoneIndices)
		{
			this.index = index;
			this.mtu = mtu;
			this.zoneIndices = zoneIndices;
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x0008172D File Offset: 0x0007F92D
		public override int Index
		{
			get
			{
				return (int)this.index;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06001B2A RID: 6954 RVA: 0x00081735 File Offset: 0x0007F935
		public override int Mtu
		{
			get
			{
				return (int)this.mtu;
			}
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x0008173D File Offset: 0x0007F93D
		public override long GetScopeId(ScopeLevel scopeLevel)
		{
			if (scopeLevel < ScopeLevel.None || scopeLevel >= (ScopeLevel)this.zoneIndices.Length)
			{
				throw new ArgumentOutOfRangeException("scopeLevel");
			}
			return (long)((ulong)this.zoneIndices[(int)scopeLevel]);
		}

		// Token: 0x04001ACD RID: 6861
		private uint index;

		// Token: 0x04001ACE RID: 6862
		private uint mtu;

		// Token: 0x04001ACF RID: 6863
		private uint[] zoneIndices;
	}
}
