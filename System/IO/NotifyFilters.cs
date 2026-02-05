using System;

namespace System.IO
{
	// Token: 0x020003FA RID: 1018
	[Flags]
	public enum NotifyFilters
	{
		// Token: 0x040020A4 RID: 8356
		FileName = 1,
		// Token: 0x040020A5 RID: 8357
		DirectoryName = 2,
		// Token: 0x040020A6 RID: 8358
		Attributes = 4,
		// Token: 0x040020A7 RID: 8359
		Size = 8,
		// Token: 0x040020A8 RID: 8360
		LastWrite = 16,
		// Token: 0x040020A9 RID: 8361
		LastAccess = 32,
		// Token: 0x040020AA RID: 8362
		CreationTime = 64,
		// Token: 0x040020AB RID: 8363
		Security = 256
	}
}
