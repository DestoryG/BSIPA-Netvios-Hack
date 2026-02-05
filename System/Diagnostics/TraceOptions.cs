using System;

namespace System.Diagnostics
{
	// Token: 0x020004B5 RID: 1205
	[Flags]
	public enum TraceOptions
	{
		// Token: 0x040026EC RID: 9964
		None = 0,
		// Token: 0x040026ED RID: 9965
		LogicalOperationStack = 1,
		// Token: 0x040026EE RID: 9966
		DateTime = 2,
		// Token: 0x040026EF RID: 9967
		Timestamp = 4,
		// Token: 0x040026F0 RID: 9968
		ProcessId = 8,
		// Token: 0x040026F1 RID: 9969
		ThreadId = 16,
		// Token: 0x040026F2 RID: 9970
		Callstack = 32
	}
}
