using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000A0 RID: 160
	internal struct ProcSym32
	{
		// Token: 0x0400036D RID: 877
		internal uint parent;

		// Token: 0x0400036E RID: 878
		internal uint end;

		// Token: 0x0400036F RID: 879
		internal uint next;

		// Token: 0x04000370 RID: 880
		internal uint len;

		// Token: 0x04000371 RID: 881
		internal uint dbgStart;

		// Token: 0x04000372 RID: 882
		internal uint dbgEnd;

		// Token: 0x04000373 RID: 883
		internal uint typind;

		// Token: 0x04000374 RID: 884
		internal uint off;

		// Token: 0x04000375 RID: 885
		internal ushort seg;

		// Token: 0x04000376 RID: 886
		internal byte flags;

		// Token: 0x04000377 RID: 887
		internal string name;
	}
}
