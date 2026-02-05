using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200009A RID: 154
	internal sealed class ExprUnaryOp : ExprOperator
	{
		// Token: 0x060004D6 RID: 1238 RVA: 0x00018838 File Offset: 0x00016A38
		public ExprUnaryOp(ExpressionKind kind, CType type, Expr operand)
			: base(kind, type)
		{
			this.Child = operand;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00018849 File Offset: 0x00016A49
		public ExprUnaryOp(ExpressionKind kind, CType type, Expr operand, Expr call, MethPropWithInst userMethod)
			: base(kind, type, call, userMethod)
		{
			this.Child = operand;
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x0001885E File Offset: 0x00016A5E
		// (set) Token: 0x060004D9 RID: 1241 RVA: 0x00018866 File Offset: 0x00016A66
		public Expr Child { get; set; }
	}
}
