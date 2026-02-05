using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200047F RID: 1151
	[Flags]
	public enum OpenFlags
	{
		// Token: 0x0400263F RID: 9791
		ReadOnly = 0,
		// Token: 0x04002640 RID: 9792
		ReadWrite = 1,
		// Token: 0x04002641 RID: 9793
		MaxAllowed = 2,
		// Token: 0x04002642 RID: 9794
		OpenExistingOnly = 4,
		// Token: 0x04002643 RID: 9795
		IncludeArchived = 8
	}
}
