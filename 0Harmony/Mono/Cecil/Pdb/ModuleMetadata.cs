using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200022B RID: 555
	internal class ModuleMetadata : IMetaDataEmit, IMetaDataImport
	{
		// Token: 0x060010EC RID: 4332 RVA: 0x00037E4F File Offset: 0x0003604F
		public ModuleMetadata(ModuleDefinition module)
		{
			this.module = module;
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00037E5E File Offset: 0x0003605E
		private bool TryGetType(uint token, out TypeDefinition type)
		{
			if (this.types == null)
			{
				this.InitializeMetadata(this.module);
			}
			return this.types.TryGetValue(token, out type);
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00037E81 File Offset: 0x00036081
		private bool TryGetMethod(uint token, out MethodDefinition method)
		{
			if (this.methods == null)
			{
				this.InitializeMetadata(this.module);
			}
			return this.methods.TryGetValue(token, out method);
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x00037EA4 File Offset: 0x000360A4
		private void InitializeMetadata(ModuleDefinition module)
		{
			this.types = new Dictionary<uint, TypeDefinition>();
			this.methods = new Dictionary<uint, MethodDefinition>();
			foreach (TypeDefinition typeDefinition in module.GetTypes())
			{
				this.types.Add(typeDefinition.MetadataToken.ToUInt32(), typeDefinition);
				this.InitializeMethods(typeDefinition);
			}
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00037F24 File Offset: 0x00036124
		private void InitializeMethods(TypeDefinition type)
		{
			foreach (MethodDefinition methodDefinition in type.Methods)
			{
				this.methods.Add(methodDefinition.MetadataToken.ToUInt32(), methodDefinition);
			}
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetModuleProps(string szName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void Save(string szFile, uint dwSaveFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SaveToStream(IntPtr pIStream, uint dwSaveFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetSaveSize(uint fSave)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineTypeDef(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineNestedType(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements, uint tdEncloser)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetHandler(object pUnk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineMethod(uint td, IntPtr zName, uint dwMethodFlags, IntPtr pvSigBlob, uint cbSigBlob, uint ulCodeRVA, uint dwImplFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void DefineMethodImpl(uint td, uint tkBody, uint tkDecl)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineTypeRefByName(uint tkResolutionScope, IntPtr szName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineImportType(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint tdImport, IntPtr pAssemEmit)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineMemberRef(uint tkImport, string szName, IntPtr pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineImportMember(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint mbMember, IntPtr pAssemEmit, uint tkParent)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineEvent(uint td, string szEvent, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetClassLayout(uint td, uint dwPackSize, IntPtr rFieldOffsets, uint ulClassSize)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void DeleteClassLayout(uint td)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetFieldMarshal(uint tk, IntPtr pvNativeType, uint cbNativeType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void DeleteFieldMarshal(uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefinePermissionSet(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetRVA(uint md, uint ulRVA)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetTokenFromSig(IntPtr pvSig, uint cbSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineModuleRef(string szName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetParent(uint mr, uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetTokenFromTypeSpec(IntPtr pvSig, uint cbSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SaveToMemory(IntPtr pbData, uint cbData)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineUserString(string szString, uint cchString)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void DeleteToken(uint tkObj)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetMethodProps(uint md, uint dwMethodFlags, uint ulCodeRVA, uint dwImplFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetTypeDefProps(uint td, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetEventProps(uint ev, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint SetPermissionSetProps(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void DefinePinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetPinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void DeletePinvokeMap(uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineCustomAttribute(uint tkObj, uint tkType, IntPtr pCustomAttribute, uint cbCustomAttribute)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetCustomAttributeValue(uint pcv, IntPtr pCustomAttribute, uint cbCustomAttribute)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineField(uint td, string szName, uint dwFieldFlags, IntPtr pvSigBlob, uint cbSigBlob, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineProperty(uint td, string szProperty, uint dwPropFlags, IntPtr pvSig, uint cbSig, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineParam(uint md, uint ulParamSeq, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetFieldProps(uint fd, uint dwFieldFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetPropertyProps(uint pr, uint dwPropFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetParamProps(uint pd, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint DefineSecurityAttributeSet(uint tkObj, IntPtr rSecAttrs, uint cSecAttrs)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void ApplyEditAndContinue(object pImport)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint TranslateSigWithScope(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport import, IntPtr pbSigBlob, uint cbSigBlob, IntPtr pAssemEmit, IMetaDataEmit emit, IntPtr pvTranslatedSig, uint cbTranslatedSigMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetMethodImplFlags(uint md, uint dwImplFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void SetFieldRVA(uint fd, uint ulRVA)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void Merge(IMetaDataImport pImport, IntPtr pHostMapToken, object pHandler)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void MergeEnd()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void CloseEnum(uint hEnum)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint CountEnum(uint hEnum)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00037AAB File Offset: 0x00035CAB
		public void ResetEnum(uint hEnum, uint ulPos)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumTypeDefs(ref uint phEnum, uint[] rTypeDefs, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumInterfaceImpls(ref uint phEnum, uint td, uint[] rImpls, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumTypeRefs(ref uint phEnum, uint[] rTypeRefs, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint FindTypeDefByName(string szTypeDef, uint tkEnclosingClass)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00037AAB File Offset: 0x00035CAB
		public Guid GetScopeProps(StringBuilder szName, uint cchName, out uint pchName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetModuleFromScope()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00037F8C File Offset: 0x0003618C
		public uint GetTypeDefProps(uint td, IntPtr szTypeDef, uint cchTypeDef, out uint pchTypeDef, IntPtr pdwTypeDefFlags)
		{
			TypeDefinition typeDefinition;
			if (!this.TryGetType(td, out typeDefinition))
			{
				Marshal.WriteInt16(szTypeDef, 0);
				pchTypeDef = 1U;
				return 0U;
			}
			ModuleMetadata.WriteString(typeDefinition.IsNested ? typeDefinition.Name : typeDefinition.FullName, szTypeDef, cchTypeDef, out pchTypeDef);
			ModuleMetadata.WriteIntPtr(pdwTypeDefFlags, (uint)typeDefinition.Attributes);
			if (typeDefinition.BaseType == null)
			{
				return 0U;
			}
			return typeDefinition.BaseType.MetadataToken.ToUInt32();
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00037FFA File Offset: 0x000361FA
		private static void WriteIntPtr(IntPtr ptr, uint value)
		{
			if (ptr == IntPtr.Zero)
			{
				return;
			}
			Marshal.WriteInt32(ptr, (int)value);
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x00038014 File Offset: 0x00036214
		private static void WriteString(string str, IntPtr buffer, uint bufferSize, out uint chars)
		{
			uint num = (((long)(str.Length + 1) >= (long)((ulong)bufferSize)) ? (bufferSize - 1U) : ((uint)str.Length));
			chars = num + 1U;
			int num2 = 0;
			int num3 = 0;
			while ((long)num3 < (long)((ulong)num))
			{
				Marshal.WriteInt16(buffer, num2, str[num3]);
				num2 += 2;
				num3++;
			}
			Marshal.WriteInt16(buffer, num2, 0);
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetInterfaceImplProps(uint iiImpl, out uint pClass)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetTypeRefProps(uint tr, out uint ptkResolutionScope, StringBuilder szName, uint cchName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint ResolveTypeRef(uint tr, ref Guid riid, out object ppIScope)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumMembers(ref uint phEnum, uint cl, uint[] rMembers, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumMembersWithName(ref uint phEnum, uint cl, string szName, uint[] rMembers, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumMethods(ref uint phEnum, uint cl, IntPtr rMethods, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumMethodsWithName(ref uint phEnum, uint cl, string szName, uint[] rMethods, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumFields(ref uint phEnum, uint cl, IntPtr rFields, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumFieldsWithName(ref uint phEnum, uint cl, string szName, uint[] rFields, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumParams(ref uint phEnum, uint mb, uint[] rParams, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumMemberRefs(ref uint phEnum, uint tkParent, uint[] rMemberRefs, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumMethodImpls(ref uint phEnum, uint td, uint[] rMethodBody, uint[] rMethodDecl, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumPermissionSets(ref uint phEnum, uint tk, uint dwActions, uint[] rPermission, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint FindMember(uint td, string szName, byte[] pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint FindMethod(uint td, string szName, byte[] pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint FindField(uint td, string szName, byte[] pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint FindMemberRef(uint td, string szName, byte[] pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0003806C File Offset: 0x0003626C
		public uint GetMethodProps(uint mb, out uint pClass, IntPtr szMethod, uint cchMethod, out uint pchMethod, IntPtr pdwAttr, IntPtr ppvSigBlob, IntPtr pcbSigBlob, IntPtr pulCodeRVA)
		{
			MethodDefinition methodDefinition;
			if (!this.TryGetMethod(mb, out methodDefinition))
			{
				Marshal.WriteInt16(szMethod, 0);
				pchMethod = 1U;
				pClass = 0U;
				return 0U;
			}
			pClass = methodDefinition.DeclaringType.MetadataToken.ToUInt32();
			ModuleMetadata.WriteString(methodDefinition.Name, szMethod, cchMethod, out pchMethod);
			ModuleMetadata.WriteIntPtr(pdwAttr, (uint)methodDefinition.Attributes);
			ModuleMetadata.WriteIntPtr(pulCodeRVA, (uint)methodDefinition.RVA);
			return (uint)methodDefinition.ImplAttributes;
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetMemberRefProps(uint mr, ref uint ptk, StringBuilder szMember, uint cchMember, out uint pchMember, out IntPtr ppvSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumProperties(ref uint phEnum, uint td, IntPtr rProperties, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumEvents(ref uint phEnum, uint td, IntPtr rEvents, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetEventProps(uint ev, out uint pClass, StringBuilder szEvent, uint cchEvent, out uint pchEvent, out uint pdwEventFlags, out uint ptkEventType, out uint pmdAddOn, out uint pmdRemoveOn, out uint pmdFire, uint[] rmdOtherMethod, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumMethodSemantics(ref uint phEnum, uint mb, uint[] rEventProp, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetMethodSemantics(uint mb, uint tkEventProp)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetClassLayout(uint td, out uint pdwPackSize, IntPtr rFieldOffset, uint cMax, out uint pcFieldOffset)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetFieldMarshal(uint tk, out IntPtr ppvNativeType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetRVA(uint tk, out uint pulCodeRVA)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetPermissionSetProps(uint pm, out uint pdwAction, out IntPtr ppvPermission)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetSigFromToken(uint mdSig, out IntPtr ppvSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetModuleRefProps(uint mur, StringBuilder szName, uint cchName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumModuleRefs(ref uint phEnum, uint[] rModuleRefs, uint cmax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetTypeSpecFromToken(uint typespec, out IntPtr ppvSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetNameFromToken(uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumUnresolvedMethods(ref uint phEnum, uint[] rMethods, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetUserString(uint stk, StringBuilder szString, uint cchString)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetPinvokeMap(uint tk, out uint pdwMappingFlags, StringBuilder szImportName, uint cchImportName, out uint pchImportName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumSignatures(ref uint phEnum, uint[] rSignatures, uint cmax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumTypeSpecs(ref uint phEnum, uint[] rTypeSpecs, uint cmax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumUserStrings(ref uint phEnum, uint[] rStrings, uint cmax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00037AAB File Offset: 0x00035CAB
		public int GetParamForMethodIndex(uint md, uint ulParamSeq, out uint pParam)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint EnumCustomAttributes(ref uint phEnum, uint tk, uint tkType, uint[] rCustomAttributes, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetCustomAttributeProps(uint cv, out uint ptkObj, out uint ptkType, out IntPtr ppBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint FindTypeRef(uint tkResolutionScope, string szName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetMemberProps(uint mb, out uint pClass, StringBuilder szMember, uint cchMember, out uint pchMember, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pulCodeRVA, out uint pdwImplFlags, out uint pdwCPlusTypeFlag, out IntPtr ppValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetFieldProps(uint mb, out uint pClass, StringBuilder szField, uint cchField, out uint pchField, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pdwCPlusTypeFlag, out IntPtr ppValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetPropertyProps(uint prop, out uint pClass, StringBuilder szProperty, uint cchProperty, out uint pchProperty, out uint pdwPropFlags, out IntPtr ppvSig, out uint pbSig, out uint pdwCPlusTypeFlag, out IntPtr ppDefaultValue, out uint pcchDefaultValue, out uint pmdSetter, out uint pmdGetter, uint[] rmdOtherMethod, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetParamProps(uint tk, out uint pmd, out uint pulSequence, StringBuilder szName, uint cchName, out uint pchName, out uint pdwAttr, out uint pdwCPlusTypeFlag, out IntPtr ppValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetCustomAttributeByName(uint tkObj, string szName, out IntPtr ppData)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x00037AAB File Offset: 0x00035CAB
		public bool IsValidToken(uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x000380DC File Offset: 0x000362DC
		public uint GetNestedClassProps(uint tdNestedClass)
		{
			TypeDefinition typeDefinition;
			if (!this.TryGetType(tdNestedClass, out typeDefinition))
			{
				return 0U;
			}
			if (!typeDefinition.IsNested)
			{
				return 0U;
			}
			return typeDefinition.DeclaringType.MetadataToken.ToUInt32();
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00037AAB File Offset: 0x00035CAB
		public uint GetNativeCallConvFromSig(IntPtr pvSig, uint cbSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x00037AAB File Offset: 0x00035CAB
		public int IsGlobal(uint pd)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000A10 RID: 2576
		private readonly ModuleDefinition module;

		// Token: 0x04000A11 RID: 2577
		private Dictionary<uint, TypeDefinition> types;

		// Token: 0x04000A12 RID: 2578
		private Dictionary<uint, MethodDefinition> methods;
	}
}
