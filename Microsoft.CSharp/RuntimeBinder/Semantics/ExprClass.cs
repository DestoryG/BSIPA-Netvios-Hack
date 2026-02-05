using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000081 RID: 129
	internal sealed class ExprClass : ExprWithType
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x00018120 File Offset: 0x00016320
		public ExprClass(CType type)
			: base(ExpressionKind.Class, type)
		{
		}
	}
}
