using System;

namespace NSpeex
{
	// Token: 0x0200000E RID: 14
	internal abstract class CodebookSearch
	{
		// Token: 0x06000058 RID: 88
		public abstract void Quantify(float[] target, float[] ak, float[] awk1, float[] awk2, int p, int nsf, float[] exc, int es, float[] r, Bits bits, int complexity);

		// Token: 0x06000059 RID: 89
		public abstract void Unquantify(float[] exc, int es, int nsf, Bits bits);
	}
}
