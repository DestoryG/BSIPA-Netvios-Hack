using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000097 RID: 151
	internal abstract class ExprStatement : Expr
	{
		// Token: 0x060004CE RID: 1230 RVA: 0x000187BA File Offset: 0x000169BA
		public ExprStatement(ExpressionKind kind)
			: base(kind)
		{
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x000187C3 File Offset: 0x000169C3
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x000187CB File Offset: 0x000169CB
		public ExprStatement OptionalNextStatement { get; set; }
	}
}
