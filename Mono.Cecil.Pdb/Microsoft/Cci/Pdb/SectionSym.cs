using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000BB RID: 187
	internal struct SectionSym
	{
		// Token: 0x04000420 RID: 1056
		internal ushort isec;

		// Token: 0x04000421 RID: 1057
		internal byte align;

		// Token: 0x04000422 RID: 1058
		internal byte bReserved;

		// Token: 0x04000423 RID: 1059
		internal uint rva;

		// Token: 0x04000424 RID: 1060
		internal uint cb;

		// Token: 0x04000425 RID: 1061
		internal uint characteristics;

		// Token: 0x04000426 RID: 1062
		internal string name;
	}
}
