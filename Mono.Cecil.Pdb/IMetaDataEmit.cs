using System;
using System.Runtime.InteropServices;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000004 RID: 4
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("BA3FEE4C-ECB9-4e41-83B7-183FA41CD859")]
	[ComImport]
	internal interface IMetaDataEmit
	{
		// Token: 0x0600001C RID: 28
		void SetModuleProps(string szName);

		// Token: 0x0600001D RID: 29
		void Save(string szFile, uint dwSaveFlags);

		// Token: 0x0600001E RID: 30
		void SaveToStream(IntPtr pIStream, uint dwSaveFlags);

		// Token: 0x0600001F RID: 31
		uint GetSaveSize(uint fSave);

		// Token: 0x06000020 RID: 32
		uint DefineTypeDef(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements);

		// Token: 0x06000021 RID: 33
		uint DefineNestedType(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements, uint tdEncloser);

		// Token: 0x06000022 RID: 34
		void SetHandler([MarshalAs(UnmanagedType.IUnknown)] [In] object pUnk);

		// Token: 0x06000023 RID: 35
		uint DefineMethod(uint td, IntPtr zName, uint dwMethodFlags, IntPtr pvSigBlob, uint cbSigBlob, uint ulCodeRVA, uint dwImplFlags);

		// Token: 0x06000024 RID: 36
		void DefineMethodImpl(uint td, uint tkBody, uint tkDecl);

		// Token: 0x06000025 RID: 37
		uint DefineTypeRefByName(uint tkResolutionScope, IntPtr szName);

		// Token: 0x06000026 RID: 38
		uint DefineImportType(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint tdImport, IntPtr pAssemEmit);

		// Token: 0x06000027 RID: 39
		uint DefineMemberRef(uint tkImport, string szName, IntPtr pvSigBlob, uint cbSigBlob);

		// Token: 0x06000028 RID: 40
		uint DefineImportMember(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint mbMember, IntPtr pAssemEmit, uint tkParent);

		// Token: 0x06000029 RID: 41
		uint DefineEvent(uint td, string szEvent, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods);

		// Token: 0x0600002A RID: 42
		void SetClassLayout(uint td, uint dwPackSize, IntPtr rFieldOffsets, uint ulClassSize);

		// Token: 0x0600002B RID: 43
		void DeleteClassLayout(uint td);

		// Token: 0x0600002C RID: 44
		void SetFieldMarshal(uint tk, IntPtr pvNativeType, uint cbNativeType);

		// Token: 0x0600002D RID: 45
		void DeleteFieldMarshal(uint tk);

		// Token: 0x0600002E RID: 46
		uint DefinePermissionSet(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission);

		// Token: 0x0600002F RID: 47
		void SetRVA(uint md, uint ulRVA);

		// Token: 0x06000030 RID: 48
		uint GetTokenFromSig(IntPtr pvSig, uint cbSig);

		// Token: 0x06000031 RID: 49
		uint DefineModuleRef(string szName);

		// Token: 0x06000032 RID: 50
		void SetParent(uint mr, uint tk);

		// Token: 0x06000033 RID: 51
		uint GetTokenFromTypeSpec(IntPtr pvSig, uint cbSig);

		// Token: 0x06000034 RID: 52
		void SaveToMemory(IntPtr pbData, uint cbData);

		// Token: 0x06000035 RID: 53
		uint DefineUserString(string szString, uint cchString);

		// Token: 0x06000036 RID: 54
		void DeleteToken(uint tkObj);

		// Token: 0x06000037 RID: 55
		void SetMethodProps(uint md, uint dwMethodFlags, uint ulCodeRVA, uint dwImplFlags);

		// Token: 0x06000038 RID: 56
		void SetTypeDefProps(uint td, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements);

		// Token: 0x06000039 RID: 57
		void SetEventProps(uint ev, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods);

		// Token: 0x0600003A RID: 58
		uint SetPermissionSetProps(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission);

		// Token: 0x0600003B RID: 59
		void DefinePinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL);

		// Token: 0x0600003C RID: 60
		void SetPinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL);

		// Token: 0x0600003D RID: 61
		void DeletePinvokeMap(uint tk);

		// Token: 0x0600003E RID: 62
		uint DefineCustomAttribute(uint tkObj, uint tkType, IntPtr pCustomAttribute, uint cbCustomAttribute);

		// Token: 0x0600003F RID: 63
		void SetCustomAttributeValue(uint pcv, IntPtr pCustomAttribute, uint cbCustomAttribute);

		// Token: 0x06000040 RID: 64
		uint DefineField(uint td, string szName, uint dwFieldFlags, IntPtr pvSigBlob, uint cbSigBlob, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x06000041 RID: 65
		uint DefineProperty(uint td, string szProperty, uint dwPropFlags, IntPtr pvSig, uint cbSig, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods);

		// Token: 0x06000042 RID: 66
		uint DefineParam(uint md, uint ulParamSeq, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x06000043 RID: 67
		void SetFieldProps(uint fd, uint dwFieldFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x06000044 RID: 68
		void SetPropertyProps(uint pr, uint dwPropFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods);

		// Token: 0x06000045 RID: 69
		void SetParamProps(uint pd, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x06000046 RID: 70
		uint DefineSecurityAttributeSet(uint tkObj, IntPtr rSecAttrs, uint cSecAttrs);

		// Token: 0x06000047 RID: 71
		void ApplyEditAndContinue([MarshalAs(UnmanagedType.IUnknown)] object pImport);

		// Token: 0x06000048 RID: 72
		uint TranslateSigWithScope(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport import, IntPtr pbSigBlob, uint cbSigBlob, IntPtr pAssemEmit, IMetaDataEmit emit, IntPtr pvTranslatedSig, uint cbTranslatedSigMax);

		// Token: 0x06000049 RID: 73
		void SetMethodImplFlags(uint md, uint dwImplFlags);

		// Token: 0x0600004A RID: 74
		void SetFieldRVA(uint fd, uint ulRVA);

		// Token: 0x0600004B RID: 75
		void Merge(IMetaDataImport pImport, IntPtr pHostMapToken, [MarshalAs(UnmanagedType.IUnknown)] object pHandler);

		// Token: 0x0600004C RID: 76
		void MergeEnd();
	}
}
