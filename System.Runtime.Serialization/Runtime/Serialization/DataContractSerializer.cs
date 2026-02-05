using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x02000077 RID: 119
	public sealed class DataContractSerializer : XmlObjectSerializer
	{
		// Token: 0x060008A5 RID: 2213 RVA: 0x00028132 File Offset: 0x00026332
		public DataContractSerializer(Type type)
			: this(type, null)
		{
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0002813C File Offset: 0x0002633C
		public DataContractSerializer(Type type, IEnumerable<Type> knownTypes)
			: this(type, knownTypes, int.MaxValue, false, false, null)
		{
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0002814E File Offset: 0x0002634E
		public DataContractSerializer(Type type, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate)
			: this(type, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, null)
		{
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00028160 File Offset: 0x00026360
		public DataContractSerializer(Type type, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate, DataContractResolver dataContractResolver)
		{
			this.Initialize(type, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, dataContractResolver, false);
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00028185 File Offset: 0x00026385
		public DataContractSerializer(Type type, string rootName, string rootNamespace)
			: this(type, rootName, rootNamespace, null)
		{
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00028194 File Offset: 0x00026394
		public DataContractSerializer(Type type, string rootName, string rootNamespace, IEnumerable<Type> knownTypes)
			: this(type, rootName, rootNamespace, knownTypes, int.MaxValue, false, false, null)
		{
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x000281B4 File Offset: 0x000263B4
		public DataContractSerializer(Type type, string rootName, string rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate)
			: this(type, rootName, rootNamespace, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, null)
		{
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x000281D8 File Offset: 0x000263D8
		public DataContractSerializer(Type type, string rootName, string rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate, DataContractResolver dataContractResolver)
		{
			XmlDictionary xmlDictionary = new XmlDictionary(2);
			this.Initialize(type, xmlDictionary.Add(rootName), xmlDictionary.Add(DataContract.GetNamespace(rootNamespace)), knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, dataContractResolver, false);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00028219 File Offset: 0x00026419
		public DataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace)
			: this(type, rootName, rootNamespace, null)
		{
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00028228 File Offset: 0x00026428
		public DataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type> knownTypes)
			: this(type, rootName, rootNamespace, knownTypes, int.MaxValue, false, false, null, null)
		{
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0002824C File Offset: 0x0002644C
		public DataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate)
			: this(type, rootName, rootNamespace, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, null)
		{
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00028270 File Offset: 0x00026470
		public DataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate, DataContractResolver dataContractResolver)
		{
			this.Initialize(type, rootName, rootNamespace, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, dataContractResolver, false);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0002829C File Offset: 0x0002649C
		public DataContractSerializer(Type type, DataContractSerializerSettings settings)
		{
			if (settings == null)
			{
				settings = new DataContractSerializerSettings();
			}
			this.Initialize(type, settings.RootName, settings.RootNamespace, settings.KnownTypes, settings.MaxItemsInObjectGraph, settings.IgnoreExtensionDataObject, settings.PreserveObjectReferences, settings.DataContractSurrogate, settings.DataContractResolver, settings.SerializeReadOnlyTypes);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x000282F8 File Offset: 0x000264F8
		private void Initialize(Type type, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate, DataContractResolver dataContractResolver, bool serializeReadOnlyTypes)
		{
			XmlObjectSerializer.CheckNull(type, "type");
			this.rootType = type;
			if (knownTypes != null)
			{
				this.knownTypeList = new List<Type>();
				foreach (Type type2 in knownTypes)
				{
					this.knownTypeList.Add(type2);
				}
			}
			if (maxItemsInObjectGraph < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("maxItemsInObjectGraph", SR.GetString("The value of this argument must be non-negative.")));
			}
			this.maxItemsInObjectGraph = maxItemsInObjectGraph;
			this.ignoreExtensionDataObject = ignoreExtensionDataObject;
			this.preserveObjectReferences = preserveObjectReferences;
			this.dataContractSurrogate = dataContractSurrogate;
			this.dataContractResolver = dataContractResolver;
			this.serializeReadOnlyTypes = serializeReadOnlyTypes;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x000283B4 File Offset: 0x000265B4
		private void Initialize(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate, DataContractResolver dataContractResolver, bool serializeReadOnlyTypes)
		{
			this.Initialize(type, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, dataContractResolver, serializeReadOnlyTypes);
			this.rootName = rootName;
			this.rootNamespace = rootNamespace;
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x000283E4 File Offset: 0x000265E4
		public ReadOnlyCollection<Type> KnownTypes
		{
			get
			{
				if (this.knownTypeCollection == null)
				{
					if (this.knownTypeList != null)
					{
						this.knownTypeCollection = new ReadOnlyCollection<Type>(this.knownTypeList);
					}
					else
					{
						this.knownTypeCollection = new ReadOnlyCollection<Type>(Globals.EmptyTypeArray);
					}
				}
				return this.knownTypeCollection;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x0002841F File Offset: 0x0002661F
		internal override Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
		{
			get
			{
				if (this.knownDataContracts == null && this.knownTypeList != null)
				{
					this.knownDataContracts = XmlObjectSerializerContext.GetDataContractsForKnownTypes(this.knownTypeList);
				}
				return this.knownDataContracts;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x00028448 File Offset: 0x00026648
		public int MaxItemsInObjectGraph
		{
			get
			{
				return this.maxItemsInObjectGraph;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x00028450 File Offset: 0x00026650
		public IDataContractSurrogate DataContractSurrogate
		{
			get
			{
				return this.dataContractSurrogate;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x00028458 File Offset: 0x00026658
		public bool PreserveObjectReferences
		{
			get
			{
				return this.preserveObjectReferences;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x00028460 File Offset: 0x00026660
		public bool IgnoreExtensionDataObject
		{
			get
			{
				return this.ignoreExtensionDataObject;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x00028468 File Offset: 0x00026668
		public DataContractResolver DataContractResolver
		{
			get
			{
				return this.dataContractResolver;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x00028470 File Offset: 0x00026670
		public bool SerializeReadOnlyTypes
		{
			get
			{
				return this.serializeReadOnlyTypes;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x00028478 File Offset: 0x00026678
		private DataContract RootContract
		{
			get
			{
				if (this.rootContract == null)
				{
					this.rootContract = DataContract.GetDataContract((this.dataContractSurrogate == null) ? this.rootType : DataContractSerializer.GetSurrogatedType(this.dataContractSurrogate, this.rootType));
					this.needsContractNsAtRoot = base.CheckIfNeedsContractNsAtRoot(this.rootName, this.rootNamespace, this.rootContract);
				}
				return this.rootContract;
			}
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x000284DD File Offset: 0x000266DD
		internal override void InternalWriteObject(XmlWriterDelegator writer, object graph)
		{
			this.InternalWriteObject(writer, graph, null);
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x000284E8 File Offset: 0x000266E8
		internal override void InternalWriteObject(XmlWriterDelegator writer, object graph, DataContractResolver dataContractResolver)
		{
			this.InternalWriteStartObject(writer, graph);
			this.InternalWriteObjectContent(writer, graph, dataContractResolver);
			this.InternalWriteEndObject(writer);
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00028502 File Offset: 0x00026702
		public override void WriteObject(XmlWriter writer, object graph)
		{
			base.WriteObjectHandleExceptions(new XmlWriterDelegator(writer), graph);
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00028511 File Offset: 0x00026711
		public override void WriteStartObject(XmlWriter writer, object graph)
		{
			base.WriteStartObjectHandleExceptions(new XmlWriterDelegator(writer), graph);
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00028520 File Offset: 0x00026720
		public override void WriteObjectContent(XmlWriter writer, object graph)
		{
			base.WriteObjectContentHandleExceptions(new XmlWriterDelegator(writer), graph);
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0002852F File Offset: 0x0002672F
		public override void WriteEndObject(XmlWriter writer)
		{
			base.WriteEndObjectHandleExceptions(new XmlWriterDelegator(writer));
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0002853D File Offset: 0x0002673D
		public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
		{
			base.WriteStartObjectHandleExceptions(new XmlWriterDelegator(writer), graph);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0002854C File Offset: 0x0002674C
		public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
		{
			base.WriteObjectContentHandleExceptions(new XmlWriterDelegator(writer), graph);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0002855B File Offset: 0x0002675B
		public override void WriteEndObject(XmlDictionaryWriter writer)
		{
			base.WriteEndObjectHandleExceptions(new XmlWriterDelegator(writer));
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00028569 File Offset: 0x00026769
		public void WriteObject(XmlDictionaryWriter writer, object graph, DataContractResolver dataContractResolver)
		{
			base.WriteObjectHandleExceptions(new XmlWriterDelegator(writer), graph, dataContractResolver);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00028579 File Offset: 0x00026779
		public override object ReadObject(XmlReader reader)
		{
			return base.ReadObjectHandleExceptions(new XmlReaderDelegator(reader), true);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00028588 File Offset: 0x00026788
		public override object ReadObject(XmlReader reader, bool verifyObjectName)
		{
			return base.ReadObjectHandleExceptions(new XmlReaderDelegator(reader), verifyObjectName);
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00028597 File Offset: 0x00026797
		public override bool IsStartObject(XmlReader reader)
		{
			return base.IsStartObjectHandleExceptions(new XmlReaderDelegator(reader));
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x000285A5 File Offset: 0x000267A5
		public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
		{
			return base.ReadObjectHandleExceptions(new XmlReaderDelegator(reader), verifyObjectName);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x000285B4 File Offset: 0x000267B4
		public override bool IsStartObject(XmlDictionaryReader reader)
		{
			return base.IsStartObjectHandleExceptions(new XmlReaderDelegator(reader));
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x000285C2 File Offset: 0x000267C2
		public object ReadObject(XmlDictionaryReader reader, bool verifyObjectName, DataContractResolver dataContractResolver)
		{
			return base.ReadObjectHandleExceptions(new XmlReaderDelegator(reader), verifyObjectName, dataContractResolver);
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x000285D2 File Offset: 0x000267D2
		internal override void InternalWriteStartObject(XmlWriterDelegator writer, object graph)
		{
			base.WriteRootElement(writer, this.RootContract, this.rootName, this.rootNamespace, this.needsContractNsAtRoot);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x000285F3 File Offset: 0x000267F3
		internal override void InternalWriteObjectContent(XmlWriterDelegator writer, object graph)
		{
			this.InternalWriteObjectContent(writer, graph, null);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00028600 File Offset: 0x00026800
		internal void InternalWriteObjectContent(XmlWriterDelegator writer, object graph, DataContractResolver dataContractResolver)
		{
			if (this.MaxItemsInObjectGraph == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Maximum number of items that can be serialized or deserialized in an object graph is '{0}'.", new object[] { this.MaxItemsInObjectGraph })));
			}
			DataContract dataContract = this.RootContract;
			Type underlyingType = dataContract.UnderlyingType;
			Type type = ((graph == null) ? underlyingType : graph.GetType());
			if (this.dataContractSurrogate != null)
			{
				graph = DataContractSerializer.SurrogateToDataContractType(this.dataContractSurrogate, graph, underlyingType, ref type);
			}
			if (dataContractResolver == null)
			{
				dataContractResolver = this.DataContractResolver;
			}
			if (graph == null)
			{
				if (base.IsRootXmlAny(this.rootName, dataContract))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("A null value cannot be serialized at the top level for IXmlSerializable root type '{0}' since its IsAny setting is 'true'. This type must write all its contents including the root element. Verify that the IXmlSerializable implementation is correct.", new object[] { underlyingType })));
				}
				XmlObjectSerializer.WriteNull(writer);
				return;
			}
			else if (underlyingType == type)
			{
				if (dataContract.CanContainReferences)
				{
					XmlObjectSerializerWriteContext xmlObjectSerializerWriteContext = XmlObjectSerializerWriteContext.CreateContext(this, dataContract, dataContractResolver);
					xmlObjectSerializerWriteContext.HandleGraphAtTopLevel(writer, graph, dataContract);
					xmlObjectSerializerWriteContext.SerializeWithoutXsiType(dataContract, writer, graph, underlyingType.TypeHandle);
					return;
				}
				dataContract.WriteXmlValue(writer, graph, null);
				return;
			}
			else
			{
				if (base.IsRootXmlAny(this.rootName, dataContract))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An object of type '{0}' cannot be serialized at the top level for IXmlSerializable root type '{1}' since its IsAny setting is 'true'. This type must write all its contents including the root element. Verify that the IXmlSerializable implementation is correct.", new object[] { type, dataContract.UnderlyingType })));
				}
				dataContract = DataContractSerializer.GetDataContract(dataContract, underlyingType, type);
				XmlObjectSerializerWriteContext xmlObjectSerializerWriteContext2 = XmlObjectSerializerWriteContext.CreateContext(this, this.RootContract, dataContractResolver);
				if (dataContract.CanContainReferences)
				{
					xmlObjectSerializerWriteContext2.HandleGraphAtTopLevel(writer, graph, dataContract);
				}
				xmlObjectSerializerWriteContext2.OnHandleIsReference(writer, dataContract, graph);
				xmlObjectSerializerWriteContext2.SerializeWithXsiTypeAtTopLevel(dataContract, writer, graph, underlyingType.TypeHandle, type);
				return;
			}
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0002876B File Offset: 0x0002696B
		internal static DataContract GetDataContract(DataContract declaredTypeContract, Type declaredType, Type objectType)
		{
			if (declaredType.IsInterface && CollectionDataContract.IsCollectionInterface(declaredType))
			{
				return declaredTypeContract;
			}
			if (declaredType.IsArray)
			{
				return declaredTypeContract;
			}
			return DataContract.GetDataContract(objectType.TypeHandle, objectType, SerializationMode.SharedContract);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00028796 File Offset: 0x00026996
		internal void SetDataContractSurrogate(IDataContractSurrogate adapter)
		{
			this.dataContractSurrogate = adapter;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0002879F File Offset: 0x0002699F
		internal override void InternalWriteEndObject(XmlWriterDelegator writer)
		{
			if (!base.IsRootXmlAny(this.rootName, this.RootContract))
			{
				writer.WriteEndElement();
			}
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x000287BB File Offset: 0x000269BB
		internal override object InternalReadObject(XmlReaderDelegator xmlReader, bool verifyObjectName)
		{
			return this.InternalReadObject(xmlReader, verifyObjectName, null);
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000287C8 File Offset: 0x000269C8
		internal override object InternalReadObject(XmlReaderDelegator xmlReader, bool verifyObjectName, DataContractResolver dataContractResolver)
		{
			if (this.MaxItemsInObjectGraph == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Maximum number of items that can be serialized or deserialized in an object graph is '{0}'.", new object[] { this.MaxItemsInObjectGraph })));
			}
			if (dataContractResolver == null)
			{
				dataContractResolver = this.DataContractResolver;
			}
			if (verifyObjectName)
			{
				if (!this.InternalIsStartObject(xmlReader))
				{
					XmlDictionaryString topLevelElementName;
					XmlDictionaryString topLevelElementNamespace;
					if (this.rootName == null)
					{
						topLevelElementName = this.RootContract.TopLevelElementName;
						topLevelElementNamespace = this.RootContract.TopLevelElementNamespace;
					}
					else
					{
						topLevelElementName = this.rootName;
						topLevelElementNamespace = this.rootNamespace;
					}
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationExceptionWithReaderDetails(SR.GetString("Expecting element '{1}' from namespace '{0}'.", new object[] { topLevelElementNamespace, topLevelElementName }), xmlReader));
				}
			}
			else if (!base.IsStartElement(xmlReader))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationExceptionWithReaderDetails(SR.GetString("Expecting state '{0}' when ReadObject is called.", new object[] { XmlNodeType.Element }), xmlReader));
			}
			DataContract dataContract = this.RootContract;
			if (dataContract.IsPrimitive && dataContract.UnderlyingType == this.rootType)
			{
				return dataContract.ReadXmlValue(xmlReader, null);
			}
			if (base.IsRootXmlAny(this.rootName, dataContract))
			{
				return XmlObjectSerializerReadContext.ReadRootIXmlSerializable(xmlReader, dataContract as XmlDataContract, false);
			}
			return XmlObjectSerializerReadContext.CreateContext(this, dataContract, dataContractResolver).InternalDeserialize(xmlReader, this.rootType, dataContract, null, null);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x000288FC File Offset: 0x00026AFC
		internal override bool InternalIsStartObject(XmlReaderDelegator reader)
		{
			return base.IsRootElement(reader, this.RootContract, this.rootName, this.rootNamespace);
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00028917 File Offset: 0x00026B17
		internal override Type GetSerializeType(object graph)
		{
			if (graph != null)
			{
				return graph.GetType();
			}
			return this.rootType;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00028929 File Offset: 0x00026B29
		internal override Type GetDeserializeType()
		{
			return this.rootType;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00028934 File Offset: 0x00026B34
		internal static object SurrogateToDataContractType(IDataContractSurrogate dataContractSurrogate, object oldObj, Type surrogatedDeclaredType, ref Type objType)
		{
			object objectToSerialize = DataContractSurrogateCaller.GetObjectToSerialize(dataContractSurrogate, oldObj, objType, surrogatedDeclaredType);
			if (objectToSerialize != oldObj)
			{
				if (objectToSerialize == null)
				{
					objType = Globals.TypeOfObject;
				}
				else
				{
					objType = objectToSerialize.GetType();
				}
			}
			return objectToSerialize;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00028965 File Offset: 0x00026B65
		internal static Type GetSurrogatedType(IDataContractSurrogate dataContractSurrogate, Type type)
		{
			return DataContractSurrogateCaller.GetDataContractType(dataContractSurrogate, DataContract.UnwrapNullableType(type));
		}

		// Token: 0x04000329 RID: 809
		private Type rootType;

		// Token: 0x0400032A RID: 810
		private DataContract rootContract;

		// Token: 0x0400032B RID: 811
		private bool needsContractNsAtRoot;

		// Token: 0x0400032C RID: 812
		private XmlDictionaryString rootName;

		// Token: 0x0400032D RID: 813
		private XmlDictionaryString rootNamespace;

		// Token: 0x0400032E RID: 814
		private int maxItemsInObjectGraph;

		// Token: 0x0400032F RID: 815
		private bool ignoreExtensionDataObject;

		// Token: 0x04000330 RID: 816
		private bool preserveObjectReferences;

		// Token: 0x04000331 RID: 817
		private IDataContractSurrogate dataContractSurrogate;

		// Token: 0x04000332 RID: 818
		private ReadOnlyCollection<Type> knownTypeCollection;

		// Token: 0x04000333 RID: 819
		internal IList<Type> knownTypeList;

		// Token: 0x04000334 RID: 820
		internal Dictionary<XmlQualifiedName, DataContract> knownDataContracts;

		// Token: 0x04000335 RID: 821
		private DataContractResolver dataContractResolver;

		// Token: 0x04000336 RID: 822
		private bool serializeReadOnlyTypes;
	}
}
