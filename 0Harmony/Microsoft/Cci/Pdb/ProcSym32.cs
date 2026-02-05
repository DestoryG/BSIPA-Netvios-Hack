using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002C7 RID: 711
	internal struct ProcSym32
	{
		// Token: 0x04000D89 RID: 3465
		internal uint parent;

		// Token: 0x04000D8A RID: 3466
		internal uint end;

		// Token: 0x04000D8B RID: 3467
		internal uint next;

		// Token: 0x04000D8C RID: 3468
		internal uint len;

		// Token: 0x04000D8D RID: 3469
		internal uint dbgStart;

		// Token: 0x04000D8E RID: 3470
		internal uint dbgEnd;

		// Token: 0x04000D8F RID: 3471
		internal uint typind;

		// Token: 0x04000D90 RID: 3472
		internal uint off;

		// Token: 0x04000D91 RID: 3473
		internal ushort seg;

		// Token: 0x04000D92 RID: 3474
		internal byte flags;

		// Token: 0x04000D93 RID: 3475
		internal string name;
	}
}
