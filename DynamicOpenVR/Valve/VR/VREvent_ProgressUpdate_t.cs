using System;

namespace Valve.VR
{
	// Token: 0x0200009E RID: 158
	public struct VREvent_ProgressUpdate_t
	{
		// Token: 0x04000621 RID: 1569
		public ulong ulApplicationPropertyContainer;

		// Token: 0x04000622 RID: 1570
		public ulong pathDevice;

		// Token: 0x04000623 RID: 1571
		public ulong pathInputSource;

		// Token: 0x04000624 RID: 1572
		public ulong pathProgressAction;

		// Token: 0x04000625 RID: 1573
		public ulong pathIcon;

		// Token: 0x04000626 RID: 1574
		public float fProgress;
	}
}
