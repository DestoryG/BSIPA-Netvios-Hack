using System;

namespace Valve.VR
{
	// Token: 0x02000028 RID: 40
	public enum ETrackingResult
	{
		// Token: 0x04000169 RID: 361
		Uninitialized = 1,
		// Token: 0x0400016A RID: 362
		Calibrating_InProgress = 100,
		// Token: 0x0400016B RID: 363
		Calibrating_OutOfRange,
		// Token: 0x0400016C RID: 364
		Running_OK = 200,
		// Token: 0x0400016D RID: 365
		Running_OutOfRange,
		// Token: 0x0400016E RID: 366
		Fallback_RotationOnly = 300
	}
}
