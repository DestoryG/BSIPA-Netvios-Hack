using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000B5 RID: 181
	internal struct FrameProcSym
	{
		// Token: 0x04000400 RID: 1024
		internal uint cbFrame;

		// Token: 0x04000401 RID: 1025
		internal uint cbPad;

		// Token: 0x04000402 RID: 1026
		internal uint offPad;

		// Token: 0x04000403 RID: 1027
		internal uint cbSaveRegs;

		// Token: 0x04000404 RID: 1028
		internal uint offExHdlr;

		// Token: 0x04000405 RID: 1029
		internal ushort secExHdlr;

		// Token: 0x04000406 RID: 1030
		internal uint flags;
	}
}
