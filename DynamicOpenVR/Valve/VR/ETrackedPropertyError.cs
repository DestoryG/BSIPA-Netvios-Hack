using System;

namespace Valve.VR
{
	// Token: 0x0200002E RID: 46
	public enum ETrackedPropertyError
	{
		// Token: 0x04000235 RID: 565
		TrackedProp_Success,
		// Token: 0x04000236 RID: 566
		TrackedProp_WrongDataType,
		// Token: 0x04000237 RID: 567
		TrackedProp_WrongDeviceClass,
		// Token: 0x04000238 RID: 568
		TrackedProp_BufferTooSmall,
		// Token: 0x04000239 RID: 569
		TrackedProp_UnknownProperty,
		// Token: 0x0400023A RID: 570
		TrackedProp_InvalidDevice,
		// Token: 0x0400023B RID: 571
		TrackedProp_CouldNotContactServer,
		// Token: 0x0400023C RID: 572
		TrackedProp_ValueNotProvidedByDevice,
		// Token: 0x0400023D RID: 573
		TrackedProp_StringExceedsMaximumLength,
		// Token: 0x0400023E RID: 574
		TrackedProp_NotYetAvailable,
		// Token: 0x0400023F RID: 575
		TrackedProp_PermissionDenied,
		// Token: 0x04000240 RID: 576
		TrackedProp_InvalidOperation,
		// Token: 0x04000241 RID: 577
		TrackedProp_CannotWriteToWildcards,
		// Token: 0x04000242 RID: 578
		TrackedProp_IPCReadFailure
	}
}
