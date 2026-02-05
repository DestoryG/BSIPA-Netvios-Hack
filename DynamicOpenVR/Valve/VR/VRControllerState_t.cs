using System;

namespace Valve.VR
{
	// Token: 0x020000A6 RID: 166
	public struct VRControllerState_t
	{
		// Token: 0x04000636 RID: 1590
		public uint unPacketNum;

		// Token: 0x04000637 RID: 1591
		public ulong ulButtonPressed;

		// Token: 0x04000638 RID: 1592
		public ulong ulButtonTouched;

		// Token: 0x04000639 RID: 1593
		public VRControllerAxis_t rAxis0;

		// Token: 0x0400063A RID: 1594
		public VRControllerAxis_t rAxis1;

		// Token: 0x0400063B RID: 1595
		public VRControllerAxis_t rAxis2;

		// Token: 0x0400063C RID: 1596
		public VRControllerAxis_t rAxis3;

		// Token: 0x0400063D RID: 1597
		public VRControllerAxis_t rAxis4;
	}
}
