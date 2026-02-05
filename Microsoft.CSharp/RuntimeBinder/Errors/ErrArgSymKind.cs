using System;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder.Errors
{
	// Token: 0x020000C9 RID: 201
	internal sealed class ErrArgSymKind : ErrArg
	{
		// Token: 0x0600067E RID: 1662 RVA: 0x0001E9D6 File Offset: 0x0001CBD6
		public ErrArgSymKind(Symbol sym)
		{
			this.eak = ErrArgKind.SymKind;
			this.eaf = ErrArgFlags.None;
			this.sk = sym.getKind();
		}
	}
}
