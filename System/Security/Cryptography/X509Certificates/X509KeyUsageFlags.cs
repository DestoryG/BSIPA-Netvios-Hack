using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000476 RID: 1142
	[Flags]
	public enum X509KeyUsageFlags
	{
		// Token: 0x04002620 RID: 9760
		None = 0,
		// Token: 0x04002621 RID: 9761
		EncipherOnly = 1,
		// Token: 0x04002622 RID: 9762
		CrlSign = 2,
		// Token: 0x04002623 RID: 9763
		KeyCertSign = 4,
		// Token: 0x04002624 RID: 9764
		KeyAgreement = 8,
		// Token: 0x04002625 RID: 9765
		DataEncipherment = 16,
		// Token: 0x04002626 RID: 9766
		KeyEncipherment = 32,
		// Token: 0x04002627 RID: 9767
		NonRepudiation = 64,
		// Token: 0x04002628 RID: 9768
		DigitalSignature = 128,
		// Token: 0x04002629 RID: 9769
		DecipherOnly = 32768
	}
}
