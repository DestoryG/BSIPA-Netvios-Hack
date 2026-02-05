using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000461 RID: 1121
	[Flags]
	public enum X500DistinguishedNameFlags
	{
		// Token: 0x04002596 RID: 9622
		None = 0,
		// Token: 0x04002597 RID: 9623
		Reversed = 1,
		// Token: 0x04002598 RID: 9624
		UseSemicolons = 16,
		// Token: 0x04002599 RID: 9625
		DoNotUsePlusSign = 32,
		// Token: 0x0400259A RID: 9626
		DoNotUseQuotes = 64,
		// Token: 0x0400259B RID: 9627
		UseCommas = 128,
		// Token: 0x0400259C RID: 9628
		UseNewLines = 256,
		// Token: 0x0400259D RID: 9629
		UseUTF8Encoding = 4096,
		// Token: 0x0400259E RID: 9630
		UseT61Encoding = 8192,
		// Token: 0x0400259F RID: 9631
		ForceUTF8Encoding = 16384
	}
}
