using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002E4 RID: 740
	[Flags]
	internal enum EXPORTSYM_FLAGS : ushort
	{
		// Token: 0x04000E49 RID: 3657
		fConstant = 1,
		// Token: 0x04000E4A RID: 3658
		fData = 2,
		// Token: 0x04000E4B RID: 3659
		fPrivate = 4,
		// Token: 0x04000E4C RID: 3660
		fNoName = 8,
		// Token: 0x04000E4D RID: 3661
		fOrdinal = 16,
		// Token: 0x04000E4E RID: 3662
		fForwarder = 32
	}
}
