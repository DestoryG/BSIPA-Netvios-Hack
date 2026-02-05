using System;
using System.Runtime.InteropServices;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000229 RID: 553
	[Guid("BA3FEE4C-ECB9-4e41-83B7-183FA41CD859")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IMetaDataEmit
	{
		// Token: 0x0600107D RID: 4221
		void SetModuleProps(string szName);

		// Token: 0x0600107E RID: 4222
		void Save(string szFile, uint dwSaveFlags);

		// Token: 0x0600107F RID: 4223
		void SaveToStream(IntPtr pIStream, uint dwSaveFlags);

		// Token: 0x06001080 RID: 4224
		uint GetSaveSize(uint fSave);

		// Token: 0x06001081 RID: 4225
		uint DefineTypeDef(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements);

		// Token: 0x06001082 RID: 4226
		uint DefineNestedType(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements, uint tdEncloser);

		// Token: 0x06001083 RID: 4227
		void SetHandler([MarshalAs(UnmanagedType.IUnknown)] [In] object pUnk);

		// Token: 0x06001084 RID: 4228
		uint DefineMethod(uint td, IntPtr zName, uint dwMethodFlags, IntPtr pvSigBlob, uint cbSigBlob, uint ulCodeRVA, uint dwImplFlags);

		// Token: 0x06001085 RID: 4229
		void DefineMethodImpl(uint td, uint tkBody, uint tkDecl);

		// Token: 0x06001086 RID: 4230
		uint DefineTypeRefByName(uint tkResolutionScope, IntPtr szName);

		// Token: 0x06001087 RID: 4231
		uint DefineImportType(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint tdImport, IntPtr pAssemEmit);

		// Token: 0x06001088 RID: 4232
		uint DefineMemberRef(uint tkImport, string szName, IntPtr pvSigBlob, uint cbSigBlob);

		// Token: 0x06001089 RID: 4233
		uint DefineImportMember(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint mbMember, IntPtr pAssemEmit, uint tkParent);

		// Token: 0x0600108A RID: 4234
		uint DefineEvent(uint td, string szEvent, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods);

		// Token: 0x0600108B RID: 4235
		void SetClassLayout(uint td, uint dwPackSize, IntPtr rFieldOffsets, uint ulClassSize);

		// Token: 0x0600108C RID: 4236
		void DeleteClassLayout(uint td);

		// Token: 0x0600108D RID: 4237
		void SetFieldMarshal(uint tk, IntPtr pvNativeType, uint cbNativeType);

		// Token: 0x0600108E RID: 4238
		void DeleteFieldMarshal(uint tk);

		// Token: 0x0600108F RID: 4239
		uint DefinePermissionSet(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission);

		// Token: 0x06001090 RID: 4240
		void SetRVA(uint md, uint ulRVA);

		// Token: 0x06001091 RID: 4241
		uint GetTokenFromSig(IntPtr pvSig, uint cbSig);

		// Token: 0x06001092 RID: 4242
		uint DefineModuleRef(string szName);

		// Token: 0x06001093 RID: 4243
		void SetParent(uint mr, uint tk);

		// Token: 0x06001094 RID: 4244
		uint GetTokenFromTypeSpec(IntPtr pvSig, uint cbSig);

		// Token: 0x06001095 RID: 4245
		void SaveToMemory(IntPtr pbData, uint cbData);

		// Token: 0x06001096 RID: 4246
		uint DefineUserString(string szString, uint cchString);

		// Token: 0x06001097 RID: 4247
		void DeleteToken(uint tkObj);

		// Token: 0x06001098 RID: 4248
		void SetMethodProps(uint md, uint dwMethodFlags, uint ulCodeRVA, uint dwImplFlags);

		// Token: 0x06001099 RID: 4249
		void SetTypeDefProps(uint td, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements);

		// Token: 0x0600109A RID: 4250
		void SetEventProps(uint ev, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods);

		// Token: 0x0600109B RID: 4251
		uint SetPermissionSetProps(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission);

		// Token: 0x0600109C RID: 4252
		void DefinePinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL);

		// Token: 0x0600109D RID: 4253
		void SetPinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL);

		// Token: 0x0600109E RID: 4254
		void DeletePinvokeMap(uint tk);

		// Token: 0x0600109F RID: 4255
		uint DefineCustomAttribute(uint tkObj, uint tkType, IntPtr pCustomAttribute, uint cbCustomAttribute);

		// Token: 0x060010A0 RID: 4256
		void SetCustomAttributeValue(uint pcv, IntPtr pCustomAttribute, uint cbCustomAttribute);

		// Token: 0x060010A1 RID: 4257
		uint DefineField(uint td, string szName, uint dwFieldFlags, IntPtr pvSigBlob, uint cbSigBlob, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x060010A2 RID: 4258
		uint DefineProperty(uint td, string szProperty, uint dwPropFlags, IntPtr pvSig, uint cbSig, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods);

		// Token: 0x060010A3 RID: 4259
		uint DefineParam(uint md, uint ulParamSeq, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x060010A4 RID: 4260
		void SetFieldProps(uint fd, uint dwFieldFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x060010A5 RID: 4261
		void SetPropertyProps(uint pr, uint dwPropFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods);

		// Token: 0x060010A6 RID: 4262
		void SetParamProps(uint pd, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x060010A7 RID: 4263
		uint DefineSecurityAttributeSet(uint tkObj, IntPtr rSecAttrs, uint cSecAttrs);

		// Token: 0x060010A8 RID: 4264
		void ApplyEditAndContinue([MarshalAs(UnmanagedType.IUnknown)] object pImport);

		// Token: 0x060010A9 RID: 4265
		uint TranslateSigWithScope(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport import, IntPtr pbSigBlob, uint cbSigBlob, IntPtr pAssemEmit, IMetaDataEmit emit, IntPtr pvTranslatedSig, uint cbTranslatedSigMax);

		// Token: 0x060010AA RID: 4266
		void SetMethodImplFlags(uint md, uint dwImplFlags);

		// Token: 0x060010AB RID: 4267
		void SetFieldRVA(uint fd, uint ulRVA);

		// Token: 0x060010AC RID: 4268
		void Merge(IMetaDataImport pImport, IntPtr pHostMapToken, [MarshalAs(UnmanagedType.IUnknown)] object pHandler);

		// Token: 0x060010AD RID: 4269
		void MergeEnd();
	}
}
