using System;

namespace Mono.Cecil
{
	// Token: 0x020000B2 RID: 178
	[Flags]
	internal enum TypeDefinitionTreatment
	{
		// Token: 0x04000253 RID: 595
		None = 0,
		// Token: 0x04000254 RID: 596
		KindMask = 15,
		// Token: 0x04000255 RID: 597
		NormalType = 1,
		// Token: 0x04000256 RID: 598
		NormalAttribute = 2,
		// Token: 0x04000257 RID: 599
		UnmangleWindowsRuntimeName = 3,
		// Token: 0x04000258 RID: 600
		PrefixWindowsRuntimeName = 4,
		// Token: 0x04000259 RID: 601
		RedirectToClrType = 5,
		// Token: 0x0400025A RID: 602
		RedirectToClrAttribute = 6,
		// Token: 0x0400025B RID: 603
		Abstract = 16,
		// Token: 0x0400025C RID: 604
		Internal = 32
	}
}
