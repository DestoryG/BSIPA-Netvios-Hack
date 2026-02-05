using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000B8 RID: 184
	internal struct LocalSym
	{
		// Token: 0x04000410 RID: 1040
		internal uint id;

		// Token: 0x04000411 RID: 1041
		internal uint typind;

		// Token: 0x04000412 RID: 1042
		internal ushort flags;

		// Token: 0x04000413 RID: 1043
		internal uint idParent;

		// Token: 0x04000414 RID: 1044
		internal uint offParent;

		// Token: 0x04000415 RID: 1045
		internal uint expr;

		// Token: 0x04000416 RID: 1046
		internal uint pad0;

		// Token: 0x04000417 RID: 1047
		internal uint pad1;

		// Token: 0x04000418 RID: 1048
		internal string name;
	}
}
