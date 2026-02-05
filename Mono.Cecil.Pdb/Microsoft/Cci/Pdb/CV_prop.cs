using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200002B RID: 43
	[Flags]
	internal enum CV_prop : ushort
	{
		// Token: 0x0400018E RID: 398
		packed = 1,
		// Token: 0x0400018F RID: 399
		ctor = 2,
		// Token: 0x04000190 RID: 400
		ovlops = 4,
		// Token: 0x04000191 RID: 401
		isnested = 8,
		// Token: 0x04000192 RID: 402
		cnested = 16,
		// Token: 0x04000193 RID: 403
		opassign = 32,
		// Token: 0x04000194 RID: 404
		opcast = 64,
		// Token: 0x04000195 RID: 405
		fwdref = 128,
		// Token: 0x04000196 RID: 406
		scoped = 256
	}
}
