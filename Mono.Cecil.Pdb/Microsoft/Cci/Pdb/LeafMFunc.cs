using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200003D RID: 61
	internal struct LeafMFunc
	{
		// Token: 0x040001D2 RID: 466
		internal uint rvtype;

		// Token: 0x040001D3 RID: 467
		internal uint classtype;

		// Token: 0x040001D4 RID: 468
		internal uint thistype;

		// Token: 0x040001D5 RID: 469
		internal byte calltype;

		// Token: 0x040001D6 RID: 470
		internal byte reserved;

		// Token: 0x040001D7 RID: 471
		internal ushort parmcount;

		// Token: 0x040001D8 RID: 472
		internal uint arglist;

		// Token: 0x040001D9 RID: 473
		internal int thisadjust;
	}
}
