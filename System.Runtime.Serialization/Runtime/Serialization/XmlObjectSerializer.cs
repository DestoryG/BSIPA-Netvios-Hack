using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Diagnostics;
using System.Runtime.Serialization.Diagnostics;
using System.Security;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000E5 RID: 229
	public abstract class XmlObjectSerializer
	{
		// Token: 0x06000CB8 RID: 3256
		public abstract void WriteStartObject(XmlDictionaryWriter writer, object graph);

		// Token: 0x06000CB9 RID: 3257
		public abstract void WriteObjectContent(XmlDictionaryWriter writer, object graph);

		// Token: 0x06000CBA RID: 3258
		public abstract void WriteEndObject(XmlDictionaryWriter writer);

		// Token: 0x06000CBB RID: 3259 RVA: 0x00035D00 File Offset: 0x00033F00
		public virtual void WriteObject(Stream stream, object graph)
		{
			XmlObjectSerializer.CheckNull(stream, "stream");
			XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, false);
			this.WriteObject(xmlDictionaryWriter, graph);
			xmlDictionaryWriter.Flush();
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00035D33 File Offset: 0x00033F33
		public virtual void WriteObject(XmlWriter writer, object graph)
		{
			XmlObjectSerializer.CheckNull(writer, "writer");
			this.WriteObject(XmlDictionaryWriter.CreateDictionaryWriter(writer), graph);
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00035D4D File Offset: 0x00033F4D
		public virtual void WriteStartObject(XmlWriter writer, object graph)
		{
			XmlObjectSerializer.CheckNull(writer, "writer");
			this.WriteStartObject(XmlDictionaryWriter.CreateDictionaryWriter(writer), graph);
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00035D67 File Offset: 0x00033F67
		public virtual void WriteObjectContent(XmlWriter writer, object graph)
		{
			XmlObjectSerializer.CheckNull(writer, "writer");
			this.WriteObjectContent(XmlDictionaryWriter.CreateDictionaryWriter(writer), graph);
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00035D81 File Offset: 0x00033F81
		public virtual void WriteEndObject(XmlWriter writer)
		{
			XmlObjectSerializer.CheckNull(writer, "writer");
			this.WriteEndObject(XmlDictionaryWriter.CreateDictionaryWriter(writer));
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00035D9A File Offset: 0x00033F9A
		public virtual void WriteObject(XmlDictionaryWriter writer, object graph)
		{
			this.WriteObjectHandleExceptions(new XmlWriterDelegator(writer), graph);
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00035DA9 File Offset: 0x00033FA9
		internal void WriteObjectHandleExceptions(XmlWriterDelegator writer, object graph)
		{
			this.WriteObjectHandleExceptions(writer, graph, null);
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00035DB4 File Offset: 0x00033FB4
		internal void WriteObjectHandleExceptions(XmlWriterDelegator writer, object graph, DataContractResolver dataContractResolver)
		{
			try
			{
				XmlObjectSerializer.CheckNull(writer, "writer");
				if (DiagnosticUtility.ShouldTraceInformation)
				{
					TraceUtility.Trace(TraceEventType.Information, 196609, SR.GetString("WriteObject begins"), new StringTraceRecord("Type", XmlObjectSerializer.GetTypeInfo(this.GetSerializeType(graph))));
					this.InternalWriteObject(writer, graph, dataContractResolver);
					TraceUtility.Trace(TraceEventType.Information, 196610, SR.GetString("WriteObject ends"), new StringTraceRecord("Type", XmlObjectSerializer.GetTypeInfo(this.GetSerializeType(graph))));
				}
				else
				{
					this.InternalWriteObject(writer, graph, dataContractResolver);
				}
			}
			catch (XmlException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("There was an error serializing the object {0}. {1}", this.GetSerializeType(graph), ex), ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("There was an error serializing the object {0}. {1}", this.GetSerializeType(graph), ex2), ex2));
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x00035E9C File Offset: 0x0003409C
		internal virtual Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x00035E9F File Offset: 0x0003409F
		internal virtual void InternalWriteObject(XmlWriterDelegator writer, object graph)
		{
			this.WriteStartObject(writer.Writer, graph);
			this.WriteObjectContent(writer.Writer, graph);
			this.WriteEndObject(writer.Writer);
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00035EC7 File Offset: 0x000340C7
		internal virtual void InternalWriteObject(XmlWriterDelegator writer, object graph, DataContractResolver dataContractResolver)
		{
			this.InternalWriteObject(writer, graph);
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00035ED1 File Offset: 0x000340D1
		internal virtual void InternalWriteStartObject(XmlWriterDelegator writer, object graph)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00035EDD File Offset: 0x000340DD
		internal virtual void InternalWriteObjectContent(XmlWriterDelegator writer, object graph)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00035EE9 File Offset: 0x000340E9
		internal virtual void InternalWriteEndObject(XmlWriterDelegator writer)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00035EF8 File Offset: 0x000340F8
		internal void WriteStartObjectHandleExceptions(XmlWriterDelegator writer, object graph)
		{
			try
			{
				XmlObjectSerializer.CheckNull(writer, "writer");
				this.InternalWriteStartObject(writer, graph);
			}
			catch (XmlException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("There was an error writing start element of object {0}. {1}", this.GetSerializeType(graph), ex), ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("There was an error writing start element of object {0}. {1}", this.GetSerializeType(graph), ex2), ex2));
			}
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00035F74 File Offset: 0x00034174
		internal void WriteObjectContentHandleExceptions(XmlWriterDelegator writer, object graph)
		{
			try
			{
				XmlObjectSerializer.CheckNull(writer, "writer");
				if (DiagnosticUtility.ShouldTraceInformation)
				{
					TraceUtility.Trace(TraceEventType.Information, 196611, SR.GetString("WriteObjectContent begins"), new StringTraceRecord("Type", XmlObjectSerializer.GetTypeInfo(this.GetSerializeType(graph))));
					if (writer.WriteState != WriteState.Element)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("WriteState '{0}' not valid. Caller must write start element before serializing in contentOnly mode.", new object[] { writer.WriteState })));
					}
					this.InternalWriteObjectContent(writer, graph);
					TraceUtility.Trace(TraceEventType.Information, 196612, SR.GetString("WriteObjectContent ends"), new StringTraceRecord("Type", XmlObjectSerializer.GetTypeInfo(this.GetSerializeType(graph))));
				}
				else
				{
					if (writer.WriteState != WriteState.Element)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("WriteState '{0}' not valid. Caller must write start element before serializing in contentOnly mode.", new object[] { writer.WriteState })));
					}
					this.InternalWriteObjectContent(writer, graph);
				}
			}
			catch (XmlException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("There was an error serializing the object {0}. {1}", this.GetSerializeType(graph), ex), ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("There was an error serializing the object {0}. {1}", this.GetSerializeType(graph), ex2), ex2));
			}
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x000360C0 File Offset: 0x000342C0
		internal void WriteEndObjectHandleExceptions(XmlWriterDelegator writer)
		{
			try
			{
				XmlObjectSerializer.CheckNull(writer, "writer");
				this.InternalWriteEndObject(writer);
			}
			catch (XmlException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("There was an error writing end element of object {0}. {1}", null, ex), ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("There was an error writing end element of object {0}. {1}", null, ex2), ex2));
			}
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x00036130 File Offset: 0x00034330
		internal void WriteRootElement(XmlWriterDelegator writer, DataContract contract, XmlDictionaryString name, XmlDictionaryString ns, bool needsContractNsAtRoot)
		{
			if (name != null)
			{
				contract.WriteRootElement(writer, name, ns);
				if (needsContractNsAtRoot)
				{
					writer.WriteNamespaceDecl(contract.Namespace);
				}
				return;
			}
			if (!contract.HasRoot)
			{
				return;
			}
			contract.WriteRootElement(writer, contract.TopLevelElementName, contract.TopLevelElementNamespace);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0003616C File Offset: 0x0003436C
		internal bool CheckIfNeedsContractNsAtRoot(XmlDictionaryString name, XmlDictionaryString ns, DataContract contract)
		{
			if (name == null)
			{
				return false;
			}
			if (contract.IsBuiltInDataContract || !contract.CanContainReferences || contract.IsISerializable)
			{
				return false;
			}
			string @string = XmlDictionaryString.GetString(contract.Namespace);
			return !string.IsNullOrEmpty(@string) && !(@string == XmlDictionaryString.GetString(ns));
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x000361BD File Offset: 0x000343BD
		internal static void WriteNull(XmlWriterDelegator writer)
		{
			writer.WriteAttributeBool("i", DictionaryGlobals.XsiNilLocalName, DictionaryGlobals.SchemaInstanceNamespace, true);
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x000361D8 File Offset: 0x000343D8
		internal static bool IsContractDeclared(DataContract contract, DataContract declaredContract)
		{
			return (contract.Name == declaredContract.Name && contract.Namespace == declaredContract.Namespace) || (contract.Name.Value == declaredContract.Name.Value && contract.Namespace.Value == declaredContract.Namespace.Value);
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0003623D File Offset: 0x0003443D
		public virtual object ReadObject(Stream stream)
		{
			XmlObjectSerializer.CheckNull(stream, "stream");
			return this.ReadObject(XmlDictionaryReader.CreateTextReader(stream, XmlDictionaryReaderQuotas.Max));
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x0003625B File Offset: 0x0003445B
		public virtual object ReadObject(XmlReader reader)
		{
			XmlObjectSerializer.CheckNull(reader, "reader");
			return this.ReadObject(XmlDictionaryReader.CreateDictionaryReader(reader));
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00036274 File Offset: 0x00034474
		public virtual object ReadObject(XmlDictionaryReader reader)
		{
			return this.ReadObjectHandleExceptions(new XmlReaderDelegator(reader), true);
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00036283 File Offset: 0x00034483
		public virtual object ReadObject(XmlReader reader, bool verifyObjectName)
		{
			XmlObjectSerializer.CheckNull(reader, "reader");
			return this.ReadObject(XmlDictionaryReader.CreateDictionaryReader(reader), verifyObjectName);
		}

		// Token: 0x06000CD4 RID: 3284
		public abstract object ReadObject(XmlDictionaryReader reader, bool verifyObjectName);

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0003629D File Offset: 0x0003449D
		public virtual bool IsStartObject(XmlReader reader)
		{
			XmlObjectSerializer.CheckNull(reader, "reader");
			return this.IsStartObject(XmlDictionaryReader.CreateDictionaryReader(reader));
		}

		// Token: 0x06000CD6 RID: 3286
		public abstract bool IsStartObject(XmlDictionaryReader reader);

		// Token: 0x06000CD7 RID: 3287 RVA: 0x000362B6 File Offset: 0x000344B6
		internal virtual object InternalReadObject(XmlReaderDelegator reader, bool verifyObjectName)
		{
			return this.ReadObject(reader.UnderlyingReader, verifyObjectName);
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x000362C5 File Offset: 0x000344C5
		internal virtual object InternalReadObject(XmlReaderDelegator reader, bool verifyObjectName, DataContractResolver dataContractResolver)
		{
			return this.InternalReadObject(reader, verifyObjectName);
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x000362CF File Offset: 0x000344CF
		internal virtual bool InternalIsStartObject(XmlReaderDelegator reader)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x000362DB File Offset: 0x000344DB
		internal object ReadObjectHandleExceptions(XmlReaderDelegator reader, bool verifyObjectName)
		{
			return this.ReadObjectHandleExceptions(reader, verifyObjectName, null);
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x000362E8 File Offset: 0x000344E8
		internal object ReadObjectHandleExceptions(XmlReaderDelegator reader, bool verifyObjectName, DataContractResolver dataContractResolver)
		{
			object obj2;
			try
			{
				XmlObjectSerializer.CheckNull(reader, "reader");
				if (DiagnosticUtility.ShouldTraceInformation)
				{
					TraceUtility.Trace(TraceEventType.Information, 196613, SR.GetString("ReadObject begins"), new StringTraceRecord("Type", XmlObjectSerializer.GetTypeInfo(this.GetDeserializeType())));
					object obj = this.InternalReadObject(reader, verifyObjectName, dataContractResolver);
					TraceUtility.Trace(TraceEventType.Information, 196614, SR.GetString("ReadObject ends"), new StringTraceRecord("Type", XmlObjectSerializer.GetTypeInfo(this.GetDeserializeType())));
					obj2 = obj;
				}
				else
				{
					obj2 = this.InternalReadObject(reader, verifyObjectName, dataContractResolver);
				}
			}
			catch (XmlException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("There was an error deserializing the object {0}. {1}", this.GetDeserializeType(), ex), ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("There was an error deserializing the object {0}. {1}", this.GetDeserializeType(), ex2), ex2));
			}
			return obj2;
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x000363CC File Offset: 0x000345CC
		internal bool IsStartObjectHandleExceptions(XmlReaderDelegator reader)
		{
			bool flag;
			try
			{
				XmlObjectSerializer.CheckNull(reader, "reader");
				flag = this.InternalIsStartObject(reader);
			}
			catch (XmlException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("There was an error checking start element of object {0}. {1}", this.GetDeserializeType(), ex), ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("There was an error checking start element of object {0}. {1}", this.GetDeserializeType(), ex2), ex2));
			}
			return flag;
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x00036448 File Offset: 0x00034648
		internal bool IsRootXmlAny(XmlDictionaryString rootName, DataContract contract)
		{
			return rootName == null && !contract.HasRoot;
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x00036458 File Offset: 0x00034658
		internal bool IsStartElement(XmlReaderDelegator reader)
		{
			return reader.MoveToElement() || reader.IsStartElement();
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0003646C File Offset: 0x0003466C
		internal bool IsRootElement(XmlReaderDelegator reader, DataContract contract, XmlDictionaryString name, XmlDictionaryString ns)
		{
			reader.MoveToElement();
			if (name != null)
			{
				return reader.IsStartElement(name, ns);
			}
			if (!contract.HasRoot)
			{
				return reader.IsStartElement();
			}
			if (reader.IsStartElement(contract.TopLevelElementName, contract.TopLevelElementNamespace))
			{
				return true;
			}
			ClassDataContract classDataContract = contract as ClassDataContract;
			if (classDataContract != null)
			{
				classDataContract = classDataContract.BaseContract;
			}
			while (classDataContract != null)
			{
				if (reader.IsStartElement(classDataContract.TopLevelElementName, classDataContract.TopLevelElementNamespace))
				{
					return true;
				}
				classDataContract = classDataContract.BaseContract;
			}
			if (classDataContract == null)
			{
				DataContract primitiveDataContract = PrimitiveDataContract.GetPrimitiveDataContract(Globals.TypeOfObject);
				if (reader.IsStartElement(primitiveDataContract.TopLevelElementName, primitiveDataContract.TopLevelElementNamespace))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0003650A File Offset: 0x0003470A
		internal static void CheckNull(object obj, string name)
		{
			if (obj == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException(name));
			}
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0003651C File Offset: 0x0003471C
		internal static string TryAddLineInfo(XmlReaderDelegator reader, string errorMessage)
		{
			if (reader.HasLineInfo())
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} {1}", SR.GetString("Error in line {0} position {1}.", new object[] { reader.LineNumber, reader.LinePosition }), errorMessage);
			}
			return errorMessage;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00036570 File Offset: 0x00034770
		internal static Exception CreateSerializationExceptionWithReaderDetails(string errorMessage, XmlReaderDelegator reader)
		{
			return XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.TryAddLineInfo(reader, SR.GetString("{0}. Encountered '{1}'  with name '{2}', namespace '{3}'.", new object[] { errorMessage, reader.NodeType, reader.LocalName, reader.NamespaceURI })));
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x000365BC File Offset: 0x000347BC
		internal static SerializationException CreateSerializationException(string errorMessage)
		{
			return XmlObjectSerializer.CreateSerializationException(errorMessage, null);
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x000365C5 File Offset: 0x000347C5
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static SerializationException CreateSerializationException(string errorMessage, Exception innerException)
		{
			return new SerializationException(errorMessage, innerException);
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x000365CE File Offset: 0x000347CE
		private static string GetTypeInfo(Type type)
		{
			if (!(type == null))
			{
				return DataContract.GetClrTypeFullName(type);
			}
			return string.Empty;
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x000365E8 File Offset: 0x000347E8
		private static string GetTypeInfoError(string errorMessage, Type type, Exception innerException)
		{
			string text = ((type == null) ? string.Empty : SR.GetString("of type {0}", new object[] { DataContract.GetClrTypeFullName(type) }));
			string text2 = ((innerException == null) ? string.Empty : innerException.Message);
			return SR.GetString(errorMessage, new object[] { text, text2 });
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00036644 File Offset: 0x00034844
		internal virtual Type GetSerializeType(object graph)
		{
			if (graph != null)
			{
				return graph.GetType();
			}
			return null;
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x00036651 File Offset: 0x00034851
		internal virtual Type GetDeserializeType()
		{
			return null;
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00036654 File Offset: 0x00034854
		internal static IFormatterConverter FormatterConverter
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlObjectSerializer.formatterConverter == null)
				{
					XmlObjectSerializer.formatterConverter = new FormatterConverter();
				}
				return XmlObjectSerializer.formatterConverter;
			}
		}

		// Token: 0x0400054E RID: 1358
		[SecurityCritical]
		private static IFormatterConverter formatterConverter;
	}
}
