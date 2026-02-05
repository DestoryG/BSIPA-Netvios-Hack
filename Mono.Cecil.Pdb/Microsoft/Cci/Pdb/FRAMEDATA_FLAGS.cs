using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000CD RID: 205
	[Flags]
	internal enum FRAMEDATA_FLAGS : uint
	{
		// Token: 0x0400046A RID: 1130
		fHasSEH = 1U,
		// Token: 0x0400046B RID: 1131
		fHasEH = 2U,
		// Token: 0x0400046C RID: 1132
		fIsFunctionStart = 4U
	}
}
