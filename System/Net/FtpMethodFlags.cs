using System;

namespace System.Net
{
	// Token: 0x020000ED RID: 237
	[Flags]
	internal enum FtpMethodFlags
	{
		// Token: 0x04000D91 RID: 3473
		None = 0,
		// Token: 0x04000D92 RID: 3474
		IsDownload = 1,
		// Token: 0x04000D93 RID: 3475
		IsUpload = 2,
		// Token: 0x04000D94 RID: 3476
		TakesParameter = 4,
		// Token: 0x04000D95 RID: 3477
		MayTakeParameter = 8,
		// Token: 0x04000D96 RID: 3478
		DoesNotTakeParameter = 16,
		// Token: 0x04000D97 RID: 3479
		ParameterIsDirectory = 32,
		// Token: 0x04000D98 RID: 3480
		ShouldParseForResponseUri = 64,
		// Token: 0x04000D99 RID: 3481
		HasHttpCommand = 128,
		// Token: 0x04000D9A RID: 3482
		MustChangeWorkingDirectoryToPath = 256
	}
}
