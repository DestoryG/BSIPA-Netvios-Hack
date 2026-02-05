using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000BD RID: 189
	[Flags]
	internal enum EXPORTSYM_FLAGS : ushort
	{
		// Token: 0x0400042D RID: 1069
		fConstant = 1,
		// Token: 0x0400042E RID: 1070
		fData = 2,
		// Token: 0x0400042F RID: 1071
		fPrivate = 4,
		// Token: 0x04000430 RID: 1072
		fNoName = 8,
		// Token: 0x04000431 RID: 1073
		fOrdinal = 16,
		// Token: 0x04000432 RID: 1074
		fForwarder = 32
	}
}
