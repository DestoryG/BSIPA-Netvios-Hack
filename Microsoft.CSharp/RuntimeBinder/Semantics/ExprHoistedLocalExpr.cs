using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200008C RID: 140
	internal sealed class ExprHoistedLocalExpr : ExprWithType
	{
		// Token: 0x060004A0 RID: 1184 RVA: 0x00018530 File Offset: 0x00016730
		public ExprHoistedLocalExpr(CType type)
			: base(ExpressionKind.HoistedLocalExpression, type)
		{
		}
	}
}
