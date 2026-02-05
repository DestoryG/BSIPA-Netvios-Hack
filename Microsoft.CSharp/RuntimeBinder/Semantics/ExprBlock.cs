using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200007D RID: 125
	internal sealed class ExprBlock : ExprStatement
	{
		// Token: 0x06000443 RID: 1091 RVA: 0x00017F8E File Offset: 0x0001618E
		public ExprBlock(ExprStatement optionalStatements)
			: base(ExpressionKind.Block)
		{
			this.OptionalStatements = optionalStatements;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00017F9E File Offset: 0x0001619E
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x00017FA6 File Offset: 0x000161A6
		public ExprStatement OptionalStatements { get; set; }
	}
}
