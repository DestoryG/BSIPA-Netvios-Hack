using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000090 RID: 144
	[Flags]
	internal enum CFLAGSYM_FLAGS : ushort
	{
		// Token: 0x04000325 RID: 805
		pcode = 1,
		// Token: 0x04000326 RID: 806
		floatprec = 6,
		// Token: 0x04000327 RID: 807
		floatpkg = 24,
		// Token: 0x04000328 RID: 808
		ambdata = 224,
		// Token: 0x04000329 RID: 809
		ambcode = 1792,
		// Token: 0x0400032A RID: 810
		mode32 = 2048
	}
}
