using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200009C RID: 156
	internal sealed class ExprUserLogicalOp : ExprWithType
	{
		// Token: 0x060004E0 RID: 1248 RVA: 0x000188E4 File Offset: 0x00016AE4
		public ExprUserLogicalOp(CType type, Expr trueFalseCall, ExprCall operatorCall)
			: base(ExpressionKind.UserLogicalOp, type)
		{
			base.Flags = EXPRFLAG.EXF_ASSGOP;
			this.TrueFalseCall = trueFalseCall;
			this.OperatorCall = operatorCall;
			Expr optionalElement = ((ExprList)operatorCall.OptionalArguments).OptionalElement;
			ExprWrap exprWrap;
			this.FirstOperandToExamine = (((exprWrap = optionalElement as ExprWrap) != null) ? exprWrap.OptionalExpression : optionalElement);
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x0001893D File Offset: 0x00016B3D
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x00018945 File Offset: 0x00016B45
		public Expr TrueFalseCall { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0001894E File Offset: 0x00016B4E
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x00018956 File Offset: 0x00016B56
		public ExprCall OperatorCall { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0001895F File Offset: 0x00016B5F
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x00018967 File Offset: 0x00016B67
		public Expr FirstOperandToExamine { get; set; }
	}
}
