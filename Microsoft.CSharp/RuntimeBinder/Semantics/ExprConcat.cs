using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000084 RID: 132
	internal sealed class ExprConcat : ExprWithType
	{
		// Token: 0x0600046C RID: 1132 RVA: 0x00018198 File Offset: 0x00016398
		public ExprConcat(Expr first, Expr second)
			: base(ExpressionKind.Concat, ExprConcat.TypeFromOperands(first, second))
		{
			this.FirstArgument = first;
			this.SecondArgument = second;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000181B8 File Offset: 0x000163B8
		private static CType TypeFromOperands(Expr first, Expr second)
		{
			CType type = first.Type;
			if (type.isPredefType(PredefinedType.PT_STRING))
			{
				return type;
			}
			return second.Type;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x000181DE File Offset: 0x000163DE
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x000181E6 File Offset: 0x000163E6
		public Expr FirstArgument { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x000181EF File Offset: 0x000163EF
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x000181F7 File Offset: 0x000163F7
		public Expr SecondArgument { get; set; }
	}
}
