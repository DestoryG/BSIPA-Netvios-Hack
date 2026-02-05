using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200008A RID: 138
	internal struct AttrManyRegSym
	{
		// Token: 0x0400030E RID: 782
		internal uint typind;

		// Token: 0x0400030F RID: 783
		internal uint offCod;

		// Token: 0x04000310 RID: 784
		internal ushort segCod;

		// Token: 0x04000311 RID: 785
		internal ushort flags;

		// Token: 0x04000312 RID: 786
		internal byte count;

		// Token: 0x04000313 RID: 787
		internal byte[] reg;

		// Token: 0x04000314 RID: 788
		internal string name;
	}
}
