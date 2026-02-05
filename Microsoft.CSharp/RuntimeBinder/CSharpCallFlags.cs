using System;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x0200000D RID: 13
	[Flags]
	internal enum CSharpCallFlags
	{
		// Token: 0x040000B0 RID: 176
		None = 0,
		// Token: 0x040000B1 RID: 177
		SimpleNameCall = 1,
		// Token: 0x040000B2 RID: 178
		EventHookup = 2,
		// Token: 0x040000B3 RID: 179
		ResultDiscarded = 4
	}
}
