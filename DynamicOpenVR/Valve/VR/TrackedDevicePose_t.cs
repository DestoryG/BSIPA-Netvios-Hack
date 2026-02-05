using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x0200007C RID: 124
	public struct TrackedDevicePose_t
	{
		// Token: 0x040005BB RID: 1467
		public HmdMatrix34_t mDeviceToAbsoluteTracking;

		// Token: 0x040005BC RID: 1468
		public HmdVector3_t vVelocity;

		// Token: 0x040005BD RID: 1469
		public HmdVector3_t vAngularVelocity;

		// Token: 0x040005BE RID: 1470
		public ETrackingResult eTrackingResult;

		// Token: 0x040005BF RID: 1471
		[MarshalAs(UnmanagedType.I1)]
		public bool bPoseIsValid;

		// Token: 0x040005C0 RID: 1472
		[MarshalAs(UnmanagedType.I1)]
		public bool bDeviceIsConnected;
	}
}
