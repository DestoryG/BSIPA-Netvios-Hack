using System;

namespace Valve.VR
{
	// Token: 0x02000054 RID: 84
	public enum EVRCompositorError
	{
		// Token: 0x040004B6 RID: 1206
		None,
		// Token: 0x040004B7 RID: 1207
		RequestFailed,
		// Token: 0x040004B8 RID: 1208
		IncompatibleVersion = 100,
		// Token: 0x040004B9 RID: 1209
		DoNotHaveFocus,
		// Token: 0x040004BA RID: 1210
		InvalidTexture,
		// Token: 0x040004BB RID: 1211
		IsNotSceneApplication,
		// Token: 0x040004BC RID: 1212
		TextureIsOnWrongDevice,
		// Token: 0x040004BD RID: 1213
		TextureUsesUnsupportedFormat,
		// Token: 0x040004BE RID: 1214
		SharedTexturesNotSupported,
		// Token: 0x040004BF RID: 1215
		IndexOutOfRange,
		// Token: 0x040004C0 RID: 1216
		AlreadySubmitted,
		// Token: 0x040004C1 RID: 1217
		InvalidBounds
	}
}
