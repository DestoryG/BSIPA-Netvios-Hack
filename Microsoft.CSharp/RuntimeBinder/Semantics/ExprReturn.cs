using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000096 RID: 150
	internal sealed class ExprReturn : ExprStatement, IExprWithObject
	{
		// Token: 0x060004CB RID: 1227 RVA: 0x00018799 File Offset: 0x00016999
		public ExprReturn(Expr optionalObject)
			: base(ExpressionKind.Return)
		{
			this.OptionalObject = optionalObject;
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x000187A9 File Offset: 0x000169A9
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x000187B1 File Offset: 0x000169B1
		public Expr OptionalObject { get; set; }
	}
}
