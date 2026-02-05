using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000005 RID: 5
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44")]
	[ComImport]
	internal interface IMetaDataImport
	{
		// Token: 0x0600004D RID: 77
		[PreserveSig]
		void CloseEnum(uint hEnum);

		// Token: 0x0600004E RID: 78
		uint CountEnum(uint hEnum);

		// Token: 0x0600004F RID: 79
		void ResetEnum(uint hEnum, uint ulPos);

		// Token: 0x06000050 RID: 80
		uint EnumTypeDefs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rTypeDefs, uint cMax);

		// Token: 0x06000051 RID: 81
		uint EnumInterfaceImpls(ref uint phEnum, uint td, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rImpls, uint cMax);

		// Token: 0x06000052 RID: 82
		uint EnumTypeRefs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rTypeRefs, uint cMax);

		// Token: 0x06000053 RID: 83
		uint FindTypeDefByName(string szTypeDef, uint tkEnclosingClass);

		// Token: 0x06000054 RID: 84
		Guid GetScopeProps(StringBuilder szName, uint cchName, out uint pchName);

		// Token: 0x06000055 RID: 85
		uint GetModuleFromScope();

		// Token: 0x06000056 RID: 86
		uint GetTypeDefProps(uint td, IntPtr szTypeDef, uint cchTypeDef, out uint pchTypeDef, IntPtr pdwTypeDefFlags);

		// Token: 0x06000057 RID: 87
		uint GetInterfaceImplProps(uint iiImpl, out uint pClass);

		// Token: 0x06000058 RID: 88
		uint GetTypeRefProps(uint tr, out uint ptkResolutionScope, StringBuilder szName, uint cchName);

		// Token: 0x06000059 RID: 89
		uint ResolveTypeRef(uint tr, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppIScope);

		// Token: 0x0600005A RID: 90
		uint EnumMembers(ref uint phEnum, uint cl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rMembers, uint cMax);

		// Token: 0x0600005B RID: 91
		uint EnumMembersWithName(ref uint phEnum, uint cl, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMembers, uint cMax);

		// Token: 0x0600005C RID: 92
		uint EnumMethods(ref uint phEnum, uint cl, IntPtr rMethods, uint cMax);

		// Token: 0x0600005D RID: 93
		uint EnumMethodsWithName(ref uint phEnum, uint cl, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMethods, uint cMax);

		// Token: 0x0600005E RID: 94
		uint EnumFields(ref uint phEnum, uint cl, IntPtr rFields, uint cMax);

		// Token: 0x0600005F RID: 95
		uint EnumFieldsWithName(ref uint phEnum, uint cl, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rFields, uint cMax);

		// Token: 0x06000060 RID: 96
		uint EnumParams(ref uint phEnum, uint mb, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rParams, uint cMax);

		// Token: 0x06000061 RID: 97
		uint EnumMemberRefs(ref uint phEnum, uint tkParent, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rMemberRefs, uint cMax);

		// Token: 0x06000062 RID: 98
		uint EnumMethodImpls(ref uint phEnum, uint td, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMethodBody, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMethodDecl, uint cMax);

		// Token: 0x06000063 RID: 99
		uint EnumPermissionSets(ref uint phEnum, uint tk, uint dwActions, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rPermission, uint cMax);

		// Token: 0x06000064 RID: 100
		uint FindMember(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x06000065 RID: 101
		uint FindMethod(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x06000066 RID: 102
		uint FindField(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x06000067 RID: 103
		uint FindMemberRef(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x06000068 RID: 104
		uint GetMethodProps(uint mb, out uint pClass, IntPtr szMethod, uint cchMethod, out uint pchMethod, IntPtr pdwAttr, IntPtr ppvSigBlob, IntPtr pcbSigBlob, IntPtr pulCodeRVA);

		// Token: 0x06000069 RID: 105
		uint GetMemberRefProps(uint mr, ref uint ptk, StringBuilder szMember, uint cchMember, out uint pchMember, out IntPtr ppvSigBlob);

		// Token: 0x0600006A RID: 106
		uint EnumProperties(ref uint phEnum, uint td, IntPtr rProperties, uint cMax);

		// Token: 0x0600006B RID: 107
		uint EnumEvents(ref uint phEnum, uint td, IntPtr rEvents, uint cMax);

		// Token: 0x0600006C RID: 108
		uint GetEventProps(uint ev, out uint pClass, StringBuilder szEvent, uint cchEvent, out uint pchEvent, out uint pdwEventFlags, out uint ptkEventType, out uint pmdAddOn, out uint pmdRemoveOn, out uint pmdFire, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 11)] uint[] rmdOtherMethod, uint cMax);

		// Token: 0x0600006D RID: 109
		uint EnumMethodSemantics(ref uint phEnum, uint mb, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rEventProp, uint cMax);

		// Token: 0x0600006E RID: 110
		uint GetMethodSemantics(uint mb, uint tkEventProp);

		// Token: 0x0600006F RID: 111
		uint GetClassLayout(uint td, out uint pdwPackSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] IntPtr rFieldOffset, uint cMax, out uint pcFieldOffset);

		// Token: 0x06000070 RID: 112
		uint GetFieldMarshal(uint tk, out IntPtr ppvNativeType);

		// Token: 0x06000071 RID: 113
		uint GetRVA(uint tk, out uint pulCodeRVA);

		// Token: 0x06000072 RID: 114
		uint GetPermissionSetProps(uint pm, out uint pdwAction, out IntPtr ppvPermission);

		// Token: 0x06000073 RID: 115
		uint GetSigFromToken(uint mdSig, out IntPtr ppvSig);

		// Token: 0x06000074 RID: 116
		uint GetModuleRefProps(uint mur, StringBuilder szName, uint cchName);

		// Token: 0x06000075 RID: 117
		uint EnumModuleRefs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rModuleRefs, uint cmax);

		// Token: 0x06000076 RID: 118
		uint GetTypeSpecFromToken(uint typespec, out IntPtr ppvSig);

		// Token: 0x06000077 RID: 119
		uint GetNameFromToken(uint tk);

		// Token: 0x06000078 RID: 120
		uint EnumUnresolvedMethods(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rMethods, uint cMax);

		// Token: 0x06000079 RID: 121
		uint GetUserString(uint stk, StringBuilder szString, uint cchString);

		// Token: 0x0600007A RID: 122
		uint GetPinvokeMap(uint tk, out uint pdwMappingFlags, StringBuilder szImportName, uint cchImportName, out uint pchImportName);

		// Token: 0x0600007B RID: 123
		uint EnumSignatures(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rSignatures, uint cmax);

		// Token: 0x0600007C RID: 124
		uint EnumTypeSpecs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rTypeSpecs, uint cmax);

		// Token: 0x0600007D RID: 125
		uint EnumUserStrings(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rStrings, uint cmax);

		// Token: 0x0600007E RID: 126
		[PreserveSig]
		int GetParamForMethodIndex(uint md, uint ulParamSeq, out uint pParam);

		// Token: 0x0600007F RID: 127
		uint EnumCustomAttributes(ref uint phEnum, uint tk, uint tkType, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rCustomAttributes, uint cMax);

		// Token: 0x06000080 RID: 128
		uint GetCustomAttributeProps(uint cv, out uint ptkObj, out uint ptkType, out IntPtr ppBlob);

		// Token: 0x06000081 RID: 129
		uint FindTypeRef(uint tkResolutionScope, string szName);

		// Token: 0x06000082 RID: 130
		uint GetMemberProps(uint mb, out uint pClass, StringBuilder szMember, uint cchMember, out uint pchMember, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pulCodeRVA, out uint pdwImplFlags, out uint pdwCPlusTypeFlag, out IntPtr ppValue);

		// Token: 0x06000083 RID: 131
		uint GetFieldProps(uint mb, out uint pClass, StringBuilder szField, uint cchField, out uint pchField, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pdwCPlusTypeFlag, out IntPtr ppValue);

		// Token: 0x06000084 RID: 132
		uint GetPropertyProps(uint prop, out uint pClass, StringBuilder szProperty, uint cchProperty, out uint pchProperty, out uint pdwPropFlags, out IntPtr ppvSig, out uint pbSig, out uint pdwCPlusTypeFlag, out IntPtr ppDefaultValue, out uint pcchDefaultValue, out uint pmdSetter, out uint pmdGetter, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 14)] uint[] rmdOtherMethod, uint cMax);

		// Token: 0x06000085 RID: 133
		uint GetParamProps(uint tk, out uint pmd, out uint pulSequence, StringBuilder szName, uint cchName, out uint pchName, out uint pdwAttr, out uint pdwCPlusTypeFlag, out IntPtr ppValue);

		// Token: 0x06000086 RID: 134
		uint GetCustomAttributeByName(uint tkObj, string szName, out IntPtr ppData);

		// Token: 0x06000087 RID: 135
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsValidToken(uint tk);

		// Token: 0x06000088 RID: 136
		uint GetNestedClassProps(uint tdNestedClass);

		// Token: 0x06000089 RID: 137
		uint GetNativeCallConvFromSig(IntPtr pvSig, uint cbSig);

		// Token: 0x0600008A RID: 138
		int IsGlobal(uint pd);
	}
}
