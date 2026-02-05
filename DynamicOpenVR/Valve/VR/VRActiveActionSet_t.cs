using System;

namespace Valve.VR
{
	// Token: 0x020000C3 RID: 195
	public struct VRActiveActionSet_t
	{
		// Token: 0x04000753 RID: 1875
		public ulong ulActionSet;

		// Token: 0x04000754 RID: 1876
		public ulong ulRestrictedToDevice;

		// Token: 0x04000755 RID: 1877
		public ulong ulSecondaryActionSet;

		// Token: 0x04000756 RID: 1878
		public uint unPadding;

		// Token: 0x04000757 RID: 1879
		public int nPriority;
	}
}
