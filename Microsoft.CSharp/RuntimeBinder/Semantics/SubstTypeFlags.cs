using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200005E RID: 94
	[Flags]
	internal enum SubstTypeFlags
	{
		// Token: 0x0400047C RID: 1148
		NormNone = 0,
		// Token: 0x0400047D RID: 1149
		NormClass = 1,
		// Token: 0x0400047E RID: 1150
		NormMeth = 2,
		// Token: 0x0400047F RID: 1151
		NormAll = 3,
		// Token: 0x04000480 RID: 1152
		DenormClass = 4,
		// Token: 0x04000481 RID: 1153
		DenormMeth = 8,
		// Token: 0x04000482 RID: 1154
		DenormAll = 12,
		// Token: 0x04000483 RID: 1155
		NoRefOutDifference = 16
	}
}
