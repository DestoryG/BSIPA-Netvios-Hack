using System;

namespace System.Diagnostics
{
	// Token: 0x0200050A RID: 1290
	public enum ThreadWaitReason
	{
		// Token: 0x040028EB RID: 10475
		Executive,
		// Token: 0x040028EC RID: 10476
		FreePage,
		// Token: 0x040028ED RID: 10477
		PageIn,
		// Token: 0x040028EE RID: 10478
		SystemAllocation,
		// Token: 0x040028EF RID: 10479
		ExecutionDelay,
		// Token: 0x040028F0 RID: 10480
		Suspended,
		// Token: 0x040028F1 RID: 10481
		UserRequest,
		// Token: 0x040028F2 RID: 10482
		EventPairHigh,
		// Token: 0x040028F3 RID: 10483
		EventPairLow,
		// Token: 0x040028F4 RID: 10484
		LpcReceive,
		// Token: 0x040028F5 RID: 10485
		LpcReply,
		// Token: 0x040028F6 RID: 10486
		VirtualMemory,
		// Token: 0x040028F7 RID: 10487
		PageOut,
		// Token: 0x040028F8 RID: 10488
		Unknown
	}
}
