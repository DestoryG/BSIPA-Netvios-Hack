using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x020000C1 RID: 193
	public struct InputSkeletalActionData_t
	{
		// Token: 0x040006CF RID: 1743
		[MarshalAs(UnmanagedType.I1)]
		public bool bActive;

		// Token: 0x040006D0 RID: 1744
		public ulong activeOrigin;
	}
}
