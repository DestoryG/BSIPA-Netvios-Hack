using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200004B RID: 75
	internal static class ExpressionKindExtensions
	{
		// Token: 0x0600029F RID: 671 RVA: 0x00012408 File Offset: 0x00010608
		public static bool IsRelational(this ExpressionKind kind)
		{
			return ExpressionKind.Eq <= kind && kind <= ExpressionKind.GreaterThanOrEqual;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00012419 File Offset: 0x00010619
		public static bool IsUnaryOperator(this ExpressionKind kind)
		{
			if (kind <= ExpressionKind.UnaryPlus)
			{
				if (kind - ExpressionKind.True > 4 && kind - ExpressionKind.Negate > 1)
				{
					return false;
				}
			}
			else if (kind != ExpressionKind.BitwiseNot && kind != ExpressionKind.Addr && kind - ExpressionKind.DecimalNegate > 2)
			{
				return false;
			}
			return true;
		}
	}
}
