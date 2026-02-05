using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000036 RID: 54
	public abstract class XmlDictionaryReader : XmlReader
	{
		// Token: 0x060003BC RID: 956 RVA: 0x0001447C File Offset: 0x0001267C
		public static XmlDictionaryReader CreateDictionaryReader(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("reader");
			}
			XmlDictionaryReader xmlDictionaryReader = reader as XmlDictionaryReader;
			if (xmlDictionaryReader == null)
			{
				xmlDictionaryReader = new XmlDictionaryReader.XmlWrappedReader(reader, null);
			}
			return xmlDictionaryReader;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000144AA File Offset: 0x000126AA
		public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, XmlDictionaryReaderQuotas quotas)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			return XmlDictionaryReader.CreateBinaryReader(buffer, 0, buffer.Length, quotas);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000144C5 File Offset: 0x000126C5
		public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, XmlDictionaryReaderQuotas quotas)
		{
			return XmlDictionaryReader.CreateBinaryReader(buffer, offset, count, null, quotas);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x000144D1 File Offset: 0x000126D1
		public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas)
		{
			return XmlDictionaryReader.CreateBinaryReader(buffer, offset, count, dictionary, quotas, null);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000144DF File Offset: 0x000126DF
		public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session)
		{
			return XmlDictionaryReader.CreateBinaryReader(buffer, offset, count, dictionary, quotas, session, null);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000144F0 File Offset: 0x000126F0
		public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session, OnXmlDictionaryReaderClose onClose)
		{
			XmlBinaryReader xmlBinaryReader = new XmlBinaryReader();
			xmlBinaryReader.SetInput(buffer, offset, count, dictionary, quotas, session, onClose);
			return xmlBinaryReader;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00014512 File Offset: 0x00012712
		public static XmlDictionaryReader CreateBinaryReader(Stream stream, XmlDictionaryReaderQuotas quotas)
		{
			return XmlDictionaryReader.CreateBinaryReader(stream, null, quotas);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001451C File Offset: 0x0001271C
		public static XmlDictionaryReader CreateBinaryReader(Stream stream, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas)
		{
			return XmlDictionaryReader.CreateBinaryReader(stream, dictionary, quotas, null);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00014527 File Offset: 0x00012727
		public static XmlDictionaryReader CreateBinaryReader(Stream stream, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session)
		{
			return XmlDictionaryReader.CreateBinaryReader(stream, dictionary, quotas, session, null);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00014533 File Offset: 0x00012733
		public static XmlDictionaryReader CreateBinaryReader(Stream stream, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session, OnXmlDictionaryReaderClose onClose)
		{
			XmlBinaryReader xmlBinaryReader = new XmlBinaryReader();
			xmlBinaryReader.SetInput(stream, dictionary, quotas, session, onClose);
			return xmlBinaryReader;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00014546 File Offset: 0x00012746
		public static XmlDictionaryReader CreateTextReader(byte[] buffer, XmlDictionaryReaderQuotas quotas)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			return XmlDictionaryReader.CreateTextReader(buffer, 0, buffer.Length, quotas);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00014561 File Offset: 0x00012761
		public static XmlDictionaryReader CreateTextReader(byte[] buffer, int offset, int count, XmlDictionaryReaderQuotas quotas)
		{
			return XmlDictionaryReader.CreateTextReader(buffer, offset, count, null, quotas, null);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001456E File Offset: 0x0001276E
		public static XmlDictionaryReader CreateTextReader(byte[] buffer, int offset, int count, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
		{
			XmlUTF8TextReader xmlUTF8TextReader = new XmlUTF8TextReader();
			xmlUTF8TextReader.SetInput(buffer, offset, count, encoding, quotas, onClose);
			return xmlUTF8TextReader;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00014583 File Offset: 0x00012783
		public static XmlDictionaryReader CreateTextReader(Stream stream, XmlDictionaryReaderQuotas quotas)
		{
			return XmlDictionaryReader.CreateTextReader(stream, null, quotas, null);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001458E File Offset: 0x0001278E
		public static XmlDictionaryReader CreateTextReader(Stream stream, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
		{
			XmlUTF8TextReader xmlUTF8TextReader = new XmlUTF8TextReader();
			xmlUTF8TextReader.SetInput(stream, encoding, quotas, onClose);
			return xmlUTF8TextReader;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0001459F File Offset: 0x0001279F
		public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding encoding, XmlDictionaryReaderQuotas quotas)
		{
			if (encoding == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
			}
			return XmlDictionaryReader.CreateMtomReader(stream, new Encoding[] { encoding }, quotas);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000145C0 File Offset: 0x000127C0
		public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding[] encodings, XmlDictionaryReaderQuotas quotas)
		{
			return XmlDictionaryReader.CreateMtomReader(stream, encodings, null, quotas);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000145CB File Offset: 0x000127CB
		public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas)
		{
			return XmlDictionaryReader.CreateMtomReader(stream, encodings, contentType, quotas, int.MaxValue, null);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x000145DC File Offset: 0x000127DC
		public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose onClose)
		{
			XmlMtomReader xmlMtomReader = new XmlMtomReader();
			xmlMtomReader.SetInput(stream, encodings, contentType, quotas, maxBufferSize, onClose);
			return xmlMtomReader;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000145F1 File Offset: 0x000127F1
		public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding encoding, XmlDictionaryReaderQuotas quotas)
		{
			if (encoding == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
			}
			return XmlDictionaryReader.CreateMtomReader(buffer, offset, count, new Encoding[] { encoding }, quotas);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00014615 File Offset: 0x00012815
		public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding[] encodings, XmlDictionaryReaderQuotas quotas)
		{
			return XmlDictionaryReader.CreateMtomReader(buffer, offset, count, encodings, null, quotas);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00014623 File Offset: 0x00012823
		public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas)
		{
			return XmlDictionaryReader.CreateMtomReader(buffer, offset, count, encodings, contentType, quotas, int.MaxValue, null);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00014638 File Offset: 0x00012838
		public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose onClose)
		{
			XmlMtomReader xmlMtomReader = new XmlMtomReader();
			xmlMtomReader.SetInput(buffer, offset, count, encodings, contentType, quotas, maxBufferSize, onClose);
			return xmlMtomReader;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0001465C File Offset: 0x0001285C
		public virtual bool CanCanonicalize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0001465F File Offset: 0x0001285F
		public virtual XmlDictionaryReaderQuotas Quotas
		{
			get
			{
				return XmlDictionaryReaderQuotas.Max;
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00014666 File Offset: 0x00012866
		public virtual void StartCanonicalization(Stream stream, bool includeComments, string[] inclusivePrefixes)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00014672 File Offset: 0x00012872
		public virtual void EndCanonicalization()
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0001467E File Offset: 0x0001287E
		public virtual void MoveToStartElement()
		{
			if (!this.IsStartElement())
			{
				XmlExceptionHelper.ThrowStartElementExpected(this);
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001468E File Offset: 0x0001288E
		public virtual void MoveToStartElement(string name)
		{
			if (!this.IsStartElement(name))
			{
				XmlExceptionHelper.ThrowStartElementExpected(this, name);
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000146A0 File Offset: 0x000128A0
		public virtual void MoveToStartElement(string localName, string namespaceUri)
		{
			if (!this.IsStartElement(localName, namespaceUri))
			{
				XmlExceptionHelper.ThrowStartElementExpected(this, localName, namespaceUri);
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000146B4 File Offset: 0x000128B4
		public virtual void MoveToStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			if (!this.IsStartElement(localName, namespaceUri))
			{
				XmlExceptionHelper.ThrowStartElementExpected(this, localName, namespaceUri);
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000146C8 File Offset: 0x000128C8
		public virtual bool IsLocalName(string localName)
		{
			return this.LocalName == localName;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000146D6 File Offset: 0x000128D6
		public virtual bool IsLocalName(XmlDictionaryString localName)
		{
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
			}
			return this.IsLocalName(localName.Value);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000146F2 File Offset: 0x000128F2
		public virtual bool IsNamespaceUri(string namespaceUri)
		{
			if (namespaceUri == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
			}
			return this.NamespaceURI == namespaceUri;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0001470E File Offset: 0x0001290E
		public virtual bool IsNamespaceUri(XmlDictionaryString namespaceUri)
		{
			if (namespaceUri == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
			}
			return this.IsNamespaceUri(namespaceUri.Value);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001472A File Offset: 0x0001292A
		public virtual void ReadFullStartElement()
		{
			this.MoveToStartElement();
			if (this.IsEmptyElement)
			{
				XmlExceptionHelper.ThrowFullStartElementExpected(this);
			}
			this.Read();
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00014747 File Offset: 0x00012947
		public virtual void ReadFullStartElement(string name)
		{
			this.MoveToStartElement(name);
			if (this.IsEmptyElement)
			{
				XmlExceptionHelper.ThrowFullStartElementExpected(this, name);
			}
			this.Read();
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00014766 File Offset: 0x00012966
		public virtual void ReadFullStartElement(string localName, string namespaceUri)
		{
			this.MoveToStartElement(localName, namespaceUri);
			if (this.IsEmptyElement)
			{
				XmlExceptionHelper.ThrowFullStartElementExpected(this, localName, namespaceUri);
			}
			this.Read();
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00014787 File Offset: 0x00012987
		public virtual void ReadFullStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			this.MoveToStartElement(localName, namespaceUri);
			if (this.IsEmptyElement)
			{
				XmlExceptionHelper.ThrowFullStartElementExpected(this, localName, namespaceUri);
			}
			this.Read();
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000147A8 File Offset: 0x000129A8
		public virtual void ReadStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			this.MoveToStartElement(localName, namespaceUri);
			this.Read();
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000147B9 File Offset: 0x000129B9
		public virtual bool IsStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return this.IsStartElement(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri));
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000147D0 File Offset: 0x000129D0
		public virtual int IndexOfLocalName(string[] localNames, string namespaceUri)
		{
			if (localNames == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localNames");
			}
			if (namespaceUri == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
			}
			if (this.NamespaceURI == namespaceUri)
			{
				string localName = this.LocalName;
				for (int i = 0; i < localNames.Length; i++)
				{
					string text = localNames[i];
					if (text == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "localNames[{0}]", i));
					}
					if (localName == text)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001484C File Offset: 0x00012A4C
		public virtual int IndexOfLocalName(XmlDictionaryString[] localNames, XmlDictionaryString namespaceUri)
		{
			if (localNames == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localNames");
			}
			if (namespaceUri == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
			}
			if (this.NamespaceURI == namespaceUri.Value)
			{
				string localName = this.LocalName;
				for (int i = 0; i < localNames.Length; i++)
				{
					XmlDictionaryString xmlDictionaryString = localNames[i];
					if (xmlDictionaryString == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "localNames[{0}]", i));
					}
					if (localName == xmlDictionaryString.Value)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000148D0 File Offset: 0x00012AD0
		public virtual string GetAttribute(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return this.GetAttribute(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri));
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000148E4 File Offset: 0x00012AE4
		public virtual bool TryGetBase64ContentLength(out int length)
		{
			length = 0;
			return false;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x000148EA File Offset: 0x00012AEA
		public virtual int ReadValueAsBase64(byte[] buffer, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x000148F6 File Offset: 0x00012AF6
		public virtual byte[] ReadContentAsBase64()
		{
			return this.ReadContentAsBase64(this.Quotas.MaxArrayLength, 65535);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00014910 File Offset: 0x00012B10
		internal byte[] ReadContentAsBase64(int maxByteArrayContentLength, int maxInitialCount)
		{
			int num;
			if (this.TryGetBase64ContentLength(out num))
			{
				if (num > maxByteArrayContentLength)
				{
					XmlExceptionHelper.ThrowMaxArrayLengthExceeded(this, maxByteArrayContentLength);
				}
				if (num <= maxInitialCount)
				{
					byte[] array = new byte[num];
					int num2;
					for (int i = 0; i < num; i += num2)
					{
						num2 = this.ReadContentAsBase64(array, i, num - i);
						if (num2 == 0)
						{
							XmlExceptionHelper.ThrowBase64DataExpected(this);
						}
					}
					return array;
				}
			}
			return this.ReadContentAsBytes(true, maxByteArrayContentLength);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00014968 File Offset: 0x00012B68
		public override string ReadContentAsString()
		{
			return this.ReadContentAsString(this.Quotas.MaxStringContentLength);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001497C File Offset: 0x00012B7C
		protected string ReadContentAsString(int maxStringContentLength)
		{
			StringBuilder stringBuilder = null;
			string text = string.Empty;
			bool flag = false;
			for (;;)
			{
				switch (this.NodeType)
				{
				case XmlNodeType.Element:
				case XmlNodeType.Entity:
				case XmlNodeType.Document:
				case XmlNodeType.DocumentType:
				case XmlNodeType.DocumentFragment:
				case XmlNodeType.Notation:
				case XmlNodeType.EndElement:
					goto IL_00B6;
				case XmlNodeType.Attribute:
					text = this.Value;
					break;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
				{
					string value = this.Value;
					if (text.Length == 0)
					{
						text = value;
					}
					else
					{
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(text);
						}
						if (stringBuilder.Length > maxStringContentLength - value.Length)
						{
							XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, maxStringContentLength);
						}
						stringBuilder.Append(value);
					}
					break;
				}
				case XmlNodeType.EntityReference:
					if (!this.CanResolveEntity)
					{
						goto IL_00B6;
					}
					this.ResolveEntity();
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
				case XmlNodeType.EndEntity:
					break;
				default:
					goto IL_00B6;
				}
				IL_00B8:
				if (flag)
				{
					break;
				}
				if (this.AttributeCount != 0)
				{
					this.ReadAttributeValue();
					continue;
				}
				this.Read();
				continue;
				IL_00B6:
				flag = true;
				goto IL_00B8;
			}
			if (stringBuilder != null)
			{
				text = stringBuilder.ToString();
			}
			if (text.Length > maxStringContentLength)
			{
				XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, maxStringContentLength);
			}
			return text;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00014A7F File Offset: 0x00012C7F
		public override string ReadString()
		{
			return this.ReadString(this.Quotas.MaxStringContentLength);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00014A94 File Offset: 0x00012C94
		protected string ReadString(int maxStringContentLength)
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return string.Empty;
			}
			if (this.NodeType != XmlNodeType.Element)
			{
				this.MoveToElement();
			}
			if (this.NodeType == XmlNodeType.Element)
			{
				if (this.IsEmptyElement)
				{
					return string.Empty;
				}
				if (!this.Read())
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("The reader cannot be advanced.")));
				}
				if (this.NodeType == XmlNodeType.EndElement)
				{
					return string.Empty;
				}
			}
			StringBuilder stringBuilder = null;
			string text = string.Empty;
			while (this.IsTextNode(this.NodeType))
			{
				string value = this.Value;
				if (text.Length == 0)
				{
					text = value;
				}
				else
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(text);
					}
					if (stringBuilder.Length > maxStringContentLength - value.Length)
					{
						XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, maxStringContentLength);
					}
					stringBuilder.Append(value);
				}
				if (!this.Read())
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("The reader cannot be advanced.")));
				}
			}
			if (stringBuilder != null)
			{
				text = stringBuilder.ToString();
			}
			if (text.Length > maxStringContentLength)
			{
				XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, maxStringContentLength);
			}
			return text;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00014B90 File Offset: 0x00012D90
		public virtual byte[] ReadContentAsBinHex()
		{
			return this.ReadContentAsBinHex(this.Quotas.MaxArrayLength);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00014BA3 File Offset: 0x00012DA3
		protected byte[] ReadContentAsBinHex(int maxByteArrayContentLength)
		{
			return this.ReadContentAsBytes(false, maxByteArrayContentLength);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00014BB0 File Offset: 0x00012DB0
		private byte[] ReadContentAsBytes(bool base64, int maxByteArrayContentLength)
		{
			byte[][] array = new byte[32][];
			int num = 384;
			int num2 = 0;
			int num3 = 0;
			byte[] array2;
			for (;;)
			{
				array2 = new byte[num];
				array[num2++] = array2;
				int i;
				int num4;
				for (i = 0; i < array2.Length; i += num4)
				{
					if (base64)
					{
						num4 = this.ReadContentAsBase64(array2, i, array2.Length - i);
					}
					else
					{
						num4 = this.ReadContentAsBinHex(array2, i, array2.Length - i);
					}
					if (num4 == 0)
					{
						break;
					}
				}
				if (num3 > maxByteArrayContentLength - i)
				{
					XmlExceptionHelper.ThrowMaxArrayLengthExceeded(this, maxByteArrayContentLength);
				}
				num3 += i;
				if (i < array2.Length)
				{
					break;
				}
				num *= 2;
			}
			array2 = new byte[num3];
			int num5 = 0;
			for (int j = 0; j < num2 - 1; j++)
			{
				Buffer.BlockCopy(array[j], 0, array2, num5, array[j].Length);
				num5 += array[j].Length;
			}
			Buffer.BlockCopy(array[num2 - 1], 0, array2, num5, num3 - num5);
			return array2;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00014C8F File Offset: 0x00012E8F
		protected bool IsTextNode(XmlNodeType nodeType)
		{
			return nodeType == XmlNodeType.Text || nodeType == XmlNodeType.Whitespace || nodeType == XmlNodeType.SignificantWhitespace || nodeType == XmlNodeType.CDATA || nodeType == XmlNodeType.Attribute;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00014CAC File Offset: 0x00012EAC
		public virtual int ReadContentAsChars(char[] chars, int offset, int count)
		{
			int num = 0;
			for (;;)
			{
				XmlNodeType nodeType = this.NodeType;
				if (nodeType == XmlNodeType.Element || nodeType == XmlNodeType.EndElement)
				{
					break;
				}
				if (this.IsTextNode(nodeType))
				{
					num = this.ReadValueChunk(chars, offset, count);
					if (num > 0 || nodeType == XmlNodeType.Attribute)
					{
						break;
					}
					if (!this.Read())
					{
						break;
					}
				}
				else if (!this.Read())
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00014CFC File Offset: 0x00012EFC
		public override object ReadContentAs(Type type, IXmlNamespaceResolver namespaceResolver)
		{
			if (type == typeof(Guid[]))
			{
				string[] array = (string[])this.ReadContentAs(typeof(string[]), namespaceResolver);
				Guid[] array2 = new Guid[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = XmlConverter.ToGuid(array[i]);
				}
				return array2;
			}
			if (type == typeof(UniqueId[]))
			{
				string[] array3 = (string[])this.ReadContentAs(typeof(string[]), namespaceResolver);
				UniqueId[] array4 = new UniqueId[array3.Length];
				for (int j = 0; j < array3.Length; j++)
				{
					array4[j] = XmlConverter.ToUniqueId(array3[j]);
				}
				return array4;
			}
			return base.ReadContentAs(type, namespaceResolver);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00014DB8 File Offset: 0x00012FB8
		public virtual string ReadContentAsString(string[] strings, out int index)
		{
			if (strings == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("strings");
			}
			string text = this.ReadContentAsString();
			index = -1;
			for (int i = 0; i < strings.Length; i++)
			{
				string text2 = strings[i];
				if (text2 == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "strings[{0}]", i));
				}
				if (text2 == text)
				{
					index = i;
					return text2;
				}
			}
			return text;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00014E1C File Offset: 0x0001301C
		public virtual string ReadContentAsString(XmlDictionaryString[] strings, out int index)
		{
			if (strings == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("strings");
			}
			string text = this.ReadContentAsString();
			index = -1;
			for (int i = 0; i < strings.Length; i++)
			{
				XmlDictionaryString xmlDictionaryString = strings[i];
				if (xmlDictionaryString == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "strings[{0}]", i));
				}
				if (xmlDictionaryString.Value == text)
				{
					index = i;
					return xmlDictionaryString.Value;
				}
			}
			return text;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00014E8A File Offset: 0x0001308A
		public override decimal ReadContentAsDecimal()
		{
			return XmlConverter.ToDecimal(this.ReadContentAsString());
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00014E97 File Offset: 0x00013097
		public override float ReadContentAsFloat()
		{
			return XmlConverter.ToSingle(this.ReadContentAsString());
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00014EA4 File Offset: 0x000130A4
		public virtual UniqueId ReadContentAsUniqueId()
		{
			return XmlConverter.ToUniqueId(this.ReadContentAsString());
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00014EB1 File Offset: 0x000130B1
		public virtual Guid ReadContentAsGuid()
		{
			return XmlConverter.ToGuid(this.ReadContentAsString());
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00014EBE File Offset: 0x000130BE
		public virtual TimeSpan ReadContentAsTimeSpan()
		{
			return XmlConverter.ToTimeSpan(this.ReadContentAsString());
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00014ECC File Offset: 0x000130CC
		public virtual void ReadContentAsQualifiedName(out string localName, out string namespaceUri)
		{
			string text;
			XmlConverter.ToQualifiedName(this.ReadContentAsString(), out text, out localName);
			namespaceUri = this.LookupNamespace(text);
			if (namespaceUri == null)
			{
				XmlExceptionHelper.ThrowUndefinedPrefix(this, text);
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00014EFC File Offset: 0x000130FC
		public override string ReadElementContentAsString()
		{
			string text;
			if (this.IsStartElement() && this.IsEmptyElement)
			{
				this.Read();
				text = string.Empty;
			}
			else
			{
				this.ReadStartElement();
				text = this.ReadContentAsString();
				this.ReadEndElement();
			}
			return text;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00014F40 File Offset: 0x00013140
		public override bool ReadElementContentAsBoolean()
		{
			bool flag;
			if (this.IsStartElement() && this.IsEmptyElement)
			{
				this.Read();
				flag = XmlConverter.ToBoolean(string.Empty);
			}
			else
			{
				this.ReadStartElement();
				flag = this.ReadContentAsBoolean();
				this.ReadEndElement();
			}
			return flag;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00014F88 File Offset: 0x00013188
		public override int ReadElementContentAsInt()
		{
			int num;
			if (this.IsStartElement() && this.IsEmptyElement)
			{
				this.Read();
				num = XmlConverter.ToInt32(string.Empty);
			}
			else
			{
				this.ReadStartElement();
				num = this.ReadContentAsInt();
				this.ReadEndElement();
			}
			return num;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00014FD0 File Offset: 0x000131D0
		public override long ReadElementContentAsLong()
		{
			long num;
			if (this.IsStartElement() && this.IsEmptyElement)
			{
				this.Read();
				num = XmlConverter.ToInt64(string.Empty);
			}
			else
			{
				this.ReadStartElement();
				num = this.ReadContentAsLong();
				this.ReadEndElement();
			}
			return num;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00015018 File Offset: 0x00013218
		public override float ReadElementContentAsFloat()
		{
			float num;
			if (this.IsStartElement() && this.IsEmptyElement)
			{
				this.Read();
				num = XmlConverter.ToSingle(string.Empty);
			}
			else
			{
				this.ReadStartElement();
				num = this.ReadContentAsFloat();
				this.ReadEndElement();
			}
			return num;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00015060 File Offset: 0x00013260
		public override double ReadElementContentAsDouble()
		{
			double num;
			if (this.IsStartElement() && this.IsEmptyElement)
			{
				this.Read();
				num = XmlConverter.ToDouble(string.Empty);
			}
			else
			{
				this.ReadStartElement();
				num = this.ReadContentAsDouble();
				this.ReadEndElement();
			}
			return num;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000150A8 File Offset: 0x000132A8
		public override decimal ReadElementContentAsDecimal()
		{
			decimal num;
			if (this.IsStartElement() && this.IsEmptyElement)
			{
				this.Read();
				num = XmlConverter.ToDecimal(string.Empty);
			}
			else
			{
				this.ReadStartElement();
				num = this.ReadContentAsDecimal();
				this.ReadEndElement();
			}
			return num;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000150F0 File Offset: 0x000132F0
		public override DateTime ReadElementContentAsDateTime()
		{
			if (this.IsStartElement() && this.IsEmptyElement)
			{
				this.Read();
				try
				{
					return DateTime.Parse(string.Empty, NumberFormatInfo.InvariantInfo);
				}
				catch (ArgumentException ex)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "DateTime", ex));
				}
				catch (FormatException ex2)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "DateTime", ex2));
				}
			}
			this.ReadStartElement();
			DateTime dateTime = this.ReadContentAsDateTime();
			this.ReadEndElement();
			return dateTime;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00015188 File Offset: 0x00013388
		public virtual UniqueId ReadElementContentAsUniqueId()
		{
			if (this.IsStartElement() && this.IsEmptyElement)
			{
				this.Read();
				try
				{
					return new UniqueId(string.Empty);
				}
				catch (ArgumentException ex)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "UniqueId", ex));
				}
				catch (FormatException ex2)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "UniqueId", ex2));
				}
			}
			this.ReadStartElement();
			UniqueId uniqueId = this.ReadContentAsUniqueId();
			this.ReadEndElement();
			return uniqueId;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0001521C File Offset: 0x0001341C
		public virtual Guid ReadElementContentAsGuid()
		{
			if (this.IsStartElement() && this.IsEmptyElement)
			{
				this.Read();
				try
				{
					return Guid.Empty;
				}
				catch (ArgumentException ex)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "Guid", ex));
				}
				catch (FormatException ex2)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "Guid", ex2));
				}
				catch (OverflowException ex3)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "Guid", ex3));
				}
			}
			this.ReadStartElement();
			Guid guid = this.ReadContentAsGuid();
			this.ReadEndElement();
			return guid;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x000152CC File Offset: 0x000134CC
		public virtual TimeSpan ReadElementContentAsTimeSpan()
		{
			TimeSpan timeSpan;
			if (this.IsStartElement() && this.IsEmptyElement)
			{
				this.Read();
				timeSpan = XmlConverter.ToTimeSpan(string.Empty);
			}
			else
			{
				this.ReadStartElement();
				timeSpan = this.ReadContentAsTimeSpan();
				this.ReadEndElement();
			}
			return timeSpan;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00015314 File Offset: 0x00013514
		public virtual byte[] ReadElementContentAsBase64()
		{
			byte[] array;
			if (this.IsStartElement() && this.IsEmptyElement)
			{
				this.Read();
				array = new byte[0];
			}
			else
			{
				this.ReadStartElement();
				array = this.ReadContentAsBase64();
				this.ReadEndElement();
			}
			return array;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00015358 File Offset: 0x00013558
		public virtual byte[] ReadElementContentAsBinHex()
		{
			byte[] array;
			if (this.IsStartElement() && this.IsEmptyElement)
			{
				this.Read();
				array = new byte[0];
			}
			else
			{
				this.ReadStartElement();
				array = this.ReadContentAsBinHex();
				this.ReadEndElement();
			}
			return array;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001539C File Offset: 0x0001359C
		public virtual void GetNonAtomizedNames(out string localName, out string namespaceUri)
		{
			localName = this.LocalName;
			namespaceUri = this.NamespaceURI;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000153AE File Offset: 0x000135AE
		public virtual bool TryGetLocalNameAsDictionaryString(out XmlDictionaryString localName)
		{
			localName = null;
			return false;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000153B4 File Offset: 0x000135B4
		public virtual bool TryGetNamespaceUriAsDictionaryString(out XmlDictionaryString namespaceUri)
		{
			namespaceUri = null;
			return false;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000153BA File Offset: 0x000135BA
		public virtual bool TryGetValueAsDictionaryString(out XmlDictionaryString value)
		{
			value = null;
			return false;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x000153C0 File Offset: 0x000135C0
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

		// Token: 0x06000410 RID: 1040 RVA: 0x0001548E File Offset: 0x0001368E
		public virtual bool IsStartArray(out Type type)
		{
			type = null;
			return false;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00015494 File Offset: 0x00013694
		public virtual bool TryGetArrayLength(out int count)
		{
			count = 0;
			return false;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001549A File Offset: 0x0001369A
		public virtual bool[] ReadBooleanArray(string localName, string namespaceUri)
		{
			return BooleanArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x000154B4 File Offset: 0x000136B4
		public virtual bool[] ReadBooleanArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return BooleanArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x000154D0 File Offset: 0x000136D0
		public virtual int ReadArray(string localName, string namespaceUri, bool[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			int num = 0;
			while (num < count && this.IsStartElement(localName, namespaceUri))
			{
				array[offset + num] = this.ReadElementContentAsBoolean();
				num++;
			}
			return num;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001550C File Offset: 0x0001370C
		public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, bool[] array, int offset, int count)
		{
			return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00015525 File Offset: 0x00013725
		public virtual short[] ReadInt16Array(string localName, string namespaceUri)
		{
			return Int16ArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001553F File Offset: 0x0001373F
		public virtual short[] ReadInt16Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return Int16ArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001555C File Offset: 0x0001375C
		public virtual int ReadArray(string localName, string namespaceUri, short[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			int num = 0;
			while (num < count && this.IsStartElement(localName, namespaceUri))
			{
				int num2 = this.ReadElementContentAsInt();
				if (num2 < -32768 || num2 > 32767)
				{
					XmlExceptionHelper.ThrowConversionOverflow(this, num2.ToString(NumberFormatInfo.CurrentInfo), "Int16");
				}
				array[offset + num] = (short)num2;
				num++;
			}
			return num;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x000155C2 File Offset: 0x000137C2
		public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, short[] array, int offset, int count)
		{
			return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x000155DB File Offset: 0x000137DB
		public virtual int[] ReadInt32Array(string localName, string namespaceUri)
		{
			return Int32ArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000155F5 File Offset: 0x000137F5
		public virtual int[] ReadInt32Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return Int32ArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00015610 File Offset: 0x00013810
		public virtual int ReadArray(string localName, string namespaceUri, int[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			int num = 0;
			while (num < count && this.IsStartElement(localName, namespaceUri))
			{
				array[offset + num] = this.ReadElementContentAsInt();
				num++;
			}
			return num;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001564C File Offset: 0x0001384C
		public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, int[] array, int offset, int count)
		{
			return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00015665 File Offset: 0x00013865
		public virtual long[] ReadInt64Array(string localName, string namespaceUri)
		{
			return Int64ArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001567F File Offset: 0x0001387F
		public virtual long[] ReadInt64Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return Int64ArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001569C File Offset: 0x0001389C
		public virtual int ReadArray(string localName, string namespaceUri, long[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			int num = 0;
			while (num < count && this.IsStartElement(localName, namespaceUri))
			{
				array[offset + num] = this.ReadElementContentAsLong();
				num++;
			}
			return num;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x000156D8 File Offset: 0x000138D8
		public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, long[] array, int offset, int count)
		{
			return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x000156F1 File Offset: 0x000138F1
		public virtual float[] ReadSingleArray(string localName, string namespaceUri)
		{
			return SingleArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001570B File Offset: 0x0001390B
		public virtual float[] ReadSingleArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return SingleArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00015728 File Offset: 0x00013928
		public virtual int ReadArray(string localName, string namespaceUri, float[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			int num = 0;
			while (num < count && this.IsStartElement(localName, namespaceUri))
			{
				array[offset + num] = this.ReadElementContentAsFloat();
				num++;
			}
			return num;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00015764 File Offset: 0x00013964
		public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, float[] array, int offset, int count)
		{
			return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001577D File Offset: 0x0001397D
		public virtual double[] ReadDoubleArray(string localName, string namespaceUri)
		{
			return DoubleArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00015797 File Offset: 0x00013997
		public virtual double[] ReadDoubleArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return DoubleArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x000157B4 File Offset: 0x000139B4
		public virtual int ReadArray(string localName, string namespaceUri, double[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			int num = 0;
			while (num < count && this.IsStartElement(localName, namespaceUri))
			{
				array[offset + num] = this.ReadElementContentAsDouble();
				num++;
			}
			return num;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x000157F0 File Offset: 0x000139F0
		public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, double[] array, int offset, int count)
		{
			return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00015809 File Offset: 0x00013A09
		public virtual decimal[] ReadDecimalArray(string localName, string namespaceUri)
		{
			return DecimalArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00015823 File Offset: 0x00013A23
		public virtual decimal[] ReadDecimalArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return DecimalArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00015840 File Offset: 0x00013A40
		public virtual int ReadArray(string localName, string namespaceUri, decimal[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			int num = 0;
			while (num < count && this.IsStartElement(localName, namespaceUri))
			{
				array[offset + num] = this.ReadElementContentAsDecimal();
				num++;
			}
			return num;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00015880 File Offset: 0x00013A80
		public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, decimal[] array, int offset, int count)
		{
			return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00015899 File Offset: 0x00013A99
		public virtual DateTime[] ReadDateTimeArray(string localName, string namespaceUri)
		{
			return DateTimeArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x000158B3 File Offset: 0x00013AB3
		public virtual DateTime[] ReadDateTimeArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return DateTimeArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000158D0 File Offset: 0x00013AD0
		public virtual int ReadArray(string localName, string namespaceUri, DateTime[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			int num = 0;
			while (num < count && this.IsStartElement(localName, namespaceUri))
			{
				array[offset + num] = this.ReadElementContentAsDateTime();
				num++;
			}
			return num;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00015910 File Offset: 0x00013B10
		public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, DateTime[] array, int offset, int count)
		{
			return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00015929 File Offset: 0x00013B29
		public virtual Guid[] ReadGuidArray(string localName, string namespaceUri)
		{
			return GuidArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00015943 File Offset: 0x00013B43
		public virtual Guid[] ReadGuidArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return GuidArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00015960 File Offset: 0x00013B60
		public virtual int ReadArray(string localName, string namespaceUri, Guid[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			int num = 0;
			while (num < count && this.IsStartElement(localName, namespaceUri))
			{
				array[offset + num] = this.ReadElementContentAsGuid();
				num++;
			}
			return num;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x000159A0 File Offset: 0x00013BA0
		public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, Guid[] array, int offset, int count)
		{
			return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x000159B9 File Offset: 0x00013BB9
		public virtual TimeSpan[] ReadTimeSpanArray(string localName, string namespaceUri)
		{
			return TimeSpanArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000159D3 File Offset: 0x00013BD3
		public virtual TimeSpan[] ReadTimeSpanArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return TimeSpanArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x000159F0 File Offset: 0x00013BF0
		public virtual int ReadArray(string localName, string namespaceUri, TimeSpan[] array, int offset, int count)
		{
			this.CheckArray(array, offset, count);
			int num = 0;
			while (num < count && this.IsStartElement(localName, namespaceUri))
			{
				array[offset + num] = this.ReadElementContentAsTimeSpan();
				num++;
			}
			return num;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00015A30 File Offset: 0x00013C30
		public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, TimeSpan[] array, int offset, int count)
		{
			return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
		}

		// Token: 0x040001E4 RID: 484
		internal const int MaxInitialArrayLength = 65535;

		// Token: 0x02000152 RID: 338
		private class XmlWrappedReader : XmlDictionaryReader, IXmlLineInfo
		{
			// Token: 0x060012AB RID: 4779 RVA: 0x0004DCB6 File Offset: 0x0004BEB6
			public XmlWrappedReader(XmlReader reader, XmlNamespaceManager nsMgr)
			{
				this.reader = reader;
				this.nsMgr = nsMgr;
			}

			// Token: 0x170003B4 RID: 948
			// (get) Token: 0x060012AC RID: 4780 RVA: 0x0004DCCC File Offset: 0x0004BECC
			public override int AttributeCount
			{
				get
				{
					return this.reader.AttributeCount;
				}
			}

			// Token: 0x170003B5 RID: 949
			// (get) Token: 0x060012AD RID: 4781 RVA: 0x0004DCD9 File Offset: 0x0004BED9
			public override string BaseURI
			{
				get
				{
					return this.reader.BaseURI;
				}
			}

			// Token: 0x170003B6 RID: 950
			// (get) Token: 0x060012AE RID: 4782 RVA: 0x0004DCE6 File Offset: 0x0004BEE6
			public override bool CanReadBinaryContent
			{
				get
				{
					return this.reader.CanReadBinaryContent;
				}
			}

			// Token: 0x170003B7 RID: 951
			// (get) Token: 0x060012AF RID: 4783 RVA: 0x0004DCF3 File Offset: 0x0004BEF3
			public override bool CanReadValueChunk
			{
				get
				{
					return this.reader.CanReadValueChunk;
				}
			}

			// Token: 0x060012B0 RID: 4784 RVA: 0x0004DD00 File Offset: 0x0004BF00
			public override void Close()
			{
				this.reader.Close();
				this.nsMgr = null;
			}

			// Token: 0x170003B8 RID: 952
			// (get) Token: 0x060012B1 RID: 4785 RVA: 0x0004DD14 File Offset: 0x0004BF14
			public override int Depth
			{
				get
				{
					return this.reader.Depth;
				}
			}

			// Token: 0x170003B9 RID: 953
			// (get) Token: 0x060012B2 RID: 4786 RVA: 0x0004DD21 File Offset: 0x0004BF21
			public override bool EOF
			{
				get
				{
					return this.reader.EOF;
				}
			}

			// Token: 0x060012B3 RID: 4787 RVA: 0x0004DD2E File Offset: 0x0004BF2E
			public override string GetAttribute(int index)
			{
				return this.reader.GetAttribute(index);
			}

			// Token: 0x060012B4 RID: 4788 RVA: 0x0004DD3C File Offset: 0x0004BF3C
			public override string GetAttribute(string name)
			{
				return this.reader.GetAttribute(name);
			}

			// Token: 0x060012B5 RID: 4789 RVA: 0x0004DD4A File Offset: 0x0004BF4A
			public override string GetAttribute(string name, string namespaceUri)
			{
				return this.reader.GetAttribute(name, namespaceUri);
			}

			// Token: 0x170003BA RID: 954
			// (get) Token: 0x060012B6 RID: 4790 RVA: 0x0004DD59 File Offset: 0x0004BF59
			public override bool HasValue
			{
				get
				{
					return this.reader.HasValue;
				}
			}

			// Token: 0x170003BB RID: 955
			// (get) Token: 0x060012B7 RID: 4791 RVA: 0x0004DD66 File Offset: 0x0004BF66
			public override bool IsDefault
			{
				get
				{
					return this.reader.IsDefault;
				}
			}

			// Token: 0x170003BC RID: 956
			// (get) Token: 0x060012B8 RID: 4792 RVA: 0x0004DD73 File Offset: 0x0004BF73
			public override bool IsEmptyElement
			{
				get
				{
					return this.reader.IsEmptyElement;
				}
			}

			// Token: 0x060012B9 RID: 4793 RVA: 0x0004DD80 File Offset: 0x0004BF80
			public override bool IsStartElement(string name)
			{
				return this.reader.IsStartElement(name);
			}

			// Token: 0x060012BA RID: 4794 RVA: 0x0004DD8E File Offset: 0x0004BF8E
			public override bool IsStartElement(string localName, string namespaceUri)
			{
				return this.reader.IsStartElement(localName, namespaceUri);
			}

			// Token: 0x170003BD RID: 957
			// (get) Token: 0x060012BB RID: 4795 RVA: 0x0004DD9D File Offset: 0x0004BF9D
			public override string LocalName
			{
				get
				{
					return this.reader.LocalName;
				}
			}

			// Token: 0x060012BC RID: 4796 RVA: 0x0004DDAA File Offset: 0x0004BFAA
			public override string LookupNamespace(string namespaceUri)
			{
				return this.reader.LookupNamespace(namespaceUri);
			}

			// Token: 0x060012BD RID: 4797 RVA: 0x0004DDB8 File Offset: 0x0004BFB8
			public override void MoveToAttribute(int index)
			{
				this.reader.MoveToAttribute(index);
			}

			// Token: 0x060012BE RID: 4798 RVA: 0x0004DDC6 File Offset: 0x0004BFC6
			public override bool MoveToAttribute(string name)
			{
				return this.reader.MoveToAttribute(name);
			}

			// Token: 0x060012BF RID: 4799 RVA: 0x0004DDD4 File Offset: 0x0004BFD4
			public override bool MoveToAttribute(string name, string namespaceUri)
			{
				return this.reader.MoveToAttribute(name, namespaceUri);
			}

			// Token: 0x060012C0 RID: 4800 RVA: 0x0004DDE3 File Offset: 0x0004BFE3
			public override bool MoveToElement()
			{
				return this.reader.MoveToElement();
			}

			// Token: 0x060012C1 RID: 4801 RVA: 0x0004DDF0 File Offset: 0x0004BFF0
			public override bool MoveToFirstAttribute()
			{
				return this.reader.MoveToFirstAttribute();
			}

			// Token: 0x060012C2 RID: 4802 RVA: 0x0004DDFD File Offset: 0x0004BFFD
			public override bool MoveToNextAttribute()
			{
				return this.reader.MoveToNextAttribute();
			}

			// Token: 0x170003BE RID: 958
			// (get) Token: 0x060012C3 RID: 4803 RVA: 0x0004DE0A File Offset: 0x0004C00A
			public override string Name
			{
				get
				{
					return this.reader.Name;
				}
			}

			// Token: 0x170003BF RID: 959
			// (get) Token: 0x060012C4 RID: 4804 RVA: 0x0004DE17 File Offset: 0x0004C017
			public override string NamespaceURI
			{
				get
				{
					return this.reader.NamespaceURI;
				}
			}

			// Token: 0x170003C0 RID: 960
			// (get) Token: 0x060012C5 RID: 4805 RVA: 0x0004DE24 File Offset: 0x0004C024
			public override XmlNameTable NameTable
			{
				get
				{
					return this.reader.NameTable;
				}
			}

			// Token: 0x170003C1 RID: 961
			// (get) Token: 0x060012C6 RID: 4806 RVA: 0x0004DE31 File Offset: 0x0004C031
			public override XmlNodeType NodeType
			{
				get
				{
					return this.reader.NodeType;
				}
			}

			// Token: 0x170003C2 RID: 962
			// (get) Token: 0x060012C7 RID: 4807 RVA: 0x0004DE3E File Offset: 0x0004C03E
			public override string Prefix
			{
				get
				{
					return this.reader.Prefix;
				}
			}

			// Token: 0x170003C3 RID: 963
			// (get) Token: 0x060012C8 RID: 4808 RVA: 0x0004DE4B File Offset: 0x0004C04B
			public override char QuoteChar
			{
				get
				{
					return this.reader.QuoteChar;
				}
			}

			// Token: 0x060012C9 RID: 4809 RVA: 0x0004DE58 File Offset: 0x0004C058
			public override bool Read()
			{
				return this.reader.Read();
			}

			// Token: 0x060012CA RID: 4810 RVA: 0x0004DE65 File Offset: 0x0004C065
			public override bool ReadAttributeValue()
			{
				return this.reader.ReadAttributeValue();
			}

			// Token: 0x060012CB RID: 4811 RVA: 0x0004DE72 File Offset: 0x0004C072
			public override string ReadElementString(string name)
			{
				return this.reader.ReadElementString(name);
			}

			// Token: 0x060012CC RID: 4812 RVA: 0x0004DE80 File Offset: 0x0004C080
			public override string ReadElementString(string localName, string namespaceUri)
			{
				return this.reader.ReadElementString(localName, namespaceUri);
			}

			// Token: 0x060012CD RID: 4813 RVA: 0x0004DE8F File Offset: 0x0004C08F
			public override string ReadInnerXml()
			{
				return this.reader.ReadInnerXml();
			}

			// Token: 0x060012CE RID: 4814 RVA: 0x0004DE9C File Offset: 0x0004C09C
			public override string ReadOuterXml()
			{
				return this.reader.ReadOuterXml();
			}

			// Token: 0x060012CF RID: 4815 RVA: 0x0004DEA9 File Offset: 0x0004C0A9
			public override void ReadStartElement(string name)
			{
				this.reader.ReadStartElement(name);
			}

			// Token: 0x060012D0 RID: 4816 RVA: 0x0004DEB7 File Offset: 0x0004C0B7
			public override void ReadStartElement(string localName, string namespaceUri)
			{
				this.reader.ReadStartElement(localName, namespaceUri);
			}

			// Token: 0x060012D1 RID: 4817 RVA: 0x0004DEC6 File Offset: 0x0004C0C6
			public override void ReadEndElement()
			{
				this.reader.ReadEndElement();
			}

			// Token: 0x060012D2 RID: 4818 RVA: 0x0004DED3 File Offset: 0x0004C0D3
			public override string ReadString()
			{
				return this.reader.ReadString();
			}

			// Token: 0x170003C4 RID: 964
			// (get) Token: 0x060012D3 RID: 4819 RVA: 0x0004DEE0 File Offset: 0x0004C0E0
			public override ReadState ReadState
			{
				get
				{
					return this.reader.ReadState;
				}
			}

			// Token: 0x060012D4 RID: 4820 RVA: 0x0004DEED File Offset: 0x0004C0ED
			public override void ResolveEntity()
			{
				this.reader.ResolveEntity();
			}

			// Token: 0x170003C5 RID: 965
			public override string this[int index]
			{
				get
				{
					return this.reader[index];
				}
			}

			// Token: 0x170003C6 RID: 966
			public override string this[string name]
			{
				get
				{
					return this.reader[name];
				}
			}

			// Token: 0x170003C7 RID: 967
			public override string this[string name, string namespaceUri]
			{
				get
				{
					return this.reader[name, namespaceUri];
				}
			}

			// Token: 0x170003C8 RID: 968
			// (get) Token: 0x060012D8 RID: 4824 RVA: 0x0004DF25 File Offset: 0x0004C125
			public override string Value
			{
				get
				{
					return this.reader.Value;
				}
			}

			// Token: 0x170003C9 RID: 969
			// (get) Token: 0x060012D9 RID: 4825 RVA: 0x0004DF32 File Offset: 0x0004C132
			public override string XmlLang
			{
				get
				{
					return this.reader.XmlLang;
				}
			}

			// Token: 0x170003CA RID: 970
			// (get) Token: 0x060012DA RID: 4826 RVA: 0x0004DF3F File Offset: 0x0004C13F
			public override XmlSpace XmlSpace
			{
				get
				{
					return this.reader.XmlSpace;
				}
			}

			// Token: 0x060012DB RID: 4827 RVA: 0x0004DF4C File Offset: 0x0004C14C
			public override int ReadElementContentAsBase64(byte[] buffer, int offset, int count)
			{
				return this.reader.ReadElementContentAsBase64(buffer, offset, count);
			}

			// Token: 0x060012DC RID: 4828 RVA: 0x0004DF5C File Offset: 0x0004C15C
			public override int ReadContentAsBase64(byte[] buffer, int offset, int count)
			{
				return this.reader.ReadContentAsBase64(buffer, offset, count);
			}

			// Token: 0x060012DD RID: 4829 RVA: 0x0004DF6C File Offset: 0x0004C16C
			public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int count)
			{
				return this.reader.ReadElementContentAsBinHex(buffer, offset, count);
			}

			// Token: 0x060012DE RID: 4830 RVA: 0x0004DF7C File Offset: 0x0004C17C
			public override int ReadContentAsBinHex(byte[] buffer, int offset, int count)
			{
				return this.reader.ReadContentAsBinHex(buffer, offset, count);
			}

			// Token: 0x060012DF RID: 4831 RVA: 0x0004DF8C File Offset: 0x0004C18C
			public override int ReadValueChunk(char[] chars, int offset, int count)
			{
				return this.reader.ReadValueChunk(chars, offset, count);
			}

			// Token: 0x170003CB RID: 971
			// (get) Token: 0x060012E0 RID: 4832 RVA: 0x0004DF9C File Offset: 0x0004C19C
			public override Type ValueType
			{
				get
				{
					return this.reader.ValueType;
				}
			}

			// Token: 0x060012E1 RID: 4833 RVA: 0x0004DFA9 File Offset: 0x0004C1A9
			public override bool ReadContentAsBoolean()
			{
				return this.reader.ReadContentAsBoolean();
			}

			// Token: 0x060012E2 RID: 4834 RVA: 0x0004DFB6 File Offset: 0x0004C1B6
			public override DateTime ReadContentAsDateTime()
			{
				return this.reader.ReadContentAsDateTime();
			}

			// Token: 0x060012E3 RID: 4835 RVA: 0x0004DFC3 File Offset: 0x0004C1C3
			public override decimal ReadContentAsDecimal()
			{
				return (decimal)this.reader.ReadContentAs(typeof(decimal), null);
			}

			// Token: 0x060012E4 RID: 4836 RVA: 0x0004DFE0 File Offset: 0x0004C1E0
			public override double ReadContentAsDouble()
			{
				return this.reader.ReadContentAsDouble();
			}

			// Token: 0x060012E5 RID: 4837 RVA: 0x0004DFED File Offset: 0x0004C1ED
			public override int ReadContentAsInt()
			{
				return this.reader.ReadContentAsInt();
			}

			// Token: 0x060012E6 RID: 4838 RVA: 0x0004DFFA File Offset: 0x0004C1FA
			public override long ReadContentAsLong()
			{
				return this.reader.ReadContentAsLong();
			}

			// Token: 0x060012E7 RID: 4839 RVA: 0x0004E007 File Offset: 0x0004C207
			public override float ReadContentAsFloat()
			{
				return this.reader.ReadContentAsFloat();
			}

			// Token: 0x060012E8 RID: 4840 RVA: 0x0004E014 File Offset: 0x0004C214
			public override string ReadContentAsString()
			{
				return this.reader.ReadContentAsString();
			}

			// Token: 0x060012E9 RID: 4841 RVA: 0x0004E021 File Offset: 0x0004C221
			public override object ReadContentAs(Type type, IXmlNamespaceResolver namespaceResolver)
			{
				return this.reader.ReadContentAs(type, namespaceResolver);
			}

			// Token: 0x060012EA RID: 4842 RVA: 0x0004E030 File Offset: 0x0004C230
			public bool HasLineInfo()
			{
				IXmlLineInfo xmlLineInfo = this.reader as IXmlLineInfo;
				return xmlLineInfo != null && xmlLineInfo.HasLineInfo();
			}

			// Token: 0x170003CC RID: 972
			// (get) Token: 0x060012EB RID: 4843 RVA: 0x0004E054 File Offset: 0x0004C254
			public int LineNumber
			{
				get
				{
					IXmlLineInfo xmlLineInfo = this.reader as IXmlLineInfo;
					if (xmlLineInfo == null)
					{
						return 1;
					}
					return xmlLineInfo.LineNumber;
				}
			}

			// Token: 0x170003CD RID: 973
			// (get) Token: 0x060012EC RID: 4844 RVA: 0x0004E078 File Offset: 0x0004C278
			public int LinePosition
			{
				get
				{
					IXmlLineInfo xmlLineInfo = this.reader as IXmlLineInfo;
					if (xmlLineInfo == null)
					{
						return 1;
					}
					return xmlLineInfo.LinePosition;
				}
			}

			// Token: 0x0400093B RID: 2363
			private XmlReader reader;

			// Token: 0x0400093C RID: 2364
			private XmlNamespaceManager nsMgr;
		}
	}
}
