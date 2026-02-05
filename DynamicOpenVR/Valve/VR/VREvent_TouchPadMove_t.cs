using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000087 RID: 135
	public struct VREvent_TouchPadMove_t
	{
		// Token: 0x040005E0 RID: 1504
		[MarshalAs(UnmanagedType.I1)]
		public bool bFingerDown;

		// Token: 0x040005E1 RID: 1505
		public float flSecondsFingerDown;

		// Token: 0x040005E2 RID: 1506
		public float fValueXFirst;

		// Token: 0x040005E3 RID: 1507
		public float fValueYFirst;

		// Token: 0x040005E4 RID: 1508
		public float fValueXRaw;

		// Token: 0x040005E5 RID: 1509
		public float fValueYRaw;
	}
}
