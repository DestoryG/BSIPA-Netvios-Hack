using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000099 RID: 153
	internal sealed class ExprTypeOf : ExprWithType
	{
		// Token: 0x060004D3 RID: 1235 RVA: 0x0001880A File Offset: 0x00016A0A
		public ExprTypeOf(CType type, ExprClass pSourceType)
			: base(ExpressionKind.TypeOf, type)
		{
			base.Flags = EXPRFLAG.EXF_CANTBENULL;
			this.SourceType = pSourceType;
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x00018827 File Offset: 0x00016A27
		// (set) Token: 0x060004D5 RID: 1237 RVA: 0x0001882F File Offset: 0x00016A2F
		public ExprClass SourceType { get; set; }
	}
}
