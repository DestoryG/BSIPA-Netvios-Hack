using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000094 RID: 148
	internal sealed class ExprProperty : ExprWithType, IExprWithArgs, IExprWithObject
	{
		// Token: 0x060004BE RID: 1214 RVA: 0x000186CC File Offset: 0x000168CC
		public ExprProperty(CType type, Expr pOptionalObjectThrough, Expr pOptionalArguments, ExprMemberGroup pMemberGroup, PropWithType pwtSlot, MethWithType mwtSet)
			: base(ExpressionKind.Property, type)
		{
			this.OptionalObjectThrough = pOptionalObjectThrough;
			this.OptionalArguments = pOptionalArguments;
			this.MemberGroup = pMemberGroup;
			if (pwtSlot != null)
			{
				this.PropWithTypeSlot = pwtSlot;
			}
			if (mwtSet != null)
			{
				this.MethWithTypeSet = mwtSet;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0001871C File Offset: 0x0001691C
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x00018724 File Offset: 0x00016924
		public Expr OptionalArguments { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x0001872D File Offset: 0x0001692D
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x00018735 File Offset: 0x00016935
		public ExprMemberGroup MemberGroup { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0001873E File Offset: 0x0001693E
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x0001874B File Offset: 0x0001694B
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

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00018759 File Offset: 0x00016959
		public Expr OptionalObjectThrough { get; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00018761 File Offset: 0x00016961
		public PropWithType PropWithTypeSlot { get; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00018769 File Offset: 0x00016969
		public MethWithType MethWithTypeSet { get; }

		// Token: 0x060004C8 RID: 1224 RVA: 0x00018771 File Offset: 0x00016971
		SymWithType IExprWithArgs.GetSymWithType()
		{
			return this.PropWithTypeSlot;
		}
	}
}
