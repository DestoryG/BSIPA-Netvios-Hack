using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200012B RID: 299
	internal struct ChainParameters
	{
		// Token: 0x04001009 RID: 4105
		public uint cbSize;

		// Token: 0x0400100A RID: 4106
		public CertUsageMatch RequestedUsage;

		// Token: 0x0400100B RID: 4107
		public CertUsageMatch RequestedIssuancePolicy;

		// Token: 0x0400100C RID: 4108
		public uint UrlRetrievalTimeout;

		// Token: 0x0400100D RID: 4109
		public int BoolCheckRevocationFreshnessTime;

		// Token: 0x0400100E RID: 4110
		public uint RevocationFreshnessTime;

		// Token: 0x0400100F RID: 4111
		public static readonly uint StructSize = (uint)Marshal.SizeOf(typeof(ChainParameters));
	}
}
