using System;

namespace System.Diagnostics
{
	// Token: 0x020004F1 RID: 1265
	internal class ThreadInfo
	{
		// Token: 0x04002869 RID: 10345
		public int threadId;

		// Token: 0x0400286A RID: 10346
		public int processId;

		// Token: 0x0400286B RID: 10347
		public int basePriority;

		// Token: 0x0400286C RID: 10348
		public int currentPriority;

		// Token: 0x0400286D RID: 10349
		public IntPtr startAddress;

		// Token: 0x0400286E RID: 10350
		public ThreadState threadState;

		// Token: 0x0400286F RID: 10351
		public ThreadWaitReason threadWaitReason;
	}
}
