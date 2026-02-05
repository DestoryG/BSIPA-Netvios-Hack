using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Configuration;
using System.Runtime.Serialization.Formatters;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x0200009B RID: 155
	public sealed class NetDataContractSerializer : XmlObjectSerializer, IFormatter
	{
		// Token: 0x06000AB2 RID: 2738 RVA: 0x0002D4E0 File Offset: 0x0002B6E0
		public NetDataContractSerializer()
			: this(new StreamingContext(StreamingContextStates.All))
		{
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0002D4F2 File Offset: 0x0002B6F2
		public NetDataContractSerializer(StreamingContext context)
			: this(context, int.MaxValue, false, FormatterAssemblyStyle.Full, null)
		{
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0002D503 File Offset: 0x0002B703
		public NetDataContractSerializer(StreamingContext context, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, FormatterAssemblyStyle assemblyFormat, ISurrogateSelector surrogateSelector)
		{
			this.Initialize(context, maxItemsInObjectGraph, ignoreExtensionDataObject, assemblyFormat, surrogateSelector);
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0002D518 File Offset: 0x0002B718
		public NetDataContractSerializer(string rootName, string rootNamespace)
			: this(rootName, rootNamespace, new StreamingContext(StreamingContextStates.All), int.MaxValue, false, FormatterAssemblyStyle.Full, null)
		{
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0002D534 File Offset: 0x0002B734
		public NetDataContractSerializer(string rootName, string rootNamespace, StreamingContext context, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, FormatterAssemblyStyle assemblyFormat, ISurrogateSelector surrogateSelector)
		{
			XmlDictionary xmlDictionary = new XmlDictionary(2);
			this.Initialize(xmlDictionary.Add(rootName), xmlDictionary.Add(DataContract.GetNamespace(rootNamespace)), context, maxItemsInObjectGraph, ignoreExtensionDataObject, assemblyFormat, surrogateSelector);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0002D570 File Offset: 0x0002B770
		public NetDataContractSerializer(XmlDictionaryString rootName, XmlDictionaryString rootNamespace)
			: this(rootName, rootNamespace, new StreamingContext(StreamingContextStates.All), int.MaxValue, false, FormatterAssemblyStyle.Full, null)
		{
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0002D58C File Offset: 0x0002B78C
		public NetDataContractSerializer(XmlDictionaryString rootName, XmlDictionaryString rootNamespace, StreamingContext context, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, FormatterAssemblyStyle assemblyFormat, ISurrogateSelector surrogateSelector)
		{
			this.Initialize(rootName, rootNamespace, context, maxItemsInObjectGraph, ignoreExtensionDataObject, assemblyFormat, surrogateSelector);
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0002D5A8 File Offset: 0x0002B7A8
		private void Initialize(StreamingContext context, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, FormatterAssemblyStyle assemblyFormat, ISurrogateSelector surrogateSelector)
		{
			this.context = context;
			if (maxItemsInObjectGraph < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("maxItemsInObjectGraph", SR.GetString("The value of this argument must be non-negative.")));
			}
			this.maxItemsInObjectGraph = maxItemsInObjectGraph;
			this.ignoreExtensionDataObject = ignoreExtensionDataObject;
			this.surrogateSelector = surrogateSelector;
			this.AssemblyFormat = assemblyFormat;
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0002D5F8 File Offset: 0x0002B7F8
		private void Initialize(XmlDictionaryString rootName, XmlDictionaryString rootNamespace, StreamingContext context, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, FormatterAssemblyStyle assemblyFormat, ISurrogateSelector surrogateSelector)
		{
			this.Initialize(context, maxItemsInObjectGraph, ignoreExtensionDataObject, assemblyFormat, surrogateSelector);
			this.rootName = rootName;
			this.rootNamespace = rootNamespace;
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x0002D618 File Offset: 0x0002B818
		internal static bool UnsafeTypeForwardingEnabled
		{
			[SecuritySafeCritical]
			get
			{
				if (NetDataContractSerializer.unsafeTypeForwardingEnabled == null)
				{
					NetDataContractSerializerSection netDataContractSerializerSection;
					if (NetDataContractSerializerSection.TryUnsafeGetSection(out netDataContractSerializerSection))
					{
						NetDataContractSerializer.unsafeTypeForwardingEnabled = new bool?(netDataContractSerializerSection.EnableUnsafeTypeForwarding);
					}
					else
					{
						NetDataContractSerializer.unsafeTypeForwardingEnabled = new bool?(false);
					}
				}
				return NetDataContractSerializer.unsafeTypeForwardingEnabled.Value;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x0002D661 File Offset: 0x0002B861
		// (set) Token: 0x06000ABD RID: 2749 RVA: 0x0002D669 File Offset: 0x0002B869
		public StreamingContext Context
		{
			get
			{
				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x0002D672 File Offset: 0x0002B872
		// (set) Token: 0x06000ABF RID: 2751 RVA: 0x0002D67A File Offset: 0x0002B87A
		public SerializationBinder Binder
		{
			get
			{
				return this.binder;
			}
			set
			{
				this.binder = value;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x0002D683 File Offset: 0x0002B883
		// (set) Token: 0x06000AC1 RID: 2753 RVA: 0x0002D68B File Offset: 0x0002B88B
		public ISurrogateSelector SurrogateSelector
		{
			get
			{
				return this.surrogateSelector;
			}
			set
			{
				this.surrogateSelector = value;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x0002D694 File Offset: 0x0002B894
		// (set) Token: 0x06000AC3 RID: 2755 RVA: 0x0002D69C File Offset: 0x0002B89C
		public FormatterAssemblyStyle AssemblyFormat
		{
			get
			{
				return this.assemblyFormat;
			}
			set
			{
				if (value != FormatterAssemblyStyle.Full && value != FormatterAssemblyStyle.Simple)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("'{0}': invalid assembly format.", new object[] { value })));
				}
				this.assemblyFormat = value;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0002D6D0 File Offset: 0x0002B8D0
		public int MaxItemsInObjectGraph
		{
			get
			{
				return this.maxItemsInObjectGraph;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0002D6D8 File Offset: 0x0002B8D8
		public bool IgnoreExtensionDataObject
		{
			get
			{
				return this.ignoreExtensionDataObject;
			}
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0002D6E0 File Offset: 0x0002B8E0
		public void Serialize(Stream stream, object graph)
		{
			base.WriteObject(stream, graph);
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x0002D6EA File Offset: 0x0002B8EA
		public object Deserialize(Stream stream)
		{
			return base.ReadObject(stream);
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0002D6F4 File Offset: 0x0002B8F4
		internal override void InternalWriteObject(XmlWriterDelegator writer, object graph)
		{
			Hashtable hashtable = null;
			DataContract dataContract = this.GetDataContract(graph, ref hashtable);
			this.InternalWriteStartObject(writer, graph, dataContract);
			this.InternalWriteObjectContent(writer, graph, dataContract, hashtable);
			this.InternalWriteEndObject(writer);
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0002D727 File Offset: 0x0002B927
		public override void WriteObject(XmlWriter writer, object graph)
		{
			base.WriteObjectHandleExceptions(new XmlWriterDelegator(writer), graph);
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0002D736 File Offset: 0x0002B936
		public override void WriteStartObject(XmlWriter writer, object graph)
		{
			base.WriteStartObjectHandleExceptions(new XmlWriterDelegator(writer), graph);
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0002D745 File Offset: 0x0002B945
		public override void WriteObjectContent(XmlWriter writer, object graph)
		{
			base.WriteObjectContentHandleExceptions(new XmlWriterDelegator(writer), graph);
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0002D754 File Offset: 0x0002B954
		public override void WriteEndObject(XmlWriter writer)
		{
			base.WriteEndObjectHandleExceptions(new XmlWriterDelegator(writer));
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0002D762 File Offset: 0x0002B962
		public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
		{
			base.WriteStartObjectHandleExceptions(new XmlWriterDelegator(writer), graph);
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0002D774 File Offset: 0x0002B974
		internal override void InternalWriteStartObject(XmlWriterDelegator writer, object graph)
		{
			Hashtable hashtable = null;
			DataContract dataContract = this.GetDataContract(graph, ref hashtable);
			this.InternalWriteStartObject(writer, graph, dataContract);
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0002D798 File Offset: 0x0002B998
		private void InternalWriteStartObject(XmlWriterDelegator writer, object graph, DataContract contract)
		{
			base.WriteRootElement(writer, contract, this.rootName, this.rootNamespace, base.CheckIfNeedsContractNsAtRoot(this.rootName, this.rootNamespace, contract));
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0002D7CC File Offset: 0x0002B9CC
		public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
		{
			base.WriteObjectContentHandleExceptions(new XmlWriterDelegator(writer), graph);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0002D7DC File Offset: 0x0002B9DC
		internal override void InternalWriteObjectContent(XmlWriterDelegator writer, object graph)
		{
			Hashtable hashtable = null;
			DataContract dataContract = this.GetDataContract(graph, ref hashtable);
			this.InternalWriteObjectContent(writer, graph, dataContract, hashtable);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0002D800 File Offset: 0x0002BA00
		private void InternalWriteObjectContent(XmlWriterDelegator writer, object graph, DataContract contract, Hashtable surrogateDataContracts)
		{
			if (this.MaxItemsInObjectGraph == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Maximum number of items that can be serialized or deserialized in an object graph is '{0}'.", new object[] { this.MaxItemsInObjectGraph })));
			}
			if (base.IsRootXmlAny(this.rootName, contract))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("For type '{0}', IsAny is not supported by NetDataContractSerializer.", new object[] { contract.UnderlyingType })));
			}
			if (graph == null)
			{
				XmlObjectSerializer.WriteNull(writer);
				return;
			}
			Type type = graph.GetType();
			if (contract.UnderlyingType != type)
			{
				contract = this.GetDataContract(graph, ref surrogateDataContracts);
			}
			XmlObjectSerializerWriteContext xmlObjectSerializerWriteContext = null;
			if (contract.CanContainReferences)
			{
				xmlObjectSerializerWriteContext = XmlObjectSerializerWriteContext.CreateContext(this, surrogateDataContracts);
				xmlObjectSerializerWriteContext.HandleGraphAtTopLevel(writer, graph, contract);
			}
			NetDataContractSerializer.WriteClrTypeInfo(writer, contract, this.binder);
			contract.WriteXmlValue(writer, graph, xmlObjectSerializerWriteContext);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0002D8D0 File Offset: 0x0002BAD0
		internal static void WriteClrTypeInfo(XmlWriterDelegator writer, DataContract dataContract, SerializationBinder binder)
		{
			if (!dataContract.IsISerializable && !(dataContract is SurrogateDataContract))
			{
				TypeInformation typeInformation = null;
				Type originalUnderlyingType = dataContract.OriginalUnderlyingType;
				string text = null;
				string text2 = null;
				if (binder != null)
				{
					binder.BindToName(originalUnderlyingType, out text2, out text);
				}
				if (text == null)
				{
					typeInformation = NetDataContractSerializer.GetTypeInformation(originalUnderlyingType);
					text = typeInformation.FullTypeName;
				}
				if (text2 == null)
				{
					text2 = ((typeInformation == null) ? NetDataContractSerializer.GetTypeInformation(originalUnderlyingType).AssemblyString : typeInformation.AssemblyString);
					if (!NetDataContractSerializer.UnsafeTypeForwardingEnabled && !originalUnderlyingType.Assembly.IsFullyTrusted && !NetDataContractSerializer.IsAssemblyNameForwardingSafe(originalUnderlyingType.Assembly.FullName, text2))
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Type '{0}' in assembly '{1}' cannot be forwarded from assembly '{2}'.", new object[]
						{
							DataContract.GetClrTypeFullName(originalUnderlyingType),
							originalUnderlyingType.Assembly.FullName,
							text2
						})));
					}
				}
				NetDataContractSerializer.WriteClrTypeInfo(writer, text, text2);
			}
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0002D9A0 File Offset: 0x0002BBA0
		internal static void WriteClrTypeInfo(XmlWriterDelegator writer, Type dataContractType, SerializationBinder binder, string defaultClrTypeName, string defaultClrAssemblyName)
		{
			string text = null;
			string text2 = null;
			if (binder != null)
			{
				binder.BindToName(dataContractType, out text2, out text);
			}
			if (text == null)
			{
				text = defaultClrTypeName;
			}
			if (text2 == null)
			{
				text2 = defaultClrAssemblyName;
			}
			NetDataContractSerializer.WriteClrTypeInfo(writer, text, text2);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0002D9D4 File Offset: 0x0002BBD4
		internal static void WriteClrTypeInfo(XmlWriterDelegator writer, Type dataContractType, SerializationBinder binder, SerializationInfo serInfo)
		{
			TypeInformation typeInformation = null;
			string text = null;
			string text2 = null;
			if (binder != null)
			{
				binder.BindToName(dataContractType, out text2, out text);
			}
			if (text == null)
			{
				if (serInfo.IsFullTypeNameSetExplicit)
				{
					text = serInfo.FullTypeName;
				}
				else
				{
					typeInformation = NetDataContractSerializer.GetTypeInformation(serInfo.ObjectType);
					text = typeInformation.FullTypeName;
				}
			}
			if (text2 == null)
			{
				if (serInfo.IsAssemblyNameSetExplicit)
				{
					text2 = serInfo.AssemblyName;
				}
				else
				{
					text2 = ((typeInformation == null) ? NetDataContractSerializer.GetTypeInformation(serInfo.ObjectType).AssemblyString : typeInformation.AssemblyString);
				}
			}
			NetDataContractSerializer.WriteClrTypeInfo(writer, text, text2);
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0002DA54 File Offset: 0x0002BC54
		private static void WriteClrTypeInfo(XmlWriterDelegator writer, string clrTypeName, string clrAssemblyName)
		{
			if (clrTypeName != null)
			{
				writer.WriteAttributeString("z", DictionaryGlobals.ClrTypeLocalName, DictionaryGlobals.SerializationNamespace, DataContract.GetClrTypeString(clrTypeName));
			}
			if (clrAssemblyName != null)
			{
				writer.WriteAttributeString("z", DictionaryGlobals.ClrAssemblyLocalName, DictionaryGlobals.SerializationNamespace, DataContract.GetClrTypeString(clrAssemblyName));
			}
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0002DA92 File Offset: 0x0002BC92
		public override void WriteEndObject(XmlDictionaryWriter writer)
		{
			base.WriteEndObjectHandleExceptions(new XmlWriterDelegator(writer));
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0002DAA0 File Offset: 0x0002BCA0
		internal override void InternalWriteEndObject(XmlWriterDelegator writer)
		{
			writer.WriteEndElement();
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0002DAA8 File Offset: 0x0002BCA8
		public override object ReadObject(XmlReader reader)
		{
			return base.ReadObjectHandleExceptions(new XmlReaderDelegator(reader), true);
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0002DAB7 File Offset: 0x0002BCB7
		public override object ReadObject(XmlReader reader, bool verifyObjectName)
		{
			return base.ReadObjectHandleExceptions(new XmlReaderDelegator(reader), verifyObjectName);
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0002DAC6 File Offset: 0x0002BCC6
		public override bool IsStartObject(XmlReader reader)
		{
			return base.IsStartObjectHandleExceptions(new XmlReaderDelegator(reader));
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0002DAD4 File Offset: 0x0002BCD4
		public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
		{
			return base.ReadObjectHandleExceptions(new XmlReaderDelegator(reader), verifyObjectName);
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0002DAE3 File Offset: 0x0002BCE3
		public override bool IsStartObject(XmlDictionaryReader reader)
		{
			return base.IsStartObjectHandleExceptions(new XmlReaderDelegator(reader));
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0002DAF4 File Offset: 0x0002BCF4
		internal override object InternalReadObject(XmlReaderDelegator xmlReader, bool verifyObjectName)
		{
			if (this.MaxItemsInObjectGraph == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Maximum number of items that can be serialized or deserialized in an object graph is '{0}'.", new object[] { this.MaxItemsInObjectGraph })));
			}
			if (!base.IsStartElement(xmlReader))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationExceptionWithReaderDetails(SR.GetString("Expecting state '{0}' when ReadObject is called.", new object[] { XmlNodeType.Element }), xmlReader));
			}
			return XmlObjectSerializerReadContext.CreateContext(this).InternalDeserialize(xmlReader, null, null, null);
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002DB6F File Offset: 0x0002BD6F
		internal override bool InternalIsStartObject(XmlReaderDelegator reader)
		{
			return base.IsStartElement(reader);
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0002DB78 File Offset: 0x0002BD78
		internal DataContract GetDataContract(object obj, ref Hashtable surrogateDataContracts)
		{
			return this.GetDataContract((obj == null) ? Globals.TypeOfObject : obj.GetType(), ref surrogateDataContracts);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0002DB91 File Offset: 0x0002BD91
		internal DataContract GetDataContract(Type type, ref Hashtable surrogateDataContracts)
		{
			return this.GetDataContract(type.TypeHandle, type, ref surrogateDataContracts);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0002DBA4 File Offset: 0x0002BDA4
		internal DataContract GetDataContract(RuntimeTypeHandle typeHandle, Type type, ref Hashtable surrogateDataContracts)
		{
			DataContract dataContract = NetDataContractSerializer.GetDataContractFromSurrogateSelector(this.surrogateSelector, this.Context, typeHandle, type, ref surrogateDataContracts);
			if (dataContract != null)
			{
				return dataContract;
			}
			if (this.cachedDataContract == null)
			{
				dataContract = DataContract.GetDataContract(typeHandle, type, SerializationMode.SharedType);
				this.cachedDataContract = dataContract;
				return dataContract;
			}
			DataContract dataContract2 = this.cachedDataContract;
			if (dataContract2.UnderlyingType.TypeHandle.Equals(typeHandle))
			{
				return dataContract2;
			}
			return DataContract.GetDataContract(typeHandle, type, SerializationMode.SharedType);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0002DC0C File Offset: 0x0002BE0C
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static ISerializationSurrogate GetSurrogate(Type type, ISurrogateSelector surrogateSelector, StreamingContext context)
		{
			ISurrogateSelector surrogateSelector2;
			return surrogateSelector.GetSurrogate(type, context, out surrogateSelector2);
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0002DC24 File Offset: 0x0002BE24
		internal static DataContract GetDataContractFromSurrogateSelector(ISurrogateSelector surrogateSelector, StreamingContext context, RuntimeTypeHandle typeHandle, Type type, ref Hashtable surrogateDataContracts)
		{
			if (surrogateSelector == null)
			{
				return null;
			}
			if (type == null)
			{
				type = Type.GetTypeFromHandle(typeHandle);
			}
			DataContract builtInDataContract = DataContract.GetBuiltInDataContract(type);
			if (builtInDataContract != null)
			{
				return builtInDataContract;
			}
			if (surrogateDataContracts != null)
			{
				DataContract dataContract = (DataContract)surrogateDataContracts[type];
				if (dataContract != null)
				{
					return dataContract;
				}
			}
			DataContract dataContract2 = null;
			ISerializationSurrogate surrogate = NetDataContractSerializer.GetSurrogate(type, surrogateSelector, context);
			if (surrogate != null)
			{
				dataContract2 = new SurrogateDataContract(type, surrogate);
			}
			else if (type.IsArray)
			{
				Type elementType = type.GetElementType();
				DataContract dataContract3 = NetDataContractSerializer.GetDataContractFromSurrogateSelector(surrogateSelector, context, elementType.TypeHandle, elementType, ref surrogateDataContracts);
				if (dataContract3 == null)
				{
					dataContract3 = DataContract.GetDataContract(elementType.TypeHandle, elementType, SerializationMode.SharedType);
				}
				dataContract2 = new CollectionDataContract(type, dataContract3);
			}
			if (dataContract2 != null)
			{
				if (surrogateDataContracts == null)
				{
					surrogateDataContracts = new Hashtable();
				}
				surrogateDataContracts.Add(type, dataContract2);
				return dataContract2;
			}
			return null;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002DCE4 File Offset: 0x0002BEE4
		internal static TypeInformation GetTypeInformation(Type type)
		{
			TypeInformation typeInformation = null;
			object obj = NetDataContractSerializer.typeNameCache[type];
			if (obj == null)
			{
				bool flag;
				string clrAssemblyName = DataContract.GetClrAssemblyName(type, out flag);
				typeInformation = new TypeInformation(DataContract.GetClrTypeFullNameUsingTypeForwardedFromAttribute(type), clrAssemblyName, flag);
				Hashtable hashtable = NetDataContractSerializer.typeNameCache;
				lock (hashtable)
				{
					NetDataContractSerializer.typeNameCache[type] = typeInformation;
					return typeInformation;
				}
			}
			typeInformation = (TypeInformation)obj;
			return typeInformation;
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0002DD60 File Offset: 0x0002BF60
		private static bool IsAssemblyNameForwardingSafe(string originalAssemblyName, string newAssemblyName)
		{
			if (originalAssemblyName == newAssemblyName)
			{
				return true;
			}
			AssemblyName assemblyName = new AssemblyName(originalAssemblyName);
			AssemblyName assemblyName2 = new AssemblyName(newAssemblyName);
			return !string.Equals(assemblyName2.Name, "mscorlib", StringComparison.OrdinalIgnoreCase) && !string.Equals(assemblyName2.Name, "mscorlib.dll", StringComparison.OrdinalIgnoreCase) && NetDataContractSerializer.IsPublicKeyTokenForwardingSafe(assemblyName.GetPublicKeyToken(), assemblyName2.GetPublicKeyToken());
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0002DDC0 File Offset: 0x0002BFC0
		private static bool IsPublicKeyTokenForwardingSafe(byte[] sourceToken, byte[] destinationToken)
		{
			if (sourceToken == null || destinationToken == null || sourceToken.Length == 0 || destinationToken.Length == 0 || sourceToken.Length != destinationToken.Length)
			{
				return false;
			}
			for (int i = 0; i < sourceToken.Length; i++)
			{
				if (sourceToken[i] != destinationToken[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040004BD RID: 1213
		private XmlDictionaryString rootName;

		// Token: 0x040004BE RID: 1214
		private XmlDictionaryString rootNamespace;

		// Token: 0x040004BF RID: 1215
		private StreamingContext context;

		// Token: 0x040004C0 RID: 1216
		private SerializationBinder binder;

		// Token: 0x040004C1 RID: 1217
		private ISurrogateSelector surrogateSelector;

		// Token: 0x040004C2 RID: 1218
		private int maxItemsInObjectGraph;

		// Token: 0x040004C3 RID: 1219
		private bool ignoreExtensionDataObject;

		// Token: 0x040004C4 RID: 1220
		private FormatterAssemblyStyle assemblyFormat;

		// Token: 0x040004C5 RID: 1221
		private DataContract cachedDataContract;

		// Token: 0x040004C6 RID: 1222
		private static Hashtable typeNameCache = new Hashtable();

		// Token: 0x040004C7 RID: 1223
		private static bool? unsafeTypeForwardingEnabled;
	}
}
