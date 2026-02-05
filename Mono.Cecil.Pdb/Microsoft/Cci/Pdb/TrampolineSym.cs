using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000A5 RID: 165
	internal struct TrampolineSym
	{
		// Token: 0x040003A0 RID: 928
		internal ushort trampType;

		// Token: 0x040003A1 RID: 929
		internal ushort cbThunk;

		// Token: 0x040003A2 RID: 930
		internal uint offThunk;

		// Token: 0x040003A3 RID: 931
		internal uint offTarget;

		// Token: 0x040003A4 RID: 932
		internal ushort sectThunk;

		// Token: 0x040003A5 RID: 933
		internal ushort sectTarget;
	}
}
