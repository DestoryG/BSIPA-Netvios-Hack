using System;

namespace Valve.VR
{
	// Token: 0x020000A2 RID: 162
	public struct VREvent_t
	{
		// Token: 0x0400062A RID: 1578
		public uint eventType;

		// Token: 0x0400062B RID: 1579
		public uint trackedDeviceIndex;

		// Token: 0x0400062C RID: 1580
		public float eventAgeSeconds;

		// Token: 0x0400062D RID: 1581
		public VREvent_Data_t data;
	}
}
