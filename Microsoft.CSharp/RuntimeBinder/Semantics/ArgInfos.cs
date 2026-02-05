using System;
using System.Collections.Generic;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200003F RID: 63
	internal sealed class ArgInfos
	{
		// Token: 0x040002FF RID: 767
		public int carg;

		// Token: 0x04000300 RID: 768
		public TypeArray types;

		// Token: 0x04000301 RID: 769
		public bool fHasExprs;

		// Token: 0x04000302 RID: 770
		public List<Expr> prgexpr;
	}
}
