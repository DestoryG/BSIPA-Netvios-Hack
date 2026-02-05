using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002F5 RID: 757
	internal struct FrameData
	{
		// Token: 0x04000E89 RID: 3721
		internal uint ulRvaStart;

		// Token: 0x04000E8A RID: 3722
		internal uint cbBlock;

		// Token: 0x04000E8B RID: 3723
		internal uint cbLocals;

		// Token: 0x04000E8C RID: 3724
		internal uint cbParams;

		// Token: 0x04000E8D RID: 3725
		internal uint cbStkMax;

		// Token: 0x04000E8E RID: 3726
		internal uint frameFunc;

		// Token: 0x04000E8F RID: 3727
		internal ushort cbProlog;

		// Token: 0x04000E90 RID: 3728
		internal ushort cbSavedRegs;

		// Token: 0x04000E91 RID: 3729
		internal uint flags;
	}
}
