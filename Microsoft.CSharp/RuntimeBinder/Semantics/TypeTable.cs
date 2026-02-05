using System;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000B4 RID: 180
	internal sealed class TypeTable
	{
		// Token: 0x06000630 RID: 1584 RVA: 0x0001DCA4 File Offset: 0x0001BEA4
		public TypeTable()
		{
			this._pAggregateTable = new Dictionary<KeyPair<AggregateSymbol, Name>, AggregateType>();
			this._pErrorWithNamespaceParentTable = new Dictionary<Name, ErrorType>();
			this._pArrayTable = new Dictionary<KeyPair<CType, Name>, ArrayType>();
			this._pParameterModifierTable = new Dictionary<KeyPair<CType, Name>, ParameterModifierType>();
			this._pPointerTable = new Dictionary<CType, PointerType>();
			this._pNullableTable = new Dictionary<CType, NullableType>();
			this._pTypeParameterTable = new Dictionary<TypeParameterSymbol, TypeParameterType>();
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0001DD04 File Offset: 0x0001BF04
		public AggregateType LookupAggregate(Name pName, AggregateSymbol pAggregate)
		{
			KeyPair<AggregateSymbol, Name> keyPair = new KeyPair<AggregateSymbol, Name>(pAggregate, pName);
			AggregateType aggregateType;
			if (this._pAggregateTable.TryGetValue(keyPair, out aggregateType))
			{
				return aggregateType;
			}
			return null;
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0001DD2D File Offset: 0x0001BF2D
		public void InsertAggregate(Name pName, AggregateSymbol pAggregateSymbol, AggregateType pAggregate)
		{
			this._pAggregateTable.Add(new KeyPair<AggregateSymbol, Name>(pAggregateSymbol, pName), pAggregate);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0001DD44 File Offset: 0x0001BF44
		public ErrorType LookupError(Name pName)
		{
			ErrorType errorType;
			if (!this._pErrorWithNamespaceParentTable.TryGetValue(pName, out errorType))
			{
				return null;
			}
			return errorType;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0001DD64 File Offset: 0x0001BF64
		public void InsertError(Name pName, ErrorType pError)
		{
			this._pErrorWithNamespaceParentTable.Add(pName, pError);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001DD74 File Offset: 0x0001BF74
		public ArrayType LookupArray(Name pName, CType pElementType)
		{
			KeyPair<CType, Name> keyPair = new KeyPair<CType, Name>(pElementType, pName);
			ArrayType arrayType;
			if (this._pArrayTable.TryGetValue(keyPair, out arrayType))
			{
				return arrayType;
			}
			return null;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001DD9D File Offset: 0x0001BF9D
		public void InsertArray(Name pName, CType pElementType, ArrayType pArray)
		{
			this._pArrayTable.Add(new KeyPair<CType, Name>(pElementType, pName), pArray);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001DDB4 File Offset: 0x0001BFB4
		public ParameterModifierType LookupParameterModifier(Name pName, CType pElementType)
		{
			KeyPair<CType, Name> keyPair = new KeyPair<CType, Name>(pElementType, pName);
			ParameterModifierType parameterModifierType;
			if (this._pParameterModifierTable.TryGetValue(keyPair, out parameterModifierType))
			{
				return parameterModifierType;
			}
			return null;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0001DDDD File Offset: 0x0001BFDD
		public void InsertParameterModifier(Name pName, CType pElementType, ParameterModifierType pParameterModifier)
		{
			this._pParameterModifierTable.Add(new KeyPair<CType, Name>(pElementType, pName), pParameterModifier);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001DDF4 File Offset: 0x0001BFF4
		public PointerType LookupPointer(CType pElementType)
		{
			PointerType pointerType;
			if (this._pPointerTable.TryGetValue(pElementType, out pointerType))
			{
				return pointerType;
			}
			return null;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001DE14 File Offset: 0x0001C014
		public void InsertPointer(CType pElementType, PointerType pPointer)
		{
			this._pPointerTable.Add(pElementType, pPointer);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0001DE24 File Offset: 0x0001C024
		public NullableType LookupNullable(CType pUnderlyingType)
		{
			NullableType nullableType;
			if (this._pNullableTable.TryGetValue(pUnderlyingType, out nullableType))
			{
				return nullableType;
			}
			return null;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001DE44 File Offset: 0x0001C044
		public void InsertNullable(CType pUnderlyingType, NullableType pNullable)
		{
			this._pNullableTable.Add(pUnderlyingType, pNullable);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001DE54 File Offset: 0x0001C054
		public TypeParameterType LookupTypeParameter(TypeParameterSymbol pTypeParameterSymbol)
		{
			TypeParameterType typeParameterType;
			if (this._pTypeParameterTable.TryGetValue(pTypeParameterSymbol, out typeParameterType))
			{
				return typeParameterType;
			}
			return null;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0001DE74 File Offset: 0x0001C074
		public void InsertTypeParameter(TypeParameterSymbol pTypeParameterSymbol, TypeParameterType pTypeParameter)
		{
			this._pTypeParameterTable.Add(pTypeParameterSymbol, pTypeParameter);
		}

		// Token: 0x040005AC RID: 1452
		private readonly Dictionary<KeyPair<AggregateSymbol, Name>, AggregateType> _pAggregateTable;

		// Token: 0x040005AD RID: 1453
		private readonly Dictionary<KeyPair<CType, Name>, ArrayType> _pArrayTable;

		// Token: 0x040005AE RID: 1454
		private readonly Dictionary<KeyPair<CType, Name>, ParameterModifierType> _pParameterModifierTable;

		// Token: 0x040005AF RID: 1455
		private readonly Dictionary<Name, ErrorType> _pErrorWithNamespaceParentTable;

		// Token: 0x040005B0 RID: 1456
		private readonly Dictionary<CType, PointerType> _pPointerTable;

		// Token: 0x040005B1 RID: 1457
		private readonly Dictionary<CType, NullableType> _pNullableTable;

		// Token: 0x040005B2 RID: 1458
		private readonly Dictionary<TypeParameterSymbol, TypeParameterType> _pTypeParameterTable;
	}
}
