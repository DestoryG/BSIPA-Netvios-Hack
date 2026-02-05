using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002DC RID: 732
	internal struct FrameProcSym
	{
		// Token: 0x04000E1C RID: 3612
		internal uint cbFrame;

		// Token: 0x04000E1D RID: 3613
		internal uint cbPad;

		// Token: 0x04000E1E RID: 3614
		internal uint offPad;

		// Token: 0x04000E1F RID: 3615
		internal uint cbSaveRegs;

		// Token: 0x04000E20 RID: 3616
		internal uint offExHdlr;

		// Token: 0x04000E21 RID: 3617
		internal ushort secExHdlr;

		// Token: 0x04000E22 RID: 3618
		internal uint flags;
	}
}
