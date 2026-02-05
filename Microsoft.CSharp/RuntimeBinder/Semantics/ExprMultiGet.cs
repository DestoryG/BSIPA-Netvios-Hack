using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000082 RID: 130
	internal sealed class ExprMultiGet : ExprWithType
	{
		// Token: 0x06000464 RID: 1124 RVA: 0x0001812B File Offset: 0x0001632B
		public ExprMultiGet(CType type, EXPRFLAG flags, ExprMulti multi)
			: base(ExpressionKind.MultiGet, type)
		{
			base.Flags = flags;
			this.OptionalMulti = multi;
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00018144 File Offset: 0x00016344
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x0001814C File Offset: 0x0001634C
		public ExprMulti OptionalMulti { get; set; }
	}
}
