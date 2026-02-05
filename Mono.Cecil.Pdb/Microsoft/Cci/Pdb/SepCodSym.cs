using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000B7 RID: 183
	internal struct SepCodSym
	{
		// Token: 0x04000408 RID: 1032
		internal uint parent;

		// Token: 0x04000409 RID: 1033
		internal uint end;

		// Token: 0x0400040A RID: 1034
		internal uint length;

		// Token: 0x0400040B RID: 1035
		internal uint scf;

		// Token: 0x0400040C RID: 1036
		internal uint off;

		// Token: 0x0400040D RID: 1037
		internal uint offParent;

		// Token: 0x0400040E RID: 1038
		internal ushort sec;

		// Token: 0x0400040F RID: 1039
		internal ushort secParent;
	}
}
