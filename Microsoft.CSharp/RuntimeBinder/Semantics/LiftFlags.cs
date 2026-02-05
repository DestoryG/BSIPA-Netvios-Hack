using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000046 RID: 70
	[Flags]
	internal enum LiftFlags
	{
		// Token: 0x0400032A RID: 810
		None = 0,
		// Token: 0x0400032B RID: 811
		Lift1 = 1,
		// Token: 0x0400032C RID: 812
		Lift2 = 2,
		// Token: 0x0400032D RID: 813
		Convert1 = 4,
		// Token: 0x0400032E RID: 814
		Convert2 = 8
	}
}
