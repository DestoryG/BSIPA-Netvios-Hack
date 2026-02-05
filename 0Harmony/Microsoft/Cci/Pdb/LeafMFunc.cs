using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000264 RID: 612
	internal struct LeafMFunc
	{
		// Token: 0x04000BEE RID: 3054
		internal uint rvtype;

		// Token: 0x04000BEF RID: 3055
		internal uint classtype;

		// Token: 0x04000BF0 RID: 3056
		internal uint thistype;

		// Token: 0x04000BF1 RID: 3057
		internal byte calltype;

		// Token: 0x04000BF2 RID: 3058
		internal byte reserved;

		// Token: 0x04000BF3 RID: 3059
		internal ushort parmcount;

		// Token: 0x04000BF4 RID: 3060
		internal uint arglist;

		// Token: 0x04000BF5 RID: 3061
		internal int thisadjust;
	}
}
