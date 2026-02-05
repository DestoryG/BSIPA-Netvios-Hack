using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000093 RID: 147
	internal struct CompileSym
	{
		// Token: 0x0400033A RID: 826
		internal uint flags;

		// Token: 0x0400033B RID: 827
		internal ushort machine;

		// Token: 0x0400033C RID: 828
		internal ushort verFEMajor;

		// Token: 0x0400033D RID: 829
		internal ushort verFEMinor;

		// Token: 0x0400033E RID: 830
		internal ushort verFEBuild;

		// Token: 0x0400033F RID: 831
		internal ushort verMajor;

		// Token: 0x04000340 RID: 832
		internal ushort verMinor;

		// Token: 0x04000341 RID: 833
		internal ushort verBuild;

		// Token: 0x04000342 RID: 834
		internal string verSt;

		// Token: 0x04000343 RID: 835
		internal string[] verArgs;
	}
}
