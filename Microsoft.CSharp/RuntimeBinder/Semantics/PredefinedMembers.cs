using System;
using Microsoft.CSharp.RuntimeBinder.Errors;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200005B RID: 91
	internal sealed class PredefinedMembers
	{
		// Token: 0x06000308 RID: 776 RVA: 0x00014E2B File Offset: 0x0001302B
		private Name GetMethName(PREDEFMETH method)
		{
			return this.GetPredefName(PredefinedMembers.GetMethPredefName(method));
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00014E39 File Offset: 0x00013039
		private AggregateSymbol GetMethParent(PREDEFMETH method)
		{
			return this.GetOptPredefAgg(PredefinedMembers.GetMethPredefType(method));
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00014E47 File Offset: 0x00013047
		private PropertySymbol LoadProperty(PREDEFPROP property)
		{
			return this.LoadProperty(property, this.GetPropName(property), PredefinedMembers.GetPropGetter(property));
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00014E5D File Offset: 0x0001305D
		private Name GetPropName(PREDEFPROP property)
		{
			return this.GetPredefName(PredefinedMembers.GetPropPredefName(property));
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00014E6B File Offset: 0x0001306B
		private PropertySymbol LoadProperty(PREDEFPROP predefProp, Name propertyName, PREDEFMETH propertyGetter)
		{
			this.RuntimeBinderSymbolTable.AddPredefinedPropertyToSymbolTable(this.GetOptPredefAgg(PredefinedMembers.GetPropPredefType(predefProp)), propertyName);
			MethodSymbol method = this.GetMethod(propertyGetter);
			method.SetMethKind(MethodKindEnum.PropAccessor);
			return method.getProperty();
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00014E98 File Offset: 0x00013098
		private SymbolLoader GetSymbolLoader()
		{
			return this._loader;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00014EA0 File Offset: 0x000130A0
		private ErrorHandling GetErrorContext()
		{
			return this.GetSymbolLoader().GetErrorContext();
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00014EAD File Offset: 0x000130AD
		private TypeManager GetTypeManager()
		{
			return this.GetSymbolLoader().GetTypeManager();
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00014EBA File Offset: 0x000130BA
		private BSYMMGR getBSymmgr()
		{
			return this.GetSymbolLoader().getBSymmgr();
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00014EC7 File Offset: 0x000130C7
		private Name GetPredefName(PredefinedName pn)
		{
			return NameManager.GetPredefinedName(pn);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00014ECF File Offset: 0x000130CF
		private AggregateSymbol GetOptPredefAgg(PredefinedType pt)
		{
			return this.GetSymbolLoader().GetPredefAgg(pt);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00014EE0 File Offset: 0x000130E0
		private CType LoadTypeFromSignature(int[] signature, ref int indexIntoSignatures, TypeArray classTyVars)
		{
			MethodSignatureEnum methodSignatureEnum = (MethodSignatureEnum)signature[indexIntoSignatures];
			indexIntoSignatures++;
			switch (methodSignatureEnum)
			{
			case (MethodSignatureEnum)49:
				return this.GetTypeManager().GetVoid();
			case MethodSignatureEnum.SIG_CLASS_TYVAR:
			{
				int num = signature[indexIntoSignatures];
				indexIntoSignatures++;
				return classTyVars[num];
			}
			case MethodSignatureEnum.SIG_METH_TYVAR:
			{
				int num2 = signature[indexIntoSignatures];
				indexIntoSignatures++;
				return this.GetTypeManager().GetStdMethTypeVar(num2);
			}
			case MethodSignatureEnum.SIG_SZ_ARRAY:
			{
				CType ctype = this.LoadTypeFromSignature(signature, ref indexIntoSignatures, classTyVars);
				if (ctype == null)
				{
					return null;
				}
				return this.GetTypeManager().GetArray(ctype, 1, true);
			}
			case MethodSignatureEnum.SIG_REF:
			{
				CType ctype2 = this.LoadTypeFromSignature(signature, ref indexIntoSignatures, classTyVars);
				if (ctype2 == null)
				{
					return null;
				}
				return this.GetTypeManager().GetParameterModifier(ctype2, false);
			}
			case MethodSignatureEnum.SIG_OUT:
			{
				CType ctype3 = this.LoadTypeFromSignature(signature, ref indexIntoSignatures, classTyVars);
				if (ctype3 == null)
				{
					return null;
				}
				return this.GetTypeManager().GetParameterModifier(ctype3, true);
			}
			default:
			{
				AggregateSymbol optPredefAgg = this.GetOptPredefAgg((PredefinedType)methodSignatureEnum);
				if (optPredefAgg == null)
				{
					return null;
				}
				CType[] array = new CType[optPredefAgg.GetTypeVars().Count];
				for (int i = 0; i < optPredefAgg.GetTypeVars().Count; i++)
				{
					array[i] = this.LoadTypeFromSignature(signature, ref indexIntoSignatures, classTyVars);
					if (array[i] == null)
					{
						return null;
					}
				}
				AggregateType aggregate = this.GetTypeManager().GetAggregate(optPredefAgg, this.getBSymmgr().AllocParams(optPredefAgg.GetTypeVars().Count, array));
				if (aggregate.isPredefType(PredefinedType.PT_G_OPTIONAL))
				{
					return this.GetTypeManager().GetNubFromNullable(aggregate);
				}
				return aggregate;
			}
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0001504C File Offset: 0x0001324C
		private TypeArray LoadTypeArrayFromSignature(int[] signature, ref int indexIntoSignatures, TypeArray classTyVars)
		{
			int num = signature[indexIntoSignatures];
			indexIntoSignatures++;
			CType[] array = new CType[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = this.LoadTypeFromSignature(signature, ref indexIntoSignatures, classTyVars);
				if (array[i] == null)
				{
					return null;
				}
			}
			return this.getBSymmgr().AllocParams(num, array);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00015098 File Offset: 0x00013298
		public PredefinedMembers(SymbolLoader loader)
		{
			this._loader = loader;
			this._methods = new MethodSymbol[81];
			this._properties = new PropertySymbol[1];
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000150E4 File Offset: 0x000132E4
		public PropertySymbol GetProperty(PREDEFPROP property)
		{
			PropertySymbol propertySymbol;
			if ((propertySymbol = this._properties[(int)property]) == null)
			{
				propertySymbol = (this._properties[(int)property] = this.LoadProperty(property));
			}
			return propertySymbol;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00015110 File Offset: 0x00013310
		public MethodSymbol GetMethod(PREDEFMETH method)
		{
			MethodSymbol methodSymbol;
			if ((methodSymbol = this._methods[(int)method]) == null)
			{
				methodSymbol = (this._methods[(int)method] = this.LoadMethod(method));
			}
			return methodSymbol;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0001513C File Offset: 0x0001333C
		private MethodSymbol LoadMethod(AggregateSymbol type, int[] signature, int cMethodTyVars, Name methodName, ACCESS methodAccess, bool isStatic, bool isVirtual)
		{
			TypeArray typeVarsAll = type.GetTypeVarsAll();
			int num = 0;
			CType ctype = this.LoadTypeFromSignature(signature, ref num, typeVarsAll);
			TypeArray typeArray = this.LoadTypeArrayFromSignature(signature, ref num, typeVarsAll);
			this.GetTypeManager().GetStdMethTyVarArray(cMethodTyVars);
			MethodSymbol methodSymbol = this.LookupMethodWhileLoading(type, cMethodTyVars, methodName, methodAccess, isStatic, isVirtual, ctype, typeArray);
			if (methodSymbol == null)
			{
				this.RuntimeBinderSymbolTable.AddPredefinedMethodToSymbolTable(type, methodName);
				methodSymbol = this.LookupMethodWhileLoading(type, cMethodTyVars, methodName, methodAccess, isStatic, isVirtual, ctype, typeArray);
			}
			return methodSymbol;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x000151B4 File Offset: 0x000133B4
		private MethodSymbol LookupMethodWhileLoading(AggregateSymbol type, int cMethodTyVars, Name methodName, ACCESS methodAccess, bool isStatic, bool isVirtual, CType returnType, TypeArray argumentTypes)
		{
			for (Symbol symbol = this.GetSymbolLoader().LookupAggMember(methodName, type, symbmask_t.MASK_ALL); symbol != null; symbol = this.GetSymbolLoader().LookupNextSym(symbol, type, symbmask_t.MASK_ALL))
			{
				MethodSymbol methodSymbol;
				if ((methodSymbol = symbol as MethodSymbol) != null && (methodSymbol.GetAccess() == methodAccess || methodAccess == ACCESS.ACC_UNKNOWN) && methodSymbol.isStatic == isStatic && methodSymbol.isVirtual == isVirtual && methodSymbol.typeVars.Count == cMethodTyVars && this.GetTypeManager().SubstEqualTypes(methodSymbol.RetType, returnType, null, methodSymbol.typeVars, SubstTypeFlags.DenormMeth) && this.GetTypeManager().SubstEqualTypeArrays(methodSymbol.Params, argumentTypes, null, methodSymbol.typeVars, SubstTypeFlags.DenormMeth))
				{
					return methodSymbol;
				}
			}
			return null;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00015263 File Offset: 0x00013463
		private MethodSymbol LoadMethod(PREDEFMETH method)
		{
			return this.LoadMethod(this.GetMethParent(method), PredefinedMembers.GetMethSignature(method), PredefinedMembers.GetMethTyVars(method), this.GetMethName(method), PredefinedMembers.GetMethAccess(method), PredefinedMembers.IsMethStatic(method), PredefinedMembers.IsMethVirtual(method));
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00015297 File Offset: 0x00013497
		private static PredefinedName GetPropPredefName(PREDEFPROP property)
		{
			return PredefinedMembers.GetPropInfo(property).name;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x000152A4 File Offset: 0x000134A4
		private static PREDEFMETH GetPropGetter(PREDEFPROP property)
		{
			return PredefinedMembers.GetPropInfo(property).getter;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x000152B1 File Offset: 0x000134B1
		private static PredefinedType GetPropPredefType(PREDEFPROP property)
		{
			return PredefinedMembers.GetMethInfo(PredefinedMembers.GetPropGetter(property)).type;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x000152C3 File Offset: 0x000134C3
		private static PredefinedPropertyInfo GetPropInfo(PREDEFPROP property)
		{
			return PredefinedMembers.s_predefinedProperties[(int)property];
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000152CC File Offset: 0x000134CC
		private static PredefinedMethodInfo GetMethInfo(PREDEFMETH method)
		{
			return PredefinedMembers.s_predefinedMethods[(int)method];
		}

		// Token: 0x06000320 RID: 800 RVA: 0x000152D5 File Offset: 0x000134D5
		private static PredefinedName GetMethPredefName(PREDEFMETH method)
		{
			return PredefinedMembers.GetMethInfo(method).name;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000152E2 File Offset: 0x000134E2
		private static PredefinedType GetMethPredefType(PREDEFMETH method)
		{
			return PredefinedMembers.GetMethInfo(method).type;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000152EF File Offset: 0x000134EF
		private static bool IsMethStatic(PREDEFMETH method)
		{
			return PredefinedMembers.GetMethInfo(method).callingConvention == MethodCallingConventionEnum.Static;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000152FF File Offset: 0x000134FF
		private static bool IsMethVirtual(PREDEFMETH method)
		{
			return PredefinedMembers.GetMethInfo(method).callingConvention == MethodCallingConventionEnum.Virtual;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0001530F File Offset: 0x0001350F
		private static ACCESS GetMethAccess(PREDEFMETH method)
		{
			return PredefinedMembers.GetMethInfo(method).access;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0001531C File Offset: 0x0001351C
		private static int GetMethTyVars(PREDEFMETH method)
		{
			return PredefinedMembers.GetMethInfo(method).cTypeVars;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00015329 File Offset: 0x00013529
		private static int[] GetMethSignature(PREDEFMETH method)
		{
			return PredefinedMembers.GetMethInfo(method).signature;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00015338 File Offset: 0x00013538
		// Note: this type is marked as 'beforefieldinit'.
		static PredefinedMembers()
		{
			PredefinedMethodInfo[] array = new PredefinedMethodInfo[81];
			array[0] = new PredefinedMethodInfo(PREDEFMETH.PM_DECIMAL_OPDECREMENT, PredefinedType.PT_DECIMAL, PredefinedName.PN_OPDECREMENT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 6, 1, 6 });
			array[1] = new PredefinedMethodInfo(PREDEFMETH.PM_DECIMAL_OPINCREMENT, PredefinedType.PT_DECIMAL, PredefinedName.PN_OPINCREMENT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 6, 1, 6 });
			array[2] = new PredefinedMethodInfo(PREDEFMETH.PM_DECIMAL_OPUNARYMINUS, PredefinedType.PT_DECIMAL, PredefinedName.PN_OPUNARYMINUS, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 6, 1, 6 });
			array[3] = new PredefinedMethodInfo(PREDEFMETH.PM_DELEGATE_COMBINE, PredefinedType.PT_DELEGATE, PredefinedName.PN_COMBINE, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 17, 2, 17, 17 });
			array[4] = new PredefinedMethodInfo(PREDEFMETH.PM_DELEGATE_OPEQUALITY, PredefinedType.PT_DELEGATE, PredefinedName.PN_OPEQUALITY, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 8, 2, 17, 17 });
			array[5] = new PredefinedMethodInfo(PREDEFMETH.PM_DELEGATE_OPINEQUALITY, PredefinedType.PT_DELEGATE, PredefinedName.PN_OPINEQUALITY, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 8, 2, 17, 17 });
			array[6] = new PredefinedMethodInfo(PREDEFMETH.PM_DELEGATE_REMOVE, PredefinedType.PT_DELEGATE, PredefinedName.PN_REMOVE, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 17, 2, 17, 17 });
			array[7] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_ADD, PredefinedType.PT_EXPRESSION, PredefinedName.PN_ADD, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[8] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_ADD_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_ADD, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[9] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_ADDCHECKED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_ADDCHECKED, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[10] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_ADDCHECKED_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_ADDCHECKED, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[11] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_AND, PredefinedType.PT_EXPRESSION, PredefinedName.PN_AND, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[12] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_AND_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_AND, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[13] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_ANDALSO, PredefinedType.PT_EXPRESSION, PredefinedName.PN_ANDALSO, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[14] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_ANDALSO_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_ANDALSO, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[15] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_ARRAYINDEX, PredefinedType.PT_EXPRESSION, PredefinedName.PN_ARRAYINDEX, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[16] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_ARRAYINDEX2, PredefinedType.PT_EXPRESSION, PredefinedName.PN_ARRAYINDEX, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 37, 2, 31, 52, 31 });
			array[17] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_ASSIGN, PredefinedType.PT_EXPRESSION, PredefinedName.PN_ASSIGN, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[18] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_CONSTANT_OBJECT_TYPE, PredefinedType.PT_EXPRESSION, PredefinedName.PN_CONSTANT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 34, 2, 15, 20 });
			array[19] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_CONVERT, PredefinedType.PT_EXPRESSION, PredefinedName.PN_CONVERT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 33, 2, 31, 20 });
			array[20] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_CONVERT_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_CONVERT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 33, 3, 31, 20, 42 });
			array[21] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_CONVERTCHECKED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_CONVERTCHECKED, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 33, 2, 31, 20 });
			array[22] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_CONVERTCHECKED_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_CONVERTCHECKED, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 33, 3, 31, 20, 42 });
			array[23] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_DIVIDE, PredefinedType.PT_EXPRESSION, PredefinedName.PN_DIVIDE, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[24] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_DIVIDE_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_DIVIDE, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[25] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_EQUAL, PredefinedType.PT_EXPRESSION, PredefinedName.PN_EQUAL, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[26] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_EQUAL_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_EQUAL, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 4, 31, 31, 8, 42 });
			array[27] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_EXCLUSIVEOR, PredefinedType.PT_EXPRESSION, PredefinedName.PN_EXCLUSIVEOR, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[28] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_EXCLUSIVEOR_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_EXCLUSIVEOR, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[29] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_FIELD, PredefinedType.PT_EXPRESSION, PredefinedName.PN_CAP_FIELD, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 36, 2, 31, 41 });
			array[30] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_GREATERTHAN, PredefinedType.PT_EXPRESSION, PredefinedName.PN_GREATERTHAN, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[31] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_GREATERTHAN_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_GREATERTHAN, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 4, 31, 31, 8, 42 });
			array[32] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_GREATERTHANOREQUAL, PredefinedType.PT_EXPRESSION, PredefinedName.PN_GREATERTHANOREQUAL, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[33] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_GREATERTHANOREQUAL_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_GREATERTHANOREQUAL, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 4, 31, 31, 8, 42 });
			array[34] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_LAMBDA, PredefinedType.PT_EXPRESSION, PredefinedName.PN_LAMBDA, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 1, new int[] { 30, 51, 0, 2, 31, 52, 35 });
			array[35] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_LEFTSHIFT, PredefinedType.PT_EXPRESSION, PredefinedName.PN_LEFTSHIFT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[36] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_LEFTSHIFT_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_LEFTSHIFT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[37] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_LESSTHAN, PredefinedType.PT_EXPRESSION, PredefinedName.PN_LESSTHAN, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[38] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_LESSTHAN_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_LESSTHAN, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 4, 31, 31, 8, 42 });
			array[39] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_LESSTHANOREQUAL, PredefinedType.PT_EXPRESSION, PredefinedName.PN_LESSTHANOREQUAL, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[40] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_LESSTHANOREQUAL_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_LESSTHANOREQUAL, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 4, 31, 31, 8, 42 });
			array[41] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_MODULO, PredefinedType.PT_EXPRESSION, PredefinedName.PN_MODULO, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[42] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_MODULO_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_MODULO, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[43] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_MULTIPLY, PredefinedType.PT_EXPRESSION, PredefinedName.PN_MULTIPLY, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[44] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_MULTIPLY_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_MULTIPLY, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[45] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_MULTIPLYCHECKED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_MULTIPLYCHECKED, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[46] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_MULTIPLYCHECKED_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_MULTIPLYCHECKED, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[47] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_NOTEQUAL, PredefinedType.PT_EXPRESSION, PredefinedName.PN_NOTEQUAL, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[48] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_NOTEQUAL_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_NOTEQUAL, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 4, 31, 31, 8, 42 });
			array[49] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_OR, PredefinedType.PT_EXPRESSION, PredefinedName.PN_OR, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[50] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_OR_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_OR, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[51] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_ORELSE, PredefinedType.PT_EXPRESSION, PredefinedName.PN_ORELSE, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[52] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_ORELSE_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_ORELSE, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[53] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_PARAMETER, PredefinedType.PT_EXPRESSION, PredefinedName.PN_PARAMETER, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 35, 2, 20, 16 });
			array[54] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_RIGHTSHIFT, PredefinedType.PT_EXPRESSION, PredefinedName.PN_RIGHTSHIFT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[55] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_RIGHTSHIFT_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_RIGHTSHIFT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[56] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_SUBTRACT, PredefinedType.PT_EXPRESSION, PredefinedName.PN_SUBTRACT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[57] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_SUBTRACT_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_SUBTRACT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[58] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_SUBTRACTCHECKED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_SUBTRACTCHECKED, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 2, 31, 31 });
			array[59] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_SUBTRACTCHECKED_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_SUBTRACTCHECKED, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 32, 3, 31, 31, 42 });
			array[60] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_UNARYPLUS_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_PLUS, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 33, 2, 31, 42 });
			array[61] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_NEGATE, PredefinedType.PT_EXPRESSION, PredefinedName.PN_NEGATE, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 33, 1, 31 });
			array[62] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_NEGATE_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_NEGATE, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 33, 2, 31, 42 });
			array[63] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_NEGATECHECKED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_NEGATECHECKED, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 33, 1, 31 });
			array[64] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_NEGATECHECKED_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_NEGATECHECKED, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 33, 2, 31, 42 });
			array[65] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_CALL, PredefinedType.PT_EXPRESSION, PredefinedName.PN_CALL, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 37, 3, 31, 42, 52, 31 });
			array[66] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_NEW, PredefinedType.PT_EXPRESSION, PredefinedName.PN_NEW, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 38, 2, 43, 52, 31 });
			array[67] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_NEW_TYPE, PredefinedType.PT_EXPRESSION, PredefinedName.PN_NEW, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 38, 1, 20 });
			array[68] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_QUOTE, PredefinedType.PT_EXPRESSION, PredefinedName.PN_QUOTE, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 33, 1, 31 });
			array[69] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_NOT, PredefinedType.PT_EXPRESSION, PredefinedName.PN_NOT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 33, 1, 31 });
			array[70] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_NOT_USER_DEFINED, PredefinedType.PT_EXPRESSION, PredefinedName.PN_NOT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 33, 2, 31, 42 });
			array[71] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_NEWARRAYINIT, PredefinedType.PT_EXPRESSION, PredefinedName.PN_NEWARRAYINIT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 39, 2, 20, 52, 31 });
			array[72] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_PROPERTY, PredefinedType.PT_EXPRESSION, PredefinedName.PN_EXPRESSION_PROPERTY, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 36, 2, 31, 44 });
			array[73] = new PredefinedMethodInfo(PREDEFMETH.PM_EXPRESSION_INVOKE, PredefinedType.PT_EXPRESSION, PredefinedName.PN_INVOKE, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 40, 2, 31, 52, 31 });
			array[74] = new PredefinedMethodInfo(PREDEFMETH.PM_G_OPTIONAL_CTOR, PredefinedType.PT_G_OPTIONAL, PredefinedName.PN_CTOR, MethodCallingConventionEnum.Instance, ACCESS.ACC_PUBLIC, 0, new int[] { 49, 1, 50, 0 });
			int num = 75;
			PREDEFMETH predefmeth = PREDEFMETH.PM_G_OPTIONAL_GETVALUE;
			PredefinedType predefinedType = PredefinedType.PT_G_OPTIONAL;
			PredefinedName predefinedName = PredefinedName.PN_GETVALUE;
			MethodCallingConventionEnum methodCallingConventionEnum = MethodCallingConventionEnum.Instance;
			ACCESS access = ACCESS.ACC_PUBLIC;
			int num2 = 0;
			int[] array2 = new int[3];
			array2[0] = 50;
			array[num] = new PredefinedMethodInfo(predefmeth, predefinedType, predefinedName, methodCallingConventionEnum, access, num2, array2);
			array[76] = new PredefinedMethodInfo(PREDEFMETH.PM_STRING_CONCAT_OBJECT_2, PredefinedType.PT_STRING, PredefinedName.PN_CONCAT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 16, 2, 15, 15 });
			array[77] = new PredefinedMethodInfo(PREDEFMETH.PM_STRING_CONCAT_OBJECT_3, PredefinedType.PT_STRING, PredefinedName.PN_CONCAT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 16, 3, 15, 15, 15 });
			array[78] = new PredefinedMethodInfo(PREDEFMETH.PM_STRING_CONCAT_STRING_2, PredefinedType.PT_STRING, PredefinedName.PN_CONCAT, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 16, 2, 16, 16 });
			array[79] = new PredefinedMethodInfo(PREDEFMETH.PM_STRING_OPEQUALITY, PredefinedType.PT_STRING, PredefinedName.PN_OPEQUALITY, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 8, 2, 16, 16 });
			array[80] = new PredefinedMethodInfo(PREDEFMETH.PM_STRING_OPINEQUALITY, PredefinedType.PT_STRING, PredefinedName.PN_OPINEQUALITY, MethodCallingConventionEnum.Static, ACCESS.ACC_PUBLIC, 0, new int[] { 8, 2, 16, 16 });
			PredefinedMembers.s_predefinedMethods = array;
		}

		// Token: 0x04000470 RID: 1136
		private readonly SymbolLoader _loader;

		// Token: 0x04000471 RID: 1137
		internal SymbolTable RuntimeBinderSymbolTable;

		// Token: 0x04000472 RID: 1138
		private readonly MethodSymbol[] _methods = new MethodSymbol[81];

		// Token: 0x04000473 RID: 1139
		private readonly PropertySymbol[] _properties = new PropertySymbol[1];

		// Token: 0x04000474 RID: 1140
		private static readonly PredefinedPropertyInfo[] s_predefinedProperties = new PredefinedPropertyInfo[]
		{
			new PredefinedPropertyInfo(PREDEFPROP.PP_G_OPTIONAL_VALUE, PredefinedName.PN_CAP_VALUE, PREDEFMETH.PM_G_OPTIONAL_GETVALUE)
		};

		// Token: 0x04000475 RID: 1141
		private static readonly PredefinedMethodInfo[] s_predefinedMethods;
	}
}
