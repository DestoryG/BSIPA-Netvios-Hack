using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x020000C0 RID: 192
	public struct InputPoseActionData_t
	{
		// Token: 0x040006CC RID: 1740
		[MarshalAs(UnmanagedType.I1)]
		public bool bActive;

		// Token: 0x040006CD RID: 1741
		public ulong activeOrigin;

		// Token: 0x040006CE RID: 1742
		public TrackedDevicePose_t pose;
	}
}
