using System;

namespace System.Net
{
	// Token: 0x02000130 RID: 304
	internal struct TlsParamaters
	{
		// Token: 0x06000B3E RID: 2878 RVA: 0x0003DC58 File Offset: 0x0003BE58
		public TlsParamaters(SchProtocols protocols)
		{
			this.cAlpnIds = (this.cDisabledCrypto = 0);
			this.pDisabledCrypto = (this.rgstrAlpnIds = IntPtr.Zero);
			this.dwFlags = TlsParamaters.Flags.Zero;
			if (protocols != SchProtocols.Zero)
			{
				this.grbitDisabledProtocols = (uint)(protocols ^ (SchProtocols)(-1));
				return;
			}
			this.grbitDisabledProtocols = 0U;
		}

		// Token: 0x0400102F RID: 4143
		public int cAlpnIds;

		// Token: 0x04001030 RID: 4144
		public IntPtr rgstrAlpnIds;

		// Token: 0x04001031 RID: 4145
		public uint grbitDisabledProtocols;

		// Token: 0x04001032 RID: 4146
		public int cDisabledCrypto;

		// Token: 0x04001033 RID: 4147
		public IntPtr pDisabledCrypto;

		// Token: 0x04001034 RID: 4148
		public TlsParamaters.Flags dwFlags;

		// Token: 0x0200070D RID: 1805
		[Flags]
		public enum Flags
		{
			// Token: 0x04003105 RID: 12549
			Zero = 0,
			// Token: 0x04003106 RID: 12550
			TLS_PARAMS_OPTIONAL = 1
		}
	}
}
