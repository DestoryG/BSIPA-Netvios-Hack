using System;

namespace Valve.VR
{
	// Token: 0x02000051 RID: 81
	public enum ChaperoneCalibrationState
	{
		// Token: 0x040004A6 RID: 1190
		OK = 1,
		// Token: 0x040004A7 RID: 1191
		Warning = 100,
		// Token: 0x040004A8 RID: 1192
		Warning_BaseStationMayHaveMoved,
		// Token: 0x040004A9 RID: 1193
		Warning_BaseStationRemoved,
		// Token: 0x040004AA RID: 1194
		Warning_SeatedBoundsInvalid,
		// Token: 0x040004AB RID: 1195
		Error = 200,
		// Token: 0x040004AC RID: 1196
		Error_BaseStationUninitialized,
		// Token: 0x040004AD RID: 1197
		Error_BaseStationConflict,
		// Token: 0x040004AE RID: 1198
		Error_PlayAreaInvalid,
		// Token: 0x040004AF RID: 1199
		Error_CollisionBoundsInvalid
	}
}
