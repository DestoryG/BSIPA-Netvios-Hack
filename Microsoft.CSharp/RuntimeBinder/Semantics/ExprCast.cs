using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000080 RID: 128
	internal sealed class ExprCast : Expr
	{
		// Token: 0x0600045D RID: 1117 RVA: 0x000180B8 File Offset: 0x000162B8
		public ExprCast(EXPRFLAG flags, ExprClass destinationType, Expr argument)
			: base(ExpressionKind.Cast)
		{
			this.Argument = argument;
			base.Flags = flags;
			this.DestinationType = destinationType;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x000180D7 File Offset: 0x000162D7
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x000180DF File Offset: 0x000162DF
		public Expr Argument { get; set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x000180E8 File Offset: 0x000162E8
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x000180F0 File Offset: 0x000162F0
		public ExprClass DestinationType
		{
			get
			{
				return this._destinationType;
			}
			set
			{
				this._destinationType = value;
				base.Type = value.Type;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00018112 File Offset: 0x00016312
		public bool IsBoxingCast
		{
			get
			{
				return (base.Flags & (EXPRFLAG.EXF_CTOR | EXPRFLAG.EXF_UNREALIZEDGOTO)) > (EXPRFLAG)0;
			}
		}

		// Token: 0x0400052F RID: 1327
		private ExprClass _destinationType;
	}
}
