using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200022A RID: 554
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44")]
	[ComImport]
	internal interface IMetaDataImport
	{
		// Token: 0x060010AE RID: 4270
		[PreserveSig]
		void CloseEnum(uint hEnum);

		// Token: 0x060010AF RID: 4271
		uint CountEnum(uint hEnum);

		// Token: 0x060010B0 RID: 4272
		void ResetEnum(uint hEnum, uint ulPos);

		// Token: 0x060010B1 RID: 4273
		uint EnumTypeDefs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rTypeDefs, uint cMax);

		// Token: 0x060010B2 RID: 4274
		uint EnumInterfaceImpls(ref uint phEnum, uint td, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rImpls, uint cMax);

		// Token: 0x060010B3 RID: 4275
		uint EnumTypeRefs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rTypeRefs, uint cMax);

		// Token: 0x060010B4 RID: 4276
		uint FindTypeDefByName(string szTypeDef, uint tkEnclosingClass);

		// Token: 0x060010B5 RID: 4277
		Guid GetScopeProps(StringBuilder szName, uint cchName, out uint pchName);

		// Token: 0x060010B6 RID: 4278
		uint GetModuleFromScope();

		// Token: 0x060010B7 RID: 4279
		uint GetTypeDefProps(uint td, IntPtr szTypeDef, uint cchTypeDef, out uint pchTypeDef, IntPtr pdwTypeDefFlags);

		// Token: 0x060010B8 RID: 4280
		uint GetInterfaceImplProps(uint iiImpl, out uint pClass);

		// Token: 0x060010B9 RID: 4281
		uint GetTypeRefProps(uint tr, out uint ptkResolutionScope, StringBuilder szName, uint cchName);

		// Token: 0x060010BA RID: 4282
		uint ResolveTypeRef(uint tr, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppIScope);

		// Token: 0x060010BB RID: 4283
		uint EnumMembers(ref uint phEnum, uint cl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rMembers, uint cMax);

		// Token: 0x060010BC RID: 4284
		uint EnumMembersWithName(ref uint phEnum, uint cl, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMembers, uint cMax);

		// Token: 0x060010BD RID: 4285
		uint EnumMethods(ref uint phEnum, uint cl, IntPtr rMethods, uint cMax);

		// Token: 0x060010BE RID: 4286
		uint EnumMethodsWithName(ref uint phEnum, uint cl, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMethods, uint cMax);

		// Token: 0x060010BF RID: 4287
		uint EnumFields(ref uint phEnum, uint cl, IntPtr rFields, uint cMax);

		// Token: 0x060010C0 RID: 4288
		uint EnumFieldsWithName(ref uint phEnum, uint cl, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rFields, uint cMax);

		// Token: 0x060010C1 RID: 4289
		uint EnumParams(ref uint phEnum, uint mb, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rParams, uint cMax);

		// Token: 0x060010C2 RID: 4290
		uint EnumMemberRefs(ref uint phEnum, uint tkParent, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rMemberRefs, uint cMax);

		// Token: 0x060010C3 RID: 4291
		uint EnumMethodImpls(ref uint phEnum, uint td, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMethodBody, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMethodDecl, uint cMax);

		// Token: 0x060010C4 RID: 4292
		uint EnumPermissionSets(ref uint phEnum, uint tk, uint dwActions, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rPermission, uint cMax);

		// Token: 0x060010C5 RID: 4293
		uint FindMember(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x060010C6 RID: 4294
		uint FindMethod(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x060010C7 RID: 4295
		uint FindField(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x060010C8 RID: 4296
		uint FindMemberRef(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x060010C9 RID: 4297
		uint GetMethodProps(uint mb, out uint pClass, IntPtr szMethod, uint cchMethod, out uint pchMethod, IntPtr pdwAttr, IntPtr ppvSigBlob, IntPtr pcbSigBlob, IntPtr pulCodeRVA);

		// Token: 0x060010CA RID: 4298
		uint GetMemberRefProps(uint mr, ref uint ptk, StringBuilder szMember, uint cchMember, out uint pchMember, out IntPtr ppvSigBlob);

		// Token: 0x060010CB RID: 4299
		uint EnumProperties(ref uint phEnum, uint td, IntPtr rProperties, uint cMax);

		// Token: 0x060010CC RID: 4300
		uint EnumEvents(ref uint phEnum, uint td, IntPtr rEvents, uint cMax);

		// Token: 0x060010CD RID: 4301
		uint GetEventProps(uint ev, out uint pClass, StringBuilder szEvent, uint cchEvent, out uint pchEvent, out uint pdwEventFlags, out uint ptkEventType, out uint pmdAddOn, out uint pmdRemoveOn, out uint pmdFire, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 11)] uint[] rmdOtherMethod, uint cMax);

		// Token: 0x060010CE RID: 4302
		uint EnumMethodSemantics(ref uint phEnum, uint mb, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rEventProp, uint cMax);

		// Token: 0x060010CF RID: 4303
		uint GetMethodSemantics(uint mb, uint tkEventProp);

		// Token: 0x060010D0 RID: 4304
		uint GetClassLayout(uint td, out uint pdwPackSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] IntPtr rFieldOffset, uint cMax, out uint pcFieldOffset);

		// Token: 0x060010D1 RID: 4305
		uint GetFieldMarshal(uint tk, out IntPtr ppvNativeType);

		// Token: 0x060010D2 RID: 4306
		uint GetRVA(uint tk, out uint pulCodeRVA);

		// Token: 0x060010D3 RID: 4307
		uint GetPermissionSetProps(uint pm, out uint pdwAction, out IntPtr ppvPermission);

		// Token: 0x060010D4 RID: 4308
		uint GetSigFromToken(uint mdSig, out IntPtr ppvSig);

		// Token: 0x060010D5 RID: 4309
		uint GetModuleRefProps(uint mur, StringBuilder szName, uint cchName);

		// Token: 0x060010D6 RID: 4310
		uint EnumModuleRefs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rModuleRefs, uint cmax);

		// Token: 0x060010D7 RID: 4311
		uint GetTypeSpecFromToken(uint typespec, out IntPtr ppvSig);

		// Token: 0x060010D8 RID: 4312
		uint GetNameFromToken(uint tk);

		// Token: 0x060010D9 RID: 4313
		uint EnumUnresolvedMethods(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rMethods, uint cMax);

		// Token: 0x060010DA RID: 4314
		uint GetUserString(uint stk, StringBuilder szString, uint cchString);

		// Token: 0x060010DB RID: 4315
		uint GetPinvokeMap(uint tk, out uint pdwMappingFlags, StringBuilder szImportName, uint cchImportName, out uint pchImportName);

		// Token: 0x060010DC RID: 4316
		uint EnumSignatures(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rSignatures, uint cmax);

		// Token: 0x060010DD RID: 4317
		uint EnumTypeSpecs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rTypeSpecs, uint cmax);

		// Token: 0x060010DE RID: 4318
		uint EnumUserStrings(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rStrings, uint cmax);

		// Token: 0x060010DF RID: 4319
		[PreserveSig]
		int GetParamForMethodIndex(uint md, uint ulParamSeq, out uint pParam);

		// Token: 0x060010E0 RID: 4320
		uint EnumCustomAttributes(ref uint phEnum, uint tk, uint tkType, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rCustomAttributes, uint cMax);

		// Token: 0x060010E1 RID: 4321
		uint GetCustomAttributeProps(uint cv, out uint ptkObj, out uint ptkType, out IntPtr ppBlob);

		// Token: 0x060010E2 RID: 4322
		uint FindTypeRef(uint tkResolutionScope, string szName);

		// Token: 0x060010E3 RID: 4323
		uint GetMemberProps(uint mb, out uint pClass, StringBuilder szMember, uint cchMember, out uint pchMember, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pulCodeRVA, out uint pdwImplFlags, out uint pdwCPlusTypeFlag, out IntPtr ppValue);

		// Token: 0x060010E4 RID: 4324
		uint GetFieldProps(uint mb, out uint pClass, StringBuilder szField, uint cchField, out uint pchField, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pdwCPlusTypeFlag, out IntPtr ppValue);

		// Token: 0x060010E5 RID: 4325
		uint GetPropertyProps(uint prop, out uint pClass, StringBuilder szProperty, uint cchProperty, out uint pchProperty, out uint pdwPropFlags, out IntPtr ppvSig, out uint pbSig, out uint pdwCPlusTypeFlag, out IntPtr ppDefaultValue, out uint pcchDefaultValue, out uint pmdSetter, out uint pmdGetter, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 14)] uint[] rmdOtherMethod, uint cMax);

		// Token: 0x060010E6 RID: 4326
		uint GetParamProps(uint tk, out uint pmd, out uint pulSequence, StringBuilder szName, uint cchName, out uint pchName, out uint pdwAttr, out uint pdwCPlusTypeFlag, out IntPtr ppValue);

		// Token: 0x060010E7 RID: 4327
		uint GetCustomAttributeByName(uint tkObj, string szName, out IntPtr ppData);

		// Token: 0x060010E8 RID: 4328
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsValidToken(uint tk);

		// Token: 0x060010E9 RID: 4329
		uint GetNestedClassProps(uint tdNestedClass);

		// Token: 0x060010EA RID: 4330
		uint GetNativeCallConvFromSig(IntPtr pvSig, uint cbSig);

		// Token: 0x060010EB RID: 4331
		int IsGlobal(uint pd);
	}
}
