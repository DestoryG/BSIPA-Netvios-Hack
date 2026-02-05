using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x020000BE RID: 190
	public struct InputAnalogActionData_t
	{
		// Token: 0x040006BE RID: 1726
		[MarshalAs(UnmanagedType.I1)]
		public bool bActive;

		// Token: 0x040006BF RID: 1727
		public ulong activeOrigin;

		// Token: 0x040006C0 RID: 1728
		public float x;

		// Token: 0x040006C1 RID: 1729
		public float y;

		// Token: 0x040006C2 RID: 1730
		public float z;

		// Token: 0x040006C3 RID: 1731
		public float deltaX;

		// Token: 0x040006C4 RID: 1732
		public float deltaY;

		// Token: 0x040006C5 RID: 1733
		public float deltaZ;

		// Token: 0x040006C6 RID: 1734
		public float fUpdateTime;
	}
}
