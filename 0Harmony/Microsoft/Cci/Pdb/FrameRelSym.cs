using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002C0 RID: 704
	internal struct FrameRelSym
	{
		// Token: 0x04000D68 RID: 3432
		internal int off;

		// Token: 0x04000D69 RID: 3433
		internal uint typind;

		// Token: 0x04000D6A RID: 3434
		internal uint offCod;

		// Token: 0x04000D6B RID: 3435
		internal ushort segCod;

		// Token: 0x04000D6C RID: 3436
		internal ushort flags;

		// Token: 0x04000D6D RID: 3437
		internal string name;
	}
}
