using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200003D RID: 61
	internal sealed class ExprFactory
	{
		// Token: 0x0600026A RID: 618 RVA: 0x00011F49 File Offset: 0x00010149
		public ExprFactory(GlobalSymbolContext globalSymbolContext)
		{
			this._globalSymbolContext = globalSymbolContext;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00011F58 File Offset: 0x00010158
		private TypeManager Types
		{
			get
			{
				return this._globalSymbolContext.GetTypes();
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00011F65 File Offset: 0x00010165
		private BSYMMGR GlobalSymbols
		{
			get
			{
				return this._globalSymbolContext.GetGlobalSymbols();
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00011F72 File Offset: 0x00010172
		public ExprCall CreateCall(EXPRFLAG flags, CType type, Expr arguments, ExprMemberGroup memberGroup, MethWithInst method)
		{
			return new ExprCall(type, flags, arguments, memberGroup, method);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00011F80 File Offset: 0x00010180
		public ExprField CreateField(CType type, Expr optionalObject, FieldWithType field, bool isLValue)
		{
			return new ExprField(type, optionalObject, field, isLValue);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00011F8C File Offset: 0x0001018C
		public ExprArrayInit CreateArrayInit(CType type, Expr arguments, Expr argumentDimensions, int[] dimSizes, int dimSize)
		{
			return new ExprArrayInit(type, arguments, argumentDimensions, dimSizes, dimSize);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00011F9A File Offset: 0x0001019A
		public ExprProperty CreateProperty(CType type, Expr optionalObjectThrough, Expr arguments, ExprMemberGroup memberGroup, PropWithType property, MethWithType setMethod)
		{
			return new ExprProperty(type, optionalObjectThrough, arguments, memberGroup, property, setMethod);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00011FAC File Offset: 0x000101AC
		public ExprMemberGroup CreateMemGroup(EXPRFLAG flags, Name name, TypeArray typeArgs, SYMKIND symKind, CType parentType, MethodOrPropertySymbol memberSymbol, Expr obj, CMemberLookupResults memberLookupResults)
		{
			return new ExprMemberGroup(this.Types.GetMethGrpType(), flags, name, typeArgs, symKind, parentType, memberSymbol, obj, memberLookupResults);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00011FD8 File Offset: 0x000101D8
		public ExprMemberGroup CreateMemGroup(Expr obj, MethPropWithInst method)
		{
			Symbol sym = method.Sym;
			Name name = ((sym != null) ? sym.name : null);
			MethodOrPropertySymbol methodOrPropertySymbol = method.MethProp();
			CType ctype = method.GetType() ?? this.Types.GetErrorSym();
			return this.CreateMemGroup((EXPRFLAG)0, name, method.TypeArgs, (methodOrPropertySymbol != null) ? methodOrPropertySymbol.getKind() : SYMKIND.SK_MethodSymbol, method.GetType(), methodOrPropertySymbol, obj, new CMemberLookupResults(this.GlobalSymbols.AllocParams(1, new CType[] { ctype }), name));
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00012053 File Offset: 0x00010253
		public ExprUserDefinedConversion CreateUserDefinedConversion(Expr arg, Expr call, MethWithInst method)
		{
			return new ExprUserDefinedConversion(arg, call, method);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0001205D File Offset: 0x0001025D
		public ExprCast CreateCast(CType type, Expr argument)
		{
			return this.CreateCast((EXPRFLAG)0, this.CreateClass(type), argument);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0001206E File Offset: 0x0001026E
		public ExprCast CreateCast(EXPRFLAG flags, ExprClass type, Expr argument)
		{
			return new ExprCast(flags, type, argument);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00012078 File Offset: 0x00010278
		public ExprReturn CreateReturn(Expr optionalObject)
		{
			return new ExprReturn(optionalObject);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00012080 File Offset: 0x00010280
		public ExprLocal CreateLocal(LocalVariableSymbol local)
		{
			return new ExprLocal(local);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00012088 File Offset: 0x00010288
		public ExprBoundLambda CreateAnonymousMethod(AggregateType delegateType, Scope argumentScope)
		{
			return new ExprBoundLambda(delegateType, argumentScope);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00012091 File Offset: 0x00010291
		public ExprHoistedLocalExpr CreateHoistedLocalInExpression()
		{
			return new ExprHoistedLocalExpr(this.Types.GetPredefAgg(PredefinedType.PT_EXPRESSION).getThisType());
		}

		// Token: 0x0600027A RID: 634 RVA: 0x000120AA File Offset: 0x000102AA
		public ExprMethodInfo CreateMethodInfo(MethPropWithInst mwi)
		{
			return this.CreateMethodInfo(mwi.Meth(), mwi.GetType(), mwi.TypeArgs);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x000120C4 File Offset: 0x000102C4
		public ExprMethodInfo CreateMethodInfo(MethodSymbol method, AggregateType methodType, TypeArray methodParameters)
		{
			return new ExprMethodInfo(this.Types.GetPredefAgg(method.IsConstructor() ? PredefinedType.PT_CONSTRUCTORINFO : PredefinedType.PT_METHODINFO).getThisType(), method, methodType, methodParameters);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000120EC File Offset: 0x000102EC
		public ExprPropertyInfo CreatePropertyInfo(PropertySymbol prop, AggregateType propertyType)
		{
			return new ExprPropertyInfo(this.Types.GetPredefAgg(PredefinedType.PT_PROPERTYINFO).getThisType(), prop, propertyType);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00012107 File Offset: 0x00010307
		public ExprFieldInfo CreateFieldInfo(FieldSymbol field, AggregateType fieldType)
		{
			return new ExprFieldInfo(field, fieldType, this.Types.GetPredefAgg(PredefinedType.PT_FIELDINFO).getThisType());
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00012122 File Offset: 0x00010322
		private ExprTypeOf CreateTypeOf(ExprClass sourceType)
		{
			return new ExprTypeOf(this.Types.GetPredefAgg(PredefinedType.PT_TYPE).getThisType(), sourceType);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0001213C File Offset: 0x0001033C
		public ExprTypeOf CreateTypeOf(CType sourceType)
		{
			return this.CreateTypeOf(this.CreateClass(sourceType));
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0001214B File Offset: 0x0001034B
		public ExprUserLogicalOp CreateUserLogOp(CType type, Expr trueFalseCall, ExprCall operatorCall)
		{
			return new ExprUserLogicalOp(type, trueFalseCall, operatorCall);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00012155 File Offset: 0x00010355
		public ExprConcat CreateConcat(Expr first, Expr second)
		{
			return new ExprConcat(first, second);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0001215E File Offset: 0x0001035E
		public ExprConstant CreateStringConstant(string str)
		{
			return this.CreateConstant(this.Types.GetPredefAgg(PredefinedType.PT_STRING).getThisType(), ConstVal.Get(str));
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0001217E File Offset: 0x0001037E
		public ExprMultiGet CreateMultiGet(EXPRFLAG flags, CType type, ExprMulti multi)
		{
			return new ExprMultiGet(type, flags, multi);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00012188 File Offset: 0x00010388
		public ExprMulti CreateMulti(EXPRFLAG flags, CType type, Expr left, Expr op)
		{
			return new ExprMulti(type, flags, left, op);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00012194 File Offset: 0x00010394
		public Expr CreateZeroInit(CType type)
		{
			if (type.isEnumType())
			{
				return this.CreateConstant(type, ConstVal.Get(Activator.CreateInstance(type.AssociatedSystemType)));
			}
			switch (type.fundType())
			{
			case FUNDTYPE.FT_STRUCT:
				if (type.isPredefType(PredefinedType.PT_DECIMAL))
				{
					goto IL_0063;
				}
				break;
			case FUNDTYPE.FT_PTR:
				return this.CreateCast((EXPRFLAG)0, this.CreateClass(type), this.CreateNull());
			case FUNDTYPE.FT_VAR:
				break;
			default:
				goto IL_0063;
			}
			return new ExprZeroInit(type);
			IL_0063:
			return this.CreateConstant(type, ConstVal.GetDefaultValue(type.constValKind()));
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00012216 File Offset: 0x00010416
		public ExprConstant CreateConstant(CType type, ConstVal constVal)
		{
			return new ExprConstant(type, constVal);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0001221F File Offset: 0x0001041F
		public ExprConstant CreateIntegerConstant(int x)
		{
			return this.CreateConstant(this.Types.GetPredefAgg(PredefinedType.PT_INT).getThisType(), ConstVal.Get(x));
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0001223E File Offset: 0x0001043E
		public ExprConstant CreateBoolConstant(bool b)
		{
			return this.CreateConstant(this.Types.GetPredefAgg(PredefinedType.PT_BOOL).getThisType(), ConstVal.Get(b));
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0001225D File Offset: 0x0001045D
		public ExprBlock CreateBlock(ExprStatement pOptionalStatements)
		{
			return new ExprBlock(pOptionalStatements);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00012265 File Offset: 0x00010465
		public ExprArrayIndex CreateArrayIndex(CType type, Expr array, Expr index)
		{
			return new ExprArrayIndex(type, array, index);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0001226F File Offset: 0x0001046F
		public ExprBinOp CreateBinop(ExpressionKind exprKind, CType type, Expr left, Expr right)
		{
			return new ExprBinOp(exprKind, type, left, right);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0001227B File Offset: 0x0001047B
		public ExprUnaryOp CreateUnaryOp(ExpressionKind exprKind, CType type, Expr operand)
		{
			return new ExprUnaryOp(exprKind, type, operand);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00012285 File Offset: 0x00010485
		public ExprOperator CreateOperator(ExpressionKind exprKind, CType type, Expr arg1, Expr arg2)
		{
			if (!exprKind.IsUnaryOperator())
			{
				return this.CreateBinop(exprKind, type, arg1, arg2);
			}
			return this.CreateUnaryOp(exprKind, type, arg1);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000122A4 File Offset: 0x000104A4
		public ExprBinOp CreateUserDefinedBinop(ExpressionKind exprKind, CType type, Expr left, Expr right, Expr call, MethPropWithInst userMethod)
		{
			return new ExprBinOp(exprKind, type, left, right, call, userMethod);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000122B4 File Offset: 0x000104B4
		public ExprUnaryOp CreateUserDefinedUnaryOperator(ExpressionKind exprKind, CType type, Expr operand, ExprCall call, MethPropWithInst userMethod)
		{
			return new ExprUnaryOp(exprKind, type, operand, call, userMethod);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000122C2 File Offset: 0x000104C2
		public ExprUnaryOp CreateNeg(EXPRFLAG flags, Expr operand)
		{
			ExprUnaryOp exprUnaryOp = this.CreateUnaryOp(ExpressionKind.Negate, operand.Type, operand);
			exprUnaryOp.Flags |= flags;
			return exprUnaryOp;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000122E1 File Offset: 0x000104E1
		public ExprBinOp CreateSequence(Expr first, Expr second)
		{
			return this.CreateBinop(ExpressionKind.Sequence, second.Type, first, second);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000122F3 File Offset: 0x000104F3
		public ExprBinOp CreateReverseSequence(Expr first, Expr second)
		{
			return this.CreateBinop(ExpressionKind.SequenceReverse, first.Type, first, second);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00012305 File Offset: 0x00010505
		public ExprAssignment CreateAssignment(Expr left, Expr right)
		{
			return new ExprAssignment(left, right);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0001230E File Offset: 0x0001050E
		public ExprNamedArgumentSpecification CreateNamedArgumentSpecification(Name name, Expr value)
		{
			return new ExprNamedArgumentSpecification(name, value);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00012317 File Offset: 0x00010517
		public ExprWrap CreateWrap(Expr expression)
		{
			return new ExprWrap(expression);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0001231F File Offset: 0x0001051F
		public ExprBinOp CreateSave(ExprWrap wrap)
		{
			ExprBinOp exprBinOp = this.CreateBinop(ExpressionKind.Save, wrap.Type, wrap.OptionalExpression, wrap);
			exprBinOp.SetAssignment();
			return exprBinOp;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0001233C File Offset: 0x0001053C
		public ExprConstant CreateNull()
		{
			return this.CreateConstant(this.Types.GetNullType(), default(ConstVal));
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00012364 File Offset: 0x00010564
		public void AppendItemToList(Expr newItem, ref Expr first, ref Expr last)
		{
			if (newItem == null)
			{
				return;
			}
			if (first == null)
			{
				first = newItem;
				last = newItem;
				return;
			}
			if (first.Kind != ExpressionKind.List)
			{
				first = this.CreateList(first, newItem);
				last = first;
				return;
			}
			ExprList exprList = (ExprList)last;
			exprList.OptionalNextListNode = this.CreateList(exprList.OptionalNextListNode, newItem);
			last = exprList.OptionalNextListNode;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x000123BD File Offset: 0x000105BD
		public ExprList CreateList(Expr op1, Expr op2)
		{
			return new ExprList(op1, op2);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x000123C6 File Offset: 0x000105C6
		public ExprList CreateList(Expr op1, Expr op2, Expr op3)
		{
			return this.CreateList(op1, this.CreateList(op2, op3));
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000123D7 File Offset: 0x000105D7
		public ExprList CreateList(Expr op1, Expr op2, Expr op3, Expr op4)
		{
			return this.CreateList(op1, this.CreateList(op2, this.CreateList(op3, op4)));
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000123F0 File Offset: 0x000105F0
		public ExprClass CreateClass(CType type)
		{
			return new ExprClass(type);
		}

		// Token: 0x040002FB RID: 763
		private readonly GlobalSymbolContext _globalSymbolContext;
	}
}
