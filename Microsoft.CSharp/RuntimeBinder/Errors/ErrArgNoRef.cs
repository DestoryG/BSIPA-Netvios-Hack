using System;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder.Errors
{
	// Token: 0x020000C7 RID: 199
	internal sealed class ErrArgNoRef : ErrArg
	{
		// Token: 0x0600067C RID: 1660 RVA: 0x0001E99C File Offset: 0x0001CB9C
		public ErrArgNoRef(CType pType)
		{
			this.eak = ErrArgKind.Type;
			this.eaf = ErrArgFlags.None;
			this.pType = pType;
		}
	}
}
