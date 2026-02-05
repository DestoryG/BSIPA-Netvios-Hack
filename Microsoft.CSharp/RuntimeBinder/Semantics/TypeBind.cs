using System;
using Microsoft.CSharp.RuntimeBinder.Errors;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000A1 RID: 161
	internal static class TypeBind
	{
		// Token: 0x06000570 RID: 1392 RVA: 0x0001AFA4 File Offset: 0x000191A4
		public static bool CheckConstraints(CSemanticChecker checker, ErrorHandling errHandling, CType type, CheckConstraintsFlags flags)
		{
			type = type.GetNakedType(false);
			NullableType nullableType;
			if ((nullableType = type as NullableType) != null)
			{
				type = nullableType.GetAts();
			}
			AggregateType aggregateType;
			if ((aggregateType = type as AggregateType) == null)
			{
				return true;
			}
			if (aggregateType.GetTypeArgsAll().Count == 0)
			{
				aggregateType.fConstraintsChecked = true;
				aggregateType.fConstraintError = false;
				return true;
			}
			if (aggregateType.fConstraintsChecked && (!aggregateType.fConstraintError || (flags & CheckConstraintsFlags.NoDupErrors) != CheckConstraintsFlags.None))
			{
				return !aggregateType.fConstraintError;
			}
			TypeArray typeVars = aggregateType.getAggregate().GetTypeVars();
			TypeArray typeArgsThis = aggregateType.GetTypeArgsThis();
			TypeArray typeArgsAll = aggregateType.GetTypeArgsAll();
			if (!aggregateType.fConstraintsChecked)
			{
				aggregateType.fConstraintsChecked = true;
				aggregateType.fConstraintError = false;
			}
			if (aggregateType.outerType != null && ((flags & CheckConstraintsFlags.Outer) != CheckConstraintsFlags.None || !aggregateType.outerType.fConstraintsChecked))
			{
				TypeBind.CheckConstraints(checker, errHandling, aggregateType.outerType, flags);
				aggregateType.fConstraintError |= aggregateType.outerType.fConstraintError;
			}
			if (typeVars.Count > 0)
			{
				aggregateType.fConstraintError |= !TypeBind.CheckConstraintsCore(checker, errHandling, aggregateType.getAggregate(), typeVars, typeArgsThis, typeArgsAll, null, flags & CheckConstraintsFlags.NoErrors);
			}
			for (int i = 0; i < typeArgsThis.Count; i++)
			{
				AggregateType aggregateType2;
				if ((aggregateType2 = typeArgsThis[i].GetNakedType(true) as AggregateType) != null && !aggregateType2.fConstraintsChecked)
				{
					TypeBind.CheckConstraints(checker, errHandling, aggregateType2, flags | CheckConstraintsFlags.Outer);
					if (aggregateType2.fConstraintError)
					{
						aggregateType.fConstraintError = true;
					}
				}
			}
			return !aggregateType.fConstraintError;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001B110 File Offset: 0x00019310
		public static void CheckMethConstraints(CSemanticChecker checker, ErrorHandling errCtx, MethWithInst mwi)
		{
			if (mwi.TypeArgs.Count > 0)
			{
				TypeBind.CheckConstraintsCore(checker, errCtx, mwi.Meth(), mwi.Meth().typeVars, mwi.TypeArgs, mwi.GetType().GetTypeArgsAll(), mwi.TypeArgs, CheckConstraintsFlags.None);
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001B15C File Offset: 0x0001935C
		private static bool CheckConstraintsCore(CSemanticChecker checker, ErrorHandling errHandling, Symbol symErr, TypeArray typeVars, TypeArray typeArgs, TypeArray typeArgsCls, TypeArray typeArgsMeth, CheckConstraintsFlags flags)
		{
			bool flag = false;
			for (int i = 0; i < typeVars.Count; i++)
			{
				TypeParameterType typeParameterType = (TypeParameterType)typeVars[i];
				CType ctype = typeArgs[i];
				bool flag2 = TypeBind.CheckSingleConstraint(checker, errHandling, symErr, typeParameterType, ctype, typeArgsCls, typeArgsMeth, flags);
				flag |= !flag2;
			}
			return !flag;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001B1B0 File Offset: 0x000193B0
		private static bool CheckSingleConstraint(CSemanticChecker checker, ErrorHandling errHandling, Symbol symErr, TypeParameterType var, CType arg, TypeArray typeArgsCls, TypeArray typeArgsMeth, CheckConstraintsFlags flags)
		{
			bool flag = (flags & CheckConstraintsFlags.NoErrors) == CheckConstraintsFlags.None;
			if (arg is ErrorType)
			{
				return false;
			}
			bool flag2 = false;
			if (var.HasRefConstraint() && !arg.IsRefType())
			{
				if (flag)
				{
					throw errHandling.Error(ErrorCode.ERR_RefConstraintNotSatisfied, new ErrArg[]
					{
						symErr,
						new ErrArgNoRef(var),
						arg
					});
				}
				flag2 = true;
			}
			TypeArray typeArray = checker.SymbolLoader.GetTypeManager().SubstTypeArray(var.GetBounds(), typeArgsCls, typeArgsMeth);
			int num = 0;
			if (var.HasValConstraint())
			{
				if (!arg.IsValType() || arg is NullableType)
				{
					if (flag)
					{
						throw errHandling.Error(ErrorCode.ERR_ValConstraintNotSatisfied, new ErrArg[]
						{
							symErr,
							new ErrArgNoRef(var),
							arg
						});
					}
					flag2 = true;
				}
				if (typeArray.Count != 0 && typeArray[0].isPredefType(PredefinedType.PT_VALUE))
				{
					num = 1;
				}
			}
			for (int i = num; i < typeArray.Count; i++)
			{
				CType ctype = typeArray[i];
				if (!TypeBind.SatisfiesBound(checker, arg, ctype))
				{
					if (flag)
					{
						ErrorCode errorCode;
						NullableType nullableType;
						if (arg.IsRefType())
						{
							errorCode = ErrorCode.ERR_GenericConstraintNotSatisfiedRefType;
						}
						else if ((nullableType = arg as NullableType) != null && checker.SymbolLoader.HasBaseConversion(nullableType.GetUnderlyingType(), ctype))
						{
							if (ctype.isPredefType(PredefinedType.PT_ENUM) || nullableType.GetUnderlyingType() == ctype)
							{
								errorCode = ErrorCode.ERR_GenericConstraintNotSatisfiedNullableEnum;
							}
							else
							{
								errorCode = ErrorCode.ERR_GenericConstraintNotSatisfiedNullableInterface;
							}
						}
						else
						{
							errorCode = ErrorCode.ERR_GenericConstraintNotSatisfiedValType;
						}
						throw errHandling.Error(errorCode, new ErrArg[]
						{
							new ErrArg(symErr),
							new ErrArg(ctype, ErrArgFlags.Unique),
							var,
							new ErrArg(arg, ErrArgFlags.Unique)
						});
					}
					flag2 = true;
				}
			}
			if (!var.HasNewConstraint() || arg.IsValType())
			{
				return !flag2;
			}
			if (arg.isClassType())
			{
				AggregateSymbol aggregate = ((AggregateType)arg).getAggregate();
				checker.SymbolLoader.LookupAggMember(NameManager.GetPredefinedName(PredefinedName.PN_CTOR), aggregate, symbmask_t.MASK_ALL);
				if (aggregate.HasPubNoArgCtor() && !aggregate.IsAbstract())
				{
					return !flag2;
				}
			}
			if (flag)
			{
				throw errHandling.Error(ErrorCode.ERR_NewConstraintNotSatisfied, new ErrArg[]
				{
					symErr,
					new ErrArgNoRef(var),
					arg
				});
			}
			return false;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001B3FC File Offset: 0x000195FC
		private static bool SatisfiesBound(CSemanticChecker checker, CType arg, CType typeBnd)
		{
			if (typeBnd == arg)
			{
				return true;
			}
			switch (typeBnd.GetTypeKind())
			{
			case TypeKind.TK_AggregateType:
			case TypeKind.TK_ArrayType:
			case TypeKind.TK_TypeParameterType:
				break;
			case TypeKind.TK_VoidType:
			case TypeKind.TK_ErrorType:
			case TypeKind.TK_PointerType:
				return false;
			default:
				return false;
			case TypeKind.TK_NullableType:
				typeBnd = ((NullableType)typeBnd).GetAts();
				break;
			}
			switch (arg.GetTypeKind())
			{
			case TypeKind.TK_AggregateType:
			case TypeKind.TK_ArrayType:
			case TypeKind.TK_TypeParameterType:
				break;
			default:
				return false;
			case TypeKind.TK_ErrorType:
			case TypeKind.TK_PointerType:
				return false;
			case TypeKind.TK_NullableType:
				arg = ((NullableType)arg).GetAts();
				break;
			}
			return checker.SymbolLoader.HasBaseConversion(arg, typeBnd);
		}
	}
}
