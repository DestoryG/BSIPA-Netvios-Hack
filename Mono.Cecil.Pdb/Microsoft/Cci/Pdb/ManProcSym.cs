using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000A1 RID: 161
	internal struct ManProcSym
	{
		// Token: 0x04000378 RID: 888
		internal uint parent;

		// Token: 0x04000379 RID: 889
		internal uint end;

		// Token: 0x0400037A RID: 890
		internal uint next;

		// Token: 0x0400037B RID: 891
		internal uint len;

		// Token: 0x0400037C RID: 892
		internal uint dbgStart;

		// Token: 0x0400037D RID: 893
		internal uint dbgEnd;

		// Token: 0x0400037E RID: 894
		internal uint token;

		// Token: 0x0400037F RID: 895
		internal uint off;

		// Token: 0x04000380 RID: 896
		internal ushort seg;

		// Token: 0x04000381 RID: 897
		internal byte flags;

		// Token: 0x04000382 RID: 898
		internal ushort retReg;

		// Token: 0x04000383 RID: 899
		internal string name;
	}
}
