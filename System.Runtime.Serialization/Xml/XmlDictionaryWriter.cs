using System;
using System.Globalization;
using System.IO;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace System.Xml
{
	// Token: 0x0200003A RID: 58
	public abstract class XmlDictionaryWriter : XmlWriter
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x00015EDF File Offset: 0x000140DF
		internal virtual bool FastAsync
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00015EE2 File Offset: 0x000140E2
		internal virtual AsyncCompletionResult WriteBase64Async(AsyncEventArgs<XmlWriteBase64AsyncArguments> state)
		{
			throw FxTrace.Exception.AsError(new NotSupportedException());
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00015EF3 File Offset: 0x000140F3
		public override Task WriteBase64Async(byte[] buffer, int index, int count)
		{
			return Task.Factory.FromAsync<byte[], int, int>(new Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(this.BeginWriteBase64), new Action<IAsyncResult>(this.EndWriteBase64), buffer, index, count, null);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00015F1D File Offset: 0x0001411D
		internal virtual IAsyncResult BeginWriteBase64(byte[] buffer, int index, int count, AsyncCallback callback, object state)
		{
			return new XmlDictionaryWriter.WriteBase64AsyncResult(buffer, index, count, this, callback, state);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00015F2C File Offset: 0x0001412C
		internal virtual void EndWriteBase64(IAsyncResult result)
		{
			ScheduleActionItemAsyncResult.End(result);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00015F34 File Offset: 0x00014134
		public static XmlDictionaryWriter CreateBinaryWriter(Stream stream)
		{
			return XmlDictionaryWriter.CreateBinaryWriter(stream, null);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00015F3D File Offset: 0x0001413D
		public static XmlDictionaryWriter CreateBinaryWriter(Stream stream, IXmlDictionary dictionary)
		{
			return XmlDictionaryWriter.CreateBinaryWriter(stream, dictionary, null);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00015F47 File Offset: 0x00014147
		public static XmlDictionaryWriter CreateBinaryWriter(Stream stream, IXmlDictionary dictionary, XmlBinaryWriterSession session)
		{
			return XmlDictionaryWriter.CreateBinaryWriter(stream, dictionary, session, true);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00015F52 File Offset: 0x00014152
		public static XmlDictionaryWriter CreateBinaryWriter(Stream stream, IXmlDictionary dictionary, XmlBinaryWriterSession session, bool ownsStream)
		{
			XmlBinaryWriter xmlBinaryWriter = new XmlBinaryWriter();
			xmlBinaryWriter.SetOutput(stream, dictionary, session, ownsStream);
			return xmlBinaryWriter;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00015F63 File Offset: 0x00014163
		public static XmlDictionaryWriter CreateTextWriter(Stream stream)
		{
			return XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, true);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00015F71 File Offset: 0x00014171
		public static XmlDictionaryWriter CreateTextWriter(Stream stream, Encoding encoding)
		{
			return XmlDictionaryWriter.CreateTextWriter(stream, encoding, true);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00015F7B File Offset: 0x0001417B
		public static XmlDictionaryWriter CreateTextWriter(Stream stream, Encoding encoding, bool ownsStream)
		{
			XmlUTF8TextWriter xmlUTF8TextWriter = new XmlUTF8TextWriter();
			xmlUTF8TextWriter.SetOutput(stream, encoding, ownsStream);
			return xmlUTF8TextWriter;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00015F8B File Offset: 0x0001418B
		public static XmlDictionaryWriter CreateMtomWriter(Stream stream, Encoding encoding, int maxSizeInBytes, string startInfo)
		{
			return XmlDictionaryWriter.CreateMtomWriter(stream, encoding, maxSizeInBytes, startInfo, null, null, true, true);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00015F9C File Offset: 0x0001419C
		public static XmlDictionaryWriter CreateMtomWriter(Stream stream, Encoding encoding, int maxSizeInBytes, string startInfo, string boundary, string startUri, bool writeMessageHeaders, bool ownsStream)
		{
			XmlMtomWriter xmlMtomWriter = new XmlMtomWriter();
			xmlMtomWriter.SetOutput(stream, encoding, maxSizeInBytes, startInfo, boundary, startUri, writeMessageHeaders, ownsStream);
			return xmlMtomWriter;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00015FC0 File Offset: 0x000141C0
		public static XmlDictionaryWriter CreateDictionaryWriter(XmlWriter writer)
		{
			if (writer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("writer");
			}
			XmlDictionaryWriter xmlDictionaryWriter = writer as XmlDictionaryWriter;
			if (xmlDictionaryWriter == null)
			{
				xmlDictionaryWriter = new XmlDictionaryWriter.XmlWrappedWriter(writer);
			}
			return xmlDictionaryWriter;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00015FED File Offset: 0x000141ED
		public void WriteStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			this.WriteStartElement(null, localName, namespaceUri);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00015FF8 File Offset: 0x000141F8
		public virtual void WriteStartElement(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			this.WriteStartElement(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri));
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0001600D File Offset: 0x0001420D
		public void WriteStartAttribute(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			this.WriteStartAttribute(null, localName, namespaceUri);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00016018 File Offset: 0x00014218
		public virtual void WriteStartAttribute(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			this.WriteStartAttribute(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri));
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001602D File Offset: 0x0001422D
		public void WriteAttributeString(XmlDictionaryString localName, XmlDictionaryString namespaceUri, string value)
		{
			this.WriteAttributeString(null, localName, namespaceUri, value);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001603C File Offset: 0x0001423C
		public virtual void WriteXmlnsAttribute(string prefix, string namespaceUri)
		{
			if (namespaceUri == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
			}
			if (prefix == null)
			{
				if (this.LookupPrefix(namespaceUri) != null)
				{
					return;
				}
				prefix = ((namespaceUri.Length == 0) ? string.Empty : ("d" + namespaceUri.Length.ToString(NumberFormatInfo.InvariantInfo)));
			}
			base.WriteAttributeString("xmlns", prefix, null, namespaceUri);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000160A0 File Offset: 0x000142A0
		public virtual void WriteXmlnsAttribute(string prefix, XmlDictionaryString namespaceUri)
		{
			this.WriteXmlnsAttribute(prefix, XmlDictionaryString.GetString(namespaceUri));
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000160AF File Offset: 0x000142AF
		public virtual void WriteXmlAttribute(string localName, string value)
		{
			base.WriteAttributeString("xml", localName, null, value);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000160BF File Offset: 0x000142BF
		public virtual void WriteXmlAttribute(XmlDictionaryString localName, XmlDictionaryString value)
		{
			this.WriteXmlAttribute(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(value));
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000160D3 File Offset: 0x000142D3
		public void WriteAttributeString(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, string value)
		{
			this.WriteStartAttribute(prefix, localName, namespaceUri);
			this.WriteString(value);
			this.WriteEndAttribute();
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x000160EC File Offset: 0x000142EC
		public void WriteElementString(XmlDictionaryString localName, XmlDictionaryString namespaceUri, string value)
		{
			this.WriteElementString(null, localName, namespaceUri, value);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000160F8 File Offset: 0x000142F8
		public void WriteElementString(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, string value)
		{
			this.WriteStartElement(prefix, localName, namespaceUri);
			this.WriteString(value);
			this.WriteEndElement();
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00016111 File Offset: 0x00014311
		public virtual void WriteString(XmlDictionaryString value)
		{
			this.WriteString(XmlDictionaryString.GetString(value));
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001611F File Offset: 0x0001431F
		public virtual void WriteQualifiedName(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
			}
			if (namespaceUri == null)
			{
				namespaceUri = XmlDictionaryString.Empty;
			}
			this.WriteQualifiedName(localName.Value, namespaceUri.Value);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00016150 File Offset: 0x00014350
		public virtual void WriteValue(XmlDictionaryString value)
		{
			this.WriteValue(XmlDictionaryString.GetString(value));
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00016160 File Offset: 0x00014360
		public virtual void WriteValue(IStreamProvider value)
		{
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
			}
			Stream stream = value.GetStream();
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Stream returned by IStreamProvider cannot be null.")));
			}
			int num = 256;
			byte[] array = new byte[num];
			for (;;)
			{
				int num2 = stream.Read(array, 0, num);
				if (num2 <= 0)
				{
					break;
				}
				this.WriteBase64(array, 0, num2);
				if (num < 65536 && num2 == num)
				{
					num *= 16;
					array = new byte[num];
				}
			}
			value.ReleaseStream(stream);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000161E6 File Offset: 0x000143E6
		public virtual Task WriteValueAsync(IStreamProvider value)
		{
			return Task.Factory.FromAsync<IStreamProvider>(new Func<IStreamProvider, AsyncCallback, object, IAsyncResult>(this.BeginWriteValue), new Action<IAsyncResult>(this.EndWriteValue), value, null);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0001620E File Offset: 0x0001440E
		internal virtual IAsyncResult BeginWriteValue(IStreamProvider value, AsyncCallback callback, object state)
		{
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
			}
			if (this.FastAsync)
			{
				return new XmlDictionaryWriter.WriteValueFastAsyncResult(this, value, callback, state);
			}
			return new XmlDictionaryWriter.WriteValueAsyncResult(this, value, callback, state);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001623E File Offset: 0x0001443E
		internal virtual void EndWriteValue(IAsyncResult result)
		{
			if (this.FastAsync)
			{
				XmlDictionaryWriter.WriteValueFastAsyncResult.End(result);
				return;
			}
			XmlDictionaryWriter.WriteValueAsyncResult.End(result);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00016255 File Offset: 0x00014455
		public virtual void WriteValue(UniqueId value)
		{
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
			}
			this.WriteString(value.ToString());
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00016277 File Offset: 0x00014477
		public virtual void WriteValue(Guid value)
		{
			this.WriteString(value.ToString());
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001628C File Offset: 0x0001448C
		public virtual void WriteValue(TimeSpan value)
		{
			this.WriteString(XmlConvert.ToString(value));
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x0001629A File Offset: 0x0001449A
		public virtual bool CanCanonicalize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001629D File Offset: 0x0001449D
		public virtual void StartCanonicalization(Stream stream, bool includeComments, string[] inclusivePrefixes)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000162A9 File Offset: 0x000144A9
		public virtual void EndCanonicalization()
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000162B8 File Offset: 0x000144B8
		private void WriteElementNode(XmlDictionaryReader reader, bool defattr)
		{
			XmlDictionaryString xmlDictionaryString;
			XmlDictionaryString xmlDictionaryString2;
			if (reader.TryGetLocalNameAsDictionaryString(out xmlDictionaryString) && reader.TryGetNamespaceUriAsDictionaryString(out xmlDictionaryString2))
			{
				this.WriteStartElement(reader.Prefix, xmlDictionaryString, xmlDictionaryString2);
			}
			else
			{
				this.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
			}
			if ((defattr || (!reader.IsDefault && (reader.SchemaInfo == null || !reader.SchemaInfo.IsDefault))) && reader.MoveToFirstAttribute())
			{
				do
				{
					if (reader.TryGetLocalNameAsDictionaryString(out xmlDictionaryString) && reader.TryGetNamespaceUriAsDictionaryString(out xmlDictionaryString2))
					{
						this.WriteStartAttribute(reader.Prefix, xmlDictionaryString, xmlDictionaryString2);
					}
					else
					{
						this.WriteStartAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI);
					}
					while (reader.ReadAttributeValue())
					{
						if (reader.NodeType == XmlNodeType.EntityReference)
						{
							this.WriteEntityRef(reader.Name);
						}
						else
						{
							this.WriteTextNode(reader, true);
						}
					}
					this.WriteEndAttribute();
				}
				while (reader.MoveToNextAttribute());
				reader.MoveToElement();
			}
			if (reader.IsEmptyElement)
			{
				this.WriteEndElement();
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000163B8 File Offset: 0x000145B8
		private void WriteArrayNode(XmlDictionaryReader reader, string prefix, string localName, string namespaceUri, Type type)
		{
			if (type == typeof(bool))
			{
				BooleanArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(short))
			{
				Int16ArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(int))
			{
				Int32ArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(long))
			{
				Int64ArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(float))
			{
				SingleArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(double))
			{
				DoubleArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(decimal))
			{
				DecimalArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(DateTime))
			{
				DateTimeArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(Guid))
			{
				GuidArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(TimeSpan))
			{
				TimeSpanArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			this.WriteElementNode(reader, false);
			reader.Read();
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001653C File Offset: 0x0001473C
		private void WriteArrayNode(XmlDictionaryReader reader, string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, Type type)
		{
			if (type == typeof(bool))
			{
				BooleanArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(short))
			{
				Int16ArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(int))
			{
				Int32ArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(long))
			{
				Int64ArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(float))
			{
				SingleArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(double))
			{
				DoubleArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(decimal))
			{
				DecimalArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(DateTime))
			{
				DateTimeArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(Guid))
			{
				GuidArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			if (type == typeof(TimeSpan))
			{
				TimeSpanArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
				return;
			}
			this.WriteElementNode(reader, false);
			reader.Read();
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000166C0 File Offset: 0x000148C0
		private void WriteArrayNode(XmlDictionaryReader reader, Type type)
		{
			XmlDictionaryString xmlDictionaryString;
			XmlDictionaryString xmlDictionaryString2;
			if (reader.TryGetLocalNameAsDictionaryString(out xmlDictionaryString) && reader.TryGetNamespaceUriAsDictionaryString(out xmlDictionaryString2))
			{
				this.WriteArrayNode(reader, reader.Prefix, xmlDictionaryString, xmlDictionaryString2, type);
				return;
			}
			this.WriteArrayNode(reader, reader.Prefix, reader.LocalName, reader.NamespaceURI, type);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0001670C File Offset: 0x0001490C
		protected virtual void WriteTextNode(XmlDictionaryReader reader, bool isAttribute)
		{
			XmlDictionaryString xmlDictionaryString;
			if (reader.TryGetValueAsDictionaryString(out xmlDictionaryString))
			{
				this.WriteString(xmlDictionaryString);
			}
			else
			{
				this.WriteString(reader.Value);
			}
			if (!isAttribute)
			{
				reader.Read();
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00016744 File Offset: 0x00014944
		public override void WriteNode(XmlReader reader, bool defattr)
		{
			XmlDictionaryReader xmlDictionaryReader = reader as XmlDictionaryReader;
			if (xmlDictionaryReader != null)
			{
				this.WriteNode(xmlDictionaryReader, defattr);
				return;
			}
			base.WriteNode(reader, defattr);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001676C File Offset: 0x0001496C
		public virtual void WriteNode(XmlDictionaryReader reader, bool defattr)
		{
			if (reader == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("reader"));
			}
			int num = ((reader.NodeType == XmlNodeType.None) ? (-1) : reader.Depth);
			do
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Text || nodeType == XmlNodeType.Whitespace || nodeType == XmlNodeType.SignificantWhitespace)
				{
					this.WriteTextNode(reader, false);
				}
				else
				{
					Type type;
					if (reader.Depth <= num || !reader.IsStartArray(out type))
					{
						switch (nodeType)
						{
						case XmlNodeType.Element:
							this.WriteElementNode(reader, defattr);
							break;
						case XmlNodeType.Attribute:
						case XmlNodeType.Text:
						case XmlNodeType.Entity:
						case XmlNodeType.Document:
							break;
						case XmlNodeType.CDATA:
							this.WriteCData(reader.Value);
							break;
						case XmlNodeType.EntityReference:
							this.WriteEntityRef(reader.Name);
							break;
						case XmlNodeType.ProcessingInstruction:
							goto IL_00C9;
						case XmlNodeType.Comment:
							this.WriteComment(reader.Value);
							break;
						case XmlNodeType.DocumentType:
							this.WriteDocType(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), reader.Value);
							break;
						default:
							if (nodeType != XmlNodeType.EndElement)
							{
								if (nodeType == XmlNodeType.XmlDeclaration)
								{
									goto IL_00C9;
								}
							}
							else
							{
								this.WriteFullEndElement();
							}
							break;
						}
						IL_011B:
						if (reader.Read())
						{
							goto IL_0123;
						}
						break;
						IL_00C9:
						this.WriteProcessingInstruction(reader.Name, reader.Value);
						goto IL_011B;
					}
					this.WriteArrayNode(reader, type);
				}
				IL_0123:;
			}
			while (num < reader.Depth || (num == reader.Depth && reader.NodeType == XmlNodeType.EndElement));
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000168C0 File Offset: 0x00014AC0
		private void CheckArray(Array array, int offset, int count)
		{
			if (array == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("array"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > array.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { array.Length })));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > array.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { array.Length - offset })));
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00016990 File Offset: 0x00014B90
		public virtual void WriteArray(string prefix, string localName, string namespaceUri, bool[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			for (int i = 0; i < count; i++)
			{
				this.WriteStartElement(prefix, localName, namespaceUri);
				this.WriteValue(array[offset + i]);
				this.WriteEndElement();
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x000169D2 File Offset: 0x00014BD2
		public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, bool[] array, int offset, int count)
		{
			this.WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x000169F0 File Offset: 0x00014BF0
		public virtual void WriteArray(string prefix, string localName, string namespaceUri, short[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			for (int i = 0; i < count; i++)
			{
				this.WriteStartElement(prefix, localName, namespaceUri);
				this.WriteValue((int)array[offset + i]);
				this.WriteEndElement();
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00016A32 File Offset: 0x00014C32
		public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, short[] array, int offset, int count)
		{
			this.WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00016A50 File Offset: 0x00014C50
		public virtual void WriteArray(string prefix, string localName, string namespaceUri, int[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			for (int i = 0; i < count; i++)
			{
				this.WriteStartElement(prefix, localName, namespaceUri);
				this.WriteValue(array[offset + i]);
				this.WriteEndElement();
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00016A92 File Offset: 0x00014C92
		public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, int[] array, int offset, int count)
		{
			this.WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00016AB0 File Offset: 0x00014CB0
		public virtual void WriteArray(string prefix, string localName, string namespaceUri, long[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			for (int i = 0; i < count; i++)
			{
				this.WriteStartElement(prefix, localName, namespaceUri);
				this.WriteValue(array[offset + i]);
				this.WriteEndElement();
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00016AF2 File Offset: 0x00014CF2
		public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, long[] array, int offset, int count)
		{
			this.WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00016B10 File Offset: 0x00014D10
		public virtual void WriteArray(string prefix, string localName, string namespaceUri, float[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			for (int i = 0; i < count; i++)
			{
				this.WriteStartElement(prefix, localName, namespaceUri);
				this.WriteValue(array[offset + i]);
				this.WriteEndElement();
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00016B52 File Offset: 0x00014D52
		public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, float[] array, int offset, int count)
		{
			this.WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00016B70 File Offset: 0x00014D70
		public virtual void WriteArray(string prefix, string localName, string namespaceUri, double[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			for (int i = 0; i < count; i++)
			{
				this.WriteStartElement(prefix, localName, namespaceUri);
				this.WriteValue(array[offset + i]);
				this.WriteEndElement();
			}
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00016BB2 File Offset: 0x00014DB2
		public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, double[] array, int offset, int count)
		{
			this.WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00016BD0 File Offset: 0x00014DD0
		public virtual void WriteArray(string prefix, string localName, string namespaceUri, decimal[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			for (int i = 0; i < count; i++)
			{
				this.WriteStartElement(prefix, localName, namespaceUri);
				this.WriteValue(array[offset + i]);
				this.WriteEndElement();
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00016C16 File Offset: 0x00014E16
		public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, decimal[] array, int offset, int count)
		{
			this.WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00016C34 File Offset: 0x00014E34
		public virtual void WriteArray(string prefix, string localName, string namespaceUri, DateTime[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			for (int i = 0; i < count; i++)
			{
				this.WriteStartElement(prefix, localName, namespaceUri);
				this.WriteValue(array[offset + i]);
				this.WriteEndElement();
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00016C7A File Offset: 0x00014E7A
		public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, DateTime[] array, int offset, int count)
		{
			this.WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00016C98 File Offset: 0x00014E98
		public virtual void WriteArray(string prefix, string localName, string namespaceUri, Guid[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			for (int i = 0; i < count; i++)
			{
				this.WriteStartElement(prefix, localName, namespaceUri);
				this.WriteValue(array[offset + i]);
				this.WriteEndElement();
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00016CDE File Offset: 0x00014EDE
		public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, Guid[] array, int offset, int count)
		{
			this.WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00016CFC File Offset: 0x00014EFC
		public virtual void WriteArray(string prefix, string localName, string namespaceUri, TimeSpan[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			for (int i = 0; i < count; i++)
			{
				this.WriteStartElement(prefix, localName, namespaceUri);
				this.WriteValue(array[offset + i]);
				this.WriteEndElement();
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00016D42 File Offset: 0x00014F42
		public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, TimeSpan[] array, int offset, int count)
		{
			this.WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x02000154 RID: 340
		private class WriteValueFastAsyncResult : AsyncResult
		{
			// Token: 0x060012F2 RID: 4850 RVA: 0x0004E120 File Offset: 0x0004C320
			public WriteValueFastAsyncResult(XmlDictionaryWriter writer, IStreamProvider value, AsyncCallback callback, object state)
				: base(callback, state)
			{
				this.streamProvider = value;
				this.writer = writer;
				this.stream = value.GetStream();
				if (this.stream == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Stream returned by IStreamProvider cannot be null.")));
				}
				this.blockSize = 256;
				this.bytesRead = 0;
				this.block = new byte[this.blockSize];
				this.nextOperation = XmlDictionaryWriter.WriteValueFastAsyncResult.Operation.Read;
				this.ContinueWork(true, null);
			}

			// Token: 0x060012F3 RID: 4851 RVA: 0x0004E19F File Offset: 0x0004C39F
			private void CompleteAndReleaseStream(bool completedSynchronously, Exception completionException = null)
			{
				if (completionException == null)
				{
					this.streamProvider.ReleaseStream(this.stream);
					this.stream = null;
				}
				base.Complete(completedSynchronously, completionException);
			}

			// Token: 0x060012F4 RID: 4852 RVA: 0x0004E1C4 File Offset: 0x0004C3C4
			private void ContinueWork(bool completedSynchronously, Exception completionException = null)
			{
				try
				{
					for (;;)
					{
						if (this.nextOperation == XmlDictionaryWriter.WriteValueFastAsyncResult.Operation.Read)
						{
							if (this.ReadAsync() != 1)
							{
								break;
							}
						}
						else if (this.nextOperation == XmlDictionaryWriter.WriteValueFastAsyncResult.Operation.Write)
						{
							if (this.WriteAsync() != 1)
							{
								break;
							}
						}
						else if (this.nextOperation == XmlDictionaryWriter.WriteValueFastAsyncResult.Operation.Complete)
						{
							goto Block_6;
						}
					}
					return;
					Block_6:;
				}
				catch (Exception ex)
				{
					if (Fx.IsFatal(ex))
					{
						throw;
					}
					if (completedSynchronously)
					{
						throw;
					}
					if (completionException == null)
					{
						completionException = ex;
					}
				}
				if (!this.completed)
				{
					this.completed = true;
					this.CompleteAndReleaseStream(completedSynchronously, completionException);
				}
			}

			// Token: 0x060012F5 RID: 4853 RVA: 0x0004E244 File Offset: 0x0004C444
			private AsyncCompletionResult ReadAsync()
			{
				IAsyncResult asyncResult = this.stream.BeginRead(this.block, 0, this.blockSize, XmlDictionaryWriter.WriteValueFastAsyncResult.onReadComplete, this);
				if (asyncResult.CompletedSynchronously)
				{
					this.HandleReadComplete(asyncResult);
					return 1;
				}
				return 0;
			}

			// Token: 0x060012F6 RID: 4854 RVA: 0x0004E282 File Offset: 0x0004C482
			private void HandleReadComplete(IAsyncResult result)
			{
				this.bytesRead = this.stream.EndRead(result);
				if (this.bytesRead > 0)
				{
					this.nextOperation = XmlDictionaryWriter.WriteValueFastAsyncResult.Operation.Write;
					return;
				}
				this.nextOperation = XmlDictionaryWriter.WriteValueFastAsyncResult.Operation.Complete;
			}

			// Token: 0x060012F7 RID: 4855 RVA: 0x0004E2B0 File Offset: 0x0004C4B0
			private static void OnReadComplete(IAsyncResult result)
			{
				if (result.CompletedSynchronously)
				{
					return;
				}
				Exception ex = null;
				XmlDictionaryWriter.WriteValueFastAsyncResult writeValueFastAsyncResult = (XmlDictionaryWriter.WriteValueFastAsyncResult)result.AsyncState;
				bool flag = false;
				try
				{
					writeValueFastAsyncResult.HandleReadComplete(result);
					flag = true;
				}
				catch (Exception ex2)
				{
					if (Fx.IsFatal(ex2))
					{
						throw;
					}
					ex = ex2;
				}
				if (!flag)
				{
					writeValueFastAsyncResult.nextOperation = XmlDictionaryWriter.WriteValueFastAsyncResult.Operation.Complete;
				}
				writeValueFastAsyncResult.ContinueWork(false, ex);
			}

			// Token: 0x060012F8 RID: 4856 RVA: 0x0004E310 File Offset: 0x0004C510
			private AsyncCompletionResult WriteAsync()
			{
				if (this.writerAsyncState == null)
				{
					this.writerAsyncArgs = new XmlWriteBase64AsyncArguments();
					this.writerAsyncState = new AsyncEventArgs<XmlWriteBase64AsyncArguments>();
				}
				if (XmlDictionaryWriter.WriteValueFastAsyncResult.onWriteComplete == null)
				{
					XmlDictionaryWriter.WriteValueFastAsyncResult.onWriteComplete = new AsyncEventArgsCallback(XmlDictionaryWriter.WriteValueFastAsyncResult.OnWriteComplete);
				}
				this.writerAsyncArgs.Buffer = this.block;
				this.writerAsyncArgs.Offset = 0;
				this.writerAsyncArgs.Count = this.bytesRead;
				this.writerAsyncState.Set(XmlDictionaryWriter.WriteValueFastAsyncResult.onWriteComplete, this.writerAsyncArgs, this);
				if (this.writer.WriteBase64Async(this.writerAsyncState) == 1)
				{
					this.HandleWriteComplete();
					this.writerAsyncState.Complete(true);
					return 1;
				}
				return 0;
			}

			// Token: 0x060012F9 RID: 4857 RVA: 0x0004E3C4 File Offset: 0x0004C5C4
			private void HandleWriteComplete()
			{
				this.nextOperation = XmlDictionaryWriter.WriteValueFastAsyncResult.Operation.Read;
				if (this.blockSize < 65536 && this.bytesRead == this.blockSize)
				{
					this.blockSize *= 16;
					this.block = new byte[this.blockSize];
				}
			}

			// Token: 0x060012FA RID: 4858 RVA: 0x0004E414 File Offset: 0x0004C614
			private static void OnWriteComplete(IAsyncEventArgs asyncState)
			{
				XmlDictionaryWriter.WriteValueFastAsyncResult writeValueFastAsyncResult = (XmlDictionaryWriter.WriteValueFastAsyncResult)asyncState.AsyncState;
				Exception ex = null;
				bool flag = false;
				try
				{
					if (asyncState.Exception != null)
					{
						ex = asyncState.Exception;
					}
					else
					{
						writeValueFastAsyncResult.HandleWriteComplete();
						flag = true;
					}
				}
				catch (Exception ex2)
				{
					if (Fx.IsFatal(ex2))
					{
						throw;
					}
					ex = ex2;
				}
				if (!flag)
				{
					writeValueFastAsyncResult.nextOperation = XmlDictionaryWriter.WriteValueFastAsyncResult.Operation.Complete;
				}
				writeValueFastAsyncResult.ContinueWork(false, ex);
			}

			// Token: 0x060012FB RID: 4859 RVA: 0x0004E47C File Offset: 0x0004C67C
			internal static void End(IAsyncResult result)
			{
				AsyncResult.End<XmlDictionaryWriter.WriteValueFastAsyncResult>(result);
			}

			// Token: 0x0400093E RID: 2366
			private bool completed;

			// Token: 0x0400093F RID: 2367
			private int blockSize;

			// Token: 0x04000940 RID: 2368
			private byte[] block;

			// Token: 0x04000941 RID: 2369
			private int bytesRead;

			// Token: 0x04000942 RID: 2370
			private Stream stream;

			// Token: 0x04000943 RID: 2371
			private XmlDictionaryWriter.WriteValueFastAsyncResult.Operation nextOperation;

			// Token: 0x04000944 RID: 2372
			private IStreamProvider streamProvider;

			// Token: 0x04000945 RID: 2373
			private XmlDictionaryWriter writer;

			// Token: 0x04000946 RID: 2374
			private AsyncEventArgs<XmlWriteBase64AsyncArguments> writerAsyncState;

			// Token: 0x04000947 RID: 2375
			private XmlWriteBase64AsyncArguments writerAsyncArgs;

			// Token: 0x04000948 RID: 2376
			private static AsyncCallback onReadComplete = Fx.ThunkCallback(new AsyncCallback(XmlDictionaryWriter.WriteValueFastAsyncResult.OnReadComplete));

			// Token: 0x04000949 RID: 2377
			private static AsyncEventArgsCallback onWriteComplete;

			// Token: 0x020001AA RID: 426
			private enum Operation
			{
				// Token: 0x04000A8E RID: 2702
				Read,
				// Token: 0x04000A8F RID: 2703
				Write,
				// Token: 0x04000A90 RID: 2704
				Complete
			}
		}

		// Token: 0x02000155 RID: 341
		private class WriteValueAsyncResult : AsyncResult
		{
			// Token: 0x060012FD RID: 4861 RVA: 0x0004E4A0 File Offset: 0x0004C6A0
			public WriteValueAsyncResult(XmlDictionaryWriter writer, IStreamProvider value, AsyncCallback callback, object state)
				: base(callback, state)
			{
				this.streamProvider = value;
				this.writer = writer;
				this.writeBlockHandler = ((this.writer.Settings != null && this.writer.Settings.Async) ? XmlDictionaryWriter.WriteValueAsyncResult.handleWriteBlockAsync : XmlDictionaryWriter.WriteValueAsyncResult.handleWriteBlock);
				this.stream = value.GetStream();
				if (this.stream == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Stream returned by IStreamProvider cannot be null.")));
				}
				this.blockSize = 256;
				this.bytesRead = 0;
				this.block = new byte[this.blockSize];
				if (this.ContinueWork(null))
				{
					this.CompleteAndReleaseStream(true, null);
				}
			}

			// Token: 0x060012FE RID: 4862 RVA: 0x0004E552 File Offset: 0x0004C752
			private void AdjustBlockSize()
			{
				if (this.blockSize < 65536 && this.bytesRead == this.blockSize)
				{
					this.blockSize *= 16;
					this.block = new byte[this.blockSize];
				}
			}

			// Token: 0x060012FF RID: 4863 RVA: 0x0004E58F File Offset: 0x0004C78F
			private void CompleteAndReleaseStream(bool completedSynchronously, Exception completionException)
			{
				if (completionException == null)
				{
					this.streamProvider.ReleaseStream(this.stream);
					this.stream = null;
				}
				base.Complete(completedSynchronously, completionException);
			}

			// Token: 0x06001300 RID: 4864 RVA: 0x0004E5B4 File Offset: 0x0004C7B4
			private bool ContinueWork(IAsyncResult result)
			{
				for (;;)
				{
					if (this.operation == XmlDictionaryWriter.WriteValueAsyncResult.Operation.Read)
					{
						if (!this.HandleReadBlock(result))
						{
							return false;
						}
						if (this.bytesRead <= 0)
						{
							break;
						}
						this.operation = XmlDictionaryWriter.WriteValueAsyncResult.Operation.Write;
					}
					else
					{
						if (!this.writeBlockHandler(result, this))
						{
							return false;
						}
						this.AdjustBlockSize();
						this.operation = XmlDictionaryWriter.WriteValueAsyncResult.Operation.Read;
					}
					result = null;
				}
				return true;
			}

			// Token: 0x06001301 RID: 4865 RVA: 0x0004E60C File Offset: 0x0004C80C
			private bool HandleReadBlock(IAsyncResult result)
			{
				if (result == null)
				{
					result = this.stream.BeginRead(this.block, 0, this.blockSize, XmlDictionaryWriter.WriteValueAsyncResult.onContinueWork, this);
					if (!result.CompletedSynchronously)
					{
						return false;
					}
				}
				this.bytesRead = this.stream.EndRead(result);
				return true;
			}

			// Token: 0x06001302 RID: 4866 RVA: 0x0004E659 File Offset: 0x0004C859
			private static bool HandleWriteBlock(IAsyncResult result, XmlDictionaryWriter.WriteValueAsyncResult thisPtr)
			{
				if (result == null)
				{
					result = thisPtr.writer.BeginWriteBase64(thisPtr.block, 0, thisPtr.bytesRead, XmlDictionaryWriter.WriteValueAsyncResult.onContinueWork, thisPtr);
					if (!result.CompletedSynchronously)
					{
						return false;
					}
				}
				thisPtr.writer.EndWriteBase64(result);
				return true;
			}

			// Token: 0x06001303 RID: 4867 RVA: 0x0004E698 File Offset: 0x0004C898
			private static bool HandleWriteBlockAsync(IAsyncResult result, XmlDictionaryWriter.WriteValueAsyncResult thisPtr)
			{
				Task task = (Task)result;
				if (task == null)
				{
					task = thisPtr.writer.WriteBase64Async(thisPtr.block, 0, thisPtr.bytesRead);
					TaskExtensions.AsAsyncResult(task, XmlDictionaryWriter.WriteValueAsyncResult.onContinueWork, thisPtr);
					return false;
				}
				task.GetAwaiter().GetResult();
				return true;
			}

			// Token: 0x06001304 RID: 4868 RVA: 0x0004E6E8 File Offset: 0x0004C8E8
			private static void OnContinueWork(IAsyncResult result)
			{
				if (result.CompletedSynchronously && !(result is Task))
				{
					return;
				}
				Exception ex = null;
				XmlDictionaryWriter.WriteValueAsyncResult writeValueAsyncResult = (XmlDictionaryWriter.WriteValueAsyncResult)result.AsyncState;
				bool flag = false;
				try
				{
					flag = writeValueAsyncResult.ContinueWork(result);
				}
				catch (Exception ex2)
				{
					if (Fx.IsFatal(ex2))
					{
						throw;
					}
					flag = true;
					ex = ex2;
				}
				if (flag)
				{
					writeValueAsyncResult.CompleteAndReleaseStream(false, ex);
				}
			}

			// Token: 0x06001305 RID: 4869 RVA: 0x0004E74C File Offset: 0x0004C94C
			public static void End(IAsyncResult result)
			{
				AsyncResult.End<XmlDictionaryWriter.WriteValueAsyncResult>(result);
			}

			// Token: 0x0400094A RID: 2378
			private int blockSize;

			// Token: 0x0400094B RID: 2379
			private byte[] block;

			// Token: 0x0400094C RID: 2380
			private int bytesRead;

			// Token: 0x0400094D RID: 2381
			private Stream stream;

			// Token: 0x0400094E RID: 2382
			private XmlDictionaryWriter.WriteValueAsyncResult.Operation operation;

			// Token: 0x0400094F RID: 2383
			private IStreamProvider streamProvider;

			// Token: 0x04000950 RID: 2384
			private XmlDictionaryWriter writer;

			// Token: 0x04000951 RID: 2385
			private Func<IAsyncResult, XmlDictionaryWriter.WriteValueAsyncResult, bool> writeBlockHandler;

			// Token: 0x04000952 RID: 2386
			private static Func<IAsyncResult, XmlDictionaryWriter.WriteValueAsyncResult, bool> handleWriteBlock = new Func<IAsyncResult, XmlDictionaryWriter.WriteValueAsyncResult, bool>(XmlDictionaryWriter.WriteValueAsyncResult.HandleWriteBlock);

			// Token: 0x04000953 RID: 2387
			private static Func<IAsyncResult, XmlDictionaryWriter.WriteValueAsyncResult, bool> handleWriteBlockAsync = new Func<IAsyncResult, XmlDictionaryWriter.WriteValueAsyncResult, bool>(XmlDictionaryWriter.WriteValueAsyncResult.HandleWriteBlockAsync);

			// Token: 0x04000954 RID: 2388
			private static AsyncCallback onContinueWork = Fx.ThunkCallback(new AsyncCallback(XmlDictionaryWriter.WriteValueAsyncResult.OnContinueWork));

			// Token: 0x020001AB RID: 427
			private enum Operation
			{
				// Token: 0x04000A92 RID: 2706
				Read,
				// Token: 0x04000A93 RID: 2707
				Write
			}
		}

		// Token: 0x02000156 RID: 342
		private class WriteBase64AsyncResult : ScheduleActionItemAsyncResult
		{
			// Token: 0x06001307 RID: 4871 RVA: 0x0004E78F File Offset: 0x0004C98F
			public WriteBase64AsyncResult(byte[] buffer, int index, int count, XmlDictionaryWriter writer, AsyncCallback callback, object state)
				: base(callback, state)
			{
				this.buffer = buffer;
				this.index = index;
				this.count = count;
				this.writer = writer;
				base.Schedule();
			}

			// Token: 0x06001308 RID: 4872 RVA: 0x0004E7BE File Offset: 0x0004C9BE
			protected override void OnDoWork()
			{
				this.writer.WriteBase64(this.buffer, this.index, this.count);
			}

			// Token: 0x04000955 RID: 2389
			private byte[] buffer;

			// Token: 0x04000956 RID: 2390
			private int index;

			// Token: 0x04000957 RID: 2391
			private int count;

			// Token: 0x04000958 RID: 2392
			private XmlDictionaryWriter writer;
		}

		// Token: 0x02000157 RID: 343
		private class XmlWrappedWriter : XmlDictionaryWriter
		{
			// Token: 0x06001309 RID: 4873 RVA: 0x0004E7DD File Offset: 0x0004C9DD
			public XmlWrappedWriter(XmlWriter writer)
			{
				this.writer = writer;
				this.depth = 0;
			}

			// Token: 0x0600130A RID: 4874 RVA: 0x0004E7F3 File Offset: 0x0004C9F3
			public override void Close()
			{
				this.writer.Close();
			}

			// Token: 0x0600130B RID: 4875 RVA: 0x0004E800 File Offset: 0x0004CA00
			public override void Flush()
			{
				this.writer.Flush();
			}

			// Token: 0x0600130C RID: 4876 RVA: 0x0004E80D File Offset: 0x0004CA0D
			public override string LookupPrefix(string namespaceUri)
			{
				return this.writer.LookupPrefix(namespaceUri);
			}

			// Token: 0x0600130D RID: 4877 RVA: 0x0004E81B File Offset: 0x0004CA1B
			public override void WriteAttributes(XmlReader reader, bool defattr)
			{
				this.writer.WriteAttributes(reader, defattr);
			}

			// Token: 0x0600130E RID: 4878 RVA: 0x0004E82A File Offset: 0x0004CA2A
			public override void WriteBase64(byte[] buffer, int index, int count)
			{
				this.writer.WriteBase64(buffer, index, count);
			}

			// Token: 0x0600130F RID: 4879 RVA: 0x0004E83A File Offset: 0x0004CA3A
			public override void WriteBinHex(byte[] buffer, int index, int count)
			{
				this.writer.WriteBinHex(buffer, index, count);
			}

			// Token: 0x06001310 RID: 4880 RVA: 0x0004E84A File Offset: 0x0004CA4A
			public override void WriteCData(string text)
			{
				this.writer.WriteCData(text);
			}

			// Token: 0x06001311 RID: 4881 RVA: 0x0004E858 File Offset: 0x0004CA58
			public override void WriteCharEntity(char ch)
			{
				this.writer.WriteCharEntity(ch);
			}

			// Token: 0x06001312 RID: 4882 RVA: 0x0004E866 File Offset: 0x0004CA66
			public override void WriteChars(char[] buffer, int index, int count)
			{
				this.writer.WriteChars(buffer, index, count);
			}

			// Token: 0x06001313 RID: 4883 RVA: 0x0004E876 File Offset: 0x0004CA76
			public override void WriteComment(string text)
			{
				this.writer.WriteComment(text);
			}

			// Token: 0x06001314 RID: 4884 RVA: 0x0004E884 File Offset: 0x0004CA84
			public override void WriteDocType(string name, string pubid, string sysid, string subset)
			{
				this.writer.WriteDocType(name, pubid, sysid, subset);
			}

			// Token: 0x06001315 RID: 4885 RVA: 0x0004E896 File Offset: 0x0004CA96
			public override void WriteEndAttribute()
			{
				this.writer.WriteEndAttribute();
			}

			// Token: 0x06001316 RID: 4886 RVA: 0x0004E8A3 File Offset: 0x0004CAA3
			public override void WriteEndDocument()
			{
				this.writer.WriteEndDocument();
			}

			// Token: 0x06001317 RID: 4887 RVA: 0x0004E8B0 File Offset: 0x0004CAB0
			public override void WriteEndElement()
			{
				this.writer.WriteEndElement();
				this.depth--;
			}

			// Token: 0x06001318 RID: 4888 RVA: 0x0004E8CB File Offset: 0x0004CACB
			public override void WriteEntityRef(string name)
			{
				this.writer.WriteEntityRef(name);
			}

			// Token: 0x06001319 RID: 4889 RVA: 0x0004E8D9 File Offset: 0x0004CAD9
			public override void WriteFullEndElement()
			{
				this.writer.WriteFullEndElement();
			}

			// Token: 0x0600131A RID: 4890 RVA: 0x0004E8E6 File Offset: 0x0004CAE6
			public override void WriteName(string name)
			{
				this.writer.WriteName(name);
			}

			// Token: 0x0600131B RID: 4891 RVA: 0x0004E8F4 File Offset: 0x0004CAF4
			public override void WriteNmToken(string name)
			{
				this.writer.WriteNmToken(name);
			}

			// Token: 0x0600131C RID: 4892 RVA: 0x0004E902 File Offset: 0x0004CB02
			public override void WriteNode(XmlReader reader, bool defattr)
			{
				this.writer.WriteNode(reader, defattr);
			}

			// Token: 0x0600131D RID: 4893 RVA: 0x0004E911 File Offset: 0x0004CB11
			public override void WriteProcessingInstruction(string name, string text)
			{
				this.writer.WriteProcessingInstruction(name, text);
			}

			// Token: 0x0600131E RID: 4894 RVA: 0x0004E920 File Offset: 0x0004CB20
			public override void WriteQualifiedName(string localName, string namespaceUri)
			{
				this.writer.WriteQualifiedName(localName, namespaceUri);
			}

			// Token: 0x0600131F RID: 4895 RVA: 0x0004E92F File Offset: 0x0004CB2F
			public override void WriteRaw(char[] buffer, int index, int count)
			{
				this.writer.WriteRaw(buffer, index, count);
			}

			// Token: 0x06001320 RID: 4896 RVA: 0x0004E93F File Offset: 0x0004CB3F
			public override void WriteRaw(string data)
			{
				this.writer.WriteRaw(data);
			}

			// Token: 0x06001321 RID: 4897 RVA: 0x0004E94D File Offset: 0x0004CB4D
			public override void WriteStartAttribute(string prefix, string localName, string namespaceUri)
			{
				this.writer.WriteStartAttribute(prefix, localName, namespaceUri);
				this.prefix++;
			}

			// Token: 0x06001322 RID: 4898 RVA: 0x0004E96B File Offset: 0x0004CB6B
			public override void WriteStartDocument()
			{
				this.writer.WriteStartDocument();
			}

			// Token: 0x06001323 RID: 4899 RVA: 0x0004E978 File Offset: 0x0004CB78
			public override void WriteStartDocument(bool standalone)
			{
				this.writer.WriteStartDocument(standalone);
			}

			// Token: 0x06001324 RID: 4900 RVA: 0x0004E986 File Offset: 0x0004CB86
			public override void WriteStartElement(string prefix, string localName, string namespaceUri)
			{
				this.writer.WriteStartElement(prefix, localName, namespaceUri);
				this.depth++;
				this.prefix = 1;
			}

			// Token: 0x170003CF RID: 975
			// (get) Token: 0x06001325 RID: 4901 RVA: 0x0004E9AB File Offset: 0x0004CBAB
			public override WriteState WriteState
			{
				get
				{
					return this.writer.WriteState;
				}
			}

			// Token: 0x06001326 RID: 4902 RVA: 0x0004E9B8 File Offset: 0x0004CBB8
			public override void WriteString(string text)
			{
				this.writer.WriteString(text);
			}

			// Token: 0x06001327 RID: 4903 RVA: 0x0004E9C6 File Offset: 0x0004CBC6
			public override void WriteSurrogateCharEntity(char lowChar, char highChar)
			{
				this.writer.WriteSurrogateCharEntity(lowChar, highChar);
			}

			// Token: 0x06001328 RID: 4904 RVA: 0x0004E9D5 File Offset: 0x0004CBD5
			public override void WriteWhitespace(string whitespace)
			{
				this.writer.WriteWhitespace(whitespace);
			}

			// Token: 0x06001329 RID: 4905 RVA: 0x0004E9E3 File Offset: 0x0004CBE3
			public override void WriteValue(object value)
			{
				this.writer.WriteValue(value);
			}

			// Token: 0x0600132A RID: 4906 RVA: 0x0004E9F1 File Offset: 0x0004CBF1
			public override void WriteValue(string value)
			{
				this.writer.WriteValue(value);
			}

			// Token: 0x0600132B RID: 4907 RVA: 0x0004E9FF File Offset: 0x0004CBFF
			public override void WriteValue(bool value)
			{
				this.writer.WriteValue(value);
			}

			// Token: 0x0600132C RID: 4908 RVA: 0x0004EA0D File Offset: 0x0004CC0D
			public override void WriteValue(DateTime value)
			{
				this.writer.WriteValue(value);
			}

			// Token: 0x0600132D RID: 4909 RVA: 0x0004EA1B File Offset: 0x0004CC1B
			public override void WriteValue(double value)
			{
				this.writer.WriteValue(value);
			}

			// Token: 0x0600132E RID: 4910 RVA: 0x0004EA29 File Offset: 0x0004CC29
			public override void WriteValue(int value)
			{
				this.writer.WriteValue(value);
			}

			// Token: 0x0600132F RID: 4911 RVA: 0x0004EA37 File Offset: 0x0004CC37
			public override void WriteValue(long value)
			{
				this.writer.WriteValue(value);
			}

			// Token: 0x06001330 RID: 4912 RVA: 0x0004EA48 File Offset: 0x0004CC48
			public override void WriteXmlnsAttribute(string prefix, string namespaceUri)
			{
				if (namespaceUri == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
				}
				if (prefix == null)
				{
					if (this.LookupPrefix(namespaceUri) != null)
					{
						return;
					}
					if (namespaceUri.Length == 0)
					{
						prefix = string.Empty;
					}
					else
					{
						string text = this.depth.ToString(NumberFormatInfo.InvariantInfo);
						string text2 = this.prefix.ToString(NumberFormatInfo.InvariantInfo);
						prefix = "d" + text + "p" + text2;
					}
				}
				base.WriteAttributeString("xmlns", prefix, null, namespaceUri);
			}

			// Token: 0x170003D0 RID: 976
			// (get) Token: 0x06001331 RID: 4913 RVA: 0x0004EAC4 File Offset: 0x0004CCC4
			public override string XmlLang
			{
				get
				{
					return this.writer.XmlLang;
				}
			}

			// Token: 0x170003D1 RID: 977
			// (get) Token: 0x06001332 RID: 4914 RVA: 0x0004EAD1 File Offset: 0x0004CCD1
			public override XmlSpace XmlSpace
			{
				get
				{
					return this.writer.XmlSpace;
				}
			}

			// Token: 0x04000959 RID: 2393
			private XmlWriter writer;

			// Token: 0x0400095A RID: 2394
			private int depth;

			// Token: 0x0400095B RID: 2395
			private int prefix;
		}
	}
}
