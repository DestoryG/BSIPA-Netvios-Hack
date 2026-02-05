using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200003B RID: 59
	internal static class EXPRExtensions
	{
		// Token: 0x06000260 RID: 608 RVA: 0x00011DAC File Offset: 0x0000FFAC
		public static Expr Map(this Expr expr, ExprFactory factory, Func<Expr, Expr> f)
		{
			if (expr == null)
			{
				return f(expr);
			}
			Expr expr2 = null;
			Expr expr3 = null;
			foreach (Expr expr4 in expr.ToEnumerable())
			{
				Expr expr5 = f(expr4);
				factory.AppendItemToList(expr5, ref expr2, ref expr3);
			}
			return expr2;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00011E18 File Offset: 0x00010018
		public static IEnumerable<Expr> ToEnumerable(this Expr expr)
		{
			Expr exprCur = expr;
			while (exprCur != null)
			{
				ExprList list;
				if ((list = exprCur as ExprList) == null)
				{
					yield return exprCur;
					yield break;
				}
				yield return list.OptionalElement;
				exprCur = list.OptionalNextListNode;
				list = null;
			}
			yield break;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00011E28 File Offset: 0x00010028
		[Conditional("DEBUG")]
		public static void AssertIsBin(this Expr expr)
		{
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00011E2A File Offset: 0x0001002A
		public static bool isLvalue(this Expr expr)
		{
			return expr != null && (expr.Flags & EXPRFLAG.EXF_LVALUE) > (EXPRFLAG)0;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00011E40 File Offset: 0x00010040
		public static bool isChecked(this Expr expr)
		{
			return expr != null && (expr.Flags & EXPRFLAG.EXF_CHECKOVERFLOW) > (EXPRFLAG)0;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00011E58 File Offset: 0x00010058
		public static bool isNull(this Expr expr)
		{
			ExprConstant exprConstant;
			return (exprConstant = expr as ExprConstant) != null && exprConstant.IsOK && expr.Type.fundType() == FUNDTYPE.FT_REF && exprConstant.Val.IsNullRef;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00011E98 File Offset: 0x00010098
		public static bool IsZero(this Expr expr)
		{
			ExprConstant exprConstant;
			return (exprConstant = expr as ExprConstant) != null && exprConstant.IsOK && exprConstant.IsZero;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00011EC0 File Offset: 0x000100C0
		private static Expr GetSeqVal(this Expr expr)
		{
			if (expr == null)
			{
				return null;
			}
			Expr expr2 = expr;
			for (;;)
			{
				ExpressionKind kind = expr2.Kind;
				if (kind != ExpressionKind.Sequence)
				{
					if (kind != ExpressionKind.SequenceReverse)
					{
						break;
					}
					expr2 = ((ExprBinOp)expr2).OptionalLeftChild;
				}
				else
				{
					expr2 = ((ExprBinOp)expr2).OptionalRightChild;
				}
			}
			return expr2;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00011F04 File Offset: 0x00010104
		public static Expr GetConst(this Expr expr)
		{
			Expr seqVal = expr.GetSeqVal();
			if (seqVal == null || (!seqVal.isCONSTANT_OK() && seqVal.Kind != ExpressionKind.ZeroInit))
			{
				return null;
			}
			return seqVal;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00011F30 File Offset: 0x00010130
		public static bool isCONSTANT_OK(this Expr expr)
		{
			return expr != null && expr.Kind == ExpressionKind.Constant && expr.IsOK;
		}
	}
}
