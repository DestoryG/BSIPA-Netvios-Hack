using System;

namespace Valve.VR
{
	// Token: 0x02000063 RID: 99
	public enum EVRScreenshotError
	{
		// Token: 0x04000520 RID: 1312
		None,
		// Token: 0x04000521 RID: 1313
		RequestFailed,
		// Token: 0x04000522 RID: 1314
		IncompatibleVersion = 100,
		// Token: 0x04000523 RID: 1315
		NotFound,
		// Token: 0x04000524 RID: 1316
		BufferTooSmall,
		// Token: 0x04000525 RID: 1317
		ScreenshotAlreadyInProgress = 108
	}
}
