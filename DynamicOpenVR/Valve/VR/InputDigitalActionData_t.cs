using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x020000BF RID: 191
	public struct InputDigitalActionData_t
	{
		// Token: 0x040006C7 RID: 1735
		[MarshalAs(UnmanagedType.I1)]
		public bool bActive;

		// Token: 0x040006C8 RID: 1736
		public ulong activeOrigin;

		// Token: 0x040006C9 RID: 1737
		[MarshalAs(UnmanagedType.I1)]
		public bool bState;

		// Token: 0x040006CA RID: 1738
		[MarshalAs(UnmanagedType.I1)]
		public bool bChanged;

		// Token: 0x040006CB RID: 1739
		public float fUpdateTime;
	}
}
