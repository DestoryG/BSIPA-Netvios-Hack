using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x020000FC RID: 252
	[TypeForwardedFrom("System.ServiceModel.Web, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
	public sealed class DataContractJsonSerializer : XmlObjectSerializer
	{
		// Token: 0x06000F7F RID: 3967 RVA: 0x00040601 File Offset: 0x0003E801
		public DataContractJsonSerializer(Type type)
			: this(type, null)
		{
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0004060B File Offset: 0x0003E80B
		public DataContractJsonSerializer(Type type, string rootName)
			: this(type, rootName, null)
		{
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x00040616 File Offset: 0x0003E816
		public DataContractJsonSerializer(Type type, XmlDictionaryString rootName)
			: this(type, rootName, null)
		{
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x00040621 File Offset: 0x0003E821
		public DataContractJsonSerializer(Type type, IEnumerable<Type> knownTypes)
			: this(type, knownTypes, int.MaxValue, false, null, false)
		{
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x00040633 File Offset: 0x0003E833
		public DataContractJsonSerializer(Type type, string rootName, IEnumerable<Type> knownTypes)
			: this(type, rootName, knownTypes, int.MaxValue, false, null, false)
		{
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x00040646 File Offset: 0x0003E846
		public DataContractJsonSerializer(Type type, XmlDictionaryString rootName, IEnumerable<Type> knownTypes)
			: this(type, rootName, knownTypes, int.MaxValue, false, null, false)
		{
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0004065C File Offset: 0x0003E85C
		public DataContractJsonSerializer(Type type, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, IDataContractSurrogate dataContractSurrogate, bool alwaysEmitTypeInformation)
		{
			EmitTypeInformation emitTypeInformation = (alwaysEmitTypeInformation ? EmitTypeInformation.Always : EmitTypeInformation.AsNeeded);
			this.Initialize(type, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, dataContractSurrogate, emitTypeInformation, false, null, false);
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0004068C File Offset: 0x0003E88C
		public DataContractJsonSerializer(Type type, string rootName, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, IDataContractSurrogate dataContractSurrogate, bool alwaysEmitTypeInformation)
		{
			EmitTypeInformation emitTypeInformation = (alwaysEmitTypeInformation ? EmitTypeInformation.Always : EmitTypeInformation.AsNeeded);
			XmlDictionary xmlDictionary = new XmlDictionary(2);
			this.Initialize(type, xmlDictionary.Add(rootName), knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, dataContractSurrogate, emitTypeInformation, false, null, false);
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x000406C8 File Offset: 0x0003E8C8
		public DataContractJsonSerializer(Type type, XmlDictionaryString rootName, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, IDataContractSurrogate dataContractSurrogate, bool alwaysEmitTypeInformation)
		{
			EmitTypeInformation emitTypeInformation = (alwaysEmitTypeInformation ? EmitTypeInformation.Always : EmitTypeInformation.AsNeeded);
			this.Initialize(type, rootName, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, dataContractSurrogate, emitTypeInformation, false, null, false);
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x000406F8 File Offset: 0x0003E8F8
		public DataContractJsonSerializer(Type type, DataContractJsonSerializerSettings settings)
		{
			if (settings == null)
			{
				settings = new DataContractJsonSerializerSettings();
			}
			XmlDictionaryString xmlDictionaryString = ((settings.RootName == null) ? null : new XmlDictionary(1).Add(settings.RootName));
			this.Initialize(type, xmlDictionaryString, settings.KnownTypes, settings.MaxItemsInObjectGraph, settings.IgnoreExtensionDataObject, settings.DataContractSurrogate, settings.EmitTypeInformation, settings.SerializeReadOnlyTypes, settings.DateTimeFormat, settings.UseSimpleDictionaryFormat);
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0004076A File Offset: 0x0003E96A
		public IDataContractSurrogate DataContractSurrogate
		{
			get
			{
				return this.dataContractSurrogate;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x00040772 File Offset: 0x0003E972
		public bool IgnoreExtensionDataObject
		{
			get
			{
				return this.ignoreExtensionDataObject;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0004077A File Offset: 0x0003E97A
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

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x000407B5 File Offset: 0x0003E9B5
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

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x000407DE File Offset: 0x0003E9DE
		public int MaxItemsInObjectGraph
		{
			get
			{
				return this.maxItemsInObjectGraph;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000F8E RID: 3982 RVA: 0x000407E6 File Offset: 0x0003E9E6
		internal bool AlwaysEmitTypeInformation
		{
			get
			{
				return this.emitTypeInformation == EmitTypeInformation.Always;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x000407F1 File Offset: 0x0003E9F1
		public EmitTypeInformation EmitTypeInformation
		{
			get
			{
				return this.emitTypeInformation;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000F90 RID: 3984 RVA: 0x000407F9 File Offset: 0x0003E9F9
		public bool SerializeReadOnlyTypes
		{
			get
			{
				return this.serializeReadOnlyTypes;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x00040801 File Offset: 0x0003EA01
		public DateTimeFormat DateTimeFormat
		{
			get
			{
				return this.dateTimeFormat;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x00040809 File Offset: 0x0003EA09
		public bool UseSimpleDictionaryFormat
		{
			get
			{
				return this.useSimpleDictionaryFormat;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000F93 RID: 3987 RVA: 0x00040814 File Offset: 0x0003EA14
		private DataContract RootContract
		{
			get
			{
				if (this.rootContract == null)
				{
					this.rootContract = DataContract.GetDataContract((this.dataContractSurrogate == null) ? this.rootType : DataContractSerializer.GetSurrogatedType(this.dataContractSurrogate, this.rootType));
					DataContractJsonSerializer.CheckIfTypeIsReference(this.rootContract);
				}
				return this.rootContract;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x00040866 File Offset: 0x0003EA66
		private XmlDictionaryString RootName
		{
			get
			{
				return this.rootName ?? JsonGlobals.rootDictionaryString;
			}
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00040877 File Offset: 0x0003EA77
		public override bool IsStartObject(XmlReader reader)
		{
			return base.IsStartObjectHandleExceptions(new JsonReaderDelegator(reader));
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00040885 File Offset: 0x0003EA85
		public override bool IsStartObject(XmlDictionaryReader reader)
		{
			return base.IsStartObjectHandleExceptions(new JsonReaderDelegator(reader));
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x00040893 File Offset: 0x0003EA93
		public override object ReadObject(Stream stream)
		{
			XmlObjectSerializer.CheckNull(stream, "stream");
			return this.ReadObject(JsonReaderWriterFactory.CreateJsonReader(stream, XmlDictionaryReaderQuotas.Max));
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x000408B1 File Offset: 0x0003EAB1
		public override object ReadObject(XmlReader reader)
		{
			return base.ReadObjectHandleExceptions(new JsonReaderDelegator(reader, this.DateTimeFormat), true);
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x000408C6 File Offset: 0x0003EAC6
		public override object ReadObject(XmlReader reader, bool verifyObjectName)
		{
			return base.ReadObjectHandleExceptions(new JsonReaderDelegator(reader, this.DateTimeFormat), verifyObjectName);
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x000408DB File Offset: 0x0003EADB
		public override object ReadObject(XmlDictionaryReader reader)
		{
			return base.ReadObjectHandleExceptions(new JsonReaderDelegator(reader, this.DateTimeFormat), true);
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x000408F0 File Offset: 0x0003EAF0
		public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
		{
			return base.ReadObjectHandleExceptions(new JsonReaderDelegator(reader, this.DateTimeFormat), verifyObjectName);
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00040905 File Offset: 0x0003EB05
		public override void WriteEndObject(XmlWriter writer)
		{
			base.WriteEndObjectHandleExceptions(new JsonWriterDelegator(writer));
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00040913 File Offset: 0x0003EB13
		public override void WriteEndObject(XmlDictionaryWriter writer)
		{
			base.WriteEndObjectHandleExceptions(new JsonWriterDelegator(writer));
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00040924 File Offset: 0x0003EB24
		public override void WriteObject(Stream stream, object graph)
		{
			XmlObjectSerializer.CheckNull(stream, "stream");
			XmlDictionaryWriter xmlDictionaryWriter = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, false);
			this.WriteObject(xmlDictionaryWriter, graph);
			xmlDictionaryWriter.Flush();
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00040957 File Offset: 0x0003EB57
		public override void WriteObject(XmlWriter writer, object graph)
		{
			base.WriteObjectHandleExceptions(new JsonWriterDelegator(writer, this.DateTimeFormat), graph);
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0004096C File Offset: 0x0003EB6C
		public override void WriteObject(XmlDictionaryWriter writer, object graph)
		{
			base.WriteObjectHandleExceptions(new JsonWriterDelegator(writer, this.DateTimeFormat), graph);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00040981 File Offset: 0x0003EB81
		public override void WriteObjectContent(XmlWriter writer, object graph)
		{
			base.WriteObjectContentHandleExceptions(new JsonWriterDelegator(writer, this.DateTimeFormat), graph);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00040996 File Offset: 0x0003EB96
		public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
		{
			base.WriteObjectContentHandleExceptions(new JsonWriterDelegator(writer, this.DateTimeFormat), graph);
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x000409AB File Offset: 0x0003EBAB
		public override void WriteStartObject(XmlWriter writer, object graph)
		{
			base.WriteStartObjectHandleExceptions(new JsonWriterDelegator(writer), graph);
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x000409BA File Offset: 0x0003EBBA
		public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
		{
			base.WriteStartObjectHandleExceptions(new JsonWriterDelegator(writer), graph);
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x000409CC File Offset: 0x0003EBCC
		internal static bool CheckIfJsonNameRequiresMapping(string jsonName)
		{
			if (jsonName != null)
			{
				if (!DataContract.IsValidNCName(jsonName))
				{
					return true;
				}
				for (int i = 0; i < jsonName.Length; i++)
				{
					if (XmlJsonWriter.CharacterNeedsEscaping(jsonName[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x00040A08 File Offset: 0x0003EC08
		internal static bool CheckIfJsonNameRequiresMapping(XmlDictionaryString jsonName)
		{
			return jsonName != null && DataContractJsonSerializer.CheckIfJsonNameRequiresMapping(jsonName.Value);
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00040A1A File Offset: 0x0003EC1A
		internal static bool CheckIfXmlNameRequiresMapping(string xmlName)
		{
			return xmlName != null && DataContractJsonSerializer.CheckIfJsonNameRequiresMapping(DataContractJsonSerializer.ConvertXmlNameToJsonName(xmlName));
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00040A2C File Offset: 0x0003EC2C
		internal static bool CheckIfXmlNameRequiresMapping(XmlDictionaryString xmlName)
		{
			return xmlName != null && DataContractJsonSerializer.CheckIfXmlNameRequiresMapping(xmlName.Value);
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00040A3E File Offset: 0x0003EC3E
		internal static string ConvertXmlNameToJsonName(string xmlName)
		{
			return XmlConvert.DecodeName(xmlName);
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00040A46 File Offset: 0x0003EC46
		internal static XmlDictionaryString ConvertXmlNameToJsonName(XmlDictionaryString xmlName)
		{
			if (xmlName != null)
			{
				return new XmlDictionary().Add(DataContractJsonSerializer.ConvertXmlNameToJsonName(xmlName.Value));
			}
			return null;
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00040A64 File Offset: 0x0003EC64
		internal static bool IsJsonLocalName(XmlReaderDelegator reader, string elementName)
		{
			string text;
			return XmlObjectSerializerReadContextComplexJson.TryGetJsonLocalName(reader, out text) && elementName == text;
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00040A84 File Offset: 0x0003EC84
		internal static object ReadJsonValue(DataContract contract, XmlReaderDelegator reader, XmlObjectSerializerReadContextComplexJson context)
		{
			return JsonDataContract.GetJsonDataContract(contract).ReadJsonValue(reader, context);
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00040A93 File Offset: 0x0003EC93
		internal static void WriteJsonNull(XmlWriterDelegator writer)
		{
			writer.WriteAttributeString(null, "type", null, "null");
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x00040AA7 File Offset: 0x0003ECA7
		internal static void WriteJsonValue(JsonDataContract contract, XmlWriterDelegator writer, object graph, XmlObjectSerializerWriteContextComplexJson context, RuntimeTypeHandle declaredTypeHandle)
		{
			contract.WriteJsonValue(writer, graph, context, declaredTypeHandle);
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x00040AB4 File Offset: 0x0003ECB4
		internal override Type GetDeserializeType()
		{
			return this.rootType;
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00040ABC File Offset: 0x0003ECBC
		internal override Type GetSerializeType(object graph)
		{
			if (graph != null)
			{
				return graph.GetType();
			}
			return this.rootType;
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00040ACE File Offset: 0x0003ECCE
		internal override bool InternalIsStartObject(XmlReaderDelegator reader)
		{
			return base.IsRootElement(reader, this.RootContract, this.RootName, XmlDictionaryString.Empty) || DataContractJsonSerializer.IsJsonLocalName(reader, this.RootName.Value);
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x00040B00 File Offset: 0x0003ED00
		internal override object InternalReadObject(XmlReaderDelegator xmlReader, bool verifyObjectName)
		{
			if (this.MaxItemsInObjectGraph == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Maximum number of items that can be serialized or deserialized in an object graph is '{0}'.", new object[] { this.MaxItemsInObjectGraph })));
			}
			if (verifyObjectName)
			{
				if (!this.InternalIsStartObject(xmlReader))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationExceptionWithReaderDetails(SR.GetString("Expecting element '{1}' from namespace '{0}'.", new object[]
					{
						XmlDictionaryString.Empty,
						this.RootName
					}), xmlReader));
				}
			}
			else if (!base.IsStartElement(xmlReader))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationExceptionWithReaderDetails(SR.GetString("Expecting state '{0}' when ReadObject is called.", new object[] { XmlNodeType.Element }), xmlReader));
			}
			DataContract dataContract = this.RootContract;
			if (dataContract.IsPrimitive && dataContract.UnderlyingType == this.rootType)
			{
				return DataContractJsonSerializer.ReadJsonValue(dataContract, xmlReader, null);
			}
			return XmlObjectSerializerReadContextComplexJson.CreateContext(this, dataContract).InternalDeserialize(xmlReader, this.rootType, dataContract, null, null);
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00040BE1 File Offset: 0x0003EDE1
		internal override void InternalWriteEndObject(XmlWriterDelegator writer)
		{
			writer.WriteEndElement();
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00040BE9 File Offset: 0x0003EDE9
		internal override void InternalWriteObject(XmlWriterDelegator writer, object graph)
		{
			this.InternalWriteStartObject(writer, graph);
			this.InternalWriteObjectContent(writer, graph);
			this.InternalWriteEndObject(writer);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x00040C04 File Offset: 0x0003EE04
		internal override void InternalWriteObjectContent(XmlWriterDelegator writer, object graph)
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
			if (graph == null)
			{
				DataContractJsonSerializer.WriteJsonNull(writer);
				return;
			}
			if (underlyingType == type)
			{
				if (dataContract.CanContainReferences)
				{
					XmlObjectSerializerWriteContextComplexJson xmlObjectSerializerWriteContextComplexJson = XmlObjectSerializerWriteContextComplexJson.CreateContext(this, dataContract);
					xmlObjectSerializerWriteContextComplexJson.OnHandleReference(writer, graph, true);
					xmlObjectSerializerWriteContextComplexJson.SerializeWithoutXsiType(dataContract, writer, graph, underlyingType.TypeHandle);
					return;
				}
				DataContractJsonSerializer.WriteJsonValue(JsonDataContract.GetJsonDataContract(dataContract), writer, graph, null, underlyingType.TypeHandle);
				return;
			}
			else
			{
				XmlObjectSerializerWriteContextComplexJson xmlObjectSerializerWriteContextComplexJson2 = XmlObjectSerializerWriteContextComplexJson.CreateContext(this, this.RootContract);
				dataContract = DataContractJsonSerializer.GetDataContract(dataContract, underlyingType, type);
				if (dataContract.CanContainReferences)
				{
					xmlObjectSerializerWriteContextComplexJson2.OnHandleReference(writer, graph, true);
					xmlObjectSerializerWriteContextComplexJson2.SerializeWithXsiTypeAtTopLevel(dataContract, writer, graph, underlyingType.TypeHandle, type);
					return;
				}
				xmlObjectSerializerWriteContextComplexJson2.SerializeWithoutXsiType(dataContract, writer, graph, underlyingType.TypeHandle);
				return;
			}
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00040D10 File Offset: 0x0003EF10
		internal override void InternalWriteStartObject(XmlWriterDelegator writer, object graph)
		{
			if (this.rootNameRequiresMapping)
			{
				writer.WriteStartElement("a", "item", "item");
				writer.WriteAttributeString(null, "item", null, this.RootName.Value);
				return;
			}
			writer.WriteStartElement(this.RootName, XmlDictionaryString.Empty);
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00040D64 File Offset: 0x0003EF64
		private void AddCollectionItemTypeToKnownTypes(Type knownType)
		{
			Type type = knownType;
			Type type2;
			while (CollectionDataContract.IsCollection(type, out type2))
			{
				if (type2.IsGenericType && type2.GetGenericTypeDefinition() == Globals.TypeOfKeyValue)
				{
					type2 = Globals.TypeOfKeyValuePair.MakeGenericType(type2.GetGenericArguments());
				}
				this.knownTypeList.Add(type2);
				type = type2;
			}
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00040DB8 File Offset: 0x0003EFB8
		private void Initialize(Type type, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, IDataContractSurrogate dataContractSurrogate, EmitTypeInformation emitTypeInformation, bool serializeReadOnlyTypes, DateTimeFormat dateTimeFormat, bool useSimpleDictionaryFormat)
		{
			XmlObjectSerializer.CheckNull(type, "type");
			this.rootType = type;
			if (knownTypes != null)
			{
				this.knownTypeList = new List<Type>();
				foreach (Type type2 in knownTypes)
				{
					this.knownTypeList.Add(type2);
					if (type2 != null)
					{
						this.AddCollectionItemTypeToKnownTypes(type2);
					}
				}
			}
			if (maxItemsInObjectGraph < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("maxItemsInObjectGraph", SR.GetString("The value of this argument must be non-negative.")));
			}
			this.maxItemsInObjectGraph = maxItemsInObjectGraph;
			this.ignoreExtensionDataObject = ignoreExtensionDataObject;
			this.dataContractSurrogate = dataContractSurrogate;
			this.emitTypeInformation = emitTypeInformation;
			this.serializeReadOnlyTypes = serializeReadOnlyTypes;
			this.dateTimeFormat = dateTimeFormat;
			this.useSimpleDictionaryFormat = useSimpleDictionaryFormat;
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00040E8C File Offset: 0x0003F08C
		private void Initialize(Type type, XmlDictionaryString rootName, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, IDataContractSurrogate dataContractSurrogate, EmitTypeInformation emitTypeInformation, bool serializeReadOnlyTypes, DateTimeFormat dateTimeFormat, bool useSimpleDictionaryFormat)
		{
			this.Initialize(type, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, dataContractSurrogate, emitTypeInformation, serializeReadOnlyTypes, dateTimeFormat, useSimpleDictionaryFormat);
			this.rootName = DataContractJsonSerializer.ConvertXmlNameToJsonName(rootName);
			this.rootNameRequiresMapping = DataContractJsonSerializer.CheckIfJsonNameRequiresMapping(this.rootName);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00040ECC File Offset: 0x0003F0CC
		internal static void CheckIfTypeIsReference(DataContract dataContract)
		{
			if (dataContract.IsReference)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Unsupported value for IsReference for type '{0}', IsReference value is {1}.", new object[]
				{
					DataContract.GetClrTypeFullName(dataContract.UnderlyingType),
					dataContract.IsReference
				})));
			}
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00040F18 File Offset: 0x0003F118
		internal static DataContract GetDataContract(DataContract declaredTypeContract, Type declaredType, Type objectType)
		{
			DataContract dataContract = DataContractSerializer.GetDataContract(declaredTypeContract, declaredType, objectType);
			DataContractJsonSerializer.CheckIfTypeIsReference(dataContract);
			return dataContract;
		}

		// Token: 0x040007C4 RID: 1988
		internal IList<Type> knownTypeList;

		// Token: 0x040007C5 RID: 1989
		internal Dictionary<XmlQualifiedName, DataContract> knownDataContracts;

		// Token: 0x040007C6 RID: 1990
		private EmitTypeInformation emitTypeInformation;

		// Token: 0x040007C7 RID: 1991
		private IDataContractSurrogate dataContractSurrogate;

		// Token: 0x040007C8 RID: 1992
		private bool ignoreExtensionDataObject;

		// Token: 0x040007C9 RID: 1993
		private ReadOnlyCollection<Type> knownTypeCollection;

		// Token: 0x040007CA RID: 1994
		private int maxItemsInObjectGraph;

		// Token: 0x040007CB RID: 1995
		private DataContract rootContract;

		// Token: 0x040007CC RID: 1996
		private XmlDictionaryString rootName;

		// Token: 0x040007CD RID: 1997
		private bool rootNameRequiresMapping;

		// Token: 0x040007CE RID: 1998
		private Type rootType;

		// Token: 0x040007CF RID: 1999
		private bool serializeReadOnlyTypes;

		// Token: 0x040007D0 RID: 2000
		private DateTimeFormat dateTimeFormat;

		// Token: 0x040007D1 RID: 2001
		private bool useSimpleDictionaryFormat;
	}
}
