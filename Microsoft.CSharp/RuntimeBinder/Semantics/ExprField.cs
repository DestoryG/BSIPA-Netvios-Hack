using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200008A RID: 138
	internal sealed class ExprField : ExprWithType, IExprWithObject
	{
		// Token: 0x06000499 RID: 1177 RVA: 0x000184C3 File Offset: 0x000166C3
		public ExprField(CType type, Expr optionalObject, FieldWithType field, bool isLValue)
			: base(ExpressionKind.Field, type)
		{
			base.Flags = (isLValue ? EXPRFLAG.EXF_LVALUE : ((EXPRFLAG)0));
			this.OptionalObject = optionalObject;
			this.FieldWithType = field;
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x000184EE File Offset: 0x000166EE
		// (set) Token: 0x0600049B RID: 1179 RVA: 0x000184F6 File Offset: 0x000166F6
		public Expr OptionalObject { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x000184FF File Offset: 0x000166FF
		public FieldWithType FieldWithType { get; }
	}
}
