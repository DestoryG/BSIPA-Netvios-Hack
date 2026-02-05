using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000039 RID: 57
	internal static class CConversions
	{
		// Token: 0x06000258 RID: 600 RVA: 0x000119DA File Offset: 0x0000FBDA
		public static bool FImpRefConv(SymbolLoader loader, CType typeSrc, CType typeDst)
		{
			return typeSrc.IsRefType() && loader.HasIdentityOrImplicitReferenceConversion(typeSrc, typeDst);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x000119F0 File Offset: 0x0000FBF0
		public static bool FExpRefConv(SymbolLoader loader, CType typeSrc, CType typeDst)
		{
			if (typeSrc.IsRefType() && typeDst.IsRefType())
			{
				if (loader.HasIdentityOrImplicitReferenceConversion(typeSrc, typeDst) || loader.HasIdentityOrImplicitReferenceConversion(typeDst, typeSrc))
				{
					return true;
				}
				if (typeSrc.isInterfaceType() && typeDst is TypeParameterType)
				{
					return true;
				}
				if (typeSrc is TypeParameterType && typeDst.isInterfaceType())
				{
					return true;
				}
				AggregateType aggregateType;
				AggregateType aggregateType2;
				if ((aggregateType = typeSrc as AggregateType) != null && (aggregateType2 = typeDst as AggregateType) != null)
				{
					AggregateSymbol aggregate = aggregateType.getAggregate();
					AggregateSymbol aggregate2 = aggregateType2.getAggregate();
					if ((aggregate.IsClass() && !aggregate.IsSealed() && aggregate2.IsInterface()) || (aggregate.IsInterface() && aggregate2.IsClass() && !aggregate2.IsSealed()) || (aggregate.IsInterface() && aggregate2.IsInterface()))
					{
						return true;
					}
				}
				ArrayType arrayType;
				ArrayType arrayType3;
				AggregateType aggregateType4;
				if ((arrayType = typeSrc as ArrayType) != null)
				{
					ArrayType arrayType2;
					if ((arrayType2 = typeDst as ArrayType) != null)
					{
						return arrayType.rank == arrayType2.rank && arrayType.IsSZArray == arrayType2.IsSZArray && CConversions.FExpRefConv(loader, arrayType.GetElementType(), arrayType2.GetElementType());
					}
					if (!arrayType.IsSZArray || !typeDst.isInterfaceType())
					{
						return false;
					}
					AggregateType aggregateType3 = (AggregateType)typeDst;
					TypeArray typeArgsAll = aggregateType3.GetTypeArgsAll();
					if (typeArgsAll.Count != 1)
					{
						return false;
					}
					AggregateSymbol predefAgg = loader.GetPredefAgg(PredefinedType.PT_G_ILIST);
					AggregateSymbol predefAgg2 = loader.GetPredefAgg(PredefinedType.PT_G_IREADONLYLIST);
					return ((predefAgg != null && loader.IsBaseAggregate(predefAgg, aggregateType3.getAggregate())) || (predefAgg2 != null && loader.IsBaseAggregate(predefAgg2, aggregateType3.getAggregate()))) && CConversions.FExpRefConv(loader, arrayType.GetElementType(), typeArgsAll[0]);
				}
				else if ((arrayType3 = typeDst as ArrayType) != null && (aggregateType4 = typeSrc as AggregateType) != null)
				{
					if (loader.HasIdentityOrImplicitReferenceConversion(loader.GetPredefindType(PredefinedType.PT_ARRAY), typeSrc))
					{
						return true;
					}
					if (!arrayType3.IsSZArray || !typeSrc.isInterfaceType() || aggregateType4.GetTypeArgsAll().Count != 1)
					{
						return false;
					}
					AggregateSymbol predefAgg3 = loader.GetPredefAgg(PredefinedType.PT_G_ILIST);
					AggregateSymbol predefAgg4 = loader.GetPredefAgg(PredefinedType.PT_G_IREADONLYLIST);
					if ((predefAgg3 == null || !loader.IsBaseAggregate(predefAgg3, aggregateType4.getAggregate())) && (predefAgg4 == null || !loader.IsBaseAggregate(predefAgg4, aggregateType4.getAggregate())))
					{
						return false;
					}
					CType elementType = arrayType3.GetElementType();
					CType ctype = aggregateType4.GetTypeArgsAll()[0];
					return elementType == ctype || CConversions.FExpRefConv(loader, elementType, ctype);
				}
				else if (CConversions.HasGenericDelegateExplicitReferenceConversion(loader, typeSrc, typeDst))
				{
					return true;
				}
			}
			else
			{
				if (typeSrc.IsRefType())
				{
					return loader.HasIdentityOrImplicitReferenceConversion(typeSrc, typeDst);
				}
				if (typeDst.IsRefType())
				{
					return loader.HasIdentityOrImplicitReferenceConversion(typeDst, typeSrc);
				}
			}
			return false;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00011C74 File Offset: 0x0000FE74
		public static bool HasGenericDelegateExplicitReferenceConversion(SymbolLoader loader, CType pSource, CType pTarget)
		{
			if (!pSource.isDelegateType() || !pTarget.isDelegateType() || pSource.getAggregate() != pTarget.getAggregate() || loader.HasIdentityOrImplicitReferenceConversion(pSource, pTarget))
			{
				return false;
			}
			TypeArray typeVarsAll = pSource.getAggregate().GetTypeVarsAll();
			TypeArray typeArgsAll = ((AggregateType)pSource).GetTypeArgsAll();
			TypeArray typeArgsAll2 = ((AggregateType)pTarget).GetTypeArgsAll();
			for (int i = 0; i < typeVarsAll.Count; i++)
			{
				CType ctype = typeArgsAll[i];
				CType ctype2 = typeArgsAll2[i];
				if (ctype != ctype2 && !(ctype2 is ErrorType) && !(ctype is ErrorType))
				{
					TypeParameterType typeParameterType = (TypeParameterType)typeVarsAll[i];
					if (typeParameterType.Invariant)
					{
						return false;
					}
					if (typeParameterType.Covariant)
					{
						if (!CConversions.FExpRefConv(loader, ctype, ctype2))
						{
							return false;
						}
					}
					else if (typeParameterType.Contravariant && (!ctype.IsRefType() || !ctype2.IsRefType()))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00011D5C File Offset: 0x0000FF5C
		public static bool FWrappingConv(CType typeSrc, CType typeDst)
		{
			NullableType nullableType;
			return (nullableType = typeDst as NullableType) != null && typeSrc == nullableType.GetUnderlyingType();
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00011D7E File Offset: 0x0000FF7E
		public static bool FUnwrappingConv(CType typeSrc, CType typeDst)
		{
			return CConversions.FWrappingConv(typeDst, typeSrc);
		}
	}
}
