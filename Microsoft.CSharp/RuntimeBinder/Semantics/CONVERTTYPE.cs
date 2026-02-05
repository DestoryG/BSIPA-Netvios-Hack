using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000036 RID: 54
	[Flags]
	internal enum CONVERTTYPE
	{
		// Token: 0x040002AF RID: 687
		NOUDC = 1,
		// Token: 0x040002B0 RID: 688
		STANDARD = 2,
		// Token: 0x040002B1 RID: 689
		ISEXPLICIT = 4,
		// Token: 0x040002B2 RID: 690
		CHECKOVERFLOW = 8,
		// Token: 0x040002B3 RID: 691
		FORCECAST = 16,
		// Token: 0x040002B4 RID: 692
		STANDARDANDNOUDC = 3
	}
}
