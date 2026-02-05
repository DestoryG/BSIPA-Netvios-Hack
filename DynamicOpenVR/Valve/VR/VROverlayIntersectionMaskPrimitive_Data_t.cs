using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x0200006D RID: 109
	[StructLayout(LayoutKind.Explicit)]
	public struct VROverlayIntersectionMaskPrimitive_Data_t
	{
		// Token: 0x0400056D RID: 1389
		[FieldOffset(0)]
		public IntersectionMaskRectangle_t m_Rectangle;

		// Token: 0x0400056E RID: 1390
		[FieldOffset(0)]
		public IntersectionMaskCircle_t m_Circle;
	}
}
