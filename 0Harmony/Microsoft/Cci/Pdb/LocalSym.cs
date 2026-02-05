using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002DF RID: 735
	internal struct LocalSym
	{
		// Token: 0x04000E2C RID: 3628
		internal uint id;

		// Token: 0x04000E2D RID: 3629
		internal uint typind;

		// Token: 0x04000E2E RID: 3630
		internal ushort flags;

		// Token: 0x04000E2F RID: 3631
		internal uint idParent;

		// Token: 0x04000E30 RID: 3632
		internal uint offParent;

		// Token: 0x04000E31 RID: 3633
		internal uint expr;

		// Token: 0x04000E32 RID: 3634
		internal uint pad0;

		// Token: 0x04000E33 RID: 3635
		internal uint pad1;

		// Token: 0x04000E34 RID: 3636
		internal string name;
	}
}
