using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200007E RID: 126
	internal sealed class ExprBoundLambda : ExprWithType
	{
		// Token: 0x06000446 RID: 1094 RVA: 0x00017FAF File Offset: 0x000161AF
		public ExprBoundLambda(CType type, Scope argumentScope)
			: base(ExpressionKind.BoundLambda, type)
		{
			this.ArgumentScope = argumentScope;
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x00017FC1 File Offset: 0x000161C1
		// (set) Token: 0x06000448 RID: 1096 RVA: 0x00017FC9 File Offset: 0x000161C9
		public ExprBlock OptionalBody { get; set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x00017FD2 File Offset: 0x000161D2
		public AggregateType DelegateType
		{
			get
			{
				return base.Type as AggregateType;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00017FDF File Offset: 0x000161DF
		public Scope ArgumentScope { get; }
	}
}
