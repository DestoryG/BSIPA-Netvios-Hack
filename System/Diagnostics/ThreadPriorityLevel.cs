using System;

namespace System.Diagnostics
{
	// Token: 0x02000508 RID: 1288
	public enum ThreadPriorityLevel
	{
		// Token: 0x040028DA RID: 10458
		Idle = -15,
		// Token: 0x040028DB RID: 10459
		Lowest = -2,
		// Token: 0x040028DC RID: 10460
		BelowNormal,
		// Token: 0x040028DD RID: 10461
		Normal,
		// Token: 0x040028DE RID: 10462
		AboveNormal,
		// Token: 0x040028DF RID: 10463
		Highest,
		// Token: 0x040028E0 RID: 10464
		TimeCritical = 15
	}
}
