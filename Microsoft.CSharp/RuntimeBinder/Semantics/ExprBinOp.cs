using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200007C RID: 124
	internal sealed class ExprBinOp : ExprOperator
	{
		// Token: 0x0600043A RID: 1082 RVA: 0x00017F03 File Offset: 0x00016103
		public ExprBinOp(ExpressionKind kind, CType type, Expr left, Expr right)
			: base(kind, type)
		{
			base.Flags = EXPRFLAG.EXF_BINOP;
			this.OptionalLeftChild = left;
			this.OptionalRightChild = right;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00017F23 File Offset: 0x00016123
		public ExprBinOp(ExpressionKind kind, CType type, Expr left, Expr right, Expr call, MethPropWithInst userMethod)
			: base(kind, type, call, userMethod)
		{
			base.Flags = EXPRFLAG.EXF_BINOP;
			this.OptionalLeftChild = left;
			this.OptionalRightChild = right;
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x00017F47 File Offset: 0x00016147
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x00017F4F File Offset: 0x0001614F
		public Expr OptionalLeftChild { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x00017F58 File Offset: 0x00016158
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x00017F60 File Offset: 0x00016160
		public Expr OptionalRightChild { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00017F69 File Offset: 0x00016169
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x00017F71 File Offset: 0x00016171
		public bool IsLifted { get; set; }

		// Token: 0x06000442 RID: 1090 RVA: 0x00017F7A File Offset: 0x0001617A
		public void SetAssignment()
		{
			base.Flags |= EXPRFLAG.EXF_ASSGOP;
		}
	}
}
