using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002E2 RID: 738
	internal struct SectionSym
	{
		// Token: 0x04000E3C RID: 3644
		internal ushort isec;

		// Token: 0x04000E3D RID: 3645
		internal byte align;

		// Token: 0x04000E3E RID: 3646
		internal byte bReserved;

		// Token: 0x04000E3F RID: 3647
		internal uint rva;

		// Token: 0x04000E40 RID: 3648
		internal uint cb;

		// Token: 0x04000E41 RID: 3649
		internal uint characteristics;

		// Token: 0x04000E42 RID: 3650
		internal string name;
	}
}
