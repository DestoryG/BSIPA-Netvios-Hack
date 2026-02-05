using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002C8 RID: 712
	internal struct ManProcSym
	{
		// Token: 0x04000D94 RID: 3476
		internal uint parent;

		// Token: 0x04000D95 RID: 3477
		internal uint end;

		// Token: 0x04000D96 RID: 3478
		internal uint next;

		// Token: 0x04000D97 RID: 3479
		internal uint len;

		// Token: 0x04000D98 RID: 3480
		internal uint dbgStart;

		// Token: 0x04000D99 RID: 3481
		internal uint dbgEnd;

		// Token: 0x04000D9A RID: 3482
		internal uint token;

		// Token: 0x04000D9B RID: 3483
		internal uint off;

		// Token: 0x04000D9C RID: 3484
		internal ushort seg;

		// Token: 0x04000D9D RID: 3485
		internal byte flags;

		// Token: 0x04000D9E RID: 3486
		internal ushort retReg;

		// Token: 0x04000D9F RID: 3487
		internal string name;
	}
}
