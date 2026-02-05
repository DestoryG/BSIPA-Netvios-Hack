using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x0200011A RID: 282
	internal class XmlObjectSerializerWriteContextComplexJson : XmlObjectSerializerWriteContextComplex
	{
		// Token: 0x06001125 RID: 4389 RVA: 0x00047420 File Offset: 0x00045620
		public XmlObjectSerializerWriteContextComplexJson(DataContractJsonSerializer serializer, DataContract rootTypeDataContract)
			: base(serializer, serializer.MaxItemsInObjectGraph, new StreamingContext(StreamingContextStates.All), serializer.IgnoreExtensionDataObject)
		{
			this.emitXsiType = serializer.EmitTypeInformation;
			this.rootTypeDataContract = rootTypeDataContract;
			this.serializerKnownTypeList = serializer.knownTypeList;
			this.dataContractSurrogate = serializer.DataContractSurrogate;
			this.serializeReadOnlyTypes = serializer.SerializeReadOnlyTypes;
			this.useSimpleDictionaryFormat = serializer.UseSimpleDictionaryFormat;
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x0004748D File Offset: 0x0004568D
		internal static XmlObjectSerializerWriteContextComplexJson CreateContext(DataContractJsonSerializer serializer, DataContract rootTypeDataContract)
		{
			return new XmlObjectSerializerWriteContextComplexJson(serializer, rootTypeDataContract);
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06001127 RID: 4391 RVA: 0x00047496 File Offset: 0x00045696
		internal IList<Type> SerializerKnownTypeList
		{
			get
			{
				return this.serializerKnownTypeList;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06001128 RID: 4392 RVA: 0x0004749E File Offset: 0x0004569E
		public bool UseSimpleDictionaryFormat
		{
			get
			{
				return this.useSimpleDictionaryFormat;
			}
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x000474A6 File Offset: 0x000456A6
		internal override bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, Type dataContractType, string clrTypeName, string clrAssemblyName)
		{
			return false;
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x000474A9 File Offset: 0x000456A9
		internal override bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, DataContract dataContract)
		{
			return false;
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x000474AC File Offset: 0x000456AC
		internal override void WriteArraySize(XmlWriterDelegator xmlWriter, int size)
		{
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x000474AE File Offset: 0x000456AE
		protected override void WriteTypeInfo(XmlWriterDelegator writer, string dataContractName, string dataContractNamespace)
		{
			if (this.emitXsiType != EmitTypeInformation.Never)
			{
				if (string.IsNullOrEmpty(dataContractNamespace))
				{
					this.WriteTypeInfo(writer, dataContractName);
					return;
				}
				this.WriteTypeInfo(writer, dataContractName + ":" + XmlObjectSerializerWriteContextComplexJson.TruncateDefaultDataContractNamespace(dataContractNamespace));
			}
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x000474E4 File Offset: 0x000456E4
		internal static string TruncateDefaultDataContractNamespace(string dataContractNamespace)
		{
			if (!string.IsNullOrEmpty(dataContractNamespace))
			{
				if (dataContractNamespace[0] == '#')
				{
					return "\\" + dataContractNamespace;
				}
				if (dataContractNamespace[0] == '\\')
				{
					return "\\" + dataContractNamespace;
				}
				if (dataContractNamespace.StartsWith("http://schemas.datacontract.org/2004/07/", StringComparison.Ordinal))
				{
					return "#" + dataContractNamespace.Substring(JsonGlobals.DataContractXsdBaseNamespaceLength);
				}
			}
			return dataContractNamespace;
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x0004754C File Offset: 0x0004574C
		private static bool RequiresJsonTypeInfo(DataContract contract)
		{
			return contract is ClassDataContract;
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x00047557 File Offset: 0x00045757
		private void WriteTypeInfo(XmlWriterDelegator writer, string typeInformation)
		{
			writer.WriteAttributeString(null, "__type", null, typeInformation);
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00047568 File Offset: 0x00045768
		protected override bool WriteTypeInfo(XmlWriterDelegator writer, DataContract contract, DataContract declaredContract)
		{
			if ((contract.Name != declaredContract.Name || contract.Namespace != declaredContract.Namespace) && (!(contract.Name.Value == declaredContract.Name.Value) || !(contract.Namespace.Value == declaredContract.Namespace.Value)) && contract.UnderlyingType != Globals.TypeOfObjectArray && this.emitXsiType != EmitTypeInformation.Never)
			{
				if (XmlObjectSerializerWriteContextComplexJson.RequiresJsonTypeInfo(contract))
				{
					this.perCallXsiTypeAlreadyEmitted = true;
					this.WriteTypeInfo(writer, contract.Name.Value, contract.Namespace.Value);
				}
				else if (declaredContract.UnderlyingType == typeof(Enum))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("Enum type is not supported by DataContractJsonSerializer. The underlying type is '{0}'.", new object[] { declaredContract.UnderlyingType })));
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00047658 File Offset: 0x00045858
		internal void WriteJsonISerializable(XmlWriterDelegator xmlWriter, ISerializable obj)
		{
			Type type = obj.GetType();
			SerializationInfo serializationInfo = new SerializationInfo(type, XmlObjectSerializer.FormatterConverter);
			base.GetObjectData(obj, serializationInfo, base.GetStreamingContext());
			if (DataContract.GetClrTypeFullName(type) != serializationInfo.FullTypeName)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Changing full type name is not supported. Serialization type name: '{0}', data contract type name: '{1}'.", new object[]
				{
					serializationInfo.FullTypeName,
					DataContract.GetClrTypeFullName(type)
				})));
			}
			base.WriteSerializationInfo(xmlWriter, type, serializationInfo);
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x000476CF File Offset: 0x000458CF
		internal static DataContract GetRevisedItemContract(DataContract oldItemContract)
		{
			if (oldItemContract != null && oldItemContract.UnderlyingType.IsGenericType && oldItemContract.UnderlyingType.GetGenericTypeDefinition() == Globals.TypeOfKeyValue)
			{
				return DataContract.GetDataContract(oldItemContract.UnderlyingType);
			}
			return oldItemContract;
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00047708 File Offset: 0x00045908
		protected override void WriteDataContractValue(DataContract dataContract, XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle declaredTypeHandle)
		{
			JsonDataContract jsonDataContract = JsonDataContract.GetJsonDataContract(dataContract);
			if (this.emitXsiType == EmitTypeInformation.Always && !this.perCallXsiTypeAlreadyEmitted && XmlObjectSerializerWriteContextComplexJson.RequiresJsonTypeInfo(dataContract))
			{
				this.WriteTypeInfo(xmlWriter, jsonDataContract.TypeName);
			}
			this.perCallXsiTypeAlreadyEmitted = false;
			DataContractJsonSerializer.WriteJsonValue(jsonDataContract, xmlWriter, obj, this, declaredTypeHandle);
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x00047754 File Offset: 0x00045954
		protected override void WriteNull(XmlWriterDelegator xmlWriter)
		{
			DataContractJsonSerializer.WriteJsonNull(xmlWriter);
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001135 RID: 4405 RVA: 0x0004775C File Offset: 0x0004595C
		internal XmlDictionaryString CollectionItemName
		{
			get
			{
				return JsonGlobals.itemDictionaryString;
			}
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00047763 File Offset: 0x00045963
		internal static void WriteJsonNameWithMapping(XmlWriterDelegator xmlWriter, XmlDictionaryString[] memberNames, int index)
		{
			xmlWriter.WriteStartElement("a", "item", "item");
			xmlWriter.WriteAttributeString(null, "item", null, memberNames[index].Value);
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00047790 File Offset: 0x00045990
		internal override void WriteExtensionDataTypeInfo(XmlWriterDelegator xmlWriter, IDataNode dataNode)
		{
			Type dataType = dataNode.DataType;
			if (dataType == Globals.TypeOfClassDataNode || dataType == Globals.TypeOfISerializableDataNode)
			{
				xmlWriter.WriteAttributeString(null, "type", null, "object");
				base.WriteExtensionDataTypeInfo(xmlWriter, dataNode);
				return;
			}
			if (dataType == Globals.TypeOfCollectionDataNode)
			{
				xmlWriter.WriteAttributeString(null, "type", null, "array");
				return;
			}
			if (!(dataType == Globals.TypeOfXmlDataNode) && dataType == Globals.TypeOfObject && dataNode.Value != null && XmlObjectSerializerWriteContextComplexJson.RequiresJsonTypeInfo(base.GetDataContract(dataNode.Value.GetType())))
			{
				base.WriteExtensionDataTypeInfo(xmlWriter, dataNode);
			}
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0004783C File Offset: 0x00045A3C
		protected override void SerializeWithXsiType(XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle objectTypeHandle, Type objectType, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle, Type declaredType)
		{
			bool flag = false;
			bool isInterface = declaredType.IsInterface;
			DataContract dataContract;
			if (isInterface && CollectionDataContract.IsCollectionInterface(declaredType))
			{
				dataContract = this.GetDataContract(declaredTypeHandle, declaredType);
			}
			else if (declaredType.IsArray)
			{
				dataContract = this.GetDataContract(declaredTypeHandle, declaredType);
			}
			else
			{
				dataContract = this.GetDataContract(objectTypeHandle, objectType);
				DataContract dataContract2 = ((declaredTypeID >= 0) ? this.GetDataContract(declaredTypeID, declaredTypeHandle) : this.GetDataContract(declaredTypeHandle, declaredType));
				flag = this.WriteTypeInfo(xmlWriter, dataContract, dataContract2);
				this.HandleCollectionAssignedToObject(declaredType, ref dataContract, ref obj, ref flag);
			}
			if (isInterface)
			{
				XmlObjectSerializerWriteContextComplexJson.VerifyObjectCompatibilityWithInterface(dataContract, obj, declaredType);
			}
			base.SerializeAndVerifyType(dataContract, xmlWriter, obj, flag, declaredType.TypeHandle, declaredType);
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x000478E0 File Offset: 0x00045AE0
		private static void VerifyObjectCompatibilityWithInterface(DataContract contract, object graph, Type declaredType)
		{
			Type type = contract.GetType();
			if (type == typeof(XmlDataContract) && !Globals.TypeOfIXmlSerializable.IsAssignableFrom(declaredType))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Object of type '{0}' is assigned to an incompatible interface '{1}'.", new object[]
				{
					graph.GetType(),
					declaredType
				})));
			}
			if (type == typeof(CollectionDataContract) && !CollectionDataContract.IsCollectionInterface(declaredType))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Collection of type '{0}' is assigned to an incompatible interface '{1}'", new object[]
				{
					graph.GetType(),
					declaredType
				})));
			}
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00047980 File Offset: 0x00045B80
		private void HandleCollectionAssignedToObject(Type declaredType, ref DataContract dataContract, ref object obj, ref bool verifyKnownType)
		{
			if (declaredType != dataContract.UnderlyingType && dataContract is CollectionDataContract)
			{
				if (verifyKnownType)
				{
					this.VerifyType(dataContract, declaredType);
					verifyKnownType = false;
				}
				if (((CollectionDataContract)dataContract).Kind == CollectionKind.Dictionary)
				{
					IDictionary dictionary = obj as IDictionary;
					Dictionary<object, object> dictionary2 = new Dictionary<object, object>();
					foreach (object obj2 in dictionary)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
						dictionary2.Add(dictionaryEntry.Key, dictionaryEntry.Value);
					}
					obj = dictionary2;
				}
				dataContract = base.GetDataContract(Globals.TypeOfIEnumerable);
			}
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00047A3C File Offset: 0x00045C3C
		internal override void SerializeWithXsiTypeAtTopLevel(DataContract dataContract, XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle originalDeclaredTypeHandle, Type graphType)
		{
			bool flag = false;
			Type underlyingType = this.rootTypeDataContract.UnderlyingType;
			bool isInterface = underlyingType.IsInterface;
			if ((!isInterface || !CollectionDataContract.IsCollectionInterface(underlyingType)) && !underlyingType.IsArray)
			{
				flag = this.WriteTypeInfo(xmlWriter, dataContract, this.rootTypeDataContract);
				this.HandleCollectionAssignedToObject(underlyingType, ref dataContract, ref obj, ref flag);
			}
			if (isInterface)
			{
				XmlObjectSerializerWriteContextComplexJson.VerifyObjectCompatibilityWithInterface(dataContract, obj, underlyingType);
			}
			base.SerializeAndVerifyType(dataContract, xmlWriter, obj, flag, underlyingType.TypeHandle, underlyingType);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00047AA8 File Offset: 0x00045CA8
		private void VerifyType(DataContract dataContract, Type declaredType)
		{
			bool flag = false;
			if (dataContract.KnownDataContracts != null)
			{
				this.scopedKnownTypes.Push(dataContract.KnownDataContracts);
				flag = true;
			}
			if (!base.IsKnownType(dataContract, declaredType))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Type '{0}' with data contract name '{1}:{2}' is not expected. Add any types not known statically to the list of known types - for example, by using the KnownTypeAttribute attribute or by adding them to the list of known types passed to DataContractSerializer.", new object[]
				{
					DataContract.GetClrTypeFullName(dataContract.UnderlyingType),
					dataContract.StableName.Name,
					dataContract.StableName.Namespace
				})));
			}
			if (flag)
			{
				this.scopedKnownTypes.Pop();
			}
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00047B2F File Offset: 0x00045D2F
		internal override DataContract GetDataContract(RuntimeTypeHandle typeHandle, Type type)
		{
			DataContract dataContract = base.GetDataContract(typeHandle, type);
			DataContractJsonSerializer.CheckIfTypeIsReference(dataContract);
			return dataContract;
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00047B3F File Offset: 0x00045D3F
		internal override DataContract GetDataContractSkipValidation(int typeId, RuntimeTypeHandle typeHandle, Type type)
		{
			DataContract dataContractSkipValidation = base.GetDataContractSkipValidation(typeId, typeHandle, type);
			DataContractJsonSerializer.CheckIfTypeIsReference(dataContractSkipValidation);
			return dataContractSkipValidation;
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x00047B50 File Offset: 0x00045D50
		internal override DataContract GetDataContract(int id, RuntimeTypeHandle typeHandle)
		{
			DataContract dataContract = base.GetDataContract(id, typeHandle);
			DataContractJsonSerializer.CheckIfTypeIsReference(dataContract);
			return dataContract;
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00047B60 File Offset: 0x00045D60
		internal static DataContract ResolveJsonDataContractFromRootDataContract(XmlObjectSerializerContext context, XmlQualifiedName typeQName, DataContract rootTypeDataContract)
		{
			if (rootTypeDataContract.StableName == typeQName)
			{
				return rootTypeDataContract;
			}
			DataContract dataContract;
			for (CollectionDataContract collectionDataContract = rootTypeDataContract as CollectionDataContract; collectionDataContract != null; collectionDataContract = dataContract as CollectionDataContract)
			{
				if (collectionDataContract.ItemType.IsGenericType && collectionDataContract.ItemType.GetGenericTypeDefinition() == typeof(KeyValue<, >))
				{
					dataContract = context.GetDataContract(Globals.TypeOfKeyValuePair.MakeGenericType(collectionDataContract.ItemType.GetGenericArguments()));
				}
				else
				{
					dataContract = context.GetDataContract(context.GetSurrogatedType(collectionDataContract.ItemType));
				}
				if (dataContract.StableName == typeQName)
				{
					return dataContract;
				}
			}
			return null;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00047BFB File Offset: 0x00045DFB
		protected override DataContract ResolveDataContractFromRootDataContract(XmlQualifiedName typeQName)
		{
			return XmlObjectSerializerWriteContextComplexJson.ResolveJsonDataContractFromRootDataContract(this, typeQName, this.rootTypeDataContract);
		}

		// Token: 0x04000861 RID: 2145
		private EmitTypeInformation emitXsiType;

		// Token: 0x04000862 RID: 2146
		private bool perCallXsiTypeAlreadyEmitted;

		// Token: 0x04000863 RID: 2147
		private bool useSimpleDictionaryFormat;
	}
}
