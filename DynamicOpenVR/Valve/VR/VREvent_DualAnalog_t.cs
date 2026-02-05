using System;

namespace Valve.VR
{
	// Token: 0x02000098 RID: 152
	public struct VREvent_DualAnalog_t
	{
		// Token: 0x0400060D RID: 1549
		public float x;

		// Token: 0x0400060E RID: 1550
		public float y;

		// Token: 0x0400060F RID: 1551
		public float transformedX;

		// Token: 0x04000610 RID: 1552
		public float transformedY;

		// Token: 0x04000611 RID: 1553
		public EDualAnalogWhich which;
	}
}
