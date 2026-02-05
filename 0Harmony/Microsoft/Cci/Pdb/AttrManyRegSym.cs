using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002B1 RID: 689
	internal struct AttrManyRegSym
	{
		// Token: 0x04000D2A RID: 3370
		internal uint typind;

		// Token: 0x04000D2B RID: 3371
		internal uint offCod;

		// Token: 0x04000D2C RID: 3372
		internal ushort segCod;

		// Token: 0x04000D2D RID: 3373
		internal ushort flags;

		// Token: 0x04000D2E RID: 3374
		internal byte count;

		// Token: 0x04000D2F RID: 3375
		internal byte[] reg;

		// Token: 0x04000D30 RID: 3376
		internal string name;
	}
}
