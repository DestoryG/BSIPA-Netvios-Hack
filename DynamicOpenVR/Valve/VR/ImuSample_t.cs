using System;

namespace Valve.VR
{
	// Token: 0x020000AC RID: 172
	public struct ImuSample_t
	{
		// Token: 0x04000662 RID: 1634
		public double fSampleTime;

		// Token: 0x04000663 RID: 1635
		public HmdVector3d_t vAccel;

		// Token: 0x04000664 RID: 1636
		public HmdVector3d_t vGyro;

		// Token: 0x04000665 RID: 1637
		public uint unOffScaleFlags;
	}
}
