using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000044 RID: 68
	[Flags]
	internal enum UnaOpMask
	{
		// Token: 0x04000317 RID: 791
		None = 0,
		// Token: 0x04000318 RID: 792
		Plus = 1,
		// Token: 0x04000319 RID: 793
		Minus = 2,
		// Token: 0x0400031A RID: 794
		Tilde = 4,
		// Token: 0x0400031B RID: 795
		Bang = 8,
		// Token: 0x0400031C RID: 796
		IncDec = 16,
		// Token: 0x0400031D RID: 797
		Signed = 7,
		// Token: 0x0400031E RID: 798
		Unsigned = 5,
		// Token: 0x0400031F RID: 799
		Real = 3,
		// Token: 0x04000320 RID: 800
		Bool = 8
	}
}
