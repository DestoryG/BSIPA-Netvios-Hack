using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200007B RID: 123
	internal sealed class ExprAssignment : Expr
	{
		// Token: 0x06000435 RID: 1077 RVA: 0x00017EA5 File Offset: 0x000160A5
		public ExprAssignment(Expr lhs, Expr rhs)
			: base(ExpressionKind.Assignment)
		{
			this.LHS = lhs;
			this.RHS = rhs;
			base.Flags = EXPRFLAG.EXF_ASSGOP;
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x00017EC7 File Offset: 0x000160C7
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x00017ED0 File Offset: 0x000160D0
		public Expr LHS
		{
			get
			{
				return this._lhs;
			}
			set
			{
				this._lhs = value;
				base.Type = value.Type;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x00017EF2 File Offset: 0x000160F2
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x00017EFA File Offset: 0x000160FA
		public Expr RHS { get; set; }

		// Token: 0x04000520 RID: 1312
		private Expr _lhs;
	}
}
