using System;

namespace Valve.VR
{
	// Token: 0x02000047 RID: 71
	public enum EVRTrackedCameraError
	{
		// Token: 0x0400044A RID: 1098
		None,
		// Token: 0x0400044B RID: 1099
		OperationFailed = 100,
		// Token: 0x0400044C RID: 1100
		InvalidHandle,
		// Token: 0x0400044D RID: 1101
		InvalidFrameHeaderVersion,
		// Token: 0x0400044E RID: 1102
		OutOfHandles,
		// Token: 0x0400044F RID: 1103
		IPCFailure,
		// Token: 0x04000450 RID: 1104
		NotSupportedForThisDevice,
		// Token: 0x04000451 RID: 1105
		SharedMemoryFailure,
		// Token: 0x04000452 RID: 1106
		FrameBufferingFailure,
		// Token: 0x04000453 RID: 1107
		StreamSetupFailure,
		// Token: 0x04000454 RID: 1108
		InvalidGLTextureId,
		// Token: 0x04000455 RID: 1109
		InvalidSharedTextureHandle,
		// Token: 0x04000456 RID: 1110
		FailedToGetGLTextureId,
		// Token: 0x04000457 RID: 1111
		SharedTextureFailure,
		// Token: 0x04000458 RID: 1112
		NoFrameAvailable,
		// Token: 0x04000459 RID: 1113
		InvalidArgument,
		// Token: 0x0400045A RID: 1114
		InvalidFrameBufferSize
	}
}
