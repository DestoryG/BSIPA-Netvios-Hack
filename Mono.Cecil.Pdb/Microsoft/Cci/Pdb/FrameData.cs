using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000CE RID: 206
	internal struct FrameData
	{
		// Token: 0x0400046D RID: 1133
		internal uint ulRvaStart;

		// Token: 0x0400046E RID: 1134
		internal uint cbBlock;

		// Token: 0x0400046F RID: 1135
		internal uint cbLocals;

		// Token: 0x04000470 RID: 1136
		internal uint cbParams;

		// Token: 0x04000471 RID: 1137
		internal uint cbStkMax;

		// Token: 0x04000472 RID: 1138
		internal uint frameFunc;

		// Token: 0x04000473 RID: 1139
		internal ushort cbProlog;

		// Token: 0x04000474 RID: 1140
		internal ushort cbSavedRegs;

		// Token: 0x04000475 RID: 1141
		internal uint flags;
	}
}
