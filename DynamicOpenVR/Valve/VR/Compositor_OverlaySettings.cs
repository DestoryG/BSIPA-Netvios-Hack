using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x020000A8 RID: 168
	public struct Compositor_OverlaySettings
	{
		// Token: 0x04000646 RID: 1606
		public uint size;

		// Token: 0x04000647 RID: 1607
		[MarshalAs(UnmanagedType.I1)]
		public bool curved;

		// Token: 0x04000648 RID: 1608
		[MarshalAs(UnmanagedType.I1)]
		public bool antialias;

		// Token: 0x04000649 RID: 1609
		public float scale;

		// Token: 0x0400064A RID: 1610
		public float distance;

		// Token: 0x0400064B RID: 1611
		public float alpha;

		// Token: 0x0400064C RID: 1612
		public float uOffset;

		// Token: 0x0400064D RID: 1613
		public float vOffset;

		// Token: 0x0400064E RID: 1614
		public float uScale;

		// Token: 0x0400064F RID: 1615
		public float vScale;

		// Token: 0x04000650 RID: 1616
		public float gridDivs;

		// Token: 0x04000651 RID: 1617
		public float gridWidth;

		// Token: 0x04000652 RID: 1618
		public float gridScale;

		// Token: 0x04000653 RID: 1619
		public HmdMatrix44_t transform;
	}
}
