using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000251 RID: 593
	[Flags]
	internal enum CV_prop : ushort
	{
		// Token: 0x04000BA8 RID: 2984
		packed = 1,
		// Token: 0x04000BA9 RID: 2985
		ctor = 2,
		// Token: 0x04000BAA RID: 2986
		ovlops = 4,
		// Token: 0x04000BAB RID: 2987
		isnested = 8,
		// Token: 0x04000BAC RID: 2988
		cnested = 16,
		// Token: 0x04000BAD RID: 2989
		opassign = 32,
		// Token: 0x04000BAE RID: 2990
		opcast = 64,
		// Token: 0x04000BAF RID: 2991
		fwdref = 128,
		// Token: 0x04000BB0 RID: 2992
		scoped = 256
	}
}
