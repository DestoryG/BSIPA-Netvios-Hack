using System;

namespace System.Net
{
	// Token: 0x020000EC RID: 236
	internal enum FtpOperation
	{
		// Token: 0x04000D82 RID: 3458
		DownloadFile,
		// Token: 0x04000D83 RID: 3459
		ListDirectory,
		// Token: 0x04000D84 RID: 3460
		ListDirectoryDetails,
		// Token: 0x04000D85 RID: 3461
		UploadFile,
		// Token: 0x04000D86 RID: 3462
		UploadFileUnique,
		// Token: 0x04000D87 RID: 3463
		AppendFile,
		// Token: 0x04000D88 RID: 3464
		DeleteFile,
		// Token: 0x04000D89 RID: 3465
		GetDateTimestamp,
		// Token: 0x04000D8A RID: 3466
		GetFileSize,
		// Token: 0x04000D8B RID: 3467
		Rename,
		// Token: 0x04000D8C RID: 3468
		MakeDirectory,
		// Token: 0x04000D8D RID: 3469
		RemoveDirectory,
		// Token: 0x04000D8E RID: 3470
		PrintWorkingDirectory,
		// Token: 0x04000D8F RID: 3471
		Other
	}
}
