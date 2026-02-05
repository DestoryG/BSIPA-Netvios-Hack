using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000A0 RID: 160
	[Flags]
	internal enum CheckConstraintsFlags
	{
		// Token: 0x0400056B RID: 1387
		None = 0,
		// Token: 0x0400056C RID: 1388
		Outer = 1,
		// Token: 0x0400056D RID: 1389
		NoDupErrors = 2,
		// Token: 0x0400056E RID: 1390
		NoErrors = 4
	}
}
