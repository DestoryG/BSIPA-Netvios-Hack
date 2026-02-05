using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200025A RID: 602
	[Flags]
	internal enum LeafPointerAttr : uint
	{
		// Token: 0x04000BC8 RID: 3016
		ptrtype = 31U,
		// Token: 0x04000BC9 RID: 3017
		ptrmode = 224U,
		// Token: 0x04000BCA RID: 3018
		isflat32 = 256U,
		// Token: 0x04000BCB RID: 3019
		isvolatile = 512U,
		// Token: 0x04000BCC RID: 3020
		isconst = 1024U,
		// Token: 0x04000BCD RID: 3021
		isunaligned = 2048U,
		// Token: 0x04000BCE RID: 3022
		isrestrict = 4096U
	}
}
