using System;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder.Errors
{
	// Token: 0x020000C6 RID: 198
	internal sealed class ErrArgRefOnly : ErrArg
	{
		// Token: 0x0600067B RID: 1659 RVA: 0x0001E98C File Offset: 0x0001CB8C
		public ErrArgRefOnly(Symbol sym)
			: base(sym)
		{
			this.eaf = ErrArgFlags.NoStr;
		}
	}
}
