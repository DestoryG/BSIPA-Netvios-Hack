using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000B1 RID: 177
	internal sealed class TypeManager
	{
		// Token: 0x060005EA RID: 1514 RVA: 0x0001CA30 File Offset: 0x0001AC30
		public TypeManager(BSYMMGR bsymmgr, PredefinedTypes predefTypes)
		{
			this._typeFactory = new TypeFactory();
			this._typeTable = new TypeTable();
			this._errorType = this._typeFactory.CreateError(null, null, null);
			this._voidType = this._typeFactory.CreateVoid();
			this._nullType = this._typeFactory.CreateNull();
			this._typeMethGrp = this._typeFactory.CreateMethodGroup();
			this._argListType = this._typeFactory.CreateArgList();
			this._errorType.SetErrors(true);
			this._stvcMethod = new TypeManager.StdTypeVarColl();
			this._stvcClass = new TypeManager.StdTypeVarColl();
			this._BSymmgr = bsymmgr;
			this._predefTypes = predefTypes;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001CAEC File Offset: 0x0001ACEC
		public void InitTypeFactory(SymbolTable table)
		{
			this._symbolTable = table;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001CAF8 File Offset: 0x0001ACF8
		public ArrayType GetArray(CType elementType, int args, bool isSZArray)
		{
			if (args != 1)
			{
				if (args != 2)
				{
					goto IL_0018;
				}
			}
			else if (!isSZArray)
			{
				goto IL_0018;
			}
			Name name = NameManager.GetPredefinedName(PredefinedName.PN_ARRAY0 + args);
			goto IL_003F;
			IL_0018:
			name = this._BSymmgr.GetNameManager().Add("[X" + args + 1);
			IL_003F:
			ArrayType arrayType = this._typeTable.LookupArray(name, elementType);
			if (arrayType == null)
			{
				arrayType = this._typeFactory.CreateArray(name, elementType, args, isSZArray);
				arrayType.InitFromParent();
				this._typeTable.InsertArray(name, elementType, arrayType);
			}
			return arrayType;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001CB7C File Offset: 0x0001AD7C
		public AggregateType GetAggregate(AggregateSymbol agg, AggregateType atsOuter, TypeArray typeArgs)
		{
			if (typeArgs == null)
			{
				typeArgs = BSYMMGR.EmptyTypeArray();
			}
			Name nameFromPtrs = this._BSymmgr.GetNameFromPtrs(typeArgs, atsOuter);
			AggregateType aggregateType = this._typeTable.LookupAggregate(nameFromPtrs, agg);
			if (aggregateType == null)
			{
				aggregateType = this._typeFactory.CreateAggregateType(nameFromPtrs, agg, typeArgs, atsOuter);
				aggregateType.SetErrors(false);
				this._typeTable.InsertAggregate(nameFromPtrs, agg, aggregateType);
				Type associatedSystemType = aggregateType.AssociatedSystemType;
				Type type = ((associatedSystemType != null) ? associatedSystemType.BaseType : null);
				if (type != null)
				{
					AggregateType baseClass = agg.GetBaseClass();
					agg.SetBaseClass(this._symbolTable.GetCTypeFromType(type) as AggregateType);
					aggregateType.GetBaseClass();
					agg.SetBaseClass(baseClass);
				}
			}
			return aggregateType;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001CC20 File Offset: 0x0001AE20
		public AggregateType GetAggregate(AggregateSymbol agg, TypeArray typeArgsAll)
		{
			if (typeArgsAll.Count == 0)
			{
				return agg.getThisType();
			}
			AggregateSymbol outerAgg = agg.GetOuterAgg();
			if (outerAgg == null)
			{
				return this.GetAggregate(agg, null, typeArgsAll);
			}
			int count = outerAgg.GetTypeVarsAll().Count;
			TypeArray typeArray = this._BSymmgr.AllocParams(count, typeArgsAll, 0);
			TypeArray typeArray2 = this._BSymmgr.AllocParams(agg.GetTypeVars().Count, typeArgsAll, count);
			AggregateType aggregate = this.GetAggregate(outerAgg, typeArray);
			return this.GetAggregate(agg, aggregate, typeArray2);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001CC98 File Offset: 0x0001AE98
		public PointerType GetPointer(CType baseType)
		{
			PointerType pointerType = this._typeTable.LookupPointer(baseType);
			if (pointerType == null)
			{
				Name predefinedName = NameManager.GetPredefinedName(PredefinedName.PN_PTR);
				pointerType = this._typeFactory.CreatePointer(predefinedName, baseType);
				pointerType.InitFromParent();
				this._typeTable.InsertPointer(baseType, pointerType);
			}
			return pointerType;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001CCE0 File Offset: 0x0001AEE0
		public NullableType GetNullable(CType pUnderlyingType)
		{
			NullableType nullableType;
			if ((nullableType = pUnderlyingType as NullableType) != null)
			{
				return nullableType;
			}
			NullableType nullableType2 = this._typeTable.LookupNullable(pUnderlyingType);
			if (nullableType2 == null)
			{
				Name predefinedName = NameManager.GetPredefinedName(PredefinedName.PN_NUB);
				nullableType2 = this._typeFactory.CreateNullable(predefinedName, pUnderlyingType, this._BSymmgr, this);
				nullableType2.InitFromParent();
				this._typeTable.InsertNullable(pUnderlyingType, nullableType2);
			}
			return nullableType2;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001CD39 File Offset: 0x0001AF39
		public NullableType GetNubFromNullable(AggregateType ats)
		{
			return this.GetNullable(ats.GetTypeArgsAll()[0]);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001CD50 File Offset: 0x0001AF50
		public ParameterModifierType GetParameterModifier(CType paramType, bool isOut)
		{
			Name predefinedName = NameManager.GetPredefinedName(isOut ? PredefinedName.PN_OUTPARAM : PredefinedName.PN_REFPARAM);
			ParameterModifierType parameterModifierType = this._typeTable.LookupParameterModifier(predefinedName, paramType);
			if (parameterModifierType == null)
			{
				parameterModifierType = this._typeFactory.CreateParameterModifier(predefinedName, paramType);
				parameterModifierType.isOut = isOut;
				parameterModifierType.InitFromParent();
				this._typeTable.InsertParameterModifier(predefinedName, paramType, parameterModifierType);
			}
			return parameterModifierType;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001CDA8 File Offset: 0x0001AFA8
		public ErrorType GetErrorType(Name nameText, TypeArray typeArgs)
		{
			if (typeArgs == null)
			{
				typeArgs = BSYMMGR.EmptyTypeArray();
			}
			Name nameFromPtrs = this._BSymmgr.GetNameFromPtrs(nameText, typeArgs);
			ErrorType errorType = this._typeTable.LookupError(nameFromPtrs);
			if (errorType == null)
			{
				errorType = this._typeFactory.CreateError(nameFromPtrs, nameText, typeArgs);
				errorType.SetErrors(true);
				this._typeTable.InsertError(nameFromPtrs, errorType);
			}
			return errorType;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001CE01 File Offset: 0x0001B001
		public VoidType GetVoid()
		{
			return this._voidType;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001CE09 File Offset: 0x0001B009
		public NullType GetNullType()
		{
			return this._nullType;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001CE11 File Offset: 0x0001B011
		public MethodGroupType GetMethGrpType()
		{
			return this._typeMethGrp;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001CE19 File Offset: 0x0001B019
		public ArgumentListType GetArgListType()
		{
			return this._argListType;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001CE21 File Offset: 0x0001B021
		public ErrorType GetErrorSym()
		{
			return this._errorType;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001CE29 File Offset: 0x0001B029
		public AggregateSymbol GetNullable()
		{
			return this.GetPredefAgg(PredefinedType.PT_G_OPTIONAL);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0001CE34 File Offset: 0x0001B034
		private CType SubstType(CType typeSrc, TypeArray typeArgsCls, TypeArray typeArgsMeth, SubstTypeFlags grfst)
		{
			if (typeSrc == null)
			{
				return null;
			}
			SubstContext substContext = new SubstContext(typeArgsCls, typeArgsMeth, grfst);
			if (!substContext.FNop())
			{
				return this.SubstTypeCore(typeSrc, substContext);
			}
			return typeSrc;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001CE62 File Offset: 0x0001B062
		public CType SubstType(CType typeSrc, TypeArray typeArgsCls)
		{
			return this.SubstType(typeSrc, typeArgsCls, null, SubstTypeFlags.NormNone);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001CE6E File Offset: 0x0001B06E
		private CType SubstType(CType typeSrc, TypeArray typeArgsCls, TypeArray typeArgsMeth)
		{
			return this.SubstType(typeSrc, typeArgsCls, typeArgsMeth, SubstTypeFlags.NormNone);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001CE7C File Offset: 0x0001B07C
		public TypeArray SubstTypeArray(TypeArray taSrc, SubstContext pctx)
		{
			if (taSrc == null || taSrc.Count == 0 || pctx == null || pctx.FNop())
			{
				return taSrc;
			}
			CType[] array = new CType[taSrc.Count];
			for (int i = 0; i < taSrc.Count; i++)
			{
				array[i] = this.SubstTypeCore(taSrc[i], pctx);
			}
			return this._BSymmgr.AllocParams(taSrc.Count, array);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0001CEE4 File Offset: 0x0001B0E4
		private TypeArray SubstTypeArray(TypeArray taSrc, TypeArray typeArgsCls, TypeArray typeArgsMeth, SubstTypeFlags grfst)
		{
			if (taSrc == null || taSrc.Count == 0)
			{
				return taSrc;
			}
			SubstContext substContext = new SubstContext(typeArgsCls, typeArgsMeth, grfst);
			if (substContext.FNop())
			{
				return taSrc;
			}
			CType[] array = new CType[taSrc.Count];
			for (int i = 0; i < taSrc.Count; i++)
			{
				array[i] = this.SubstTypeCore(taSrc[i], substContext);
			}
			return this._BSymmgr.AllocParams(taSrc.Count, array);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001CF52 File Offset: 0x0001B152
		public TypeArray SubstTypeArray(TypeArray taSrc, TypeArray typeArgsCls, TypeArray typeArgsMeth)
		{
			return this.SubstTypeArray(taSrc, typeArgsCls, typeArgsMeth, SubstTypeFlags.NormNone);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001CF5E File Offset: 0x0001B15E
		public TypeArray SubstTypeArray(TypeArray taSrc, TypeArray typeArgsCls)
		{
			return this.SubstTypeArray(taSrc, typeArgsCls, null, SubstTypeFlags.NormNone);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001CF6C File Offset: 0x0001B16C
		private CType SubstTypeCore(CType type, SubstContext pctx)
		{
			switch (type.GetTypeKind())
			{
			case TypeKind.TK_AggregateType:
			{
				AggregateType aggregateType = (AggregateType)type;
				if (aggregateType.GetTypeArgsAll().Count > 0)
				{
					TypeArray typeArray = this.SubstTypeArray(aggregateType.GetTypeArgsAll(), pctx);
					if (aggregateType.GetTypeArgsAll() != typeArray)
					{
						return this.GetAggregate(aggregateType.getAggregate(), typeArray);
					}
				}
				return type;
			}
			case TypeKind.TK_VoidType:
			case TypeKind.TK_NullType:
			case TypeKind.TK_MethodGroupType:
			case TypeKind.TK_ArgumentListType:
				return type;
			case TypeKind.TK_ErrorType:
			{
				ErrorType errorType = (ErrorType)type;
				if (errorType.HasParent)
				{
					TypeArray typeArray2 = this.SubstTypeArray(errorType.typeArgs, pctx);
					if (typeArray2 != errorType.typeArgs)
					{
						return this.GetErrorType(errorType.nameText, typeArray2);
					}
				}
				return type;
			}
			case TypeKind.TK_ArrayType:
			{
				ArrayType arrayType = (ArrayType)type;
				CType ctype2;
				CType ctype = this.SubstTypeCore(ctype2 = arrayType.GetElementType(), pctx);
				if (ctype != ctype2)
				{
					return this.GetArray(ctype, arrayType.rank, arrayType.IsSZArray);
				}
				return type;
			}
			case TypeKind.TK_PointerType:
			{
				CType ctype2;
				CType ctype = this.SubstTypeCore(ctype2 = ((PointerType)type).GetReferentType(), pctx);
				if (ctype != ctype2)
				{
					return this.GetPointer(ctype);
				}
				return type;
			}
			case TypeKind.TK_ParameterModifierType:
			{
				ParameterModifierType parameterModifierType = (ParameterModifierType)type;
				CType ctype2;
				CType ctype = this.SubstTypeCore(ctype2 = parameterModifierType.GetParameterType(), pctx);
				if (ctype != ctype2)
				{
					return this.GetParameterModifier(ctype, parameterModifierType.isOut);
				}
				return type;
			}
			case TypeKind.TK_NullableType:
			{
				CType ctype2;
				CType ctype = this.SubstTypeCore(ctype2 = ((NullableType)type).GetUnderlyingType(), pctx);
				if (ctype != ctype2)
				{
					return this.GetNullable(ctype);
				}
				return type;
			}
			case TypeKind.TK_TypeParameterType:
			{
				TypeParameterSymbol typeParameterSymbol = ((TypeParameterType)type).GetTypeParameterSymbol();
				int indexInTotalParameters = typeParameterSymbol.GetIndexInTotalParameters();
				if (typeParameterSymbol.IsMethodTypeParameter())
				{
					if ((pctx.grfst & SubstTypeFlags.DenormMeth) != SubstTypeFlags.NormNone && typeParameterSymbol.parent != null)
					{
						return type;
					}
					if (indexInTotalParameters < pctx.ctypeMeth)
					{
						return pctx.prgtypeMeth[indexInTotalParameters];
					}
					if ((pctx.grfst & SubstTypeFlags.NormMeth) == SubstTypeFlags.NormNone)
					{
						return type;
					}
					return this.GetStdMethTypeVar(indexInTotalParameters);
				}
				else
				{
					if ((pctx.grfst & SubstTypeFlags.DenormClass) != SubstTypeFlags.NormNone && typeParameterSymbol.parent != null)
					{
						return type;
					}
					if (indexInTotalParameters < pctx.ctypeCls)
					{
						return pctx.prgtypeCls[indexInTotalParameters];
					}
					if ((pctx.grfst & SubstTypeFlags.NormClass) == SubstTypeFlags.NormNone)
					{
						return type;
					}
					return this.GetStdClsTypeVar(indexInTotalParameters);
				}
				break;
			}
			default:
				return type;
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001D178 File Offset: 0x0001B378
		public bool SubstEqualTypes(CType typeDst, CType typeSrc, TypeArray typeArgsCls, TypeArray typeArgsMeth, SubstTypeFlags grfst)
		{
			if (typeDst.Equals(typeSrc))
			{
				return true;
			}
			SubstContext substContext = new SubstContext(typeArgsCls, typeArgsMeth, grfst);
			return !substContext.FNop() && this.SubstEqualTypesCore(typeDst, typeSrc, substContext);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001D1B0 File Offset: 0x0001B3B0
		public bool SubstEqualTypeArrays(TypeArray taDst, TypeArray taSrc, TypeArray typeArgsCls, TypeArray typeArgsMeth, SubstTypeFlags grfst)
		{
			if (taDst == taSrc || (taDst != null && taDst.Equals(taSrc)))
			{
				return true;
			}
			if (taDst.Count != taSrc.Count)
			{
				return false;
			}
			if (taDst.Count == 0)
			{
				return true;
			}
			SubstContext substContext = new SubstContext(typeArgsCls, typeArgsMeth, grfst);
			if (substContext.FNop())
			{
				return false;
			}
			for (int i = 0; i < taDst.Count; i++)
			{
				if (!this.SubstEqualTypesCore(taDst[i], taSrc[i], substContext))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001D22C File Offset: 0x0001B42C
		private bool SubstEqualTypesCore(CType typeDst, CType typeSrc, SubstContext pctx)
		{
			while (typeDst != typeSrc && !typeDst.Equals(typeSrc))
			{
				switch (typeSrc.GetTypeKind())
				{
				case TypeKind.TK_AggregateType:
				{
					AggregateType aggregateType;
					if ((aggregateType = typeDst as AggregateType) == null)
					{
						return false;
					}
					AggregateType aggregateType2 = (AggregateType)typeSrc;
					if (aggregateType2.getAggregate() != aggregateType.getAggregate())
					{
						return false;
					}
					for (int i = 0; i < aggregateType2.GetTypeArgsAll().Count; i++)
					{
						if (!this.SubstEqualTypesCore(aggregateType.GetTypeArgsAll()[i], aggregateType2.GetTypeArgsAll()[i], pctx))
						{
							return false;
						}
					}
					return true;
				}
				case TypeKind.TK_VoidType:
				case TypeKind.TK_NullType:
					return false;
				default:
					return false;
				case TypeKind.TK_ErrorType:
				{
					ErrorType errorType = (ErrorType)typeSrc;
					ErrorType errorType2;
					if ((errorType2 = typeDst as ErrorType) == null || !errorType.HasParent || !errorType2.HasParent)
					{
						return false;
					}
					if (errorType.nameText != errorType2.nameText || errorType.typeArgs.Count != errorType2.typeArgs.Count || errorType.HasParent != errorType2.HasParent)
					{
						return false;
					}
					for (int j = 0; j < errorType.typeArgs.Count; j++)
					{
						if (!this.SubstEqualTypesCore(errorType2.typeArgs[j], errorType.typeArgs[j], pctx))
						{
							return false;
						}
					}
					return true;
				}
				case TypeKind.TK_ArrayType:
				{
					ArrayType arrayType = (ArrayType)typeSrc;
					ArrayType arrayType2;
					if ((arrayType2 = typeDst as ArrayType) == null || arrayType2.rank != arrayType.rank || arrayType2.IsSZArray != arrayType.IsSZArray)
					{
						return false;
					}
					break;
				}
				case TypeKind.TK_PointerType:
				case TypeKind.TK_NullableType:
					if (typeDst.GetTypeKind() != typeSrc.GetTypeKind())
					{
						return false;
					}
					break;
				case TypeKind.TK_ParameterModifierType:
				{
					ParameterModifierType parameterModifierType;
					if ((parameterModifierType = typeDst as ParameterModifierType) == null || ((pctx.grfst & SubstTypeFlags.NoRefOutDifference) == SubstTypeFlags.NormNone && parameterModifierType.isOut != ((ParameterModifierType)typeSrc).isOut))
					{
						return false;
					}
					break;
				}
				case TypeKind.TK_TypeParameterType:
				{
					TypeParameterSymbol typeParameterSymbol = ((TypeParameterType)typeSrc).GetTypeParameterSymbol();
					int indexInTotalParameters = typeParameterSymbol.GetIndexInTotalParameters();
					if (typeParameterSymbol.IsMethodTypeParameter())
					{
						if ((pctx.grfst & SubstTypeFlags.DenormMeth) != SubstTypeFlags.NormNone && typeParameterSymbol.parent != null)
						{
							return false;
						}
						if (indexInTotalParameters < pctx.ctypeMeth && pctx.prgtypeMeth != null)
						{
							return typeDst == pctx.prgtypeMeth[indexInTotalParameters];
						}
						if ((pctx.grfst & SubstTypeFlags.NormMeth) != SubstTypeFlags.NormNone)
						{
							return typeDst == this.GetStdMethTypeVar(indexInTotalParameters);
						}
					}
					else
					{
						if ((pctx.grfst & SubstTypeFlags.DenormClass) != SubstTypeFlags.NormNone && typeParameterSymbol.parent != null)
						{
							return false;
						}
						if (indexInTotalParameters < pctx.ctypeCls)
						{
							return typeDst == pctx.prgtypeCls[indexInTotalParameters];
						}
						if ((pctx.grfst & SubstTypeFlags.NormClass) != SubstTypeFlags.NormNone)
						{
							return typeDst == this.GetStdClsTypeVar(indexInTotalParameters);
						}
					}
					return false;
				}
				}
				typeSrc = typeSrc.GetBaseOrParameterOrElementType();
				typeDst = typeDst.GetBaseOrParameterOrElementType();
			}
			return true;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001D4C4 File Offset: 0x0001B6C4
		public static bool TypeContainsType(CType type, CType typeFind)
		{
			while (type != typeFind && !type.Equals(typeFind))
			{
				switch (type.GetTypeKind())
				{
				case TypeKind.TK_AggregateType:
				{
					AggregateType aggregateType = (AggregateType)type;
					for (int i = 0; i < aggregateType.GetTypeArgsAll().Count; i++)
					{
						if (TypeManager.TypeContainsType(aggregateType.GetTypeArgsAll()[i], typeFind))
						{
							return true;
						}
					}
					return false;
				}
				case TypeKind.TK_VoidType:
				case TypeKind.TK_NullType:
					return false;
				default:
					return false;
				case TypeKind.TK_ErrorType:
				{
					ErrorType errorType = (ErrorType)type;
					if (errorType.HasParent)
					{
						for (int j = 0; j < errorType.typeArgs.Count; j++)
						{
							if (TypeManager.TypeContainsType(errorType.typeArgs[j], typeFind))
							{
								return true;
							}
						}
					}
					return false;
				}
				case TypeKind.TK_ArrayType:
				case TypeKind.TK_PointerType:
				case TypeKind.TK_ParameterModifierType:
				case TypeKind.TK_NullableType:
					type = type.GetBaseOrParameterOrElementType();
					break;
				case TypeKind.TK_TypeParameterType:
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001D5A0 File Offset: 0x0001B7A0
		public static bool TypeContainsTyVars(CType type, TypeArray typeVars)
		{
			for (;;)
			{
				switch (type.GetTypeKind())
				{
				case TypeKind.TK_AggregateType:
					goto IL_0047;
				case TypeKind.TK_VoidType:
				case TypeKind.TK_NullType:
				case TypeKind.TK_MethodGroupType:
					return false;
				case TypeKind.TK_ErrorType:
					goto IL_007C;
				default:
					return false;
				case TypeKind.TK_ArrayType:
				case TypeKind.TK_PointerType:
				case TypeKind.TK_ParameterModifierType:
				case TypeKind.TK_NullableType:
					type = type.GetBaseOrParameterOrElementType();
					break;
				case TypeKind.TK_TypeParameterType:
					goto IL_00BE;
				}
			}
			return false;
			IL_0047:
			AggregateType aggregateType = (AggregateType)type;
			for (int i = 0; i < aggregateType.GetTypeArgsAll().Count; i++)
			{
				if (TypeManager.TypeContainsTyVars(aggregateType.GetTypeArgsAll()[i], typeVars))
				{
					return true;
				}
			}
			return false;
			IL_007C:
			ErrorType errorType = (ErrorType)type;
			if (errorType.HasParent)
			{
				for (int j = 0; j < errorType.typeArgs.Count; j++)
				{
					if (TypeManager.TypeContainsTyVars(errorType.typeArgs[j], typeVars))
					{
						return true;
					}
				}
			}
			return false;
			IL_00BE:
			if (typeVars != null && typeVars.Count > 0)
			{
				int indexInTotalParameters = ((TypeParameterType)type).GetIndexInTotalParameters();
				return indexInTotalParameters < typeVars.Count && type == typeVars[indexInTotalParameters];
			}
			return true;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0001D6A0 File Offset: 0x0001B8A0
		public static bool ParametersContainTyVar(TypeArray @params, TypeParameterType typeFind)
		{
			for (int i = 0; i < @params.Count; i++)
			{
				if (TypeManager.TypeContainsType(@params[i], typeFind))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0001D6D0 File Offset: 0x0001B8D0
		public AggregateSymbol GetPredefAgg(PredefinedType pt)
		{
			return this._predefTypes.GetPredefinedAggregate(pt);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001D6DE File Offset: 0x0001B8DE
		public TypeArray ConcatenateTypeArrays(TypeArray pTypeArray1, TypeArray pTypeArray2)
		{
			return this._BSymmgr.ConcatParams(pTypeArray1, pTypeArray2);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001D6F0 File Offset: 0x0001B8F0
		public TypeArray GetStdMethTyVarArray(int cTyVars)
		{
			TypeParameterType[] array = new TypeParameterType[cTyVars];
			for (int i = 0; i < cTyVars; i++)
			{
				array[i] = this.GetStdMethTypeVar(i);
			}
			return this._BSymmgr.AllocParams(cTyVars, array);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001D727 File Offset: 0x0001B927
		public CType SubstType(CType typeSrc, SubstContext pctx)
		{
			if (pctx != null && !pctx.FNop())
			{
				return this.SubstTypeCore(typeSrc, pctx);
			}
			return typeSrc;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001D73E File Offset: 0x0001B93E
		public CType SubstType(CType typeSrc, AggregateType atsCls)
		{
			return this.SubstType(typeSrc, atsCls, null);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001D749 File Offset: 0x0001B949
		public CType SubstType(CType typeSrc, AggregateType atsCls, TypeArray typeArgsMeth)
		{
			return this.SubstType(typeSrc, (atsCls != null) ? atsCls.GetTypeArgsAll() : null, typeArgsMeth);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0001D75F File Offset: 0x0001B95F
		public CType SubstType(CType typeSrc, CType typeCls, TypeArray typeArgsMeth)
		{
			AggregateType aggregateType = typeCls as AggregateType;
			return this.SubstType(typeSrc, (aggregateType != null) ? aggregateType.GetTypeArgsAll() : null, typeArgsMeth);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001D77B File Offset: 0x0001B97B
		public TypeArray SubstTypeArray(TypeArray taSrc, AggregateType atsCls, TypeArray typeArgsMeth)
		{
			return this.SubstTypeArray(taSrc, (atsCls != null) ? atsCls.GetTypeArgsAll() : null, typeArgsMeth);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0001D791 File Offset: 0x0001B991
		public TypeArray SubstTypeArray(TypeArray taSrc, AggregateType atsCls)
		{
			return this.SubstTypeArray(taSrc, atsCls, null);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0001D79C File Offset: 0x0001B99C
		private bool SubstEqualTypes(CType typeDst, CType typeSrc, CType typeCls, TypeArray typeArgsMeth)
		{
			AggregateType aggregateType = typeCls as AggregateType;
			return this.SubstEqualTypes(typeDst, typeSrc, (aggregateType != null) ? aggregateType.GetTypeArgsAll() : null, typeArgsMeth, SubstTypeFlags.NormNone);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0001D7BB File Offset: 0x0001B9BB
		public bool SubstEqualTypes(CType typeDst, CType typeSrc, CType typeCls)
		{
			return this.SubstEqualTypes(typeDst, typeSrc, typeCls, null);
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0001D7C7 File Offset: 0x0001B9C7
		public TypeParameterType GetStdMethTypeVar(int iv)
		{
			return this._stvcMethod.GetTypeVarSym(iv, this, true);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0001D7D7 File Offset: 0x0001B9D7
		private TypeParameterType GetStdClsTypeVar(int iv)
		{
			return this._stvcClass.GetTypeVarSym(iv, this, false);
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0001D7E8 File Offset: 0x0001B9E8
		public TypeParameterType GetTypeParameter(TypeParameterSymbol pSymbol)
		{
			TypeParameterType typeParameterType = this._typeTable.LookupTypeParameter(pSymbol);
			if (typeParameterType == null)
			{
				typeParameterType = this._typeFactory.CreateTypeParameter(pSymbol);
				this._typeTable.InsertTypeParameter(pSymbol, typeParameterType);
			}
			return typeParameterType;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001D820 File Offset: 0x0001BA20
		internal bool GetBestAccessibleType(CSemanticChecker semanticChecker, BindingContext bindingContext, CType typeSrc, out CType typeDst)
		{
			typeDst = null;
			if (semanticChecker.CheckTypeAccess(typeSrc, bindingContext.ContextForMemberLookup))
			{
				typeDst = typeSrc;
				return true;
			}
			if (typeSrc is ParameterModifierType || typeSrc is PointerType)
			{
				return false;
			}
			AggregateType aggregateType;
			CType ctype;
			if ((aggregateType = typeSrc as AggregateType) != null && (aggregateType.isInterfaceType() || aggregateType.isDelegateType()) && this.TryVarianceAdjustmentToGetAccessibleType(semanticChecker, bindingContext, aggregateType, out ctype))
			{
				typeDst = ctype;
				return true;
			}
			ArrayType arrayType;
			if ((arrayType = typeSrc as ArrayType) != null && this.TryArrayVarianceAdjustmentToGetAccessibleType(semanticChecker, bindingContext, arrayType, out ctype))
			{
				typeDst = ctype;
				return true;
			}
			if (typeSrc is NullableType)
			{
				typeDst = this.GetPredefAgg(PredefinedType.PT_VALUE).getThisType();
				return true;
			}
			if (typeSrc is ArrayType)
			{
				typeDst = this.GetPredefAgg(PredefinedType.PT_ARRAY).getThisType();
				return true;
			}
			AggregateType aggregateType2;
			if ((aggregateType2 = typeSrc as AggregateType) != null)
			{
				AggregateType aggregateType3 = aggregateType2.GetBaseClass();
				if (aggregateType3 == null)
				{
					aggregateType3 = this.GetPredefAgg(PredefinedType.PT_OBJECT).getThisType();
				}
				return this.GetBestAccessibleType(semanticChecker, bindingContext, aggregateType3, out typeDst);
			}
			return false;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0001D90C File Offset: 0x0001BB0C
		private bool TryVarianceAdjustmentToGetAccessibleType(CSemanticChecker semanticChecker, BindingContext bindingContext, AggregateType typeSrc, out CType typeDst)
		{
			typeDst = null;
			AggregateSymbol owningAggregate = typeSrc.GetOwningAggregate();
			AggregateType thisType = owningAggregate.getThisType();
			if (!semanticChecker.CheckTypeAccess(thisType, bindingContext.ContextForMemberLookup))
			{
				return false;
			}
			TypeArray typeArgsThis = typeSrc.GetTypeArgsThis();
			TypeArray typeArgsThis2 = thisType.GetTypeArgsThis();
			CType[] array = new CType[typeArgsThis.Count];
			for (int i = 0; i < typeArgsThis.Count; i++)
			{
				if (semanticChecker.CheckTypeAccess(typeArgsThis[i], bindingContext.ContextForMemberLookup))
				{
					array[i] = typeArgsThis[i];
				}
				else
				{
					if (!typeArgsThis[i].IsRefType() || !((TypeParameterType)typeArgsThis2[i]).Covariant)
					{
						return false;
					}
					CType ctype;
					if (!this.GetBestAccessibleType(semanticChecker, bindingContext, typeArgsThis[i], out ctype))
					{
						return false;
					}
					array[i] = ctype;
				}
			}
			TypeArray typeArray = semanticChecker.getBSymmgr().AllocParams(typeArgsThis.Count, array);
			CType aggregate = this.GetAggregate(owningAggregate, typeSrc.outerType, typeArray);
			if (!TypeBind.CheckConstraints(semanticChecker, null, aggregate, CheckConstraintsFlags.NoErrors))
			{
				return false;
			}
			typeDst = aggregate;
			return true;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0001DA10 File Offset: 0x0001BC10
		private bool TryArrayVarianceAdjustmentToGetAccessibleType(CSemanticChecker semanticChecker, BindingContext bindingContext, ArrayType typeSrc, out CType typeDst)
		{
			typeDst = null;
			CType elementType = typeSrc.GetElementType();
			if (!elementType.IsRefType())
			{
				return false;
			}
			CType ctype;
			if (this.GetBestAccessibleType(semanticChecker, bindingContext, elementType, out ctype))
			{
				typeDst = this.GetArray(ctype, typeSrc.rank, typeSrc.IsSZArray);
				return true;
			}
			return false;
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x0001DA58 File Offset: 0x0001BC58
		public AggregateType ObjectAggregateType
		{
			get
			{
				return (AggregateType)this._symbolTable.GetCTypeFromType(typeof(object));
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001DA74 File Offset: 0x0001BC74
		internal bool InternalsVisibleTo(Assembly assemblyThatDefinesAttribute, Assembly assemblyToCheck)
		{
			Tuple<Assembly, Assembly> tuple = Tuple.Create<Assembly, Assembly>(assemblyThatDefinesAttribute, assemblyToCheck);
			bool flag;
			if (!this._internalsVisibleToCalculated.TryGetValue(tuple, out flag))
			{
				AssemblyName assyName = null;
				try
				{
					assyName = assemblyToCheck.GetName();
				}
				catch (SecurityException)
				{
					flag = false;
					goto IL_0079;
				}
				flag = (from ivta in assemblyThatDefinesAttribute.GetCustomAttributes().OfType<InternalsVisibleToAttribute>()
					select new AssemblyName(ivta.AssemblyName)).Any((AssemblyName an) => AssemblyName.ReferenceMatchesDefinition(an, assyName));
				IL_0079:
				this._internalsVisibleToCalculated[tuple] = flag;
			}
			return flag;
		}

		// Token: 0x0400059C RID: 1436
		private BSYMMGR _BSymmgr;

		// Token: 0x0400059D RID: 1437
		private PredefinedTypes _predefTypes;

		// Token: 0x0400059E RID: 1438
		private readonly TypeFactory _typeFactory;

		// Token: 0x0400059F RID: 1439
		private readonly TypeTable _typeTable;

		// Token: 0x040005A0 RID: 1440
		private SymbolTable _symbolTable;

		// Token: 0x040005A1 RID: 1441
		private readonly VoidType _voidType;

		// Token: 0x040005A2 RID: 1442
		private readonly NullType _nullType;

		// Token: 0x040005A3 RID: 1443
		private readonly MethodGroupType _typeMethGrp;

		// Token: 0x040005A4 RID: 1444
		private readonly ArgumentListType _argListType;

		// Token: 0x040005A5 RID: 1445
		private readonly ErrorType _errorType;

		// Token: 0x040005A6 RID: 1446
		private readonly TypeManager.StdTypeVarColl _stvcMethod;

		// Token: 0x040005A7 RID: 1447
		private readonly TypeManager.StdTypeVarColl _stvcClass;

		// Token: 0x040005A8 RID: 1448
		private readonly Dictionary<Tuple<Assembly, Assembly>, bool> _internalsVisibleToCalculated = new Dictionary<Tuple<Assembly, Assembly>, bool>();

		// Token: 0x020000F2 RID: 242
		private sealed class StdTypeVarColl
		{
			// Token: 0x06000759 RID: 1881 RVA: 0x00023BCA File Offset: 0x00021DCA
			public StdTypeVarColl()
			{
				this.prgptvs = new List<TypeParameterType>();
			}

			// Token: 0x0600075A RID: 1882 RVA: 0x00023BE0 File Offset: 0x00021DE0
			public TypeParameterType GetTypeVarSym(int iv, TypeManager pTypeManager, bool fMeth)
			{
				TypeParameterType typeParameterType;
				if (iv >= this.prgptvs.Count)
				{
					TypeParameterSymbol typeParameterSymbol = new TypeParameterSymbol();
					typeParameterSymbol.SetIsMethodTypeParameter(fMeth);
					typeParameterSymbol.SetIndexInOwnParameters(iv);
					typeParameterSymbol.SetIndexInTotalParameters(iv);
					typeParameterSymbol.SetAccess(ACCESS.ACC_PRIVATE);
					typeParameterType = pTypeManager.GetTypeParameter(typeParameterSymbol);
					this.prgptvs.Add(typeParameterType);
				}
				else
				{
					typeParameterType = this.prgptvs[iv];
				}
				return typeParameterType;
			}

			// Token: 0x0400071E RID: 1822
			private readonly List<TypeParameterType> prgptvs;
		}
	}
}
