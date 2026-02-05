using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000088 RID: 136
	internal abstract class ExprWithType : Expr
	{
		// Token: 0x06000492 RID: 1170 RVA: 0x000183E1 File Offset: 0x000165E1
		protected ExprWithType(ExpressionKind kind, CType type)
			: base(kind)
		{
			base.Type = type;
		}
	}
}
