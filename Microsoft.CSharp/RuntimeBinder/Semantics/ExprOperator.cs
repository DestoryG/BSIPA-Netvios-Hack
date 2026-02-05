using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000087 RID: 135
	internal abstract class ExprOperator : ExprWithType
	{
		// Token: 0x0600048B RID: 1163 RVA: 0x00018386 File Offset: 0x00016586
		protected ExprOperator(ExpressionKind kind, CType type)
			: base(kind, type)
		{
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00018390 File Offset: 0x00016590
		protected ExprOperator(ExpressionKind kind, CType type, Expr call, MethPropWithInst userDefinedMethod)
			: this(kind, type)
		{
			this.OptionalUserDefinedCall = call;
			this.UserDefinedCallMethod = userDefinedMethod;
			if (call.HasError)
			{
				base.SetError();
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x000183B7 File Offset: 0x000165B7
		public Expr OptionalUserDefinedCall { get; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x000183BF File Offset: 0x000165BF
		// (set) Token: 0x0600048F RID: 1167 RVA: 0x000183C7 File Offset: 0x000165C7
		public MethWithInst PredefinedMethodToCall { get; set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x000183D0 File Offset: 0x000165D0
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x000183D8 File Offset: 0x000165D8
		public MethPropWithInst UserDefinedCallMethod { get; set; }
	}
}
