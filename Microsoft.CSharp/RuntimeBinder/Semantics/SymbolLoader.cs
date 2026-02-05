using System;
using Microsoft.CSharp.RuntimeBinder.Errors;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000073 RID: 115
	internal sealed class SymbolLoader
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x000170D3 File Offset: 0x000152D3
		public PredefinedMembers PredefinedMembers { get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x000170DB File Offset: 0x000152DB
		private GlobalSymbolContext GlobalSymbolContext { get; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x000170E3 File Offset: 0x000152E3
		public ErrorHandling ErrorContext { get; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x000170EB File Offset: 0x000152EB
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x000170F3 File Offset: 0x000152F3
		public SymbolTable RuntimeBinderSymbolTable { get; private set; }

		// Token: 0x060003D9 RID: 985 RVA: 0x000170FC File Offset: 0x000152FC
		public SymbolLoader()
		{
			GlobalSymbolContext globalSymbolContext = new GlobalSymbolContext(new NameManager());
			this._nameManager = globalSymbolContext.GetNameManager();
			this.PredefinedMembers = new PredefinedMembers(this);
			this.ErrorContext = new ErrorHandling(globalSymbolContext);
			this.GlobalSymbolContext = globalSymbolContext;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00017145 File Offset: 0x00015345
		public ErrorHandling GetErrorContext()
		{
			return this.ErrorContext;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001714D File Offset: 0x0001534D
		public GlobalSymbolContext GetGlobalSymbolContext()
		{
			return this.GlobalSymbolContext;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00017158 File Offset: 0x00015358
		public MethodSymbol LookupInvokeMeth(AggregateSymbol pAggDel)
		{
			for (Symbol symbol = this.LookupAggMember(NameManager.GetPredefinedName(PredefinedName.PN_INVOKE), pAggDel, symbmask_t.MASK_ALL); symbol != null; symbol = this.LookupNextSym(symbol, pAggDel, symbmask_t.MASK_ALL))
			{
				MethodSymbol methodSymbol;
				if ((methodSymbol = symbol as MethodSymbol) != null && methodSymbol.isInvoke())
				{
					return methodSymbol;
				}
			}
			return null;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001719B File Offset: 0x0001539B
		public NameManager GetNameManager()
		{
			return this._nameManager;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000171A3 File Offset: 0x000153A3
		public PredefinedTypes GetPredefindTypes()
		{
			return this.GlobalSymbolContext.GetPredefTypes();
		}

		// Token: 0x060003DF RID: 991 RVA: 0x000171B0 File Offset: 0x000153B0
		public TypeManager GetTypeManager()
		{
			return this.TypeManager;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x000171B8 File Offset: 0x000153B8
		public TypeManager TypeManager
		{
			get
			{
				return this.GlobalSymbolContext.TypeManager;
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x000171C5 File Offset: 0x000153C5
		public PredefinedMembers getPredefinedMembers()
		{
			return this.PredefinedMembers;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000171CD File Offset: 0x000153CD
		public BSYMMGR getBSymmgr()
		{
			return this.GlobalSymbolContext.GetGlobalSymbols();
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000171DA File Offset: 0x000153DA
		public SymFactory GetGlobalSymbolFactory()
		{
			return this.GlobalSymbolContext.GetGlobalSymbolFactory();
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000171E7 File Offset: 0x000153E7
		public AggregateSymbol GetPredefAgg(PredefinedType pt)
		{
			return this.GetTypeManager().GetPredefAgg(pt);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000171F5 File Offset: 0x000153F5
		public AggregateType GetPredefindType(PredefinedType pt)
		{
			return this.GetPredefAgg(pt).getThisType();
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00017203 File Offset: 0x00015403
		public Symbol LookupAggMember(Name name, AggregateSymbol agg, symbmask_t mask)
		{
			return this.getBSymmgr().LookupAggMember(name, agg, mask);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00017213 File Offset: 0x00015413
		public Symbol LookupNextSym(Symbol sym, ParentSymbol parent, symbmask_t kindmask)
		{
			return BSYMMGR.LookupNextSym(sym, parent, kindmask);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00017220 File Offset: 0x00015420
		public AggregateType GetAggTypeSym(CType typeSym)
		{
			TypeKind typeKind = typeSym.GetTypeKind();
			if (typeKind == TypeKind.TK_AggregateType)
			{
				return (AggregateType)typeSym;
			}
			if (typeKind == TypeKind.TK_ArrayType)
			{
				return this.GetPredefindType(PredefinedType.PT_ARRAY);
			}
			if (typeKind != TypeKind.TK_NullableType)
			{
				return null;
			}
			return ((NullableType)typeSym).GetAts();
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00017260 File Offset: 0x00015460
		private bool IsBaseInterface(AggregateType atsDer, AggregateType pBase)
		{
			if (pBase.isInterfaceType())
			{
				while (atsDer != null)
				{
					TypeArray ifacesAll = atsDer.GetIfacesAll();
					for (int i = 0; i < ifacesAll.Count; i++)
					{
						if (this.AreTypesEqualForConversion(ifacesAll[i], pBase))
						{
							return true;
						}
					}
					atsDer = atsDer.GetBaseClass();
				}
			}
			return false;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x000172AD File Offset: 0x000154AD
		public bool IsBaseClassOfClass(CType pDerived, CType pBase)
		{
			return pDerived.isClassType() && this.IsBaseClass(pDerived, pBase);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x000172C4 File Offset: 0x000154C4
		private bool IsBaseClass(CType pDerived, CType pBase)
		{
			AggregateType aggregateType;
			if ((aggregateType = pBase as AggregateType) == null || !aggregateType.isClassType())
			{
				return false;
			}
			NullableType nullableType;
			if ((nullableType = pDerived as NullableType) != null)
			{
				pDerived = nullableType.GetAts();
			}
			AggregateType aggregateType2;
			if ((aggregateType2 = pDerived as AggregateType) == null)
			{
				return false;
			}
			for (AggregateType aggregateType3 = aggregateType2.GetBaseClass(); aggregateType3 != null; aggregateType3 = aggregateType3.GetBaseClass())
			{
				if (aggregateType3 == aggregateType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001731D File Offset: 0x0001551D
		private bool HasCovariantArrayConversion(ArrayType pSource, ArrayType pDest)
		{
			return pSource.rank == pDest.rank && pSource.IsSZArray == pDest.IsSZArray && this.HasImplicitReferenceConversion(pSource.GetElementType(), pDest.GetElementType());
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001734F File Offset: 0x0001554F
		public bool HasIdentityOrImplicitReferenceConversion(CType pSource, CType pDest)
		{
			return this.AreTypesEqualForConversion(pSource, pDest) || this.HasImplicitReferenceConversion(pSource, pDest);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00017365 File Offset: 0x00015565
		private bool AreTypesEqualForConversion(CType pType1, CType pType2)
		{
			return pType1.Equals(pType2);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00017370 File Offset: 0x00015570
		private bool HasArrayConversionToInterface(ArrayType pSource, CType pDest)
		{
			if (!pSource.IsSZArray)
			{
				return false;
			}
			if (!pDest.isInterfaceType())
			{
				return false;
			}
			if (pDest.isPredefType(PredefinedType.PT_IENUMERABLE))
			{
				return true;
			}
			AggregateType aggregateType = (AggregateType)pDest;
			AggregateSymbol aggregate = aggregateType.getAggregate();
			if (!aggregate.isPredefAgg(PredefinedType.PT_G_ILIST) && !aggregate.isPredefAgg(PredefinedType.PT_G_ICOLLECTION) && !aggregate.isPredefAgg(PredefinedType.PT_G_IENUMERABLE) && !aggregate.isPredefAgg(PredefinedType.PT_G_IREADONLYCOLLECTION) && !aggregate.isPredefAgg(PredefinedType.PT_G_IREADONLYLIST))
			{
				return false;
			}
			CType elementType = pSource.GetElementType();
			CType ctype = aggregateType.GetTypeArgsAll()[0];
			return this.HasIdentityOrImplicitReferenceConversion(elementType, ctype);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x000173FC File Offset: 0x000155FC
		private bool HasImplicitReferenceConversion(CType pSource, CType pDest)
		{
			if (pSource.IsRefType() && pDest.isPredefType(PredefinedType.PT_OBJECT))
			{
				return true;
			}
			AggregateType aggregateType;
			ArrayType arrayType;
			if ((aggregateType = pSource as AggregateType) != null)
			{
				AggregateType aggregateType2;
				if ((aggregateType2 = pDest as AggregateType) != null)
				{
					switch (aggregateType.GetOwningAggregate().AggKind())
					{
					case AggKindEnum.Class:
					{
						AggKindEnum aggKindEnum = aggregateType2.GetOwningAggregate().AggKind();
						if (aggKindEnum == AggKindEnum.Class)
						{
							return this.IsBaseClass(aggregateType, aggregateType2);
						}
						if (aggKindEnum == AggKindEnum.Interface)
						{
							return this.HasAnyBaseInterfaceConversion(aggregateType, aggregateType2);
						}
						break;
					}
					case AggKindEnum.Delegate:
						return aggregateType2.isPredefType(PredefinedType.PT_MULTIDEL) || aggregateType2.isPredefType(PredefinedType.PT_DELEGATE) || this.IsBaseInterface(this.GetPredefindType(PredefinedType.PT_MULTIDEL), aggregateType2) || (pDest.isDelegateType() && this.HasDelegateConversion(aggregateType, aggregateType2));
					case AggKindEnum.Interface:
						if (aggregateType2.isInterfaceType())
						{
							return this.HasAnyBaseInterfaceConversion(aggregateType, aggregateType2) || this.HasInterfaceConversion(aggregateType, aggregateType2);
						}
						break;
					}
				}
			}
			else if ((arrayType = pSource as ArrayType) != null)
			{
				ArrayType arrayType2;
				if ((arrayType2 = pDest as ArrayType) != null)
				{
					return this.HasCovariantArrayConversion(arrayType, arrayType2);
				}
				AggregateType aggregateType3;
				if ((aggregateType3 = pDest as AggregateType) != null)
				{
					return aggregateType3.isPredefType(PredefinedType.PT_ARRAY) || this.IsBaseInterface(this.GetPredefindType(PredefinedType.PT_ARRAY), aggregateType3) || this.HasArrayConversionToInterface(arrayType, pDest);
				}
			}
			else if (pSource is NullType)
			{
				return pDest.IsRefType() || pDest is NullableType;
			}
			return false;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00017550 File Offset: 0x00015750
		private bool HasAnyBaseInterfaceConversion(CType pDerived, CType pBase)
		{
			if (!pBase.isInterfaceType())
			{
				return false;
			}
			AggregateType aggregateType;
			if ((aggregateType = pDerived as AggregateType) == null)
			{
				return false;
			}
			AggregateType aggregateType2 = (AggregateType)pBase;
			while (aggregateType != null)
			{
				foreach (AggregateType aggregateType3 in aggregateType.GetIfacesAll().Items)
				{
					if (this.HasInterfaceConversion(aggregateType3, aggregateType2))
					{
						return true;
					}
				}
				aggregateType = aggregateType.GetBaseClass();
			}
			return false;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x000175B8 File Offset: 0x000157B8
		private bool HasInterfaceConversion(AggregateType pSource, AggregateType pDest)
		{
			return this.HasVariantConversion(pSource, pDest);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x000175C2 File Offset: 0x000157C2
		private bool HasDelegateConversion(AggregateType pSource, AggregateType pDest)
		{
			return this.HasVariantConversion(pSource, pDest);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000175CC File Offset: 0x000157CC
		private bool HasVariantConversion(AggregateType pSource, AggregateType pDest)
		{
			if (pSource == pDest)
			{
				return true;
			}
			AggregateSymbol aggregate = pSource.getAggregate();
			if (aggregate != pDest.getAggregate())
			{
				return false;
			}
			TypeArray typeVarsAll = aggregate.GetTypeVarsAll();
			TypeArray typeArgsAll = pSource.GetTypeArgsAll();
			TypeArray typeArgsAll2 = pDest.GetTypeArgsAll();
			for (int i = 0; i < typeVarsAll.Count; i++)
			{
				CType ctype = typeArgsAll[i];
				CType ctype2 = typeArgsAll2[i];
				if (ctype != ctype2)
				{
					TypeParameterType typeParameterType = (TypeParameterType)typeVarsAll[i];
					if (typeParameterType.Invariant)
					{
						return false;
					}
					if (typeParameterType.Covariant && !this.HasImplicitReferenceConversion(ctype, ctype2))
					{
						return false;
					}
					if (typeParameterType.Contravariant && !this.HasImplicitReferenceConversion(ctype2, ctype))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00017680 File Offset: 0x00015880
		public bool HasImplicitBoxingConversion(CType pSource, CType pDest)
		{
			if (!pSource.IsValType() || !pDest.IsRefType())
			{
				return false;
			}
			NullableType nullableType;
			if ((nullableType = pSource as NullableType) != null)
			{
				return this.HasImplicitBoxingConversion(nullableType.GetUnderlyingType(), pDest);
			}
			return this.IsBaseClass(pSource, pDest) || this.HasAnyBaseInterfaceConversion(pSource, pDest);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000176D0 File Offset: 0x000158D0
		public bool HasBaseConversion(CType pSource, CType pDest)
		{
			return (pSource is AggregateType && pDest.isPredefType(PredefinedType.PT_OBJECT)) || this.HasIdentityOrImplicitReferenceConversion(pSource, pDest) || this.HasImplicitBoxingConversion(pSource, pDest);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x000176FC File Offset: 0x000158FC
		public bool IsBaseAggregate(AggregateSymbol derived, AggregateSymbol @base)
		{
			if (derived == @base)
			{
				return true;
			}
			if (@base.IsInterface())
			{
				while (derived != null)
				{
					CType[] items = derived.GetIfacesAll().Items;
					for (int i = 0; i < items.Length; i++)
					{
						if (((AggregateType)items[i]).getAggregate() == @base)
						{
							return true;
						}
					}
					derived = derived.GetBaseAgg();
				}
				return false;
			}
			while (derived.GetBaseClass() != null)
			{
				derived = derived.GetBaseClass().getAggregate();
				if (derived == @base)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001776E File Offset: 0x0001596E
		internal void SetSymbolTable(SymbolTable symbolTable)
		{
			this.RuntimeBinderSymbolTable = symbolTable;
		}

		// Token: 0x040004F8 RID: 1272
		private readonly NameManager _nameManager;
	}
}
