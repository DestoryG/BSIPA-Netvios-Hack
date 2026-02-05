using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000098 RID: 152
	internal sealed class ExprWrap : Expr
	{
		// Token: 0x060004D1 RID: 1233 RVA: 0x000187D4 File Offset: 0x000169D4
		public ExprWrap(Expr expression)
			: base(ExpressionKind.Wrap)
		{
			this.OptionalExpression = expression;
			base.Type = ((expression != null) ? expression.Type : null);
			base.Flags = EXPRFLAG.EXF_LVALUE;
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00018802 File Offset: 0x00016A02
		public Expr OptionalExpression { get; }
	}
}
