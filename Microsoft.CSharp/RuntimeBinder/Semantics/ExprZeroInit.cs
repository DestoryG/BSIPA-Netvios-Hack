using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200009F RID: 159
	internal sealed class ExprZeroInit : ExprWithType
	{
		// Token: 0x0600056F RID: 1391 RVA: 0x0001AF96 File Offset: 0x00019196
		public ExprZeroInit(CType type)
			: base(ExpressionKind.ZeroInit, type)
		{
		}
	}
}
