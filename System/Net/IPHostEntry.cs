using System;

namespace System.Net
{
	// Token: 0x0200014D RID: 333
	public class IPHostEntry
	{
		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x0003F563 File Offset: 0x0003D763
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x0003F56B File Offset: 0x0003D76B
		public string HostName
		{
			get
			{
				return this.hostName;
			}
			set
			{
				this.hostName = value;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x0003F574 File Offset: 0x0003D774
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x0003F57C File Offset: 0x0003D77C
		public string[] Aliases
		{
			get
			{
				return this.aliases;
			}
			set
			{
				this.aliases = value;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x0003F585 File Offset: 0x0003D785
		// (set) Token: 0x06000BA5 RID: 2981 RVA: 0x0003F58D File Offset: 0x0003D78D
		public IPAddress[] AddressList
		{
			get
			{
				return this.addressList;
			}
			set
			{
				this.addressList = value;
			}
		}

		// Token: 0x04001108 RID: 4360
		private string hostName;

		// Token: 0x04001109 RID: 4361
		private string[] aliases;

		// Token: 0x0400110A RID: 4362
		private IPAddress[] addressList;

		// Token: 0x0400110B RID: 4363
		internal bool isTrustedHost = true;
	}
}
