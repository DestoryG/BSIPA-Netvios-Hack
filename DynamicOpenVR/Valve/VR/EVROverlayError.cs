using System;

namespace Valve.VR
{
	// Token: 0x0200003E RID: 62
	public enum EVROverlayError
	{
		// Token: 0x0400035F RID: 863
		None,
		// Token: 0x04000360 RID: 864
		UnknownOverlay = 10,
		// Token: 0x04000361 RID: 865
		InvalidHandle,
		// Token: 0x04000362 RID: 866
		PermissionDenied,
		// Token: 0x04000363 RID: 867
		OverlayLimitExceeded,
		// Token: 0x04000364 RID: 868
		WrongVisibilityType,
		// Token: 0x04000365 RID: 869
		KeyTooLong,
		// Token: 0x04000366 RID: 870
		NameTooLong,
		// Token: 0x04000367 RID: 871
		KeyInUse,
		// Token: 0x04000368 RID: 872
		WrongTransformType,
		// Token: 0x04000369 RID: 873
		InvalidTrackedDevice,
		// Token: 0x0400036A RID: 874
		InvalidParameter,
		// Token: 0x0400036B RID: 875
		ThumbnailCantBeDestroyed,
		// Token: 0x0400036C RID: 876
		ArrayTooSmall,
		// Token: 0x0400036D RID: 877
		RequestFailed,
		// Token: 0x0400036E RID: 878
		InvalidTexture,
		// Token: 0x0400036F RID: 879
		UnableToLoadFile,
		// Token: 0x04000370 RID: 880
		KeyboardAlreadyInUse,
		// Token: 0x04000371 RID: 881
		NoNeighbor,
		// Token: 0x04000372 RID: 882
		TooManyMaskPrimitives = 29,
		// Token: 0x04000373 RID: 883
		BadMaskPrimitive,
		// Token: 0x04000374 RID: 884
		TextureAlreadyLocked,
		// Token: 0x04000375 RID: 885
		TextureLockCapacityReached,
		// Token: 0x04000376 RID: 886
		TextureNotLocked
	}
}
