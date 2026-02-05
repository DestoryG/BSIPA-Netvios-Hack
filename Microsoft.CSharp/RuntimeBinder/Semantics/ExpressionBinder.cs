using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder.Errors;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200002C RID: 44
	internal sealed class ExpressionBinder
	{
		// Token: 0x0600016F RID: 367 RVA: 0x00009F20 File Offset: 0x00008120
		private BetterType WhichMethodIsBetterTieBreaker(CandidateFunctionMember node1, CandidateFunctionMember node2, CType pTypeThrough, ArgInfos args)
		{
			MethPropWithInst mpwi = node1.mpwi;
			MethPropWithInst mpwi2 = node2.mpwi;
			if (node1.ctypeLift != node2.ctypeLift)
			{
				if (node1.ctypeLift >= node2.ctypeLift)
				{
					return BetterType.Right;
				}
				return BetterType.Left;
			}
			else
			{
				if (mpwi.TypeArgs.Count != 0)
				{
					if (mpwi2.TypeArgs.Count == 0)
					{
						return BetterType.Right;
					}
				}
				else if (mpwi2.TypeArgs.Count != 0)
				{
					return BetterType.Left;
				}
				if (node1.fExpanded)
				{
					if (!node2.fExpanded)
					{
						return BetterType.Right;
					}
				}
				else if (node2.fExpanded)
				{
					return BetterType.Left;
				}
				BetterType betterType = this.GetGlobalSymbols().CompareTypes(this.RearrangeNamedArguments(mpwi.MethProp().Params, mpwi, pTypeThrough, args), this.RearrangeNamedArguments(mpwi2.MethProp().Params, mpwi2, pTypeThrough, args));
				if (betterType == BetterType.Left || betterType == BetterType.Right)
				{
					return betterType;
				}
				if (mpwi.MethProp().modOptCount == mpwi2.MethProp().modOptCount)
				{
					return BetterType.Neither;
				}
				if (mpwi.MethProp().modOptCount >= mpwi2.MethProp().modOptCount)
				{
					return BetterType.Right;
				}
				return BetterType.Left;
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000A017 File Offset: 0x00008217
		private static int FindName(List<Name> names, Name name)
		{
			return names.IndexOf(name);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000A020 File Offset: 0x00008220
		private TypeArray RearrangeNamedArguments(TypeArray pta, MethPropWithInst mpwi, CType pTypeThrough, ArgInfos args)
		{
			if (!args.fHasExprs)
			{
				return pta;
			}
			CType ctype = ((pTypeThrough != null) ? pTypeThrough : mpwi.GetType());
			CType[] array = new CType[pta.Count];
			MethodOrPropertySymbol methodOrPropertySymbol = ExpressionBinder.GroupToArgsBinder.FindMostDerivedMethod(this.GetSymbolLoader(), mpwi.MethProp(), ctype);
			for (int i = 0; i < pta.Count; i++)
			{
				array[i] = pta[i];
			}
			List<Expr> prgexpr = args.prgexpr;
			for (int j = 0; j < args.carg; j++)
			{
				ExprNamedArgumentSpecification exprNamedArgumentSpecification;
				if ((exprNamedArgumentSpecification = prgexpr[j] as ExprNamedArgumentSpecification) != null)
				{
					int num = ExpressionBinder.FindName(methodOrPropertySymbol.ParameterNames, exprNamedArgumentSpecification.Name);
					CType ctype2 = pta[num];
					for (int k = j; k < num; k++)
					{
						array[k + 1] = array[k];
					}
					array[j] = ctype2;
				}
			}
			return this.GetSymbolLoader().getBSymmgr().AllocParams(pta.Count, array);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A10C File Offset: 0x0000830C
		private BetterType WhichMethodIsBetter(CandidateFunctionMember node1, CandidateFunctionMember node2, CType pTypeThrough, ArgInfos args)
		{
			MethPropWithInst mpwi = node1.mpwi;
			MethPropWithInst mpwi2 = node2.mpwi;
			TypeArray typeArray = this.RearrangeNamedArguments(node1.@params, mpwi, pTypeThrough, args);
			TypeArray typeArray2 = this.RearrangeNamedArguments(node2.@params, mpwi2, pTypeThrough, args);
			if (typeArray == typeArray2)
			{
				return this.WhichMethodIsBetterTieBreaker(node1, node2, pTypeThrough, args);
			}
			BetterType betterType = BetterType.Neither;
			int carg = args.carg;
			for (int i = 0; i < carg; i++)
			{
				object obj = (args.fHasExprs ? args.prgexpr[i] : null);
				CType ctype = typeArray[i];
				CType ctype2 = typeArray2[i];
				object obj2 = obj;
				CType ctype3 = ((obj2 != null) ? obj2.RuntimeObjectActualType : null) ?? args.types[i];
				BetterType betterType2 = this.WhichConversionIsBetter(ctype3, ctype, ctype2);
				if (betterType == BetterType.Right)
				{
					if (betterType2 == BetterType.Left)
					{
						betterType = BetterType.Neither;
						break;
					}
				}
				else if (betterType == BetterType.Left)
				{
					if (betterType2 == BetterType.Right)
					{
						betterType = BetterType.Neither;
						break;
					}
				}
				else if (betterType2 == BetterType.Right || betterType2 == BetterType.Left)
				{
					betterType = betterType2;
				}
			}
			if (typeArray.Count == typeArray2.Count || betterType != BetterType.Neither)
			{
				return betterType;
			}
			if (node1.fExpanded)
			{
				if (!node2.fExpanded)
				{
					return BetterType.Right;
				}
			}
			else if (node2.fExpanded)
			{
				return BetterType.Left;
			}
			if (typeArray.Count == carg)
			{
				return BetterType.Left;
			}
			if (typeArray2.Count == carg)
			{
				return BetterType.Right;
			}
			return BetterType.Neither;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000A250 File Offset: 0x00008450
		private BetterType WhichConversionIsBetter(CType argType, CType p1, CType p2)
		{
			if (p1 == p2)
			{
				return BetterType.Same;
			}
			if (argType == p1)
			{
				return BetterType.Left;
			}
			if (argType == p2)
			{
				return BetterType.Right;
			}
			bool flag = this.canConvert(p1, p2);
			bool flag2 = this.canConvert(p2, p1);
			if (flag == flag2)
			{
				if (p1.isPredefined() && p2.isPredefined())
				{
					PredefinedType predefType = p1.getPredefType();
					if (predefType <= PredefinedType.PT_OBJECT)
					{
						PredefinedType predefType2 = p2.getPredefType();
						if (predefType2 <= PredefinedType.PT_OBJECT)
						{
							return (BetterType)ExpressionBinder.s_betterConversionTable[(int)predefType][(int)predefType2];
						}
					}
				}
				return BetterType.Neither;
			}
			if (!flag)
			{
				return BetterType.Right;
			}
			return BetterType.Left;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A2C0 File Offset: 0x000084C0
		private CandidateFunctionMember FindBestMethod(List<CandidateFunctionMember> list, CType pTypeThrough, ArgInfos args, out CandidateFunctionMember methAmbig1, out CandidateFunctionMember methAmbig2)
		{
			CandidateFunctionMember candidateFunctionMember = null;
			CandidateFunctionMember candidateFunctionMember2 = null;
			bool flag = false;
			CandidateFunctionMember candidateFunctionMember3 = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				CandidateFunctionMember candidateFunctionMember4 = list[i];
				BetterType betterType = this.WhichMethodIsBetter(candidateFunctionMember3, candidateFunctionMember4, pTypeThrough, args);
				if (betterType != BetterType.Left)
				{
					if (betterType != BetterType.Right)
					{
						candidateFunctionMember = candidateFunctionMember3;
						candidateFunctionMember2 = candidateFunctionMember4;
						i++;
						if (i < list.Count)
						{
							candidateFunctionMember4 = list[i];
							candidateFunctionMember3 = candidateFunctionMember4;
						}
						else
						{
							flag = true;
						}
					}
					else
					{
						flag = false;
						candidateFunctionMember3 = candidateFunctionMember4;
					}
				}
				else
				{
					flag = false;
				}
			}
			if (!flag)
			{
				using (List<CandidateFunctionMember>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CandidateFunctionMember candidateFunctionMember5 = enumerator.Current;
						if (candidateFunctionMember5 == candidateFunctionMember3)
						{
							methAmbig1 = null;
							methAmbig2 = null;
							return candidateFunctionMember3;
						}
						switch (this.WhichMethodIsBetter(candidateFunctionMember5, candidateFunctionMember3, pTypeThrough, args))
						{
						case BetterType.Same:
						case BetterType.Neither:
							candidateFunctionMember = candidateFunctionMember3;
							candidateFunctionMember2 = candidateFunctionMember5;
							goto IL_00D5;
						case BetterType.Left:
							goto IL_00D5;
						case BetterType.Right:
							break;
						default:
							goto IL_00D5;
						}
					}
					IL_00D5:;
				}
			}
			if ((candidateFunctionMember != null) & (candidateFunctionMember2 != null))
			{
				methAmbig1 = candidateFunctionMember;
				methAmbig2 = candidateFunctionMember2;
			}
			else
			{
				methAmbig1 = list.First<CandidateFunctionMember>();
				methAmbig2 = list.Skip(1).First<CandidateFunctionMember>();
			}
			return null;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000A3F4 File Offset: 0x000085F4
		private RuntimeBinderException ReportLocalError(LocalVariableSymbol local, CheckLvalueKind kind, bool isNested)
		{
			int num = ((kind == CheckLvalueKind.OutParameter) ? 0 : 1);
			ErrorCode errorCode = ExpressionBinder.s_ReadOnlyLocalErrors[num];
			return this.ErrorContext.Error(errorCode, new ErrArg[] { local.name });
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000A434 File Offset: 0x00008634
		private RuntimeBinderException ReportReadOnlyError(ExprField field, CheckLvalueKind kind, bool isNested)
		{
			bool isStatic = field.FieldWithType.Field().isStatic;
			int num = (isNested ? 4 : 0) + (isStatic ? 2 : 0) + ((kind == CheckLvalueKind.OutParameter) ? 0 : 1);
			ErrorCode errorCode = ExpressionBinder.s_ReadOnlyErrors[num];
			ErrorHandling errorContext = this.ErrorContext;
			ErrorCode errorCode2 = errorCode;
			ErrArg[] array;
			if (!isNested)
			{
				array = Array.Empty<ErrArg>();
			}
			else
			{
				(array = new ErrArg[1])[0] = field.FieldWithType;
			}
			return errorContext.Error(errorCode2, array);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000A4A0 File Offset: 0x000086A0
		private void TryReportLvalueFailure(Expr expr, CheckLvalueKind kind)
		{
			bool flag = false;
			ExprLocal exprLocal;
			while ((exprLocal = expr as ExprLocal) == null || !exprLocal.IsOK)
			{
				Expr expr2 = null;
				ExprProperty exprProperty;
				ExprField exprField;
				if ((exprProperty = expr as ExprProperty) != null)
				{
					expr2 = exprProperty.MemberGroup.OptionalObject;
				}
				else if ((exprField = expr as ExprField) != null)
				{
					if (exprField.FieldWithType.Field().isReadOnly)
					{
						throw this.ReportReadOnlyError(exprField, kind, flag);
					}
					if (!exprField.FieldWithType.Field().isStatic)
					{
						expr2 = exprField.OptionalObject;
					}
				}
				if (expr2 != null && expr2.Type.isStructOrEnum())
				{
					IExprWithArgs exprWithArgs;
					if ((exprWithArgs = expr2 as IExprWithArgs) != null)
					{
						throw this.ErrorContext.Error(ErrorCode.ERR_ReturnNotLValue, new ErrArg[] { exprWithArgs.GetSymWithType() });
					}
					if (expr2 is ExprCast)
					{
						expr2.Flags |= EXPRFLAG.EXF_USERCALLABLE;
						return;
					}
				}
				if (expr2 == null || expr2.isLvalue() || (!(expr is ExprField) && (flag || !(expr is ExprProperty))))
				{
					throw this.ErrorContext.Error(ExpressionBinder.GetStandardLvalueError(kind), Array.Empty<ErrArg>());
				}
				expr = expr2;
				flag = true;
			}
			throw this.ReportLocalError(exprLocal.Local, kind, flag);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000A5CA File Offset: 0x000087CA
		private static void RoundToFloat(double d, out float f)
		{
			f = (float)d;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000A5D0 File Offset: 0x000087D0
		private static long I64(long x)
		{
			return x;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000A5D3 File Offset: 0x000087D3
		private static long I64(ulong x)
		{
			return (long)x;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000A5D6 File Offset: 0x000087D6
		private static ConvKind GetConvKind(PredefinedType ptSrc, PredefinedType ptDst)
		{
			if (ptSrc < PredefinedType.FirstNonSimpleType && ptDst < PredefinedType.FirstNonSimpleType)
			{
				return (ConvKind)(ExpressionBinder.s_simpleTypeConversions[(int)ptSrc][(int)ptDst] & 15);
			}
			if (ptSrc == ptDst || (ptDst == PredefinedType.PT_OBJECT && ptSrc < PredefinedType.PT_COUNT))
			{
				return ConvKind.Implicit;
			}
			if (ptSrc == PredefinedType.PT_OBJECT && ptDst < PredefinedType.PT_COUNT)
			{
				return ConvKind.Explicit;
			}
			return ConvKind.Unknown;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000A60C File Offset: 0x0000880C
		private static bool isUserDefinedConversion(PredefinedType ptSrc, PredefinedType ptDst)
		{
			return ptSrc < PredefinedType.FirstNonSimpleType && ptDst < PredefinedType.FirstNonSimpleType && (ExpressionBinder.s_simpleTypeConversions[(int)ptSrc][(int)ptDst] & 64) > 0;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000A629 File Offset: 0x00008829
		private BetterType WhichSimpleConversionIsBetter(PredefinedType pt1, PredefinedType pt2)
		{
			return (BetterType)ExpressionBinder.s_simpleTypeBetter[(int)pt1][(int)pt2];
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000A634 File Offset: 0x00008834
		private BetterType WhichTypeIsBetter(PredefinedType pt1, PredefinedType pt2, CType typeGiven)
		{
			if (pt1 == pt2)
			{
				return BetterType.Same;
			}
			if (typeGiven.isPredefType(pt1))
			{
				return BetterType.Left;
			}
			if (typeGiven.isPredefType(pt2))
			{
				return BetterType.Right;
			}
			if (pt1 <= PredefinedType.PT_STRING && pt2 <= PredefinedType.PT_STRING)
			{
				return this.WhichSimpleConversionIsBetter(pt1, pt2);
			}
			if (pt2 == PredefinedType.PT_OBJECT && pt1 < PredefinedType.PT_COUNT)
			{
				return BetterType.Left;
			}
			if (pt1 == PredefinedType.PT_OBJECT && pt2 < PredefinedType.PT_COUNT)
			{
				return BetterType.Right;
			}
			return this.WhichTypeIsBetter(this.GetPredefindType(pt1), this.GetPredefindType(pt2), typeGiven);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000A6A0 File Offset: 0x000088A0
		private BetterType WhichTypeIsBetter(CType type1, CType type2, CType typeGiven)
		{
			if (type1 == type2)
			{
				return BetterType.Same;
			}
			if (typeGiven == type1)
			{
				return BetterType.Left;
			}
			if (typeGiven == type2)
			{
				return BetterType.Right;
			}
			bool flag = this.canConvert(type1, type2);
			bool flag2 = this.canConvert(type2, type1);
			if (flag != flag2)
			{
				if (!flag)
				{
					return BetterType.Right;
				}
				return BetterType.Left;
			}
			else
			{
				NullableType nullableType;
				NullableType nullableType2;
				if ((nullableType = type1 as NullableType) == null || (nullableType2 = type2 as NullableType) == null || !nullableType.UnderlyingType.isPredefined() || !nullableType2.UnderlyingType.isPredefined())
				{
					return BetterType.Neither;
				}
				PredefinedType predefType = (type1 as NullableType).UnderlyingType.getPredefType();
				PredefinedType predefType2 = (type2 as NullableType).UnderlyingType.getPredefType();
				if (predefType <= PredefinedType.PT_STRING && predefType2 <= PredefinedType.PT_STRING)
				{
					return this.WhichSimpleConversionIsBetter(predefType, predefType2);
				}
				return BetterType.Neither;
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000A748 File Offset: 0x00008948
		private bool canConvert(CType src, CType dest, CONVERTTYPE flags)
		{
			ExprClass exprClass = this.ExprFactory.CreateClass(dest);
			return this.BindImplicitConversion(null, src, exprClass, dest, flags);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000A76D File Offset: 0x0000896D
		public bool canConvert(CType src, CType dest)
		{
			return this.canConvert(src, dest, (CONVERTTYPE)0);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000A778 File Offset: 0x00008978
		private bool canConvert(Expr expr, CType dest)
		{
			return this.canConvert(expr, dest, (CONVERTTYPE)0);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000A784 File Offset: 0x00008984
		private bool canConvert(Expr expr, CType dest, CONVERTTYPE flags)
		{
			ExprClass exprClass = this.ExprFactory.CreateClass(dest);
			return this.BindImplicitConversion(expr, expr.Type, exprClass, dest, flags);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000A7AE File Offset: 0x000089AE
		private Expr mustConvertCore(Expr expr, ExprClass destExpr)
		{
			return this.mustConvertCore(expr, destExpr, (CONVERTTYPE)0);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000A7BC File Offset: 0x000089BC
		private Expr mustConvertCore(Expr expr, ExprClass destExpr, CONVERTTYPE flags)
		{
			CType type = destExpr.Type;
			Expr expr2;
			if (this.BindImplicitConversion(expr, expr.Type, destExpr, type, out expr2, flags))
			{
				this.checkUnsafe(expr.Type);
				this.checkUnsafe(type);
				return expr2;
			}
			if (!expr.IsOK || type is ErrorType)
			{
				expr2 = this.ExprFactory.CreateCast((EXPRFLAG)0, destExpr, expr);
				expr2.SetError();
				return expr2;
			}
			FUNDTYPE fundtype = expr.Type.fundType();
			FUNDTYPE fundtype2 = type.fundType();
			ExprConstant exprConstant;
			if ((exprConstant = expr as ExprConstant) != null && exprConstant.IsOK && expr.Type.isSimpleType() && type.isSimpleType() && ((fundtype == FUNDTYPE.FT_I4 && (fundtype2 <= FUNDTYPE.FT_U4 || fundtype2 == FUNDTYPE.FT_U8)) || (fundtype == FUNDTYPE.FT_I8 && fundtype2 == FUNDTYPE.FT_U8)))
			{
				string text = exprConstant.Int64Value.ToString(CultureInfo.InvariantCulture);
				throw this.ErrorContext.Error(ErrorCode.ERR_ConstOutOfRange, new ErrArg[] { text, type });
			}
			if (expr.Type is NullType && type.fundType() != FUNDTYPE.FT_REF)
			{
				throw this.ErrorContext.Error(ErrorCode.ERR_ValueCantBeNull, new ErrArg[] { type });
			}
			throw this.ErrorContext.Error(this.canCast(expr.Type, type, flags) ? ErrorCode.ERR_NoImplicitConvCast : ErrorCode.ERR_NoImplicitConv, new ErrArg[]
			{
				new ErrArg(expr.Type, ErrArgFlags.Unique),
				new ErrArg(type, ErrArgFlags.Unique)
			});
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000A92B File Offset: 0x00008B2B
		public Expr tryConvert(Expr expr, CType dest)
		{
			return this.tryConvert(expr, dest, (CONVERTTYPE)0);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000A938 File Offset: 0x00008B38
		private Expr tryConvert(Expr expr, CType dest, CONVERTTYPE flags)
		{
			ExprClass exprClass = this.ExprFactory.CreateClass(dest);
			Expr expr2;
			if (this.BindImplicitConversion(expr, expr.Type, exprClass, dest, out expr2, flags))
			{
				this.checkUnsafe(expr.Type);
				this.checkUnsafe(dest);
				return expr2;
			}
			return null;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000A97C File Offset: 0x00008B7C
		public Expr mustConvert(Expr expr, CType dest)
		{
			return this.mustConvert(expr, dest, (CONVERTTYPE)0);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000A988 File Offset: 0x00008B88
		private Expr mustConvert(Expr expr, CType dest, CONVERTTYPE flags)
		{
			ExprClass exprClass = this.ExprFactory.CreateClass(dest);
			return this.mustConvert(expr, exprClass, flags);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000A9AB File Offset: 0x00008BAB
		private Expr mustConvert(Expr expr, ExprClass dest, CONVERTTYPE flags)
		{
			return this.mustConvertCore(expr, dest, flags);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000A9B8 File Offset: 0x00008BB8
		private Expr mustCastCore(Expr expr, ExprClass destExpr, CONVERTTYPE flags)
		{
			CType type = destExpr.Type;
			this.SemanticChecker.CheckForStaticClass(null, type, ErrorCode.ERR_ConvertToStaticClass);
			Expr expr2;
			if (expr.IsOK)
			{
				if (this.BindExplicitConversion(expr, expr.Type, destExpr, type, out expr2, flags))
				{
					this.checkUnsafe(expr.Type);
					this.checkUnsafe(type);
					return expr2;
				}
				if (type != null && !(type is ErrorType))
				{
					string text = "";
					Expr @const = expr.GetConst();
					FUNDTYPE fundtype = expr.Type.fundType();
					bool flag = @const != null && expr.Type.isSimpleOrEnum() && type.isSimpleOrEnum();
					if (flag && fundtype == FUNDTYPE.FT_STRUCT)
					{
						throw this.ErrorContext.Error(ErrorCode.ERR_ConstOutOfRange, new ErrArg[]
						{
							((ExprConstant)@const).Val.DecimalVal.ToString(CultureInfo.InvariantCulture),
							type
						});
					}
					if (flag && this.Context.CheckedConstant)
					{
						if (this.canExplicitConversionBeBoundInUncheckedContext(expr, expr.Type, destExpr, flags | CONVERTTYPE.NOUDC))
						{
							if (fundtype <= FUNDTYPE.FT_U8)
							{
								if (expr.Type.isUnsigned())
								{
									text = ((ulong)((ExprConstant)@const).Int64Value).ToString(CultureInfo.InvariantCulture);
								}
								else
								{
									text = ((ExprConstant)@const).Int64Value.ToString(CultureInfo.InvariantCulture);
								}
							}
							else if (fundtype <= FUNDTYPE.FT_R8)
							{
								text = ((ExprConstant)@const).Val.DoubleVal.ToString(CultureInfo.InvariantCulture);
							}
							throw this.ErrorContext.Error(ErrorCode.ERR_ConstOutOfRangeChecked, new ErrArg[] { text, type });
						}
						this.CantConvert(expr, type);
					}
					else
					{
						if (expr.Type is NullType && type.fundType() != FUNDTYPE.FT_REF)
						{
							throw this.ErrorContext.Error(ErrorCode.ERR_ValueCantBeNull, new ErrArg[] { type });
						}
						this.CantConvert(expr, type);
					}
				}
			}
			expr2 = this.ExprFactory.CreateCast((EXPRFLAG)0, destExpr, expr);
			expr2.SetError();
			return expr2;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000ABD0 File Offset: 0x00008DD0
		private void CantConvert(Expr expr, CType dest)
		{
			if (expr.Type != null && !(expr.Type is ErrorType))
			{
				throw this.ErrorContext.Error(ErrorCode.ERR_NoExplicitConv, new ErrArg[]
				{
					new ErrArg(expr.Type, ErrArgFlags.Unique),
					new ErrArg(dest, ErrArgFlags.Unique)
				});
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000AC1F File Offset: 0x00008E1F
		public Expr mustCast(Expr expr, CType dest)
		{
			return this.mustCast(expr, dest, (CONVERTTYPE)0);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000AC2C File Offset: 0x00008E2C
		public Expr mustCast(Expr expr, CType dest, CONVERTTYPE flags)
		{
			ExprClass exprClass = this.ExprFactory.CreateClass(dest);
			return this.mustCastCore(expr, exprClass, flags);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000AC4F File Offset: 0x00008E4F
		private Expr mustCastInUncheckedContext(Expr expr, CType dest, CONVERTTYPE flags)
		{
			return new ExpressionBinder(new BindingContext(this.Context)).mustCast(expr, dest, flags);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000AC6C File Offset: 0x00008E6C
		private bool canCast(CType src, CType dest, CONVERTTYPE flags)
		{
			ExprClass exprClass = this.ExprFactory.CreateClass(dest);
			return this.BindExplicitConversion(null, src, exprClass, dest, flags);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000AC91 File Offset: 0x00008E91
		private bool BindImplicitConversion(Expr pSourceExpr, CType pSourceType, ExprClass pDestinationTypeExpr, CType pDestinationTypeForLambdaErrorReporting, CONVERTTYPE flags)
		{
			return new ExpressionBinder.ImplicitConversion(this, pSourceExpr, pSourceType, pDestinationTypeExpr, false, flags).Bind();
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000ACA4 File Offset: 0x00008EA4
		private bool BindImplicitConversion(Expr pSourceExpr, CType pSourceType, ExprClass pDestinationTypeExpr, CType pDestinationTypeForLambdaErrorReporting, out Expr ppDestinationExpr, CONVERTTYPE flags)
		{
			ExpressionBinder.ImplicitConversion implicitConversion = new ExpressionBinder.ImplicitConversion(this, pSourceExpr, pSourceType, pDestinationTypeExpr, true, flags);
			bool flag = implicitConversion.Bind();
			ppDestinationExpr = implicitConversion.ExprDest;
			return flag;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000ACD0 File Offset: 0x00008ED0
		private bool BindImplicitConversion(Expr pSourceExpr, CType pSourceType, ExprClass pDestinationTypeExpr, CType pDestinationTypeForLambdaErrorReporting, bool needsExprDest, out Expr ppDestinationExpr, CONVERTTYPE flags)
		{
			ExpressionBinder.ImplicitConversion implicitConversion = new ExpressionBinder.ImplicitConversion(this, pSourceExpr, pSourceType, pDestinationTypeExpr, needsExprDest, flags);
			bool flag = implicitConversion.Bind();
			ppDestinationExpr = (needsExprDest ? implicitConversion.ExprDest : null);
			return flag;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000AD04 File Offset: 0x00008F04
		private bool BindExplicitConversion(Expr pSourceExpr, CType pSourceType, ExprClass pDestinationTypeExpr, CType pDestinationTypeForLambdaErrorReporting, bool needsExprDest, out Expr ppDestinationExpr, CONVERTTYPE flags)
		{
			ExpressionBinder.ExplicitConversion explicitConversion = new ExpressionBinder.ExplicitConversion(this, pSourceExpr, pSourceType, pDestinationTypeExpr, pDestinationTypeForLambdaErrorReporting, needsExprDest, flags);
			bool flag = explicitConversion.Bind();
			ppDestinationExpr = (needsExprDest ? explicitConversion.ExprDest : null);
			return flag;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000AD38 File Offset: 0x00008F38
		private bool BindExplicitConversion(Expr pSourceExpr, CType pSourceType, ExprClass pDestinationTypeExpr, CType pDestinationTypeForLambdaErrorReporting, out Expr ppDestinationExpr, CONVERTTYPE flags)
		{
			ExpressionBinder.ExplicitConversion explicitConversion = new ExpressionBinder.ExplicitConversion(this, pSourceExpr, pSourceType, pDestinationTypeExpr, pDestinationTypeForLambdaErrorReporting, true, flags);
			bool flag = explicitConversion.Bind();
			ppDestinationExpr = explicitConversion.ExprDest;
			return flag;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000AD63 File Offset: 0x00008F63
		private bool BindExplicitConversion(Expr pSourceExpr, CType pSourceType, ExprClass pDestinationTypeExpr, CType pDestinationTypeForLambdaErrorReporting, CONVERTTYPE flags)
		{
			return new ExpressionBinder.ExplicitConversion(this, pSourceExpr, pSourceType, pDestinationTypeExpr, pDestinationTypeForLambdaErrorReporting, false, flags).Bind();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000AD78 File Offset: 0x00008F78
		private bool bindUserDefinedConversion(Expr exprSrc, CType typeSrc, CType typeDst, bool needExprDest, out Expr pexprDst, bool fImplicitOnly)
		{
			pexprDst = null;
			if (typeSrc == null || typeDst == null || typeSrc.isInterfaceType() || typeDst.isInterfaceType())
			{
				return false;
			}
			CType ctype = typeSrc.StripNubs();
			CType ctype2 = typeDst.StripNubs();
			bool flag = ctype != typeSrc;
			bool flag2 = ctype2 != typeDst;
			bool flag3 = flag2 || typeDst.IsRefType() || typeDst is PointerType;
			AggregateType[] array = new AggregateType[2];
			int num = 0;
			bool flag4 = false;
			AggregateType aggregateType;
			if ((aggregateType = ctype as AggregateType) != null && aggregateType.getAggregate().HasConversion(this.GetSymbolLoader()))
			{
				array[num++] = aggregateType;
				flag4 = aggregateType.isPredefType(PredefinedType.FirstNonSimpleType) || aggregateType.isPredefType(PredefinedType.PT_UINTPTR);
			}
			AggregateType aggregateType2;
			if ((aggregateType2 = ctype2 as AggregateType) != null)
			{
				if (ctype2.getAggregate().HasConversion(this.GetSymbolLoader()))
				{
					array[num++] = aggregateType2;
				}
				if (flag4 && !ctype2.isPredefType(PredefinedType.PT_LONG) && !ctype2.isPredefType(PredefinedType.PT_ULONG))
				{
					flag4 = false;
				}
			}
			else
			{
				flag4 = false;
			}
			if (num == 0)
			{
				return false;
			}
			List<UdConvInfo> list = new List<UdConvInfo>();
			CType ctype3 = null;
			CType ctype4 = null;
			bool flag5 = false;
			bool flag6 = false;
			int num2 = -1;
			int num3 = -1;
			CType ctype5;
			CType ctype6;
			for (int i = 0; i < num; i++)
			{
				AggregateType aggregateType3 = array[i];
				while (aggregateType3 != null && aggregateType3.getAggregate().HasConversion(this.GetSymbolLoader()))
				{
					AggregateSymbol aggregate = aggregateType3.getAggregate();
					PredefinedType predefType = aggregate.GetPredefType();
					bool flag7 = aggregate.IsPredefined() && (predefType == PredefinedType.FirstNonSimpleType || predefType == PredefinedType.PT_UINTPTR || predefType == PredefinedType.PT_DECIMAL);
					for (MethodSymbol methodSymbol = aggregate.GetFirstUDConversion(); methodSymbol != null; methodSymbol = methodSymbol.ConvNext())
					{
						if (methodSymbol.Params.Count == 1 && (!fImplicitOnly || methodSymbol.isImplicit()))
						{
							ctype5 = this.GetTypes().SubstType(methodSymbol.Params[0], aggregateType3);
							ctype6 = this.GetTypes().SubstType(methodSymbol.RetType, aggregateType3);
							bool flag8 = fImplicitOnly;
							if (fImplicitOnly && !flag8 && ctype5.StripNubs() != ctype)
							{
								if (!methodSymbol.isImplicit())
								{
									goto IL_045E;
								}
								flag8 = true;
							}
							FUNDTYPE fundtype;
							FUNDTYPE fundtype2;
							if (((fundtype = ctype6.fundType()) > FUNDTYPE.FT_R8 || fundtype <= FUNDTYPE.FT_NONE || (fundtype2 = ctype5.fundType()) > FUNDTYPE.FT_R8 || fundtype2 <= FUNDTYPE.FT_NONE) && (!flag4 || (!ctype6.isPredefType(PredefinedType.PT_INT) && !ctype6.isPredefType(PredefinedType.PT_UINT))))
							{
								if (flag && (flag3 || !flag8) && ctype5.IsNonNubValType())
								{
									ctype5 = this.GetTypes().GetNullable(ctype5);
								}
								if (flag2 && ctype6.IsNonNubValType())
								{
									ctype6 = this.GetTypes().GetNullable(ctype6);
								}
								bool flag9 = ((exprSrc != null) ? this.canConvert(exprSrc, ctype5, CONVERTTYPE.STANDARDANDNOUDC) : this.canConvert(typeSrc, ctype5, CONVERTTYPE.STANDARDANDNOUDC));
								if (flag9 || (!flag8 && (this.canConvert(ctype5, typeSrc, CONVERTTYPE.STANDARDANDNOUDC) || (flag7 && !(typeSrc is PointerType) && !(ctype5 is PointerType) && this.canCast(typeSrc, ctype5, CONVERTTYPE.NOUDC)))))
								{
									bool flag10 = this.canConvert(ctype6, typeDst, CONVERTTYPE.STANDARDANDNOUDC);
									if ((flag10 || (!flag8 && (this.canConvert(typeDst, ctype6, CONVERTTYPE.STANDARDANDNOUDC) || (flag7 && !(typeDst is PointerType) && !(ctype6 is PointerType) && this.canCast(ctype6, typeDst, CONVERTTYPE.NOUDC))))) && !this.isConvInTable(list, methodSymbol, aggregateType3, flag9, flag10))
									{
										list.Add(new UdConvInfo());
										list[list.Count - 1].mwt = new MethWithType();
										list[list.Count - 1].mwt.Set(methodSymbol, aggregateType3);
										list[list.Count - 1].fSrcImplicit = flag9;
										list[list.Count - 1].fDstImplicit = flag10;
										if (!flag5)
										{
											if (ctype5 == typeSrc)
											{
												ctype3 = ctype5;
												num2 = list.Count - 1;
												flag5 = true;
											}
											else if (ctype3 == null)
											{
												ctype3 = ctype5;
												num2 = list.Count - 1;
											}
											else if (ctype3 != ctype5 && this.CompareSrcTypesBased(ctype3, list[num2].fSrcImplicit, ctype5, flag9) > 0)
											{
												ctype3 = ctype5;
												num2 = list.Count - 1;
											}
										}
										if (!flag6)
										{
											if (ctype6 == typeDst)
											{
												ctype4 = ctype6;
												num3 = list.Count - 1;
												flag6 = true;
											}
											else if (ctype4 == null)
											{
												ctype4 = ctype6;
												num3 = list.Count - 1;
											}
											else if (ctype4 != ctype6 && this.CompareDstTypesBased(ctype4, list[num3].fDstImplicit, ctype6, flag10) > 0)
											{
												ctype4 = ctype6;
												num3 = list.Count - 1;
											}
										}
									}
								}
							}
						}
						IL_045E:;
					}
					aggregateType3 = aggregateType3.GetBaseClass();
				}
			}
			if (ctype3 == null)
			{
				return false;
			}
			int num4 = 3;
			int num5 = -1;
			int num6 = -1;
			for (int j = 0; j < list.Count; j++)
			{
				UdConvInfo udConvInfo = list[j];
				ctype5 = this.GetTypes().SubstType(udConvInfo.mwt.Meth().Params[0], udConvInfo.mwt.GetType());
				ctype6 = this.GetTypes().SubstType(udConvInfo.mwt.Meth().RetType, udConvInfo.mwt.GetType());
				int num7 = 0;
				if (flag && ctype5.IsNonNubValType())
				{
					ctype5 = this.GetTypes().GetNullable(ctype5);
					num7++;
				}
				if (flag2 && ctype6.IsNonNubValType())
				{
					ctype6 = this.GetTypes().GetNullable(ctype6);
					num7++;
				}
				if (ctype5 == ctype3 && ctype6 == ctype4)
				{
					if (num4 > num7)
					{
						num5 = j;
						num6 = -1;
						num4 = num7;
					}
					else if (num4 >= num7 && num6 < 0)
					{
						num6 = j;
						if (num7 == 0)
						{
							break;
						}
					}
				}
				else if (!flag5 && ctype5 != ctype3 && this.CompareSrcTypesBased(ctype3, list[num2].fSrcImplicit, ctype5, udConvInfo.fSrcImplicit) >= 0)
				{
					if (!needExprDest)
					{
						return true;
					}
					num3 = j;
					pexprDst = this.HandleAmbiguity(exprSrc, typeSrc, typeDst, list, num2, num3);
					return true;
				}
				else if (!flag6 && ctype6 != ctype4 && this.CompareDstTypesBased(ctype4, list[num3].fDstImplicit, ctype6, udConvInfo.fDstImplicit) >= 0)
				{
					if (!needExprDest)
					{
						return true;
					}
					num3 = j;
					pexprDst = this.HandleAmbiguity(exprSrc, typeSrc, typeDst, list, num2, num3);
					return true;
				}
			}
			if (!needExprDest)
			{
				return true;
			}
			if (num5 < 0)
			{
				pexprDst = this.HandleAmbiguity(exprSrc, typeSrc, typeDst, list, num2, num3);
				return true;
			}
			if (num6 >= 0)
			{
				num2 = num5;
				num3 = num6;
				pexprDst = this.HandleAmbiguity(exprSrc, typeSrc, typeDst, list, num2, num3);
				return true;
			}
			MethWithInst methWithInst = new MethWithInst(list[num5].mwt.Meth(), list[num5].mwt.GetType(), null);
			ctype5 = this.GetTypes().SubstType(methWithInst.Meth().Params[0], methWithInst.GetType());
			ctype6 = this.GetTypes().SubstType(methWithInst.Meth().RetType, methWithInst.GetType());
			Expr expr = exprSrc;
			Expr expr2;
			if (num4 > 0 && !(ctype5 is NullableType) && flag3)
			{
				ExprMemberGroup exprMemberGroup = this.ExprFactory.CreateMemGroup(null, methWithInst);
				ExprCall exprCall = this.ExprFactory.CreateCall((EXPRFLAG)0, typeDst, exprSrc, exprMemberGroup, methWithInst);
				expr2 = exprCall;
				Expr expr3 = this.mustCast(exprSrc, ctype5);
				this.MarkAsIntermediateConversion(expr3);
				Expr expr4 = this.BindUDConversionCore(expr3, ctype5, ctype6, typeDst, methWithInst);
				exprCall.CastOfNonLiftedResultToLiftedType = this.mustCast(expr4, typeDst);
				exprCall.NullableCallLiftKind = NullableCallLiftKind.UserDefinedConversion;
				if (flag)
				{
					Expr expr5;
					if (ctype5 != ctype)
					{
						NullableType nullable = this.SymbolLoader.GetTypeManager().GetNullable(ctype5);
						expr5 = this.mustCast(exprSrc, nullable);
						this.MarkAsIntermediateConversion(expr5);
					}
					else if (ctype6 is NullableType)
					{
						expr5 = this.mustCast(exprSrc, ctype5);
					}
					else
					{
						expr5 = exprSrc;
					}
					ExprCall exprCall2 = this.ExprFactory.CreateCall((EXPRFLAG)0, typeDst, expr5, exprMemberGroup, methWithInst);
					exprCall2.NullableCallLiftKind = NullableCallLiftKind.NotLiftedIntermediateConversion;
					exprCall.PConversions = exprCall2;
				}
				else
				{
					Expr expr6 = this.BindUDConversionCore(expr3, ctype5, ctype6, typeDst, methWithInst);
					this.MarkAsIntermediateConversion(expr6);
					exprCall.PConversions = expr6;
				}
			}
			else
			{
				expr2 = this.BindUDConversionCore(exprSrc, ctype5, ctype6, typeDst, methWithInst, out expr);
			}
			pexprDst = this.ExprFactory.CreateUserDefinedConversion(expr, expr2, methWithInst);
			return true;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000B5CC File Offset: 0x000097CC
		private Expr HandleAmbiguity(Expr exprSrc, CType typeSrc, CType typeDst, List<UdConvInfo> prguci, int iuciBestSrc, int iuciBestDst)
		{
			throw this.ErrorContext.Error(ErrorCode.ERR_AmbigUDConv, new ErrArg[]
			{
				prguci[iuciBestSrc].mwt,
				prguci[iuciBestDst].mwt,
				typeSrc,
				typeDst
			});
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000B630 File Offset: 0x00009830
		private void MarkAsIntermediateConversion(Expr pExpr)
		{
			ExprCall exprCall;
			for (;;)
			{
				if ((exprCall = pExpr as ExprCall) != null)
				{
					NullableCallLiftKind nullableCallLiftKind = exprCall.NullableCallLiftKind;
					if (nullableCallLiftKind == NullableCallLiftKind.NotLifted)
					{
						goto IL_001D;
					}
					if (nullableCallLiftKind == NullableCallLiftKind.NullableConversion)
					{
						goto IL_0025;
					}
					if (nullableCallLiftKind != NullableCallLiftKind.NullableConversionConstructor)
					{
						break;
					}
					pExpr = exprCall.OptionalArguments;
				}
				else
				{
					ExprUserDefinedConversion exprUserDefinedConversion;
					if ((exprUserDefinedConversion = pExpr as ExprUserDefinedConversion) == null)
					{
						return;
					}
					pExpr = exprUserDefinedConversion.UserDefinedCall;
				}
			}
			return;
			IL_001D:
			exprCall.NullableCallLiftKind = NullableCallLiftKind.NotLiftedIntermediateConversion;
			return;
			IL_0025:
			exprCall.NullableCallLiftKind = NullableCallLiftKind.NullableIntermediateConversion;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000B688 File Offset: 0x00009888
		private Expr BindUDConversionCore(Expr pFrom, CType pTypeFrom, CType pTypeTo, CType pTypeDestination, MethWithInst mwiBest)
		{
			Expr expr;
			return this.BindUDConversionCore(pFrom, pTypeFrom, pTypeTo, pTypeDestination, mwiBest, out expr);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000B6A4 File Offset: 0x000098A4
		private Expr BindUDConversionCore(Expr pFrom, CType pTypeFrom, CType pTypeTo, CType pTypeDestination, MethWithInst mwiBest, out Expr ppTransformedArgument)
		{
			ExprClass exprClass = this.ExprFactory.CreateClass(pTypeFrom);
			Expr expr = this.mustCastCore(pFrom, exprClass, CONVERTTYPE.NOUDC);
			ExprMemberGroup exprMemberGroup = this.ExprFactory.CreateMemGroup(null, mwiBest);
			ExprCall exprCall = this.ExprFactory.CreateCall((EXPRFLAG)0, pTypeTo, expr, exprMemberGroup, mwiBest);
			ExprClass exprClass2 = this.ExprFactory.CreateClass(pTypeDestination);
			Expr expr2 = this.mustCastCore(exprCall, exprClass2, CONVERTTYPE.NOUDC);
			ppTransformedArgument = expr;
			return expr2;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000B708 File Offset: 0x00009908
		private ConstCastResult bindConstantCast(Expr exprSrc, ExprClass exprTypeDest, bool needExprDest, out Expr pexprDest, bool explicitConversion)
		{
			pexprDest = null;
			long num = 0L;
			double num2 = 0.0;
			CType type = exprTypeDest.Type;
			FUNDTYPE fundtype = exprSrc.Type.fundType();
			FUNDTYPE fundtype2 = type.fundType();
			bool flag = fundtype <= FUNDTYPE.FT_U8;
			bool flag2 = fundtype <= FUNDTYPE.FT_R8;
			ExprConstant exprConstant = (ExprConstant)exprSrc.GetConst();
			if (fundtype == FUNDTYPE.FT_STRUCT || fundtype2 == FUNDTYPE.FT_STRUCT)
			{
				Expr expr = this.bindDecimalConstCast(exprTypeDest, exprSrc.Type, exprConstant);
				if (expr != null)
				{
					if (needExprDest)
					{
						pexprDest = expr;
					}
					return ConstCastResult.Success;
				}
				if (explicitConversion)
				{
					return ConstCastResult.CheckFailure;
				}
				return ConstCastResult.Failure;
			}
			else
			{
				if (explicitConversion && this.Context.CheckedConstant && !ExpressionBinder.isConstantInRange(exprConstant, type, true))
				{
					return ConstCastResult.CheckFailure;
				}
				if (!needExprDest)
				{
					return ConstCastResult.Success;
				}
				if (flag)
				{
					if (exprConstant.Type.fundType() == FUNDTYPE.FT_U8)
					{
						if (fundtype2 == FUNDTYPE.FT_U8)
						{
							ConstVal constVal = ConstVal.Get(exprConstant.UInt64Value);
							pexprDest = this.ExprFactory.CreateConstant(type, constVal);
							return ConstCastResult.Success;
						}
						num = (long)(exprConstant.UInt64Value & ulong.MaxValue);
					}
					else
					{
						num = exprConstant.Int64Value;
					}
				}
				else
				{
					if (!flag2)
					{
						return ConstCastResult.Failure;
					}
					num2 = exprConstant.Val.DoubleVal;
				}
				switch (fundtype2)
				{
				case FUNDTYPE.FT_I1:
					if (!flag)
					{
						num = (long)num2;
					}
					num = (long)((sbyte)(num & 255L));
					break;
				case FUNDTYPE.FT_I2:
					if (!flag)
					{
						num = (long)num2;
					}
					num = (long)((short)(num & 65535L));
					break;
				case FUNDTYPE.FT_I4:
					if (!flag)
					{
						num = (long)num2;
					}
					num = (long)((int)(num & (long)((ulong)(-1))));
					break;
				case FUNDTYPE.FT_U1:
					if (!flag)
					{
						num = (long)num2;
					}
					num = (long)((ulong)((byte)(num & 255L)));
					break;
				case FUNDTYPE.FT_U2:
					if (!flag)
					{
						num = (long)num2;
					}
					num = (long)((ulong)((ushort)(num & 65535L)));
					break;
				case FUNDTYPE.FT_U4:
					if (!flag)
					{
						num = (long)num2;
					}
					num = (long)((ulong)((uint)(num & (long)((ulong)(-1)))));
					break;
				case FUNDTYPE.FT_I8:
					if (!flag)
					{
						num = (long)num2;
					}
					break;
				case FUNDTYPE.FT_U8:
					if (!flag)
					{
						num = (long)((ulong)num2);
						if (num2 < 9.223372036854776E+18)
						{
							num = (long)num2;
						}
						else
						{
							num = (long)(num2 - 9.223372036854776E+18) + ExpressionBinder.I64(9223372036854775808UL);
						}
					}
					break;
				case FUNDTYPE.FT_R4:
				case FUNDTYPE.FT_R8:
					if (flag)
					{
						if (fundtype == FUNDTYPE.FT_U8)
						{
							num2 = num;
						}
						else
						{
							num2 = (double)num;
						}
					}
					if (fundtype2 == FUNDTYPE.FT_R4)
					{
						float num3;
						ExpressionBinder.RoundToFloat(num2, out num3);
						num2 = (double)num3;
					}
					break;
				}
				ConstVal constVal2;
				if (fundtype2 == FUNDTYPE.FT_U4)
				{
					constVal2 = ConstVal.Get((uint)num);
				}
				else if (fundtype2 <= FUNDTYPE.FT_U4)
				{
					constVal2 = ConstVal.Get((int)num);
				}
				else if (fundtype2 <= FUNDTYPE.FT_U8)
				{
					constVal2 = ConstVal.Get(num);
				}
				else
				{
					constVal2 = ConstVal.Get(num2);
				}
				ExprConstant exprConstant2 = this.ExprFactory.CreateConstant(type, constVal2);
				pexprDest = exprConstant2;
				return ConstCastResult.Success;
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000B984 File Offset: 0x00009B84
		private int CompareSrcTypesBased(CType type1, bool fImplicit1, CType type2, bool fImplicit2)
		{
			if (fImplicit1 != fImplicit2)
			{
				if (!fImplicit1)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				bool flag = this.canConvert(type1, type2, CONVERTTYPE.NOUDC);
				bool flag2 = this.canConvert(type2, type1, CONVERTTYPE.NOUDC);
				if (flag == flag2)
				{
					return 0;
				}
				if (fImplicit1 != flag)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000B9C0 File Offset: 0x00009BC0
		private int CompareDstTypesBased(CType type1, bool fImplicit1, CType type2, bool fImplicit2)
		{
			if (fImplicit1 != fImplicit2)
			{
				if (!fImplicit1)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				bool flag = this.canConvert(type1, type2, CONVERTTYPE.NOUDC);
				bool flag2 = this.canConvert(type2, type1, CONVERTTYPE.NOUDC);
				if (flag == flag2)
				{
					return 0;
				}
				if (fImplicit1 != flag)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000B9FC File Offset: 0x00009BFC
		private Expr bindDecimalConstCast(ExprClass exprDestType, CType srcType, ExprConstant src)
		{
			CType type = exprDestType.Type;
			CType predefindType = this.SymbolLoader.GetPredefindType(PredefinedType.PT_DECIMAL);
			if (predefindType == null)
			{
				return null;
			}
			if (type == predefindType)
			{
				decimal num;
				switch (srcType.fundType())
				{
				case FUNDTYPE.FT_I1:
				case FUNDTYPE.FT_I2:
				case FUNDTYPE.FT_I4:
					num = Convert.ToDecimal(src.Val.Int32Val);
					break;
				case FUNDTYPE.FT_U1:
				case FUNDTYPE.FT_U2:
				case FUNDTYPE.FT_U4:
					num = Convert.ToDecimal(src.Val.UInt32Val);
					break;
				case FUNDTYPE.FT_I8:
					num = Convert.ToDecimal(src.Val.Int64Val);
					break;
				case FUNDTYPE.FT_U8:
					num = Convert.ToDecimal((ulong)src.Val.Int64Val);
					break;
				case FUNDTYPE.FT_R4:
					num = Convert.ToDecimal((float)src.Val.DoubleVal);
					break;
				case FUNDTYPE.FT_R8:
					num = Convert.ToDecimal(src.Val.DoubleVal);
					break;
				default:
					return null;
				}
				ConstVal constVal = ConstVal.Get(num);
				return this.ExprFactory.CreateConstant(predefindType, constVal);
			}
			if (srcType == predefindType)
			{
				decimal num2 = 0m;
				FUNDTYPE fundtype = type.fundType();
				ConstVal constVal;
				try
				{
					if (fundtype != FUNDTYPE.FT_R4 && fundtype != FUNDTYPE.FT_R8)
					{
						num2 = decimal.Truncate(src.Val.DecimalVal);
					}
					switch (fundtype)
					{
					case FUNDTYPE.FT_I1:
						constVal = ConstVal.Get((int)Convert.ToSByte(num2));
						break;
					case FUNDTYPE.FT_I2:
						constVal = ConstVal.Get((int)Convert.ToInt16(num2));
						break;
					case FUNDTYPE.FT_I4:
						constVal = ConstVal.Get(Convert.ToInt32(num2));
						break;
					case FUNDTYPE.FT_U1:
						constVal = ConstVal.Get((uint)Convert.ToByte(num2));
						break;
					case FUNDTYPE.FT_U2:
						constVal = ConstVal.Get((uint)Convert.ToUInt16(num2));
						break;
					case FUNDTYPE.FT_U4:
						constVal = ConstVal.Get(Convert.ToUInt32(num2));
						break;
					case FUNDTYPE.FT_I8:
						constVal = ConstVal.Get(Convert.ToInt64(num2));
						break;
					case FUNDTYPE.FT_U8:
						constVal = ConstVal.Get(Convert.ToUInt64(num2));
						break;
					case FUNDTYPE.FT_R4:
						constVal = ConstVal.Get(Convert.ToSingle(src.Val.DecimalVal));
						break;
					case FUNDTYPE.FT_R8:
						constVal = ConstVal.Get(Convert.ToDouble(src.Val.DecimalVal));
						break;
					default:
						return null;
					}
				}
				catch (OverflowException)
				{
					return null;
				}
				return this.ExprFactory.CreateConstant(type, constVal);
			}
			return null;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000BC74 File Offset: 0x00009E74
		private bool canExplicitConversionBeBoundInUncheckedContext(Expr exprSrc, CType typeSrc, ExprClass typeDest, CONVERTTYPE flags)
		{
			return new ExpressionBinder(new BindingContext(this.Context)).BindExplicitConversion(exprSrc, typeSrc, typeDest, typeDest.Type, flags);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000BC96 File Offset: 0x00009E96
		public BindingContext GetContext()
		{
			return this.Context;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000BCA0 File Offset: 0x00009EA0
		public ExpressionBinder(BindingContext context)
		{
			this.Context = context;
			this.m_nullable = new CNullable(this.GetSymbolLoader(), this.GetErrorContext(), this.GetExprFactory());
			this.g_binopSignatures = new ExpressionBinder.BinOpSig[]
			{
				new ExpressionBinder.BinOpSig(PredefinedType.PT_INT, PredefinedType.PT_INT, BinOpMask.Integer, 8, new ExpressionBinder.PfnBindBinOp(this.BindIntBinOp), OpSigFlags.Value, BinOpFuncKind.IntBinOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_UINT, PredefinedType.PT_UINT, BinOpMask.Integer, 7, new ExpressionBinder.PfnBindBinOp(this.BindIntBinOp), OpSigFlags.Value, BinOpFuncKind.IntBinOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_LONG, PredefinedType.PT_LONG, BinOpMask.Integer, 6, new ExpressionBinder.PfnBindBinOp(this.BindIntBinOp), OpSigFlags.Value, BinOpFuncKind.IntBinOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_ULONG, PredefinedType.PT_ULONG, BinOpMask.Integer, 5, new ExpressionBinder.PfnBindBinOp(this.BindIntBinOp), OpSigFlags.Value, BinOpFuncKind.IntBinOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_ULONG, PredefinedType.PT_LONG, BinOpMask.Integer, 4, null, OpSigFlags.Value, BinOpFuncKind.None),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_LONG, PredefinedType.PT_ULONG, BinOpMask.Integer, 3, null, OpSigFlags.Value, BinOpFuncKind.None),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_FLOAT, PredefinedType.PT_FLOAT, BinOpMask.Real, 1, new ExpressionBinder.PfnBindBinOp(this.BindRealBinOp), OpSigFlags.Value, BinOpFuncKind.RealBinOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_DOUBLE, PredefinedType.PT_DOUBLE, BinOpMask.Real, 0, new ExpressionBinder.PfnBindBinOp(this.BindRealBinOp), OpSigFlags.Value, BinOpFuncKind.RealBinOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_DECIMAL, PredefinedType.PT_DECIMAL, BinOpMask.Real, 0, new ExpressionBinder.PfnBindBinOp(this.BindDecBinOp), OpSigFlags.Value, BinOpFuncKind.DecBinOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_STRING, PredefinedType.PT_STRING, BinOpMask.Equal, 0, new ExpressionBinder.PfnBindBinOp(this.BindStrCmpOp), OpSigFlags.Convert, BinOpFuncKind.StrCmpOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_STRING, PredefinedType.PT_STRING, BinOpMask.Add, 2, new ExpressionBinder.PfnBindBinOp(this.BindStrBinOp), OpSigFlags.Convert, BinOpFuncKind.StrBinOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_STRING, PredefinedType.PT_OBJECT, BinOpMask.Add, 1, new ExpressionBinder.PfnBindBinOp(this.BindStrBinOp), OpSigFlags.Convert, BinOpFuncKind.StrBinOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_OBJECT, PredefinedType.PT_STRING, BinOpMask.Add, 0, new ExpressionBinder.PfnBindBinOp(this.BindStrBinOp), OpSigFlags.Convert, BinOpFuncKind.StrBinOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_INT, PredefinedType.PT_INT, BinOpMask.Shift, 3, new ExpressionBinder.PfnBindBinOp(this.BindShiftOp), OpSigFlags.Value, BinOpFuncKind.ShiftOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_UINT, PredefinedType.PT_INT, BinOpMask.Shift, 2, new ExpressionBinder.PfnBindBinOp(this.BindShiftOp), OpSigFlags.Value, BinOpFuncKind.ShiftOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_LONG, PredefinedType.PT_INT, BinOpMask.Shift, 1, new ExpressionBinder.PfnBindBinOp(this.BindShiftOp), OpSigFlags.Value, BinOpFuncKind.ShiftOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_ULONG, PredefinedType.PT_INT, BinOpMask.Shift, 0, new ExpressionBinder.PfnBindBinOp(this.BindShiftOp), OpSigFlags.Value, BinOpFuncKind.ShiftOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_BOOL, PredefinedType.PT_BOOL, BinOpMask.BoolNorm, 0, new ExpressionBinder.PfnBindBinOp(this.BindBoolBinOp), OpSigFlags.Value, BinOpFuncKind.BoolBinOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_BOOL, PredefinedType.PT_BOOL, BinOpMask.Logical, 0, new ExpressionBinder.PfnBindBinOp(this.BindBoolBinOp), OpSigFlags.BoolBit, BinOpFuncKind.BoolBinOp),
				new ExpressionBinder.BinOpSig(PredefinedType.PT_BOOL, PredefinedType.PT_BOOL, BinOpMask.Bitwise, 0, new ExpressionBinder.PfnBindBinOp(this.BindLiftedBoolBitwiseOp), OpSigFlags.BoolBit, BinOpFuncKind.BoolBitwiseOp)
			};
			this.g_rguos = new ExpressionBinder.UnaOpSig[]
			{
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_INT, UnaOpMask.Signed, 7, new ExpressionBinder.PfnBindUnaOp(this.BindIntUnaOp), UnaOpFuncKind.IntUnaOp),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_UINT, UnaOpMask.Unsigned, 6, new ExpressionBinder.PfnBindUnaOp(this.BindIntUnaOp), UnaOpFuncKind.IntUnaOp),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_LONG, UnaOpMask.Signed, 5, new ExpressionBinder.PfnBindUnaOp(this.BindIntUnaOp), UnaOpFuncKind.IntUnaOp),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_ULONG, UnaOpMask.Unsigned, 4, new ExpressionBinder.PfnBindUnaOp(this.BindIntUnaOp), UnaOpFuncKind.IntUnaOp),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_ULONG, UnaOpMask.Minus, 3, null, UnaOpFuncKind.None),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_FLOAT, UnaOpMask.Real, 1, new ExpressionBinder.PfnBindUnaOp(this.BindRealUnaOp), UnaOpFuncKind.RealUnaOp),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_DOUBLE, UnaOpMask.Real, 0, new ExpressionBinder.PfnBindUnaOp(this.BindRealUnaOp), UnaOpFuncKind.RealUnaOp),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_DECIMAL, UnaOpMask.Real, 0, new ExpressionBinder.PfnBindUnaOp(this.BindDecUnaOp), UnaOpFuncKind.DecUnaOp),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_BOOL, UnaOpMask.Bang, 0, new ExpressionBinder.PfnBindUnaOp(this.BindBoolUnaOp), UnaOpFuncKind.BoolUnaOp),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_INT, UnaOpMask.IncDec, 6, null, UnaOpFuncKind.None),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_UINT, UnaOpMask.IncDec, 5, null, UnaOpFuncKind.None),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_LONG, UnaOpMask.IncDec, 4, null, UnaOpFuncKind.None),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_ULONG, UnaOpMask.IncDec, 3, null, UnaOpFuncKind.None),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_FLOAT, UnaOpMask.IncDec, 1, null, UnaOpFuncKind.None),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_DOUBLE, UnaOpMask.IncDec, 0, null, UnaOpFuncKind.None),
				new ExpressionBinder.UnaOpSig(PredefinedType.PT_DECIMAL, UnaOpMask.IncDec, 0, null, UnaOpFuncKind.None)
			};
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000C068 File Offset: 0x0000A268
		private SymbolLoader GetSymbolLoader()
		{
			return this.SymbolLoader;
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000C070 File Offset: 0x0000A270
		private SymbolLoader SymbolLoader
		{
			get
			{
				return this.Context.SymbolLoader;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000C07D File Offset: 0x0000A27D
		private CSemanticChecker SemanticChecker
		{
			get
			{
				return this.Context.SemanticChecker;
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000C08A File Offset: 0x0000A28A
		public CSemanticChecker GetSemanticChecker()
		{
			return this.SemanticChecker;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000C092 File Offset: 0x0000A292
		private ErrorHandling ErrorContext
		{
			get
			{
				return this.SymbolLoader.ErrorContext;
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000C09F File Offset: 0x0000A29F
		private ErrorHandling GetErrorContext()
		{
			return this.ErrorContext;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000C0A7 File Offset: 0x0000A2A7
		private BSYMMGR GetGlobalSymbols()
		{
			return this.GetSymbolLoader().getBSymmgr();
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000C0B4 File Offset: 0x0000A2B4
		private TypeManager GetTypes()
		{
			return this.TypeManager;
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000C0BC File Offset: 0x0000A2BC
		private TypeManager TypeManager
		{
			get
			{
				return this.SymbolLoader.TypeManager;
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000C0C9 File Offset: 0x0000A2C9
		private ExprFactory GetExprFactory()
		{
			return this.ExprFactory;
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000C0D1 File Offset: 0x0000A2D1
		private ExprFactory ExprFactory
		{
			get
			{
				return this.Context.ExprFactory;
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000C0DE File Offset: 0x0000A2DE
		private AggregateType GetPredefindType(PredefinedType pt)
		{
			return this.GetSymbolLoader().GetPredefindType(pt);
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000C0EC File Offset: 0x0000A2EC
		private CType VoidType
		{
			get
			{
				return this.GetSymbolLoader().GetTypeManager().GetVoid();
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000C0FE File Offset: 0x0000A2FE
		private CType getVoidType()
		{
			return this.VoidType;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000C106 File Offset: 0x0000A306
		private Expr GenerateAssignmentConversion(Expr op1, Expr op2, bool allowExplicit)
		{
			if (allowExplicit)
			{
				return this.mustCastCore(op2, this.GetExprFactory().CreateClass(op1.Type), (CONVERTTYPE)0);
			}
			return this.mustConvertCore(op2, this.GetExprFactory().CreateClass(op1.Type));
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000C13D File Offset: 0x0000A33D
		public Expr BindAssignment(Expr op1, Expr op2, bool allowExplicit)
		{
			if (!this.checkLvalue(op1, CheckLvalueKind.Assignment))
			{
				ExprAssignment exprAssignment = this.GetExprFactory().CreateAssignment(op1, op2);
				exprAssignment.SetError();
				return exprAssignment;
			}
			op2 = this.GenerateAssignmentConversion(op1, op2, allowExplicit);
			return this.GenerateOptimizedAssignment(op1, op2);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000C170 File Offset: 0x0000A370
		internal Expr BindArrayIndexCore(Expr pOp1, Expr pOp2)
		{
			bool flag = !pOp1.IsOK || !pOp2.IsOK;
			CType pIntType = this.GetPredefindType(PredefinedType.PT_INT);
			CType elementType = (pOp1.Type as ArrayType).GetElementType();
			this.checkUnsafe(elementType);
			CType pDestType = this.ChooseArrayIndexType(pOp2);
			Expr expr = pOp2.Map(this.GetExprFactory(), delegate(Expr x)
			{
				Expr expr3 = this.mustConvert(x, pDestType);
				if (pDestType == pIntType)
				{
					return expr3;
				}
				ExprClass exprClass = this.GetExprFactory().CreateClass(pDestType);
				return this.GetExprFactory().CreateCast(EXPRFLAG.EXF_LITERALCONST, exprClass, expr3);
			});
			Expr expr2 = this.GetExprFactory().CreateArrayIndex(elementType, pOp1, expr);
			if (flag)
			{
				expr2.SetError();
			}
			return expr2;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000C201 File Offset: 0x0000A401
		private void bindSimpleCast(Expr exprSrc, ExprClass typeDest, out Expr pexprDest)
		{
			this.bindSimpleCast(exprSrc, typeDest, out pexprDest, (EXPRFLAG)0);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000C210 File Offset: 0x0000A410
		private void bindSimpleCast(Expr exprSrc, ExprClass exprTypeDest, out Expr pexprDest, EXPRFLAG exprFlags)
		{
			CType type = exprTypeDest.Type;
			pexprDest = null;
			Expr @const = exprSrc.GetConst();
			ExprCast exprCast = this.GetExprFactory().CreateCast(exprFlags, exprTypeDest, exprSrc);
			if (this.Context.CheckedNormal)
			{
				exprCast.Flags |= EXPRFLAG.EXF_CHECKOVERFLOW;
			}
			ExprConstant exprConstant;
			if ((exprConstant = @const as ExprConstant) != null && exprFlags == (EXPRFLAG)0 && exprSrc.Type.fundType() == type.fundType() && (!exprSrc.Type.isPredefType(PredefinedType.PT_STRING) || exprConstant.Val.IsNullRef))
			{
				ExprConstant exprConstant2 = this.GetExprFactory().CreateConstant(type, exprConstant.Val);
				pexprDest = exprConstant2;
				return;
			}
			pexprDest = exprCast;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000C2B8 File Offset: 0x0000A4B8
		private ExprCall BindToMethod(MethWithInst mwi, Expr pArguments, ExprMemberGroup pMemGroup, MemLookFlags flags)
		{
			Expr expr = pMemGroup.OptionalObject;
			CType ctype = ((expr != null) ? expr.Type : null);
			this.PostBindMethod(ref mwi, expr);
			bool flag;
			expr = this.AdjustMemberObject(mwi, expr, out flag);
			pMemGroup.OptionalObject = expr;
			CType ctype2;
			if ((flags & (MemLookFlags.Ctor | MemLookFlags.NewObj)) == (MemLookFlags.Ctor | MemLookFlags.NewObj))
			{
				ctype2 = mwi.Ats;
			}
			else
			{
				ctype2 = this.GetTypes().SubstType(mwi.Meth().RetType, mwi.GetType(), mwi.TypeArgs);
			}
			ExprCall exprCall = this.GetExprFactory().CreateCall((EXPRFLAG)0, ctype2, pArguments, pMemGroup, mwi);
			if (!exprCall.IsOK)
			{
				return exprCall;
			}
			if ((flags & MemLookFlags.Ctor) != MemLookFlags.None && (flags & MemLookFlags.NewObj) != MemLookFlags.None)
			{
				exprCall.Flags |= EXPRFLAG.EXF_LITERALCONST | EXPRFLAG.EXF_CANTBENULL;
			}
			if (flag && expr != null)
			{
				exprCall.Flags |= EXPRFLAG.EXF_UNREALIZEDGOTO;
			}
			this.verifyMethodArgs(exprCall, ctype);
			return exprCall;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000C38C File Offset: 0x0000A58C
		internal Expr BindToField(Expr pOptionalObject, FieldWithType fwt, BindingFlag bindFlags)
		{
			CType ctype = this.GetTypes().SubstType(fwt.Field().GetType(), fwt.GetType());
			if (pOptionalObject != null && !pOptionalObject.IsOK)
			{
				ExprField exprField = this.GetExprFactory().CreateField(ctype, pOptionalObject, fwt, false);
				exprField.SetError();
				return exprField;
			}
			bool flag;
			pOptionalObject = this.AdjustMemberObject(fwt, pOptionalObject, out flag);
			this.checkUnsafe(ctype);
			bool flag2 = ((pOptionalObject != null) ? pOptionalObject.Type : null) is PointerType || this.objectIsLvalue(pOptionalObject);
			if (fwt.Field().isReadOnly)
			{
				flag2 = false;
			}
			AggregateType aggregateType = null;
			if (fwt.Field().isEvent && fwt.Field().getEvent(this.GetSymbolLoader()) != null && fwt.Field().getEvent(this.GetSymbolLoader()).IsWindowsRuntimeEvent)
			{
				aggregateType = fwt.Field().GetType() as AggregateType;
				if (aggregateType != null)
				{
					ctype = this.GetTypes().GetParameterModifier(ctype, false);
				}
			}
			ExprField exprField2 = this.GetExprFactory().CreateField(ctype, pOptionalObject, fwt, flag2);
			if (ctype is ErrorType)
			{
				exprField2.SetError();
			}
			exprField2.Flags |= (EXPRFLAG)(bindFlags & BindingFlag.BIND_MEMBERSET);
			if (aggregateType != null)
			{
				Name predefinedName = NameManager.GetPredefinedName(PredefinedName.PN_GETORCREATEEVENTREGISTRATIONTOKENTABLE);
				this.GetSymbolLoader().RuntimeBinderSymbolTable.PopulateSymbolTableWithName(predefinedName.Text, null, aggregateType.AssociatedSystemType);
				MethPropWithInst methPropWithInst = new MethPropWithInst(this.GetSymbolLoader().LookupAggMember(predefinedName, aggregateType.getAggregate(), symbmask_t.MASK_MethodSymbol) as MethodSymbol, aggregateType);
				ExprMemberGroup exprMemberGroup = this.GetExprFactory().CreateMemGroup(null, methPropWithInst);
				Expr expr = this.BindToMethod(new MethWithInst(methPropWithInst), exprField2, exprMemberGroup, MemLookFlags.None);
				AggregateSymbol owningAggregate = aggregateType.GetOwningAggregate();
				Name predefinedName2 = NameManager.GetPredefinedName(PredefinedName.PN_INVOCATIONLIST);
				this.GetSymbolLoader().RuntimeBinderSymbolTable.PopulateSymbolTableWithName(predefinedName2.Text, null, aggregateType.AssociatedSystemType);
				PropertySymbol propertySymbol = this.GetSymbolLoader().LookupAggMember(predefinedName2, owningAggregate, symbmask_t.MASK_PropertySymbol) as PropertySymbol;
				MethPropWithInst methPropWithInst2 = new MethPropWithInst(propertySymbol, aggregateType);
				ExprMemberGroup exprMemberGroup2 = this.GetExprFactory().CreateMemGroup(expr, methPropWithInst2);
				PropWithType propWithType = new PropWithType(propertySymbol, aggregateType);
				return this.BindToProperty(expr, propWithType, bindFlags, null, null, exprMemberGroup2);
			}
			return exprField2;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000C598 File Offset: 0x0000A798
		internal Expr BindToProperty(Expr pObject, PropWithType pwt, BindingFlag bindFlags, Expr args, AggregateType pOtherType, ExprMemberGroup pMemGroup)
		{
			Expr expr = pObject;
			MethWithType methWithType;
			MethWithType methWithType2;
			this.PostBindProperty(pwt, pObject, out methWithType, out methWithType2);
			bool flag;
			if (methWithType && (!methWithType2 || methWithType2.GetType() == methWithType.GetType() || this.GetSymbolLoader().HasBaseConversion(methWithType.GetType(), methWithType2.GetType())))
			{
				pObject = this.AdjustMemberObject(methWithType, pObject, out flag);
			}
			else if (methWithType2)
			{
				pObject = this.AdjustMemberObject(methWithType2, pObject, out flag);
			}
			else
			{
				pObject = this.AdjustMemberObject(pwt, pObject, out flag);
			}
			pMemGroup.OptionalObject = pObject;
			CType ctype = this.GetTypes().SubstType(pwt.Prop().RetType, pwt.GetType());
			if (pObject != null && !pObject.IsOK)
			{
				ExprProperty exprProperty = this.GetExprFactory().CreateProperty(ctype, expr, args, pMemGroup, pwt, null);
				exprProperty.SetError();
				return exprProperty;
			}
			if ((bindFlags & BindingFlag.BIND_RVALUEREQUIRED) != (BindingFlag)0)
			{
				if (!methWithType)
				{
					if (pOtherType != null)
					{
						return this.GetExprFactory().CreateClass(pOtherType);
					}
					throw this.ErrorContext.Error(ErrorCode.ERR_PropertyLacksGet, new ErrArg[] { pwt });
				}
				else
				{
					CType ctype2 = null;
					if (expr != null)
					{
						ctype2 = expr.Type;
					}
					ACCESSERROR accesserror = this.SemanticChecker.CheckAccess2(methWithType.Meth(), methWithType.GetType(), this.ContextForMemberLookup(), ctype2);
					if (accesserror != ACCESSERROR.ACCESSERROR_NOERROR)
					{
						if (pOtherType != null)
						{
							return this.GetExprFactory().CreateClass(pOtherType);
						}
						if (accesserror == ACCESSERROR.ACCESSERROR_NOACCESSTHRU)
						{
							throw this.ErrorContext.Error(ErrorCode.ERR_BadProtectedAccess, new ErrArg[]
							{
								pwt,
								ctype2,
								this.ContextForMemberLookup()
							});
						}
						throw this.ErrorContext.Error(ErrorCode.ERR_InaccessibleGetter, new ErrArg[] { pwt });
					}
				}
			}
			ExprProperty exprProperty2 = this.GetExprFactory().CreateProperty(ctype, expr, args, pMemGroup, pwt, methWithType2);
			if (flag && pObject != null)
			{
				exprProperty2.Flags |= EXPRFLAG.EXF_UNREALIZEDGOTO;
			}
			if (exprProperty2.OptionalArguments != null)
			{
				this.verifyMethodArgs(exprProperty2, (expr != null) ? expr.Type : null);
			}
			if (methWithType2 && this.objectIsLvalue(exprProperty2.MemberGroup.OptionalObject))
			{
				exprProperty2.Flags |= EXPRFLAG.EXF_LVALUE;
			}
			if (pOtherType != null)
			{
				exprProperty2.Flags |= EXPRFLAG.EXF_SAMENAMETYPE;
			}
			return exprProperty2;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000C7E0 File Offset: 0x0000A9E0
		internal Expr bindUDUnop(ExpressionKind ek, Expr arg)
		{
			Name name = this.ekName(ek);
			CType ctype = arg.Type;
			for (;;)
			{
				TypeKind typeKind = ctype.GetTypeKind();
				if (typeKind == TypeKind.TK_AggregateType)
				{
					break;
				}
				if (typeKind != TypeKind.TK_NullableType)
				{
					goto IL_004E;
				}
				ctype = ctype.StripNubs();
			}
			if ((!ctype.isClassType() && !ctype.isStructType()) || ((AggregateType)ctype).getAggregate().IsSkipUDOps())
			{
				return null;
			}
			ArgInfos argInfos = new ArgInfos();
			argInfos.carg = 1;
			this.FillInArgInfoFromArgList(argInfos, arg);
			List<CandidateFunctionMember> list = new List<CandidateFunctionMember>();
			MethodSymbol methodSymbol = null;
			AggregateType aggregateType = (AggregateType)ctype;
			for (;;)
			{
				methodSymbol = ((methodSymbol == null) ? (this.GetSymbolLoader().LookupAggMember(name, aggregateType.getAggregate(), symbmask_t.MASK_MethodSymbol) as MethodSymbol) : (this.GetSymbolLoader().LookupNextSym(methodSymbol, aggregateType.getAggregate(), symbmask_t.MASK_MethodSymbol) as MethodSymbol));
				if (methodSymbol == null)
				{
					if (!list.IsEmpty<CandidateFunctionMember>())
					{
						break;
					}
					aggregateType = aggregateType.GetBaseClass();
					if (aggregateType == null)
					{
						break;
					}
				}
				else if (methodSymbol.isOperator && methodSymbol.Params.Count == 1)
				{
					TypeArray typeArray = this.GetTypes().SubstTypeArray(methodSymbol.Params, aggregateType);
					CType ctype2 = typeArray[0];
					NullableType nullable;
					if (this.canConvert(arg, ctype2))
					{
						list.Add(new CandidateFunctionMember(new MethPropWithInst(methodSymbol, aggregateType, BSYMMGR.EmptyTypeArray()), typeArray, 0, false));
					}
					else if (ctype2.IsNonNubValType() && this.GetTypes().SubstType(methodSymbol.RetType, aggregateType).IsNonNubValType() && this.canConvert(arg, nullable = this.GetTypes().GetNullable(ctype2)))
					{
						list.Add(new CandidateFunctionMember(new MethPropWithInst(methodSymbol, aggregateType, BSYMMGR.EmptyTypeArray()), this.GetGlobalSymbols().AllocParams(1, new CType[] { nullable }), 1, false));
					}
				}
			}
			if (list.IsEmpty<CandidateFunctionMember>())
			{
				return null;
			}
			CandidateFunctionMember candidateFunctionMember2;
			CandidateFunctionMember candidateFunctionMember3;
			CandidateFunctionMember candidateFunctionMember = this.FindBestMethod(list, null, argInfos, out candidateFunctionMember2, out candidateFunctionMember3);
			if (candidateFunctionMember == null)
			{
				throw this.ErrorContext.Error(ErrorCode.ERR_AmbigCall, new ErrArg[] { candidateFunctionMember2.mpwi, candidateFunctionMember3.mpwi });
			}
			ExprCall exprCall;
			if (candidateFunctionMember.ctypeLift != 0)
			{
				exprCall = this.BindLiftedUDUnop(arg, candidateFunctionMember.@params[0], candidateFunctionMember.mpwi);
			}
			else
			{
				exprCall = this.BindUDUnopCall(arg, candidateFunctionMember.@params[0], candidateFunctionMember.mpwi);
			}
			return this.GetExprFactory().CreateUserDefinedUnaryOperator(ek, exprCall.Type, arg, exprCall, candidateFunctionMember.mpwi);
			IL_004E:
			return null;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000CA5C File Offset: 0x0000AC5C
		private ExprCall BindLiftedUDUnop(Expr arg, CType typeArg, MethPropWithInst mpwi)
		{
			CType ctype = typeArg.StripNubs();
			if (!(arg.Type is NullableType) || !this.canConvert(arg.Type.StripNubs(), ctype, CONVERTTYPE.NOUDC))
			{
				arg = this.mustConvert(arg, typeArg);
			}
			CType ctype2 = this.GetTypes().SubstType(mpwi.Meth().RetType, mpwi.GetType());
			if (!(ctype2 is NullableType))
			{
				ctype2 = this.GetTypes().GetNullable(ctype2);
			}
			Expr expr = this.mustCast(arg, ctype);
			ExprCall exprCall = this.BindUDUnopCall(expr, ctype, mpwi);
			ExprMemberGroup exprMemberGroup = this.GetExprFactory().CreateMemGroup(null, mpwi);
			ExprCall exprCall2 = this.GetExprFactory().CreateCall((EXPRFLAG)0, ctype2, arg, exprMemberGroup, null);
			exprCall2.MethWithInst = new MethWithInst(mpwi);
			exprCall2.CastOfNonLiftedResultToLiftedType = this.mustCast(exprCall, ctype2, (CONVERTTYPE)0);
			exprCall2.NullableCallLiftKind = NullableCallLiftKind.Operator;
			return exprCall2;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000CB24 File Offset: 0x0000AD24
		private ExprCall BindUDUnopCall(Expr arg, CType typeArg, MethPropWithInst mpwi)
		{
			CType ctype = this.GetTypes().SubstType(mpwi.Meth().RetType, mpwi.GetType());
			this.checkUnsafe(ctype);
			ExprMemberGroup exprMemberGroup = this.GetExprFactory().CreateMemGroup(null, mpwi);
			ExprCall exprCall = this.GetExprFactory().CreateCall((EXPRFLAG)0, ctype, this.mustConvert(arg, typeArg), exprMemberGroup, null);
			exprCall.MethWithInst = new MethWithInst(mpwi);
			this.verifyMethodArgs(exprCall, mpwi.GetType());
			return exprCall;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000CB98 File Offset: 0x0000AD98
		private bool BindMethodGroupToArgumentsCore(out ExpressionBinder.GroupToArgsBinderResult pResults, BindingFlag bindFlags, ExprMemberGroup grp, ref Expr args, int carg, bool bHasNamedArgumentSpecifiers)
		{
			ArgInfos argInfos = new ArgInfos
			{
				carg = carg
			};
			this.FillInArgInfoFromArgList(argInfos, args);
			ArgInfos argInfos2 = new ArgInfos
			{
				carg = carg
			};
			this.FillInArgInfoFromArgList(argInfos2, args);
			ExpressionBinder.GroupToArgsBinder groupToArgsBinder = new ExpressionBinder.GroupToArgsBinder(this, bindFlags, grp, argInfos, argInfos2, bHasNamedArgumentSpecifiers, null);
			bool flag = groupToArgsBinder.Bind(true);
			pResults = groupToArgsBinder.GetResultsOfBind();
			return flag;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000CBF4 File Offset: 0x0000ADF4
		internal Expr BindMethodGroupToArguments(BindingFlag bindFlags, ExprMemberGroup grp, Expr args)
		{
			bool flag;
			int num = ExpressionBinder.CountArguments(args, out flag);
			Expr optionalObject = grp.OptionalObject;
			if (grp.Name == null)
			{
				ExprCall exprCall = this.GetExprFactory().CreateCall((EXPRFLAG)0, this.GetTypes().GetErrorSym(), args, grp, null);
				exprCall.SetError();
				return exprCall;
			}
			bool flag2 = this.VerifyNamedArgumentsAfterFixed(args);
			ExpressionBinder.GroupToArgsBinderResult groupToArgsBinderResult;
			if (!this.BindMethodGroupToArgumentsCore(out groupToArgsBinderResult, bindFlags, grp, ref args, num, flag2))
			{
				return null;
			}
			MethPropWithInst bestResult = groupToArgsBinderResult.GetBestResult();
			Expr expr;
			if (grp.SymKind == SYMKIND.SK_PropertySymbol)
			{
				expr = this.BindToProperty(grp.OptionalObject, new PropWithType(bestResult), bindFlags, args, null, grp);
			}
			else
			{
				expr = this.BindToMethod(new MethWithInst(bestResult), args, grp, (MemLookFlags)grp.Flags);
			}
			return expr;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000CC9C File Offset: 0x0000AE9C
		private bool VerifyNamedArgumentsAfterFixed(Expr args)
		{
			Expr expr = args;
			bool flag = false;
			while (expr != null)
			{
				ExprList exprList;
				Expr expr2;
				if ((exprList = expr as ExprList) != null)
				{
					expr2 = exprList.OptionalElement;
					expr = exprList.OptionalNextListNode;
				}
				else
				{
					expr2 = expr;
					expr = null;
				}
				if (expr2 is ExprNamedArgumentSpecification)
				{
					flag = true;
				}
				else if (flag)
				{
					throw this.GetErrorContext().Error(ErrorCode.ERR_NamedArgumentSpecificationBeforeFixedArgument, Array.Empty<ErrArg>());
				}
			}
			return flag;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000CCF6 File Offset: 0x0000AEF6
		private ExprOperator BadOperatorTypesError(ExpressionKind ek, Expr pOperand1, Expr pOperand2)
		{
			return this.BadOperatorTypesError(ek, pOperand1, pOperand2, null);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000CD04 File Offset: 0x0000AF04
		private ExprOperator BadOperatorTypesError(ExpressionKind ek, Expr pOperand1, Expr pOperand2, CType pTypeErr)
		{
			string errorString = pOperand1.ErrorString;
			pOperand1 = this.UnwrapExpression(pOperand1);
			if (pOperand1 != null)
			{
				if (pOperand2 != null)
				{
					pOperand2 = this.UnwrapExpression(pOperand2);
					if (pOperand1.Type != null && !(pOperand1.Type is ErrorType) && pOperand2.Type != null && !(pOperand2.Type is ErrorType))
					{
						throw this.ErrorContext.Error(ErrorCode.ERR_BadBinaryOps, new ErrArg[] { errorString, pOperand1.Type, pOperand2.Type });
					}
				}
				else if (pOperand1.Type != null && !(pOperand1.Type is ErrorType))
				{
					throw this.ErrorContext.Error(ErrorCode.ERR_BadUnaryOp, new ErrArg[] { errorString, pOperand1.Type });
				}
			}
			if (pTypeErr == null)
			{
				pTypeErr = this.GetPredefindType(PredefinedType.PT_OBJECT);
			}
			ExprOperator exprOperator = this.GetExprFactory().CreateOperator(ek, pTypeErr, pOperand1, pOperand2);
			exprOperator.SetError();
			return exprOperator;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000CE04 File Offset: 0x0000B004
		private Expr UnwrapExpression(Expr pExpression)
		{
			ExprWrap exprWrap;
			while ((exprWrap = pExpression as ExprWrap) != null)
			{
				Expr optionalExpression = exprWrap.OptionalExpression;
				if (optionalExpression == null)
				{
					break;
				}
				pExpression = optionalExpression;
			}
			return pExpression;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000CE2B File Offset: 0x0000B02B
		private static ErrorCode GetStandardLvalueError(CheckLvalueKind kind)
		{
			if (kind == CheckLvalueKind.OutParameter)
			{
				return ErrorCode.ERR_RefLvalueExpected;
			}
			if (kind != CheckLvalueKind.Increment)
			{
				return ErrorCode.ERR_AssgLvalueExpected;
			}
			return ErrorCode.ERR_IncrementLvalueExpected;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000CE48 File Offset: 0x0000B048
		private void CheckLvalueProp(ExprProperty prop)
		{
			CType ctype = null;
			if (prop.OptionalObjectThrough != null)
			{
				ctype = prop.OptionalObjectThrough.Type;
			}
			this.CheckPropertyAccess(prop.MethWithTypeSet, prop.PropWithTypeSlot, ctype);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000CE80 File Offset: 0x0000B080
		private void CheckPropertyAccess(MethWithType mwt, PropWithType pwtSlot, CType type)
		{
			ACCESSERROR accesserror = this.SemanticChecker.CheckAccess2(mwt.Meth(), mwt.GetType(), this.ContextForMemberLookup(), type);
			if (accesserror == ACCESSERROR.ACCESSERROR_NOACCESS)
			{
				throw this.ErrorContext.Error(mwt.Meth().isSetAccessor() ? ErrorCode.ERR_InaccessibleSetter : ErrorCode.ERR_InaccessibleGetter, new ErrArg[] { pwtSlot });
			}
			if (accesserror == ACCESSERROR.ACCESSERROR_NOACCESSTHRU)
			{
				throw this.ErrorContext.Error(ErrorCode.ERR_BadProtectedAccess, new ErrArg[]
				{
					pwtSlot,
					type,
					this.ContextForMemberLookup()
				});
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000CF20 File Offset: 0x0000B120
		private bool checkLvalue(Expr expr, CheckLvalueKind kind)
		{
			if (!expr.IsOK)
			{
				return false;
			}
			if (expr.isLvalue())
			{
				ExprProperty exprProperty;
				if ((exprProperty = expr as ExprProperty) != null)
				{
					this.CheckLvalueProp(exprProperty);
				}
				this.markFieldAssigned(expr);
				return true;
			}
			ExpressionKind kind2 = expr.Kind;
			if (kind2 <= ExpressionKind.Property)
			{
				if (kind2 != ExpressionKind.Constant)
				{
					if (kind2 != ExpressionKind.Property)
					{
						goto IL_0101;
					}
					if (kind == CheckLvalueKind.OutParameter)
					{
						throw this.ErrorContext.Error(ErrorCode.ERR_RefProperty, Array.Empty<ErrArg>());
					}
					ExprProperty exprProperty2 = (ExprProperty)expr;
					if (!exprProperty2.MethWithTypeSet)
					{
						throw this.ErrorContext.Error(ErrorCode.ERR_AssgReadonlyProp, new ErrArg[] { exprProperty2.PropWithTypeSlot });
					}
					goto IL_0101;
				}
			}
			else
			{
				if (kind2 == ExpressionKind.MemberGroup)
				{
					ErrorCode errorCode = ((kind == CheckLvalueKind.OutParameter) ? ErrorCode.ERR_RefReadonlyLocalCause : ErrorCode.ERR_AssgReadonlyLocalCause);
					throw this.ErrorContext.Error(errorCode, new ErrArg[]
					{
						((ExprMemberGroup)expr).Name,
						new ErrArgIds(MessageID.MethodGroup)
					});
				}
				if (kind2 != ExpressionKind.BoundLambda)
				{
					goto IL_0101;
				}
			}
			throw this.ErrorContext.Error(ExpressionBinder.GetStandardLvalueError(kind), Array.Empty<ErrArg>());
			IL_0101:
			this.TryReportLvalueFailure(expr, kind);
			return true;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000D038 File Offset: 0x0000B238
		private void PostBindMethod(ref MethWithInst pMWI, Expr pObject)
		{
			if (pObject != null && pObject.Type.isSimpleType())
			{
				ExpressionBinder.RemapToOverride(this.GetSymbolLoader(), pMWI, pObject.Type);
			}
			if (pMWI.Meth().RetType != null)
			{
				this.checkUnsafe(pMWI.Meth().RetType);
				TypeArray @params = pMWI.Meth().Params;
				for (int i = 0; i < @params.Count; i++)
				{
					CType ctype = @params[i];
					if (ctype.isUnsafe())
					{
						this.checkUnsafe(ctype);
					}
				}
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000D0C0 File Offset: 0x0000B2C0
		private void PostBindProperty(PropWithType pwt, Expr pObject, out MethWithType pmwtGet, out MethWithType pmwtSet)
		{
			pmwtGet = new MethWithType();
			pmwtSet = new MethWithType();
			if (pwt.Prop().GetterMethod != null)
			{
				pmwtGet.Set(pwt.Prop().GetterMethod, pwt.GetType());
			}
			else
			{
				pmwtGet.Clear();
			}
			if (pwt.Prop().SetterMethod != null)
			{
				pmwtSet.Set(pwt.Prop().SetterMethod, pwt.GetType());
			}
			else
			{
				pmwtSet.Clear();
			}
			if (pwt.Prop().RetType != null)
			{
				this.checkUnsafe(pwt.Prop().RetType);
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000D158 File Offset: 0x0000B358
		private Expr AdjustMemberObject(SymWithType swt, Expr pObject, out bool pfConstrained)
		{
			bool flag = this.IsMatchingStatic(swt, pObject);
			pfConstrained = false;
			bool isStatic = swt.Sym.isStatic;
			if (!flag)
			{
				if (!isStatic)
				{
					throw this.ErrorContext.Error(ErrorCode.ERR_ObjectRequired, new ErrArg[] { swt });
				}
				if ((pObject.Flags & EXPRFLAG.EXF_UNREALIZEDGOTO) != (EXPRFLAG)0)
				{
					return null;
				}
				throw this.ErrorContext.Error(ErrorCode.ERR_ObjectProhibited, new ErrArg[] { swt });
			}
			else
			{
				if (isStatic)
				{
					return null;
				}
				if (swt.Sym is MethodSymbol && swt.Meth().IsConstructor())
				{
					return pObject;
				}
				if (pObject == null)
				{
					return null;
				}
				CType ctype = pObject.Type;
				NullableType nullableType;
				CType ats;
				if ((nullableType = ctype as NullableType) != null && (ats = nullableType.GetAts()) != swt.GetType())
				{
					ctype = ats;
				}
				if (ctype is TypeParameterType || ctype is AggregateType)
				{
					ParentSymbol parent = swt.Sym.parent;
					ExprField exprField;
					if ((exprField = pObject as ExprField) != null && !exprField.FieldWithType.Field().isAssigned && !(swt.Sym is FieldSymbol) && ctype.isStructType() && !ctype.isPredefined())
					{
						exprField.FieldWithType.Field().isAssigned = true;
					}
					if (pfConstrained && (ctype is TypeParameterType || (ctype.isStructType() && swt.GetType().IsRefType() && swt.Sym.IsVirtual())))
					{
						pfConstrained = true;
					}
					Expr expr = this.tryConvert(pObject, swt.GetType(), CONVERTTYPE.NOUDC);
					if (expr == null)
					{
						throw this.ErrorContext.Error(ErrorCode.ERR_WrongNestedThis, new ErrArg[]
						{
							swt.GetType(),
							pObject.Type
						});
					}
					pObject = expr;
				}
				return pObject;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000D2FC File Offset: 0x0000B4FC
		private bool IsMatchingStatic(SymWithType swt, Expr pObject)
		{
			MethodSymbol methodSymbol;
			if ((methodSymbol = swt.Sym as MethodSymbol) != null && methodSymbol.IsConstructor())
			{
				return !methodSymbol.isStatic;
			}
			if (swt.Sym.isStatic)
			{
				if (pObject == null)
				{
					return true;
				}
				if ((pObject.Flags & EXPRFLAG.EXF_SAMENAMETYPE) == (EXPRFLAG)0)
				{
					return false;
				}
			}
			else if (pObject == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000D352 File Offset: 0x0000B552
		private bool objectIsLvalue(Expr pObject)
		{
			return pObject == null || ((pObject.Flags & EXPRFLAG.EXF_LVALUE) != (EXPRFLAG)0 && pObject.Kind != ExpressionKind.Property) || !pObject.Type.isStructOrEnum();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000D380 File Offset: 0x0000B580
		private static void RemapToOverride(SymbolLoader symbolLoader, SymWithType pswt, CType typeObj)
		{
			NullableType nullableType;
			if ((nullableType = typeObj as NullableType) != null)
			{
				typeObj = nullableType.GetAts();
			}
			AggregateType aggregateType;
			if ((aggregateType = typeObj as AggregateType) == null || aggregateType.isInterfaceType() || !pswt.Sym.IsVirtual())
			{
				return;
			}
			symbmask_t symbmask_t = pswt.Sym.mask();
			while (aggregateType != null && aggregateType.getAggregate() != pswt.Sym.parent)
			{
				for (Symbol symbol = symbolLoader.LookupAggMember(pswt.Sym.name, aggregateType.getAggregate(), symbmask_t); symbol != null; symbol = symbolLoader.LookupNextSym(symbol, aggregateType.getAggregate(), symbmask_t))
				{
					if (symbol.IsOverride() && (symbol.SymBaseVirtual() == pswt.Sym || symbol.SymBaseVirtual() == pswt.Sym.SymBaseVirtual()))
					{
						pswt.Set(symbol, aggregateType);
						return;
					}
				}
				aggregateType = aggregateType.GetBaseClass();
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000D44C File Offset: 0x0000B64C
		private void verifyMethodArgs(IExprWithArgs call, CType callingObjectType)
		{
			Expr optionalArguments = call.OptionalArguments;
			SymWithType symWithType = call.GetSymWithType();
			MethodOrPropertySymbol methodOrPropertySymbol = symWithType.Sym as MethodOrPropertySymbol;
			ExprCall exprCall = call as ExprCall;
			TypeArray typeArray = ((exprCall != null) ? exprCall.MethWithInst.TypeArgs : null);
			Expr expr;
			this.AdjustCallArgumentsForParams(callingObjectType, symWithType.GetType(), methodOrPropertySymbol, typeArray, optionalArguments, out expr);
			call.OptionalArguments = expr;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000D4A8 File Offset: 0x0000B6A8
		private void AdjustCallArgumentsForParams(CType callingObjectType, CType type, MethodOrPropertySymbol mp, TypeArray pTypeArgs, Expr argsPtr, out Expr newArgs)
		{
			newArgs = null;
			Expr expr = null;
			MethodOrPropertySymbol methodOrPropertySymbol = ExpressionBinder.GroupToArgsBinder.FindMostDerivedMethod(this.GetSymbolLoader(), mp, callingObjectType);
			int num = mp.Params.Count;
			TypeArray @params = mp.Params;
			int num2 = 0;
			MethodSymbol methodSymbol = mp as MethodSymbol;
			int num3 = ExpressionIterator.Count(argsPtr);
			if (methodSymbol != null && methodSymbol.isVarargs)
			{
				num--;
			}
			bool flag = false;
			ExpressionIterator expressionIterator = new ExpressionIterator(argsPtr);
			if (argsPtr != null)
			{
				while (!expressionIterator.AtEnd())
				{
					Expr expr2 = expressionIterator.Current();
					if (expr2.Type is ParameterModifierType)
					{
						if (num != 0)
						{
							num--;
						}
						this.GetExprFactory().AppendItemToList(expr2, ref newArgs, ref expr);
					}
					else if (num != 0)
					{
						if (num == 1 && mp.isParamArray && num3 > mp.Params.Count)
						{
							goto IL_0277;
						}
						Expr expr3 = expr2;
						ExprNamedArgumentSpecification exprNamedArgumentSpecification;
						Expr expr4;
						if ((exprNamedArgumentSpecification = expr3 as ExprNamedArgumentSpecification) != null)
						{
							int num4 = 0;
							using (List<Name>.Enumerator enumerator = methodOrPropertySymbol.ParameterNames.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									if (enumerator.Current == exprNamedArgumentSpecification.Name)
									{
										break;
									}
									num4++;
								}
							}
							CType ctype = this.GetTypes().SubstType(@params[num4], type, pTypeArgs);
							if (!this.canConvert(exprNamedArgumentSpecification.Value, ctype) && mp.isParamArray && num4 == mp.Params.Count - 1)
							{
								CType ctype2 = (ArrayType)this.GetTypes().SubstType(mp.Params[mp.Params.Count - 1], type, pTypeArgs);
								ExprArrayInit exprArrayInit = this.GetExprFactory().CreateArrayInit(ctype2, null, null, new int[1], 1);
								exprArrayInit.GeneratedForParamArray = true;
								exprArrayInit.OptionalArguments = exprNamedArgumentSpecification.Value;
								exprNamedArgumentSpecification.Value = exprArrayInit;
								flag = true;
							}
							else
							{
								exprNamedArgumentSpecification.Value = this.tryConvert(exprNamedArgumentSpecification.Value, ctype);
							}
							expr4 = expr3;
						}
						else
						{
							CType ctype3 = this.GetTypes().SubstType(@params[num2], type, pTypeArgs);
							expr4 = this.tryConvert(expr2, ctype3);
						}
						if (expr4 == null)
						{
							if (!mp.isParamArray || num != 1 || num3 < mp.Params.Count)
							{
								return;
							}
							goto IL_0277;
						}
						else
						{
							this.GetExprFactory().AppendItemToList(expr4, ref newArgs, ref expr);
							num--;
						}
					}
					num2++;
					if (num != 0 && mp.isParamArray && num2 == num3)
					{
						expressionIterator.MoveNext();
						goto IL_0277;
					}
					expressionIterator.MoveNext();
				}
				return;
			}
			if (!mp.isParamArray)
			{
				return;
			}
			IL_0277:
			if (flag)
			{
				return;
			}
			CType ctype4 = this.GetTypes().SubstType(mp.Params[mp.Params.Count - 1], type, pTypeArgs);
			ArrayType arrayType;
			if ((arrayType = ctype4 as ArrayType) == null || !arrayType.IsSZArray)
			{
				return;
			}
			CType elementType = arrayType.GetElementType();
			ExprArrayInit exprArrayInit2 = this.GetExprFactory().CreateArrayInit(ctype4, null, null, new int[1], 1);
			exprArrayInit2.GeneratedForParamArray = true;
			if (expressionIterator.AtEnd())
			{
				exprArrayInit2.DimensionSize = 0;
				exprArrayInit2.DimensionSizes[0] = 0;
				exprArrayInit2.OptionalArguments = null;
				if (argsPtr == null)
				{
					argsPtr = exprArrayInit2;
				}
				else
				{
					argsPtr = this.GetExprFactory().CreateList(argsPtr, exprArrayInit2);
				}
				this.GetExprFactory().AppendItemToList(exprArrayInit2, ref newArgs, ref expr);
				return;
			}
			Expr expr5 = null;
			Expr expr6 = null;
			int num5 = 0;
			while (!expressionIterator.AtEnd())
			{
				Expr expr7 = expressionIterator.Current();
				num5++;
				ExprNamedArgumentSpecification exprNamedArgumentSpecification2;
				if ((exprNamedArgumentSpecification2 = expr7 as ExprNamedArgumentSpecification) != null)
				{
					exprNamedArgumentSpecification2.Value = this.tryConvert(exprNamedArgumentSpecification2.Value, elementType);
				}
				else
				{
					expr7 = this.tryConvert(expr7, elementType);
				}
				this.GetExprFactory().AppendItemToList(expr7, ref expr5, ref expr6);
				expressionIterator.MoveNext();
			}
			exprArrayInit2.DimensionSize = num5;
			exprArrayInit2.DimensionSizes[0] = num5;
			exprArrayInit2.OptionalArguments = expr5;
			this.GetExprFactory().AppendItemToList(exprArrayInit2, ref newArgs, ref expr);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000D894 File Offset: 0x0000BA94
		private void markFieldAssigned(Expr expr)
		{
			ExprField exprField;
			if ((expr.Flags & EXPRFLAG.EXF_LVALUE) != (EXPRFLAG)0 && (exprField = expr as ExprField) != null)
			{
				FieldSymbol fieldSymbol;
				do
				{
					fieldSymbol = exprField.FieldWithType.Field();
					fieldSymbol.isAssigned = true;
					expr = exprField.OptionalObject;
				}
				while (fieldSymbol.getClass().IsStruct() && !fieldSymbol.isStatic && expr != null && (exprField = expr as ExprField) != null);
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000D8F8 File Offset: 0x0000BAF8
		internal CType ChooseArrayIndexType(Expr args)
		{
			foreach (PredefinedType predefinedType in ExpressionBinder.s_rgptIntOp)
			{
				CType predefindType = this.GetPredefindType(predefinedType);
				foreach (Expr expr in args.ToEnumerable())
				{
					if (!this.canConvert(expr, predefindType))
					{
						goto IL_0054;
					}
				}
				return predefindType;
				IL_0054:;
			}
			return this.GetPredefindType(PredefinedType.PT_INT);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000D97C File Offset: 0x0000BB7C
		internal void FillInArgInfoFromArgList(ArgInfos argInfo, Expr args)
		{
			CType[] array = new CType[argInfo.carg];
			argInfo.fHasExprs = true;
			argInfo.prgexpr = new List<Expr>();
			int num = 0;
			Expr expr = args;
			while (expr != null)
			{
				ExprList exprList;
				Expr expr2;
				if ((exprList = expr as ExprList) != null)
				{
					expr2 = exprList.OptionalElement;
					expr = exprList.OptionalNextListNode;
				}
				else
				{
					expr2 = expr;
					expr = null;
				}
				if (expr2.Type != null)
				{
					array[num] = expr2.Type;
				}
				else
				{
					array[num] = this.GetTypes().GetErrorSym();
				}
				argInfo.prgexpr.Add(expr2);
				num++;
			}
			argInfo.types = this.GetGlobalSymbols().AllocParams(num, array);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000DA18 File Offset: 0x0000BC18
		private bool TryGetExpandedParams(TypeArray @params, int count, out TypeArray ppExpandedParams)
		{
			CType[] array;
			if (count < @params.Count - 1)
			{
				array = new CType[@params.Count - 1];
				@params.CopyItems(0, @params.Count - 1, array);
				ppExpandedParams = this.GetGlobalSymbols().AllocParams(@params.Count - 1, array);
				return true;
			}
			array = new CType[count];
			@params.CopyItems(0, @params.Count - 1, array);
			ArrayType arrayType;
			if ((arrayType = @params[@params.Count - 1] as ArrayType) == null)
			{
				ppExpandedParams = null;
				return false;
			}
			CType elementType = arrayType.GetElementType();
			for (int i = @params.Count - 1; i < count; i++)
			{
				array[i] = elementType;
			}
			ppExpandedParams = this.GetGlobalSymbols().AllocParams(array);
			return true;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000DAC8 File Offset: 0x0000BCC8
		public static bool IsMethPropCallable(MethodOrPropertySymbol sym, bool requireUC)
		{
			return (!sym.isOverride || sym.isHideByName) && (!requireUC || sym.isUserCallable());
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000DAE8 File Offset: 0x0000BCE8
		private bool isConvInTable(List<UdConvInfo> convTable, MethodSymbol meth, AggregateType ats, bool fSrc, bool fDst)
		{
			foreach (UdConvInfo udConvInfo in convTable)
			{
				if (udConvInfo.mwt.Meth() == meth && udConvInfo.mwt.GetType() == ats && udConvInfo.fSrcImplicit == fSrc && udConvInfo.fDstImplicit == fDst)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000DB68 File Offset: 0x0000BD68
		private static bool isConstantInRange(ExprConstant exprSrc, CType typeDest)
		{
			return ExpressionBinder.isConstantInRange(exprSrc, typeDest, false);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000DB74 File Offset: 0x0000BD74
		private static bool isConstantInRange(ExprConstant exprSrc, CType typeDest, bool realsOk)
		{
			FUNDTYPE fundtype = exprSrc.Type.fundType();
			FUNDTYPE fundtype2 = typeDest.fundType();
			if (fundtype > FUNDTYPE.FT_U8 || fundtype2 > FUNDTYPE.FT_U8)
			{
				if (!realsOk)
				{
					return false;
				}
				if (fundtype > FUNDTYPE.FT_R8 || fundtype2 > FUNDTYPE.FT_R8)
				{
					return false;
				}
			}
			if (fundtype2 > FUNDTYPE.FT_U8)
			{
				return true;
			}
			if (fundtype > FUNDTYPE.FT_U8)
			{
				double doubleVal = exprSrc.Val.DoubleVal;
				switch (fundtype2)
				{
				case FUNDTYPE.FT_I1:
					if (doubleVal > -129.0 && doubleVal < 128.0)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_I2:
					if (doubleVal > -32769.0 && doubleVal < 32768.0)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_I4:
					if (doubleVal > (double)ExpressionBinder.I64(-2147483649L) && doubleVal < (double)ExpressionBinder.I64((long)((ulong)(-2147483648))))
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_U1:
					if (doubleVal > -1.0 && doubleVal < 256.0)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_U2:
					if (doubleVal > -1.0 && doubleVal < 65536.0)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_U4:
					if (doubleVal > -1.0 && doubleVal < (double)ExpressionBinder.I64(4294967296L))
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_I8:
					if (doubleVal >= -9.223372036854776E+18 && doubleVal < 9.223372036854776E+18)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_U8:
					if (doubleVal > -1.0 && doubleVal < 1.8446744073709552E+19)
					{
						return true;
					}
					break;
				}
				return false;
			}
			if (fundtype == FUNDTYPE.FT_U8)
			{
				ulong uint64Value = exprSrc.UInt64Value;
				switch (fundtype2)
				{
				case FUNDTYPE.FT_I1:
					if (uint64Value <= 127UL)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_I2:
					if (uint64Value <= 32767UL)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_I4:
					if (uint64Value <= 2147483647UL)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_U1:
					if (uint64Value <= 255UL)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_U2:
					if (uint64Value <= 65535UL)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_U4:
					if (uint64Value <= (ulong)(-1))
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_I8:
					if (uint64Value <= 9223372036854775807UL)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_U8:
					return true;
				}
			}
			else
			{
				long int64Value = exprSrc.Int64Value;
				switch (fundtype2)
				{
				case FUNDTYPE.FT_I1:
					if (int64Value >= -128L && int64Value <= 127L)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_I2:
					if (int64Value >= -32768L && int64Value <= 32767L)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_I4:
					if (int64Value >= ExpressionBinder.I64(-2147483648L) && int64Value <= ExpressionBinder.I64(2147483647L))
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_U1:
					if (int64Value >= 0L && int64Value <= 255L)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_U2:
					if (int64Value >= 0L && int64Value <= 65535L)
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_U4:
					if (int64Value >= 0L && int64Value <= ExpressionBinder.I64((long)((ulong)(-1))))
					{
						return true;
					}
					break;
				case FUNDTYPE.FT_I8:
					return true;
				case FUNDTYPE.FT_U8:
					if (int64Value >= 0L)
					{
						return true;
					}
					break;
				}
			}
			return false;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000DE4C File Offset: 0x0000C04C
		private Name ekName(ExpressionKind ek)
		{
			return NameManager.GetPredefinedName(ExpressionBinder.s_EK2NAME[ek - ExpressionKind.EqualsParam]);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000DE5D File Offset: 0x0000C05D
		private void checkUnsafe(CType type)
		{
			if (type == null || type.isUnsafe())
			{
				throw this.ErrorContext.Error(ErrorCode.ERR_UnsafeNeeded, Array.Empty<ErrArg>());
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000DE80 File Offset: 0x0000C080
		private AggregateDeclaration ContextForMemberLookup()
		{
			return this.Context.ContextForMemberLookup;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000DE8D File Offset: 0x0000C08D
		private ExprWrap WrapShortLivedExpression(Expr expr)
		{
			return this.GetExprFactory().CreateWrap(expr);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000DE9B File Offset: 0x0000C09B
		private ExprAssignment GenerateOptimizedAssignment(Expr op1, Expr op2)
		{
			return this.GetExprFactory().CreateAssignment(op1, op2);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000DEAC File Offset: 0x0000C0AC
		internal static int CountArguments(Expr args, out bool typeErrors)
		{
			int num = 0;
			typeErrors = false;
			Expr expr = args;
			while (expr != null)
			{
				ExprList exprList;
				Expr expr2;
				if ((exprList = expr as ExprList) != null)
				{
					expr2 = exprList.OptionalElement;
					expr = exprList.OptionalNextListNode;
				}
				else
				{
					expr2 = expr;
					expr = null;
				}
				if (expr2.Type == null || expr2.Type is ErrorType)
				{
					typeErrors = true;
				}
				num++;
			}
			return num;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000DF00 File Offset: 0x0000C100
		private Expr BindNubValue(Expr exprSrc)
		{
			return this.m_nullable.BindValue(exprSrc);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000DF0E File Offset: 0x0000C10E
		private ExprCall BindNubNew(Expr exprSrc)
		{
			return this.m_nullable.BindNew(exprSrc);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000DF1C File Offset: 0x0000C11C
		private ExprBinOp BindUserDefinedBinOp(ExpressionKind ek, ExpressionBinder.BinOpArgInfo info)
		{
			MethPropWithInst methPropWithInst = null;
			if (info.pt1 <= PredefinedType.PT_ULONG && info.pt2 <= PredefinedType.PT_ULONG)
			{
				return null;
			}
			Expr expr = null;
			BinOpKind binopKind = info.binopKind;
			if (binopKind == BinOpKind.Logical)
			{
				ExprCall exprCall = this.BindUDBinop(ek - 58 + 52, info.arg1, info.arg2, true, out methPropWithInst);
				if (exprCall != null)
				{
					if (exprCall.IsOK)
					{
						expr = this.BindUserBoolOp(ek, exprCall);
					}
					else
					{
						expr = exprCall;
					}
				}
			}
			else
			{
				expr = this.BindUDBinop(ek, info.arg1, info.arg2, false, out methPropWithInst);
			}
			if (expr == null)
			{
				return null;
			}
			return this.GetExprFactory().CreateUserDefinedBinop(ek, expr.Type, info.arg1, info.arg2, expr, methPropWithInst);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000DFC1 File Offset: 0x0000C1C1
		private bool GetSpecialBinopSignatures(List<ExpressionBinder.BinOpFullSig> prgbofs, ExpressionBinder.BinOpArgInfo info)
		{
			return (info.pt1 > PredefinedType.PT_ULONG || info.pt2 > PredefinedType.PT_ULONG) && (this.GetDelBinOpSigs(prgbofs, info) || this.GetEnumBinOpSigs(prgbofs, info) || this.GetPtrBinOpSigs(prgbofs, info) || this.GetRefEqualSigs(prgbofs, info));
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000E004 File Offset: 0x0000C204
		private bool GetStandardAndLiftedBinopSignatures(List<ExpressionBinder.BinOpFullSig> rgbofs, ExpressionBinder.BinOpArgInfo info)
		{
			int num = 0;
			for (int i = 0; i < this.g_binopSignatures.Length; i++)
			{
				ExpressionBinder.BinOpSig binOpSig = this.g_binopSignatures[i];
				if ((binOpSig.mask & info.mask) != BinOpMask.None)
				{
					CType ctype = this.GetPredefindType(binOpSig.pt1);
					CType ctype2 = this.GetPredefindType(binOpSig.pt2);
					if (ctype != null && ctype2 != null)
					{
						ConvKind convKind = ExpressionBinder.GetConvKind(info.pt1, binOpSig.pt1);
						ConvKind convKind2 = ExpressionBinder.GetConvKind(info.pt2, binOpSig.pt2);
						LiftFlags liftFlags = LiftFlags.None;
						switch (convKind)
						{
						case ConvKind.Identity:
							if (convKind2 == ConvKind.Identity)
							{
								ExpressionBinder.BinOpFullSig binOpFullSig = new ExpressionBinder.BinOpFullSig(this, binOpSig);
								if (binOpFullSig.Type1() != null && binOpFullSig.Type2() != null)
								{
									rgbofs.Add(binOpFullSig);
									return true;
								}
							}
							break;
						case ConvKind.Implicit:
							break;
						case ConvKind.Explicit:
							if (!info.arg1.isCONSTANT_OK())
							{
								goto IL_031A;
							}
							if (!this.canConvert(info.arg1, ctype))
							{
								if (i < num || !binOpSig.CanLift())
								{
									goto IL_031A;
								}
								ctype = this.GetSymbolLoader().GetTypeManager().GetNullable(ctype);
								if (!this.canConvert(info.arg1, ctype))
								{
									goto IL_031A;
								}
								ConvKind convKind3 = ExpressionBinder.GetConvKind(info.ptRaw1, binOpSig.pt1);
								if (convKind3 - ConvKind.Identity > 1)
								{
									liftFlags |= LiftFlags.Convert1;
								}
								else
								{
									liftFlags |= LiftFlags.Lift1;
								}
							}
							break;
						case ConvKind.Unknown:
							if (!this.canConvert(info.arg1, ctype))
							{
								if (i < num || !binOpSig.CanLift())
								{
									goto IL_031A;
								}
								ctype = this.GetSymbolLoader().GetTypeManager().GetNullable(ctype);
								if (!this.canConvert(info.arg1, ctype))
								{
									goto IL_031A;
								}
								ConvKind convKind3 = ExpressionBinder.GetConvKind(info.ptRaw1, binOpSig.pt1);
								if (convKind3 - ConvKind.Identity > 1)
								{
									liftFlags |= LiftFlags.Convert1;
								}
								else
								{
									liftFlags |= LiftFlags.Lift1;
								}
							}
							break;
						case ConvKind.None:
							goto IL_031A;
						default:
							goto IL_031A;
						}
						switch (convKind2)
						{
						case ConvKind.Identity:
						case ConvKind.Implicit:
							break;
						case ConvKind.Explicit:
							if (!info.arg2.isCONSTANT_OK())
							{
								goto IL_031A;
							}
							if (!this.canConvert(info.arg2, ctype2))
							{
								if (i < num || !binOpSig.CanLift())
								{
									goto IL_031A;
								}
								ctype2 = this.GetSymbolLoader().GetTypeManager().GetNullable(ctype2);
								if (!this.canConvert(info.arg2, ctype2))
								{
									goto IL_031A;
								}
								ConvKind convKind3 = ExpressionBinder.GetConvKind(info.ptRaw2, binOpSig.pt2);
								if (convKind3 - ConvKind.Identity > 1)
								{
									liftFlags |= LiftFlags.Convert2;
								}
								else
								{
									liftFlags |= LiftFlags.Lift2;
								}
							}
							break;
						case ConvKind.Unknown:
							if (!this.canConvert(info.arg2, ctype2))
							{
								if (i < num || !binOpSig.CanLift())
								{
									goto IL_031A;
								}
								ctype2 = this.GetSymbolLoader().GetTypeManager().GetNullable(ctype2);
								if (!this.canConvert(info.arg2, ctype2))
								{
									goto IL_031A;
								}
								ConvKind convKind3 = ExpressionBinder.GetConvKind(info.ptRaw2, binOpSig.pt2);
								if (convKind3 - ConvKind.Identity > 1)
								{
									liftFlags |= LiftFlags.Convert2;
								}
								else
								{
									liftFlags |= LiftFlags.Lift2;
								}
							}
							break;
						case ConvKind.None:
							goto IL_031A;
						default:
							goto IL_031A;
						}
						if (liftFlags != LiftFlags.None)
						{
							rgbofs.Add(new ExpressionBinder.BinOpFullSig(ctype, ctype2, binOpSig.pfn, binOpSig.grfos, liftFlags, binOpSig.fnkind));
							num = i + binOpSig.cbosSkip + 1;
						}
						else
						{
							rgbofs.Add(new ExpressionBinder.BinOpFullSig(this, binOpSig));
							i += binOpSig.cbosSkip;
						}
					}
				}
				IL_031A:;
			}
			return false;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000E340 File Offset: 0x0000C540
		private int FindBestSignatureInList(List<ExpressionBinder.BinOpFullSig> binopSignatures, ExpressionBinder.BinOpArgInfo info)
		{
			if (binopSignatures.Count == 1)
			{
				return 0;
			}
			int num = 0;
			for (int i = 1; i < binopSignatures.Count; i++)
			{
				if (num < 0)
				{
					num = i;
				}
				else
				{
					int num2 = this.WhichBofsIsBetter(binopSignatures[num], binopSignatures[i], info.type1, info.type2);
					if (num2 == 0)
					{
						num = -1;
					}
					else if (num2 > 0)
					{
						num = i;
					}
				}
			}
			if (num == -1)
			{
				return -1;
			}
			for (int i = 0; i < binopSignatures.Count; i++)
			{
				if (i != num && this.WhichBofsIsBetter(binopSignatures[num], binopSignatures[i], info.type1, info.type2) >= 0)
				{
					return -1;
				}
			}
			return num;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000E3E4 File Offset: 0x0000C5E4
		private ExprBinOp bindNullEqualityComparison(ExpressionKind ek, ExpressionBinder.BinOpArgInfo info)
		{
			Expr expr = info.arg1;
			Expr expr2 = info.arg2;
			if (info.binopKind == BinOpKind.Equal)
			{
				CType predefindType = this.GetPredefindType(PredefinedType.PT_BOOL);
				ExprBinOp exprBinOp = null;
				if (info.type1 is NullableType && info.type2 is NullType)
				{
					expr2 = this.GetExprFactory().CreateZeroInit(info.type1);
					exprBinOp = this.GetExprFactory().CreateBinop(ek, predefindType, expr, expr2);
				}
				if (info.type1 is NullType && info.type2 is NullableType)
				{
					expr = this.GetExprFactory().CreateZeroInit(info.type2);
					exprBinOp = this.GetExprFactory().CreateBinop(ek, predefindType, expr, expr2);
				}
				if (exprBinOp != null)
				{
					exprBinOp.IsLifted = true;
					return exprBinOp;
				}
			}
			return (ExprBinOp)this.BadOperatorTypesError(ek, info.arg1, info.arg2, this.GetTypes().GetErrorSym());
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000E4BC File Offset: 0x0000C6BC
		public Expr BindStandardBinop(ExpressionKind ek, Expr arg1, Expr arg2)
		{
			EXPRFLAG exprflag = (EXPRFLAG)0;
			ExpressionBinder.BinOpArgInfo binOpArgInfo = new ExpressionBinder.BinOpArgInfo(arg1, arg2);
			if (!this.GetBinopKindAndFlags(ek, out binOpArgInfo.binopKind, out exprflag))
			{
				return this.BadOperatorTypesError(ek, arg1, arg2);
			}
			binOpArgInfo.mask = (BinOpMask)(1 << (int)binOpArgInfo.binopKind);
			List<ExpressionBinder.BinOpFullSig> list = new List<ExpressionBinder.BinOpFullSig>();
			ExprBinOp exprBinOp = this.BindUserDefinedBinOp(ek, binOpArgInfo);
			if (exprBinOp != null)
			{
				return exprBinOp;
			}
			bool flag = this.GetSpecialBinopSignatures(list, binOpArgInfo);
			if (!flag)
			{
				flag = this.GetStandardAndLiftedBinopSignatures(list, binOpArgInfo);
			}
			int num;
			if (flag)
			{
				num = list.Count - 1;
			}
			else
			{
				if (list.Count == 0)
				{
					return this.bindNullEqualityComparison(ek, binOpArgInfo);
				}
				num = this.FindBestSignatureInList(list, binOpArgInfo);
				if (num < 0)
				{
					throw this.AmbiguousOperatorError(ek, arg1, arg2);
				}
			}
			return this.BindStandardBinopCore(binOpArgInfo, list[num], ek, exprflag);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000E578 File Offset: 0x0000C778
		private Expr BindStandardBinopCore(ExpressionBinder.BinOpArgInfo info, ExpressionBinder.BinOpFullSig bofs, ExpressionKind ek, EXPRFLAG flags)
		{
			if (bofs.pfn == null)
			{
				return this.BadOperatorTypesError(ek, info.arg1, info.arg2);
			}
			if (!bofs.isLifted() || !bofs.AutoLift())
			{
				Expr expr = info.arg1;
				Expr expr2 = info.arg2;
				if (bofs.ConvertOperandsBeforeBinding())
				{
					expr = this.mustConvert(expr, bofs.Type1());
					expr2 = this.mustConvert(expr2, bofs.Type2());
				}
				if (bofs.fnkind == BinOpFuncKind.BoolBitwiseOp)
				{
					return this.BindBoolBitwiseOp(ek, flags, expr, expr2, bofs);
				}
				return bofs.pfn(ek, flags, expr, expr2);
			}
			else
			{
				if (this.IsEnumArithmeticBinOp(ek, info))
				{
					Expr expr3 = info.arg1;
					Expr expr4 = info.arg2;
					if (bofs.ConvertOperandsBeforeBinding())
					{
						expr3 = this.mustConvert(expr3, bofs.Type1());
						expr4 = this.mustConvert(expr4, bofs.Type2());
					}
					return this.BindLiftedEnumArithmeticBinOp(ek, flags, expr3, expr4);
				}
				return this.BindLiftedStandardBinOp(info, bofs, ek, flags);
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000E660 File Offset: 0x0000C860
		private ExprBinOp BindLiftedStandardBinOp(ExpressionBinder.BinOpArgInfo info, ExpressionBinder.BinOpFullSig bofs, ExpressionKind ek, EXPRFLAG flags)
		{
			Expr arg = info.arg1;
			Expr arg2 = info.arg2;
			Expr expr = null;
			Expr expr2 = null;
			Expr expr3 = null;
			Expr expr4 = null;
			Expr expr5 = null;
			this.LiftArgument(arg, bofs.Type1(), bofs.ConvertFirst(), out expr, out expr3);
			this.LiftArgument(arg2, bofs.Type2(), bofs.ConvertSecond(), out expr2, out expr4);
			if (!expr3.isNull() && !expr4.isNull())
			{
				expr5 = bofs.pfn(ek, flags, expr3, expr4);
			}
			CType ctype;
			if (info.binopKind == BinOpKind.Compare || info.binopKind == BinOpKind.Equal)
			{
				ctype = this.GetPredefindType(PredefinedType.PT_BOOL);
			}
			else
			{
				if (bofs.fnkind == BinOpFuncKind.EnumBinOp)
				{
					AggregateType aggregateType;
					ctype = this.GetEnumBinOpType(ek, expr3.Type, expr4.Type, out aggregateType);
				}
				else
				{
					ctype = expr.Type;
				}
				ctype = ((ctype is NullableType) ? ctype : this.GetSymbolLoader().GetTypeManager().GetNullable(ctype));
			}
			ExprBinOp exprBinOp = this.GetExprFactory().CreateBinop(ek, ctype, expr, expr2);
			this.mustCast(expr5, ctype, (CONVERTTYPE)0);
			exprBinOp.IsLifted = true;
			exprBinOp.Flags |= flags;
			return exprBinOp;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000E77C File Offset: 0x0000C97C
		private void LiftArgument(Expr pArgument, CType pParameterType, bool bConvertBeforeLift, out Expr ppLiftedArgument, out Expr ppNonLiftedArgument)
		{
			Expr expr = this.mustConvert(pArgument, pParameterType);
			if (expr != pArgument)
			{
				this.MarkAsIntermediateConversion(expr);
			}
			Expr expr2 = pArgument;
			NullableType nullableType;
			if ((nullableType = pParameterType as NullableType) != null)
			{
				if (expr2.isNull())
				{
					expr2 = this.mustCast(expr2, pParameterType);
				}
				expr2 = this.mustCast(expr2, nullableType.GetUnderlyingType());
				if (bConvertBeforeLift)
				{
					this.MarkAsIntermediateConversion(expr2);
				}
			}
			else
			{
				expr2 = expr;
			}
			ppLiftedArgument = expr;
			ppNonLiftedArgument = expr2;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000E7E0 File Offset: 0x0000C9E0
		private bool GetDelBinOpSigs(List<ExpressionBinder.BinOpFullSig> prgbofs, ExpressionBinder.BinOpArgInfo info)
		{
			if (!info.ValidForDelegate())
			{
				return false;
			}
			if (!info.type1.isDelegateType() && !info.type2.isDelegateType())
			{
				return false;
			}
			if (info.type1 == info.type2)
			{
				prgbofs.Add(new ExpressionBinder.BinOpFullSig(info.type1, info.type2, new ExpressionBinder.PfnBindBinOp(this.BindDelBinOp), OpSigFlags.Convert, LiftFlags.None, BinOpFuncKind.DelBinOp));
				return true;
			}
			bool flag = info.type2.isDelegateType() && this.canConvert(info.arg1, info.type2);
			bool flag2 = info.type1.isDelegateType() && this.canConvert(info.arg2, info.type1);
			if (flag)
			{
				prgbofs.Add(new ExpressionBinder.BinOpFullSig(info.type2, info.type2, new ExpressionBinder.PfnBindBinOp(this.BindDelBinOp), OpSigFlags.Convert, LiftFlags.None, BinOpFuncKind.DelBinOp));
			}
			if (flag2)
			{
				prgbofs.Add(new ExpressionBinder.BinOpFullSig(info.type1, info.type1, new ExpressionBinder.PfnBindBinOp(this.BindDelBinOp), OpSigFlags.Convert, LiftFlags.None, BinOpFuncKind.DelBinOp));
			}
			return false;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000E8E0 File Offset: 0x0000CAE0
		private bool CanConvertArg1(ExpressionBinder.BinOpArgInfo info, CType typeDst, out LiftFlags pgrflt, out CType ptypeSig1, out CType ptypeSig2)
		{
			ptypeSig1 = null;
			ptypeSig2 = null;
			if (this.canConvert(info.arg1, typeDst))
			{
				pgrflt = LiftFlags.None;
			}
			else
			{
				pgrflt = LiftFlags.None;
				typeDst = this.GetSymbolLoader().GetTypeManager().GetNullable(typeDst);
				if (!this.canConvert(info.arg1, typeDst))
				{
					return false;
				}
				pgrflt = LiftFlags.Convert1;
			}
			ptypeSig1 = typeDst;
			if (info.type2 is NullableType)
			{
				pgrflt |= LiftFlags.Lift2;
				ptypeSig2 = this.GetSymbolLoader().GetTypeManager().GetNullable(info.typeRaw2);
			}
			else
			{
				ptypeSig2 = info.typeRaw2;
			}
			return true;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000E970 File Offset: 0x0000CB70
		private bool CanConvertArg2(ExpressionBinder.BinOpArgInfo info, CType typeDst, out LiftFlags pgrflt, out CType ptypeSig1, out CType ptypeSig2)
		{
			ptypeSig1 = null;
			ptypeSig2 = null;
			if (this.canConvert(info.arg2, typeDst))
			{
				pgrflt = LiftFlags.None;
			}
			else
			{
				pgrflt = LiftFlags.None;
				typeDst = this.GetSymbolLoader().GetTypeManager().GetNullable(typeDst);
				if (!this.canConvert(info.arg2, typeDst))
				{
					return false;
				}
				pgrflt = LiftFlags.Convert2;
			}
			ptypeSig2 = typeDst;
			if (info.type1 is NullableType)
			{
				pgrflt |= LiftFlags.Lift1;
				ptypeSig1 = this.GetSymbolLoader().GetTypeManager().GetNullable(info.typeRaw1);
			}
			else
			{
				ptypeSig1 = info.typeRaw1;
			}
			return true;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000EA00 File Offset: 0x0000CC00
		private void RecordBinOpSigFromArgs(List<ExpressionBinder.BinOpFullSig> prgbofs, ExpressionBinder.BinOpArgInfo info)
		{
			LiftFlags liftFlags = LiftFlags.None;
			CType ctype;
			if (info.type1 != info.typeRaw1)
			{
				liftFlags |= LiftFlags.Lift1;
				ctype = this.GetSymbolLoader().GetTypeManager().GetNullable(info.typeRaw1);
			}
			else
			{
				ctype = info.typeRaw1;
			}
			CType ctype2;
			if (info.type2 != info.typeRaw2)
			{
				liftFlags |= LiftFlags.Lift2;
				ctype2 = this.GetSymbolLoader().GetTypeManager().GetNullable(info.typeRaw2);
			}
			else
			{
				ctype2 = info.typeRaw2;
			}
			prgbofs.Add(new ExpressionBinder.BinOpFullSig(ctype, ctype2, new ExpressionBinder.PfnBindBinOp(this.BindEnumBinOp), OpSigFlags.Value, liftFlags, BinOpFuncKind.EnumBinOp));
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000EA90 File Offset: 0x0000CC90
		private bool GetEnumBinOpSigs(List<ExpressionBinder.BinOpFullSig> prgbofs, ExpressionBinder.BinOpArgInfo info)
		{
			if (!info.typeRaw1.isEnumType() && !info.typeRaw2.isEnumType())
			{
				return false;
			}
			CType ctype = null;
			CType ctype2 = null;
			LiftFlags liftFlags = LiftFlags.None;
			if (info.typeRaw1 == info.typeRaw2)
			{
				if (!info.ValidForEnum())
				{
					return false;
				}
				this.RecordBinOpSigFromArgs(prgbofs, info);
				return true;
			}
			else
			{
				bool flag;
				if (info.typeRaw1.isEnumType())
				{
					flag = info.typeRaw2 == info.typeRaw1.underlyingEnumType() && info.ValidForEnumAndUnderlyingType();
				}
				else
				{
					flag = info.typeRaw1 == info.typeRaw2.underlyingEnumType() && info.ValidForUnderlyingTypeAndEnum();
				}
				if (flag)
				{
					this.RecordBinOpSigFromArgs(prgbofs, info);
					return true;
				}
				if (info.typeRaw1.isEnumType())
				{
					flag = (info.ValidForEnum() && this.CanConvertArg2(info, info.typeRaw1, out liftFlags, out ctype, out ctype2)) || (info.ValidForEnumAndUnderlyingType() && this.CanConvertArg2(info, info.typeRaw1.underlyingEnumType(), out liftFlags, out ctype, out ctype2));
				}
				else
				{
					flag = (info.ValidForEnum() && this.CanConvertArg1(info, info.typeRaw2, out liftFlags, out ctype, out ctype2)) || (info.ValidForEnumAndUnderlyingType() && this.CanConvertArg1(info, info.typeRaw2.underlyingEnumType(), out liftFlags, out ctype, out ctype2));
				}
				if (flag)
				{
					prgbofs.Add(new ExpressionBinder.BinOpFullSig(ctype, ctype2, new ExpressionBinder.PfnBindBinOp(this.BindEnumBinOp), OpSigFlags.Value, liftFlags, BinOpFuncKind.EnumBinOp));
				}
				return false;
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000EBEE File Offset: 0x0000CDEE
		private bool IsEnumArithmeticBinOp(ExpressionKind ek, ExpressionBinder.BinOpArgInfo info)
		{
			if (ek != ExpressionKind.Add)
			{
				return ek == ExpressionKind.Subtract && (info.typeRaw1.isEnumType() | info.typeRaw2.isEnumType());
			}
			return info.typeRaw1.isEnumType() ^ info.typeRaw2.isEnumType();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000EC30 File Offset: 0x0000CE30
		private bool GetPtrBinOpSigs(List<ExpressionBinder.BinOpFullSig> prgbofs, ExpressionBinder.BinOpArgInfo info)
		{
			if (!(info.type1 is PointerType) && !(info.type2 is PointerType))
			{
				return false;
			}
			if (info.type1 is PointerType && info.type2 is PointerType)
			{
				if (info.ValidForVoidPointer())
				{
					prgbofs.Add(new ExpressionBinder.BinOpFullSig(info.type1, info.type2, new ExpressionBinder.PfnBindBinOp(this.BindPtrCmpOp), OpSigFlags.None, LiftFlags.None, BinOpFuncKind.PtrCmpOp));
					return true;
				}
				if (info.type1 == info.type2 && info.ValidForPointer())
				{
					prgbofs.Add(new ExpressionBinder.BinOpFullSig(info.type1, info.type2, new ExpressionBinder.PfnBindBinOp(this.BindPtrBinOp), OpSigFlags.None, LiftFlags.None, BinOpFuncKind.PtrBinOp));
					return true;
				}
				return false;
			}
			else if (info.type1 is PointerType)
			{
				if (info.type2 is NullType)
				{
					if (!info.ValidForVoidPointer())
					{
						return false;
					}
					prgbofs.Add(new ExpressionBinder.BinOpFullSig(info.type1, info.type1, new ExpressionBinder.PfnBindBinOp(this.BindPtrCmpOp), OpSigFlags.Convert, LiftFlags.None, BinOpFuncKind.PtrCmpOp));
					return true;
				}
				else
				{
					if (!info.ValidForPointerAndNumber())
					{
						return false;
					}
					uint num = 0U;
					while ((ulong)num < (ulong)((long)ExpressionBinder.s_rgptIntOp.Length))
					{
						CType ctype;
						if (this.canConvert(info.arg2, ctype = this.GetPredefindType(ExpressionBinder.s_rgptIntOp[(int)num])))
						{
							prgbofs.Add(new ExpressionBinder.BinOpFullSig(info.type1, ctype, new ExpressionBinder.PfnBindBinOp(this.BindPtrBinOp), OpSigFlags.Convert, LiftFlags.None, BinOpFuncKind.PtrBinOp));
							return true;
						}
						num += 1U;
					}
					return false;
				}
			}
			else if (info.type1 is NullType)
			{
				if (!info.ValidForVoidPointer())
				{
					return false;
				}
				prgbofs.Add(new ExpressionBinder.BinOpFullSig(info.type2, info.type2, new ExpressionBinder.PfnBindBinOp(this.BindPtrCmpOp), OpSigFlags.Convert, LiftFlags.None, BinOpFuncKind.PtrCmpOp));
				return true;
			}
			else
			{
				if (!info.ValidForNumberAndPointer())
				{
					return false;
				}
				uint num2 = 0U;
				while ((ulong)num2 < (ulong)((long)ExpressionBinder.s_rgptIntOp.Length))
				{
					CType ctype;
					if (this.canConvert(info.arg1, ctype = this.GetPredefindType(ExpressionBinder.s_rgptIntOp[(int)num2])))
					{
						prgbofs.Add(new ExpressionBinder.BinOpFullSig(ctype, info.type2, new ExpressionBinder.PfnBindBinOp(this.BindPtrBinOp), OpSigFlags.Convert, LiftFlags.None, BinOpFuncKind.PtrBinOp));
						return true;
					}
					num2 += 1U;
				}
				return false;
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000EE30 File Offset: 0x0000D030
		private bool GetRefEqualSigs(List<ExpressionBinder.BinOpFullSig> prgbofs, ExpressionBinder.BinOpArgInfo info)
		{
			if (info.mask != BinOpMask.Equal)
			{
				return false;
			}
			if (info.type1 != info.typeRaw1 || info.type2 != info.typeRaw2)
			{
				return false;
			}
			bool flag = false;
			CType ctype = info.type1;
			CType ctype2 = info.type2;
			CType predefindType = this.GetPredefindType(PredefinedType.PT_OBJECT);
			CType ctype3 = null;
			if (ctype is NullType && ctype2 is NullType)
			{
				ctype3 = predefindType;
				flag = true;
			}
			else
			{
				CType predefindType2 = this.GetPredefindType(PredefinedType.PT_DELEGATE);
				if (this.canConvert(info.arg1, predefindType2) && this.canConvert(info.arg2, predefindType2) && !ctype.isDelegateType() && !ctype2.isDelegateType())
				{
					prgbofs.Add(new ExpressionBinder.BinOpFullSig(predefindType2, predefindType2, new ExpressionBinder.PfnBindBinOp(this.BindDelBinOp), OpSigFlags.Convert, LiftFlags.None, BinOpFuncKind.DelBinOp));
				}
				if (ctype.fundType() != FUNDTYPE.FT_REF)
				{
					return false;
				}
				if (ctype2 is NullType)
				{
					flag = true;
					ctype3 = predefindType;
				}
				else
				{
					if (ctype2.fundType() != FUNDTYPE.FT_REF)
					{
						return false;
					}
					if (ctype is NullType)
					{
						flag = true;
						ctype3 = predefindType;
					}
					else
					{
						if (!this.canCast(ctype, ctype2, CONVERTTYPE.NOUDC) && !this.canCast(ctype2, ctype, CONVERTTYPE.NOUDC))
						{
							return false;
						}
						if (ctype.isInterfaceType() || ctype.isPredefType(PredefinedType.PT_STRING) || this.GetSymbolLoader().HasBaseConversion(ctype, predefindType2))
						{
							ctype = predefindType;
						}
						else if (ctype is ArrayType)
						{
							ctype = this.GetPredefindType(PredefinedType.PT_ARRAY);
						}
						else if (!ctype.isClassType())
						{
							return false;
						}
						if (ctype2.isInterfaceType() || ctype2.isPredefType(PredefinedType.PT_STRING) || this.GetSymbolLoader().HasBaseConversion(ctype2, predefindType2))
						{
							ctype2 = predefindType;
						}
						else if (ctype2 is ArrayType)
						{
							ctype2 = this.GetPredefindType(PredefinedType.PT_ARRAY);
						}
						else if (!ctype2.isClassType())
						{
							return false;
						}
						if (this.GetSymbolLoader().HasBaseConversion(ctype2, ctype))
						{
							ctype3 = ctype;
						}
						else if (this.GetSymbolLoader().HasBaseConversion(ctype, ctype2))
						{
							ctype3 = ctype2;
						}
					}
				}
			}
			prgbofs.Add(new ExpressionBinder.BinOpFullSig(ctype3, ctype3, new ExpressionBinder.PfnBindBinOp(this.BindRefCmpOp), OpSigFlags.None, LiftFlags.None, BinOpFuncKind.RefCmpOp));
			return flag;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000F018 File Offset: 0x0000D218
		private int WhichBofsIsBetter(ExpressionBinder.BinOpFullSig bofs1, ExpressionBinder.BinOpFullSig bofs2, CType type1, CType type2)
		{
			BetterType betterType;
			BetterType betterType2;
			if (bofs1.FPreDef() && bofs2.FPreDef())
			{
				betterType = this.WhichTypeIsBetter(bofs1.pt1, bofs2.pt1, type1);
				betterType2 = this.WhichTypeIsBetter(bofs1.pt2, bofs2.pt2, type2);
			}
			else
			{
				betterType = this.WhichTypeIsBetter(bofs1.Type1(), bofs2.Type1(), type1);
				betterType2 = this.WhichTypeIsBetter(bofs1.Type2(), bofs2.Type2(), type2);
			}
			int num;
			if (betterType != BetterType.Left)
			{
				if (betterType != BetterType.Right)
				{
					num = 0;
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				num = -1;
			}
			if (betterType2 != BetterType.Left)
			{
				if (betterType2 == BetterType.Right)
				{
					num++;
				}
			}
			else
			{
				num--;
			}
			return num;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000F0B4 File Offset: 0x0000D2B4
		private static bool CalculateExprAndUnaryOpKinds(OperatorKind op, bool bChecked, out ExpressionKind ek, out UnaOpKind uok, out EXPRFLAG flags)
		{
			flags = (EXPRFLAG)0;
			ek = ExpressionKind.Block;
			uok = UnaOpKind.Plus;
			switch (op)
			{
			case OperatorKind.OP_UPLUS:
				uok = UnaOpKind.Plus;
				ek = ExpressionKind.UnaryPlus;
				break;
			case OperatorKind.OP_NEG:
				if (bChecked)
				{
					flags |= EXPRFLAG.EXF_CHECKOVERFLOW;
				}
				uok = UnaOpKind.Minus;
				ek = ExpressionKind.Negate;
				break;
			case OperatorKind.OP_BITNOT:
				uok = UnaOpKind.Tilde;
				ek = ExpressionKind.BitwiseNot;
				break;
			case OperatorKind.OP_LOGNOT:
				uok = UnaOpKind.Bang;
				ek = ExpressionKind.LogicalNot;
				break;
			case OperatorKind.OP_PREINC:
				if (bChecked)
				{
					flags |= EXPRFLAG.EXF_CHECKOVERFLOW;
				}
				uok = UnaOpKind.IncDec;
				ek = ExpressionKind.Add;
				break;
			case OperatorKind.OP_PREDEC:
				if (bChecked)
				{
					flags |= EXPRFLAG.EXF_CHECKOVERFLOW;
				}
				uok = UnaOpKind.IncDec;
				ek = ExpressionKind.Subtract;
				break;
			default:
				if (op != OperatorKind.OP_POSTINC)
				{
					if (op != OperatorKind.OP_POSTDEC)
					{
						return false;
					}
					flags |= EXPRFLAG.EXF_OPERATOR;
					if (bChecked)
					{
						flags |= EXPRFLAG.EXF_CHECKOVERFLOW;
					}
					uok = UnaOpKind.IncDec;
					ek = ExpressionKind.Subtract;
				}
				else
				{
					flags |= EXPRFLAG.EXF_OPERATOR;
					if (bChecked)
					{
						flags |= EXPRFLAG.EXF_CHECKOVERFLOW;
					}
					uok = UnaOpKind.IncDec;
					ek = ExpressionKind.Add;
				}
				break;
			}
			return true;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000F1A8 File Offset: 0x0000D3A8
		public Expr BindStandardUnaryOperator(OperatorKind op, Expr pArgument)
		{
			ExpressionKind expressionKind;
			UnaOpKind unaOpKind;
			EXPRFLAG exprflag;
			if (pArgument.Type == null || !ExpressionBinder.CalculateExprAndUnaryOpKinds(op, this.Context.CheckedNormal, out expressionKind, out unaOpKind, out exprflag))
			{
				return this.BadOperatorTypesError(ExpressionKind.UnaryOp, pArgument, null);
			}
			UnaOpMask unaOpMask = (UnaOpMask)(1 << (int)unaOpKind);
			CType type = pArgument.Type;
			List<ExpressionBinder.UnaOpFullSig> list = new List<ExpressionBinder.UnaOpFullSig>();
			Expr expr = null;
			UnaryOperatorSignatureFindResult unaryOperatorSignatureFindResult = this.PopulateSignatureList(pArgument, unaOpKind, unaOpMask, expressionKind, exprflag, list, out expr);
			int num = list.Count - 1;
			if (unaryOperatorSignatureFindResult == UnaryOperatorSignatureFindResult.Return)
			{
				return expr;
			}
			if (unaryOperatorSignatureFindResult != UnaryOperatorSignatureFindResult.Match)
			{
				if (!this.FindApplicableSignatures(pArgument, unaOpMask, list))
				{
					if (list.Count == 0)
					{
						return this.BadOperatorTypesError(expressionKind, pArgument, null);
					}
					num = 0;
					if (list.Count != 1)
					{
						for (int i = 1; i < list.Count; i++)
						{
							if (num < 0)
							{
								num = i;
							}
							else
							{
								int num2 = this.WhichUofsIsBetter(list[num], list[i], type);
								if (num2 == 0)
								{
									num = -1;
								}
								else if (num2 > 0)
								{
									num = i;
								}
							}
						}
						if (num < 0)
						{
							throw this.AmbiguousOperatorError(expressionKind, pArgument, null);
						}
						for (int j = 0; j < list.Count; j++)
						{
							if (j != num && this.WhichUofsIsBetter(list[num], list[j], type) >= 0)
							{
								throw this.AmbiguousOperatorError(expressionKind, pArgument, null);
							}
						}
					}
				}
				else
				{
					num = list.Count - 1;
				}
			}
			ExpressionBinder.UnaOpFullSig unaOpFullSig = list[num];
			if (unaOpFullSig.pfn == null)
			{
				if (unaOpKind == UnaOpKind.IncDec)
				{
					return this.BindIncOp(expressionKind, exprflag, pArgument, unaOpFullSig);
				}
				return this.BadOperatorTypesError(expressionKind, pArgument, null);
			}
			else
			{
				if (unaOpFullSig.isLifted())
				{
					return this.BindLiftedStandardUnop(expressionKind, exprflag, pArgument, unaOpFullSig);
				}
				Expr expr2 = this.tryConvert(pArgument, unaOpFullSig.GetType());
				if (expr2 == null)
				{
					expr2 = this.mustCast(pArgument, unaOpFullSig.GetType(), CONVERTTYPE.NOUDC);
				}
				return unaOpFullSig.pfn(expressionKind, exprflag, expr2);
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000F37C File Offset: 0x0000D57C
		private UnaryOperatorSignatureFindResult PopulateSignatureList(Expr pArgument, UnaOpKind unaryOpKind, UnaOpMask unaryOpMask, ExpressionKind exprKind, EXPRFLAG flags, List<ExpressionBinder.UnaOpFullSig> pSignatures, out Expr ppResult)
		{
			ppResult = null;
			CType type = pArgument.Type;
			CType ctype = type.StripNubs();
			if ((ctype.isPredefined() ? ctype.getPredefType() : PredefinedType.PT_COUNT) > PredefinedType.PT_ULONG)
			{
				if (ctype.isEnumType())
				{
					if ((unaryOpMask & (UnaOpMask.Tilde | UnaOpMask.IncDec)) != UnaOpMask.None)
					{
						LiftFlags liftFlags = LiftFlags.None;
						CType ctype2 = type;
						NullableType nullableType;
						if ((nullableType = ctype2 as NullableType) != null)
						{
							if (nullableType.GetUnderlyingType() != ctype)
							{
								ctype2 = this.GetSymbolLoader().GetTypeManager().GetNullable(ctype);
							}
							liftFlags = LiftFlags.Lift1;
						}
						if (unaryOpKind == UnaOpKind.Tilde)
						{
							pSignatures.Add(new ExpressionBinder.UnaOpFullSig(ctype2.getAggregate().GetUnderlyingType(), new ExpressionBinder.PfnBindUnaOp(this.BindEnumUnaOp), liftFlags, UnaOpFuncKind.EnumUnaOp));
						}
						else
						{
							pSignatures.Add(new ExpressionBinder.UnaOpFullSig(ctype2.getAggregate().GetUnderlyingType(), null, liftFlags, UnaOpFuncKind.None));
						}
						return UnaryOperatorSignatureFindResult.Match;
					}
				}
				else if (unaryOpKind == UnaOpKind.IncDec)
				{
					if (type is PointerType)
					{
						pSignatures.Add(new ExpressionBinder.UnaOpFullSig(type, null, LiftFlags.None, UnaOpFuncKind.None));
						return UnaryOperatorSignatureFindResult.Match;
					}
					ExprMultiGet exprMultiGet = this.GetExprFactory().CreateMultiGet((EXPRFLAG)0, type, null);
					Expr expr = this.bindUDUnop(exprKind - 45 + 36, exprMultiGet);
					if (expr != null)
					{
						if (expr.Type != null && !(expr.Type is ErrorType) && expr.Type != type)
						{
							expr = this.mustConvert(expr, type);
						}
						ExprMulti exprMulti = this.GetExprFactory().CreateMulti(EXPRFLAG.EXF_ASSGOP | flags, type, pArgument, expr);
						exprMultiGet.OptionalMulti = exprMulti;
						if (!this.checkLvalue(pArgument, CheckLvalueKind.Increment))
						{
							exprMulti.SetError();
						}
						ppResult = exprMulti;
						return UnaryOperatorSignatureFindResult.Return;
					}
				}
				else
				{
					Expr expr2 = this.bindUDUnop(exprKind, pArgument);
					if (expr2 != null)
					{
						ppResult = expr2;
						return UnaryOperatorSignatureFindResult.Return;
					}
				}
			}
			return UnaryOperatorSignatureFindResult.Continue;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000F500 File Offset: 0x0000D700
		private bool FindApplicableSignatures(Expr pArgument, UnaOpMask unaryOpMask, List<ExpressionBinder.UnaOpFullSig> pSignatures)
		{
			long num = 0L;
			CType type = pArgument.Type;
			CType ctype = type.StripNubs();
			PredefinedType predefinedType = (type.isPredefined() ? type.getPredefType() : PredefinedType.PT_COUNT);
			PredefinedType predefinedType2 = (ctype.isPredefined() ? ctype.getPredefType() : PredefinedType.PT_COUNT);
			for (int i = 0; i < this.g_rguos.Length; i++)
			{
				ExpressionBinder.UnaOpSig unaOpSig = this.g_rguos[i];
				if ((unaOpSig.grfuom & unaryOpMask) != UnaOpMask.None)
				{
					ConvKind convKind = ExpressionBinder.GetConvKind(predefinedType, this.g_rguos[i].pt);
					CType ctype2 = null;
					switch (convKind)
					{
					case ConvKind.Identity:
					{
						ExpressionBinder.UnaOpFullSig unaOpFullSig = new ExpressionBinder.UnaOpFullSig(this, unaOpSig);
						if (unaOpFullSig.GetType() != null)
						{
							pSignatures.Add(unaOpFullSig);
							return true;
						}
						break;
					}
					case ConvKind.Implicit:
						break;
					case ConvKind.Explicit:
						if (!pArgument.isCONSTANT_OK())
						{
							goto IL_01D0;
						}
						if (!this.canConvert(pArgument, ctype2 = this.GetPredefindType(unaOpSig.pt)))
						{
							if ((long)i < num)
							{
								goto IL_01D0;
							}
							ctype2 = this.GetSymbolLoader().GetTypeManager().GetNullable(ctype2);
							if (!this.canConvert(pArgument, ctype2))
							{
								goto IL_01D0;
							}
						}
						break;
					case ConvKind.Unknown:
						if (!this.canConvert(pArgument, ctype2 = this.GetPredefindType(unaOpSig.pt)))
						{
							if ((long)i < num)
							{
								goto IL_01D0;
							}
							ctype2 = this.GetSymbolLoader().GetTypeManager().GetNullable(ctype2);
							if (!this.canConvert(pArgument, ctype2))
							{
								goto IL_01D0;
							}
						}
						break;
					case ConvKind.None:
						goto IL_01D0;
					default:
						goto IL_01D0;
					}
					if (ctype2 is NullableType)
					{
						LiftFlags liftFlags = LiftFlags.None;
						ConvKind convKind2 = ExpressionBinder.GetConvKind(predefinedType2, unaOpSig.pt);
						if (convKind2 - ConvKind.Identity > 1)
						{
							liftFlags |= LiftFlags.Convert1;
						}
						else
						{
							liftFlags |= LiftFlags.Lift1;
						}
						pSignatures.Add(new ExpressionBinder.UnaOpFullSig(ctype2, unaOpSig.pfn, liftFlags, unaOpSig.fnkind));
						num = (long)(i + unaOpSig.cuosSkip + 1);
					}
					else
					{
						ExpressionBinder.UnaOpFullSig unaOpFullSig2 = new ExpressionBinder.UnaOpFullSig(this, unaOpSig);
						if (unaOpFullSig2.GetType() != null)
						{
							pSignatures.Add(unaOpFullSig2);
						}
						i += unaOpSig.cuosSkip;
					}
				}
				IL_01D0:;
			}
			return false;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000F6F4 File Offset: 0x0000D8F4
		private ExprOperator BindLiftedStandardUnop(ExpressionKind ek, EXPRFLAG flags, Expr arg, ExpressionBinder.UnaOpFullSig uofs)
		{
			NullableType nullableType = uofs.GetType() as NullableType;
			if (arg.Type is NullType)
			{
				return this.BadOperatorTypesError(ek, arg, null, nullableType);
			}
			Expr expr = null;
			Expr expr2 = null;
			this.LiftArgument(arg, uofs.GetType(), uofs.Convert(), out expr, out expr2);
			Expr expr3 = uofs.pfn(ek, flags, expr2);
			ExprUnaryOp exprUnaryOp = this.GetExprFactory().CreateUnaryOp(ek, nullableType, expr);
			this.mustCast(expr3, nullableType, (CONVERTTYPE)0);
			exprUnaryOp.Flags |= flags;
			return exprUnaryOp;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000F77C File Offset: 0x0000D97C
		private int WhichUofsIsBetter(ExpressionBinder.UnaOpFullSig uofs1, ExpressionBinder.UnaOpFullSig uofs2, CType typeArg)
		{
			BetterType betterType;
			if (uofs1.FPreDef() && uofs2.FPreDef())
			{
				betterType = this.WhichTypeIsBetter(uofs1.pt, uofs2.pt, typeArg);
			}
			else
			{
				betterType = this.WhichTypeIsBetter(uofs1.GetType(), uofs2.GetType(), typeArg);
			}
			if (betterType == BetterType.Left)
			{
				return -1;
			}
			if (betterType != BetterType.Right)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000F7D2 File Offset: 0x0000D9D2
		private ExprOperator BindIntBinOp(ExpressionKind ek, EXPRFLAG flags, Expr arg1, Expr arg2)
		{
			return this.BindIntOp(ek, flags, arg1, arg2, arg1.Type.getPredefType());
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000F7EA File Offset: 0x0000D9EA
		private ExprOperator BindIntUnaOp(ExpressionKind ek, EXPRFLAG flags, Expr arg)
		{
			return this.BindIntOp(ek, flags, arg, null, arg.Type.getPredefType());
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000F801 File Offset: 0x0000DA01
		private ExprOperator BindRealBinOp(ExpressionKind ek, EXPRFLAG flags, Expr arg1, Expr arg2)
		{
			return this.bindFloatOp(ek, flags, arg1, arg2);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000F80E File Offset: 0x0000DA0E
		private ExprOperator BindRealUnaOp(ExpressionKind ek, EXPRFLAG flags, Expr arg)
		{
			return this.bindFloatOp(ek, flags, arg, null);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000F81C File Offset: 0x0000DA1C
		private Expr BindIncOp(ExpressionKind ek, EXPRFLAG flags, Expr arg, ExpressionBinder.UnaOpFullSig uofs)
		{
			if (!this.checkLvalue(arg, CheckLvalueKind.Increment))
			{
				ExprBinOp exprBinOp = this.GetExprFactory().CreateBinop(ek, arg.Type, arg, null);
				exprBinOp.SetError();
				return exprBinOp;
			}
			FUNDTYPE fundtype = uofs.GetType().StripNubs().fundType();
			if (fundtype == FUNDTYPE.FT_R8 || fundtype == FUNDTYPE.FT_R4)
			{
				flags &= ~EXPRFLAG.EXF_CHECKOVERFLOW;
			}
			if (uofs.isLifted())
			{
				return this.BindLiftedIncOp(ek, flags, arg, uofs);
			}
			return this.BindNonliftedIncOp(ek, flags, arg, uofs);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000F894 File Offset: 0x0000DA94
		private Expr BindIncOpCore(ExpressionKind ek, EXPRFLAG flags, Expr exprVal, CType type)
		{
			Expr expr = null;
			if (type.isEnumType() && type.fundType() > FUNDTYPE.FT_U8)
			{
				type = this.GetPredefindType(PredefinedType.PT_INT);
			}
			FUNDTYPE fundtype = type.fundType();
			CType ctype = type;
			switch (fundtype)
			{
			case FUNDTYPE.FT_I1:
			case FUNDTYPE.FT_I2:
			case FUNDTYPE.FT_U1:
			case FUNDTYPE.FT_U2:
			{
				ctype = this.GetPredefindType(PredefinedType.PT_INT);
				ConstVal constVal = ConstVal.Get(1);
				expr = this.LScalar(ek, flags, exprVal, type, constVal, expr, ctype);
				break;
			}
			case FUNDTYPE.FT_I4:
			case FUNDTYPE.FT_U4:
			{
				ConstVal constVal = ConstVal.Get(1);
				expr = this.LScalar(ek, flags, exprVal, type, constVal, expr, ctype);
				break;
			}
			case FUNDTYPE.FT_I8:
			case FUNDTYPE.FT_U8:
			{
				ConstVal constVal = ConstVal.Get(1L);
				expr = this.LScalar(ek, flags, exprVal, type, constVal, expr, ctype);
				break;
			}
			case FUNDTYPE.FT_R4:
			case FUNDTYPE.FT_R8:
			{
				ConstVal constVal = ConstVal.Get(1.0);
				expr = this.LScalar(ek, flags, exprVal, type, constVal, expr, ctype);
				break;
			}
			default:
			{
				ek = ((ek == ExpressionKind.Add) ? ExpressionKind.DecimalInc : ExpressionKind.DecimalDec);
				PREDEFMETH predefmeth = ((ek == ExpressionKind.DecimalInc) ? PREDEFMETH.PM_DECIMAL_OPINCREMENT : PREDEFMETH.PM_DECIMAL_OPDECREMENT);
				expr = this.CreateUnaryOpForPredefMethodCall(ek, predefmeth, type, exprVal);
				break;
			}
			case FUNDTYPE.FT_PTR:
			{
				ConstVal constVal = ConstVal.Get(1);
				expr = this.BindPtrBinOp(ek, flags, exprVal, this.GetExprFactory().CreateConstant(this.GetPredefindType(PredefinedType.PT_INT), constVal));
				break;
			}
			}
			return expr;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000F9C8 File Offset: 0x0000DBC8
		private Expr LScalar(ExpressionKind ek, EXPRFLAG flags, Expr exprVal, CType type, ConstVal cv, Expr pExprResult, CType typeTmp)
		{
			CType ctype = type;
			if (ctype.isEnumType())
			{
				ctype = ctype.underlyingEnumType();
			}
			pExprResult = this.GetExprFactory().CreateBinop(ek, typeTmp, exprVal, this.GetExprFactory().CreateConstant(ctype, cv));
			pExprResult.Flags |= flags;
			if (typeTmp != type)
			{
				pExprResult = this.mustCast(pExprResult, type, CONVERTTYPE.NOUDC);
			}
			return pExprResult;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000FA2C File Offset: 0x0000DC2C
		private ExprMulti BindNonliftedIncOp(ExpressionKind ek, EXPRFLAG flags, Expr arg, ExpressionBinder.UnaOpFullSig uofs)
		{
			Expr expr;
			ExprMultiGet exprMultiGet = (expr = this.GetExprFactory().CreateMultiGet(EXPRFLAG.EXF_ASSGOP, arg.Type, null));
			CType type = uofs.GetType();
			expr = this.mustCast(expr, type);
			expr = this.BindIncOpCore(ek, flags, expr, type);
			Expr expr2 = this.mustCast(expr, arg.Type, CONVERTTYPE.NOUDC);
			ExprMulti exprMulti = this.GetExprFactory().CreateMulti(EXPRFLAG.EXF_ASSGOP | flags, arg.Type, arg, expr2);
			exprMultiGet.OptionalMulti = exprMulti;
			return exprMulti;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000FAA0 File Offset: 0x0000DCA0
		private ExprMulti BindLiftedIncOp(ExpressionKind ek, EXPRFLAG flags, Expr arg, ExpressionBinder.UnaOpFullSig uofs)
		{
			NullableType nullableType = uofs.GetType() as NullableType;
			Expr expr;
			ExprMultiGet exprMultiGet = (expr = this.GetExprFactory().CreateMultiGet(EXPRFLAG.EXF_ASSGOP, arg.Type, null));
			Expr expr2 = expr;
			expr2 = this.mustCast(expr2, nullableType.GetUnderlyingType());
			Expr expr3 = this.BindIncOpCore(ek, flags, expr2, nullableType.GetUnderlyingType());
			expr = this.mustCast(expr, nullableType);
			ExprUnaryOp exprUnaryOp = this.GetExprFactory().CreateUnaryOp((ek == ExpressionKind.Add) ? ExpressionKind.Inc : ExpressionKind.Dec, arg.Type, expr);
			this.mustCast(this.mustCast(expr3, nullableType), arg.Type);
			exprUnaryOp.Flags |= flags;
			ExprMulti exprMulti = this.GetExprFactory().CreateMulti(EXPRFLAG.EXF_ASSGOP | flags, arg.Type, arg, exprUnaryOp);
			exprMultiGet.OptionalMulti = exprMulti;
			return exprMulti;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000FB68 File Offset: 0x0000DD68
		private ExprBinOp BindDecBinOp(ExpressionKind ek, EXPRFLAG flags, Expr arg1, Expr arg2)
		{
			CType predefindType = this.GetPredefindType(PredefinedType.PT_DECIMAL);
			CType ctype;
			if (ek - ExpressionKind.Eq > 5)
			{
				if (ek - ExpressionKind.Add > 4)
				{
					ctype = null;
				}
				else
				{
					ctype = predefindType;
				}
			}
			else
			{
				ctype = this.GetPredefindType(PredefinedType.PT_BOOL);
			}
			return this.GetExprFactory().CreateBinop(ek, ctype, arg1, arg2);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000FBAC File Offset: 0x0000DDAC
		private ExprUnaryOp BindDecUnaOp(ExpressionKind ek, EXPRFLAG flags, Expr arg)
		{
			CType predefindType = this.GetPredefindType(PredefinedType.PT_DECIMAL);
			if (ek == ExpressionKind.Negate)
			{
				PREDEFMETH predefmeth = PREDEFMETH.PM_DECIMAL_OPUNARYMINUS;
				return this.CreateUnaryOpForPredefMethodCall(ExpressionKind.DecimalNegate, predefmeth, predefindType, arg);
			}
			return this.GetExprFactory().CreateUnaryOp(ExpressionKind.UnaryPlus, predefindType, arg);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000FBE3 File Offset: 0x0000DDE3
		private Expr BindStrBinOp(ExpressionKind ek, EXPRFLAG flags, Expr arg1, Expr arg2)
		{
			return this.bindStringConcat(arg1, arg2);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000FBEE File Offset: 0x0000DDEE
		private ExprBinOp BindShiftOp(ExpressionKind ek, EXPRFLAG flags, Expr arg1, Expr arg2)
		{
			arg1.Type.getPredefType();
			return this.GetExprFactory().CreateBinop(ek, arg1.Type, arg1, arg2);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000FC11 File Offset: 0x0000DE11
		private ExprBinOp BindBoolBinOp(ExpressionKind ek, EXPRFLAG flags, Expr arg1, Expr arg2)
		{
			return this.GetExprFactory().CreateBinop(ek, this.GetPredefindType(PredefinedType.PT_BOOL), arg1, arg2);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000FC2C File Offset: 0x0000DE2C
		private ExprOperator BindBoolBitwiseOp(ExpressionKind ek, EXPRFLAG flags, Expr expr1, Expr expr2, ExpressionBinder.BinOpFullSig bofs)
		{
			if (expr1.Type is NullableType || expr2.Type is NullableType)
			{
				CType predefindType = this.GetPredefindType(PredefinedType.PT_BOOL);
				CType nullable = this.GetSymbolLoader().GetTypeManager().GetNullable(predefindType);
				Expr expr3 = CNullable.StripNullableConstructor(expr1);
				Expr expr4 = CNullable.StripNullableConstructor(expr2);
				Expr expr5 = null;
				if (!(expr3.Type is NullableType) && !(expr4.Type is NullableType))
				{
					expr5 = this.BindBoolBinOp(ek, flags, expr3, expr4);
				}
				ExprBinOp exprBinOp = this.GetExprFactory().CreateBinop(ek, nullable, expr1, expr2);
				if (expr5 != null)
				{
					this.mustCast(expr5, nullable, (CONVERTTYPE)0);
				}
				exprBinOp.IsLifted = true;
				exprBinOp.Flags |= flags;
				return exprBinOp;
			}
			return this.BindBoolBinOp(ek, flags, expr1, expr2);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000FCE9 File Offset: 0x0000DEE9
		private Expr BindLiftedBoolBitwiseOp(ExpressionKind ek, EXPRFLAG flags, Expr expr1, Expr expr2)
		{
			return null;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000FCEC File Offset: 0x0000DEEC
		private Expr BindBoolUnaOp(ExpressionKind ek, EXPRFLAG flags, Expr arg)
		{
			CType predefindType = this.GetPredefindType(PredefinedType.PT_BOOL);
			Expr @const = arg.GetConst();
			if (@const == null)
			{
				return this.GetExprFactory().CreateUnaryOp(ExpressionKind.LogicalNot, predefindType, arg);
			}
			return this.GetExprFactory().CreateConstant(predefindType, ConstVal.Get(((ExprConstant)@const).Val.Int32Val == 0));
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000FD44 File Offset: 0x0000DF44
		private ExprBinOp BindStrCmpOp(ExpressionKind ek, EXPRFLAG flags, Expr arg1, Expr arg2)
		{
			PREDEFMETH predefmeth = ((ek == ExpressionKind.Eq) ? PREDEFMETH.PM_STRING_OPEQUALITY : PREDEFMETH.PM_STRING_OPINEQUALITY);
			ek = ((ek == ExpressionKind.Eq) ? ExpressionKind.StringEq : ExpressionKind.StringNotEq);
			return this.CreateBinopForPredefMethodCall(ek, predefmeth, this.GetPredefindType(PredefinedType.PT_BOOL), arg1, arg2);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000FD7C File Offset: 0x0000DF7C
		private ExprBinOp BindRefCmpOp(ExpressionKind ek, EXPRFLAG flags, Expr arg1, Expr arg2)
		{
			arg1 = this.mustConvert(arg1, this.GetPredefindType(PredefinedType.PT_OBJECT), CONVERTTYPE.NOUDC);
			arg2 = this.mustConvert(arg2, this.GetPredefindType(PredefinedType.PT_OBJECT), CONVERTTYPE.NOUDC);
			return this.GetExprFactory().CreateBinop(ek, this.GetPredefindType(PredefinedType.PT_BOOL), arg1, arg2);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000FDBC File Offset: 0x0000DFBC
		private Expr BindDelBinOp(ExpressionKind ek, EXPRFLAG flags, Expr arg1, Expr arg2)
		{
			PREDEFMETH predefmeth = PREDEFMETH.PM_DECIMAL_OPDECREMENT;
			CType ctype = null;
			if (ek <= ExpressionKind.NotEq)
			{
				if (ek != ExpressionKind.Eq)
				{
					if (ek == ExpressionKind.NotEq)
					{
						predefmeth = PREDEFMETH.PM_DELEGATE_OPINEQUALITY;
						ctype = this.GetPredefindType(PredefinedType.PT_BOOL);
						ek = ExpressionKind.DelegateNotEq;
					}
				}
				else
				{
					predefmeth = PREDEFMETH.PM_DELEGATE_OPEQUALITY;
					ctype = this.GetPredefindType(PredefinedType.PT_BOOL);
					ek = ExpressionKind.DelegateEq;
				}
			}
			else if (ek != ExpressionKind.Add)
			{
				if (ek == ExpressionKind.Subtract)
				{
					predefmeth = PREDEFMETH.PM_DELEGATE_REMOVE;
					ctype = arg1.Type;
					ek = ExpressionKind.DelegateSubtract;
				}
			}
			else
			{
				predefmeth = PREDEFMETH.PM_DELEGATE_COMBINE;
				ctype = arg1.Type;
				ek = ExpressionKind.DelegateAdd;
			}
			return this.CreateBinopForPredefMethodCall(ek, predefmeth, ctype, arg1, arg2);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000FE34 File Offset: 0x0000E034
		private Expr BindEnumBinOp(ExpressionKind ek, EXPRFLAG flags, Expr arg1, Expr arg2)
		{
			AggregateType aggregateType = null;
			AggregateType enumBinOpType = this.GetEnumBinOpType(ek, arg1.Type, arg2.Type, out aggregateType);
			PredefinedType predefinedType;
			switch (aggregateType.fundType())
			{
			case FUNDTYPE.FT_U4:
				predefinedType = PredefinedType.PT_UINT;
				break;
			case FUNDTYPE.FT_I8:
				predefinedType = PredefinedType.PT_LONG;
				break;
			case FUNDTYPE.FT_U8:
				predefinedType = PredefinedType.PT_ULONG;
				break;
			default:
				predefinedType = PredefinedType.PT_INT;
				break;
			}
			CType predefindType = this.GetPredefindType(predefinedType);
			arg1 = this.mustCast(arg1, predefindType, CONVERTTYPE.NOUDC);
			arg2 = this.mustCast(arg2, predefindType, CONVERTTYPE.NOUDC);
			Expr expr = this.BindIntOp(ek, flags, arg1, arg2, predefinedType);
			if (!expr.IsOK)
			{
				return expr;
			}
			if (expr.Type != enumBinOpType)
			{
				expr = this.mustCast(expr, enumBinOpType, CONVERTTYPE.NOUDC);
			}
			return expr;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000FED8 File Offset: 0x0000E0D8
		private Expr BindLiftedEnumArithmeticBinOp(ExpressionKind ek, EXPRFLAG flags, Expr arg1, Expr arg2)
		{
			NullableType nullableType;
			CType ctype = (((nullableType = arg1.Type as NullableType) != null) ? nullableType.UnderlyingType : arg1.Type);
			NullableType nullableType2;
			CType ctype2 = (((nullableType2 = arg2.Type as NullableType) != null) ? nullableType2.UnderlyingType : arg2.Type);
			if (ctype is NullType)
			{
				ctype = ctype2.underlyingEnumType();
			}
			else if (ctype2 is NullType)
			{
				ctype2 = ctype.underlyingEnumType();
			}
			AggregateType aggregateType;
			NullableType nullable = this.GetTypes().GetNullable(this.GetEnumBinOpType(ek, ctype, ctype2, out aggregateType));
			PredefinedType predefinedType;
			switch (aggregateType.fundType())
			{
			case FUNDTYPE.FT_U4:
				predefinedType = PredefinedType.PT_UINT;
				break;
			case FUNDTYPE.FT_I8:
				predefinedType = PredefinedType.PT_LONG;
				break;
			case FUNDTYPE.FT_U8:
				predefinedType = PredefinedType.PT_ULONG;
				break;
			default:
				predefinedType = PredefinedType.PT_INT;
				break;
			}
			NullableType nullable2 = this.GetTypes().GetNullable(this.GetPredefindType(predefinedType));
			arg1 = this.mustCast(arg1, nullable2, CONVERTTYPE.NOUDC);
			arg2 = this.mustCast(arg2, nullable2, CONVERTTYPE.NOUDC);
			ExprBinOp exprBinOp = this.GetExprFactory().CreateBinop(ek, nullable2, arg1, arg2);
			exprBinOp.IsLifted = true;
			exprBinOp.Flags |= flags;
			if (!exprBinOp.IsOK)
			{
				return exprBinOp;
			}
			if (exprBinOp.Type != nullable)
			{
				return this.mustCast(exprBinOp, nullable, CONVERTTYPE.NOUDC);
			}
			return exprBinOp;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0001000C File Offset: 0x0000E20C
		private Expr BindEnumUnaOp(ExpressionKind ek, EXPRFLAG flags, Expr arg)
		{
			CType type = ((ExprCast)arg).Argument.Type;
			PredefinedType predefinedType;
			switch (type.fundType())
			{
			case FUNDTYPE.FT_U4:
				predefinedType = PredefinedType.PT_UINT;
				break;
			case FUNDTYPE.FT_I8:
				predefinedType = PredefinedType.PT_LONG;
				break;
			case FUNDTYPE.FT_U8:
				predefinedType = PredefinedType.PT_ULONG;
				break;
			default:
				predefinedType = PredefinedType.PT_INT;
				break;
			}
			CType predefindType = this.GetPredefindType(predefinedType);
			arg = this.mustCast(arg, predefindType, CONVERTTYPE.NOUDC);
			Expr expr = this.BindIntOp(ek, flags, arg, null, predefinedType);
			if (!expr.IsOK)
			{
				return expr;
			}
			return this.mustCastInUncheckedContext(expr, type, CONVERTTYPE.NOUDC);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00010089 File Offset: 0x0000E289
		private Expr BindPtrBinOp(ExpressionKind ek, EXPRFLAG flags, Expr arg1, Expr arg2)
		{
			return null;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0001008C File Offset: 0x0000E28C
		private Expr BindPtrCmpOp(ExpressionKind ek, EXPRFLAG flags, Expr arg1, Expr arg2)
		{
			return null;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00010090 File Offset: 0x0000E290
		private bool GetBinopKindAndFlags(ExpressionKind ek, out BinOpKind pBinopKind, out EXPRFLAG flags)
		{
			flags = (EXPRFLAG)0;
			switch (ek)
			{
			case ExpressionKind.Eq:
			case ExpressionKind.NotEq:
				pBinopKind = BinOpKind.Equal;
				return true;
			case ExpressionKind.LessThan:
			case ExpressionKind.LessThanOrEqual:
			case ExpressionKind.GreaterThan:
			case ExpressionKind.GreaterThanOrEqual:
				pBinopKind = BinOpKind.Compare;
				return true;
			case ExpressionKind.Add:
				if (this.Context.CheckedNormal)
				{
					flags |= EXPRFLAG.EXF_CHECKOVERFLOW;
				}
				pBinopKind = BinOpKind.Add;
				return true;
			case ExpressionKind.Subtract:
				if (this.Context.CheckedNormal)
				{
					flags |= EXPRFLAG.EXF_CHECKOVERFLOW;
				}
				pBinopKind = BinOpKind.Sub;
				return true;
			case ExpressionKind.Multiply:
				if (this.Context.CheckedNormal)
				{
					flags |= EXPRFLAG.EXF_CHECKOVERFLOW;
				}
				pBinopKind = BinOpKind.Mul;
				return true;
			case ExpressionKind.Divide:
			case ExpressionKind.Modulo:
				flags |= EXPRFLAG.EXF_ASSGOP;
				if (this.Context.CheckedNormal)
				{
					flags |= EXPRFLAG.EXF_CHECKOVERFLOW;
				}
				pBinopKind = BinOpKind.Mul;
				return true;
			case ExpressionKind.BitwiseAnd:
			case ExpressionKind.BitwiseOr:
				pBinopKind = BinOpKind.Bitwise;
				return true;
			case ExpressionKind.BitwiseExclusiveOr:
				pBinopKind = BinOpKind.BitXor;
				return true;
			case ExpressionKind.LeftShirt:
			case ExpressionKind.RightShift:
				pBinopKind = BinOpKind.Shift;
				return true;
			case ExpressionKind.LogicalAnd:
			case ExpressionKind.LogicalOr:
				pBinopKind = BinOpKind.Logical;
				return true;
			}
			pBinopKind = BinOpKind.Add;
			return false;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x000101A4 File Offset: 0x0000E3A4
		private ExprOperator BindIntOp(ExpressionKind kind, EXPRFLAG flags, Expr op1, Expr op2, PredefinedType ptOp)
		{
			CType predefindType = this.GetPredefindType(ptOp);
			if (kind == ExpressionKind.Negate)
			{
				return this.BindIntegerNeg(flags, op1, ptOp);
			}
			CType ctype = (kind.IsRelational() ? this.GetPredefindType(PredefinedType.PT_BOOL) : predefindType);
			ExprOperator exprOperator = this.GetExprFactory().CreateOperator(kind, ctype, op1, op2);
			exprOperator.Flags |= flags;
			return exprOperator;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000101FC File Offset: 0x0000E3FC
		private ExprOperator BindIntegerNeg(EXPRFLAG flags, Expr op, PredefinedType ptOp)
		{
			this.GetPredefindType(ptOp);
			if (ptOp == PredefinedType.PT_ULONG)
			{
				return this.BadOperatorTypesError(ExpressionKind.Negate, op, null);
			}
			if (ptOp == PredefinedType.PT_UINT && op.Type.fundType() == FUNDTYPE.FT_U4)
			{
				ExprClass exprClass = this.GetExprFactory().CreateClass(this.GetPredefindType(PredefinedType.PT_LONG));
				op = this.mustConvertCore(op, exprClass, CONVERTTYPE.NOUDC);
			}
			return this.GetExprFactory().CreateNeg(flags, op);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00010260 File Offset: 0x0000E460
		private ExprOperator bindFloatOp(ExpressionKind kind, EXPRFLAG flags, Expr op1, Expr op2)
		{
			CType ctype = (kind.IsRelational() ? this.GetPredefindType(PredefinedType.PT_BOOL) : op1.Type);
			ExprOperator exprOperator = this.GetExprFactory().CreateOperator(kind, ctype, op1, op2);
			flags = ~EXPRFLAG.EXF_CHECKOVERFLOW;
			exprOperator.Flags &= flags;
			return exprOperator;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000102AA File Offset: 0x0000E4AA
		private ExprConcat bindStringConcat(Expr op1, Expr op2)
		{
			return this.GetExprFactory().CreateConcat(op1, op2);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x000102BC File Offset: 0x0000E4BC
		private RuntimeBinderException AmbiguousOperatorError(ExpressionKind ek, Expr op1, Expr op2)
		{
			string errorString = op1.ErrorString;
			if (op2 == null)
			{
				return this.GetErrorContext().Error(ErrorCode.ERR_AmbigUnaryOp, new ErrArg[] { errorString, op1.Type });
			}
			return this.GetErrorContext().Error(ErrorCode.ERR_AmbigBinaryOps, new ErrArg[] { errorString, op1.Type, op2.Type });
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00010338 File Offset: 0x0000E538
		private Expr BindUserBoolOp(ExpressionKind kind, ExprCall pCall)
		{
			CType type = pCall.Type;
			if (!this.GetTypes().SubstEqualTypes(type, pCall.MethWithInst.Meth().Params[0], type) || !this.GetTypes().SubstEqualTypes(type, pCall.MethWithInst.Meth().Params[1], type))
			{
				throw this.GetErrorContext().Error(ErrorCode.ERR_BadBoolOp, new ErrArg[] { pCall.MethWithInst });
			}
			ExprList exprList = (ExprList)pCall.OptionalArguments;
			Expr optionalElement = exprList.OptionalElement;
			ExprWrap exprWrap = this.WrapShortLivedExpression(optionalElement);
			exprList.OptionalElement = exprWrap;
			this.SymbolLoader.RuntimeBinderSymbolTable.PopulateSymbolTableWithName("op_True", null, exprWrap.Type.AssociatedSystemType);
			this.SymbolLoader.RuntimeBinderSymbolTable.PopulateSymbolTableWithName("op_False", null, exprWrap.Type.AssociatedSystemType);
			Expr expr = this.bindUDUnop(ExpressionKind.True, exprWrap);
			Expr expr2 = this.bindUDUnop(ExpressionKind.False, exprWrap);
			if (expr == null || expr2 == null)
			{
				throw this.GetErrorContext().Error(ErrorCode.ERR_MustHaveOpTF, new ErrArg[] { type });
			}
			expr = this.mustConvert(expr, this.GetPredefindType(PredefinedType.PT_BOOL));
			expr2 = this.mustConvert(expr2, this.GetPredefindType(PredefinedType.PT_BOOL));
			return this.GetExprFactory().CreateUserLogOp(type, (kind == ExpressionKind.LogicalAnd) ? expr2 : expr, pCall);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00010490 File Offset: 0x0000E690
		private AggregateType GetUserDefinedBinopArgumentType(CType type)
		{
			for (;;)
			{
				TypeKind typeKind = type.GetTypeKind();
				if (typeKind == TypeKind.TK_AggregateType)
				{
					break;
				}
				if (typeKind != TypeKind.TK_NullableType)
				{
					goto IL_0041;
				}
				type = type.StripNubs();
			}
			AggregateType aggregateType = (AggregateType)type;
			if ((aggregateType.isClassType() || aggregateType.isStructType()) && !aggregateType.getAggregate().IsSkipUDOps())
			{
				return aggregateType;
			}
			return null;
			IL_0041:
			return null;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000104E0 File Offset: 0x0000E6E0
		private int GetUserDefinedBinopArgumentTypes(CType type1, CType type2, AggregateType[] rgats)
		{
			int num = 0;
			rgats[0] = this.GetUserDefinedBinopArgumentType(type1);
			if (rgats[0] != null)
			{
				num++;
			}
			rgats[num] = this.GetUserDefinedBinopArgumentType(type2);
			if (rgats[num] != null)
			{
				num++;
			}
			if (num == 2 && rgats[0] == rgats[1])
			{
				num = 1;
			}
			return num;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00010524 File Offset: 0x0000E724
		private bool UserDefinedBinaryOperatorCanBeLifted(ExpressionKind ek, MethodSymbol method, AggregateType ats, TypeArray Params)
		{
			if (!Params[0].IsNonNubValType())
			{
				return false;
			}
			if (!Params[1].IsNonNubValType())
			{
				return false;
			}
			CType ctype = this.GetTypes().SubstType(method.RetType, ats);
			if (!ctype.IsNonNubValType())
			{
				return false;
			}
			if (ek - ExpressionKind.Eq > 1)
			{
				return ek - ExpressionKind.LessThan > 3 || ctype.isPredefType(PredefinedType.PT_BOOL);
			}
			return ctype.isPredefType(PredefinedType.PT_BOOL) && Params[0] == Params[1];
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000105B0 File Offset: 0x0000E7B0
		private bool UserDefinedBinaryOperatorIsApplicable(List<CandidateFunctionMember> candidateList, ExpressionKind ek, MethodSymbol method, AggregateType ats, Expr arg1, Expr arg2, bool fDontLift)
		{
			if (!method.isOperator || method.Params.Count != 2)
			{
				return false;
			}
			TypeArray typeArray = this.GetTypes().SubstTypeArray(method.Params, ats);
			if (this.canConvert(arg1, typeArray[0]) && this.canConvert(arg2, typeArray[1]))
			{
				candidateList.Add(new CandidateFunctionMember(new MethPropWithInst(method, ats, BSYMMGR.EmptyTypeArray()), typeArray, 0, false));
				return true;
			}
			if (fDontLift || !this.UserDefinedBinaryOperatorCanBeLifted(ek, method, ats, typeArray))
			{
				return false;
			}
			CType[] array = new CType[]
			{
				this.GetTypes().GetNullable(typeArray[0]),
				this.GetTypes().GetNullable(typeArray[1])
			};
			if (!this.canConvert(arg1, array[0]) || !this.canConvert(arg2, array[1]))
			{
				return false;
			}
			candidateList.Add(new CandidateFunctionMember(new MethPropWithInst(method, ats, BSYMMGR.EmptyTypeArray()), this.GetGlobalSymbols().AllocParams(2, array), 2, false));
			return true;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000106B0 File Offset: 0x0000E8B0
		private bool GetApplicableUserDefinedBinaryOperatorCandidates(List<CandidateFunctionMember> candidateList, ExpressionKind ek, AggregateType type, Expr arg1, Expr arg2, bool fDontLift)
		{
			Name name = this.ekName(ek);
			bool flag = false;
			for (MethodSymbol methodSymbol = this.GetSymbolLoader().LookupAggMember(name, type.getAggregate(), symbmask_t.MASK_MethodSymbol) as MethodSymbol; methodSymbol != null; methodSymbol = this.GetSymbolLoader().LookupNextSym(methodSymbol, type.getAggregate(), symbmask_t.MASK_MethodSymbol) as MethodSymbol)
			{
				if (this.UserDefinedBinaryOperatorIsApplicable(candidateList, ek, methodSymbol, type, arg1, arg2, fDontLift))
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00010720 File Offset: 0x0000E920
		private AggregateType GetApplicableUserDefinedBinaryOperatorCandidatesInBaseTypes(List<CandidateFunctionMember> candidateList, ExpressionKind ek, AggregateType type, Expr arg1, Expr arg2, bool fDontLift, AggregateType atsStop)
		{
			AggregateType aggregateType = type;
			while (aggregateType != null && aggregateType != atsStop)
			{
				if (this.GetApplicableUserDefinedBinaryOperatorCandidates(candidateList, ek, aggregateType, arg1, arg2, fDontLift))
				{
					return aggregateType;
				}
				aggregateType = aggregateType.GetBaseClass();
			}
			return null;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00010754 File Offset: 0x0000E954
		private ExprCall BindUDBinop(ExpressionKind ek, Expr arg1, Expr arg2, bool fDontLift, out MethPropWithInst ppmpwi)
		{
			List<CandidateFunctionMember> list = new List<CandidateFunctionMember>();
			ppmpwi = null;
			AggregateType[] array = new AggregateType[2];
			int userDefinedBinopArgumentTypes = this.GetUserDefinedBinopArgumentTypes(arg1.Type, arg2.Type, array);
			if (userDefinedBinopArgumentTypes == 0)
			{
				return null;
			}
			if (userDefinedBinopArgumentTypes == 1)
			{
				this.GetApplicableUserDefinedBinaryOperatorCandidatesInBaseTypes(list, ek, array[0], arg1, arg2, fDontLift, null);
			}
			else
			{
				AggregateType applicableUserDefinedBinaryOperatorCandidatesInBaseTypes = this.GetApplicableUserDefinedBinaryOperatorCandidatesInBaseTypes(list, ek, array[0], arg1, arg2, fDontLift, null);
				this.GetApplicableUserDefinedBinaryOperatorCandidatesInBaseTypes(list, ek, array[1], arg1, arg2, fDontLift, applicableUserDefinedBinaryOperatorCandidatesInBaseTypes);
			}
			if (list.IsEmpty<CandidateFunctionMember>())
			{
				return null;
			}
			ExprList exprList = this.GetExprFactory().CreateList(arg1, arg2);
			ArgInfos argInfos = new ArgInfos();
			argInfos.carg = 2;
			this.FillInArgInfoFromArgList(argInfos, exprList);
			CandidateFunctionMember candidateFunctionMember2;
			CandidateFunctionMember candidateFunctionMember3;
			CandidateFunctionMember candidateFunctionMember = this.FindBestMethod(list, null, argInfos, out candidateFunctionMember2, out candidateFunctionMember3);
			if (candidateFunctionMember == null)
			{
				throw this.GetErrorContext().Error(ErrorCode.ERR_AmbigCall, new ErrArg[] { candidateFunctionMember2.mpwi, candidateFunctionMember3.mpwi });
			}
			ppmpwi = candidateFunctionMember.mpwi;
			if (candidateFunctionMember.ctypeLift != 0)
			{
				return this.BindLiftedUDBinop(ek, arg1, arg2, candidateFunctionMember.@params, candidateFunctionMember.mpwi);
			}
			CType ctype = this.GetTypes().SubstType(candidateFunctionMember.mpwi.Meth().RetType, candidateFunctionMember.mpwi.GetType());
			return this.BindUDBinopCall(arg1, arg2, candidateFunctionMember.@params, ctype, candidateFunctionMember.mpwi);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000108AC File Offset: 0x0000EAAC
		private ExprCall BindUDBinopCall(Expr arg1, Expr arg2, TypeArray Params, CType typeRet, MethPropWithInst mpwi)
		{
			arg1 = this.mustConvert(arg1, Params[0]);
			arg2 = this.mustConvert(arg2, Params[1]);
			ExprList exprList = this.GetExprFactory().CreateList(arg1, arg2);
			this.checkUnsafe(arg1.Type);
			this.checkUnsafe(arg2.Type);
			this.checkUnsafe(typeRet);
			ExprMemberGroup exprMemberGroup = this.GetExprFactory().CreateMemGroup(null, mpwi);
			ExprCall exprCall = this.GetExprFactory().CreateCall((EXPRFLAG)0, typeRet, exprList, exprMemberGroup, null);
			exprCall.MethWithInst = new MethWithInst(mpwi);
			this.verifyMethodArgs(exprCall, mpwi.GetType());
			return exprCall;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00010944 File Offset: 0x0000EB44
		private ExprCall BindLiftedUDBinop(ExpressionKind ek, Expr arg1, Expr arg2, TypeArray Params, MethPropWithInst mpwi)
		{
			Expr expr = arg1;
			Expr expr2 = arg2;
			CType ctype = this.GetTypes().SubstType(mpwi.Meth().RetType, mpwi.GetType());
			TypeArray typeArray = this.GetTypes().SubstTypeArray(mpwi.Meth().Params, mpwi.GetType());
			if (!this.canConvert(arg1.Type.StripNubs(), typeArray[0], CONVERTTYPE.NOUDC))
			{
				expr = this.mustConvert(arg1, Params[0]);
			}
			if (!this.canConvert(arg2.Type.StripNubs(), typeArray[1], CONVERTTYPE.NOUDC))
			{
				expr2 = this.mustConvert(arg2, Params[1]);
			}
			Expr expr3 = this.mustCast(expr, typeArray[0]);
			Expr expr4 = this.mustCast(expr2, typeArray[1]);
			CType ctype2;
			if (ek - ExpressionKind.Eq > 1)
			{
				if (ek - ExpressionKind.LessThan > 3)
				{
					ctype2 = this.GetTypes().GetNullable(ctype);
				}
				else
				{
					ctype2 = ctype;
				}
			}
			else
			{
				ctype2 = ctype;
			}
			ExprCall exprCall = this.BindUDBinopCall(expr3, expr4, typeArray, ctype, mpwi);
			ExprList exprList = this.GetExprFactory().CreateList(expr, expr2);
			ExprMemberGroup exprMemberGroup = this.GetExprFactory().CreateMemGroup(null, mpwi);
			ExprCall exprCall2 = this.GetExprFactory().CreateCall((EXPRFLAG)0, ctype2, exprList, exprMemberGroup, null);
			exprCall2.MethWithInst = new MethWithInst(mpwi);
			if (ek != ExpressionKind.Eq)
			{
				if (ek != ExpressionKind.NotEq)
				{
					exprCall2.NullableCallLiftKind = NullableCallLiftKind.Operator;
				}
				else
				{
					exprCall2.NullableCallLiftKind = NullableCallLiftKind.InequalityOperator;
				}
			}
			else
			{
				exprCall2.NullableCallLiftKind = NullableCallLiftKind.EqualityOperator;
			}
			exprCall2.CastOfNonLiftedResultToLiftedType = this.mustCast(exprCall, ctype2, (CONVERTTYPE)0);
			return exprCall2;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00010AC0 File Offset: 0x0000ECC0
		private AggregateType GetEnumBinOpType(ExpressionKind ek, CType argType1, CType argType2, out AggregateType ppEnumType)
		{
			AggregateType aggregateType = argType1 as AggregateType;
			AggregateType aggregateType2 = argType2 as AggregateType;
			AggregateType aggregateType3 = (aggregateType.isEnumType() ? aggregateType : aggregateType2);
			AggregateType aggregateType4 = aggregateType3;
			if (ek != ExpressionKind.Add)
			{
				if (ek != ExpressionKind.Subtract)
				{
					if (ek - ExpressionKind.BitwiseAnd > 2)
					{
						aggregateType4 = this.GetPredefindType(PredefinedType.PT_BOOL);
					}
				}
				else if (aggregateType == aggregateType2)
				{
					aggregateType4 = aggregateType3.underlyingEnumType();
				}
			}
			ppEnumType = aggregateType3;
			return aggregateType4;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00010B18 File Offset: 0x0000ED18
		private ExprBinOp CreateBinopForPredefMethodCall(ExpressionKind ek, PREDEFMETH predefMeth, CType RetType, Expr arg1, Expr arg2)
		{
			MethodSymbol method = this.GetSymbolLoader().getPredefinedMembers().GetMethod(predefMeth);
			ExprBinOp exprBinOp = this.GetExprFactory().CreateBinop(ek, RetType, arg1, arg2);
			if (method != null)
			{
				AggregateSymbol @class = method.getClass();
				AggregateType aggregate = this.GetTypes().GetAggregate(@class, BSYMMGR.EmptyTypeArray());
				exprBinOp.PredefinedMethodToCall = new MethWithInst(method, aggregate, null);
				exprBinOp.UserDefinedCallMethod = exprBinOp.PredefinedMethodToCall;
			}
			else
			{
				exprBinOp.SetError();
			}
			return exprBinOp;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00010B88 File Offset: 0x0000ED88
		private ExprUnaryOp CreateUnaryOpForPredefMethodCall(ExpressionKind ek, PREDEFMETH predefMeth, CType pRetType, Expr pArg)
		{
			MethodSymbol method = this.GetSymbolLoader().getPredefinedMembers().GetMethod(predefMeth);
			ExprUnaryOp exprUnaryOp = this.GetExprFactory().CreateUnaryOp(ek, pRetType, pArg);
			if (method != null)
			{
				AggregateSymbol @class = method.getClass();
				AggregateType aggregate = this.GetTypes().GetAggregate(@class, BSYMMGR.EmptyTypeArray());
				exprUnaryOp.PredefinedMethodToCall = new MethWithInst(method, aggregate, null);
				exprUnaryOp.UserDefinedCallMethod = exprUnaryOp.PredefinedMethodToCall;
			}
			else
			{
				exprUnaryOp.SetError();
			}
			return exprUnaryOp;
		}

		// Token: 0x04000247 RID: 583
		private static readonly byte[][] s_betterConversionTable = new byte[][]
		{
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 2,
				3, 3, 3, 3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				1, 1, 1, 3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				3, 1, 1, 3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				3, 3, 1, 3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				3, 3, 3, 3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				3, 3, 3, 3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				3, 3, 3, 3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				3, 3, 3, 3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				3, 3, 3, 3, 3, 3
			},
			new byte[]
			{
				1, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				1, 1, 1, 3, 3, 3
			},
			new byte[]
			{
				3, 2, 3, 3, 3, 3, 3, 3, 3, 2,
				3, 3, 3, 3, 3, 3
			},
			new byte[]
			{
				3, 2, 2, 3, 3, 3, 3, 3, 3, 2,
				3, 3, 3, 3, 3, 3
			},
			new byte[]
			{
				3, 2, 2, 2, 3, 3, 3, 3, 3, 2,
				3, 3, 3, 3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				3, 3, 3, 3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				3, 3, 3, 3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				3, 3, 3, 3, 3, 3
			}
		};

		// Token: 0x04000248 RID: 584
		private static readonly ErrorCode[] s_ReadOnlyLocalErrors = new ErrorCode[]
		{
			ErrorCode.ERR_RefReadonlyLocal,
			ErrorCode.ERR_AssgReadonlyLocal
		};

		// Token: 0x04000249 RID: 585
		private static readonly ErrorCode[] s_ReadOnlyErrors = new ErrorCode[]
		{
			ErrorCode.ERR_RefReadonly,
			ErrorCode.ERR_AssgReadonly,
			ErrorCode.ERR_RefReadonlyStatic,
			ErrorCode.ERR_AssgReadonlyStatic,
			ErrorCode.ERR_RefReadonly2,
			ErrorCode.ERR_AssgReadonly2,
			ErrorCode.ERR_RefReadonlyStatic2,
			ErrorCode.ERR_AssgReadonlyStatic2
		};

		// Token: 0x0400024A RID: 586
		private const byte ID = 1;

		// Token: 0x0400024B RID: 587
		private const byte IMP = 2;

		// Token: 0x0400024C RID: 588
		private const byte EXP = 3;

		// Token: 0x0400024D RID: 589
		private const byte NO = 5;

		// Token: 0x0400024E RID: 590
		private const byte CONV_KIND_MASK = 15;

		// Token: 0x0400024F RID: 591
		private const byte UDC = 64;

		// Token: 0x04000250 RID: 592
		private const byte XUD = 67;

		// Token: 0x04000251 RID: 593
		private const byte IUD = 66;

		// Token: 0x04000252 RID: 594
		private static readonly byte[][] s_simpleTypeConversions = new byte[][]
		{
			new byte[]
			{
				1, 2, 2, 2, 2, 2, 66, 3, 5, 3,
				2, 2, 2
			},
			new byte[]
			{
				3, 1, 2, 2, 2, 2, 66, 3, 5, 3,
				3, 3, 3
			},
			new byte[]
			{
				3, 3, 1, 2, 2, 2, 66, 3, 5, 3,
				3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 1, 2, 2, 66, 3, 5, 3,
				3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 3, 1, 2, 67, 3, 5, 3,
				3, 3, 3
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 1, 67, 3, 5, 3,
				3, 3, 3
			},
			new byte[]
			{
				67, 67, 67, 67, 67, 67, 1, 67, 5, 67,
				67, 67, 67
			},
			new byte[]
			{
				3, 3, 2, 2, 2, 2, 66, 1, 5, 3,
				2, 2, 2
			},
			new byte[]
			{
				5, 5, 5, 5, 5, 5, 5, 5, 1, 5,
				5, 5, 5
			},
			new byte[]
			{
				3, 2, 2, 2, 2, 2, 66, 3, 5, 1,
				3, 3, 3
			},
			new byte[]
			{
				3, 3, 2, 2, 2, 2, 66, 3, 5, 3,
				1, 2, 2
			},
			new byte[]
			{
				3, 3, 3, 2, 2, 2, 66, 3, 5, 3,
				3, 1, 2
			},
			new byte[]
			{
				3, 3, 3, 3, 2, 2, 66, 3, 5, 3,
				3, 3, 1
			}
		};

		// Token: 0x04000253 RID: 595
		private const int NUM_SIMPLE_TYPES = 13;

		// Token: 0x04000254 RID: 596
		private const int NUM_EXT_TYPES = 16;

		// Token: 0x04000255 RID: 597
		private const byte same = 0;

		// Token: 0x04000256 RID: 598
		private const byte left = 1;

		// Token: 0x04000257 RID: 599
		private const byte right = 2;

		// Token: 0x04000258 RID: 600
		private const byte neither = 3;

		// Token: 0x04000259 RID: 601
		private static readonly byte[][] s_simpleTypeBetter = new byte[][]
		{
			new byte[]
			{
				0, 1, 1, 1, 1, 1, 1, 3, 3, 2,
				1, 1, 1, 3, 3, 1
			},
			new byte[]
			{
				2, 0, 1, 1, 1, 1, 1, 3, 3, 2,
				1, 1, 1, 3, 3, 1
			},
			new byte[]
			{
				2, 2, 0, 1, 1, 1, 1, 2, 3, 2,
				2, 1, 1, 3, 3, 1
			},
			new byte[]
			{
				2, 2, 2, 0, 1, 1, 1, 2, 3, 2,
				2, 2, 1, 3, 3, 1
			},
			new byte[]
			{
				2, 2, 2, 2, 0, 1, 3, 2, 3, 2,
				2, 2, 2, 3, 3, 1
			},
			new byte[]
			{
				2, 2, 2, 2, 2, 0, 3, 2, 3, 2,
				2, 2, 2, 3, 3, 1
			},
			new byte[]
			{
				2, 2, 2, 2, 3, 3, 0, 2, 3, 2,
				2, 2, 2, 3, 3, 1
			},
			new byte[]
			{
				3, 3, 1, 1, 1, 1, 1, 0, 3, 3,
				1, 1, 1, 3, 3, 1
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 0, 3,
				3, 3, 3, 3, 3, 1
			},
			new byte[]
			{
				1, 1, 1, 1, 1, 1, 1, 3, 3, 0,
				1, 1, 1, 3, 3, 1
			},
			new byte[]
			{
				2, 2, 1, 1, 1, 1, 1, 2, 3, 2,
				0, 1, 1, 3, 3, 1
			},
			new byte[]
			{
				2, 2, 2, 1, 1, 1, 1, 2, 3, 2,
				2, 0, 1, 3, 3, 1
			},
			new byte[]
			{
				2, 2, 2, 2, 1, 1, 1, 2, 3, 2,
				2, 2, 0, 3, 3, 1
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				3, 3, 3, 0, 3, 1
			},
			new byte[]
			{
				3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
				3, 3, 3, 3, 0, 1
			},
			new byte[]
			{
				2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
				2, 2, 2, 2, 2, 0
			}
		};

		// Token: 0x0400025A RID: 602
		private BindingContext Context;

		// Token: 0x0400025B RID: 603
		private CNullable m_nullable;

		// Token: 0x0400025C RID: 604
		private static readonly PredefinedType[] s_rgptIntOp = new PredefinedType[]
		{
			PredefinedType.PT_INT,
			PredefinedType.PT_UINT,
			PredefinedType.PT_LONG,
			PredefinedType.PT_ULONG
		};

		// Token: 0x0400025D RID: 605
		private static readonly PredefinedName[] s_EK2NAME = new PredefinedName[]
		{
			PredefinedName.PN_OPEQUALS,
			PredefinedName.PN_OPCOMPARE,
			PredefinedName.PN_OPTRUE,
			PredefinedName.PN_OPFALSE,
			PredefinedName.PN_OPINCREMENT,
			PredefinedName.PN_OPDECREMENT,
			PredefinedName.PN_OPNEGATION,
			PredefinedName.PN_OPEQUALITY,
			PredefinedName.PN_OPINEQUALITY,
			PredefinedName.PN_OPLESSTHAN,
			PredefinedName.PN_OPLESSTHANOREQUAL,
			PredefinedName.PN_OPGREATERTHAN,
			PredefinedName.PN_OPGREATERTHANOREQUAL,
			PredefinedName.PN_OPPLUS,
			PredefinedName.PN_OPMINUS,
			PredefinedName.PN_OPMULTIPLY,
			PredefinedName.PN_OPDIVISION,
			PredefinedName.PN_OPMODULUS,
			PredefinedName.PN_OPUNARYMINUS,
			PredefinedName.PN_OPUNARYPLUS,
			PredefinedName.PN_OPBITWISEAND,
			PredefinedName.PN_OPBITWISEOR,
			PredefinedName.PN_OPXOR,
			PredefinedName.PN_OPCOMPLEMENT,
			PredefinedName.PN_OPLEFTSHIFT,
			PredefinedName.PN_OPRIGHTSHIFT
		};

		// Token: 0x0400025E RID: 606
		private readonly ExpressionBinder.BinOpSig[] g_binopSignatures;

		// Token: 0x0400025F RID: 607
		private readonly ExpressionBinder.UnaOpSig[] g_rguos;

		// Token: 0x020000DC RID: 220
		private sealed class BinOpArgInfo
		{
			// Token: 0x060006BF RID: 1727 RVA: 0x0001FAF8 File Offset: 0x0001DCF8
			public BinOpArgInfo(Expr op1, Expr op2)
			{
				this.arg1 = op1;
				this.arg2 = op2;
				this.type1 = this.arg1.Type;
				this.type2 = this.arg2.Type;
				this.typeRaw1 = this.type1.StripNubs();
				this.typeRaw2 = this.type2.StripNubs();
				this.pt1 = (this.type1.isPredefined() ? this.type1.getPredefType() : PredefinedType.PT_COUNT);
				this.pt2 = (this.type2.isPredefined() ? this.type2.getPredefType() : PredefinedType.PT_COUNT);
				this.ptRaw1 = (this.typeRaw1.isPredefined() ? this.typeRaw1.getPredefType() : PredefinedType.PT_COUNT);
				this.ptRaw2 = (this.typeRaw2.isPredefined() ? this.typeRaw2.getPredefType() : PredefinedType.PT_COUNT);
			}

			// Token: 0x060006C0 RID: 1728 RVA: 0x0001FBE5 File Offset: 0x0001DDE5
			public bool ValidForDelegate()
			{
				return (this.mask & BinOpMask.Delegate) > BinOpMask.None;
			}

			// Token: 0x060006C1 RID: 1729 RVA: 0x0001FBF3 File Offset: 0x0001DDF3
			public bool ValidForEnumAndUnderlyingType()
			{
				return (this.mask & BinOpMask.EnumUnder) > BinOpMask.None;
			}

			// Token: 0x060006C2 RID: 1730 RVA: 0x0001FC00 File Offset: 0x0001DE00
			public bool ValidForUnderlyingTypeAndEnum()
			{
				return (this.mask & BinOpMask.Add) > BinOpMask.None;
			}

			// Token: 0x060006C3 RID: 1731 RVA: 0x0001FC0D File Offset: 0x0001DE0D
			public bool ValidForEnum()
			{
				return (this.mask & BinOpMask.Enum) > BinOpMask.None;
			}

			// Token: 0x060006C4 RID: 1732 RVA: 0x0001FC1E File Offset: 0x0001DE1E
			public bool ValidForPointer()
			{
				return (this.mask & BinOpMask.Sub) > BinOpMask.None;
			}

			// Token: 0x060006C5 RID: 1733 RVA: 0x0001FC2B File Offset: 0x0001DE2B
			public bool ValidForVoidPointer()
			{
				return (this.mask & BinOpMask.VoidPtr) > BinOpMask.None;
			}

			// Token: 0x060006C6 RID: 1734 RVA: 0x0001FC39 File Offset: 0x0001DE39
			public bool ValidForPointerAndNumber()
			{
				return (this.mask & BinOpMask.EnumUnder) > BinOpMask.None;
			}

			// Token: 0x060006C7 RID: 1735 RVA: 0x0001FC46 File Offset: 0x0001DE46
			public bool ValidForNumberAndPointer()
			{
				return (this.mask & BinOpMask.Add) > BinOpMask.None;
			}

			// Token: 0x04000699 RID: 1689
			public Expr arg1;

			// Token: 0x0400069A RID: 1690
			public Expr arg2;

			// Token: 0x0400069B RID: 1691
			public PredefinedType pt1;

			// Token: 0x0400069C RID: 1692
			public PredefinedType pt2;

			// Token: 0x0400069D RID: 1693
			public PredefinedType ptRaw1;

			// Token: 0x0400069E RID: 1694
			public PredefinedType ptRaw2;

			// Token: 0x0400069F RID: 1695
			public CType type1;

			// Token: 0x040006A0 RID: 1696
			public CType type2;

			// Token: 0x040006A1 RID: 1697
			public CType typeRaw1;

			// Token: 0x040006A2 RID: 1698
			public CType typeRaw2;

			// Token: 0x040006A3 RID: 1699
			public BinOpKind binopKind;

			// Token: 0x040006A4 RID: 1700
			public BinOpMask mask;
		}

		// Token: 0x020000DD RID: 221
		private class BinOpSig
		{
			// Token: 0x060006C8 RID: 1736 RVA: 0x0001FC53 File Offset: 0x0001DE53
			protected BinOpSig()
			{
			}

			// Token: 0x060006C9 RID: 1737 RVA: 0x0001FC5B File Offset: 0x0001DE5B
			public BinOpSig(PredefinedType pt1, PredefinedType pt2, BinOpMask mask, int cbosSkip, ExpressionBinder.PfnBindBinOp pfn, OpSigFlags grfos, BinOpFuncKind fnkind)
			{
				this.pt1 = pt1;
				this.pt2 = pt2;
				this.mask = mask;
				this.cbosSkip = cbosSkip;
				this.pfn = pfn;
				this.grfos = grfos;
				this.fnkind = fnkind;
			}

			// Token: 0x060006CA RID: 1738 RVA: 0x0001FC98 File Offset: 0x0001DE98
			public bool ConvertOperandsBeforeBinding()
			{
				return (this.grfos & OpSigFlags.Convert) > OpSigFlags.None;
			}

			// Token: 0x060006CB RID: 1739 RVA: 0x0001FCA5 File Offset: 0x0001DEA5
			public bool CanLift()
			{
				return (this.grfos & OpSigFlags.CanLift) > OpSigFlags.None;
			}

			// Token: 0x060006CC RID: 1740 RVA: 0x0001FCB2 File Offset: 0x0001DEB2
			public bool AutoLift()
			{
				return (this.grfos & OpSigFlags.AutoLift) > OpSigFlags.None;
			}

			// Token: 0x040006A5 RID: 1701
			public PredefinedType pt1;

			// Token: 0x040006A6 RID: 1702
			public PredefinedType pt2;

			// Token: 0x040006A7 RID: 1703
			public BinOpMask mask;

			// Token: 0x040006A8 RID: 1704
			public int cbosSkip;

			// Token: 0x040006A9 RID: 1705
			public ExpressionBinder.PfnBindBinOp pfn;

			// Token: 0x040006AA RID: 1706
			public OpSigFlags grfos;

			// Token: 0x040006AB RID: 1707
			public BinOpFuncKind fnkind;
		}

		// Token: 0x020000DE RID: 222
		private sealed class BinOpFullSig : ExpressionBinder.BinOpSig
		{
			// Token: 0x060006CD RID: 1741 RVA: 0x0001FCC0 File Offset: 0x0001DEC0
			public BinOpFullSig(CType type1, CType type2, ExpressionBinder.PfnBindBinOp pfn, OpSigFlags grfos, LiftFlags grflt, BinOpFuncKind fnkind)
			{
				this.pt1 = (PredefinedType)4294967295U;
				this.pt2 = (PredefinedType)4294967295U;
				this.mask = BinOpMask.None;
				this.cbosSkip = 0;
				this.pfn = pfn;
				this.grfos = grfos;
				this._type1 = type1;
				this._type2 = type2;
				this._grflt = grflt;
				this.fnkind = fnkind;
			}

			// Token: 0x060006CE RID: 1742 RVA: 0x0001FD1C File Offset: 0x0001DF1C
			public BinOpFullSig(ExpressionBinder fnc, ExpressionBinder.BinOpSig bos)
			{
				this.pt1 = bos.pt1;
				this.pt2 = bos.pt2;
				this.mask = bos.mask;
				this.cbosSkip = bos.cbosSkip;
				this.pfn = bos.pfn;
				this.grfos = bos.grfos;
				this.fnkind = bos.fnkind;
				this._type1 = ((this.pt1 != (PredefinedType)4294967295U) ? fnc.GetPredefindType(this.pt1) : null);
				this._type2 = ((this.pt2 != (PredefinedType)4294967295U) ? fnc.GetPredefindType(this.pt2) : null);
				this._grflt = LiftFlags.None;
			}

			// Token: 0x060006CF RID: 1743 RVA: 0x0001FDC6 File Offset: 0x0001DFC6
			public bool FPreDef()
			{
				return this.pt1 != (PredefinedType)4294967295U;
			}

			// Token: 0x060006D0 RID: 1744 RVA: 0x0001FDD4 File Offset: 0x0001DFD4
			public bool isLifted()
			{
				return this._grflt != LiftFlags.None;
			}

			// Token: 0x060006D1 RID: 1745 RVA: 0x0001FDE1 File Offset: 0x0001DFE1
			public bool ConvertFirst()
			{
				return (this._grflt & LiftFlags.Convert1) > LiftFlags.None;
			}

			// Token: 0x060006D2 RID: 1746 RVA: 0x0001FDEE File Offset: 0x0001DFEE
			public bool ConvertSecond()
			{
				return (this._grflt & LiftFlags.Convert2) > LiftFlags.None;
			}

			// Token: 0x060006D3 RID: 1747 RVA: 0x0001FDFB File Offset: 0x0001DFFB
			public CType Type1()
			{
				return this._type1;
			}

			// Token: 0x060006D4 RID: 1748 RVA: 0x0001FE03 File Offset: 0x0001E003
			public CType Type2()
			{
				return this._type2;
			}

			// Token: 0x040006AC RID: 1708
			private readonly LiftFlags _grflt;

			// Token: 0x040006AD RID: 1709
			private readonly CType _type1;

			// Token: 0x040006AE RID: 1710
			private readonly CType _type2;
		}

		// Token: 0x020000DF RID: 223
		// (Invoke) Token: 0x060006D6 RID: 1750
		private delegate bool ConversionFunc(Expr pSourceExpr, CType pSourceType, ExprClass pDestinationTypeExpr, CType pDestinationTypeForLambdaErrorReporting, bool needsExprDest, out Expr ppDestinationExpr, CONVERTTYPE flags);

		// Token: 0x020000E0 RID: 224
		private sealed class ExplicitConversion
		{
			// Token: 0x060006D9 RID: 1753 RVA: 0x0001FE0C File Offset: 0x0001E00C
			public ExplicitConversion(ExpressionBinder binder, Expr exprSrc, CType typeSrc, ExprClass typeDest, CType pDestinationTypeForLambdaErrorReporting, bool needsExprDest, CONVERTTYPE flags)
			{
				this._binder = binder;
				this._exprSrc = exprSrc;
				this._typeSrc = typeSrc;
				this._typeDest = typeDest.Type;
				this._pDestinationTypeForLambdaErrorReporting = pDestinationTypeForLambdaErrorReporting;
				this._exprTypeDest = typeDest;
				this._needsExprDest = needsExprDest;
				this._flags = flags;
				this._exprDest = null;
			}

			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x060006DA RID: 1754 RVA: 0x0001FE68 File Offset: 0x0001E068
			public Expr ExprDest
			{
				get
				{
					return this._exprDest;
				}
			}

			// Token: 0x060006DB RID: 1755 RVA: 0x0001FE70 File Offset: 0x0001E070
			public bool Bind()
			{
				if (this._binder.BindImplicitConversion(this._exprSrc, this._typeSrc, this._exprTypeDest, this._pDestinationTypeForLambdaErrorReporting, this._needsExprDest, out this._exprDest, this._flags | CONVERTTYPE.ISEXPLICIT))
				{
					return true;
				}
				if (this._typeSrc == null || this._typeDest == null || this._typeSrc is ErrorType || this._typeDest is ErrorType || this._typeDest.IsNeverSameType())
				{
					return false;
				}
				if (this._typeDest is NullableType)
				{
					return false;
				}
				if (this._typeSrc is NullableType)
				{
					return this.bindExplicitConversionFromNub();
				}
				if (this.bindExplicitConversionFromArrayToIList())
				{
					return true;
				}
				switch (this._typeDest.GetTypeKind())
				{
				case TypeKind.TK_AggregateType:
				{
					AggCastResult aggCastResult = this.bindExplicitConversionToAggregate(this._typeDest as AggregateType);
					if (aggCastResult == AggCastResult.Success)
					{
						return true;
					}
					if (aggCastResult == AggCastResult.Abort)
					{
						return false;
					}
					break;
				}
				case TypeKind.TK_VoidType:
					return false;
				case TypeKind.TK_NullType:
					return false;
				default:
					return false;
				case TypeKind.TK_ArrayType:
					if (this.bindExplicitConversionToArray((ArrayType)this._typeDest))
					{
						return true;
					}
					break;
				case TypeKind.TK_PointerType:
					if (this.bindExplicitConversionToPointer())
					{
						return true;
					}
					break;
				}
				return (this._flags & CONVERTTYPE.NOUDC) == (CONVERTTYPE)0 && this._binder.bindUserDefinedConversion(this._exprSrc, this._typeSrc, this._typeDest, this._needsExprDest, out this._exprDest, false);
			}

			// Token: 0x060006DC RID: 1756 RVA: 0x0001FFC8 File Offset: 0x0001E1C8
			private bool bindExplicitConversionFromNub()
			{
				if (this._typeDest.IsValType() && this._binder.BindExplicitConversion(null, this._typeSrc.StripNubs(), this._exprTypeDest, this._pDestinationTypeForLambdaErrorReporting, this._flags | CONVERTTYPE.NOUDC))
				{
					if (this._needsExprDest)
					{
						Expr expr = this._exprSrc;
						if (expr.Type is NullableType)
						{
							expr = this._binder.BindNubValue(expr);
						}
						if (!this._binder.BindExplicitConversion(expr, expr.Type, this._exprTypeDest, this._pDestinationTypeForLambdaErrorReporting, this._needsExprDest, out this._exprDest, this._flags | CONVERTTYPE.NOUDC))
						{
							return false;
						}
						ExprUserDefinedConversion exprUserDefinedConversion;
						if ((exprUserDefinedConversion = this._exprDest as ExprUserDefinedConversion) != null)
						{
							exprUserDefinedConversion.Argument = this._exprSrc;
						}
					}
					return true;
				}
				return (this._flags & CONVERTTYPE.NOUDC) == (CONVERTTYPE)0 && this._binder.bindUserDefinedConversion(this._exprSrc, this._typeSrc, this._typeDest, this._needsExprDest, out this._exprDest, false);
			}

			// Token: 0x060006DD RID: 1757 RVA: 0x000200C4 File Offset: 0x0001E2C4
			private bool bindExplicitConversionFromArrayToIList()
			{
				ArrayType arrayType;
				AggregateType aggregateType;
				if ((arrayType = this._typeSrc as ArrayType) == null || !arrayType.IsSZArray || (aggregateType = this._typeDest as AggregateType) == null || !aggregateType.isInterfaceType() || aggregateType.GetTypeArgsAll().Count != 1)
				{
					return false;
				}
				AggregateSymbol predefAgg = this.GetSymbolLoader().GetPredefAgg(PredefinedType.PT_G_ILIST);
				AggregateSymbol predefAgg2 = this.GetSymbolLoader().GetPredefAgg(PredefinedType.PT_G_IREADONLYLIST);
				if ((predefAgg == null || !this.GetSymbolLoader().IsBaseAggregate(predefAgg, aggregateType.getAggregate())) && (predefAgg2 == null || !this.GetSymbolLoader().IsBaseAggregate(predefAgg2, aggregateType.getAggregate())))
				{
					return false;
				}
				CType elementType = arrayType.GetElementType();
				CType ctype = aggregateType.GetTypeArgsAll()[0];
				if (!CConversions.FExpRefConv(this.GetSymbolLoader(), elementType, ctype))
				{
					return false;
				}
				if (this._needsExprDest)
				{
					this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest, EXPRFLAG.EXF_OPERATOR);
				}
				return true;
			}

			// Token: 0x060006DE RID: 1758 RVA: 0x000201AC File Offset: 0x0001E3AC
			private bool bindExplicitConversionFromIListToArray(ArrayType arrayDest)
			{
				AggregateType aggregateType;
				if (!arrayDest.IsSZArray || (aggregateType = this._typeSrc as AggregateType) == null || !aggregateType.isInterfaceType() || aggregateType.GetTypeArgsAll().Count != 1)
				{
					return false;
				}
				AggregateSymbol predefAgg = this.GetSymbolLoader().GetPredefAgg(PredefinedType.PT_G_ILIST);
				AggregateSymbol predefAgg2 = this.GetSymbolLoader().GetPredefAgg(PredefinedType.PT_G_IREADONLYLIST);
				if ((predefAgg == null || !this.GetSymbolLoader().IsBaseAggregate(predefAgg, aggregateType.getAggregate())) && (predefAgg2 == null || !this.GetSymbolLoader().IsBaseAggregate(predefAgg2, aggregateType.getAggregate())))
				{
					return false;
				}
				CType elementType = arrayDest.GetElementType();
				CType ctype = aggregateType.GetTypeArgsAll()[0];
				if (elementType != ctype && !CConversions.FExpRefConv(this.GetSymbolLoader(), elementType, ctype))
				{
					return false;
				}
				if (this._needsExprDest)
				{
					this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest, EXPRFLAG.EXF_OPERATOR);
				}
				return true;
			}

			// Token: 0x060006DF RID: 1759 RVA: 0x00020288 File Offset: 0x0001E488
			private bool bindExplicitConversionFromArrayToArray(ArrayType arraySrc, ArrayType arrayDest)
			{
				if (arraySrc.rank != arrayDest.rank || arraySrc.IsSZArray != arrayDest.IsSZArray)
				{
					return false;
				}
				if (CConversions.FExpRefConv(this.GetSymbolLoader(), arraySrc.GetElementType(), arrayDest.GetElementType()))
				{
					if (this._needsExprDest)
					{
						this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest, EXPRFLAG.EXF_OPERATOR);
					}
					return true;
				}
				return false;
			}

			// Token: 0x060006E0 RID: 1760 RVA: 0x000202F8 File Offset: 0x0001E4F8
			private bool bindExplicitConversionToArray(ArrayType arrayDest)
			{
				ArrayType arrayType;
				if ((arrayType = this._typeSrc as ArrayType) != null)
				{
					return this.bindExplicitConversionFromArrayToArray(arrayType, arrayDest);
				}
				if (this.bindExplicitConversionFromIListToArray(arrayDest))
				{
					return true;
				}
				if (this._binder.canConvert(this._binder.GetPredefindType(PredefinedType.PT_ARRAY), this._typeSrc, CONVERTTYPE.NOUDC))
				{
					if (this._needsExprDest)
					{
						this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest, EXPRFLAG.EXF_OPERATOR);
					}
					return true;
				}
				return false;
			}

			// Token: 0x060006E1 RID: 1761 RVA: 0x00020374 File Offset: 0x0001E574
			private bool bindExplicitConversionToPointer()
			{
				if (this._typeSrc is PointerType || (this._typeSrc.fundType() <= FUNDTYPE.FT_U8 && this._typeSrc.isNumericType()))
				{
					if (this._needsExprDest)
					{
						this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest);
					}
					return true;
				}
				return false;
			}

			// Token: 0x060006E2 RID: 1762 RVA: 0x000203D4 File Offset: 0x0001E5D4
			private AggCastResult bindExplicitConversionFromEnumToAggregate(AggregateType aggTypeDest)
			{
				if (!this._typeSrc.isEnumType())
				{
					return AggCastResult.Failure;
				}
				AggregateSymbol aggregate = aggTypeDest.getAggregate();
				if (aggregate.isPredefAgg(PredefinedType.PT_DECIMAL))
				{
					return this.bindExplicitConversionFromEnumToDecimal(aggTypeDest);
				}
				if (!aggregate.getThisType().isNumericType() && !aggregate.IsEnum() && (!aggregate.IsPredefined() || aggregate.GetPredefType() != PredefinedType.PT_CHAR))
				{
					return AggCastResult.Failure;
				}
				if (this._exprSrc.GetConst() != null)
				{
					ConstCastResult constCastResult = this._binder.bindConstantCast(this._exprSrc, this._exprTypeDest, this._needsExprDest, out this._exprDest, true);
					if (constCastResult == ConstCastResult.Success)
					{
						return AggCastResult.Success;
					}
					if (constCastResult == ConstCastResult.CheckFailure)
					{
						return AggCastResult.Abort;
					}
				}
				if (this._needsExprDest)
				{
					this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest);
				}
				return AggCastResult.Success;
			}

			// Token: 0x060006E3 RID: 1763 RVA: 0x00020494 File Offset: 0x0001E694
			private AggCastResult bindExplicitConversionFromDecimalToEnum(AggregateType aggTypeDest)
			{
				if (this._exprSrc.GetConst() != null)
				{
					ConstCastResult constCastResult = this._binder.bindConstantCast(this._exprSrc, this._exprTypeDest, this._needsExprDest, out this._exprDest, true);
					if (constCastResult == ConstCastResult.Success)
					{
						return AggCastResult.Success;
					}
					if (constCastResult == ConstCastResult.CheckFailure && (this._flags & CONVERTTYPE.CHECKOVERFLOW) == (CONVERTTYPE)0)
					{
						return AggCastResult.Abort;
					}
				}
				bool flag = true;
				if (this._needsExprDest)
				{
					CType ctype = aggTypeDest.underlyingType();
					flag = this._binder.bindUserDefinedConversion(this._exprSrc, this._typeSrc, ctype, this._needsExprDest, out this._exprDest, false);
					if (flag)
					{
						this._binder.bindSimpleCast(this._exprDest, this._exprTypeDest, out this._exprDest);
					}
				}
				if (!flag)
				{
					return AggCastResult.Failure;
				}
				return AggCastResult.Success;
			}

			// Token: 0x060006E4 RID: 1764 RVA: 0x00020548 File Offset: 0x0001E748
			private AggCastResult bindExplicitConversionFromEnumToDecimal(AggregateType aggTypeDest)
			{
				AggregateType aggregateType = this._typeSrc.underlyingType() as AggregateType;
				Expr expr;
				if (this._exprSrc == null)
				{
					expr = null;
				}
				else
				{
					ExprClass exprClass = this.GetExprFactory().CreateClass(aggregateType);
					this._binder.bindSimpleCast(this._exprSrc, exprClass, out expr);
				}
				if (expr.GetConst() != null)
				{
					ConstCastResult constCastResult = this._binder.bindConstantCast(expr, this._exprTypeDest, this._needsExprDest, out this._exprDest, true);
					if (constCastResult == ConstCastResult.Success)
					{
						return AggCastResult.Success;
					}
					if (constCastResult == ConstCastResult.CheckFailure && (this._flags & CONVERTTYPE.CHECKOVERFLOW) == (CONVERTTYPE)0)
					{
						return AggCastResult.Abort;
					}
				}
				if (this._needsExprDest)
				{
					this._binder.bindUserDefinedConversion(expr, aggregateType, aggTypeDest, this._needsExprDest, out this._exprDest, false);
				}
				return AggCastResult.Success;
			}

			// Token: 0x060006E5 RID: 1765 RVA: 0x000205F8 File Offset: 0x0001E7F8
			private AggCastResult bindExplicitConversionToEnum(AggregateType aggTypeDest)
			{
				if (!aggTypeDest.getAggregate().IsEnum())
				{
					return AggCastResult.Failure;
				}
				if (this._typeSrc.isPredefType(PredefinedType.PT_DECIMAL))
				{
					return this.bindExplicitConversionFromDecimalToEnum(aggTypeDest);
				}
				if (this._typeSrc.isNumericType() || (this._typeSrc.isPredefined() && this._typeSrc.getPredefType() == PredefinedType.PT_CHAR))
				{
					if (this._exprSrc.GetConst() != null)
					{
						ConstCastResult constCastResult = this._binder.bindConstantCast(this._exprSrc, this._exprTypeDest, this._needsExprDest, out this._exprDest, true);
						if (constCastResult == ConstCastResult.Success)
						{
							return AggCastResult.Success;
						}
						if (constCastResult == ConstCastResult.CheckFailure)
						{
							return AggCastResult.Abort;
						}
					}
					if (this._needsExprDest)
					{
						this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest);
					}
					return AggCastResult.Success;
				}
				if (this._typeSrc.isPredefined() && (this._typeSrc.isPredefType(PredefinedType.PT_OBJECT) || this._typeSrc.isPredefType(PredefinedType.PT_VALUE) || this._typeSrc.isPredefType(PredefinedType.PT_ENUM)))
				{
					if (this._needsExprDest)
					{
						this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest, EXPRFLAG.EXF_INDEXER);
					}
					return AggCastResult.Success;
				}
				return AggCastResult.Failure;
			}

			// Token: 0x060006E6 RID: 1766 RVA: 0x0002071C File Offset: 0x0001E91C
			private AggCastResult bindExplicitConversionBetweenSimpleTypes(AggregateType aggTypeDest)
			{
				if (!this._typeSrc.isSimpleType() || !aggTypeDest.isSimpleType())
				{
					return AggCastResult.Failure;
				}
				AggregateSymbol aggregate = aggTypeDest.getAggregate();
				PredefinedType predefType = this._typeSrc.getPredefType();
				PredefinedType predefType2 = aggregate.GetPredefType();
				if (ExpressionBinder.GetConvKind(predefType, predefType2) != ConvKind.Explicit)
				{
					return AggCastResult.Failure;
				}
				if (this._exprSrc.GetConst() != null)
				{
					ConstCastResult constCastResult = this._binder.bindConstantCast(this._exprSrc, this._exprTypeDest, this._needsExprDest, out this._exprDest, true);
					if (constCastResult == ConstCastResult.Success)
					{
						return AggCastResult.Success;
					}
					if (constCastResult == ConstCastResult.CheckFailure && (this._flags & CONVERTTYPE.CHECKOVERFLOW) == (CONVERTTYPE)0)
					{
						return AggCastResult.Abort;
					}
				}
				bool flag = true;
				if (this._needsExprDest)
				{
					if (ExpressionBinder.isUserDefinedConversion(predefType, predefType2))
					{
						flag = this._binder.bindUserDefinedConversion(this._exprSrc, this._typeSrc, aggTypeDest, this._needsExprDest, out this._exprDest, false);
					}
					else
					{
						this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest, ((this._flags & CONVERTTYPE.CHECKOVERFLOW) != (CONVERTTYPE)0) ? EXPRFLAG.EXF_CHECKOVERFLOW : ((EXPRFLAG)0));
					}
				}
				if (!flag)
				{
					return AggCastResult.Failure;
				}
				return AggCastResult.Success;
			}

			// Token: 0x060006E7 RID: 1767 RVA: 0x0002081C File Offset: 0x0001EA1C
			private AggCastResult bindExplicitConversionBetweenAggregates(AggregateType aggTypeDest)
			{
				AggregateType aggregateType;
				if ((aggregateType = this._typeSrc as AggregateType) == null)
				{
					return AggCastResult.Failure;
				}
				AggregateSymbol aggregate = aggregateType.getAggregate();
				AggregateSymbol aggregate2 = aggTypeDest.getAggregate();
				if (this.GetSymbolLoader().HasBaseConversion(aggTypeDest, aggregateType))
				{
					if (this._needsExprDest)
					{
						if (aggregate2.IsValueType() && aggregate.getThisType().fundType() == FUNDTYPE.FT_REF)
						{
							this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest, EXPRFLAG.EXF_INDEXER);
						}
						else
						{
							ExpressionBinder binder = this._binder;
							Expr exprSrc = this._exprSrc;
							ExprClass exprTypeDest = this._exprTypeDest;
							EXPRFLAG exprflag = EXPRFLAG.EXF_OPERATOR;
							Expr exprSrc2 = this._exprSrc;
							binder.bindSimpleCast(exprSrc, exprTypeDest, out this._exprDest, exprflag | ((exprSrc2 != null) ? (exprSrc2.Flags & EXPRFLAG.EXF_CANTBENULL) : ((EXPRFLAG)0)));
						}
					}
					return AggCastResult.Success;
				}
				if ((aggregate.IsClass() && !aggregate.IsSealed() && aggregate2.IsInterface()) || (aggregate.IsInterface() && aggregate2.IsClass() && !aggregate2.IsSealed()) || (aggregate.IsInterface() && aggregate2.IsInterface()) || CConversions.HasGenericDelegateExplicitReferenceConversion(this.GetSymbolLoader(), this._typeSrc, aggTypeDest))
				{
					if (this._needsExprDest)
					{
						ExpressionBinder binder2 = this._binder;
						Expr exprSrc3 = this._exprSrc;
						ExprClass exprTypeDest2 = this._exprTypeDest;
						EXPRFLAG exprflag2 = EXPRFLAG.EXF_OPERATOR;
						Expr exprSrc4 = this._exprSrc;
						binder2.bindSimpleCast(exprSrc3, exprTypeDest2, out this._exprDest, exprflag2 | ((exprSrc4 != null) ? (exprSrc4.Flags & EXPRFLAG.EXF_CANTBENULL) : ((EXPRFLAG)0)));
					}
					return AggCastResult.Success;
				}
				return AggCastResult.Failure;
			}

			// Token: 0x060006E8 RID: 1768 RVA: 0x00020968 File Offset: 0x0001EB68
			private AggCastResult bindExplicitConversionFromPointerToInt(AggregateType aggTypeDest)
			{
				if (!(this._typeSrc is PointerType) || aggTypeDest.fundType() > FUNDTYPE.FT_U8 || !aggTypeDest.isNumericType())
				{
					return AggCastResult.Failure;
				}
				if (this._needsExprDest)
				{
					this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest);
				}
				return AggCastResult.Success;
			}

			// Token: 0x060006E9 RID: 1769 RVA: 0x000209BC File Offset: 0x0001EBBC
			private AggCastResult bindExplicitConversionToAggregate(AggregateType aggTypeDest)
			{
				AggCastResult aggCastResult = this.bindExplicitConversionFromEnumToAggregate(aggTypeDest);
				if (aggCastResult != AggCastResult.Failure)
				{
					return aggCastResult;
				}
				aggCastResult = this.bindExplicitConversionToEnum(aggTypeDest);
				if (aggCastResult != AggCastResult.Failure)
				{
					return aggCastResult;
				}
				aggCastResult = this.bindExplicitConversionBetweenSimpleTypes(aggTypeDest);
				if (aggCastResult != AggCastResult.Failure)
				{
					return aggCastResult;
				}
				aggCastResult = this.bindExplicitConversionBetweenAggregates(aggTypeDest);
				if (aggCastResult != AggCastResult.Failure)
				{
					return aggCastResult;
				}
				aggCastResult = this.bindExplicitConversionFromPointerToInt(aggTypeDest);
				if (aggCastResult != AggCastResult.Failure)
				{
					return aggCastResult;
				}
				if (this._typeSrc is VoidType)
				{
					return AggCastResult.Abort;
				}
				return AggCastResult.Failure;
			}

			// Token: 0x060006EA RID: 1770 RVA: 0x00020A1F File Offset: 0x0001EC1F
			private SymbolLoader GetSymbolLoader()
			{
				return this._binder.GetSymbolLoader();
			}

			// Token: 0x060006EB RID: 1771 RVA: 0x00020A2C File Offset: 0x0001EC2C
			private ExprFactory GetExprFactory()
			{
				return this._binder.GetExprFactory();
			}

			// Token: 0x040006AF RID: 1711
			private readonly ExpressionBinder _binder;

			// Token: 0x040006B0 RID: 1712
			private Expr _exprSrc;

			// Token: 0x040006B1 RID: 1713
			private readonly CType _typeSrc;

			// Token: 0x040006B2 RID: 1714
			private readonly CType _typeDest;

			// Token: 0x040006B3 RID: 1715
			private readonly ExprClass _exprTypeDest;

			// Token: 0x040006B4 RID: 1716
			private readonly CType _pDestinationTypeForLambdaErrorReporting;

			// Token: 0x040006B5 RID: 1717
			private Expr _exprDest;

			// Token: 0x040006B6 RID: 1718
			private readonly bool _needsExprDest;

			// Token: 0x040006B7 RID: 1719
			private readonly CONVERTTYPE _flags;
		}

		// Token: 0x020000E1 RID: 225
		// (Invoke) Token: 0x060006ED RID: 1773
		private delegate Expr PfnBindBinOp(ExpressionKind ek, EXPRFLAG flags, Expr op1, Expr op2);

		// Token: 0x020000E2 RID: 226
		// (Invoke) Token: 0x060006F1 RID: 1777
		private delegate Expr PfnBindUnaOp(ExpressionKind ek, EXPRFLAG flags, Expr op);

		// Token: 0x020000E3 RID: 227
		internal sealed class GroupToArgsBinder
		{
			// Token: 0x060006F4 RID: 1780 RVA: 0x00020A3C File Offset: 0x0001EC3C
			public GroupToArgsBinder(ExpressionBinder exprBinder, BindingFlag bindFlags, ExprMemberGroup grp, ArgInfos args, ArgInfos originalArgs, bool bHasNamedArguments, AggregateType atsDelegate)
			{
				this._pExprBinder = exprBinder;
				this._fCandidatesUnsupported = false;
				this._fBindFlags = bindFlags;
				this._pGroup = grp;
				this._pArguments = args;
				this._pOriginalArguments = originalArgs;
				this._bHasNamedArguments = bHasNamedArguments;
				this._pDelegate = atsDelegate;
				this._pCurrentType = null;
				this._pCurrentSym = null;
				this._pCurrentTypeArgs = null;
				this._pCurrentParameters = null;
				this._pBestParameters = null;
				this._nArgBest = -1;
				this._results = new ExpressionBinder.GroupToArgsBinderResult();
				this._methList = new List<CandidateFunctionMember>();
				this._mpwiParamTypeConstraints = new MethPropWithInst();
				this._mpwiBogus = new MethPropWithInst();
				this._mpwiCantInferInstArg = new MethPropWithInst();
				this._mwtBadArity = new MethWithType();
				this._HiddenTypes = new List<CType>();
			}

			// Token: 0x060006F5 RID: 1781 RVA: 0x00020B02 File Offset: 0x0001ED02
			public bool Bind(bool bReportErrors)
			{
				this.LookForCandidates();
				if (this.GetResultOfBind(bReportErrors))
				{
					return true;
				}
				if (bReportErrors)
				{
					throw this.ReportErrorsOnFailure();
				}
				return false;
			}

			// Token: 0x060006F6 RID: 1782 RVA: 0x00020B20 File Offset: 0x0001ED20
			public ExpressionBinder.GroupToArgsBinderResult GetResultsOfBind()
			{
				return this._results;
			}

			// Token: 0x060006F7 RID: 1783 RVA: 0x00020B28 File Offset: 0x0001ED28
			private SymbolLoader GetSymbolLoader()
			{
				return this._pExprBinder.GetSymbolLoader();
			}

			// Token: 0x060006F8 RID: 1784 RVA: 0x00020B35 File Offset: 0x0001ED35
			private CSemanticChecker GetSemanticChecker()
			{
				return this._pExprBinder.GetSemanticChecker();
			}

			// Token: 0x060006F9 RID: 1785 RVA: 0x00020B42 File Offset: 0x0001ED42
			private ErrorHandling GetErrorContext()
			{
				return this._pExprBinder.GetErrorContext();
			}

			// Token: 0x060006FA RID: 1786 RVA: 0x00020B4F File Offset: 0x0001ED4F
			private static CType GetTypeQualifier(ExprMemberGroup pGroup)
			{
				if ((pGroup.Flags & EXPRFLAG.EXF_CTOR) != (EXPRFLAG)0)
				{
					return pGroup.ParentType;
				}
				Expr optionalObject = pGroup.OptionalObject;
				if (optionalObject == null)
				{
					return null;
				}
				return optionalObject.Type;
			}

			// Token: 0x060006FB RID: 1787 RVA: 0x00020B74 File Offset: 0x0001ED74
			private void LookForCandidates()
			{
				bool flag = false;
				bool flag2 = true;
				bool flag3 = true;
				bool flag4 = false;
				symbmask_t symbmask_t = (symbmask_t)(1 << (int)this._pGroup.SymKind);
				Expr optionalObject = this._pGroup.OptionalObject;
				CType ctype = ((optionalObject != null) ? optionalObject.Type : null);
				CMemberLookupResults.CMethodIterator methodIterator = this._pGroup.MemberLookupResults.GetMethodIterator(this.GetSemanticChecker(), this.GetSymbolLoader(), ctype, ExpressionBinder.GroupToArgsBinder.GetTypeQualifier(this._pGroup), this._pExprBinder.ContextForMemberLookup(), true, false, this._pGroup.TypeArgs.Count, this._pGroup.Flags, symbmask_t);
				for (;;)
				{
					bool flag5 = false;
					if (flag2 && !flag)
					{
						flag = (flag5 = this.ConstructExpandedParameters());
					}
					if (!flag5)
					{
						flag = false;
						if (!this.GetNextSym(methodIterator))
						{
							break;
						}
						this._pCurrentParameters = this._pCurrentSym.Params;
						flag2 = true;
					}
					if (this._bArgumentsChangedForNamedOrOptionalArguments)
					{
						this._bArgumentsChangedForNamedOrOptionalArguments = false;
						this.CopyArgInfos(this._pOriginalArguments, this._pArguments);
					}
					if (this._pArguments.fHasExprs)
					{
						if (this._bHasNamedArguments)
						{
							if (!this.ReOrderArgsForNamedArguments())
							{
								continue;
							}
						}
						else if (this.HasOptionalParameters() && !this.AddArgumentsForOptionalParameters())
						{
							continue;
						}
					}
					if (!flag5)
					{
						flag4 = true;
						flag3 &= CSemanticChecker.CheckBogus(this._pCurrentSym);
						if (this._pCurrentParameters.Count != this._pArguments.carg)
						{
							flag2 = true;
							continue;
						}
					}
					if (methodIterator.CanUseCurrentSymbol())
					{
						ExpressionBinder.GroupToArgsBinder.Result result = this.DetermineCurrentTypeArgs();
						if (result != ExpressionBinder.GroupToArgsBinder.Result.Success)
						{
							flag2 = result == ExpressionBinder.GroupToArgsBinder.Result.Failure_SearchForExpanded;
						}
						else
						{
							bool flag6 = !methodIterator.IsCurrentSymbolInaccessible();
							if (!flag6 && (!this._methList.IsEmpty<CandidateFunctionMember>() || this._results.GetInaccessibleResult()))
							{
								flag2 = false;
							}
							else
							{
								bool flag7 = flag6 && methodIterator.IsCurrentSymbolBogus();
								if (flag7 && (!this._methList.IsEmpty<CandidateFunctionMember>() || this._results.GetInaccessibleResult() || this._mpwiBogus))
								{
									flag2 = false;
								}
								else if (!this.ArgumentsAreConvertible())
								{
									flag2 = true;
								}
								else
								{
									if (!flag6)
									{
										this._results.GetInaccessibleResult().Set(this._pCurrentSym, this._pCurrentType, this._pCurrentTypeArgs);
									}
									else if (flag7)
									{
										this._mpwiBogus.Set(this._pCurrentSym, this._pCurrentType, this._pCurrentTypeArgs);
									}
									else
									{
										this._methList.Add(new CandidateFunctionMember(new MethPropWithInst(this._pCurrentSym, this._pCurrentType, this._pCurrentTypeArgs), this._pCurrentParameters, 0, flag));
										if (this._pCurrentType.isInterfaceType())
										{
											TypeArray ifacesAll = this._pCurrentType.GetIfacesAll();
											for (int i = 0; i < ifacesAll.Count; i++)
											{
												AggregateType aggregateType = ifacesAll[i] as AggregateType;
												this._HiddenTypes.Add(aggregateType);
											}
											AggregateType predefindType = this.GetSymbolLoader().GetPredefindType(PredefinedType.PT_OBJECT);
											this._HiddenTypes.Add(predefindType);
										}
									}
									flag2 = false;
								}
							}
						}
					}
				}
				this._fCandidatesUnsupported = flag3 && flag4;
				if (this._bArgumentsChangedForNamedOrOptionalArguments)
				{
					this.CopyArgInfos(this._pOriginalArguments, this._pArguments);
				}
			}

			// Token: 0x060006FC RID: 1788 RVA: 0x00020E8C File Offset: 0x0001F08C
			private void CopyArgInfos(ArgInfos src, ArgInfos dst)
			{
				dst.carg = src.carg;
				dst.types = src.types;
				dst.fHasExprs = src.fHasExprs;
				dst.prgexpr.Clear();
				for (int i = 0; i < src.prgexpr.Count; i++)
				{
					dst.prgexpr.Add(src.prgexpr[i]);
				}
			}

			// Token: 0x060006FD RID: 1789 RVA: 0x00020EF8 File Offset: 0x0001F0F8
			private bool GetResultOfBind(bool bReportErrors)
			{
				if (!this._methList.IsEmpty<CandidateFunctionMember>())
				{
					CandidateFunctionMember candidateFunctionMember;
					if (this._methList.Count == 1)
					{
						candidateFunctionMember = this._methList.Head<CandidateFunctionMember>();
					}
					else
					{
						CandidateFunctionMember candidateFunctionMember2 = null;
						CandidateFunctionMember candidateFunctionMember3 = null;
						Expr optionalObject = this._pGroup.OptionalObject;
						CType ctype = ((optionalObject != null) ? optionalObject.Type : null);
						candidateFunctionMember = this._pExprBinder.FindBestMethod(this._methList, ctype, this._pArguments, out candidateFunctionMember2, out candidateFunctionMember3);
						if (candidateFunctionMember == null)
						{
							candidateFunctionMember = candidateFunctionMember2;
							this._results.AmbiguousResult = candidateFunctionMember3.mpwi;
							if (bReportErrors)
							{
								if (candidateFunctionMember2.@params != candidateFunctionMember3.@params || candidateFunctionMember2.mpwi.MethProp().Params.Count != candidateFunctionMember3.mpwi.MethProp().Params.Count || candidateFunctionMember2.mpwi.TypeArgs != candidateFunctionMember3.mpwi.TypeArgs || candidateFunctionMember2.mpwi.GetType() != candidateFunctionMember3.mpwi.GetType() || candidateFunctionMember2.mpwi.MethProp().Params == candidateFunctionMember3.mpwi.MethProp().Params)
								{
									throw this.GetErrorContext().Error(ErrorCode.ERR_AmbigCall, new ErrArg[] { candidateFunctionMember2.mpwi, candidateFunctionMember3.mpwi });
								}
								throw this.GetErrorContext().Error(ErrorCode.ERR_AmbigCall, new ErrArg[]
								{
									candidateFunctionMember2.mpwi.MethProp(),
									candidateFunctionMember3.mpwi.MethProp()
								});
							}
						}
					}
					this._results.BestResult = candidateFunctionMember.mpwi;
					if (bReportErrors)
					{
						this.ReportErrorsOnSuccess();
					}
					return true;
				}
				return false;
			}

			// Token: 0x060006FE RID: 1790 RVA: 0x000210A0 File Offset: 0x0001F2A0
			private bool ReOrderArgsForNamedArguments()
			{
				MethodOrPropertySymbol methodOrPropertySymbol = this.FindMostDerivedMethod(this._pCurrentSym, this._pGroup.OptionalObject);
				if (methodOrPropertySymbol == null)
				{
					return false;
				}
				int count = this._pCurrentParameters.Count;
				if (count == 0 || count < this._pArguments.carg)
				{
					return false;
				}
				if (!this.NamedArgumentNamesAppearInParameterList(methodOrPropertySymbol))
				{
					return false;
				}
				this._bArgumentsChangedForNamedOrOptionalArguments = ExpressionBinder.GroupToArgsBinder.ReOrderArgsForNamedArguments(methodOrPropertySymbol, this._pCurrentParameters, this._pCurrentType, this._pGroup, this._pArguments, this._pExprBinder.GetTypes(), this._pExprBinder.GetExprFactory(), this.GetSymbolLoader());
				return this._bArgumentsChangedForNamedOrOptionalArguments;
			}

			// Token: 0x060006FF RID: 1791 RVA: 0x0002113C File Offset: 0x0001F33C
			internal static bool ReOrderArgsForNamedArguments(MethodOrPropertySymbol methprop, TypeArray pCurrentParameters, AggregateType pCurrentType, ExprMemberGroup pGroup, ArgInfos pArguments, TypeManager typeManager, ExprFactory exprFactory, SymbolLoader symbolLoader)
			{
				int count = pCurrentParameters.Count;
				Expr[] array = new Expr[count];
				int num = 0;
				Expr expr = null;
				TypeArray typeArray = typeManager.SubstTypeArray(pCurrentParameters, pCurrentType, pGroup.TypeArgs);
				foreach (Name name in methprop.ParameterNames)
				{
					if (num >= pCurrentParameters.Count)
					{
						break;
					}
					ExprArrayInit exprArrayInit;
					if (methprop.isParamArray && num < pArguments.carg && (exprArrayInit = pArguments.prgexpr[num] as ExprArrayInit) != null && exprArrayInit.GeneratedForParamArray)
					{
						expr = pArguments.prgexpr[num];
					}
					ExprArrayInit exprArrayInit2;
					if (num < pArguments.carg && !(pArguments.prgexpr[num] is ExprNamedArgumentSpecification) && ((exprArrayInit2 = pArguments.prgexpr[num] as ExprArrayInit) == null || !exprArrayInit2.GeneratedForParamArray))
					{
						array[num] = pArguments.prgexpr[num++];
					}
					else
					{
						Expr expr2 = ExpressionBinder.GroupToArgsBinder.FindArgumentWithName(pArguments, name);
						if (expr2 == null)
						{
							if (methprop.IsParameterOptional(num))
							{
								expr2 = ExpressionBinder.GroupToArgsBinder.GenerateOptionalArgument(symbolLoader, exprFactory, methprop, typeArray[num], num);
							}
							else
							{
								if (expr == null || num != methprop.Params.Count - 1)
								{
									return false;
								}
								expr2 = expr;
							}
						}
						array[num++] = expr2;
					}
				}
				CType[] array2 = new CType[pCurrentParameters.Count];
				for (int i = 0; i < count; i++)
				{
					if (i < pArguments.prgexpr.Count)
					{
						pArguments.prgexpr[i] = array[i];
					}
					else
					{
						pArguments.prgexpr.Add(array[i]);
					}
					array2[i] = pArguments.prgexpr[i].Type;
				}
				pArguments.carg = pCurrentParameters.Count;
				pArguments.types = symbolLoader.getBSymmgr().AllocParams(pCurrentParameters.Count, array2);
				return true;
			}

			// Token: 0x06000700 RID: 1792 RVA: 0x00021350 File Offset: 0x0001F550
			private static Expr GenerateOptionalArgument(SymbolLoader symbolLoader, ExprFactory exprFactory, MethodOrPropertySymbol methprop, CType type, int index)
			{
				CType ctype = type.StripNubs();
				Expr expr;
				if (methprop.HasDefaultParameterValue(index))
				{
					CType defaultParameterValueConstValType = methprop.GetDefaultParameterValueConstValType(index);
					ConstVal defaultParameterValue = methprop.GetDefaultParameterValue(index);
					if (defaultParameterValueConstValType.isPredefType(PredefinedType.PT_DATETIME) && (ctype.isPredefType(PredefinedType.PT_DATETIME) || ctype.isPredefType(PredefinedType.PT_OBJECT) || ctype.isPredefType(PredefinedType.PT_VALUE)))
					{
						AggregateType predefindType = symbolLoader.GetPredefindType(PredefinedType.PT_DATETIME);
						expr = exprFactory.CreateConstant(predefindType, ConstVal.Get(DateTime.FromBinary(defaultParameterValue.Int64Val)));
					}
					else if (defaultParameterValueConstValType.isSimpleOrEnumOrString())
					{
						if (ctype.isEnumType() && defaultParameterValueConstValType == ctype.underlyingType())
						{
							expr = exprFactory.CreateConstant(ctype, defaultParameterValue);
						}
						else
						{
							expr = exprFactory.CreateConstant(defaultParameterValueConstValType, defaultParameterValue);
						}
					}
					else if ((type.IsRefType() || type is NullableType) && defaultParameterValue.IsNullRef)
					{
						expr = exprFactory.CreateNull();
					}
					else
					{
						expr = exprFactory.CreateZeroInit(type);
					}
				}
				else if (type.isPredefType(PredefinedType.PT_OBJECT))
				{
					if (methprop.MarshalAsObject(index))
					{
						expr = exprFactory.CreateNull();
					}
					else
					{
						AggregateSymbol predefAgg = symbolLoader.GetPredefAgg(PredefinedType.PT_MISSING);
						Name predefinedName = NameManager.GetPredefinedName(PredefinedName.PN_CAP_VALUE);
						FieldWithType fieldWithType = new FieldWithType(symbolLoader.LookupAggMember(predefinedName, predefAgg, symbmask_t.MASK_FieldSymbol) as FieldSymbol, predefAgg.getThisType());
						ExprField exprField = exprFactory.CreateField(predefAgg.getThisType(), null, fieldWithType, false);
						if (predefAgg.getThisType() != type)
						{
							expr = exprFactory.CreateCast(type, exprField);
						}
						else
						{
							expr = exprField;
						}
					}
				}
				else
				{
					expr = exprFactory.CreateZeroInit(type);
				}
				expr.IsOptionalArgument = true;
				return expr;
			}

			// Token: 0x06000701 RID: 1793 RVA: 0x000214D5 File Offset: 0x0001F6D5
			private MethodOrPropertySymbol FindMostDerivedMethod(MethodOrPropertySymbol pMethProp, Expr pObject)
			{
				return ExpressionBinder.GroupToArgsBinder.FindMostDerivedMethod(this.GetSymbolLoader(), pMethProp, (pObject != null) ? pObject.Type : null);
			}

			// Token: 0x06000702 RID: 1794 RVA: 0x000214F0 File Offset: 0x0001F6F0
			public static MethodOrPropertySymbol FindMostDerivedMethod(SymbolLoader symbolLoader, MethodOrPropertySymbol pMethProp, CType pType)
			{
				bool flag = false;
				MethodSymbol methodSymbol;
				if ((methodSymbol = pMethProp as MethodSymbol) == null)
				{
					PropertySymbol propertySymbol = (PropertySymbol)pMethProp;
					methodSymbol = propertySymbol.GetterMethod ?? propertySymbol.SetterMethod;
					if (methodSymbol == null)
					{
						return null;
					}
					flag = propertySymbol is IndexerSymbol;
				}
				if (!methodSymbol.isVirtual || pType == null)
				{
					return methodSymbol;
				}
				SymWithType swtSlot = methodSymbol.swtSlot;
				MethodSymbol methodSymbol2 = ((swtSlot != null) ? swtSlot.Meth() : null);
				if (methodSymbol2 != null)
				{
					methodSymbol = methodSymbol2;
				}
				AggregateType aggregateType;
				if ((aggregateType = pType as AggregateType) == null)
				{
					return methodSymbol;
				}
				AggregateSymbol aggregateSymbol = aggregateType.GetOwningAggregate();
				while (((aggregateSymbol != null) ? aggregateSymbol.GetBaseAgg() : null) != null)
				{
					MethodOrPropertySymbol methodOrPropertySymbol = symbolLoader.LookupAggMember(methodSymbol.name, aggregateSymbol, symbmask_t.MASK_MethodSymbol | symbmask_t.MASK_PropertySymbol) as MethodOrPropertySymbol;
					while (methodOrPropertySymbol != null)
					{
						if (methodOrPropertySymbol.isOverride && methodOrPropertySymbol.swtSlot.Sym != null && methodOrPropertySymbol.swtSlot.Sym == methodSymbol)
						{
							if (flag)
							{
								return ((MethodSymbol)methodOrPropertySymbol).getProperty();
							}
							return methodOrPropertySymbol;
						}
						else
						{
							methodOrPropertySymbol = symbolLoader.LookupNextSym(methodOrPropertySymbol, aggregateSymbol, symbmask_t.MASK_MethodSymbol | symbmask_t.MASK_PropertySymbol) as MethodOrPropertySymbol;
						}
					}
					aggregateSymbol = aggregateSymbol.GetBaseAgg();
				}
				return methodSymbol;
			}

			// Token: 0x06000703 RID: 1795 RVA: 0x00021600 File Offset: 0x0001F800
			private bool HasOptionalParameters()
			{
				MethodOrPropertySymbol methodOrPropertySymbol = this.FindMostDerivedMethod(this._pCurrentSym, this._pGroup.OptionalObject);
				return methodOrPropertySymbol != null && methodOrPropertySymbol.HasOptionalParameters();
			}

			// Token: 0x06000704 RID: 1796 RVA: 0x00021630 File Offset: 0x0001F830
			private bool AddArgumentsForOptionalParameters()
			{
				if (this._pCurrentParameters.Count <= this._pArguments.carg)
				{
					return true;
				}
				MethodOrPropertySymbol methodOrPropertySymbol = this.FindMostDerivedMethod(this._pCurrentSym, this._pGroup.OptionalObject);
				if (methodOrPropertySymbol == null)
				{
					return false;
				}
				int i = this._pArguments.carg;
				int num = 0;
				TypeArray typeArray = this._pExprBinder.GetTypes().SubstTypeArray(this._pCurrentParameters, this._pCurrentType, this._pGroup.TypeArgs);
				Expr[] array = new Expr[this._pCurrentParameters.Count - i];
				while (i < typeArray.Count)
				{
					if (!methodOrPropertySymbol.IsParameterOptional(i))
					{
						return false;
					}
					array[num] = ExpressionBinder.GroupToArgsBinder.GenerateOptionalArgument(this.GetSymbolLoader(), this._pExprBinder.GetExprFactory(), methodOrPropertySymbol, typeArray[i], i);
					i++;
					num++;
				}
				for (int j = 0; j < num; j++)
				{
					this._pArguments.prgexpr.Add(array[j]);
				}
				CType[] array2 = new CType[typeArray.Count];
				for (int k = 0; k < typeArray.Count; k++)
				{
					array2[k] = this._pArguments.prgexpr[k].Type;
				}
				this._pArguments.types = this.GetSymbolLoader().getBSymmgr().AllocParams(typeArray.Count, array2);
				this._pArguments.carg = typeArray.Count;
				this._bArgumentsChangedForNamedOrOptionalArguments = true;
				return true;
			}

			// Token: 0x06000705 RID: 1797 RVA: 0x000217A0 File Offset: 0x0001F9A0
			private static Expr FindArgumentWithName(ArgInfos pArguments, Name pName)
			{
				List<Expr> prgexpr = pArguments.prgexpr;
				for (int i = 0; i < pArguments.carg; i++)
				{
					Expr expr = prgexpr[i];
					ExprNamedArgumentSpecification exprNamedArgumentSpecification;
					if ((exprNamedArgumentSpecification = expr as ExprNamedArgumentSpecification) != null && exprNamedArgumentSpecification.Name == pName)
					{
						return expr;
					}
				}
				return null;
			}

			// Token: 0x06000706 RID: 1798 RVA: 0x000217E4 File Offset: 0x0001F9E4
			private bool NamedArgumentNamesAppearInParameterList(MethodOrPropertySymbol methprop)
			{
				List<Name> list = methprop.ParameterNames;
				HashSet<Name> hashSet = new HashSet<Name>();
				for (int i = 0; i < this._pArguments.carg; i++)
				{
					ExprNamedArgumentSpecification exprNamedArgumentSpecification;
					if ((exprNamedArgumentSpecification = this._pArguments.prgexpr[i] as ExprNamedArgumentSpecification) == null)
					{
						if (!list.IsEmpty<Name>())
						{
							list = list.Tail<Name>();
						}
					}
					else
					{
						Name name = exprNamedArgumentSpecification.Name;
						if (!methprop.ParameterNames.Contains(name))
						{
							if (this._pInvalidSpecifiedName == null)
							{
								this._pInvalidSpecifiedName = name;
							}
							return false;
						}
						if (!list.Contains(name))
						{
							if (this._pNameUsedInPositionalArgument == null)
							{
								this._pNameUsedInPositionalArgument = name;
							}
							return false;
						}
						if (!hashSet.Add(name))
						{
							if (this._pDuplicateSpecifiedName == null)
							{
								this._pDuplicateSpecifiedName = name;
							}
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x06000707 RID: 1799 RVA: 0x000218A8 File Offset: 0x0001FAA8
			private bool GetNextSym(CMemberLookupResults.CMethodIterator iterator)
			{
				if (!iterator.MoveNext(this._methList.IsEmpty<CandidateFunctionMember>()))
				{
					return false;
				}
				this._pCurrentSym = iterator.GetCurrentSymbol();
				AggregateType currentType = iterator.GetCurrentType();
				if (this._pCurrentType != currentType && this._pCurrentType != null && !this._methList.IsEmpty<CandidateFunctionMember>() && !this._methList.Head<CandidateFunctionMember>().mpwi.GetType().isInterfaceType())
				{
					return false;
				}
				this._pCurrentType = currentType;
				while (this._HiddenTypes.Contains(this._pCurrentType))
				{
					while (iterator.GetCurrentType() == this._pCurrentType)
					{
						iterator.MoveNext(this._methList.IsEmpty<CandidateFunctionMember>());
					}
					this._pCurrentSym = iterator.GetCurrentSymbol();
					this._pCurrentType = iterator.GetCurrentType();
					if (iterator.AtEnd())
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06000708 RID: 1800 RVA: 0x00021978 File Offset: 0x0001FB78
			private bool ConstructExpandedParameters()
			{
				if (this._pCurrentSym == null || this._pArguments == null || this._pCurrentParameters == null)
				{
					return false;
				}
				if ((this._fBindFlags & BindingFlag.BIND_NOPARAMS) != (BindingFlag)0)
				{
					return false;
				}
				if (!this._pCurrentSym.isParamArray)
				{
					return false;
				}
				int num = 0;
				for (int i = this._pArguments.carg; i < this._pCurrentSym.Params.Count; i++)
				{
					if (this._pCurrentSym.IsParameterOptional(i))
					{
						num++;
					}
				}
				return this._pArguments.carg + num >= this._pCurrentParameters.Count - 1 && this._pExprBinder.TryGetExpandedParams(this._pCurrentSym.Params, this._pArguments.carg, out this._pCurrentParameters);
			}

			// Token: 0x06000709 RID: 1801 RVA: 0x00021A3C File Offset: 0x0001FC3C
			private ExpressionBinder.GroupToArgsBinder.Result DetermineCurrentTypeArgs()
			{
				TypeArray typeArgs = this._pGroup.TypeArgs;
				MethodSymbol methodSymbol;
				if ((methodSymbol = this._pCurrentSym as MethodSymbol) != null && methodSymbol.typeVars.Count != typeArgs.Count)
				{
					if (typeArgs.Count > 0)
					{
						if (!this._mwtBadArity)
						{
							this._mwtBadArity.Set(methodSymbol, this._pCurrentType);
						}
						return ExpressionBinder.GroupToArgsBinder.Result.Failure_NoSearchForExpanded;
					}
					if (!MethodTypeInferrer.Infer(this._pExprBinder, this.GetSymbolLoader(), methodSymbol, this._pCurrentType.GetTypeArgsAll(), this._pCurrentParameters, this._pArguments, out this._pCurrentTypeArgs))
					{
						if (this._results.IsBetterUninferableResult(this._pCurrentTypeArgs))
						{
							TypeArray typeVars = methodSymbol.typeVars;
							if (typeVars != null && this._pCurrentTypeArgs != null && typeVars.Count == this._pCurrentTypeArgs.Count)
							{
								this._mpwiCantInferInstArg.Set(methodSymbol, this._pCurrentType, this._pCurrentTypeArgs);
							}
							else
							{
								this._mpwiCantInferInstArg.Set(methodSymbol, this._pCurrentType, typeVars);
							}
						}
						return ExpressionBinder.GroupToArgsBinder.Result.Failure_SearchForExpanded;
					}
				}
				else
				{
					this._pCurrentTypeArgs = typeArgs;
				}
				return ExpressionBinder.GroupToArgsBinder.Result.Success;
			}

			// Token: 0x0600070A RID: 1802 RVA: 0x00021B48 File Offset: 0x0001FD48
			private bool ArgumentsAreConvertible()
			{
				bool flag = false;
				if (this._pArguments.carg != 0)
				{
					this.UpdateArguments();
					for (int i = 0; i < this._pArguments.carg; i++)
					{
						CType ctype = this._pCurrentParameters[i];
						if (!TypeBind.CheckConstraints(this.GetSemanticChecker(), this.GetErrorContext(), ctype, CheckConstraintsFlags.NoErrors) && !this.DoesTypeArgumentsContainErrorSym(ctype))
						{
							this._mpwiParamTypeConstraints.Set(this._pCurrentSym, this._pCurrentType, this._pCurrentTypeArgs);
							return false;
						}
					}
					for (int j = 0; j < this._pArguments.carg; j++)
					{
						CType ctype2 = this._pCurrentParameters[j];
						flag |= this.DoesTypeArgumentsContainErrorSym(ctype2);
						bool flag2;
						if (this._pArguments.fHasExprs)
						{
							Expr expr = this._pArguments.prgexpr[j];
							ExprNamedArgumentSpecification exprNamedArgumentSpecification;
							if ((exprNamedArgumentSpecification = expr as ExprNamedArgumentSpecification) != null)
							{
								expr = exprNamedArgumentSpecification.Value;
							}
							flag2 = this._pExprBinder.canConvert(expr, ctype2);
						}
						else
						{
							flag2 = this._pExprBinder.canConvert(this._pArguments.types[j], ctype2);
						}
						if (!flag2 && !flag)
						{
							if (j > this._nArgBest)
							{
								this._nArgBest = j;
								if (!this._results.GetBestResult())
								{
									this._results.GetBestResult().Set(this._pCurrentSym, this._pCurrentType, this._pCurrentTypeArgs);
									this._pBestParameters = this._pCurrentParameters;
								}
							}
							else if (j == this._nArgBest && this._pArguments.types[j] != ctype2)
							{
								ParameterModifierType parameterModifierType;
								CType ctype3 = (((parameterModifierType = this._pArguments.types[j] as ParameterModifierType) != null) ? parameterModifierType.GetParameterType() : this._pArguments.types[j]);
								ParameterModifierType parameterModifierType2;
								CType ctype4 = (((parameterModifierType2 = ctype2 as ParameterModifierType) != null) ? parameterModifierType2.GetParameterType() : ctype2);
								if (ctype3 == ctype4 && !this._results.GetBestResult())
								{
									this._results.GetBestResult().Set(this._pCurrentSym, this._pCurrentType, this._pCurrentTypeArgs);
									this._pBestParameters = this._pCurrentParameters;
								}
							}
							MethodSymbol methodSymbol;
							if ((methodSymbol = this._pCurrentSym as MethodSymbol) != null)
							{
								this._results.AddInconvertibleResult(methodSymbol, this._pCurrentType, this._pCurrentTypeArgs);
							}
							return false;
						}
					}
				}
				MethodSymbol methodSymbol3;
				if (flag)
				{
					MethodSymbol methodSymbol2;
					if (this._results.IsBetterUninferableResult(this._pCurrentTypeArgs) && (methodSymbol2 = this._pCurrentSym as MethodSymbol) != null)
					{
						this._results.GetUninferableResult().Set(methodSymbol2, this._pCurrentType, this._pCurrentTypeArgs);
					}
				}
				else if ((methodSymbol3 = this._pCurrentSym as MethodSymbol) != null)
				{
					this._results.AddInconvertibleResult(methodSymbol3, this._pCurrentType, this._pCurrentTypeArgs);
				}
				return !flag;
			}

			// Token: 0x0600070B RID: 1803 RVA: 0x00021E28 File Offset: 0x00020028
			private void UpdateArguments()
			{
				this._pCurrentParameters = this._pExprBinder.GetTypes().SubstTypeArray(this._pCurrentParameters, this._pCurrentType, this._pCurrentTypeArgs);
				if (this._pArguments.prgexpr == null || this._pArguments.prgexpr.Count == 0)
				{
					return;
				}
				MethodOrPropertySymbol methodOrPropertySymbol = null;
				for (int i = 0; i < this._pCurrentParameters.Count; i++)
				{
					Expr expr = this._pArguments.prgexpr[i];
					if (expr.IsOptionalArgument && this._pCurrentParameters[i] != expr.Type)
					{
						if (methodOrPropertySymbol == null)
						{
							methodOrPropertySymbol = this.FindMostDerivedMethod(this._pCurrentSym, this._pGroup.OptionalObject);
						}
						Expr expr2 = ExpressionBinder.GroupToArgsBinder.GenerateOptionalArgument(this.GetSymbolLoader(), this._pExprBinder.GetExprFactory(), methodOrPropertySymbol, this._pCurrentParameters[i], i);
						this._pArguments.prgexpr[i] = expr2;
					}
				}
			}

			// Token: 0x0600070C RID: 1804 RVA: 0x00021F1C File Offset: 0x0002011C
			private bool DoesTypeArgumentsContainErrorSym(CType var)
			{
				AggregateType aggregateType;
				if ((aggregateType = var as AggregateType) == null)
				{
					return false;
				}
				TypeArray typeArgsAll = aggregateType.GetTypeArgsAll();
				for (int i = 0; i < typeArgsAll.Count; i++)
				{
					CType ctype = typeArgsAll[i];
					if (ctype is ErrorType)
					{
						return true;
					}
					if (ctype is AggregateType && this.DoesTypeArgumentsContainErrorSym(ctype))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600070D RID: 1805 RVA: 0x00021F74 File Offset: 0x00020174
			private void ReportErrorsOnSuccess()
			{
				if (this._pGroup.SymKind == SYMKIND.SK_MethodSymbol && this._results.GetBestResult().TypeArgs.Count > 0)
				{
					TypeBind.CheckMethConstraints(this.GetSemanticChecker(), this.GetErrorContext(), new MethWithInst(this._results.GetBestResult()));
				}
			}

			// Token: 0x0600070E RID: 1806 RVA: 0x00021FC8 File Offset: 0x000201C8
			private Exception ReportErrorsOnFailure()
			{
				if (this._pDuplicateSpecifiedName != null)
				{
					return this.GetErrorContext().Error(ErrorCode.ERR_DuplicateNamedArgument, new ErrArg[] { this._pDuplicateSpecifiedName });
				}
				if (this._results.GetInaccessibleResult())
				{
					return this.GetSemanticChecker().ReportAccessError(this._results.GetInaccessibleResult(), this._pExprBinder.ContextForMemberLookup(), ExpressionBinder.GroupToArgsBinder.GetTypeQualifier(this._pGroup));
				}
				if (this._mpwiBogus)
				{
					return this.GetErrorContext().Error(ErrorCode.ERR_BindToBogus, new ErrArg[] { this._mpwiBogus });
				}
				bool flag = false;
				Name name = this._pGroup.Name;
				if (this._pGroup.OptionalObject != null && this._pGroup.OptionalObject.Type != null && this._pGroup.OptionalObject.Type.isDelegateType() && this._pGroup.Name == NameManager.GetPredefinedName(PredefinedName.PN_INVOKE))
				{
					flag = true;
					name = this._pGroup.OptionalObject.Type.getAggregate().name;
				}
				if (this._results.GetBestResult())
				{
					return this.ReportErrorsForBestMatching(flag, name);
				}
				if (this._results.GetUninferableResult() || this._mpwiCantInferInstArg)
				{
					if (!this._results.GetUninferableResult())
					{
						this._results.GetUninferableResult().Set(this._mpwiCantInferInstArg.Sym as MethodSymbol, this._mpwiCantInferInstArg.GetType(), this._mpwiCantInferInstArg.TypeArgs);
					}
					MethWithType methWithType = new MethWithType();
					methWithType.Set(this._results.GetUninferableResult().Meth(), this._results.GetUninferableResult().GetType());
					return this.GetErrorContext().Error(ErrorCode.ERR_CantInferMethTypeArgs, new ErrArg[] { methWithType });
				}
				if (this._mwtBadArity)
				{
					int count = this._mwtBadArity.Meth().typeVars.Count;
					return this.GetErrorContext().Error((count > 0) ? ErrorCode.ERR_BadArity : ErrorCode.ERR_HasNoTypeVars, new ErrArg[]
					{
						this._mwtBadArity,
						new ErrArgSymKind(this._mwtBadArity.Meth()),
						this._pArguments.carg
					});
				}
				if (this._mpwiParamTypeConstraints)
				{
					TypeBind.CheckMethConstraints(this.GetSemanticChecker(), this.GetErrorContext(), new MethWithInst(this._mpwiParamTypeConstraints));
					return null;
				}
				if (this._pInvalidSpecifiedName != null)
				{
					AggregateType aggregateType;
					if (this._pGroup.OptionalObject == null || (aggregateType = this._pGroup.OptionalObject.Type as AggregateType) == null || !aggregateType.GetOwningAggregate().IsDelegate())
					{
						return this.GetErrorContext().Error(ErrorCode.ERR_BadNamedArgument, new ErrArg[]
						{
							this._pGroup.Name,
							this._pInvalidSpecifiedName
						});
					}
					return this.GetErrorContext().Error(ErrorCode.ERR_BadNamedArgumentForDelegateInvoke, new ErrArg[]
					{
						aggregateType.GetOwningAggregate().name,
						this._pInvalidSpecifiedName
					});
				}
				else
				{
					if (this._pNameUsedInPositionalArgument != null)
					{
						return this.GetErrorContext().Error(ErrorCode.ERR_NamedArgumentUsedInPositional, new ErrArg[] { this._pNameUsedInPositionalArgument });
					}
					if (this._pDelegate != null)
					{
						return this.GetErrorContext().Error(ErrorCode.ERR_MethDelegateMismatch, new ErrArg[] { name, this._pDelegate });
					}
					if (this._fCandidatesUnsupported)
					{
						return this.GetErrorContext().Error(ErrorCode.ERR_BindToBogus, new ErrArg[] { name });
					}
					if (flag)
					{
						return this.GetErrorContext().Error(ErrorCode.ERR_BadDelArgCount, new ErrArg[]
						{
							name,
							this._pArguments.carg
						});
					}
					if ((this._pGroup.Flags & EXPRFLAG.EXF_CTOR) != (EXPRFLAG)0)
					{
						return this.GetErrorContext().Error(ErrorCode.ERR_BadCtorArgCount, new ErrArg[]
						{
							this._pGroup.ParentType,
							this._pArguments.carg
						});
					}
					return this.GetErrorContext().Error(ErrorCode.ERR_BadArgCount, new ErrArg[]
					{
						name,
						this._pArguments.carg
					});
				}
			}

			// Token: 0x0600070F RID: 1807 RVA: 0x0002244C File Offset: 0x0002064C
			private RuntimeBinderException ReportErrorsForBestMatching(bool bUseDelegateErrors, Name nameErr)
			{
				if (this._pDelegate != null)
				{
					return this.GetErrorContext().Error(ErrorCode.ERR_MethDelegateMismatch, new ErrArg[]
					{
						nameErr,
						this._pDelegate,
						this._results.GetBestResult()
					});
				}
				if (bUseDelegateErrors)
				{
					return this.GetErrorContext().Error(ErrorCode.ERR_BadDelArgTypes, new ErrArg[] { this._results.GetBestResult().GetType() });
				}
				return this.GetErrorContext().Error(ErrorCode.ERR_BadArgTypes, new ErrArg[] { this._results.GetBestResult() });
			}

			// Token: 0x040006B8 RID: 1720
			private readonly ExpressionBinder _pExprBinder;

			// Token: 0x040006B9 RID: 1721
			private bool _fCandidatesUnsupported;

			// Token: 0x040006BA RID: 1722
			private readonly BindingFlag _fBindFlags;

			// Token: 0x040006BB RID: 1723
			private readonly ExprMemberGroup _pGroup;

			// Token: 0x040006BC RID: 1724
			private readonly ArgInfos _pArguments;

			// Token: 0x040006BD RID: 1725
			private readonly ArgInfos _pOriginalArguments;

			// Token: 0x040006BE RID: 1726
			private readonly bool _bHasNamedArguments;

			// Token: 0x040006BF RID: 1727
			private readonly AggregateType _pDelegate;

			// Token: 0x040006C0 RID: 1728
			private AggregateType _pCurrentType;

			// Token: 0x040006C1 RID: 1729
			private MethodOrPropertySymbol _pCurrentSym;

			// Token: 0x040006C2 RID: 1730
			private TypeArray _pCurrentTypeArgs;

			// Token: 0x040006C3 RID: 1731
			private TypeArray _pCurrentParameters;

			// Token: 0x040006C4 RID: 1732
			private TypeArray _pBestParameters;

			// Token: 0x040006C5 RID: 1733
			private int _nArgBest;

			// Token: 0x040006C6 RID: 1734
			private readonly ExpressionBinder.GroupToArgsBinderResult _results;

			// Token: 0x040006C7 RID: 1735
			private readonly List<CandidateFunctionMember> _methList;

			// Token: 0x040006C8 RID: 1736
			private readonly MethPropWithInst _mpwiParamTypeConstraints;

			// Token: 0x040006C9 RID: 1737
			private readonly MethPropWithInst _mpwiBogus;

			// Token: 0x040006CA RID: 1738
			private readonly MethPropWithInst _mpwiCantInferInstArg;

			// Token: 0x040006CB RID: 1739
			private readonly MethWithType _mwtBadArity;

			// Token: 0x040006CC RID: 1740
			private Name _pInvalidSpecifiedName;

			// Token: 0x040006CD RID: 1741
			private Name _pNameUsedInPositionalArgument;

			// Token: 0x040006CE RID: 1742
			private Name _pDuplicateSpecifiedName;

			// Token: 0x040006CF RID: 1743
			private readonly List<CType> _HiddenTypes;

			// Token: 0x040006D0 RID: 1744
			private bool _bArgumentsChangedForNamedOrOptionalArguments;

			// Token: 0x020000FF RID: 255
			private enum Result
			{
				// Token: 0x04000731 RID: 1841
				Success,
				// Token: 0x04000732 RID: 1842
				Failure_SearchForExpanded,
				// Token: 0x04000733 RID: 1843
				Failure_NoSearchForExpanded
			}
		}

		// Token: 0x020000E4 RID: 228
		internal sealed class GroupToArgsBinderResult
		{
			// Token: 0x06000710 RID: 1808 RVA: 0x000224FA File Offset: 0x000206FA
			public MethPropWithInst GetBestResult()
			{
				return this.BestResult;
			}

			// Token: 0x06000711 RID: 1809 RVA: 0x00022502 File Offset: 0x00020702
			public MethPropWithInst GetAmbiguousResult()
			{
				return this.AmbiguousResult;
			}

			// Token: 0x06000712 RID: 1810 RVA: 0x0002250A File Offset: 0x0002070A
			public MethPropWithInst GetInaccessibleResult()
			{
				return this.InaccessibleResult;
			}

			// Token: 0x06000713 RID: 1811 RVA: 0x00022512 File Offset: 0x00020712
			public MethPropWithInst GetUninferableResult()
			{
				return this.UninferableResult;
			}

			// Token: 0x06000714 RID: 1812 RVA: 0x0002251C File Offset: 0x0002071C
			public GroupToArgsBinderResult()
			{
				this.BestResult = new MethPropWithInst();
				this.AmbiguousResult = new MethPropWithInst();
				this.InaccessibleResult = new MethPropWithInst();
				this.UninferableResult = new MethPropWithInst();
				this.InconvertibleResult = new MethPropWithInst();
				this._inconvertibleResults = new List<MethPropWithInst>();
			}

			// Token: 0x06000715 RID: 1813 RVA: 0x00022571 File Offset: 0x00020771
			public void AddInconvertibleResult(MethodSymbol method, AggregateType currentType, TypeArray currentTypeArgs)
			{
				if (this.InconvertibleResult.Sym == null)
				{
					this.InconvertibleResult.Set(method, currentType, currentTypeArgs);
				}
				this._inconvertibleResults.Add(new MethPropWithInst(method, currentType, currentTypeArgs));
			}

			// Token: 0x06000716 RID: 1814 RVA: 0x000225A4 File Offset: 0x000207A4
			private static int NumberOfErrorTypes(TypeArray pTypeArgs)
			{
				int num = 0;
				for (int i = 0; i < pTypeArgs.Count; i++)
				{
					if (pTypeArgs[i] is ErrorType)
					{
						num++;
					}
				}
				return num;
			}

			// Token: 0x06000717 RID: 1815 RVA: 0x000225D8 File Offset: 0x000207D8
			private static bool IsBetterThanCurrent(TypeArray pTypeArgs1, TypeArray pTypeArgs2)
			{
				int num = ExpressionBinder.GroupToArgsBinderResult.NumberOfErrorTypes(pTypeArgs1);
				int num2 = ExpressionBinder.GroupToArgsBinderResult.NumberOfErrorTypes(pTypeArgs2);
				if (num == num2)
				{
					int num3 = ((pTypeArgs1.Count > pTypeArgs2.Count) ? pTypeArgs2.Count : pTypeArgs1.Count);
					for (int i = 0; i < num3; i++)
					{
						AggregateType aggregateType;
						if ((aggregateType = pTypeArgs1[i] as AggregateType) != null)
						{
							num += ExpressionBinder.GroupToArgsBinderResult.NumberOfErrorTypes(aggregateType.GetTypeArgsAll());
						}
						AggregateType aggregateType2;
						if ((aggregateType2 = pTypeArgs2[i] as AggregateType) != null)
						{
							num2 += ExpressionBinder.GroupToArgsBinderResult.NumberOfErrorTypes(aggregateType2.GetTypeArgsAll());
						}
					}
				}
				return num2 < num;
			}

			// Token: 0x06000718 RID: 1816 RVA: 0x00022664 File Offset: 0x00020864
			public bool IsBetterUninferableResult(TypeArray pTypeArguments)
			{
				return this.UninferableResult.Sym == null || (pTypeArguments != null && ExpressionBinder.GroupToArgsBinderResult.IsBetterThanCurrent(this.UninferableResult.TypeArgs, pTypeArguments));
			}

			// Token: 0x040006D1 RID: 1745
			public MethPropWithInst BestResult;

			// Token: 0x040006D2 RID: 1746
			public MethPropWithInst AmbiguousResult;

			// Token: 0x040006D3 RID: 1747
			private MethPropWithInst InaccessibleResult;

			// Token: 0x040006D4 RID: 1748
			private MethPropWithInst UninferableResult;

			// Token: 0x040006D5 RID: 1749
			private MethPropWithInst InconvertibleResult;

			// Token: 0x040006D6 RID: 1750
			private readonly List<MethPropWithInst> _inconvertibleResults;
		}

		// Token: 0x020000E5 RID: 229
		private sealed class ImplicitConversion
		{
			// Token: 0x06000719 RID: 1817 RVA: 0x0002268C File Offset: 0x0002088C
			public ImplicitConversion(ExpressionBinder binder, Expr exprSrc, CType typeSrc, ExprClass typeDest, bool needsExprDest, CONVERTTYPE flags)
			{
				this._binder = binder;
				this._exprSrc = exprSrc;
				this._typeSrc = typeSrc;
				this._typeDest = typeDest.Type;
				this._exprTypeDest = typeDest;
				this._needsExprDest = needsExprDest;
				this._flags = flags;
				this._exprDest = null;
			}

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x0600071A RID: 1818 RVA: 0x000226E0 File Offset: 0x000208E0
			public Expr ExprDest
			{
				get
				{
					return this._exprDest;
				}
			}

			// Token: 0x0600071B RID: 1819 RVA: 0x000226E8 File Offset: 0x000208E8
			public bool Bind()
			{
				if (this._typeSrc == null || this._typeDest == null || this._typeDest.IsNeverSameType())
				{
					return false;
				}
				switch (this._typeDest.GetTypeKind())
				{
				case TypeKind.TK_VoidType:
					return false;
				case TypeKind.TK_NullType:
					if (!(this._typeSrc is NullType))
					{
						return false;
					}
					if (this._needsExprDest)
					{
						this._exprDest = this._exprSrc;
					}
					return true;
				case TypeKind.TK_MethodGroupType:
					return false;
				case TypeKind.TK_ErrorType:
					if (this._typeSrc != this._typeDest)
					{
						return false;
					}
					if (this._needsExprDest)
					{
						this._exprDest = this._exprSrc;
					}
					return true;
				case TypeKind.TK_ArgumentListType:
					return this._typeSrc == this._typeDest;
				default:
				{
					if (this._typeSrc is ErrorType)
					{
						return false;
					}
					if (this._typeSrc == this._typeDest && ((this._flags & CONVERTTYPE.ISEXPLICIT) == (CONVERTTYPE)0 || (!this._typeSrc.isPredefType(PredefinedType.PT_FLOAT) && !this._typeSrc.isPredefType(PredefinedType.PT_DOUBLE))))
					{
						if (this._needsExprDest)
						{
							this._exprDest = this._exprSrc;
						}
						return true;
					}
					NullableType nullableType;
					if ((nullableType = this._typeDest as NullableType) != null)
					{
						return this.BindNubConversion(nullableType);
					}
					NullableType nullableType2;
					if ((nullableType2 = this._typeSrc as NullableType) != null)
					{
						return this.bindImplicitConversionFromNullable(nullableType2);
					}
					if ((this._flags & CONVERTTYPE.ISEXPLICIT) != (CONVERTTYPE)0)
					{
						this._flags |= CONVERTTYPE.NOUDC;
					}
					this._typeDest.fundType();
					switch (this._typeSrc.GetTypeKind())
					{
					case TypeKind.TK_AggregateType:
						if (this.bindImplicitConversionFromAgg(this._typeSrc as AggregateType))
						{
							return true;
						}
						break;
					case TypeKind.TK_VoidType:
					case TypeKind.TK_ErrorType:
					case TypeKind.TK_ArgumentListType:
					case TypeKind.TK_ParameterModifierType:
						return false;
					case TypeKind.TK_NullType:
						if (this.bindImplicitConversionFromNull())
						{
							return true;
						}
						break;
					case TypeKind.TK_ArrayType:
						if (this.bindImplicitConversionFromArray())
						{
							return true;
						}
						break;
					case TypeKind.TK_PointerType:
						if (this.bindImplicitConversionFromPointer())
						{
							return true;
						}
						break;
					}
					Expr exprSrc = this._exprSrc;
					object obj = ((exprSrc != null) ? exprSrc.RuntimeObject : null);
					if (obj != null && this._typeDest.AssociatedSystemType.IsInstanceOfType(obj) && this._binder.GetSemanticChecker().CheckTypeAccess(this._typeDest, this._binder.Context.ContextForMemberLookup))
					{
						if (this._needsExprDest)
						{
							this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest, this._exprSrc.Flags & EXPRFLAG.EXF_CANTBENULL);
						}
						return true;
					}
					return (this._flags & CONVERTTYPE.NOUDC) == (CONVERTTYPE)0 && this._binder.bindUserDefinedConversion(this._exprSrc, this._typeSrc, this._typeDest, this._needsExprDest, out this._exprDest, true);
				}
				}
			}

			// Token: 0x0600071C RID: 1820 RVA: 0x00022974 File Offset: 0x00020B74
			private bool BindNubConversion(NullableType nubDst)
			{
				nubDst.GetAts();
				if (this.GetSymbolLoader().HasBaseConversion(nubDst.GetUnderlyingType(), this._typeSrc) && !CConversions.FWrappingConv(this._typeSrc, nubDst))
				{
					if ((this._flags & CONVERTTYPE.ISEXPLICIT) == (CONVERTTYPE)0)
					{
						return false;
					}
					if (this._needsExprDest)
					{
						this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest, EXPRFLAG.EXF_INDEXER);
					}
					return true;
				}
				else
				{
					bool flag;
					CType ctype = nubDst.StripNubs(out flag);
					ExprClass exprClass = this.GetExprFactory().CreateClass(ctype);
					bool flag2;
					CType ctype2 = this._typeSrc.StripNubs(out flag2);
					ExpressionBinder.ConversionFunc conversionFunc = (((this._flags & CONVERTTYPE.ISEXPLICIT) != (CONVERTTYPE)0) ? new ExpressionBinder.ConversionFunc(this._binder.BindExplicitConversion) : new ExpressionBinder.ConversionFunc(this._binder.BindImplicitConversion));
					if (!flag2)
					{
						if (this._typeSrc is NullType)
						{
							if (this._needsExprDest)
							{
								if (this._exprSrc.isCONSTANT_OK())
								{
									this._exprDest = this.GetExprFactory().CreateZeroInit(nubDst);
								}
								else
								{
									this._exprDest = this.GetExprFactory().CreateCast(this._typeDest, this._exprSrc);
								}
							}
							return true;
						}
						Expr expr = this._exprSrc;
						if (this._typeSrc == ctype || conversionFunc(this._exprSrc, this._typeSrc, exprClass, nubDst, this._needsExprDest, out expr, this._flags | CONVERTTYPE.NOUDC))
						{
							if (this._needsExprDest)
							{
								ExprUserDefinedConversion exprUserDefinedConversion = expr as ExprUserDefinedConversion;
								if (exprUserDefinedConversion != null)
								{
									expr = exprUserDefinedConversion.UserDefinedCall;
								}
								if (flag)
								{
									(expr = this._binder.BindNubNew(expr)).NullableCallLiftKind = NullableCallLiftKind.NullableConversionConstructor;
								}
								if (exprUserDefinedConversion != null)
								{
									exprUserDefinedConversion.UserDefinedCall = expr;
									expr = exprUserDefinedConversion;
								}
								this._exprDest = expr;
							}
							return true;
						}
						return (this._flags & CONVERTTYPE.NOUDC) == (CONVERTTYPE)0 && this._binder.bindUserDefinedConversion(this._exprSrc, this._typeSrc, nubDst, this._needsExprDest, out this._exprDest, (this._flags & CONVERTTYPE.ISEXPLICIT) == (CONVERTTYPE)0);
					}
					else
					{
						if (ctype2 != ctype && !conversionFunc(null, ctype2, exprClass, nubDst, false, out this._exprDest, this._flags | CONVERTTYPE.NOUDC))
						{
							return (this._flags & CONVERTTYPE.NOUDC) == (CONVERTTYPE)0 && this._binder.bindUserDefinedConversion(this._exprSrc, this._typeSrc, nubDst, this._needsExprDest, out this._exprDest, (this._flags & CONVERTTYPE.ISEXPLICIT) == (CONVERTTYPE)0);
						}
						if (this._needsExprDest)
						{
							MethWithInst methWithInst = new MethWithInst(null, null);
							ExprMemberGroup exprMemberGroup = this.GetExprFactory().CreateMemGroup(null, methWithInst);
							ExprCall exprCall = this.GetExprFactory().CreateCall((EXPRFLAG)0, nubDst, this._exprSrc, exprMemberGroup, null);
							Expr expr2 = this._binder.mustCast(this._exprSrc, ctype2);
							ExprClass exprClass2 = this.GetExprFactory().CreateClass(ctype);
							if (!(((this._flags & CONVERTTYPE.ISEXPLICIT) != (CONVERTTYPE)0) ? this._binder.BindExplicitConversion(expr2, expr2.Type, exprClass2, ctype, out expr2, this._flags | CONVERTTYPE.NOUDC) : this._binder.BindImplicitConversion(expr2, expr2.Type, exprClass2, ctype, out expr2, this._flags | CONVERTTYPE.NOUDC)))
							{
								return false;
							}
							exprCall.CastOfNonLiftedResultToLiftedType = this._binder.mustCast(expr2, nubDst, (CONVERTTYPE)0);
							exprCall.NullableCallLiftKind = NullableCallLiftKind.NullableConversion;
							exprCall.PConversions = exprCall.CastOfNonLiftedResultToLiftedType;
							this._exprDest = exprCall;
						}
						return true;
					}
				}
			}

			// Token: 0x0600071D RID: 1821 RVA: 0x00022CA0 File Offset: 0x00020EA0
			private bool bindImplicitConversionFromNull()
			{
				FUNDTYPE fundtype = this._typeDest.fundType();
				if (fundtype != FUNDTYPE.FT_REF && fundtype != FUNDTYPE.FT_PTR && !this._typeDest.isPredefType(PredefinedType.PT_G_OPTIONAL))
				{
					return false;
				}
				if (this._needsExprDest)
				{
					if (this._exprSrc.isCONSTANT_OK())
					{
						this._exprDest = this.GetExprFactory().CreateZeroInit(this._typeDest);
					}
					else
					{
						this._exprDest = this.GetExprFactory().CreateCast(this._typeDest, this._exprSrc);
					}
				}
				return true;
			}

			// Token: 0x0600071E RID: 1822 RVA: 0x00022D20 File Offset: 0x00020F20
			private bool bindImplicitConversionFromNullable(NullableType nubSrc)
			{
				if (nubSrc.GetAts() == this._typeDest)
				{
					if (this._needsExprDest)
					{
						this._exprDest = this._exprSrc;
					}
					return true;
				}
				if (this.GetSymbolLoader().HasBaseConversion(nubSrc.GetUnderlyingType(), this._typeDest) && !CConversions.FUnwrappingConv(nubSrc, this._typeDest))
				{
					if (this._needsExprDest)
					{
						this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest, EXPRFLAG.EXF_CTOR);
						if (!this._typeDest.isPredefType(PredefinedType.PT_OBJECT))
						{
							this._binder.bindSimpleCast(this._exprDest, this._exprTypeDest, out this._exprDest, EXPRFLAG.EXF_ASFINALLYLEAVE);
						}
					}
					return true;
				}
				return (this._flags & CONVERTTYPE.NOUDC) == (CONVERTTYPE)0 && this._binder.bindUserDefinedConversion(this._exprSrc, nubSrc, this._typeDest, this._needsExprDest, out this._exprDest, true);
			}

			// Token: 0x0600071F RID: 1823 RVA: 0x00022E00 File Offset: 0x00021000
			private bool bindImplicitConversionFromArray()
			{
				if (!this.GetSymbolLoader().HasBaseConversion(this._typeSrc, this._typeDest))
				{
					return false;
				}
				EXPRFLAG exprflag = (EXPRFLAG)0;
				AggregateType aggregateType;
				if ((this._typeDest is ArrayType || ((aggregateType = this._typeDest as AggregateType) != null && aggregateType.isInterfaceType() && aggregateType.GetTypeArgsAll().Count == 1 && (aggregateType.GetTypeArgsAll()[0] != ((ArrayType)this._typeSrc).GetElementType() || (this._flags & CONVERTTYPE.FORCECAST) != (CONVERTTYPE)0))) && ((this._flags & CONVERTTYPE.FORCECAST) != (CONVERTTYPE)0 || TypeManager.TypeContainsTyVars(this._typeSrc, null) || TypeManager.TypeContainsTyVars(this._typeDest, null)))
				{
					exprflag = EXPRFLAG.EXF_OPERATOR;
				}
				if (this._needsExprDest)
				{
					this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest, exprflag);
				}
				return true;
			}

			// Token: 0x06000720 RID: 1824 RVA: 0x00022ED8 File Offset: 0x000210D8
			private bool bindImplicitConversionFromPointer()
			{
				PointerType pointerType;
				if ((pointerType = this._typeDest as PointerType) != null && pointerType.GetReferentType() == this._binder.getVoidType())
				{
					if (this._needsExprDest)
					{
						this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest);
					}
					return true;
				}
				return false;
			}

			// Token: 0x06000721 RID: 1825 RVA: 0x00022F30 File Offset: 0x00021130
			private bool bindImplicitConversionFromAgg(AggregateType aggTypeSrc)
			{
				AggregateSymbol aggregate = aggTypeSrc.getAggregate();
				if (aggregate.IsEnum())
				{
					return this.bindImplicitConversionFromEnum(aggTypeSrc);
				}
				if (this._typeDest.isEnumType())
				{
					if (this.bindImplicitConversionToEnum(aggTypeSrc))
					{
						return true;
					}
				}
				else if (aggregate.getThisType().isSimpleType() && this._typeDest.isSimpleType() && this.bindImplicitConversionBetweenSimpleTypes(aggTypeSrc))
				{
					return true;
				}
				return this.bindImplicitConversionToBase(aggTypeSrc);
			}

			// Token: 0x06000722 RID: 1826 RVA: 0x00022F98 File Offset: 0x00021198
			private bool bindImplicitConversionToBase(AggregateType pSource)
			{
				if (!(this._typeDest is AggregateType) || !this.GetSymbolLoader().HasBaseConversion(pSource, this._typeDest))
				{
					return false;
				}
				EXPRFLAG exprflag = (EXPRFLAG)0;
				if (pSource.getAggregate().IsStruct() && this._typeDest.fundType() == FUNDTYPE.FT_REF)
				{
					exprflag = EXPRFLAG.EXF_CTOR | EXPRFLAG.EXF_CANTBENULL;
				}
				else if (this._exprSrc != null)
				{
					exprflag = this._exprSrc.Flags & EXPRFLAG.EXF_CANTBENULL;
				}
				if (this._needsExprDest)
				{
					this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest, exprflag);
				}
				return true;
			}

			// Token: 0x06000723 RID: 1827 RVA: 0x00023030 File Offset: 0x00021230
			private bool bindImplicitConversionFromEnum(AggregateType aggTypeSrc)
			{
				AggregateType aggregateType;
				if ((aggregateType = this._typeDest as AggregateType) != null && this.GetSymbolLoader().HasBaseConversion(aggTypeSrc, aggregateType))
				{
					if (this._needsExprDest)
					{
						this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest, EXPRFLAG.EXF_CTOR | EXPRFLAG.EXF_CANTBENULL);
					}
					return true;
				}
				return false;
			}

			// Token: 0x06000724 RID: 1828 RVA: 0x00023088 File Offset: 0x00021288
			private bool bindImplicitConversionToEnum(AggregateType aggTypeSrc)
			{
				if (aggTypeSrc.getAggregate().GetPredefType() != PredefinedType.PT_BOOL && this._exprSrc != null && this._exprSrc.IsZero() && this._exprSrc.Type.isNumericType() && (this._flags & CONVERTTYPE.STANDARD) == (CONVERTTYPE)0)
				{
					if (this._needsExprDest)
					{
						this._exprDest = this.GetExprFactory().CreateConstant(this._typeDest, ConstVal.GetDefaultValue(this._typeDest.constValKind()));
					}
					return true;
				}
				return false;
			}

			// Token: 0x06000725 RID: 1829 RVA: 0x00023108 File Offset: 0x00021308
			private bool bindImplicitConversionBetweenSimpleTypes(AggregateType aggTypeSrc)
			{
				PredefinedType predefType = aggTypeSrc.getAggregate().GetPredefType();
				PredefinedType predefType2 = this._typeDest.getPredefType();
				ExprConstant exprConstant;
				ConvKind convKind;
				if ((exprConstant = this._exprSrc as ExprConstant) != null && this._exprSrc.IsOK && ((predefType == PredefinedType.PT_INT && predefType2 != PredefinedType.PT_BOOL && predefType2 != PredefinedType.PT_CHAR) || (predefType == PredefinedType.PT_LONG && predefType2 == PredefinedType.PT_ULONG)) && ExpressionBinder.isConstantInRange(exprConstant, this._typeDest))
				{
					convKind = ConvKind.Implicit;
					if (this._needsExprDest)
					{
						bool flag = ExpressionBinder.GetConvKind(predefType, predefType2) != ConvKind.Implicit;
					}
				}
				else if (predefType == predefType2)
				{
					convKind = ConvKind.Implicit;
				}
				else
				{
					convKind = ExpressionBinder.GetConvKind(predefType, predefType2);
				}
				if (convKind != ConvKind.Implicit)
				{
					return false;
				}
				if (this._exprSrc.GetConst() != null && this._binder.bindConstantCast(this._exprSrc, this._exprTypeDest, this._needsExprDest, out this._exprDest, false) == ConstCastResult.Success)
				{
					return true;
				}
				if (ExpressionBinder.isUserDefinedConversion(predefType, predefType2))
				{
					return !this._needsExprDest || this._binder.bindUserDefinedConversion(this._exprSrc, aggTypeSrc, this._typeDest, this._needsExprDest, out this._exprDest, true);
				}
				if (this._needsExprDest)
				{
					this._binder.bindSimpleCast(this._exprSrc, this._exprTypeDest, out this._exprDest);
				}
				return true;
			}

			// Token: 0x06000726 RID: 1830 RVA: 0x00023233 File Offset: 0x00021433
			private SymbolLoader GetSymbolLoader()
			{
				return this._binder.GetSymbolLoader();
			}

			// Token: 0x06000727 RID: 1831 RVA: 0x00023240 File Offset: 0x00021440
			private ExprFactory GetExprFactory()
			{
				return this._binder.GetExprFactory();
			}

			// Token: 0x040006D7 RID: 1751
			private Expr _exprDest;

			// Token: 0x040006D8 RID: 1752
			private readonly ExpressionBinder _binder;

			// Token: 0x040006D9 RID: 1753
			private readonly Expr _exprSrc;

			// Token: 0x040006DA RID: 1754
			private readonly CType _typeSrc;

			// Token: 0x040006DB RID: 1755
			private readonly CType _typeDest;

			// Token: 0x040006DC RID: 1756
			private readonly ExprClass _exprTypeDest;

			// Token: 0x040006DD RID: 1757
			private readonly bool _needsExprDest;

			// Token: 0x040006DE RID: 1758
			private CONVERTTYPE _flags;
		}

		// Token: 0x020000E6 RID: 230
		private class UnaOpSig
		{
			// Token: 0x06000728 RID: 1832 RVA: 0x0002324D File Offset: 0x0002144D
			protected UnaOpSig()
			{
			}

			// Token: 0x06000729 RID: 1833 RVA: 0x00023255 File Offset: 0x00021455
			public UnaOpSig(PredefinedType pt, UnaOpMask grfuom, int cuosSkip, ExpressionBinder.PfnBindUnaOp pfn, UnaOpFuncKind fnkind)
			{
				this.pt = pt;
				this.grfuom = grfuom;
				this.cuosSkip = cuosSkip;
				this.pfn = pfn;
				this.fnkind = fnkind;
			}

			// Token: 0x040006DF RID: 1759
			public PredefinedType pt;

			// Token: 0x040006E0 RID: 1760
			public UnaOpMask grfuom;

			// Token: 0x040006E1 RID: 1761
			public int cuosSkip;

			// Token: 0x040006E2 RID: 1762
			public ExpressionBinder.PfnBindUnaOp pfn;

			// Token: 0x040006E3 RID: 1763
			public UnaOpFuncKind fnkind;
		}

		// Token: 0x020000E7 RID: 231
		private sealed class UnaOpFullSig : ExpressionBinder.UnaOpSig
		{
			// Token: 0x0600072A RID: 1834 RVA: 0x00023282 File Offset: 0x00021482
			public UnaOpFullSig(CType type, ExpressionBinder.PfnBindUnaOp pfn, LiftFlags grflt, UnaOpFuncKind fnkind)
			{
				this.pt = (PredefinedType)4294967295U;
				this.grfuom = UnaOpMask.None;
				this.cuosSkip = 0;
				this.pfn = pfn;
				this._type = type;
				this._grflt = grflt;
				this.fnkind = fnkind;
			}

			// Token: 0x0600072B RID: 1835 RVA: 0x000232BC File Offset: 0x000214BC
			public UnaOpFullSig(ExpressionBinder fnc, ExpressionBinder.UnaOpSig uos)
			{
				this.pt = uos.pt;
				this.grfuom = uos.grfuom;
				this.cuosSkip = uos.cuosSkip;
				this.pfn = uos.pfn;
				this.fnkind = uos.fnkind;
				this._type = ((this.pt != (PredefinedType)4294967295U) ? fnc.GetPredefindType(this.pt) : null);
				this._grflt = LiftFlags.None;
			}

			// Token: 0x0600072C RID: 1836 RVA: 0x00023330 File Offset: 0x00021530
			public bool FPreDef()
			{
				return this.pt != (PredefinedType)4294967295U;
			}

			// Token: 0x0600072D RID: 1837 RVA: 0x0002333E File Offset: 0x0002153E
			public bool isLifted()
			{
				return this._grflt != LiftFlags.None;
			}

			// Token: 0x0600072E RID: 1838 RVA: 0x0002334B File Offset: 0x0002154B
			public bool Convert()
			{
				return (this._grflt & LiftFlags.Convert1) > LiftFlags.None;
			}

			// Token: 0x0600072F RID: 1839 RVA: 0x00023358 File Offset: 0x00021558
			public new CType GetType()
			{
				return this._type;
			}

			// Token: 0x040006E4 RID: 1764
			private readonly LiftFlags _grflt;

			// Token: 0x040006E5 RID: 1765
			private readonly CType _type;
		}
	}
}
