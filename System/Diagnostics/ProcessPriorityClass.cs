using System;

namespace System.Diagnostics
{
	// Token: 0x020004FE RID: 1278
	public enum ProcessPriorityClass
	{
		// Token: 0x0400288A RID: 10378
		Normal = 32,
		// Token: 0x0400288B RID: 10379
		Idle = 64,
		// Token: 0x0400288C RID: 10380
		High = 128,
		// Token: 0x0400288D RID: 10381
		RealTime = 256,
		// Token: 0x0400288E RID: 10382
		BelowNormal = 16384,
		// Token: 0x0400288F RID: 10383
		AboveNormal = 32768
	}
}
