using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002D4 RID: 724
	internal struct IPOptions
	{
		// Token: 0x060019B8 RID: 6584 RVA: 0x0007E12C File Offset: 0x0007C32C
		internal IPOptions(PingOptions options)
		{
			this.ttl = 128;
			this.tos = 0;
			this.flags = 0;
			this.optionsSize = 0;
			this.optionsData = IntPtr.Zero;
			if (options != null)
			{
				this.ttl = (byte)options.Ttl;
				if (options.DontFragment)
				{
					this.flags = 2;
				}
			}
		}

		// Token: 0x04001A2B RID: 6699
		internal byte ttl;

		// Token: 0x04001A2C RID: 6700
		internal byte tos;

		// Token: 0x04001A2D RID: 6701
		internal byte flags;

		// Token: 0x04001A2E RID: 6702
		internal byte optionsSize;

		// Token: 0x04001A2F RID: 6703
		internal IntPtr optionsData;
	}
}
