using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000AF RID: 175
	internal sealed class TypeFactory
	{
		// Token: 0x060005DE RID: 1502 RVA: 0x0001C8DA File Offset: 0x0001AADA
		public AggregateType CreateAggregateType(Name name, AggregateSymbol parent, TypeArray typeArgsThis, AggregateType outerType)
		{
			AggregateType aggregateType = new AggregateType();
			aggregateType.outerType = outerType;
			aggregateType.SetOwningAggregate(parent);
			aggregateType.SetTypeArgsThis(typeArgsThis);
			aggregateType.SetName(name);
			aggregateType.SetTypeKind(TypeKind.TK_AggregateType);
			return aggregateType;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001C908 File Offset: 0x0001AB08
		public TypeParameterType CreateTypeParameter(TypeParameterSymbol pSymbol)
		{
			TypeParameterType typeParameterType = new TypeParameterType();
			typeParameterType.SetTypeParameterSymbol(pSymbol);
			typeParameterType.SetName(pSymbol.name);
			pSymbol.SetTypeParameterType(typeParameterType);
			typeParameterType.SetTypeKind(TypeKind.TK_TypeParameterType);
			return typeParameterType;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001C93E File Offset: 0x0001AB3E
		public VoidType CreateVoid()
		{
			VoidType voidType = new VoidType();
			voidType.SetTypeKind(TypeKind.TK_VoidType);
			return voidType;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001C94C File Offset: 0x0001AB4C
		public NullType CreateNull()
		{
			NullType nullType = new NullType();
			nullType.SetTypeKind(TypeKind.TK_NullType);
			return nullType;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001C95A File Offset: 0x0001AB5A
		public MethodGroupType CreateMethodGroup()
		{
			MethodGroupType methodGroupType = new MethodGroupType();
			methodGroupType.SetTypeKind(TypeKind.TK_MethodGroupType);
			return methodGroupType;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001C968 File Offset: 0x0001AB68
		public ArgumentListType CreateArgList()
		{
			ArgumentListType argumentListType = new ArgumentListType();
			argumentListType.SetTypeKind(TypeKind.TK_ArgumentListType);
			return argumentListType;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001C976 File Offset: 0x0001AB76
		public ErrorType CreateError(Name name, Name nameText, TypeArray typeArgs)
		{
			ErrorType errorType = new ErrorType();
			errorType.SetName(name);
			errorType.nameText = nameText;
			errorType.typeArgs = typeArgs;
			errorType.SetTypeKind(TypeKind.TK_ErrorType);
			return errorType;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001C999 File Offset: 0x0001AB99
		public ArrayType CreateArray(Name name, CType pElementType, int rank, bool isSZArray)
		{
			ArrayType arrayType = new ArrayType();
			arrayType.SetName(name);
			arrayType.rank = rank;
			arrayType.IsSZArray = isSZArray;
			arrayType.SetElementType(pElementType);
			arrayType.SetTypeKind(TypeKind.TK_ArrayType);
			return arrayType;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001C9C4 File Offset: 0x0001ABC4
		public PointerType CreatePointer(Name name, CType pReferentType)
		{
			PointerType pointerType = new PointerType();
			pointerType.SetName(name);
			pointerType.SetReferentType(pReferentType);
			pointerType.SetTypeKind(TypeKind.TK_PointerType);
			return pointerType;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001C9E0 File Offset: 0x0001ABE0
		public ParameterModifierType CreateParameterModifier(Name name, CType pParameterType)
		{
			ParameterModifierType parameterModifierType = new ParameterModifierType();
			parameterModifierType.SetName(name);
			parameterModifierType.SetParameterType(pParameterType);
			parameterModifierType.SetTypeKind(TypeKind.TK_ParameterModifierType);
			return parameterModifierType;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001C9FC File Offset: 0x0001ABFC
		public NullableType CreateNullable(Name name, CType pUnderlyingType, BSYMMGR symmgr, TypeManager typeManager)
		{
			NullableType nullableType = new NullableType();
			nullableType.SetName(name);
			nullableType.SetUnderlyingType(pUnderlyingType);
			nullableType.symmgr = symmgr;
			nullableType.typeManager = typeManager;
			nullableType.SetTypeKind(TypeKind.TK_NullableType);
			return nullableType;
		}
	}
}
