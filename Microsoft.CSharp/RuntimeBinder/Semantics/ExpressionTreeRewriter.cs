using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200009E RID: 158
	internal sealed class ExpressionTreeRewriter : ExprVisitorBase
	{
		// Token: 0x06000537 RID: 1335 RVA: 0x000196E8 File Offset: 0x000178E8
		public static Expr Rewrite(Expr expr, ExprFactory expressionFactory, SymbolLoader symbolLoader)
		{
			return new ExpressionTreeRewriter(expressionFactory, symbolLoader).Visit(expr);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x000196F7 File Offset: 0x000178F7
		private ExprFactory GetExprFactory()
		{
			return this.expressionFactory;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x000196FF File Offset: 0x000178FF
		private SymbolLoader GetSymbolLoader()
		{
			return this.symbolLoader;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00019707 File Offset: 0x00017907
		private ExpressionTreeRewriter(ExprFactory expressionFactory, SymbolLoader symbolLoader)
		{
			this.expressionFactory = expressionFactory;
			this.symbolLoader = symbolLoader;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001971D File Offset: 0x0001791D
		protected override Expr Dispatch(Expr expr)
		{
			Expr expr2 = base.Dispatch(expr);
			if (expr2 == expr)
			{
				throw Error.InternalCompilerError();
			}
			return expr2;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00019730 File Offset: 0x00017930
		protected override Expr VisitASSIGNMENT(ExprAssignment assignment)
		{
			ExprProperty exprProperty;
			Expr expr;
			if ((exprProperty = assignment.LHS as ExprProperty) != null)
			{
				if (exprProperty.OptionalArguments == null)
				{
					expr = base.Visit(exprProperty);
				}
				else
				{
					Expr expr2 = base.Visit(exprProperty.MemberGroup.OptionalObject);
					Expr expr3 = this.GetExprFactory().CreatePropertyInfo(exprProperty.PropWithTypeSlot.Prop(), exprProperty.PropWithTypeSlot.Ats);
					Expr expr4 = this.GenerateParamsArray(this.GenerateArgsList(exprProperty.OptionalArguments), PredefinedType.PT_EXPRESSION);
					expr = this.GenerateCall(PREDEFMETH.PM_EXPRESSION_PROPERTY, expr2, expr3, expr4);
				}
			}
			else
			{
				expr = base.Visit(assignment.LHS);
			}
			Expr expr5 = base.Visit(assignment.RHS);
			return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_ASSIGN, expr, expr5);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x000197DD File Offset: 0x000179DD
		protected override Expr VisitMULTIGET(ExprMultiGet pExpr)
		{
			return base.Visit(pExpr.OptionalMulti.Left);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x000197F0 File Offset: 0x000179F0
		protected override Expr VisitMULTI(ExprMulti pExpr)
		{
			Expr expr = base.Visit(pExpr.Operator);
			Expr expr2 = base.Visit(pExpr.Left);
			return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_ASSIGN, expr2, expr);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00019824 File Offset: 0x00017A24
		protected override Expr VisitBOUNDLAMBDA(ExprBoundLambda anonmeth)
		{
			ExprBoundLambda exprBoundLambda = this.currentAnonMeth;
			this.currentAnonMeth = anonmeth;
			MethodSymbol preDefMethod = this.GetPreDefMethod(PREDEFMETH.PM_EXPRESSION_LAMBDA);
			CType delegateType = anonmeth.DelegateType;
			TypeArray typeArray = this.GetSymbolLoader().getBSymmgr().AllocParams(1, new CType[] { delegateType });
			AggregateType predefindType = this.GetSymbolLoader().GetPredefindType(PredefinedType.PT_EXPRESSION);
			MethWithInst methWithInst = new MethWithInst(preDefMethod, predefindType, typeArray);
			Expr expr = this.CreateWraps(anonmeth);
			Expr expr2 = this.RewriteLambdaBody(anonmeth);
			Expr expr3 = this.RewriteLambdaParameters(anonmeth);
			Expr expr4 = this.GetExprFactory().CreateList(expr2, expr3);
			CType ctype = this.GetSymbolLoader().GetTypeManager().SubstType(methWithInst.Meth().RetType, methWithInst.GetType(), methWithInst.TypeArgs);
			ExprMemberGroup exprMemberGroup = this.GetExprFactory().CreateMemGroup(null, methWithInst);
			Expr expr5;
			(expr5 = this.GetExprFactory().CreateCall((EXPRFLAG)0, ctype, expr4, exprMemberGroup, methWithInst)).PredefinedMethod = PREDEFMETH.PM_EXPRESSION_LAMBDA;
			this.currentAnonMeth = exprBoundLambda;
			if (expr != null)
			{
				expr5 = this.GetExprFactory().CreateSequence(expr, expr5);
			}
			Expr expr6 = this.DestroyWraps(anonmeth, expr5);
			if (this.currentAnonMeth != null)
			{
				expr6 = this.GenerateCall(PREDEFMETH.PM_EXPRESSION_QUOTE, expr6);
			}
			return expr6;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00019948 File Offset: 0x00017B48
		protected override Expr VisitCONSTANT(ExprConstant expr)
		{
			return this.GenerateConstant(expr);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00019951 File Offset: 0x00017B51
		protected override Expr VisitLOCAL(ExprLocal local)
		{
			if (local.Local.wrap != null)
			{
				return local.Local.wrap;
			}
			return this.GetExprFactory().CreateHoistedLocalInExpression();
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00019978 File Offset: 0x00017B78
		protected override Expr VisitFIELD(ExprField expr)
		{
			Expr expr2;
			if (expr.OptionalObject == null)
			{
				expr2 = this.GetExprFactory().CreateNull();
			}
			else
			{
				expr2 = base.Visit(expr.OptionalObject);
			}
			ExprFieldInfo exprFieldInfo = this.GetExprFactory().CreateFieldInfo(expr.FieldWithType.Field(), expr.FieldWithType.GetType());
			return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_FIELD, expr2, exprFieldInfo);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x000199D4 File Offset: 0x00017BD4
		protected override Expr VisitUSERDEFINEDCONVERSION(ExprUserDefinedConversion expr)
		{
			return this.GenerateUserDefinedConversion(expr, expr.Argument);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000199E4 File Offset: 0x00017BE4
		protected override Expr VisitCAST(ExprCast pExpr)
		{
			Expr argument = pExpr.Argument;
			if (argument.Type == pExpr.Type || this.GetSymbolLoader().IsBaseClassOfClass(argument.Type, pExpr.Type) || CConversions.FImpRefConv(this.GetSymbolLoader(), argument.Type, pExpr.Type))
			{
				return base.Visit(argument);
			}
			if (pExpr.Type != null && pExpr.Type.isPredefType(PredefinedType.PT_G_EXPRESSION) && argument is ExprBoundLambda)
			{
				return base.Visit(argument);
			}
			Expr expr = this.GenerateConversion(argument, pExpr.Type, pExpr.isChecked());
			if ((pExpr.Flags & EXPRFLAG.EXF_USERCALLABLE) != (EXPRFLAG)0)
			{
				expr.Flags |= EXPRFLAG.EXF_USERCALLABLE;
			}
			return expr;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00019A9C File Offset: 0x00017C9C
		protected override Expr VisitCONCAT(ExprConcat expr)
		{
			PREDEFMETH predefmeth;
			if (expr.FirstArgument.Type.isPredefType(PredefinedType.PT_STRING) && expr.SecondArgument.Type.isPredefType(PredefinedType.PT_STRING))
			{
				predefmeth = PREDEFMETH.PM_STRING_CONCAT_STRING_2;
			}
			else
			{
				predefmeth = PREDEFMETH.PM_STRING_CONCAT_OBJECT_2;
			}
			Expr expr2 = base.Visit(expr.FirstArgument);
			Expr expr3 = base.Visit(expr.SecondArgument);
			MethodSymbol preDefMethod = this.GetPreDefMethod(predefmeth);
			Expr expr4 = this.GetExprFactory().CreateMethodInfo(preDefMethod, this.GetSymbolLoader().GetPredefindType(PredefinedType.PT_STRING), null);
			return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_ADD_USER_DEFINED, expr2, expr3, expr4);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00019B22 File Offset: 0x00017D22
		protected override Expr VisitBINOP(ExprBinOp expr)
		{
			if (expr.UserDefinedCallMethod != null)
			{
				return this.GenerateUserDefinedBinaryOperator(expr);
			}
			return this.GenerateBuiltInBinaryOperator(expr);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00019B41 File Offset: 0x00017D41
		protected override Expr VisitUNARYOP(ExprUnaryOp pExpr)
		{
			if (pExpr.UserDefinedCallMethod != null)
			{
				return this.GenerateUserDefinedUnaryOperator(pExpr);
			}
			return this.GenerateBuiltInUnaryOperator(pExpr);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00019B60 File Offset: 0x00017D60
		protected override Expr VisitARRAYINDEX(ExprArrayIndex pExpr)
		{
			Expr expr = base.Visit(pExpr.Array);
			Expr expr2 = this.GenerateIndexList(pExpr.Index);
			if (expr2 is ExprList)
			{
				Expr expr3 = this.GenerateParamsArray(expr2, PredefinedType.PT_EXPRESSION);
				return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_ARRAYINDEX2, expr, expr3);
			}
			return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_ARRAYINDEX, expr, expr2);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00019BB0 File Offset: 0x00017DB0
		protected override Expr VisitCALL(ExprCall expr)
		{
			switch (expr.NullableCallLiftKind)
			{
			case NullableCallLiftKind.UserDefinedConversion:
			case NullableCallLiftKind.NotLiftedIntermediateConversion:
				return this.GenerateUserDefinedConversion(expr.OptionalArguments, expr.Type, expr.MethWithInst);
			case NullableCallLiftKind.NullableConversion:
			case NullableCallLiftKind.NullableConversionConstructor:
			case NullableCallLiftKind.NullableIntermediateConversion:
				return this.GenerateConversion(expr.OptionalArguments, expr.Type, expr.isChecked());
			default:
			{
				if (expr.MethWithInst.Meth().IsConstructor())
				{
					return this.GenerateConstructor(expr);
				}
				if (expr.MemberGroup.IsDelegate)
				{
					return this.GenerateDelegateInvoke(expr);
				}
				Expr expr2;
				if (expr.MethWithInst.Meth().isStatic || expr.MemberGroup.OptionalObject == null)
				{
					expr2 = this.GetExprFactory().CreateNull();
				}
				else
				{
					expr2 = expr.MemberGroup.OptionalObject;
					ExprCast exprCast;
					if (expr2 != null && (exprCast = expr2 as ExprCast) != null && exprCast.IsBoxingCast)
					{
						expr2 = exprCast.Argument;
					}
					expr2 = base.Visit(expr2);
				}
				Expr expr3 = this.GetExprFactory().CreateMethodInfo(expr.MethWithInst);
				Expr expr4 = this.GenerateArgsList(expr.OptionalArguments);
				Expr expr5 = this.GenerateParamsArray(expr4, PredefinedType.PT_EXPRESSION);
				PREDEFMETH predefmeth = PREDEFMETH.PM_EXPRESSION_CALL;
				return this.GenerateCall(predefmeth, expr2, expr3, expr5);
			}
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00019CE0 File Offset: 0x00017EE0
		protected override Expr VisitPROP(ExprProperty expr)
		{
			Expr expr2;
			if (expr.PropWithTypeSlot.Prop().isStatic || expr.MemberGroup.OptionalObject == null)
			{
				expr2 = this.GetExprFactory().CreateNull();
			}
			else
			{
				expr2 = base.Visit(expr.MemberGroup.OptionalObject);
			}
			Expr expr3 = this.GetExprFactory().CreatePropertyInfo(expr.PropWithTypeSlot.Prop(), expr.PropWithTypeSlot.GetType());
			if (expr.OptionalArguments != null)
			{
				Expr expr4 = this.GenerateArgsList(expr.OptionalArguments);
				Expr expr5 = this.GenerateParamsArray(expr4, PredefinedType.PT_EXPRESSION);
				return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_PROPERTY, expr2, expr3, expr5);
			}
			return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_PROPERTY, expr2, expr3);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00019D84 File Offset: 0x00017F84
		protected override Expr VisitARRINIT(ExprArrayInit expr)
		{
			Expr expr2 = this.CreateTypeOf(((ArrayType)expr.Type).GetElementType());
			Expr expr3 = this.GenerateArgsList(expr.OptionalArguments);
			Expr expr4 = this.GenerateParamsArray(expr3, PredefinedType.PT_EXPRESSION);
			return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_NEWARRAYINIT, expr2, expr4);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00019DC9 File Offset: 0x00017FC9
		protected override Expr VisitZEROINIT(ExprZeroInit expr)
		{
			return this.GenerateConstant(expr);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00019DD2 File Offset: 0x00017FD2
		protected override Expr VisitTYPEOF(ExprTypeOf expr)
		{
			return this.GenerateConstant(expr);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00019DDC File Offset: 0x00017FDC
		private Expr GenerateDelegateInvoke(ExprCall expr)
		{
			Expr optionalObject = expr.MemberGroup.OptionalObject;
			Expr expr2 = base.Visit(optionalObject);
			Expr expr3 = this.GenerateArgsList(expr.OptionalArguments);
			Expr expr4 = this.GenerateParamsArray(expr3, PredefinedType.PT_EXPRESSION);
			return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_INVOKE, expr2, expr4);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00019E20 File Offset: 0x00018020
		private Expr GenerateBuiltInBinaryOperator(ExprBinOp expr)
		{
			PREDEFMETH predefmeth;
			switch (expr.Kind)
			{
			case ExpressionKind.Eq:
				predefmeth = PREDEFMETH.PM_EXPRESSION_EQUAL;
				goto IL_011E;
			case ExpressionKind.NotEq:
				predefmeth = PREDEFMETH.PM_EXPRESSION_NOTEQUAL;
				goto IL_011E;
			case ExpressionKind.LessThan:
				predefmeth = PREDEFMETH.PM_EXPRESSION_LESSTHAN;
				goto IL_011E;
			case ExpressionKind.LessThanOrEqual:
				predefmeth = PREDEFMETH.PM_EXPRESSION_LESSTHANOREQUAL;
				goto IL_011E;
			case ExpressionKind.GreaterThan:
				predefmeth = PREDEFMETH.PM_EXPRESSION_GREATERTHAN;
				goto IL_011E;
			case ExpressionKind.GreaterThanOrEqual:
				predefmeth = PREDEFMETH.PM_EXPRESSION_GREATERTHANOREQUAL;
				goto IL_011E;
			case ExpressionKind.Add:
				predefmeth = (expr.isChecked() ? PREDEFMETH.PM_EXPRESSION_ADDCHECKED : PREDEFMETH.PM_EXPRESSION_ADD);
				goto IL_011E;
			case ExpressionKind.Subtract:
				predefmeth = (expr.isChecked() ? PREDEFMETH.PM_EXPRESSION_SUBTRACTCHECKED : PREDEFMETH.PM_EXPRESSION_SUBTRACT);
				goto IL_011E;
			case ExpressionKind.Multiply:
				predefmeth = (expr.isChecked() ? PREDEFMETH.PM_EXPRESSION_MULTIPLYCHECKED : PREDEFMETH.PM_EXPRESSION_MULTIPLY);
				goto IL_011E;
			case ExpressionKind.Divide:
				predefmeth = PREDEFMETH.PM_EXPRESSION_DIVIDE;
				goto IL_011E;
			case ExpressionKind.Modulo:
				predefmeth = PREDEFMETH.PM_EXPRESSION_MODULO;
				goto IL_011E;
			case ExpressionKind.BitwiseAnd:
				predefmeth = PREDEFMETH.PM_EXPRESSION_AND;
				goto IL_011E;
			case ExpressionKind.BitwiseOr:
				predefmeth = PREDEFMETH.PM_EXPRESSION_OR;
				goto IL_011E;
			case ExpressionKind.BitwiseExclusiveOr:
				predefmeth = PREDEFMETH.PM_EXPRESSION_EXCLUSIVEOR;
				goto IL_011E;
			case ExpressionKind.LeftShirt:
				predefmeth = PREDEFMETH.PM_EXPRESSION_LEFTSHIFT;
				goto IL_011E;
			case ExpressionKind.RightShift:
				predefmeth = PREDEFMETH.PM_EXPRESSION_RIGHTSHIFT;
				goto IL_011E;
			case ExpressionKind.LogicalAnd:
				predefmeth = PREDEFMETH.PM_EXPRESSION_ANDALSO;
				goto IL_011E;
			case ExpressionKind.LogicalOr:
				predefmeth = PREDEFMETH.PM_EXPRESSION_ORELSE;
				goto IL_011E;
			case ExpressionKind.StringEq:
				predefmeth = PREDEFMETH.PM_EXPRESSION_EQUAL;
				goto IL_011E;
			case ExpressionKind.StringNotEq:
				predefmeth = PREDEFMETH.PM_EXPRESSION_NOTEQUAL;
				goto IL_011E;
			}
			throw Error.InternalCompilerError();
			IL_011E:
			Expr optionalLeftChild = expr.OptionalLeftChild;
			Expr optionalRightChild = expr.OptionalRightChild;
			CType ctype = optionalLeftChild.Type;
			CType ctype2 = optionalRightChild.Type;
			Expr expr2 = base.Visit(optionalLeftChild);
			Expr expr3 = base.Visit(optionalRightChild);
			bool flag = false;
			CType ctype3 = null;
			CType ctype4 = null;
			NullableType nullableType;
			if (ctype.isEnumType())
			{
				ctype3 = this.GetSymbolLoader().GetTypeManager().GetNullable(ctype.underlyingEnumType());
				ctype = ctype3;
				flag = true;
			}
			else if ((nullableType = ctype as NullableType) != null && nullableType.UnderlyingType.isEnumType())
			{
				ctype3 = this.GetSymbolLoader().GetTypeManager().GetNullable(nullableType.UnderlyingType.underlyingEnumType());
				ctype = ctype3;
				flag = true;
			}
			NullableType nullableType2;
			if (ctype2.isEnumType())
			{
				ctype4 = this.GetSymbolLoader().GetTypeManager().GetNullable(ctype2.underlyingEnumType());
				ctype2 = ctype4;
				flag = true;
			}
			else if ((nullableType2 = ctype2 as NullableType) != null && nullableType2.UnderlyingType.isEnumType())
			{
				ctype4 = this.GetSymbolLoader().GetTypeManager().GetNullable(nullableType2.UnderlyingType.underlyingEnumType());
				ctype2 = ctype4;
				flag = true;
			}
			NullableType nullableType3;
			if ((nullableType3 = ctype as NullableType) != null && nullableType3.UnderlyingType == ctype2)
			{
				ctype4 = ctype;
			}
			NullableType nullableType4;
			if ((nullableType4 = ctype2 as NullableType) != null && nullableType4.UnderlyingType == ctype)
			{
				ctype3 = ctype2;
			}
			if (ctype3 != null)
			{
				expr2 = this.GenerateCall(PREDEFMETH.PM_EXPRESSION_CONVERT, expr2, this.CreateTypeOf(ctype3));
			}
			if (ctype4 != null)
			{
				expr3 = this.GenerateCall(PREDEFMETH.PM_EXPRESSION_CONVERT, expr3, this.CreateTypeOf(ctype4));
			}
			Expr expr4 = this.GenerateCall(predefmeth, expr2, expr3);
			if (flag && expr.Type.StripNubs().isEnumType())
			{
				expr4 = this.GenerateCall(PREDEFMETH.PM_EXPRESSION_CONVERT, expr4, this.CreateTypeOf(expr.Type));
			}
			return expr4;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001A0F4 File Offset: 0x000182F4
		private Expr GenerateBuiltInUnaryOperator(ExprUnaryOp expr)
		{
			ExpressionKind kind = expr.Kind;
			PREDEFMETH predefmeth;
			if (kind <= ExpressionKind.Negate)
			{
				if (kind == ExpressionKind.LogicalNot)
				{
					predefmeth = PREDEFMETH.PM_EXPRESSION_NOT;
					goto IL_0052;
				}
				if (kind == ExpressionKind.Negate)
				{
					predefmeth = (expr.isChecked() ? PREDEFMETH.PM_EXPRESSION_NEGATECHECKED : PREDEFMETH.PM_EXPRESSION_NEGATE);
					goto IL_0052;
				}
			}
			else
			{
				if (kind == ExpressionKind.UnaryPlus)
				{
					return base.Visit(expr.Child);
				}
				if (kind == ExpressionKind.BitwiseNot)
				{
					predefmeth = PREDEFMETH.PM_EXPRESSION_NOT;
					goto IL_0052;
				}
			}
			throw Error.InternalCompilerError();
			IL_0052:
			Expr child = expr.Child;
			return this.GenerateBuiltInUnaryOperator(predefmeth, child, expr);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001A164 File Offset: 0x00018364
		private Expr GenerateBuiltInUnaryOperator(PREDEFMETH pdm, Expr pOriginalOperator, Expr pOperator)
		{
			Expr expr = base.Visit(pOriginalOperator);
			NullableType nullableType;
			bool flag = (nullableType = pOriginalOperator.Type as NullableType) != null && nullableType.underlyingType().isEnumType();
			if (flag)
			{
				CType ctype = pOriginalOperator.Type.StripNubs().underlyingEnumType();
				CType nullable = this.GetSymbolLoader().GetTypeManager().GetNullable(ctype);
				expr = this.GenerateCall(PREDEFMETH.PM_EXPRESSION_CONVERT, expr, this.CreateTypeOf(nullable));
			}
			Expr expr2 = this.GenerateCall(pdm, expr);
			if (flag)
			{
				expr2 = this.GenerateCall(PREDEFMETH.PM_EXPRESSION_CONVERT, expr2, this.CreateTypeOf(pOperator.Type));
			}
			return expr2;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001A1F4 File Offset: 0x000183F4
		private Expr GenerateUserDefinedBinaryOperator(ExprBinOp expr)
		{
			PREDEFMETH predefmeth;
			switch (expr.Kind)
			{
			case ExpressionKind.Eq:
			case ExpressionKind.NotEq:
			case ExpressionKind.LessThan:
			case ExpressionKind.LessThanOrEqual:
			case ExpressionKind.GreaterThan:
			case ExpressionKind.GreaterThanOrEqual:
			case ExpressionKind.StringEq:
			case ExpressionKind.StringNotEq:
			case ExpressionKind.DelegateEq:
			case ExpressionKind.DelegateNotEq:
				return this.GenerateUserDefinedComparisonOperator(expr);
			case ExpressionKind.Add:
			case ExpressionKind.DelegateAdd:
				predefmeth = (expr.isChecked() ? PREDEFMETH.PM_EXPRESSION_ADDCHECKED_USER_DEFINED : PREDEFMETH.PM_EXPRESSION_ADD_USER_DEFINED);
				goto IL_0105;
			case ExpressionKind.Subtract:
			case ExpressionKind.DelegateSubtract:
				predefmeth = (expr.isChecked() ? PREDEFMETH.PM_EXPRESSION_SUBTRACTCHECKED_USER_DEFINED : PREDEFMETH.PM_EXPRESSION_SUBTRACT_USER_DEFINED);
				goto IL_0105;
			case ExpressionKind.Multiply:
				predefmeth = (expr.isChecked() ? PREDEFMETH.PM_EXPRESSION_MULTIPLYCHECKED_USER_DEFINED : PREDEFMETH.PM_EXPRESSION_MULTIPLY_USER_DEFINED);
				goto IL_0105;
			case ExpressionKind.Divide:
				predefmeth = PREDEFMETH.PM_EXPRESSION_DIVIDE_USER_DEFINED;
				goto IL_0105;
			case ExpressionKind.Modulo:
				predefmeth = PREDEFMETH.PM_EXPRESSION_MODULO_USER_DEFINED;
				goto IL_0105;
			case ExpressionKind.BitwiseAnd:
				predefmeth = PREDEFMETH.PM_EXPRESSION_AND_USER_DEFINED;
				goto IL_0105;
			case ExpressionKind.BitwiseOr:
				predefmeth = PREDEFMETH.PM_EXPRESSION_OR_USER_DEFINED;
				goto IL_0105;
			case ExpressionKind.BitwiseExclusiveOr:
				predefmeth = PREDEFMETH.PM_EXPRESSION_EXCLUSIVEOR_USER_DEFINED;
				goto IL_0105;
			case ExpressionKind.LeftShirt:
				predefmeth = PREDEFMETH.PM_EXPRESSION_LEFTSHIFT_USER_DEFINED;
				goto IL_0105;
			case ExpressionKind.RightShift:
				predefmeth = PREDEFMETH.PM_EXPRESSION_RIGHTSHIFT_USER_DEFINED;
				goto IL_0105;
			case ExpressionKind.LogicalAnd:
				predefmeth = PREDEFMETH.PM_EXPRESSION_ANDALSO_USER_DEFINED;
				goto IL_0105;
			case ExpressionKind.LogicalOr:
				predefmeth = PREDEFMETH.PM_EXPRESSION_ORELSE_USER_DEFINED;
				goto IL_0105;
			}
			throw Error.InternalCompilerError();
			IL_0105:
			Expr expr2 = expr.OptionalLeftChild;
			Expr expr3 = expr.OptionalRightChild;
			Expr optionalUserDefinedCall = expr.OptionalUserDefinedCall;
			if (optionalUserDefinedCall != null)
			{
				ExprCall exprCall;
				if ((exprCall = optionalUserDefinedCall as ExprCall) != null)
				{
					ExprList exprList = (ExprList)exprCall.OptionalArguments;
					expr2 = exprList.OptionalElement;
					expr3 = exprList.OptionalNextListNode;
				}
				else
				{
					ExprList exprList2 = (ExprList)(optionalUserDefinedCall as ExprUserLogicalOp).OperatorCall.OptionalArguments;
					expr2 = ((ExprWrap)exprList2.OptionalElement).OptionalExpression;
					expr3 = exprList2.OptionalNextListNode;
				}
			}
			expr2 = base.Visit(expr2);
			expr3 = base.Visit(expr3);
			this.FixLiftedUserDefinedBinaryOperators(expr, ref expr2, ref expr3);
			Expr expr4 = this.GetExprFactory().CreateMethodInfo(expr.UserDefinedCallMethod);
			Expr expr5 = this.GenerateCall(predefmeth, expr2, expr3, expr4);
			if (expr.Kind == ExpressionKind.DelegateSubtract || expr.Kind == ExpressionKind.DelegateAdd)
			{
				Expr expr6 = this.CreateTypeOf(expr.Type);
				return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_CONVERT, expr5, expr6);
			}
			return expr5;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001A3DC File Offset: 0x000185DC
		private Expr GenerateUserDefinedUnaryOperator(ExprUnaryOp expr)
		{
			Expr expr2 = expr.Child;
			ExprCall exprCall = (ExprCall)expr.OptionalUserDefinedCall;
			if (exprCall != null)
			{
				expr2 = exprCall.OptionalArguments;
			}
			ExpressionKind kind = expr.Kind;
			PREDEFMETH predefmeth;
			if (kind <= ExpressionKind.UnaryPlus)
			{
				switch (kind)
				{
				case ExpressionKind.True:
				case ExpressionKind.False:
					return base.Visit(exprCall);
				case ExpressionKind.Inc:
				case ExpressionKind.Dec:
					goto IL_0095;
				case ExpressionKind.LogicalNot:
					predefmeth = PREDEFMETH.PM_EXPRESSION_NOT_USER_DEFINED;
					goto IL_00A0;
				default:
					if (kind != ExpressionKind.Negate)
					{
						if (kind != ExpressionKind.UnaryPlus)
						{
							goto IL_009A;
						}
						predefmeth = PREDEFMETH.PM_EXPRESSION_UNARYPLUS_USER_DEFINED;
						goto IL_00A0;
					}
					break;
				}
			}
			else
			{
				if (kind == ExpressionKind.BitwiseNot)
				{
					predefmeth = PREDEFMETH.PM_EXPRESSION_NOT_USER_DEFINED;
					goto IL_00A0;
				}
				if (kind != ExpressionKind.DecimalNegate)
				{
					if (kind - ExpressionKind.DecimalInc > 1)
					{
						goto IL_009A;
					}
					goto IL_0095;
				}
			}
			predefmeth = (expr.isChecked() ? PREDEFMETH.PM_EXPRESSION_NEGATECHECKED_USER_DEFINED : PREDEFMETH.PM_EXPRESSION_NEGATE_USER_DEFINED);
			goto IL_00A0;
			IL_0095:
			predefmeth = PREDEFMETH.PM_EXPRESSION_CALL;
			goto IL_00A0;
			IL_009A:
			throw Error.InternalCompilerError();
			IL_00A0:
			Expr expr3 = base.Visit(expr2);
			Expr expr4 = this.GetExprFactory().CreateMethodInfo(expr.UserDefinedCallMethod);
			if (expr.Kind == ExpressionKind.Inc || expr.Kind == ExpressionKind.Dec || expr.Kind == ExpressionKind.DecimalInc || expr.Kind == ExpressionKind.DecimalDec)
			{
				return this.GenerateCall(predefmeth, null, expr4, this.GenerateParamsArray(expr3, PredefinedType.PT_EXPRESSION));
			}
			return this.GenerateCall(predefmeth, expr3, expr4);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001A4EC File Offset: 0x000186EC
		private Expr GenerateUserDefinedComparisonOperator(ExprBinOp expr)
		{
			ExpressionKind kind = expr.Kind;
			PREDEFMETH predefmeth;
			switch (kind)
			{
			case ExpressionKind.Eq:
				predefmeth = PREDEFMETH.PM_EXPRESSION_EQUAL_USER_DEFINED;
				break;
			case ExpressionKind.NotEq:
				predefmeth = PREDEFMETH.PM_EXPRESSION_NOTEQUAL_USER_DEFINED;
				break;
			case ExpressionKind.LessThan:
				predefmeth = PREDEFMETH.PM_EXPRESSION_LESSTHAN_USER_DEFINED;
				break;
			case ExpressionKind.LessThanOrEqual:
				predefmeth = PREDEFMETH.PM_EXPRESSION_LESSTHANOREQUAL_USER_DEFINED;
				break;
			case ExpressionKind.GreaterThan:
				predefmeth = PREDEFMETH.PM_EXPRESSION_GREATERTHAN_USER_DEFINED;
				break;
			case ExpressionKind.GreaterThanOrEqual:
				predefmeth = PREDEFMETH.PM_EXPRESSION_GREATERTHANOREQUAL_USER_DEFINED;
				break;
			default:
				switch (kind)
				{
				case ExpressionKind.StringEq:
					predefmeth = PREDEFMETH.PM_EXPRESSION_EQUAL_USER_DEFINED;
					break;
				case ExpressionKind.StringNotEq:
					predefmeth = PREDEFMETH.PM_EXPRESSION_NOTEQUAL_USER_DEFINED;
					break;
				case ExpressionKind.DelegateEq:
					predefmeth = PREDEFMETH.PM_EXPRESSION_EQUAL_USER_DEFINED;
					break;
				case ExpressionKind.DelegateNotEq:
					predefmeth = PREDEFMETH.PM_EXPRESSION_NOTEQUAL_USER_DEFINED;
					break;
				default:
					throw Error.InternalCompilerError();
				}
				break;
			}
			Expr expr2 = expr.OptionalLeftChild;
			Expr expr3 = expr.OptionalRightChild;
			if (expr.OptionalUserDefinedCall != null)
			{
				ExprList exprList = (ExprList)((ExprCall)expr.OptionalUserDefinedCall).OptionalArguments;
				expr2 = exprList.OptionalElement;
				expr3 = exprList.OptionalNextListNode;
			}
			expr2 = base.Visit(expr2);
			expr3 = base.Visit(expr3);
			this.FixLiftedUserDefinedBinaryOperators(expr, ref expr2, ref expr3);
			Expr expr4 = this.GetExprFactory().CreateBoolConstant(false);
			Expr expr5 = this.GetExprFactory().CreateMethodInfo(expr.UserDefinedCallMethod);
			return this.GenerateCall(predefmeth, expr2, expr3, expr4, expr5);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001A5F8 File Offset: 0x000187F8
		private Expr RewriteLambdaBody(ExprBoundLambda anonmeth)
		{
			ExprReturn exprReturn;
			if ((exprReturn = anonmeth.OptionalBody.OptionalStatements as ExprReturn) != null)
			{
				return base.Visit(exprReturn.OptionalObject);
			}
			throw Error.InternalCompilerError();
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001A62C File Offset: 0x0001882C
		private Expr RewriteLambdaParameters(ExprBoundLambda anonmeth)
		{
			Expr expr = null;
			Expr expr2 = expr;
			for (Symbol symbol = anonmeth.ArgumentScope; symbol != null; symbol = symbol.nextChild)
			{
				LocalVariableSymbol localVariableSymbol;
				if ((localVariableSymbol = symbol as LocalVariableSymbol) != null && !localVariableSymbol.isThis)
				{
					this.GetExprFactory().AppendItemToList(localVariableSymbol.wrap, ref expr, ref expr2);
				}
			}
			return this.GenerateParamsArray(expr, PredefinedType.PT_PARAMETEREXPRESSION);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001A680 File Offset: 0x00018880
		private Expr GenerateConversion(Expr arg, CType CType, bool bChecked)
		{
			return this.GenerateConversionWithSource(base.Visit(arg), CType, bChecked || arg.isChecked());
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001A69C File Offset: 0x0001889C
		private Expr GenerateConversionWithSource(Expr pTarget, CType pType, bool bChecked)
		{
			PREDEFMETH predefmeth = (bChecked ? PREDEFMETH.PM_EXPRESSION_CONVERTCHECKED : PREDEFMETH.PM_EXPRESSION_CONVERT);
			Expr expr = this.CreateTypeOf(pType);
			return this.GenerateCall(predefmeth, pTarget, expr);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001A6C4 File Offset: 0x000188C4
		private Expr GenerateValueAccessConversion(Expr pArgument)
		{
			CType ctype = pArgument.Type.StripNubs();
			Expr expr = this.CreateTypeOf(ctype);
			return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_CONVERT, base.Visit(pArgument), expr);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001A6F8 File Offset: 0x000188F8
		private Expr GenerateUserDefinedConversion(Expr arg, CType type, MethWithInst method)
		{
			Expr expr = base.Visit(arg);
			return this.GenerateUserDefinedConversion(arg, type, expr, method);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001A718 File Offset: 0x00018918
		private Expr GenerateUserDefinedConversion(Expr arg, CType CType, Expr target, MethWithInst method)
		{
			if (ExpressionTreeRewriter.isEnumToDecimalConversion(arg.Type, CType))
			{
				CType ctype = arg.Type.StripNubs().underlyingEnumType();
				CType nullable = this.GetSymbolLoader().GetTypeManager().GetNullable(ctype);
				Expr expr = this.CreateTypeOf(nullable);
				target = this.GenerateCall(PREDEFMETH.PM_EXPRESSION_CONVERT, target, expr);
			}
			CType ctype2 = this.GetSymbolLoader().GetTypeManager().SubstType(method.Meth().RetType, method.GetType(), method.TypeArgs);
			bool flag = ctype2 == CType || (this.IsNullableValueType(arg.Type) && this.IsNullableValueType(CType));
			Expr expr2 = this.CreateTypeOf(flag ? CType : ctype2);
			Expr expr3 = this.GetExprFactory().CreateMethodInfo(method);
			PREDEFMETH predefmeth = (arg.isChecked() ? PREDEFMETH.PM_EXPRESSION_CONVERTCHECKED_USER_DEFINED : PREDEFMETH.PM_EXPRESSION_CONVERT_USER_DEFINED);
			Expr expr4 = this.GenerateCall(predefmeth, target, expr2, expr3);
			if (flag)
			{
				return expr4;
			}
			PREDEFMETH predefmeth2 = (arg.isChecked() ? PREDEFMETH.PM_EXPRESSION_CONVERTCHECKED : PREDEFMETH.PM_EXPRESSION_CONVERT);
			Expr expr5 = this.CreateTypeOf(CType);
			return this.GenerateCall(predefmeth2, expr4, expr5);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001A820 File Offset: 0x00018A20
		private Expr GenerateUserDefinedConversion(ExprUserDefinedConversion pExpr, Expr pArgument)
		{
			Expr userDefinedCall = pExpr.UserDefinedCall;
			Expr argument = pExpr.Argument;
			Expr expr;
			if (!ExpressionTreeRewriter.isEnumToDecimalConversion(pArgument.Type, pExpr.Type) && this.IsNullableValueAccess(argument, pArgument))
			{
				expr = this.GenerateValueAccessConversion(pArgument);
			}
			else
			{
				ExprCall exprCall = userDefinedCall as ExprCall;
				Expr expr2 = ((exprCall != null) ? exprCall.PConversions : null);
				if (expr2 != null)
				{
					ExprCall exprCall2;
					if ((exprCall2 = expr2 as ExprCall) != null)
					{
						Expr optionalArguments = exprCall2.OptionalArguments;
						if (this.IsNullableValueAccess(optionalArguments, pArgument))
						{
							expr = this.GenerateValueAccessConversion(pArgument);
						}
						else
						{
							expr = base.Visit(optionalArguments);
						}
						return this.GenerateConversionWithSource(expr, userDefinedCall.Type, exprCall.isChecked());
					}
					return this.GenerateUserDefinedConversion((ExprUserDefinedConversion)expr2, pArgument);
				}
				else
				{
					expr = base.Visit(argument);
				}
			}
			return this.GenerateUserDefinedConversion(argument, pExpr.Type, expr, pExpr.UserDefinedCallMethod);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001A8F0 File Offset: 0x00018AF0
		private Expr GenerateParameter(string name, CType CType)
		{
			this.GetSymbolLoader().GetPredefindType(PredefinedType.PT_STRING);
			ExprConstant exprConstant = this.GetExprFactory().CreateStringConstant(name);
			ExprTypeOf exprTypeOf = this.CreateTypeOf(CType);
			return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_PARAMETER, exprTypeOf, exprConstant);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001A92A File Offset: 0x00018B2A
		private MethodSymbol GetPreDefMethod(PREDEFMETH pdm)
		{
			return this.GetSymbolLoader().getPredefinedMembers().GetMethod(pdm);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001A93D File Offset: 0x00018B3D
		private ExprTypeOf CreateTypeOf(CType CType)
		{
			return this.GetExprFactory().CreateTypeOf(CType);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001A94C File Offset: 0x00018B4C
		private Expr CreateWraps(ExprBoundLambda anonmeth)
		{
			Expr expr = null;
			for (Symbol symbol = anonmeth.ArgumentScope.firstChild; symbol != null; symbol = symbol.nextChild)
			{
				LocalVariableSymbol localVariableSymbol;
				if ((localVariableSymbol = symbol as LocalVariableSymbol) != null && !localVariableSymbol.isThis)
				{
					Expr expr2 = this.GenerateParameter(localVariableSymbol.name.Text, localVariableSymbol.GetType());
					localVariableSymbol.wrap = this.GetExprFactory().CreateWrap(expr2);
					Expr expr3 = this.GetExprFactory().CreateSave(localVariableSymbol.wrap);
					if (expr == null)
					{
						expr = expr3;
					}
					else
					{
						expr = this.GetExprFactory().CreateSequence(expr, expr3);
					}
				}
			}
			return expr;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001A9DC File Offset: 0x00018BDC
		private Expr DestroyWraps(ExprBoundLambda anonmeth, Expr sequence)
		{
			for (Symbol symbol = anonmeth.ArgumentScope; symbol != null; symbol = symbol.nextChild)
			{
				LocalVariableSymbol localVariableSymbol;
				if ((localVariableSymbol = symbol as LocalVariableSymbol) != null && !localVariableSymbol.isThis)
				{
					Expr expr = this.GetExprFactory().CreateWrap(localVariableSymbol.wrap);
					sequence = this.GetExprFactory().CreateReverseSequence(sequence, expr);
				}
			}
			return sequence;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001AA30 File Offset: 0x00018C30
		private Expr GenerateConstructor(ExprCall expr)
		{
			Expr expr2 = this.GetExprFactory().CreateMethodInfo(expr.MethWithInst);
			Expr expr3 = this.GenerateArgsList(expr.OptionalArguments);
			Expr expr4 = this.GenerateParamsArray(expr3, PredefinedType.PT_EXPRESSION);
			return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_NEW, expr2, expr4);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001AA70 File Offset: 0x00018C70
		private Expr GenerateArgsList(Expr oldArgs)
		{
			Expr expr = null;
			Expr expr2 = expr;
			ExpressionIterator expressionIterator = new ExpressionIterator(oldArgs);
			while (!expressionIterator.AtEnd())
			{
				Expr expr3 = expressionIterator.Current();
				this.GetExprFactory().AppendItemToList(base.Visit(expr3), ref expr, ref expr2);
				expressionIterator.MoveNext();
			}
			return expr;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001AAB8 File Offset: 0x00018CB8
		private Expr GenerateIndexList(Expr oldIndices)
		{
			CType predefindType = this.symbolLoader.GetPredefindType(PredefinedType.PT_INT);
			Expr expr = null;
			Expr expr2 = expr;
			ExpressionIterator expressionIterator = new ExpressionIterator(oldIndices);
			while (!expressionIterator.AtEnd())
			{
				Expr expr3 = expressionIterator.Current();
				if (expr3.Type != predefindType)
				{
					ExprClass exprClass = this.expressionFactory.CreateClass(predefindType);
					expr3 = this.expressionFactory.CreateCast(EXPRFLAG.EXF_LITERALCONST, exprClass, expr3);
					expr3.Flags |= EXPRFLAG.EXF_CHECKOVERFLOW;
				}
				Expr expr4 = base.Visit(expr3);
				this.expressionFactory.AppendItemToList(expr4, ref expr, ref expr2);
				expressionIterator.MoveNext();
			}
			return expr;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001AB50 File Offset: 0x00018D50
		private Expr GenerateConstant(Expr expr)
		{
			EXPRFLAG exprflag = (EXPRFLAG)0;
			AggregateType predefindType = this.GetSymbolLoader().GetPredefindType(PredefinedType.PT_OBJECT);
			if (expr.Type is NullType)
			{
				ExprTypeOf exprTypeOf = this.CreateTypeOf(predefindType);
				return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_CONSTANT_OBJECT_TYPE, expr, exprTypeOf);
			}
			AggregateType predefindType2 = this.GetSymbolLoader().GetPredefindType(PredefinedType.PT_STRING);
			if (expr.Type != predefindType2)
			{
				exprflag = EXPRFLAG.EXF_CTOR;
			}
			ExprClass exprClass = this.GetExprFactory().CreateClass(predefindType);
			ExprCast exprCast = this.GetExprFactory().CreateCast(exprflag, exprClass, expr);
			ExprTypeOf exprTypeOf2 = this.CreateTypeOf(expr.Type);
			return this.GenerateCall(PREDEFMETH.PM_EXPRESSION_CONSTANT_OBJECT_TYPE, exprCast, exprTypeOf2);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001ABE0 File Offset: 0x00018DE0
		private ExprCall GenerateCall(PREDEFMETH pdm, Expr arg1)
		{
			MethodSymbol preDefMethod = this.GetPreDefMethod(pdm);
			if (preDefMethod == null)
			{
				return null;
			}
			AggregateType predefindType = this.GetSymbolLoader().GetPredefindType(PredefinedType.PT_EXPRESSION);
			MethWithInst methWithInst = new MethWithInst(preDefMethod, predefindType);
			ExprMemberGroup exprMemberGroup = this.GetExprFactory().CreateMemGroup(null, methWithInst);
			ExprCall exprCall = this.GetExprFactory().CreateCall((EXPRFLAG)0, methWithInst.Meth().RetType, arg1, exprMemberGroup, methWithInst);
			exprCall.PredefinedMethod = pdm;
			return exprCall;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001AC40 File Offset: 0x00018E40
		private ExprCall GenerateCall(PREDEFMETH pdm, Expr arg1, Expr arg2)
		{
			MethodSymbol preDefMethod = this.GetPreDefMethod(pdm);
			if (preDefMethod == null)
			{
				return null;
			}
			AggregateType predefindType = this.GetSymbolLoader().GetPredefindType(PredefinedType.PT_EXPRESSION);
			Expr expr = this.GetExprFactory().CreateList(arg1, arg2);
			MethWithInst methWithInst = new MethWithInst(preDefMethod, predefindType);
			ExprMemberGroup exprMemberGroup = this.GetExprFactory().CreateMemGroup(null, methWithInst);
			ExprCall exprCall = this.GetExprFactory().CreateCall((EXPRFLAG)0, methWithInst.Meth().RetType, expr, exprMemberGroup, methWithInst);
			exprCall.PredefinedMethod = pdm;
			return exprCall;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001ACB0 File Offset: 0x00018EB0
		private ExprCall GenerateCall(PREDEFMETH pdm, Expr arg1, Expr arg2, Expr arg3)
		{
			MethodSymbol preDefMethod = this.GetPreDefMethod(pdm);
			if (preDefMethod == null)
			{
				return null;
			}
			AggregateType predefindType = this.GetSymbolLoader().GetPredefindType(PredefinedType.PT_EXPRESSION);
			Expr expr = this.GetExprFactory().CreateList(arg1, arg2, arg3);
			MethWithInst methWithInst = new MethWithInst(preDefMethod, predefindType);
			ExprMemberGroup exprMemberGroup = this.GetExprFactory().CreateMemGroup(null, methWithInst);
			ExprCall exprCall = this.GetExprFactory().CreateCall((EXPRFLAG)0, methWithInst.Meth().RetType, expr, exprMemberGroup, methWithInst);
			exprCall.PredefinedMethod = pdm;
			return exprCall;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001AD24 File Offset: 0x00018F24
		private ExprCall GenerateCall(PREDEFMETH pdm, Expr arg1, Expr arg2, Expr arg3, Expr arg4)
		{
			MethodSymbol preDefMethod = this.GetPreDefMethod(pdm);
			if (preDefMethod == null)
			{
				return null;
			}
			AggregateType predefindType = this.GetSymbolLoader().GetPredefindType(PredefinedType.PT_EXPRESSION);
			Expr expr = this.GetExprFactory().CreateList(arg1, arg2, arg3, arg4);
			MethWithInst methWithInst = new MethWithInst(preDefMethod, predefindType);
			ExprMemberGroup exprMemberGroup = this.GetExprFactory().CreateMemGroup(null, methWithInst);
			ExprCall exprCall = this.GetExprFactory().CreateCall((EXPRFLAG)0, methWithInst.Meth().RetType, expr, exprMemberGroup, methWithInst);
			exprCall.PredefinedMethod = pdm;
			return exprCall;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001AD98 File Offset: 0x00018F98
		private ExprArrayInit GenerateParamsArray(Expr args, PredefinedType pt)
		{
			int num = ExpressionIterator.Count(args);
			AggregateType predefindType = this.GetSymbolLoader().GetPredefindType(pt);
			ArrayType array = this.GetSymbolLoader().GetTypeManager().GetArray(predefindType, 1, true);
			ExprConstant exprConstant = this.GetExprFactory().CreateIntegerConstant(num);
			return this.GetExprFactory().CreateArrayInit(array, args, exprConstant, new int[] { num }, num);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001ADF4 File Offset: 0x00018FF4
		private void FixLiftedUserDefinedBinaryOperators(ExprBinOp expr, ref Expr pp1, ref Expr pp2)
		{
			MethodSymbol methodSymbol = expr.UserDefinedCallMethod.Meth();
			Expr optionalLeftChild = expr.OptionalLeftChild;
			Expr optionalRightChild = expr.OptionalRightChild;
			Expr expr2 = pp1;
			Expr expr3 = pp2;
			CType ctype = methodSymbol.Params[0];
			CType ctype2 = methodSymbol.Params[1];
			CType type = optionalLeftChild.Type;
			CType type2 = optionalRightChild.Type;
			AggregateType aggregateType;
			AggregateType aggregateType2;
			if ((aggregateType = ctype as AggregateType) == null || !aggregateType.getAggregate().IsValueType() || (aggregateType2 = ctype2 as AggregateType) == null || !aggregateType2.getAggregate().IsValueType())
			{
				return;
			}
			CType nullable = this.GetSymbolLoader().GetTypeManager().GetNullable(ctype);
			CType nullable2 = this.GetSymbolLoader().GetTypeManager().GetNullable(ctype2);
			if (type is NullType || (type == ctype && (type2 == nullable2 || type2 is NullType)))
			{
				expr2 = this.GenerateCall(PREDEFMETH.PM_EXPRESSION_CONVERT, expr2, this.CreateTypeOf(nullable));
			}
			if (type2 is NullType || (type2 == ctype2 && (type == nullable || type is NullType)))
			{
				expr3 = this.GenerateCall(PREDEFMETH.PM_EXPRESSION_CONVERT, expr3, this.CreateTypeOf(nullable2));
			}
			pp1 = expr2;
			pp2 = expr3;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001AF10 File Offset: 0x00019110
		private bool IsNullableValueType(CType pType)
		{
			AggregateType aggregateType;
			return pType is NullableType && (aggregateType = pType.StripNubs() as AggregateType) != null && aggregateType.getAggregate().IsValueType();
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001AF44 File Offset: 0x00019144
		private bool IsNullableValueAccess(Expr pExpr, Expr pObject)
		{
			ExprProperty exprProperty;
			return (exprProperty = pExpr as ExprProperty) != null && exprProperty.MemberGroup.OptionalObject == pObject && pObject.Type is NullableType;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001AF79 File Offset: 0x00019179
		private static bool isEnumToDecimalConversion(CType argtype, CType desttype)
		{
			return argtype.StripNubs().isEnumType() && desttype.StripNubs().isPredefType(PredefinedType.PT_DECIMAL);
		}

		// Token: 0x04000567 RID: 1383
		private ExprFactory expressionFactory;

		// Token: 0x04000568 RID: 1384
		private SymbolLoader symbolLoader;

		// Token: 0x04000569 RID: 1385
		private ExprBoundLambda currentAnonMeth;
	}
}
