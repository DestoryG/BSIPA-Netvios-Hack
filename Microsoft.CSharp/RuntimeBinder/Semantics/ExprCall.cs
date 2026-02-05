using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200007F RID: 127
	internal sealed class ExprCall : ExprWithType, IExprWithArgs, IExprWithObject
	{
		// Token: 0x0600044B RID: 1099 RVA: 0x00017FE7 File Offset: 0x000161E7
		public ExprCall(CType type, EXPRFLAG flags, Expr arguments, ExprMemberGroup member, MethWithInst method)
			: base(ExpressionKind.Call, type)
		{
			base.Flags = flags;
			this.OptionalArguments = arguments;
			this.MemberGroup = member;
			this.NullableCallLiftKind = NullableCallLiftKind.NotLifted;
			this.MethWithInst = method;
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x0001801E File Offset: 0x0001621E
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x00018026 File Offset: 0x00016226
		public Expr OptionalArguments { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x0001802F File Offset: 0x0001622F
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x00018037 File Offset: 0x00016237
		public ExprMemberGroup MemberGroup { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x00018040 File Offset: 0x00016240
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x0001804D File Offset: 0x0001624D
		public Expr OptionalObject
		{
			get
			{
				return this.MemberGroup.OptionalObject;
			}
			set
			{
				this.MemberGroup.OptionalObject = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0001805B File Offset: 0x0001625B
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x00018063 File Offset: 0x00016263
		public MethWithInst MethWithInst { get; set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x0001806C File Offset: 0x0001626C
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x00018074 File Offset: 0x00016274
		public PREDEFMETH PredefinedMethod { get; set; } = PREDEFMETH.PM_COUNT;

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0001807D File Offset: 0x0001627D
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x00018085 File Offset: 0x00016285
		public NullableCallLiftKind NullableCallLiftKind { get; set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0001808E File Offset: 0x0001628E
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x00018096 File Offset: 0x00016296
		public Expr PConversions { get; set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x0001809F File Offset: 0x0001629F
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x000180A7 File Offset: 0x000162A7
		public Expr CastOfNonLiftedResultToLiftedType { get; set; }

		// Token: 0x0600045C RID: 1116 RVA: 0x000180B0 File Offset: 0x000162B0
		SymWithType IExprWithArgs.GetSymWithType()
		{
			return this.MethWithInst;
		}
	}
}
