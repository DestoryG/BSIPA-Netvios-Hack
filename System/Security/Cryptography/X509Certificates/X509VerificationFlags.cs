using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000473 RID: 1139
	[Flags]
	public enum X509VerificationFlags
	{
		// Token: 0x04002608 RID: 9736
		NoFlag = 0,
		// Token: 0x04002609 RID: 9737
		IgnoreNotTimeValid = 1,
		// Token: 0x0400260A RID: 9738
		IgnoreCtlNotTimeValid = 2,
		// Token: 0x0400260B RID: 9739
		IgnoreNotTimeNested = 4,
		// Token: 0x0400260C RID: 9740
		IgnoreInvalidBasicConstraints = 8,
		// Token: 0x0400260D RID: 9741
		AllowUnknownCertificateAuthority = 16,
		// Token: 0x0400260E RID: 9742
		IgnoreWrongUsage = 32,
		// Token: 0x0400260F RID: 9743
		IgnoreInvalidName = 64,
		// Token: 0x04002610 RID: 9744
		IgnoreInvalidPolicy = 128,
		// Token: 0x04002611 RID: 9745
		IgnoreEndRevocationUnknown = 256,
		// Token: 0x04002612 RID: 9746
		IgnoreCtlSignerRevocationUnknown = 512,
		// Token: 0x04002613 RID: 9747
		IgnoreCertificateAuthorityRevocationUnknown = 1024,
		// Token: 0x04002614 RID: 9748
		IgnoreRootRevocationUnknown = 2048,
		// Token: 0x04002615 RID: 9749
		AllFlags = 4095
	}
}
