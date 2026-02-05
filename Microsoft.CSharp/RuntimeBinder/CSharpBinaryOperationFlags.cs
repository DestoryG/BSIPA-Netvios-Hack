using System;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x0200000B RID: 11
	[Flags]
	internal enum CSharpBinaryOperationFlags
	{
		// Token: 0x040000A1 RID: 161
		None = 0,
		// Token: 0x040000A2 RID: 162
		MemberAccess = 1,
		// Token: 0x040000A3 RID: 163
		LogicalOperation = 2
	}
}
