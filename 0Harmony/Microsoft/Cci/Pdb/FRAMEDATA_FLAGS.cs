using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002F4 RID: 756
	[Flags]
	internal enum FRAMEDATA_FLAGS : uint
	{
		// Token: 0x04000E86 RID: 3718
		fHasSEH = 1U,
		// Token: 0x04000E87 RID: 3719
		fHasEH = 2U,
		// Token: 0x04000E88 RID: 3720
		fIsFunctionStart = 4U
	}
}
