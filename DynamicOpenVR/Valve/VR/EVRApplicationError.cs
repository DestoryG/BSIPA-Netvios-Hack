using System;

namespace Valve.VR
{
	// Token: 0x0200004E RID: 78
	public enum EVRApplicationError
	{
		// Token: 0x04000479 RID: 1145
		None,
		// Token: 0x0400047A RID: 1146
		AppKeyAlreadyExists = 100,
		// Token: 0x0400047B RID: 1147
		NoManifest,
		// Token: 0x0400047C RID: 1148
		NoApplication,
		// Token: 0x0400047D RID: 1149
		InvalidIndex,
		// Token: 0x0400047E RID: 1150
		UnknownApplication,
		// Token: 0x0400047F RID: 1151
		IPCFailed,
		// Token: 0x04000480 RID: 1152
		ApplicationAlreadyRunning,
		// Token: 0x04000481 RID: 1153
		InvalidManifest,
		// Token: 0x04000482 RID: 1154
		InvalidApplication,
		// Token: 0x04000483 RID: 1155
		LaunchFailed,
		// Token: 0x04000484 RID: 1156
		ApplicationAlreadyStarting,
		// Token: 0x04000485 RID: 1157
		LaunchInProgress,
		// Token: 0x04000486 RID: 1158
		OldApplicationQuitting,
		// Token: 0x04000487 RID: 1159
		TransitionAborted,
		// Token: 0x04000488 RID: 1160
		IsTemplate,
		// Token: 0x04000489 RID: 1161
		SteamVRIsExiting,
		// Token: 0x0400048A RID: 1162
		BufferTooSmall = 200,
		// Token: 0x0400048B RID: 1163
		PropertyNotSet,
		// Token: 0x0400048C RID: 1164
		UnknownProperty,
		// Token: 0x0400048D RID: 1165
		InvalidParameter
	}
}
