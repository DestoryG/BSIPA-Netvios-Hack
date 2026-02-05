using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000AD RID: 173
	internal abstract class CType
	{
		// Token: 0x060005AA RID: 1450 RVA: 0x0001BF21 File Offset: 0x0001A121
		public bool IsWindowsRuntimeType()
		{
			return (this.AssociatedSystemType.Attributes & TypeAttributes.WindowsRuntime) == TypeAttributes.WindowsRuntime;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001BF3C File Offset: 0x0001A13C
		public bool IsCollectionType()
		{
			return (this.AssociatedSystemType.IsGenericType && (this.AssociatedSystemType.GetGenericTypeDefinition() == typeof(IList<>) || this.AssociatedSystemType.GetGenericTypeDefinition() == typeof(ICollection<>) || this.AssociatedSystemType.GetGenericTypeDefinition() == typeof(IEnumerable<>) || this.AssociatedSystemType.GetGenericTypeDefinition() == typeof(IReadOnlyList<>) || this.AssociatedSystemType.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>) || this.AssociatedSystemType.GetGenericTypeDefinition() == typeof(IDictionary<, >) || this.AssociatedSystemType.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<, >))) || this.AssociatedSystemType == typeof(IList) || this.AssociatedSystemType == typeof(ICollection) || this.AssociatedSystemType == typeof(IEnumerable) || this.AssociatedSystemType == typeof(INotifyCollectionChanged) || this.AssociatedSystemType == typeof(INotifyPropertyChanged);
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x0001C0A5 File Offset: 0x0001A2A5
		public Type AssociatedSystemType
		{
			get
			{
				if (this._associatedSystemType == null)
				{
					this._associatedSystemType = CType.CalculateAssociatedSystemType(this);
				}
				return this._associatedSystemType;
			}
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001C0C8 File Offset: 0x0001A2C8
		private static Type CalculateAssociatedSystemType(CType src)
		{
			Type type = null;
			switch (src.GetTypeKind())
			{
			case TypeKind.TK_AggregateType:
				type = CType.CalculateAssociatedSystemTypeForAggregate((AggregateType)src);
				break;
			case TypeKind.TK_ArrayType:
			{
				ArrayType arrayType = (ArrayType)src;
				Type associatedSystemType = arrayType.GetElementType().AssociatedSystemType;
				type = (arrayType.IsSZArray ? associatedSystemType.MakeArrayType() : associatedSystemType.MakeArrayType(arrayType.rank));
				break;
			}
			case TypeKind.TK_PointerType:
				type = ((PointerType)src).GetReferentType().AssociatedSystemType.MakePointerType();
				break;
			case TypeKind.TK_ParameterModifierType:
				type = ((ParameterModifierType)src).GetParameterType().AssociatedSystemType.MakeByRefType();
				break;
			case TypeKind.TK_NullableType:
			{
				Type associatedSystemType2 = ((NullableType)src).GetUnderlyingType().AssociatedSystemType;
				type = typeof(Nullable<>).MakeGenericType(new Type[] { associatedSystemType2 });
				break;
			}
			case TypeKind.TK_TypeParameterType:
			{
				TypeParameterType typeParameterType = (TypeParameterType)src;
				if (typeParameterType.IsMethodTypeParameter())
				{
					type = (((MethodSymbol)typeParameterType.GetOwningSymbol()).AssociatedMemberInfo as MethodInfo).GetGenericArguments()[typeParameterType.GetIndexInOwnParameters()];
				}
				else
				{
					type = ((AggregateSymbol)typeParameterType.GetOwningSymbol()).AssociatedSystemType.GetGenericArguments()[typeParameterType.GetIndexInOwnParameters()];
				}
				break;
			}
			}
			return type;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001C214 File Offset: 0x0001A414
		private static Type CalculateAssociatedSystemTypeForAggregate(AggregateType aggtype)
		{
			AggregateSymbol owningAggregate = aggtype.GetOwningAggregate();
			TypeArray typeArgsAll = aggtype.GetTypeArgsAll();
			List<Type> list = new List<Type>();
			for (int i = 0; i < typeArgsAll.Count; i++)
			{
				TypeParameterType typeParameterType;
				if ((typeParameterType = typeArgsAll[i] as TypeParameterType) != null && typeParameterType.GetTypeParameterSymbol().name == null)
				{
					return null;
				}
				list.Add(typeArgsAll[i].AssociatedSystemType);
			}
			Type[] array = list.ToArray();
			Type associatedSystemType = owningAggregate.AssociatedSystemType;
			if (associatedSystemType.IsGenericType)
			{
				try
				{
					return associatedSystemType.MakeGenericType(array);
				}
				catch (ArgumentException)
				{
					return associatedSystemType;
				}
				return associatedSystemType;
			}
			return associatedSystemType;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001C2C0 File Offset: 0x0001A4C0
		public TypeKind GetTypeKind()
		{
			return this._typeKind;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001C2C8 File Offset: 0x0001A4C8
		public void SetTypeKind(TypeKind kind)
		{
			this._typeKind = kind;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001C2D1 File Offset: 0x0001A4D1
		public Name GetName()
		{
			return this._pName;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001C2D9 File Offset: 0x0001A4D9
		public void SetName(Name pName)
		{
			this._pName = pName;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001C2E4 File Offset: 0x0001A4E4
		public CType GetBaseOrParameterOrElementType()
		{
			switch (this.GetTypeKind())
			{
			case TypeKind.TK_ArrayType:
				return ((ArrayType)this).GetElementType();
			case TypeKind.TK_PointerType:
				return ((PointerType)this).GetReferentType();
			case TypeKind.TK_ParameterModifierType:
				return ((ParameterModifierType)this).GetParameterType();
			case TypeKind.TK_NullableType:
				return ((NullableType)this).GetUnderlyingType();
			default:
				return null;
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001C343 File Offset: 0x0001A543
		public void InitFromParent()
		{
			this._fHasErrors = this.GetBaseOrParameterOrElementType().HasErrors();
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001C356 File Offset: 0x0001A556
		public bool HasErrors()
		{
			return this._fHasErrors;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001C35E File Offset: 0x0001A55E
		public void SetErrors(bool fHasErrors)
		{
			this._fHasErrors = fHasErrors;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001C368 File Offset: 0x0001A568
		public FUNDTYPE fundType()
		{
			switch (this.GetTypeKind())
			{
			case TypeKind.TK_AggregateType:
			{
				AggregateSymbol aggregateSymbol = ((AggregateType)this).getAggregate();
				if (aggregateSymbol.IsEnum())
				{
					aggregateSymbol = aggregateSymbol.GetUnderlyingType().getAggregate();
				}
				if (!aggregateSymbol.IsStruct())
				{
					return FUNDTYPE.FT_REF;
				}
				if (aggregateSymbol.IsPredefined())
				{
					return PredefinedTypeFacts.GetFundType(aggregateSymbol.GetPredefType());
				}
				return FUNDTYPE.FT_STRUCT;
			}
			case TypeKind.TK_NullType:
			case TypeKind.TK_ArrayType:
				return FUNDTYPE.FT_REF;
			case TypeKind.TK_PointerType:
				return FUNDTYPE.FT_PTR;
			case TypeKind.TK_NullableType:
				return FUNDTYPE.FT_STRUCT;
			case TypeKind.TK_TypeParameterType:
				return FUNDTYPE.FT_VAR;
			}
			return FUNDTYPE.FT_NONE;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001C400 File Offset: 0x0001A600
		public ConstValKind constValKind()
		{
			if (this.isPointerLike())
			{
				return ConstValKind.IntPtr;
			}
			switch (this.fundType())
			{
			case FUNDTYPE.FT_I1:
				return ConstValKind.Boolean;
			case FUNDTYPE.FT_I8:
			case FUNDTYPE.FT_U8:
				return ConstValKind.Long;
			case FUNDTYPE.FT_R4:
				return ConstValKind.Float;
			case FUNDTYPE.FT_R8:
				return ConstValKind.Double;
			case FUNDTYPE.FT_REF:
				if (this.isPredefined() && this.getPredefType() == PredefinedType.PT_STRING)
				{
					return ConstValKind.String;
				}
				return ConstValKind.IntPtr;
			case FUNDTYPE.FT_STRUCT:
				if (this.isPredefined() && this.getPredefType() == PredefinedType.PT_DATETIME)
				{
					return ConstValKind.Long;
				}
				return ConstValKind.Decimal;
			}
			return ConstValKind.Int;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001C48D File Offset: 0x0001A68D
		public CType underlyingType()
		{
			if (this is AggregateType && this.getAggregate().IsEnum())
			{
				return this.getAggregate().GetUnderlyingType();
			}
			return this;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001C4B4 File Offset: 0x0001A6B4
		public CType GetNakedType(bool fStripNub)
		{
			CType ctype = this;
			for (;;)
			{
				TypeKind typeKind = ctype.GetTypeKind();
				if (typeKind - TypeKind.TK_ArrayType > 2)
				{
					if (typeKind != TypeKind.TK_NullableType)
					{
						break;
					}
					if (!fStripNub)
					{
						return ctype;
					}
					ctype = ctype.GetBaseOrParameterOrElementType();
				}
				else
				{
					ctype = ctype.GetBaseOrParameterOrElementType();
				}
			}
			return ctype;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001C4ED File Offset: 0x0001A6ED
		public AggregateSymbol GetNakedAgg()
		{
			return this.GetNakedAgg(false);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001C4F6 File Offset: 0x0001A6F6
		private AggregateSymbol GetNakedAgg(bool fStripNub)
		{
			AggregateType aggregateType = this.GetNakedType(fStripNub) as AggregateType;
			if (aggregateType == null)
			{
				return null;
			}
			return aggregateType.getAggregate();
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001C50F File Offset: 0x0001A70F
		public AggregateSymbol getAggregate()
		{
			return ((AggregateType)this).GetOwningAggregate();
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001C51C File Offset: 0x0001A71C
		public virtual CType StripNubs()
		{
			return this;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001C51F File Offset: 0x0001A71F
		public virtual CType StripNubs(out bool wasNullable)
		{
			wasNullable = false;
			return this;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001C525 File Offset: 0x0001A725
		public bool isDelegateType()
		{
			return this is AggregateType && this.getAggregate().IsDelegate();
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001C53C File Offset: 0x0001A73C
		public bool isSimpleType()
		{
			return this.isPredefined() && PredefinedTypeFacts.IsSimpleType(this.getPredefType());
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001C553 File Offset: 0x0001A753
		public bool isSimpleOrEnum()
		{
			return this.isSimpleType() || this.isEnumType();
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001C565 File Offset: 0x0001A765
		public bool isSimpleOrEnumOrString()
		{
			return this.isSimpleType() || this.isPredefType(PredefinedType.PT_STRING) || this.isEnumType();
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001C581 File Offset: 0x0001A781
		private bool isPointerLike()
		{
			return this is PointerType || this.isPredefType(PredefinedType.FirstNonSimpleType) || this.isPredefType(PredefinedType.PT_UINTPTR);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001C59F File Offset: 0x0001A79F
		public bool isNumericType()
		{
			return this.isPredefined() && PredefinedTypeFacts.IsNumericType(this.getPredefType());
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001C5B6 File Offset: 0x0001A7B6
		public bool isStructOrEnum()
		{
			return (this is AggregateType && (this.getAggregate().IsStruct() || this.getAggregate().IsEnum())) || this is NullableType;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001C5E5 File Offset: 0x0001A7E5
		public bool isStructType()
		{
			return (this is AggregateType && this.getAggregate().IsStruct()) || this is NullableType;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001C607 File Offset: 0x0001A807
		public bool isEnumType()
		{
			return this is AggregateType && this.getAggregate().IsEnum();
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001C61E File Offset: 0x0001A81E
		public bool isInterfaceType()
		{
			return this is AggregateType && this.getAggregate().IsInterface();
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001C635 File Offset: 0x0001A835
		public bool isClassType()
		{
			return this is AggregateType && this.getAggregate().IsClass();
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001C64C File Offset: 0x0001A84C
		public AggregateType underlyingEnumType()
		{
			return this.getAggregate().GetUnderlyingType();
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001C65C File Offset: 0x0001A85C
		public bool isUnsigned()
		{
			AggregateType aggregateType;
			if ((aggregateType = this as AggregateType) == null)
			{
				return this is PointerType;
			}
			if (aggregateType.isEnumType())
			{
				aggregateType = aggregateType.underlyingEnumType();
			}
			if (aggregateType.isPredefined())
			{
				PredefinedType predefType = aggregateType.getPredefType();
				return predefType == PredefinedType.PT_UINTPTR || predefType == PredefinedType.PT_BYTE || (predefType >= PredefinedType.PT_USHORT && predefType <= PredefinedType.PT_ULONG);
			}
			return false;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001C6B8 File Offset: 0x0001A8B8
		public bool isUnsafe()
		{
			ArrayType arrayType;
			return this is PointerType || ((arrayType = this as ArrayType) != null && arrayType.GetElementType().isUnsafe());
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001C6E8 File Offset: 0x0001A8E8
		public bool isPredefType(PredefinedType pt)
		{
			AggregateType aggregateType;
			if ((aggregateType = this as AggregateType) != null)
			{
				return aggregateType.getAggregate().IsPredefined() && aggregateType.getAggregate().GetPredefType() == pt;
			}
			return this is VoidType && pt == PredefinedType.PT_VOID;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001C72C File Offset: 0x0001A92C
		public bool isPredefined()
		{
			return this is AggregateType && this.getAggregate().IsPredefined();
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001C743 File Offset: 0x0001A943
		public PredefinedType getPredefType()
		{
			return this.getAggregate().GetPredefType();
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001C750 File Offset: 0x0001A950
		public bool isStaticClass()
		{
			AggregateSymbol nakedAgg = this.GetNakedAgg(false);
			return nakedAgg != null && nakedAgg.IsStatic();
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001C775 File Offset: 0x0001A975
		public CType GetDelegateTypeOfPossibleExpression()
		{
			if (this.isPredefType(PredefinedType.PT_G_EXPRESSION))
			{
				return ((AggregateType)this).GetTypeArgsThis()[0];
			}
			return this;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001C794 File Offset: 0x0001A994
		public bool IsValType()
		{
			TypeKind typeKind = this.GetTypeKind();
			if (typeKind != TypeKind.TK_AggregateType)
			{
				return typeKind == TypeKind.TK_NullableType || (typeKind == TypeKind.TK_TypeParameterType && ((TypeParameterType)this).IsValueType());
			}
			return ((AggregateType)this).getAggregate().IsValueType();
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001C7D8 File Offset: 0x0001A9D8
		public bool IsNonNubValType()
		{
			TypeKind typeKind = this.GetTypeKind();
			if (typeKind != TypeKind.TK_AggregateType)
			{
				return typeKind != TypeKind.TK_NullableType && typeKind == TypeKind.TK_TypeParameterType && ((TypeParameterType)this).IsNonNullableValueType();
			}
			return ((AggregateType)this).getAggregate().IsValueType();
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001C81C File Offset: 0x0001AA1C
		public bool IsRefType()
		{
			TypeKind typeKind = this.GetTypeKind();
			if (typeKind <= TypeKind.TK_NullType)
			{
				if (typeKind == TypeKind.TK_AggregateType)
				{
					return ((AggregateType)this).getAggregate().IsRefType();
				}
				if (typeKind != TypeKind.TK_NullType)
				{
					return false;
				}
			}
			else if (typeKind != TypeKind.TK_ArrayType)
			{
				if (typeKind != TypeKind.TK_TypeParameterType)
				{
					return false;
				}
				return ((TypeParameterType)this).IsReferenceType();
			}
			return true;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001C868 File Offset: 0x0001AA68
		public bool IsNeverSameType()
		{
			ErrorType errorType;
			return this is MethodGroupType || ((errorType = this as ErrorType) != null && !errorType.HasParent);
		}

		// Token: 0x0400058B RID: 1419
		private TypeKind _typeKind;

		// Token: 0x0400058C RID: 1420
		private Name _pName;

		// Token: 0x0400058D RID: 1421
		private bool _fHasErrors;

		// Token: 0x0400058E RID: 1422
		private Type _associatedSystemType;
	}
}
