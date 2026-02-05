using System;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000031 RID: 49
	internal static class Operators
	{
		// Token: 0x0600022E RID: 558 RVA: 0x00011114 File Offset: 0x0000F314
		private static Dictionary<Name, string> GetOperatorByName()
		{
			Dictionary<Name, string> dictionary = new Dictionary<Name, string>(28)
			{
				{
					NameManager.GetPredefinedName(PredefinedName.PN_OPEQUALS),
					"equals"
				},
				{
					NameManager.GetPredefinedName(PredefinedName.PN_OPCOMPARE),
					"compare"
				}
			};
			foreach (Operators.OperatorInfo operatorInfo in Operators.s_operatorInfos)
			{
				PredefinedName methodName = operatorInfo.MethodName;
				TokenKind tokenKind = operatorInfo.TokenKind;
				if (methodName != PredefinedName.PN_COUNT && tokenKind != TokenKind.Unknown)
				{
					dictionary.Add(NameManager.GetPredefinedName(methodName), TokenFacts.GetText(tokenKind));
				}
			}
			return dictionary;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00011191 File Offset: 0x0000F391
		private static Operators.OperatorInfo GetInfo(OperatorKind op)
		{
			return Operators.s_operatorInfos[(int)op];
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0001119A File Offset: 0x0000F39A
		public static string OperatorOfMethodName(Name name)
		{
			Dictionary<Name, string> dictionary;
			if ((dictionary = Operators.s_operatorsByName) == null)
			{
				dictionary = (Operators.s_operatorsByName = Operators.GetOperatorByName());
			}
			return dictionary[name];
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000111B6 File Offset: 0x0000F3B6
		public static string GetDisplayName(OperatorKind op)
		{
			return TokenFacts.GetText(Operators.GetInfo(op).TokenKind);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x000111C8 File Offset: 0x0000F3C8
		public static ExpressionKind GetExpressionKind(OperatorKind op)
		{
			return Operators.GetInfo(op).ExpressionKind;
		}

		// Token: 0x04000295 RID: 661
		private static readonly Operators.OperatorInfo[] s_operatorInfos = new Operators.OperatorInfo[]
		{
			new Operators.OperatorInfo(TokenKind.Unknown, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Equal, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.PlusEqual, PredefinedName.PN_COUNT, (ExpressionKind)120),
			new Operators.OperatorInfo(TokenKind.MinusEqual, PredefinedName.PN_COUNT, (ExpressionKind)121),
			new Operators.OperatorInfo(TokenKind.SplatEqual, PredefinedName.PN_COUNT, (ExpressionKind)122),
			new Operators.OperatorInfo(TokenKind.SlashEqual, PredefinedName.PN_COUNT, (ExpressionKind)123),
			new Operators.OperatorInfo(TokenKind.PercentEqual, PredefinedName.PN_COUNT, (ExpressionKind)124),
			new Operators.OperatorInfo(TokenKind.AndEqual, PredefinedName.PN_COUNT, (ExpressionKind)127),
			new Operators.OperatorInfo(TokenKind.HatEqual, PredefinedName.PN_COUNT, (ExpressionKind)129),
			new Operators.OperatorInfo(TokenKind.BarEqual, PredefinedName.PN_COUNT, (ExpressionKind)128),
			new Operators.OperatorInfo(TokenKind.LeftShiftEqual, PredefinedName.PN_COUNT, (ExpressionKind)131),
			new Operators.OperatorInfo(TokenKind.RightShiftEqual, PredefinedName.PN_COUNT, (ExpressionKind)132),
			new Operators.OperatorInfo(TokenKind.Question, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.QuestionQuestion, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.LogicalOr, PredefinedName.PN_COUNT, ExpressionKind.LogicalOr),
			new Operators.OperatorInfo(TokenKind.LogicalAnd, PredefinedName.PN_COUNT, ExpressionKind.LogicalAnd),
			new Operators.OperatorInfo(TokenKind.Bar, PredefinedName.PN_OPBITWISEOR, ExpressionKind.BitwiseOr),
			new Operators.OperatorInfo(TokenKind.Hat, PredefinedName.PN_OPXOR, ExpressionKind.BitwiseExclusiveOr),
			new Operators.OperatorInfo(TokenKind.Ampersand, PredefinedName.PN_OPBITWISEAND, ExpressionKind.BitwiseAnd),
			new Operators.OperatorInfo(TokenKind.EqualEqual, PredefinedName.PN_OPEQUALITY, ExpressionKind.Eq),
			new Operators.OperatorInfo(TokenKind.NotEqual, PredefinedName.PN_OPINEQUALITY, ExpressionKind.NotEq),
			new Operators.OperatorInfo(TokenKind.LessThan, PredefinedName.PN_OPLESSTHAN, ExpressionKind.LessThan),
			new Operators.OperatorInfo(TokenKind.LessThanEqual, PredefinedName.PN_OPLESSTHANOREQUAL, ExpressionKind.LessThanOrEqual),
			new Operators.OperatorInfo(TokenKind.GreaterThan, PredefinedName.PN_OPGREATERTHAN, ExpressionKind.GreaterThan),
			new Operators.OperatorInfo(TokenKind.GreaterThanEqual, PredefinedName.PN_OPGREATERTHANOREQUAL, ExpressionKind.GreaterThanOrEqual),
			new Operators.OperatorInfo(TokenKind.Is, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.As, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.LeftShift, PredefinedName.PN_OPLEFTSHIFT, ExpressionKind.LeftShirt),
			new Operators.OperatorInfo(TokenKind.RightShift, PredefinedName.PN_OPRIGHTSHIFT, ExpressionKind.RightShift),
			new Operators.OperatorInfo(TokenKind.Plus, PredefinedName.PN_OPPLUS, ExpressionKind.Add),
			new Operators.OperatorInfo(TokenKind.Minus, PredefinedName.PN_OPMINUS, ExpressionKind.Subtract),
			new Operators.OperatorInfo(TokenKind.Splat, PredefinedName.PN_OPMULTIPLY, ExpressionKind.Multiply),
			new Operators.OperatorInfo(TokenKind.Slash, PredefinedName.PN_OPDIVISION, ExpressionKind.Divide),
			new Operators.OperatorInfo(TokenKind.Percent, PredefinedName.PN_OPMODULUS, ExpressionKind.Modulo),
			new Operators.OperatorInfo(TokenKind.Unknown, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Plus, PredefinedName.PN_OPUNARYPLUS, ExpressionKind.UnaryPlus),
			new Operators.OperatorInfo(TokenKind.Minus, PredefinedName.PN_OPUNARYMINUS, ExpressionKind.Negate),
			new Operators.OperatorInfo(TokenKind.Tilde, PredefinedName.PN_OPCOMPLEMENT, ExpressionKind.BitwiseNot),
			new Operators.OperatorInfo(TokenKind.Bang, PredefinedName.PN_OPNEGATION, ExpressionKind.LogicalNot),
			new Operators.OperatorInfo(TokenKind.PlusPlus, PredefinedName.PN_OPINCREMENT, ExpressionKind.Add),
			new Operators.OperatorInfo(TokenKind.MinusMinus, PredefinedName.PN_OPDECREMENT, ExpressionKind.Subtract),
			new Operators.OperatorInfo(TokenKind.TypeOf, PredefinedName.PN_COUNT, ExpressionKind.TypeOf),
			new Operators.OperatorInfo(TokenKind.Checked, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Unchecked, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.MakeRef, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.RefValue, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.RefType, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.ArgList, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Unknown, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Splat, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Ampersand, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Colon, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.This, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Base, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Null, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.True, PredefinedName.PN_OPTRUE, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.False, PredefinedName.PN_OPFALSE, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Unknown, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Unknown, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Unknown, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.PlusPlus, PredefinedName.PN_COUNT, ExpressionKind.Add),
			new Operators.OperatorInfo(TokenKind.MinusMinus, PredefinedName.PN_COUNT, ExpressionKind.Subtract),
			new Operators.OperatorInfo(TokenKind.Dot, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Implicit, PredefinedName.PN_OPIMPLICITMN, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Explicit, PredefinedName.PN_OPEXPLICITMN, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Unknown, PredefinedName.PN_OPEQUALS, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Unknown, PredefinedName.PN_OPCOMPARE, ExpressionKind.ExpressionKindCount),
			new Operators.OperatorInfo(TokenKind.Unknown, PredefinedName.PN_COUNT, ExpressionKind.ExpressionKindCount)
		};

		// Token: 0x04000296 RID: 662
		private static Dictionary<Name, string> s_operatorsByName;

		// Token: 0x020000E9 RID: 233
		private sealed class OperatorInfo
		{
			// Token: 0x06000732 RID: 1842 RVA: 0x000233C3 File Offset: 0x000215C3
			public OperatorInfo(TokenKind kind, PredefinedName pn, ExpressionKind e)
			{
				this.TokenKind = kind;
				this.MethodName = pn;
				this.ExpressionKind = e;
			}

			// Token: 0x040006E9 RID: 1769
			public readonly TokenKind TokenKind;

			// Token: 0x040006EA RID: 1770
			public readonly PredefinedName MethodName;

			// Token: 0x040006EB RID: 1771
			public readonly ExpressionKind ExpressionKind;
		}
	}
}
