using System;

namespace Valve.VR
{
	// Token: 0x02000099 RID: 153
	public struct VREvent_HapticVibration_t
	{
		// Token: 0x04000612 RID: 1554
		public ulong containerHandle;

		// Token: 0x04000613 RID: 1555
		public ulong componentHandle;

		// Token: 0x04000614 RID: 1556
		public float fDurationSeconds;

		// Token: 0x04000615 RID: 1557
		public float fFrequency;

		// Token: 0x04000616 RID: 1558
		public float fAmplitude;
	}
}
