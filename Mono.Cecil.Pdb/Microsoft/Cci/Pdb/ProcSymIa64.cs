using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000AF RID: 175
	internal struct ProcSymIa64
	{
		// Token: 0x040003DC RID: 988
		internal uint parent;

		// Token: 0x040003DD RID: 989
		internal uint end;

		// Token: 0x040003DE RID: 990
		internal uint next;

		// Token: 0x040003DF RID: 991
		internal uint len;

		// Token: 0x040003E0 RID: 992
		internal uint dbgStart;

		// Token: 0x040003E1 RID: 993
		internal uint dbgEnd;

		// Token: 0x040003E2 RID: 994
		internal uint typind;

		// Token: 0x040003E3 RID: 995
		internal uint off;

		// Token: 0x040003E4 RID: 996
		internal ushort seg;

		// Token: 0x040003E5 RID: 997
		internal ushort retReg;

		// Token: 0x040003E6 RID: 998
		internal byte flags;

		// Token: 0x040003E7 RID: 999
		internal string name;
	}
}
