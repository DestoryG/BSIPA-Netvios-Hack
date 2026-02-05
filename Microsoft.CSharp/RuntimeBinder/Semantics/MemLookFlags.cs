using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200004E RID: 78
	[Flags]
	internal enum MemLookFlags : uint
	{
		// Token: 0x040003B4 RID: 948
		None = 0U,
		// Token: 0x040003B5 RID: 949
		Ctor = 2U,
		// Token: 0x040003B6 RID: 950
		NewObj = 16U,
		// Token: 0x040003B7 RID: 951
		Operator = 8U,
		// Token: 0x040003B8 RID: 952
		Indexer = 4U,
		// Token: 0x040003B9 RID: 953
		UserCallable = 256U,
		// Token: 0x040003BA RID: 954
		BaseCall = 64U,
		// Token: 0x040003BB RID: 955
		MustBeInvocable = 536870912U,
		// Token: 0x040003BC RID: 956
		All = 536871262U
	}
}
