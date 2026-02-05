using System;
using System.Security.Permissions;

namespace System.IO
{
	// Token: 0x02000400 RID: 1024
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal static class Direct
	{
		// Token: 0x040020C8 RID: 8392
		public const int FILE_ACTION_ADDED = 1;

		// Token: 0x040020C9 RID: 8393
		public const int FILE_ACTION_REMOVED = 2;

		// Token: 0x040020CA RID: 8394
		public const int FILE_ACTION_MODIFIED = 3;

		// Token: 0x040020CB RID: 8395
		public const int FILE_ACTION_RENAMED_OLD_NAME = 4;

		// Token: 0x040020CC RID: 8396
		public const int FILE_ACTION_RENAMED_NEW_NAME = 5;

		// Token: 0x040020CD RID: 8397
		public const int FILE_NOTIFY_CHANGE_FILE_NAME = 1;

		// Token: 0x040020CE RID: 8398
		public const int FILE_NOTIFY_CHANGE_DIR_NAME = 2;

		// Token: 0x040020CF RID: 8399
		public const int FILE_NOTIFY_CHANGE_NAME = 3;

		// Token: 0x040020D0 RID: 8400
		public const int FILE_NOTIFY_CHANGE_ATTRIBUTES = 4;

		// Token: 0x040020D1 RID: 8401
		public const int FILE_NOTIFY_CHANGE_SIZE = 8;

		// Token: 0x040020D2 RID: 8402
		public const int FILE_NOTIFY_CHANGE_LAST_WRITE = 16;

		// Token: 0x040020D3 RID: 8403
		public const int FILE_NOTIFY_CHANGE_LAST_ACCESS = 32;

		// Token: 0x040020D4 RID: 8404
		public const int FILE_NOTIFY_CHANGE_CREATION = 64;

		// Token: 0x040020D5 RID: 8405
		public const int FILE_NOTIFY_CHANGE_SECURITY = 256;
	}
}
