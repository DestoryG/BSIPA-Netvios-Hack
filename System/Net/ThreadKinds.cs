using System;

namespace System.Net
{
	// Token: 0x020001C2 RID: 450
	[Flags]
	internal enum ThreadKinds
	{
		// Token: 0x04001462 RID: 5218
		Unknown = 0,
		// Token: 0x04001463 RID: 5219
		User = 1,
		// Token: 0x04001464 RID: 5220
		System = 2,
		// Token: 0x04001465 RID: 5221
		Sync = 4,
		// Token: 0x04001466 RID: 5222
		Async = 8,
		// Token: 0x04001467 RID: 5223
		Timer = 16,
		// Token: 0x04001468 RID: 5224
		CompletionPort = 32,
		// Token: 0x04001469 RID: 5225
		Worker = 64,
		// Token: 0x0400146A RID: 5226
		Finalization = 128,
		// Token: 0x0400146B RID: 5227
		Other = 256,
		// Token: 0x0400146C RID: 5228
		OwnerMask = 3,
		// Token: 0x0400146D RID: 5229
		SyncMask = 12,
		// Token: 0x0400146E RID: 5230
		SourceMask = 496,
		// Token: 0x0400146F RID: 5231
		SafeSources = 352,
		// Token: 0x04001470 RID: 5232
		ThreadPool = 96
	}
}
