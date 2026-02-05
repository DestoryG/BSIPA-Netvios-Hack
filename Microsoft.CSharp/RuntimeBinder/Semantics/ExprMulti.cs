using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000083 RID: 131
	internal sealed class ExprMulti : ExprWithType
	{
		// Token: 0x06000467 RID: 1127 RVA: 0x00018155 File Offset: 0x00016355
		public ExprMulti(CType type, EXPRFLAG flags, Expr left, Expr op)
			: base(ExpressionKind.Multi, type)
		{
			base.Flags = flags;
			this.Left = left;
			this.Operator = op;
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x00018176 File Offset: 0x00016376
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x0001817E File Offset: 0x0001637E
		public Expr Left { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x00018187 File Offset: 0x00016387
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x0001818F File Offset: 0x0001638F
		public Expr Operator { get; set; }
	}
}
