using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002D2 RID: 722
	internal struct AttrRegRel
	{
		// Token: 0x04000DDA RID: 3546
		internal uint off;

		// Token: 0x04000DDB RID: 3547
		internal uint typind;

		// Token: 0x04000DDC RID: 3548
		internal ushort reg;

		// Token: 0x04000DDD RID: 3549
		internal uint offCod;

		// Token: 0x04000DDE RID: 3550
		internal ushort segCod;

		// Token: 0x04000DDF RID: 3551
		internal ushort flags;

		// Token: 0x04000DE0 RID: 3552
		internal string name;
	}
}
