using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000090 RID: 144
	internal sealed class ExprLocal : Expr
	{
		// Token: 0x060004AB RID: 1195 RVA: 0x00018574 File Offset: 0x00016774
		public ExprLocal(LocalVariableSymbol local)
			: base(ExpressionKind.Local)
		{
			base.Flags = EXPRFLAG.EXF_LVALUE;
			this.Local = local;
			base.Type = ((local != null) ? local.GetType() : null);
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x000185A2 File Offset: 0x000167A2
		public LocalVariableSymbol Local { get; }
	}
}
