using System;

namespace Valve.VR
{
	// Token: 0x0200006A RID: 106
	public enum EIOBufferError
	{
		// Token: 0x04000544 RID: 1348
		IOBuffer_Success,
		// Token: 0x04000545 RID: 1349
		IOBuffer_OperationFailed = 100,
		// Token: 0x04000546 RID: 1350
		IOBuffer_InvalidHandle,
		// Token: 0x04000547 RID: 1351
		IOBuffer_InvalidArgument,
		// Token: 0x04000548 RID: 1352
		IOBuffer_PathExists,
		// Token: 0x04000549 RID: 1353
		IOBuffer_PathDoesNotExist,
		// Token: 0x0400054A RID: 1354
		IOBuffer_Permission
	}
}
