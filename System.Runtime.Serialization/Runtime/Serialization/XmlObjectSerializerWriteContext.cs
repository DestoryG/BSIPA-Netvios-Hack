using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Diagnostics;
using System.Security;
using System.Xml;
using System.Xml.Serialization;

namespace System.Runtime.Serialization
{
	// Token: 0x020000E9 RID: 233
	internal class XmlObjectSerializerWriteContext : XmlObjectSerializerContext
	{
		// Token: 0x06000D66 RID: 3430 RVA: 0x00038CE0 File Offset: 0x00036EE0
		internal static XmlObjectSerializerWriteContext CreateContext(DataContractSerializer serializer, DataContract rootTypeDataContract, DataContractResolver dataContractResolver)
		{
			if (!serializer.PreserveObjectReferences && serializer.DataContractSurrogate == null)
			{
				return new XmlObjectSerializerWriteContext(serializer, rootTypeDataContract, dataContractResolver);
			}
			return new XmlObjectSerializerWriteContextComplex(serializer, rootTypeDataContract, dataContractResolver);
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00038D03 File Offset: 0x00036F03
		internal static XmlObjectSerializerWriteContext CreateContext(NetDataContractSerializer serializer, Hashtable surrogateDataContracts)
		{
			return new XmlObjectSerializerWriteContextComplex(serializer, surrogateDataContracts);
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x00038D0C File Offset: 0x00036F0C
		protected XmlObjectSerializerWriteContext(DataContractSerializer serializer, DataContract rootTypeDataContract, DataContractResolver resolver)
			: base(serializer, rootTypeDataContract, resolver)
		{
			this.serializeReadOnlyTypes = serializer.SerializeReadOnlyTypes;
			this.unsafeTypeForwardingEnabled = true;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00038D2A File Offset: 0x00036F2A
		protected XmlObjectSerializerWriteContext(NetDataContractSerializer serializer)
			: base(serializer)
		{
			this.unsafeTypeForwardingEnabled = NetDataContractSerializer.UnsafeTypeForwardingEnabled;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00038D3E File Offset: 0x00036F3E
		internal XmlObjectSerializerWriteContext(XmlObjectSerializer serializer, int maxItemsInObjectGraph, StreamingContext streamingContext, bool ignoreExtensionDataObject)
			: base(serializer, maxItemsInObjectGraph, streamingContext, ignoreExtensionDataObject)
		{
			this.unsafeTypeForwardingEnabled = true;
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x00038D52 File Offset: 0x00036F52
		protected ObjectToIdCache SerializedObjects
		{
			get
			{
				if (this.serializedObjects == null)
				{
					this.serializedObjects = new ObjectToIdCache();
				}
				return this.serializedObjects;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x00038D6D File Offset: 0x00036F6D
		// (set) Token: 0x06000D6D RID: 3437 RVA: 0x00038D75 File Offset: 0x00036F75
		internal override bool IsGetOnlyCollection
		{
			get
			{
				return this.isGetOnlyCollection;
			}
			set
			{
				this.isGetOnlyCollection = value;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x00038D7E File Offset: 0x00036F7E
		internal bool SerializeReadOnlyTypes
		{
			get
			{
				return this.serializeReadOnlyTypes;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000D6F RID: 3439 RVA: 0x00038D86 File Offset: 0x00036F86
		internal bool UnsafeTypeForwardingEnabled
		{
			get
			{
				return this.unsafeTypeForwardingEnabled;
			}
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00038D8E File Offset: 0x00036F8E
		internal void StoreIsGetOnlyCollection()
		{
			this.isGetOnlyCollection = true;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x00038D97 File Offset: 0x00036F97
		public void InternalSerializeReference(XmlWriterDelegator xmlWriter, object obj, bool isDeclaredType, bool writeXsiType, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle)
		{
			if (!this.OnHandleReference(xmlWriter, obj, true))
			{
				this.InternalSerialize(xmlWriter, obj, isDeclaredType, writeXsiType, declaredTypeID, declaredTypeHandle);
			}
			this.OnEndHandleReference(xmlWriter, obj, true);
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00038DBC File Offset: 0x00036FBC
		public virtual void InternalSerialize(XmlWriterDelegator xmlWriter, object obj, bool isDeclaredType, bool writeXsiType, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle)
		{
			if (writeXsiType)
			{
				Type typeOfObject = Globals.TypeOfObject;
				this.SerializeWithXsiType(xmlWriter, obj, Type.GetTypeHandle(obj), null, -1, typeOfObject.TypeHandle, typeOfObject);
				return;
			}
			if (isDeclaredType)
			{
				DataContract dataContract = this.GetDataContract(declaredTypeID, declaredTypeHandle);
				this.SerializeWithoutXsiType(dataContract, xmlWriter, obj, declaredTypeHandle);
				return;
			}
			RuntimeTypeHandle typeHandle = Type.GetTypeHandle(obj);
			if (declaredTypeHandle.Equals(typeHandle))
			{
				DataContract dataContract2 = ((declaredTypeID >= 0) ? this.GetDataContract(declaredTypeID, declaredTypeHandle) : this.GetDataContract(declaredTypeHandle, null));
				this.SerializeWithoutXsiType(dataContract2, xmlWriter, obj, declaredTypeHandle);
				return;
			}
			this.SerializeWithXsiType(xmlWriter, obj, typeHandle, null, declaredTypeID, declaredTypeHandle, Type.GetTypeFromHandle(declaredTypeHandle));
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00038E54 File Offset: 0x00037054
		internal void SerializeWithoutXsiType(DataContract dataContract, XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle declaredTypeHandle)
		{
			if (this.OnHandleIsReference(xmlWriter, dataContract, obj))
			{
				return;
			}
			if (dataContract.KnownDataContracts != null)
			{
				this.scopedKnownTypes.Push(dataContract.KnownDataContracts);
				this.WriteDataContractValue(dataContract, xmlWriter, obj, declaredTypeHandle);
				this.scopedKnownTypes.Pop();
				return;
			}
			this.WriteDataContractValue(dataContract, xmlWriter, obj, declaredTypeHandle);
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00038EA8 File Offset: 0x000370A8
		internal virtual void SerializeWithXsiTypeAtTopLevel(DataContract dataContract, XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle originalDeclaredTypeHandle, Type graphType)
		{
			bool flag = false;
			Type originalUnderlyingType = this.rootTypeDataContract.OriginalUnderlyingType;
			if (originalUnderlyingType.IsInterface && CollectionDataContract.IsCollectionInterface(originalUnderlyingType))
			{
				if (base.DataContractResolver != null)
				{
					this.WriteResolvedTypeInfo(xmlWriter, graphType, originalUnderlyingType);
				}
			}
			else if (!originalUnderlyingType.IsArray)
			{
				flag = this.WriteTypeInfo(xmlWriter, dataContract, this.rootTypeDataContract);
			}
			this.SerializeAndVerifyType(dataContract, xmlWriter, obj, flag, originalDeclaredTypeHandle, originalUnderlyingType);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00038F0C File Offset: 0x0003710C
		protected virtual void SerializeWithXsiType(XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle objectTypeHandle, Type objectType, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle, Type declaredType)
		{
			bool flag = false;
			DataContract dataContract;
			if (declaredType.IsInterface && CollectionDataContract.IsCollectionInterface(declaredType))
			{
				dataContract = this.GetDataContractSkipValidation(DataContract.GetId(objectTypeHandle), objectTypeHandle, objectType);
				if (this.OnHandleIsReference(xmlWriter, dataContract, obj))
				{
					return;
				}
				if (this.Mode == SerializationMode.SharedType && dataContract.IsValidContract(this.Mode))
				{
					dataContract = dataContract.GetValidContract(this.Mode);
				}
				else
				{
					dataContract = this.GetDataContract(declaredTypeHandle, declaredType);
				}
				if (!this.WriteClrTypeInfo(xmlWriter, dataContract) && base.DataContractResolver != null)
				{
					if (objectType == null)
					{
						objectType = Type.GetTypeFromHandle(objectTypeHandle);
					}
					this.WriteResolvedTypeInfo(xmlWriter, objectType, declaredType);
				}
			}
			else if (declaredType.IsArray)
			{
				dataContract = this.GetDataContract(objectTypeHandle, objectType);
				this.WriteClrTypeInfo(xmlWriter, dataContract);
				dataContract = this.GetDataContract(declaredTypeHandle, declaredType);
			}
			else
			{
				dataContract = this.GetDataContract(objectTypeHandle, objectType);
				if (this.OnHandleIsReference(xmlWriter, dataContract, obj))
				{
					return;
				}
				if (!this.WriteClrTypeInfo(xmlWriter, dataContract))
				{
					DataContract dataContract2 = ((declaredTypeID >= 0) ? this.GetDataContract(declaredTypeID, declaredTypeHandle) : this.GetDataContract(declaredTypeHandle, declaredType));
					flag = this.WriteTypeInfo(xmlWriter, dataContract, dataContract2);
				}
			}
			this.SerializeAndVerifyType(dataContract, xmlWriter, obj, flag, declaredTypeHandle, declaredType);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00039034 File Offset: 0x00037234
		internal bool OnHandleIsReference(XmlWriterDelegator xmlWriter, DataContract contract, object obj)
		{
			if (this.preserveObjectReferences || !contract.IsReference || this.isGetOnlyCollection)
			{
				return false;
			}
			bool flag = true;
			int id = this.SerializedObjects.GetId(obj, ref flag);
			this.byValObjectsInScope.EnsureSetAsIsReference(obj);
			if (flag)
			{
				xmlWriter.WriteAttributeString("z", DictionaryGlobals.IdLocalName, DictionaryGlobals.SerializationNamespace, string.Format(CultureInfo.InvariantCulture, "{0}{1}", "i", id));
				return false;
			}
			xmlWriter.WriteAttributeString("z", DictionaryGlobals.RefLocalName, DictionaryGlobals.SerializationNamespace, string.Format(CultureInfo.InvariantCulture, "{0}{1}", "i", id));
			return true;
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000390DC File Offset: 0x000372DC
		protected void SerializeAndVerifyType(DataContract dataContract, XmlWriterDelegator xmlWriter, object obj, bool verifyKnownType, RuntimeTypeHandle declaredTypeHandle, Type declaredType)
		{
			bool flag = false;
			if (dataContract.KnownDataContracts != null)
			{
				this.scopedKnownTypes.Push(dataContract.KnownDataContracts);
				flag = true;
			}
			if (verifyKnownType && !base.IsKnownType(dataContract, declaredType))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Type '{0}' with data contract name '{1}:{2}' is not expected. Add any types not known statically to the list of known types - for example, by using the KnownTypeAttribute attribute or by adding them to the list of known types passed to DataContractSerializer.", new object[]
				{
					DataContract.GetClrTypeFullName(dataContract.UnderlyingType),
					dataContract.StableName.Name,
					dataContract.StableName.Namespace
				})));
			}
			this.WriteDataContractValue(dataContract, xmlWriter, obj, declaredTypeHandle);
			if (flag)
			{
				this.scopedKnownTypes.Pop();
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00039173 File Offset: 0x00037373
		internal virtual bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, DataContract dataContract)
		{
			return false;
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00039176 File Offset: 0x00037376
		internal virtual bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, Type dataContractType, string clrTypeName, string clrAssemblyName)
		{
			return false;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00039179 File Offset: 0x00037379
		internal virtual bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, Type dataContractType, SerializationInfo serInfo)
		{
			return false;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0003917C File Offset: 0x0003737C
		public virtual void WriteAnyType(XmlWriterDelegator xmlWriter, object value)
		{
			xmlWriter.WriteAnyType(value);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00039185 File Offset: 0x00037385
		public virtual void WriteString(XmlWriterDelegator xmlWriter, string value)
		{
			xmlWriter.WriteString(value);
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0003918E File Offset: 0x0003738E
		public virtual void WriteString(XmlWriterDelegator xmlWriter, string value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			if (value == null)
			{
				this.WriteNull(xmlWriter, typeof(string), true, name, ns);
				return;
			}
			xmlWriter.WriteStartElementPrimitive(name, ns);
			xmlWriter.WriteString(value);
			xmlWriter.WriteEndElementPrimitive();
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x000391BF File Offset: 0x000373BF
		public virtual void WriteBase64(XmlWriterDelegator xmlWriter, byte[] value)
		{
			xmlWriter.WriteBase64(value);
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x000391C8 File Offset: 0x000373C8
		public virtual void WriteBase64(XmlWriterDelegator xmlWriter, byte[] value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			if (value == null)
			{
				this.WriteNull(xmlWriter, typeof(byte[]), true, name, ns);
				return;
			}
			xmlWriter.WriteStartElementPrimitive(name, ns);
			xmlWriter.WriteBase64(value);
			xmlWriter.WriteEndElementPrimitive();
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x000391F9 File Offset: 0x000373F9
		public virtual void WriteUri(XmlWriterDelegator xmlWriter, Uri value)
		{
			xmlWriter.WriteUri(value);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00039202 File Offset: 0x00037402
		public virtual void WriteUri(XmlWriterDelegator xmlWriter, Uri value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			if (value == null)
			{
				this.WriteNull(xmlWriter, typeof(Uri), true, name, ns);
				return;
			}
			xmlWriter.WriteStartElementPrimitive(name, ns);
			xmlWriter.WriteUri(value);
			xmlWriter.WriteEndElementPrimitive();
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00039239 File Offset: 0x00037439
		public virtual void WriteQName(XmlWriterDelegator xmlWriter, XmlQualifiedName value)
		{
			xmlWriter.WriteQName(value);
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00039244 File Offset: 0x00037444
		public virtual void WriteQName(XmlWriterDelegator xmlWriter, XmlQualifiedName value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			if (value == null)
			{
				this.WriteNull(xmlWriter, typeof(XmlQualifiedName), true, name, ns);
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
			xmlWriter.WriteQName(value);
			xmlWriter.WriteEndElement();
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x000392B2 File Offset: 0x000374B2
		internal void HandleGraphAtTopLevel(XmlWriterDelegator writer, object obj, DataContract contract)
		{
			writer.WriteXmlnsAttribute("i", DictionaryGlobals.SchemaInstanceNamespace);
			if (contract.IsISerializable)
			{
				writer.WriteXmlnsAttribute("x", DictionaryGlobals.SchemaNamespace);
			}
			this.OnHandleReference(writer, obj, true);
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x000392E8 File Offset: 0x000374E8
		internal virtual bool OnHandleReference(XmlWriterDelegator xmlWriter, object obj, bool canContainCyclicReference)
		{
			if (xmlWriter.depth < 512)
			{
				return false;
			}
			if (canContainCyclicReference)
			{
				if (this.byValObjectsInScope.Count == 0 && DiagnosticUtility.ShouldTraceWarning)
				{
					TraceUtility.Trace(TraceEventType.Warning, 196626, SR.GetString("Object with large depth"));
				}
				if (this.byValObjectsInScope.Contains(obj))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Object graph for type '{0}' contains cycles and cannot be serialized if references are not tracked. Consider using the DataContractAttribute with the IsReference property set to true.", new object[] { DataContract.GetClrTypeFullName(obj.GetType()) })));
				}
				this.byValObjectsInScope.Push(obj);
			}
			return false;
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00039374 File Offset: 0x00037574
		internal virtual void OnEndHandleReference(XmlWriterDelegator xmlWriter, object obj, bool canContainCyclicReference)
		{
			if (xmlWriter.depth < 512)
			{
				return;
			}
			if (canContainCyclicReference)
			{
				this.byValObjectsInScope.Pop(obj);
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00039393 File Offset: 0x00037593
		public void WriteNull(XmlWriterDelegator xmlWriter, Type memberType, bool isMemberTypeSerializable)
		{
			this.CheckIfTypeSerializable(memberType, isMemberTypeSerializable);
			this.WriteNull(xmlWriter);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x000393A4 File Offset: 0x000375A4
		internal void WriteNull(XmlWriterDelegator xmlWriter, Type memberType, bool isMemberTypeSerializable, XmlDictionaryString name, XmlDictionaryString ns)
		{
			xmlWriter.WriteStartElement(name, ns);
			this.WriteNull(xmlWriter, memberType, isMemberTypeSerializable);
			xmlWriter.WriteEndElement();
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x000393BF File Offset: 0x000375BF
		public void IncrementArrayCount(XmlWriterDelegator xmlWriter, Array array)
		{
			this.IncrementCollectionCount(xmlWriter, array.GetLength(0));
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x000393CF File Offset: 0x000375CF
		public void IncrementCollectionCount(XmlWriterDelegator xmlWriter, ICollection collection)
		{
			this.IncrementCollectionCount(xmlWriter, collection.Count);
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x000393DE File Offset: 0x000375DE
		public void IncrementCollectionCountGeneric<T>(XmlWriterDelegator xmlWriter, ICollection<T> collection)
		{
			this.IncrementCollectionCount(xmlWriter, collection.Count);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x000393ED File Offset: 0x000375ED
		private void IncrementCollectionCount(XmlWriterDelegator xmlWriter, int size)
		{
			base.IncrementItemCount(size);
			this.WriteArraySize(xmlWriter, size);
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x000393FE File Offset: 0x000375FE
		internal virtual void WriteArraySize(XmlWriterDelegator xmlWriter, int size)
		{
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00039400 File Offset: 0x00037600
		public static T GetDefaultValue<T>()
		{
			return default(T);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00039416 File Offset: 0x00037616
		public static T GetNullableValue<T>(T? value) where T : struct
		{
			return value.Value;
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0003941F File Offset: 0x0003761F
		public static void ThrowRequiredMemberMustBeEmitted(string memberName, Type type)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("Member {0} in type {1} cannot be serialized. This exception is usually caused by trying to use a null value where a null value is not allowed. The '{0}' member is set to its default value (usually null or zero). The member's EmitDefault setting is 'false', indicating that the member should not be serialized. However, the member's IsRequired setting is 'true', indicating that it must be serialized. This conflict cannot be resolved.  Consider setting '{0}' to a non-default value. Alternatively, you can change the EmitDefaultValue property on the DataMemberAttribute attribute to true, or changing the IsRequired property to false.", new object[] { memberName, type.FullName })));
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00039448 File Offset: 0x00037648
		public static bool GetHasValue<T>(T? value) where T : struct
		{
			return value != null;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00039451 File Offset: 0x00037651
		internal void WriteIXmlSerializable(XmlWriterDelegator xmlWriter, object obj)
		{
			if (this.xmlSerializableWriter == null)
			{
				this.xmlSerializableWriter = new XmlSerializableWriter();
			}
			XmlObjectSerializerWriteContext.WriteIXmlSerializable(xmlWriter, obj, this.xmlSerializableWriter);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00039473 File Offset: 0x00037673
		internal static void WriteRootIXmlSerializable(XmlWriterDelegator xmlWriter, object obj)
		{
			XmlObjectSerializerWriteContext.WriteIXmlSerializable(xmlWriter, obj, new XmlSerializableWriter());
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00039484 File Offset: 0x00037684
		private static void WriteIXmlSerializable(XmlWriterDelegator xmlWriter, object obj, XmlSerializableWriter xmlSerializableWriter)
		{
			xmlSerializableWriter.BeginWrite(xmlWriter.Writer, obj);
			IXmlSerializable xmlSerializable = obj as IXmlSerializable;
			if (xmlSerializable != null)
			{
				xmlSerializable.WriteXml(xmlSerializableWriter);
			}
			else
			{
				XmlElement xmlElement = obj as XmlElement;
				if (xmlElement != null)
				{
					xmlElement.WriteTo(xmlSerializableWriter);
				}
				else
				{
					XmlNode[] array = obj as XmlNode[];
					if (array == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Unknown XML type: '{0}'.", new object[] { DataContract.GetClrTypeFullName(obj.GetType()) })));
					}
					XmlNode[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i].WriteTo(xmlSerializableWriter);
					}
				}
			}
			xmlSerializableWriter.EndWrite();
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0003951D File Offset: 0x0003771D
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void GetObjectData(ISerializable obj, SerializationInfo serInfo, StreamingContext context)
		{
			obj.GetObjectData(serInfo, context);
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00039528 File Offset: 0x00037728
		public void WriteISerializable(XmlWriterDelegator xmlWriter, ISerializable obj)
		{
			Type type = obj.GetType();
			SerializationInfo serializationInfo = new SerializationInfo(type, XmlObjectSerializer.FormatterConverter, !this.UnsafeTypeForwardingEnabled);
			this.GetObjectData(obj, serializationInfo, base.GetStreamingContext());
			if (!this.UnsafeTypeForwardingEnabled && serializationInfo.AssemblyName == "0")
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("ISerializable AssemblyName is set to \"0\" for type '{0}'.", new object[] { DataContract.GetClrTypeFullName(obj.GetType()) })));
			}
			this.WriteSerializationInfo(xmlWriter, type, serializationInfo);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x000395AC File Offset: 0x000377AC
		internal void WriteSerializationInfo(XmlWriterDelegator xmlWriter, Type objType, SerializationInfo serInfo)
		{
			if (DataContract.GetClrTypeFullName(objType) != serInfo.FullTypeName)
			{
				if (base.DataContractResolver != null)
				{
					XmlDictionaryString xmlDictionaryString;
					XmlDictionaryString xmlDictionaryString2;
					if (this.ResolveType(serInfo.ObjectType, objType, out xmlDictionaryString, out xmlDictionaryString2))
					{
						xmlWriter.WriteAttributeQualifiedName("z", DictionaryGlobals.ISerializableFactoryTypeLocalName, DictionaryGlobals.SerializationNamespace, xmlDictionaryString, xmlDictionaryString2);
					}
				}
				else
				{
					string text;
					string text2;
					DataContract.GetDefaultStableName(serInfo.FullTypeName, out text, out text2);
					xmlWriter.WriteAttributeQualifiedName("z", DictionaryGlobals.ISerializableFactoryTypeLocalName, DictionaryGlobals.SerializationNamespace, DataContract.GetClrTypeString(text), DataContract.GetClrTypeString(text2));
				}
			}
			this.WriteClrTypeInfo(xmlWriter, objType, serInfo);
			base.IncrementItemCount(serInfo.MemberCount);
			foreach (SerializationEntry serializationEntry in serInfo)
			{
				XmlDictionaryString clrTypeString = DataContract.GetClrTypeString(DataContract.EncodeLocalName(serializationEntry.Name));
				xmlWriter.WriteStartElement(clrTypeString, DictionaryGlobals.EmptyString);
				object value = serializationEntry.Value;
				if (value == null)
				{
					this.WriteNull(xmlWriter);
				}
				else
				{
					this.InternalSerializeReference(xmlWriter, value, false, false, -1, Globals.TypeOfObject.TypeHandle);
				}
				xmlWriter.WriteEndElement();
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x000396B4 File Offset: 0x000378B4
		public void WriteExtensionData(XmlWriterDelegator xmlWriter, ExtensionDataObject extensionData, int memberIndex)
		{
			if (base.IgnoreExtensionDataObject || extensionData == null)
			{
				return;
			}
			if (extensionData.Members != null)
			{
				for (int i = 0; i < extensionData.Members.Count; i++)
				{
					ExtensionDataMember extensionDataMember = extensionData.Members[i];
					if (extensionDataMember.MemberIndex == memberIndex)
					{
						this.WriteExtensionDataMember(xmlWriter, extensionDataMember);
					}
				}
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0003970C File Offset: 0x0003790C
		private void WriteExtensionDataMember(XmlWriterDelegator xmlWriter, ExtensionDataMember member)
		{
			xmlWriter.WriteStartElement(member.Name, member.Namespace);
			IDataNode value = member.Value;
			this.WriteExtensionDataValue(xmlWriter, value);
			xmlWriter.WriteEndElement();
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00039740 File Offset: 0x00037940
		internal virtual void WriteExtensionDataTypeInfo(XmlWriterDelegator xmlWriter, IDataNode dataNode)
		{
			if (dataNode.DataContractName != null)
			{
				this.WriteTypeInfo(xmlWriter, dataNode.DataContractName, dataNode.DataContractNamespace);
			}
			this.WriteClrTypeInfo(xmlWriter, dataNode.DataType, dataNode.ClrTypeName, dataNode.ClrAssemblyName);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00039778 File Offset: 0x00037978
		internal void WriteExtensionDataValue(XmlWriterDelegator xmlWriter, IDataNode dataNode)
		{
			base.IncrementItemCount(1);
			if (dataNode == null)
			{
				this.WriteNull(xmlWriter);
				return;
			}
			if (dataNode.PreservesReferences && this.OnHandleReference(xmlWriter, (dataNode.Value == null) ? dataNode : dataNode.Value, true))
			{
				return;
			}
			Type dataType = dataNode.DataType;
			if (dataType == Globals.TypeOfClassDataNode)
			{
				this.WriteExtensionClassData(xmlWriter, (ClassDataNode)dataNode);
			}
			else if (dataType == Globals.TypeOfCollectionDataNode)
			{
				this.WriteExtensionCollectionData(xmlWriter, (CollectionDataNode)dataNode);
			}
			else if (dataType == Globals.TypeOfXmlDataNode)
			{
				this.WriteExtensionXmlData(xmlWriter, (XmlDataNode)dataNode);
			}
			else if (dataType == Globals.TypeOfISerializableDataNode)
			{
				this.WriteExtensionISerializableData(xmlWriter, (ISerializableDataNode)dataNode);
			}
			else
			{
				this.WriteExtensionDataTypeInfo(xmlWriter, dataNode);
				if (dataType == Globals.TypeOfObject)
				{
					object value = dataNode.Value;
					if (value != null)
					{
						this.InternalSerialize(xmlWriter, value, false, false, -1, value.GetType().TypeHandle);
					}
				}
				else
				{
					xmlWriter.WriteExtensionData(dataNode);
				}
			}
			if (dataNode.PreservesReferences)
			{
				this.OnEndHandleReference(xmlWriter, (dataNode.Value == null) ? dataNode : dataNode.Value, true);
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00039894 File Offset: 0x00037A94
		internal bool TryWriteDeserializedExtensionData(XmlWriterDelegator xmlWriter, IDataNode dataNode)
		{
			object value = dataNode.Value;
			if (value == null)
			{
				return false;
			}
			Type type = ((dataNode.DataContractName == null) ? value.GetType() : Globals.TypeOfObject);
			this.InternalSerialize(xmlWriter, value, false, false, -1, type.TypeHandle);
			return true;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x000398D8 File Offset: 0x00037AD8
		private void WriteExtensionClassData(XmlWriterDelegator xmlWriter, ClassDataNode dataNode)
		{
			if (!this.TryWriteDeserializedExtensionData(xmlWriter, dataNode))
			{
				this.WriteExtensionDataTypeInfo(xmlWriter, dataNode);
				IList<ExtensionDataMember> members = dataNode.Members;
				if (members != null)
				{
					for (int i = 0; i < members.Count; i++)
					{
						this.WriteExtensionDataMember(xmlWriter, members[i]);
					}
				}
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00039920 File Offset: 0x00037B20
		private void WriteExtensionCollectionData(XmlWriterDelegator xmlWriter, CollectionDataNode dataNode)
		{
			if (!this.TryWriteDeserializedExtensionData(xmlWriter, dataNode))
			{
				this.WriteExtensionDataTypeInfo(xmlWriter, dataNode);
				this.WriteArraySize(xmlWriter, dataNode.Size);
				IList<IDataNode> items = dataNode.Items;
				if (items != null)
				{
					for (int i = 0; i < items.Count; i++)
					{
						xmlWriter.WriteStartElement(dataNode.ItemName, dataNode.ItemNamespace);
						this.WriteExtensionDataValue(xmlWriter, items[i]);
						xmlWriter.WriteEndElement();
					}
				}
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00039990 File Offset: 0x00037B90
		private void WriteExtensionISerializableData(XmlWriterDelegator xmlWriter, ISerializableDataNode dataNode)
		{
			if (!this.TryWriteDeserializedExtensionData(xmlWriter, dataNode))
			{
				this.WriteExtensionDataTypeInfo(xmlWriter, dataNode);
				if (dataNode.FactoryTypeName != null)
				{
					xmlWriter.WriteAttributeQualifiedName("z", DictionaryGlobals.ISerializableFactoryTypeLocalName, DictionaryGlobals.SerializationNamespace, dataNode.FactoryTypeName, dataNode.FactoryTypeNamespace);
				}
				IList<ISerializableDataMember> members = dataNode.Members;
				if (members != null)
				{
					for (int i = 0; i < members.Count; i++)
					{
						ISerializableDataMember serializableDataMember = members[i];
						xmlWriter.WriteStartElement(serializableDataMember.Name, string.Empty);
						this.WriteExtensionDataValue(xmlWriter, serializableDataMember.Value);
						xmlWriter.WriteEndElement();
					}
				}
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00039A20 File Offset: 0x00037C20
		private void WriteExtensionXmlData(XmlWriterDelegator xmlWriter, XmlDataNode dataNode)
		{
			if (!this.TryWriteDeserializedExtensionData(xmlWriter, dataNode))
			{
				IList<XmlAttribute> xmlAttributes = dataNode.XmlAttributes;
				if (xmlAttributes != null)
				{
					foreach (XmlAttribute xmlAttribute in xmlAttributes)
					{
						xmlAttribute.WriteTo(xmlWriter.Writer);
					}
				}
				this.WriteExtensionDataTypeInfo(xmlWriter, dataNode);
				IList<XmlNode> xmlChildNodes = dataNode.XmlChildNodes;
				if (xmlChildNodes != null)
				{
					foreach (XmlNode xmlNode in xmlChildNodes)
					{
						xmlNode.WriteTo(xmlWriter.Writer);
					}
				}
			}
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00039ACC File Offset: 0x00037CCC
		protected virtual void WriteDataContractValue(DataContract dataContract, XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle declaredTypeHandle)
		{
			dataContract.WriteXmlValue(xmlWriter, obj, this);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00039AD7 File Offset: 0x00037CD7
		protected virtual void WriteNull(XmlWriterDelegator xmlWriter)
		{
			XmlObjectSerializer.WriteNull(xmlWriter);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00039AE0 File Offset: 0x00037CE0
		private void WriteResolvedTypeInfo(XmlWriterDelegator writer, Type objectType, Type declaredType)
		{
			XmlDictionaryString xmlDictionaryString;
			XmlDictionaryString xmlDictionaryString2;
			if (this.ResolveType(objectType, declaredType, out xmlDictionaryString, out xmlDictionaryString2))
			{
				this.WriteTypeInfo(writer, xmlDictionaryString, xmlDictionaryString2);
			}
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00039B04 File Offset: 0x00037D04
		private bool ResolveType(Type objectType, Type declaredType, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
		{
			if (!base.DataContractResolver.TryResolveType(objectType, declaredType, base.KnownTypeResolver, out typeName, out typeNamespace))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An object of type '{0}' which derives from DataContractResolver returned false from its TryResolveType method when attempting to resolve the name for an object of type '{1}', indicating that the resolution failed. Change the TryResolveType implementation to return true.", new object[]
				{
					DataContract.GetClrTypeFullName(base.DataContractResolver.GetType()),
					DataContract.GetClrTypeFullName(objectType)
				})));
			}
			if (typeName == null)
			{
				if (typeNamespace == null)
				{
					return false;
				}
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An object of type '{0}' which derives from DataContractResolver returned a null typeName or typeNamespace but not both from its TryResolveType method when attempting to resolve the name for an object of type '{1}'. Change the TryResolveType implementation to return non-null values, or to return null values for both typeName and typeNamespace in order to serialize as the declared type.", new object[]
				{
					DataContract.GetClrTypeFullName(base.DataContractResolver.GetType()),
					DataContract.GetClrTypeFullName(objectType)
				})));
			}
			else
			{
				if (typeNamespace == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An object of type '{0}' which derives from DataContractResolver returned a null typeName or typeNamespace but not both from its TryResolveType method when attempting to resolve the name for an object of type '{1}'. Change the TryResolveType implementation to return non-null values, or to return null values for both typeName and typeNamespace in order to serialize as the declared type.", new object[]
					{
						DataContract.GetClrTypeFullName(base.DataContractResolver.GetType()),
						DataContract.GetClrTypeFullName(objectType)
					})));
				}
				return true;
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00039BDF File Offset: 0x00037DDF
		protected virtual bool WriteTypeInfo(XmlWriterDelegator writer, DataContract contract, DataContract declaredContract)
		{
			if (XmlObjectSerializer.IsContractDeclared(contract, declaredContract))
			{
				return false;
			}
			if (base.DataContractResolver == null)
			{
				this.WriteTypeInfo(writer, contract.Name, contract.Namespace);
				return true;
			}
			this.WriteResolvedTypeInfo(writer, contract.OriginalUnderlyingType, declaredContract.OriginalUnderlyingType);
			return false;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00039C1D File Offset: 0x00037E1D
		protected virtual void WriteTypeInfo(XmlWriterDelegator writer, string dataContractName, string dataContractNamespace)
		{
			writer.WriteAttributeQualifiedName("i", DictionaryGlobals.XsiTypeLocalName, DictionaryGlobals.SchemaInstanceNamespace, dataContractName, dataContractNamespace);
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00039C36 File Offset: 0x00037E36
		protected virtual void WriteTypeInfo(XmlWriterDelegator writer, XmlDictionaryString dataContractName, XmlDictionaryString dataContractNamespace)
		{
			writer.WriteAttributeQualifiedName("i", DictionaryGlobals.XsiTypeLocalName, DictionaryGlobals.SchemaInstanceNamespace, dataContractName, dataContractNamespace);
		}

		// Token: 0x0400056E RID: 1390
		private ObjectReferenceStack byValObjectsInScope;

		// Token: 0x0400056F RID: 1391
		private XmlSerializableWriter xmlSerializableWriter;

		// Token: 0x04000570 RID: 1392
		private const int depthToCheckCyclicReference = 512;

		// Token: 0x04000571 RID: 1393
		protected bool preserveObjectReferences;

		// Token: 0x04000572 RID: 1394
		private ObjectToIdCache serializedObjects;

		// Token: 0x04000573 RID: 1395
		private bool isGetOnlyCollection;

		// Token: 0x04000574 RID: 1396
		private readonly bool unsafeTypeForwardingEnabled;

		// Token: 0x04000575 RID: 1397
		protected bool serializeReadOnlyTypes;
	}
}
