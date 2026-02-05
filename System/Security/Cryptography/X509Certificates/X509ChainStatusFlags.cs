using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200046B RID: 1131
	[Flags]
	public enum X509ChainStatusFlags
	{
		// Token: 0x040025D5 RID: 9685
		NoError = 0,
		// Token: 0x040025D6 RID: 9686
		NotTimeValid = 1,
		// Token: 0x040025D7 RID: 9687
		NotTimeNested = 2,
		// Token: 0x040025D8 RID: 9688
		Revoked = 4,
		// Token: 0x040025D9 RID: 9689
		NotSignatureValid = 8,
		// Token: 0x040025DA RID: 9690
		NotValidForUsage = 16,
		// Token: 0x040025DB RID: 9691
		UntrustedRoot = 32,
		// Token: 0x040025DC RID: 9692
		RevocationStatusUnknown = 64,
		// Token: 0x040025DD RID: 9693
		Cyclic = 128,
		// Token: 0x040025DE RID: 9694
		InvalidExtension = 256,
		// Token: 0x040025DF RID: 9695
		InvalidPolicyConstraints = 512,
		// Token: 0x040025E0 RID: 9696
		InvalidBasicConstraints = 1024,
		// Token: 0x040025E1 RID: 9697
		InvalidNameConstraints = 2048,
		// Token: 0x040025E2 RID: 9698
		HasNotSupportedNameConstraint = 4096,
		// Token: 0x040025E3 RID: 9699
		HasNotDefinedNameConstraint = 8192,
		// Token: 0x040025E4 RID: 9700
		HasNotPermittedNameConstraint = 16384,
		// Token: 0x040025E5 RID: 9701
		HasExcludedNameConstraint = 32768,
		// Token: 0x040025E6 RID: 9702
		PartialChain = 65536,
		// Token: 0x040025E7 RID: 9703
		CtlNotTimeValid = 131072,
		// Token: 0x040025E8 RID: 9704
		CtlNotSignatureValid = 262144,
		// Token: 0x040025E9 RID: 9705
		CtlNotValidForUsage = 524288,
		// Token: 0x040025EA RID: 9706
		OfflineRevocation = 16777216,
		// Token: 0x040025EB RID: 9707
		NoIssuanceChainPolicy = 33554432,
		// Token: 0x040025EC RID: 9708
		ExplicitDistrust = 67108864,
		// Token: 0x040025ED RID: 9709
		HasNotSupportedCriticalExtension = 134217728,
		// Token: 0x040025EE RID: 9710
		HasWeakSignature = 1048576
	}
}
