using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000EA RID: 234
	internal class XmlObjectSerializerWriteContextComplex : XmlObjectSerializerWriteContext
	{
		// Token: 0x06000DA8 RID: 3496 RVA: 0x00039C4F File Offset: 0x00037E4F
		internal XmlObjectSerializerWriteContextComplex(DataContractSerializer serializer, DataContract rootTypeDataContract, DataContractResolver dataContractResolver)
			: base(serializer, rootTypeDataContract, dataContractResolver)
		{
			this.mode = SerializationMode.SharedContract;
			this.preserveObjectReferences = serializer.PreserveObjectReferences;
			this.dataContractSurrogate = serializer.DataContractSurrogate;
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00039C7C File Offset: 0x00037E7C
		internal XmlObjectSerializerWriteContextComplex(NetDataContractSerializer serializer, Hashtable surrogateDataContracts)
			: base(serializer)
		{
			this.mode = SerializationMode.SharedType;
			this.preserveObjectReferences = true;
			this.streamingContext = serializer.Context;
			this.binder = serializer.Binder;
			this.surrogateSelector = serializer.SurrogateSelector;
			this.surrogateDataContracts = surrogateDataContracts;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00039CC9 File Offset: 0x00037EC9
		internal XmlObjectSerializerWriteContextComplex(XmlObjectSerializer serializer, int maxItemsInObjectGraph, StreamingContext streamingContext, bool ignoreExtensionDataObject)
			: base(serializer, maxItemsInObjectGraph, streamingContext, ignoreExtensionDataObject)
		{
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x00039CD6 File Offset: 0x00037ED6
		internal override SerializationMode Mode
		{
			get
			{
				return this.mode;
			}
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00039CE0 File Offset: 0x00037EE0
		internal override DataContract GetDataContract(RuntimeTypeHandle typeHandle, Type type)
		{
			DataContract dataContract = null;
			if (this.mode == SerializationMode.SharedType && this.surrogateSelector != null)
			{
				dataContract = NetDataContractSerializer.GetDataContractFromSurrogateSelector(this.surrogateSelector, this.streamingContext, typeHandle, type, ref this.surrogateDataContracts);
			}
			if (dataContract == null)
			{
				return base.GetDataContract(typeHandle, type);
			}
			if (this.IsGetOnlyCollection && dataContract is SurrogateDataContract)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Found on type '{0}'.", new object[] { DataContract.GetClrTypeFullName(dataContract.UnderlyingType) })));
			}
			return dataContract;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00039D60 File Offset: 0x00037F60
		internal override DataContract GetDataContract(int id, RuntimeTypeHandle typeHandle)
		{
			DataContract dataContract = null;
			if (this.mode == SerializationMode.SharedType && this.surrogateSelector != null)
			{
				dataContract = NetDataContractSerializer.GetDataContractFromSurrogateSelector(this.surrogateSelector, this.streamingContext, typeHandle, null, ref this.surrogateDataContracts);
			}
			if (dataContract == null)
			{
				return base.GetDataContract(id, typeHandle);
			}
			if (this.IsGetOnlyCollection && dataContract is SurrogateDataContract)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Found on type '{0}'.", new object[] { DataContract.GetClrTypeFullName(dataContract.UnderlyingType) })));
			}
			return dataContract;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00039DE0 File Offset: 0x00037FE0
		internal override DataContract GetDataContractSkipValidation(int typeId, RuntimeTypeHandle typeHandle, Type type)
		{
			DataContract dataContract = null;
			if (this.mode == SerializationMode.SharedType && this.surrogateSelector != null)
			{
				dataContract = NetDataContractSerializer.GetDataContractFromSurrogateSelector(this.surrogateSelector, this.streamingContext, typeHandle, null, ref this.surrogateDataContracts);
			}
			if (dataContract == null)
			{
				return base.GetDataContractSkipValidation(typeId, typeHandle, type);
			}
			if (this.IsGetOnlyCollection && dataContract is SurrogateDataContract)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Found on type '{0}'.", new object[] { DataContract.GetClrTypeFullName(dataContract.UnderlyingType) })));
			}
			return dataContract;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00039E61 File Offset: 0x00038061
		internal override bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, DataContract dataContract)
		{
			if (this.mode == SerializationMode.SharedType)
			{
				NetDataContractSerializer.WriteClrTypeInfo(xmlWriter, dataContract, this.binder);
				return true;
			}
			return false;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00039E7C File Offset: 0x0003807C
		internal override bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, Type dataContractType, string clrTypeName, string clrAssemblyName)
		{
			if (this.mode == SerializationMode.SharedType)
			{
				NetDataContractSerializer.WriteClrTypeInfo(xmlWriter, dataContractType, this.binder, clrTypeName, clrAssemblyName);
				return true;
			}
			return false;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00039E9A File Offset: 0x0003809A
		internal override bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, Type dataContractType, SerializationInfo serInfo)
		{
			if (this.mode == SerializationMode.SharedType)
			{
				NetDataContractSerializer.WriteClrTypeInfo(xmlWriter, dataContractType, this.binder, serInfo);
				return true;
			}
			return false;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00039EB6 File Offset: 0x000380B6
		public override void WriteAnyType(XmlWriterDelegator xmlWriter, object value)
		{
			if (!this.OnHandleReference(xmlWriter, value, false))
			{
				xmlWriter.WriteAnyType(value);
			}
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00039ECA File Offset: 0x000380CA
		public override void WriteString(XmlWriterDelegator xmlWriter, string value)
		{
			if (!this.OnHandleReference(xmlWriter, value, false))
			{
				xmlWriter.WriteString(value);
			}
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00039EDE File Offset: 0x000380DE
		public override void WriteString(XmlWriterDelegator xmlWriter, string value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			if (value == null)
			{
				base.WriteNull(xmlWriter, typeof(string), true, name, ns);
				return;
			}
			xmlWriter.WriteStartElementPrimitive(name, ns);
			if (!this.OnHandleReference(xmlWriter, value, false))
			{
				xmlWriter.WriteString(value);
			}
			xmlWriter.WriteEndElementPrimitive();
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00039F1A File Offset: 0x0003811A
		public override void WriteBase64(XmlWriterDelegator xmlWriter, byte[] value)
		{
			if (!this.OnHandleReference(xmlWriter, value, false))
			{
				xmlWriter.WriteBase64(value);
			}
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00039F2E File Offset: 0x0003812E
		public override void WriteBase64(XmlWriterDelegator xmlWriter, byte[] value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			if (value == null)
			{
				base.WriteNull(xmlWriter, typeof(byte[]), true, name, ns);
				return;
			}
			xmlWriter.WriteStartElementPrimitive(name, ns);
			if (!this.OnHandleReference(xmlWriter, value, false))
			{
				xmlWriter.WriteBase64(value);
			}
			xmlWriter.WriteEndElementPrimitive();
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00039F6A File Offset: 0x0003816A
		public override void WriteUri(XmlWriterDelegator xmlWriter, Uri value)
		{
			if (!this.OnHandleReference(xmlWriter, value, false))
			{
				xmlWriter.WriteUri(value);
			}
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00039F80 File Offset: 0x00038180
		public override void WriteUri(XmlWriterDelegator xmlWriter, Uri value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			if (value == null)
			{
				base.WriteNull(xmlWriter, typeof(Uri), true, name, ns);
				return;
			}
			xmlWriter.WriteStartElementPrimitive(name, ns);
			if (!this.OnHandleReference(xmlWriter, value, false))
			{
				xmlWriter.WriteUri(value);
			}
			xmlWriter.WriteEndElementPrimitive();
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00039FCD File Offset: 0x000381CD
		public override void WriteQName(XmlWriterDelegator xmlWriter, XmlQualifiedName value)
		{
			if (!this.OnHandleReference(xmlWriter, value, false))
			{
				xmlWriter.WriteQName(value);
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00039FE4 File Offset: 0x000381E4
		public override void WriteQName(XmlWriterDelegator xmlWriter, XmlQualifiedName value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			if (value == null)
			{
				base.WriteNull(xmlWriter, typeof(XmlQualifiedName), true, name, ns);
				return;
			}
			if (ns != null && ns.Value != null && ns.Value.Length > 0)
			{
				xmlWriter.WriteStartElement("q", name, ns);
			}
			else
			{
				xmlWriter.WriteStartElement(name, ns);
			}
			if (!this.OnHandleReference(xmlWriter, value, false))
			{
				xmlWriter.WriteQName(value);
			}
			xmlWriter.WriteEndElement();
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0003A05D File Offset: 0x0003825D
		public override void InternalSerialize(XmlWriterDelegator xmlWriter, object obj, bool isDeclaredType, bool writeXsiType, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle)
		{
			if (this.dataContractSurrogate == null)
			{
				base.InternalSerialize(xmlWriter, obj, isDeclaredType, writeXsiType, declaredTypeID, declaredTypeHandle);
				return;
			}
			this.InternalSerializeWithSurrogate(xmlWriter, obj, isDeclaredType, writeXsiType, declaredTypeID, declaredTypeHandle);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0003A088 File Offset: 0x00038288
		internal override bool OnHandleReference(XmlWriterDelegator xmlWriter, object obj, bool canContainCyclicReference)
		{
			if (this.preserveObjectReferences && !this.IsGetOnlyCollection)
			{
				bool flag = true;
				int id = base.SerializedObjects.GetId(obj, ref flag);
				if (flag)
				{
					xmlWriter.WriteAttributeInt("z", DictionaryGlobals.IdLocalName, DictionaryGlobals.SerializationNamespace, id);
				}
				else
				{
					xmlWriter.WriteAttributeInt("z", DictionaryGlobals.RefLocalName, DictionaryGlobals.SerializationNamespace, id);
					xmlWriter.WriteAttributeBool("i", DictionaryGlobals.XsiNilLocalName, DictionaryGlobals.SchemaInstanceNamespace, true);
				}
				return !flag;
			}
			return base.OnHandleReference(xmlWriter, obj, canContainCyclicReference);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0003A10B File Offset: 0x0003830B
		internal override void OnEndHandleReference(XmlWriterDelegator xmlWriter, object obj, bool canContainCyclicReference)
		{
			if (this.preserveObjectReferences && !this.IsGetOnlyCollection)
			{
				return;
			}
			base.OnEndHandleReference(xmlWriter, obj, canContainCyclicReference);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0003A128 File Offset: 0x00038328
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool CheckIfTypeSerializableForSharedTypeMode(Type memberType)
		{
			ISurrogateSelector surrogateSelector;
			return this.surrogateSelector.GetSurrogate(memberType, this.streamingContext, out surrogateSelector) != null;
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0003A14C File Offset: 0x0003834C
		internal override void CheckIfTypeSerializable(Type memberType, bool isMemberTypeSerializable)
		{
			if (this.mode == SerializationMode.SharedType && this.surrogateSelector != null && this.CheckIfTypeSerializableForSharedTypeMode(memberType))
			{
				return;
			}
			if (this.dataContractSurrogate == null)
			{
				base.CheckIfTypeSerializable(memberType, isMemberTypeSerializable);
				return;
			}
			while (memberType.IsArray)
			{
				memberType = memberType.GetElementType();
			}
			memberType = DataContractSurrogateCaller.GetDataContractType(this.dataContractSurrogate, memberType);
			if (!DataContract.IsTypeSerializable(memberType))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot be serialized. Consider marking it with the DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute. Alternatively, you can ensure that the type is public and has a parameterless constructor - all public members of the type will then be serialized, and no attributes will be required.", new object[] { memberType })));
			}
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0003A1CC File Offset: 0x000383CC
		internal override Type GetSurrogatedType(Type type)
		{
			if (this.dataContractSurrogate == null)
			{
				return base.GetSurrogatedType(type);
			}
			type = DataContract.UnwrapNullableType(type);
			Type surrogatedType = DataContractSerializer.GetSurrogatedType(this.dataContractSurrogate, type);
			if (this.IsGetOnlyCollection && surrogatedType != type)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Found on type '{0}'.", new object[] { DataContract.GetClrTypeFullName(type) })));
			}
			return surrogatedType;
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0003A234 File Offset: 0x00038434
		private void InternalSerializeWithSurrogate(XmlWriterDelegator xmlWriter, object obj, bool isDeclaredType, bool writeXsiType, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle)
		{
			RuntimeTypeHandle runtimeTypeHandle = (isDeclaredType ? declaredTypeHandle : Type.GetTypeHandle(obj));
			object obj2 = obj;
			int num = 0;
			Type typeFromHandle = Type.GetTypeFromHandle(runtimeTypeHandle);
			Type type = this.GetSurrogatedType(Type.GetTypeFromHandle(declaredTypeHandle));
			if (TD.DCSerializeWithSurrogateStartIsEnabled())
			{
				TD.DCSerializeWithSurrogateStart(type.FullName);
			}
			declaredTypeHandle = type.TypeHandle;
			obj = DataContractSerializer.SurrogateToDataContractType(this.dataContractSurrogate, obj, type, ref typeFromHandle);
			runtimeTypeHandle = typeFromHandle.TypeHandle;
			if (obj2 != obj)
			{
				num = base.SerializedObjects.ReassignId(0, obj2, obj);
			}
			if (writeXsiType)
			{
				type = Globals.TypeOfObject;
				this.SerializeWithXsiType(xmlWriter, obj, runtimeTypeHandle, typeFromHandle, -1, type.TypeHandle, type);
			}
			else if (declaredTypeHandle.Equals(runtimeTypeHandle))
			{
				DataContract dataContract = this.GetDataContract(runtimeTypeHandle, typeFromHandle);
				base.SerializeWithoutXsiType(dataContract, xmlWriter, obj, declaredTypeHandle);
			}
			else
			{
				this.SerializeWithXsiType(xmlWriter, obj, runtimeTypeHandle, typeFromHandle, -1, declaredTypeHandle, type);
			}
			if (obj2 != obj)
			{
				base.SerializedObjects.ReassignId(num, obj, obj2);
			}
			if (TD.DCSerializeWithSurrogateStopIsEnabled())
			{
				TD.DCSerializeWithSurrogateStop();
			}
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0003A322 File Offset: 0x00038522
		internal override void WriteArraySize(XmlWriterDelegator xmlWriter, int size)
		{
			if (this.preserveObjectReferences && size > -1)
			{
				xmlWriter.WriteAttributeInt("z", DictionaryGlobals.ArraySizeLocalName, DictionaryGlobals.SerializationNamespace, size);
			}
		}

		// Token: 0x04000576 RID: 1398
		protected IDataContractSurrogate dataContractSurrogate;

		// Token: 0x04000577 RID: 1399
		private SerializationMode mode;

		// Token: 0x04000578 RID: 1400
		private SerializationBinder binder;

		// Token: 0x04000579 RID: 1401
		private ISurrogateSelector surrogateSelector;

		// Token: 0x0400057A RID: 1402
		private StreamingContext streamingContext;

		// Token: 0x0400057B RID: 1403
		private Hashtable surrogateDataContracts;
	}
}
