using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000A5 RID: 165
	internal sealed class ErrorType : CType
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0001B720 File Offset: 0x00019920
		public bool HasParent
		{
			get
			{
				return this.nameText != null;
			}
		}

		// Token: 0x0400057D RID: 1405
		public Name nameText;

		// Token: 0x0400057E RID: 1406
		public TypeArray typeArgs;
	}
}
