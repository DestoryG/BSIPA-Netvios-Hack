using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000027 RID: 39
	internal abstract class XmlBaseReader : XmlDictionaryReader
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00005EB0 File Offset: 0x000040B0
		protected XmlBaseReader()
		{
			this.bufferReader = new XmlBufferReader(this);
			this.nsMgr = new XmlBaseReader.NamespaceManager(this.bufferReader);
			this.quotas = new XmlDictionaryReaderQuotas();
			this.rootElementNode = new XmlBaseReader.XmlElementNode(this.bufferReader);
			this.atomicTextNode = new XmlBaseReader.XmlAtomicTextNode(this.bufferReader);
			this.node = XmlBaseReader.closedNode;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00005F18 File Offset: 0x00004118
		private static BinHexEncoding BinHexEncoding
		{
			get
			{
				if (XmlBaseReader.binhexEncoding == null)
				{
					XmlBaseReader.binhexEncoding = new BinHexEncoding();
				}
				return XmlBaseReader.binhexEncoding;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00005F30 File Offset: 0x00004130
		private static Base64Encoding Base64Encoding
		{
			get
			{
				if (XmlBaseReader.base64Encoding == null)
				{
					XmlBaseReader.base64Encoding = new Base64Encoding();
				}
				return XmlBaseReader.base64Encoding;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00005F48 File Offset: 0x00004148
		protected XmlBufferReader BufferReader
		{
			get
			{
				return this.bufferReader;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00005F50 File Offset: 0x00004150
		public override XmlDictionaryReaderQuotas Quotas
		{
			get
			{
				return this.quotas;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00005F58 File Offset: 0x00004158
		protected XmlBaseReader.XmlNode Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005F60 File Offset: 0x00004160
		protected void MoveToNode(XmlBaseReader.XmlNode node)
		{
			this.node = node;
			this.ns = null;
			this.localName = null;
			this.prefix = null;
			this.value = null;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005F88 File Offset: 0x00004188
		protected void MoveToInitial(XmlDictionaryReaderQuotas quotas)
		{
			if (quotas == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("quotas");
			}
			quotas.InternalCopyTo(this.quotas);
			this.quotas.MakeReadOnly();
			this.nsMgr.Clear();
			this.depth = 0;
			this.attributeCount = 0;
			this.attributeStart = -1;
			this.attributeIndex = -1;
			this.rootElement = false;
			this.readingElement = false;
			this.signing = false;
			this.MoveToNode(XmlBaseReader.initialNode);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006004 File Offset: 0x00004204
		protected XmlBaseReader.XmlDeclarationNode MoveToDeclaration()
		{
			if (this.attributeCount < 1)
			{
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("Version not found in XML declaration.")));
			}
			if (this.attributeCount > 3)
			{
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("Malformed XML declaration.")));
			}
			if (!this.CheckDeclAttribute(0, "version", "1.0", false, "XML version must be '1.0'."))
			{
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("Version not found in XML declaration.")));
			}
			if (this.attributeCount > 1)
			{
				if (this.CheckDeclAttribute(1, "encoding", null, true, "XML encoding must be 'UTF-8'."))
				{
					if (this.attributeCount == 3 && !this.CheckStandalone(2))
					{
						XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("Malformed XML declaration.")));
					}
				}
				else if (!this.CheckStandalone(1) || this.attributeCount > 2)
				{
					XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("Malformed XML declaration.")));
				}
			}
			if (this.declarationNode == null)
			{
				this.declarationNode = new XmlBaseReader.XmlDeclarationNode(this.bufferReader);
			}
			this.MoveToNode(this.declarationNode);
			return this.declarationNode;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006114 File Offset: 0x00004314
		private bool CheckStandalone(int attr)
		{
			XmlBaseReader.XmlAttributeNode xmlAttributeNode = this.attributeNodes[attr];
			if (!xmlAttributeNode.Prefix.IsEmpty)
			{
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("Malformed XML declaration.")));
			}
			if (xmlAttributeNode.LocalName != "standalone")
			{
				return false;
			}
			if (!xmlAttributeNode.Value.Equals2("yes", false) && !xmlAttributeNode.Value.Equals2("no", false))
			{
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("'standalone' value in declaration must be 'yes' or 'no'.")));
			}
			return true;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000619C File Offset: 0x0000439C
		private bool CheckDeclAttribute(int index, string localName, string value, bool checkLower, string valueSR)
		{
			XmlBaseReader.XmlAttributeNode xmlAttributeNode = this.attributeNodes[index];
			if (!xmlAttributeNode.Prefix.IsEmpty)
			{
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("Malformed XML declaration.")));
			}
			if (xmlAttributeNode.LocalName != localName)
			{
				return false;
			}
			if (value != null && !xmlAttributeNode.Value.Equals2(value, checkLower))
			{
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString(valueSR)));
			}
			return true;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000620A File Offset: 0x0000440A
		protected XmlBaseReader.XmlCommentNode MoveToComment()
		{
			if (this.commentNode == null)
			{
				this.commentNode = new XmlBaseReader.XmlCommentNode(this.bufferReader);
			}
			this.MoveToNode(this.commentNode);
			return this.commentNode;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006237 File Offset: 0x00004437
		protected XmlBaseReader.XmlCDataNode MoveToCData()
		{
			if (this.cdataNode == null)
			{
				this.cdataNode = new XmlBaseReader.XmlCDataNode(this.bufferReader);
			}
			this.MoveToNode(this.cdataNode);
			return this.cdataNode;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006264 File Offset: 0x00004464
		protected XmlBaseReader.XmlAtomicTextNode MoveToAtomicText()
		{
			XmlBaseReader.XmlAtomicTextNode xmlAtomicTextNode = this.atomicTextNode;
			this.MoveToNode(xmlAtomicTextNode);
			return xmlAtomicTextNode;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006280 File Offset: 0x00004480
		protected XmlBaseReader.XmlComplexTextNode MoveToComplexText()
		{
			if (this.complexTextNode == null)
			{
				this.complexTextNode = new XmlBaseReader.XmlComplexTextNode(this.bufferReader);
			}
			this.MoveToNode(this.complexTextNode);
			return this.complexTextNode;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000062B0 File Offset: 0x000044B0
		protected XmlBaseReader.XmlTextNode MoveToWhitespaceText()
		{
			if (this.whitespaceTextNode == null)
			{
				this.whitespaceTextNode = new XmlBaseReader.XmlWhitespaceTextNode(this.bufferReader);
			}
			if (this.nsMgr.XmlSpace == XmlSpace.Preserve)
			{
				this.whitespaceTextNode.NodeType = XmlNodeType.SignificantWhitespace;
			}
			else
			{
				this.whitespaceTextNode.NodeType = XmlNodeType.Whitespace;
			}
			this.MoveToNode(this.whitespaceTextNode);
			return this.whitespaceTextNode;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00006312 File Offset: 0x00004512
		protected XmlBaseReader.XmlElementNode ElementNode
		{
			get
			{
				if (this.depth == 0)
				{
					return this.rootElementNode;
				}
				return this.elementNodes[this.depth];
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006330 File Offset: 0x00004530
		protected void MoveToEndElement()
		{
			if (this.depth == 0)
			{
				XmlExceptionHelper.ThrowInvalidBinaryFormat(this);
			}
			XmlBaseReader.XmlElementNode xmlElementNode = this.elementNodes[this.depth];
			XmlBaseReader.XmlEndElementNode endElement = xmlElementNode.EndElement;
			endElement.Namespace = xmlElementNode.Namespace;
			this.MoveToNode(endElement);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006373 File Offset: 0x00004573
		protected void MoveToEndOfFile()
		{
			if (this.depth != 0)
			{
				XmlExceptionHelper.ThrowUnexpectedEndOfFile(this);
			}
			this.MoveToNode(XmlBaseReader.endOfFileNode);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006390 File Offset: 0x00004590
		protected XmlBaseReader.XmlElementNode EnterScope()
		{
			if (this.depth == 0)
			{
				if (this.rootElement)
				{
					XmlExceptionHelper.ThrowMultipleRootElements(this);
				}
				this.rootElement = true;
			}
			this.nsMgr.EnterScope();
			this.depth++;
			if (this.depth > this.quotas.MaxDepth)
			{
				XmlExceptionHelper.ThrowMaxDepthExceeded(this, this.quotas.MaxDepth);
			}
			if (this.elementNodes == null)
			{
				this.elementNodes = new XmlBaseReader.XmlElementNode[4];
			}
			else if (this.elementNodes.Length == this.depth)
			{
				XmlBaseReader.XmlElementNode[] array = new XmlBaseReader.XmlElementNode[this.depth * 2];
				Array.Copy(this.elementNodes, array, this.depth);
				this.elementNodes = array;
			}
			XmlBaseReader.XmlElementNode xmlElementNode = this.elementNodes[this.depth];
			if (xmlElementNode == null)
			{
				xmlElementNode = new XmlBaseReader.XmlElementNode(this.bufferReader);
				this.elementNodes[this.depth] = xmlElementNode;
			}
			this.attributeCount = 0;
			this.attributeStart = -1;
			this.attributeIndex = -1;
			this.MoveToNode(xmlElementNode);
			return xmlElementNode;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000648C File Offset: 0x0000468C
		protected void ExitScope()
		{
			if (this.depth == 0)
			{
				XmlExceptionHelper.ThrowUnexpectedEndElement(this);
			}
			this.depth--;
			this.nsMgr.ExitScope();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000064B8 File Offset: 0x000046B8
		private XmlBaseReader.XmlAttributeNode AddAttribute(XmlBaseReader.QNameType qnameType, bool isAtomicValue)
		{
			int num = this.attributeCount;
			if (this.attributeNodes == null)
			{
				this.attributeNodes = new XmlBaseReader.XmlAttributeNode[4];
			}
			else if (this.attributeNodes.Length == num)
			{
				XmlBaseReader.XmlAttributeNode[] array = new XmlBaseReader.XmlAttributeNode[num * 2];
				Array.Copy(this.attributeNodes, array, num);
				this.attributeNodes = array;
			}
			XmlBaseReader.XmlAttributeNode xmlAttributeNode = this.attributeNodes[num];
			if (xmlAttributeNode == null)
			{
				xmlAttributeNode = new XmlBaseReader.XmlAttributeNode(this.bufferReader);
				this.attributeNodes[num] = xmlAttributeNode;
			}
			xmlAttributeNode.QNameType = qnameType;
			xmlAttributeNode.IsAtomicValue = isAtomicValue;
			xmlAttributeNode.AttributeText.QNameType = qnameType;
			xmlAttributeNode.AttributeText.IsAtomicValue = isAtomicValue;
			this.attributeCount++;
			return xmlAttributeNode;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006560 File Offset: 0x00004760
		protected XmlBaseReader.Namespace AddNamespace()
		{
			return this.nsMgr.AddNamespace();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000656D File Offset: 0x0000476D
		protected XmlBaseReader.XmlAttributeNode AddAttribute()
		{
			return this.AddAttribute(XmlBaseReader.QNameType.Normal, true);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006577 File Offset: 0x00004777
		protected XmlBaseReader.XmlAttributeNode AddXmlAttribute()
		{
			return this.AddAttribute(XmlBaseReader.QNameType.Normal, true);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006584 File Offset: 0x00004784
		protected XmlBaseReader.XmlAttributeNode AddXmlnsAttribute(XmlBaseReader.Namespace ns)
		{
			if (!ns.Prefix.IsEmpty && ns.Uri.IsEmpty)
			{
				XmlExceptionHelper.ThrowEmptyNamespace(this);
			}
			if (ns.Prefix.IsXml && ns.Uri != "http://www.w3.org/XML/1998/namespace")
			{
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("The prefix '{0}' can only be bound to the namespace '{1}'.", new object[] { "xml", "http://www.w3.org/XML/1998/namespace" })));
			}
			else if (ns.Prefix.IsXmlns && ns.Uri != "http://www.w3.org/2000/xmlns/")
			{
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("The prefix '{0}' can only be bound to the namespace '{1}'.", new object[] { "xmlns", "http://www.w3.org/2000/xmlns/" })));
			}
			this.nsMgr.Register(ns);
			XmlBaseReader.XmlAttributeNode xmlAttributeNode = this.AddAttribute(XmlBaseReader.QNameType.Xmlns, false);
			xmlAttributeNode.Namespace = ns;
			xmlAttributeNode.AttributeText.Namespace = ns;
			return xmlAttributeNode;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006670 File Offset: 0x00004870
		protected void FixXmlAttribute(XmlBaseReader.XmlAttributeNode attributeNode)
		{
			if (attributeNode.Prefix == "xml")
			{
				if (attributeNode.LocalName == "lang")
				{
					this.nsMgr.AddLangAttribute(attributeNode.Value.GetString());
					return;
				}
				if (attributeNode.LocalName == "space")
				{
					string @string = attributeNode.Value.GetString();
					if (@string == "preserve")
					{
						this.nsMgr.AddSpaceAttribute(XmlSpace.Preserve);
						return;
					}
					if (@string == "default")
					{
						this.nsMgr.AddSpaceAttribute(XmlSpace.Default);
					}
				}
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00006709 File Offset: 0x00004909
		protected bool OutsideRootElement
		{
			get
			{
				return this.depth == 0;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00006714 File Offset: 0x00004914
		public override bool CanReadBinaryContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00006717 File Offset: 0x00004917
		public override bool CanReadValueChunk
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000671A File Offset: 0x0000491A
		public override string BaseURI
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00006721 File Offset: 0x00004921
		public override bool HasValue
		{
			get
			{
				return this.node.HasValue;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0000672E File Offset: 0x0000492E
		public override bool IsDefault
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001F RID: 31
		public override string this[int index]
		{
			get
			{
				return this.GetAttribute(index);
			}
		}

		// Token: 0x17000020 RID: 32
		public override string this[string name]
		{
			get
			{
				return this.GetAttribute(name);
			}
		}

		// Token: 0x17000021 RID: 33
		public override string this[string localName, string namespaceUri]
		{
			get
			{
				return this.GetAttribute(localName, namespaceUri);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0000674D File Offset: 0x0000494D
		public override int AttributeCount
		{
			get
			{
				if (this.node.CanGetAttribute)
				{
					return this.attributeCount;
				}
				return 0;
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006764 File Offset: 0x00004964
		public override void Close()
		{
			this.MoveToNode(XmlBaseReader.closedNode);
			this.nameTable = null;
			if (this.attributeNodes != null && this.attributeNodes.Length > 16)
			{
				this.attributeNodes = null;
			}
			if (this.elementNodes != null && this.elementNodes.Length > 16)
			{
				this.elementNodes = null;
			}
			this.nsMgr.Close();
			this.bufferReader.Close();
			if (this.signingWriter != null)
			{
				this.signingWriter.Close();
			}
			if (this.attributeSorter != null)
			{
				this.attributeSorter.Close();
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600013F RID: 319 RVA: 0x000067F5 File Offset: 0x000049F5
		public sealed override int Depth
		{
			get
			{
				return this.depth + this.node.DepthDelta;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00006809 File Offset: 0x00004A09
		public override bool EOF
		{
			get
			{
				return this.node.ReadState == ReadState.EndOfFile;
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000681C File Offset: 0x00004A1C
		private XmlBaseReader.XmlAttributeNode GetAttributeNode(int index)
		{
			if (!this.node.CanGetAttribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", global::System.Runtime.Serialization.SR.GetString("Only Element nodes have attributes.")));
			}
			if (index < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (index >= this.attributeCount)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { this.attributeCount })));
			}
			return this.attributeNodes[index];
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000068B0 File Offset: 0x00004AB0
		private XmlBaseReader.XmlAttributeNode GetAttributeNode(string name)
		{
			if (name == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("name"));
			}
			if (!this.node.CanGetAttribute)
			{
				return null;
			}
			int num = name.IndexOf(':');
			string text;
			string text2;
			if (num == -1)
			{
				if (name == "xmlns")
				{
					text = "xmlns";
					text2 = string.Empty;
				}
				else
				{
					text = string.Empty;
					text2 = name;
				}
			}
			else
			{
				text = name.Substring(0, num);
				text2 = name.Substring(num + 1);
			}
			XmlBaseReader.XmlAttributeNode[] array = this.attributeNodes;
			int num2 = this.attributeCount;
			int num3 = this.attributeStart;
			for (int i = 0; i < num2; i++)
			{
				if (++num3 >= num2)
				{
					num3 = 0;
				}
				XmlBaseReader.XmlAttributeNode xmlAttributeNode = array[num3];
				if (xmlAttributeNode.IsPrefixAndLocalName(text, text2))
				{
					this.attributeStart = num3;
					return xmlAttributeNode;
				}
			}
			return null;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00006978 File Offset: 0x00004B78
		private XmlBaseReader.XmlAttributeNode GetAttributeNode(string localName, string namespaceUri)
		{
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
			}
			if (namespaceUri == null)
			{
				namespaceUri = string.Empty;
			}
			if (!this.node.CanGetAttribute)
			{
				return null;
			}
			XmlBaseReader.XmlAttributeNode[] array = this.attributeNodes;
			int num = this.attributeCount;
			int num2 = this.attributeStart;
			for (int i = 0; i < num; i++)
			{
				if (++num2 >= num)
				{
					num2 = 0;
				}
				XmlBaseReader.XmlAttributeNode xmlAttributeNode = array[num2];
				if (xmlAttributeNode.IsLocalNameAndNamespaceUri(localName, namespaceUri))
				{
					this.attributeStart = num2;
					return xmlAttributeNode;
				}
			}
			return null;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000069F8 File Offset: 0x00004BF8
		private XmlBaseReader.XmlAttributeNode GetAttributeNode(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
			}
			if (namespaceUri == null)
			{
				namespaceUri = XmlDictionaryString.Empty;
			}
			if (!this.node.CanGetAttribute)
			{
				return null;
			}
			XmlBaseReader.XmlAttributeNode[] array = this.attributeNodes;
			int num = this.attributeCount;
			int num2 = this.attributeStart;
			for (int i = 0; i < num; i++)
			{
				if (++num2 >= num)
				{
					num2 = 0;
				}
				XmlBaseReader.XmlAttributeNode xmlAttributeNode = array[num2];
				if (xmlAttributeNode.IsLocalNameAndNamespaceUri(localName, namespaceUri))
				{
					this.attributeStart = num2;
					return xmlAttributeNode;
				}
			}
			return null;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006A77 File Offset: 0x00004C77
		public override string GetAttribute(int index)
		{
			return this.GetAttributeNode(index).ValueAsString;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006A88 File Offset: 0x00004C88
		public override string GetAttribute(string name)
		{
			XmlBaseReader.XmlAttributeNode attributeNode = this.GetAttributeNode(name);
			if (attributeNode == null)
			{
				return null;
			}
			return attributeNode.ValueAsString;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006AA8 File Offset: 0x00004CA8
		public override string GetAttribute(string localName, string namespaceUri)
		{
			XmlBaseReader.XmlAttributeNode attributeNode = this.GetAttributeNode(localName, namespaceUri);
			if (attributeNode == null)
			{
				return null;
			}
			return attributeNode.ValueAsString;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006ACC File Offset: 0x00004CCC
		public override string GetAttribute(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			XmlBaseReader.XmlAttributeNode attributeNode = this.GetAttributeNode(localName, namespaceUri);
			if (attributeNode == null)
			{
				return null;
			}
			return attributeNode.ValueAsString;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00006AED File Offset: 0x00004CED
		public sealed override bool IsEmptyElement
		{
			get
			{
				return this.node.IsEmptyElement;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00006AFA File Offset: 0x00004CFA
		public override string LocalName
		{
			get
			{
				if (this.localName == null)
				{
					this.localName = this.GetLocalName(true);
				}
				return this.localName;
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00006B18 File Offset: 0x00004D18
		public override string LookupNamespace(string prefix)
		{
			XmlBaseReader.Namespace @namespace = this.nsMgr.LookupNamespace(prefix);
			if (@namespace != null)
			{
				return @namespace.Uri.GetString(this.NameTable);
			}
			if (prefix == "xmlns")
			{
				return "http://www.w3.org/2000/xmlns/";
			}
			return null;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006B5B File Offset: 0x00004D5B
		protected XmlBaseReader.Namespace LookupNamespace(PrefixHandleType prefix)
		{
			XmlBaseReader.Namespace @namespace = this.nsMgr.LookupNamespace(prefix);
			if (@namespace == null)
			{
				XmlExceptionHelper.ThrowUndefinedPrefix(this, PrefixHandle.GetString(prefix));
			}
			return @namespace;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00006B78 File Offset: 0x00004D78
		protected XmlBaseReader.Namespace LookupNamespace(PrefixHandle prefix)
		{
			XmlBaseReader.Namespace @namespace = this.nsMgr.LookupNamespace(prefix);
			if (@namespace == null)
			{
				XmlExceptionHelper.ThrowUndefinedPrefix(this, prefix.GetString());
			}
			return @namespace;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00006B95 File Offset: 0x00004D95
		protected void ProcessAttributes()
		{
			if (this.attributeCount > 0)
			{
				this.ProcessAttributes(this.attributeNodes, this.attributeCount);
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00006BB4 File Offset: 0x00004DB4
		private void ProcessAttributes(XmlBaseReader.XmlAttributeNode[] attributeNodes, int attributeCount)
		{
			for (int i = 0; i < attributeCount; i++)
			{
				XmlBaseReader.XmlAttributeNode xmlAttributeNode = attributeNodes[i];
				if (xmlAttributeNode.QNameType == XmlBaseReader.QNameType.Normal)
				{
					PrefixHandle prefixHandle = xmlAttributeNode.Prefix;
					if (!prefixHandle.IsEmpty)
					{
						xmlAttributeNode.Namespace = this.LookupNamespace(prefixHandle);
					}
					else
					{
						xmlAttributeNode.Namespace = XmlBaseReader.NamespaceManager.EmptyNamespace;
					}
					xmlAttributeNode.AttributeText.Namespace = xmlAttributeNode.Namespace;
				}
			}
			if (attributeCount > 1)
			{
				if (attributeCount < 12)
				{
					for (int j = 0; j < attributeCount - 1; j++)
					{
						XmlBaseReader.XmlAttributeNode xmlAttributeNode2 = attributeNodes[j];
						if (xmlAttributeNode2.QNameType == XmlBaseReader.QNameType.Normal)
						{
							for (int k = j + 1; k < attributeCount; k++)
							{
								XmlBaseReader.XmlAttributeNode xmlAttributeNode3 = attributeNodes[k];
								if (xmlAttributeNode3.QNameType == XmlBaseReader.QNameType.Normal && xmlAttributeNode2.LocalName == xmlAttributeNode3.LocalName && xmlAttributeNode2.Namespace.Uri == xmlAttributeNode3.Namespace.Uri)
								{
									XmlExceptionHelper.ThrowDuplicateAttribute(this, xmlAttributeNode2.Prefix.GetString(), xmlAttributeNode3.Prefix.GetString(), xmlAttributeNode2.LocalName.GetString(), xmlAttributeNode2.Namespace.Uri.GetString());
								}
							}
						}
						else
						{
							for (int l = j + 1; l < attributeCount; l++)
							{
								XmlBaseReader.XmlAttributeNode xmlAttributeNode4 = attributeNodes[l];
								if (xmlAttributeNode4.QNameType == XmlBaseReader.QNameType.Xmlns && xmlAttributeNode2.Namespace.Prefix == xmlAttributeNode4.Namespace.Prefix)
								{
									XmlExceptionHelper.ThrowDuplicateAttribute(this, "xmlns", "xmlns", xmlAttributeNode2.Namespace.Prefix.GetString(), "http://www.w3.org/2000/xmlns/");
								}
							}
						}
					}
					return;
				}
				this.CheckAttributes(attributeNodes, attributeCount);
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00006D50 File Offset: 0x00004F50
		private void CheckAttributes(XmlBaseReader.XmlAttributeNode[] attributeNodes, int attributeCount)
		{
			if (this.attributeSorter == null)
			{
				this.attributeSorter = new XmlBaseReader.AttributeSorter();
			}
			if (!this.attributeSorter.Sort(attributeNodes, attributeCount))
			{
				int num;
				int num2;
				this.attributeSorter.GetIndeces(out num, out num2);
				if (attributeNodes[num].QNameType == XmlBaseReader.QNameType.Xmlns)
				{
					XmlExceptionHelper.ThrowDuplicateXmlnsAttribute(this, attributeNodes[num].Namespace.Prefix.GetString(), "http://www.w3.org/2000/xmlns/");
					return;
				}
				XmlExceptionHelper.ThrowDuplicateAttribute(this, attributeNodes[num].Prefix.GetString(), attributeNodes[num2].Prefix.GetString(), attributeNodes[num].LocalName.GetString(), attributeNodes[num].Namespace.Uri.GetString());
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006DF6 File Offset: 0x00004FF6
		public override void MoveToAttribute(int index)
		{
			this.MoveToNode(this.GetAttributeNode(index));
			this.attributeIndex = index;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006E0C File Offset: 0x0000500C
		public override bool MoveToAttribute(string name)
		{
			XmlBaseReader.XmlNode attributeNode = this.GetAttributeNode(name);
			if (attributeNode == null)
			{
				return false;
			}
			this.MoveToNode(attributeNode);
			this.attributeIndex = this.attributeStart;
			return true;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006E3C File Offset: 0x0000503C
		public override bool MoveToAttribute(string localName, string namespaceUri)
		{
			XmlBaseReader.XmlNode attributeNode = this.GetAttributeNode(localName, namespaceUri);
			if (attributeNode == null)
			{
				return false;
			}
			this.MoveToNode(attributeNode);
			this.attributeIndex = this.attributeStart;
			return true;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00006E6B File Offset: 0x0000506B
		public override bool MoveToElement()
		{
			if (!this.node.CanMoveToElement)
			{
				return false;
			}
			if (this.depth == 0)
			{
				this.MoveToDeclaration();
			}
			else
			{
				this.MoveToNode(this.elementNodes[this.depth]);
			}
			this.attributeIndex = -1;
			return true;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00006EA8 File Offset: 0x000050A8
		public override XmlNodeType MoveToContent()
		{
			do
			{
				if (this.node.HasContent)
				{
					if ((this.node.NodeType != XmlNodeType.Text && this.node.NodeType != XmlNodeType.CDATA) || this.trailByteCount > 0)
					{
						break;
					}
					if (this.value == null)
					{
						if (!this.node.Value.IsWhitespace())
						{
							break;
						}
					}
					else if (!XmlConverter.IsWhitespace(this.value))
					{
						break;
					}
				}
				else if (this.node.NodeType == XmlNodeType.Attribute)
				{
					goto Block_6;
				}
			}
			while (this.Read());
			goto IL_007C;
			Block_6:
			this.MoveToElement();
			IL_007C:
			return this.node.NodeType;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00006F3C File Offset: 0x0000513C
		public override bool MoveToFirstAttribute()
		{
			if (!this.node.CanGetAttribute || this.attributeCount == 0)
			{
				return false;
			}
			this.MoveToNode(this.GetAttributeNode(0));
			this.attributeIndex = 0;
			return true;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006F6C File Offset: 0x0000516C
		public override bool MoveToNextAttribute()
		{
			if (!this.node.CanGetAttribute)
			{
				return false;
			}
			int num = this.attributeIndex + 1;
			if (num >= this.attributeCount)
			{
				return false;
			}
			this.MoveToNode(this.GetAttributeNode(num));
			this.attributeIndex = num;
			return true;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00006FB1 File Offset: 0x000051B1
		public override string NamespaceURI
		{
			get
			{
				if (this.ns == null)
				{
					this.ns = this.GetNamespaceUri(true);
				}
				return this.ns;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00006FD0 File Offset: 0x000051D0
		public override XmlNameTable NameTable
		{
			get
			{
				if (this.nameTable == null)
				{
					this.nameTable = new XmlBaseReader.QuotaNameTable(this, this.quotas.MaxNameTableCharCount);
					this.nameTable.Add("xml");
					this.nameTable.Add("xmlns");
					this.nameTable.Add("http://www.w3.org/2000/xmlns/");
					this.nameTable.Add("http://www.w3.org/XML/1998/namespace");
					for (PrefixHandleType prefixHandleType = PrefixHandleType.A; prefixHandleType <= PrefixHandleType.Z; prefixHandleType++)
					{
						this.nameTable.Add(PrefixHandle.GetString(prefixHandleType));
					}
				}
				return this.nameTable;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00007065 File Offset: 0x00005265
		public sealed override XmlNodeType NodeType
		{
			get
			{
				return this.node.NodeType;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00007074 File Offset: 0x00005274
		public override string Prefix
		{
			get
			{
				if (this.prefix == null)
				{
					XmlBaseReader.QNameType qnameType = this.node.QNameType;
					if (qnameType == XmlBaseReader.QNameType.Normal)
					{
						this.prefix = this.node.Prefix.GetString(this.NameTable);
					}
					else if (qnameType == XmlBaseReader.QNameType.Xmlns)
					{
						if (this.node.Namespace.Prefix.IsEmpty)
						{
							this.prefix = string.Empty;
						}
						else
						{
							this.prefix = "xmlns";
						}
					}
					else
					{
						this.prefix = "xml";
					}
				}
				return this.prefix;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000070FC File Offset: 0x000052FC
		public override char QuoteChar
		{
			get
			{
				return this.node.QuoteChar;
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000710C File Offset: 0x0000530C
		private string GetLocalName(bool enforceAtomization)
		{
			if (this.localName != null)
			{
				return this.localName;
			}
			if (this.node.QNameType == XmlBaseReader.QNameType.Normal)
			{
				if (enforceAtomization || this.nameTable != null)
				{
					return this.node.LocalName.GetString(this.NameTable);
				}
				return this.node.LocalName.GetString();
			}
			else
			{
				if (this.node.Namespace.Prefix.IsEmpty)
				{
					return "xmlns";
				}
				if (enforceAtomization || this.nameTable != null)
				{
					return this.node.Namespace.Prefix.GetString(this.NameTable);
				}
				return this.node.Namespace.Prefix.GetString();
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000071C4 File Offset: 0x000053C4
		private string GetNamespaceUri(bool enforceAtomization)
		{
			if (this.ns != null)
			{
				return this.ns;
			}
			if (this.node.QNameType != XmlBaseReader.QNameType.Normal)
			{
				return "http://www.w3.org/2000/xmlns/";
			}
			if (enforceAtomization || this.nameTable != null)
			{
				return this.node.Namespace.Uri.GetString(this.NameTable);
			}
			return this.node.Namespace.Uri.GetString();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000722F File Offset: 0x0000542F
		public override void GetNonAtomizedNames(out string localName, out string namespaceUri)
		{
			localName = this.GetLocalName(false);
			namespaceUri = this.GetNamespaceUri(false);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00007243 File Offset: 0x00005443
		public override bool IsLocalName(string localName)
		{
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
			}
			return this.node.IsLocalName(localName);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007264 File Offset: 0x00005464
		public override bool IsLocalName(XmlDictionaryString localName)
		{
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
			}
			return this.node.IsLocalName(localName);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007285 File Offset: 0x00005485
		public override bool IsNamespaceUri(string namespaceUri)
		{
			if (namespaceUri == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
			}
			return this.node.IsNamespaceUri(namespaceUri);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000072A1 File Offset: 0x000054A1
		public override bool IsNamespaceUri(XmlDictionaryString namespaceUri)
		{
			if (namespaceUri == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
			}
			return this.node.IsNamespaceUri(namespaceUri);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000072C0 File Offset: 0x000054C0
		public sealed override bool IsStartElement()
		{
			XmlNodeType nodeType = this.node.NodeType;
			if (nodeType == XmlNodeType.Element)
			{
				return true;
			}
			if (nodeType == XmlNodeType.EndElement)
			{
				return false;
			}
			if (nodeType == XmlNodeType.None)
			{
				this.Read();
				if (this.node.NodeType == XmlNodeType.Element)
				{
					return true;
				}
			}
			return this.MoveToContent() == XmlNodeType.Element;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000730C File Offset: 0x0000550C
		public override bool IsStartElement(string name)
		{
			if (name == null)
			{
				return false;
			}
			int num = name.IndexOf(':');
			string text;
			string text2;
			if (num == -1)
			{
				text = string.Empty;
				text2 = name;
			}
			else
			{
				text = name.Substring(0, num);
				text2 = name.Substring(num + 1);
			}
			return (this.node.NodeType == XmlNodeType.Element || this.IsStartElement()) && this.node.Prefix == text && this.node.LocalName == text2;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007384 File Offset: 0x00005584
		public override bool IsStartElement(string localName, string namespaceUri)
		{
			return localName != null && namespaceUri != null && ((this.node.NodeType == XmlNodeType.Element || this.IsStartElement()) && this.node.LocalName == localName) && this.node.IsNamespaceUri(namespaceUri);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000073D4 File Offset: 0x000055D4
		public override bool IsStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
			}
			if (namespaceUri == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
			}
			return (this.node.NodeType == XmlNodeType.Element || this.IsStartElement()) && this.node.LocalName == localName && this.node.IsNamespaceUri(namespaceUri);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007434 File Offset: 0x00005634
		public override int IndexOfLocalName(string[] localNames, string namespaceUri)
		{
			if (localNames == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localNames");
			}
			if (namespaceUri == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
			}
			XmlBaseReader.QNameType qnameType = this.node.QNameType;
			if (this.node.IsNamespaceUri(namespaceUri))
			{
				if (qnameType == XmlBaseReader.QNameType.Normal)
				{
					StringHandle stringHandle = this.node.LocalName;
					for (int i = 0; i < localNames.Length; i++)
					{
						string text = localNames[i];
						if (text == null)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "localNames[{0}]", i));
						}
						if (stringHandle == text)
						{
							return i;
						}
					}
				}
				else
				{
					PrefixHandle prefixHandle = this.node.Namespace.Prefix;
					for (int j = 0; j < localNames.Length; j++)
					{
						string text2 = localNames[j];
						if (text2 == null)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "localNames[{0}]", j));
						}
						if (prefixHandle == text2)
						{
							return j;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007520 File Offset: 0x00005720
		public override int IndexOfLocalName(XmlDictionaryString[] localNames, XmlDictionaryString namespaceUri)
		{
			if (localNames == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localNames");
			}
			if (namespaceUri == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
			}
			XmlBaseReader.QNameType qnameType = this.node.QNameType;
			if (this.node.IsNamespaceUri(namespaceUri))
			{
				if (qnameType == XmlBaseReader.QNameType.Normal)
				{
					StringHandle stringHandle = this.node.LocalName;
					for (int i = 0; i < localNames.Length; i++)
					{
						XmlDictionaryString xmlDictionaryString = localNames[i];
						if (xmlDictionaryString == null)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "localNames[{0}]", i));
						}
						if (stringHandle == xmlDictionaryString)
						{
							return i;
						}
					}
				}
				else
				{
					PrefixHandle prefixHandle = this.node.Namespace.Prefix;
					for (int j = 0; j < localNames.Length; j++)
					{
						XmlDictionaryString xmlDictionaryString2 = localNames[j];
						if (xmlDictionaryString2 == null)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "localNames[{0}]", j));
						}
						if (prefixHandle == xmlDictionaryString2)
						{
							return j;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000760C File Offset: 0x0000580C
		public override int ReadValueChunk(char[] chars, int offset, int count)
		{
			if (chars == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > chars.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { chars.Length })));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > chars.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { chars.Length - offset })));
			}
			int num;
			if (this.value == null && this.node.QNameType == XmlBaseReader.QNameType.Normal && this.node.Value.TryReadChars(chars, offset, count, out num))
			{
				return num;
			}
			string text = this.Value;
			num = Math.Min(count, text.Length);
			text.CopyTo(0, chars, offset, num);
			this.value = text.Substring(num);
			return num;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00007728 File Offset: 0x00005928
		public override int ReadValueAsBase64(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > buffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { buffer.Length })));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { buffer.Length - offset })));
			}
			if (count == 0)
			{
				return 0;
			}
			int num;
			if (this.value == null && this.trailByteCount == 0 && this.trailCharCount == 0 && this.node.QNameType == XmlBaseReader.QNameType.Normal && this.node.Value.TryReadBase64(buffer, offset, count, out num))
			{
				return num;
			}
			return this.ReadBytes(XmlBaseReader.Base64Encoding, 3, 4, buffer, offset, Math.Min(count, 512), false);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007848 File Offset: 0x00005A48
		public override string ReadElementContentAsString()
		{
			if (this.node.NodeType != XmlNodeType.Element)
			{
				this.MoveToStartElement();
			}
			if (this.node.IsEmptyElement)
			{
				this.Read();
				return string.Empty;
			}
			this.Read();
			string text = this.ReadContentAsString();
			this.ReadEndElement();
			return text;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007896 File Offset: 0x00005A96
		public override string ReadElementString()
		{
			this.MoveToStartElement();
			if (this.IsEmptyElement)
			{
				this.Read();
				return string.Empty;
			}
			this.Read();
			string text = this.ReadString();
			this.ReadEndElement();
			return text;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000078C6 File Offset: 0x00005AC6
		public override string ReadElementString(string name)
		{
			this.MoveToStartElement(name);
			return this.ReadElementString();
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000078D5 File Offset: 0x00005AD5
		public override string ReadElementString(string localName, string namespaceUri)
		{
			this.MoveToStartElement(localName, namespaceUri);
			return this.ReadElementString();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000078E5 File Offset: 0x00005AE5
		public override void ReadStartElement()
		{
			if (this.node.NodeType != XmlNodeType.Element)
			{
				this.MoveToStartElement();
			}
			this.Read();
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007902 File Offset: 0x00005B02
		public override void ReadStartElement(string name)
		{
			this.MoveToStartElement(name);
			this.Read();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007912 File Offset: 0x00005B12
		public override void ReadStartElement(string localName, string namespaceUri)
		{
			this.MoveToStartElement(localName, namespaceUri);
			this.Read();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00007924 File Offset: 0x00005B24
		public override void ReadEndElement()
		{
			if (this.node.NodeType != XmlNodeType.EndElement && this.MoveToContent() != XmlNodeType.EndElement)
			{
				int num = ((this.node.NodeType == XmlNodeType.Element) ? (this.depth - 1) : this.depth);
				if (num == 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("No corresponding start element is open.")));
				}
				XmlBaseReader.XmlElementNode xmlElementNode = this.elementNodes[num];
				XmlExceptionHelper.ThrowEndElementExpected(this, xmlElementNode.LocalName.GetString(), xmlElementNode.Namespace.Uri.GetString());
			}
			this.Read();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000079B4 File Offset: 0x00005BB4
		public override bool ReadAttributeValue()
		{
			XmlBaseReader.XmlAttributeTextNode attributeText = this.node.AttributeText;
			if (attributeText == null)
			{
				return false;
			}
			this.MoveToNode(attributeText);
			return true;
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000079DA File Offset: 0x00005BDA
		public override ReadState ReadState
		{
			get
			{
				return this.node.ReadState;
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000079E7 File Offset: 0x00005BE7
		private void SkipValue(XmlBaseReader.XmlNode node)
		{
			if (node.SkipValue)
			{
				this.Read();
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000079F8 File Offset: 0x00005BF8
		public override bool TryGetBase64ContentLength(out int length)
		{
			if (this.trailByteCount == 0 && this.trailCharCount == 0 && this.value == null)
			{
				XmlBaseReader.XmlNode xmlNode = this.Node;
				if (xmlNode.IsAtomicValue)
				{
					return xmlNode.Value.TryGetByteArrayLength(out length);
				}
			}
			return base.TryGetBase64ContentLength(out length);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007A40 File Offset: 0x00005C40
		public override byte[] ReadContentAsBase64()
		{
			if (this.trailByteCount == 0 && this.trailCharCount == 0 && this.value == null)
			{
				XmlBaseReader.XmlNode xmlNode = this.Node;
				if (xmlNode.IsAtomicValue)
				{
					byte[] array = xmlNode.Value.ToByteArray();
					if (array.Length > this.quotas.MaxArrayLength)
					{
						XmlExceptionHelper.ThrowMaxArrayLengthExceeded(this, this.quotas.MaxArrayLength);
					}
					this.SkipValue(xmlNode);
					return array;
				}
			}
			if (!this.bufferReader.IsStreamed)
			{
				return base.ReadContentAsBase64(this.quotas.MaxArrayLength, this.bufferReader.Buffer.Length);
			}
			return base.ReadContentAsBase64(this.quotas.MaxArrayLength, 65535);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007AEC File Offset: 0x00005CEC
		public override int ReadElementContentAsBase64(byte[] buffer, int offset, int count)
		{
			if (!this.readingElement)
			{
				if (this.IsEmptyElement)
				{
					this.Read();
					return 0;
				}
				this.ReadStartElement();
				this.readingElement = true;
			}
			int num = this.ReadContentAsBase64(buffer, offset, count);
			if (num == 0)
			{
				this.ReadEndElement();
				this.readingElement = false;
			}
			return num;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007B38 File Offset: 0x00005D38
		public override int ReadContentAsBase64(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > buffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { buffer.Length })));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { buffer.Length - offset })));
			}
			if (count == 0)
			{
				return 0;
			}
			if (this.trailByteCount == 0 && this.trailCharCount == 0 && this.value == null && this.node.QNameType == XmlBaseReader.QNameType.Normal)
			{
				int num;
				while (this.node.NodeType != XmlNodeType.Comment && this.node.Value.TryReadBase64(buffer, offset, count, out num))
				{
					if (num != 0)
					{
						return num;
					}
					this.Read();
				}
			}
			XmlNodeType nodeType = this.node.NodeType;
			if (nodeType == XmlNodeType.Element || nodeType == XmlNodeType.EndElement)
			{
				return 0;
			}
			return this.ReadBytes(XmlBaseReader.Base64Encoding, 3, 4, buffer, offset, Math.Min(count, 512), true);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007C89 File Offset: 0x00005E89
		public override byte[] ReadContentAsBinHex()
		{
			return base.ReadContentAsBinHex(this.quotas.MaxArrayLength);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007C9C File Offset: 0x00005E9C
		public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int count)
		{
			if (!this.readingElement)
			{
				if (this.IsEmptyElement)
				{
					this.Read();
					return 0;
				}
				this.ReadStartElement();
				this.readingElement = true;
			}
			int num = this.ReadContentAsBinHex(buffer, offset, count);
			if (num == 0)
			{
				this.ReadEndElement();
				this.readingElement = false;
			}
			return num;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007CE8 File Offset: 0x00005EE8
		public override int ReadContentAsBinHex(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > buffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { buffer.Length })));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { buffer.Length - offset })));
			}
			if (count == 0)
			{
				return 0;
			}
			return this.ReadBytes(XmlBaseReader.BinHexEncoding, 1, 2, buffer, offset, Math.Min(count, 512), true);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007DCC File Offset: 0x00005FCC
		private int ReadBytes(Encoding encoding, int byteBlock, int charBlock, byte[] buffer, int offset, int byteCount, bool readContent)
		{
			if (this.trailByteCount > 0)
			{
				int num = Math.Min(this.trailByteCount, byteCount);
				Array.Copy(this.trailBytes, 0, buffer, offset, num);
				this.trailByteCount -= num;
				Array.Copy(this.trailBytes, num, this.trailBytes, 0, this.trailByteCount);
				return num;
			}
			XmlNodeType nodeType = this.node.NodeType;
			if (nodeType == XmlNodeType.Element || nodeType == XmlNodeType.EndElement)
			{
				return 0;
			}
			int num2;
			if (byteCount < byteBlock)
			{
				num2 = charBlock;
			}
			else
			{
				num2 = byteCount / byteBlock * charBlock;
			}
			char[] charBuffer = this.GetCharBuffer(num2);
			int i = 0;
			int num5;
			for (;;)
			{
				if (this.trailCharCount > 0)
				{
					Array.Copy(this.trailChars, 0, charBuffer, i, this.trailCharCount);
					i += this.trailCharCount;
					this.trailCharCount = 0;
				}
				while (i < charBlock)
				{
					int num3;
					if (readContent)
					{
						num3 = this.ReadContentAsChars(charBuffer, i, num2 - i);
						if (num3 == 1 && charBuffer[i] == '\n')
						{
							continue;
						}
					}
					else
					{
						num3 = this.ReadValueChunk(charBuffer, i, num2 - i);
					}
					if (num3 == 0)
					{
						break;
					}
					i += num3;
				}
				if (i >= charBlock)
				{
					this.trailCharCount = i % charBlock;
					if (this.trailCharCount > 0)
					{
						if (this.trailChars == null)
						{
							this.trailChars = new char[4];
						}
						i -= this.trailCharCount;
						Array.Copy(charBuffer, i, this.trailChars, 0, this.trailCharCount);
					}
				}
				try
				{
					if (byteCount < byteBlock)
					{
						if (this.trailBytes == null)
						{
							this.trailBytes = new byte[3];
						}
						this.trailByteCount = encoding.GetBytes(charBuffer, 0, i, this.trailBytes, 0);
						int num4 = Math.Min(this.trailByteCount, byteCount);
						Array.Copy(this.trailBytes, 0, buffer, offset, num4);
						this.trailByteCount -= num4;
						Array.Copy(this.trailBytes, num4, this.trailBytes, 0, this.trailByteCount);
						num5 = num4;
					}
					else
					{
						num5 = encoding.GetBytes(charBuffer, 0, i, buffer, offset);
					}
				}
				catch (FormatException ex)
				{
					int num6 = 0;
					int num7 = 0;
					for (;;)
					{
						if (num7 >= i || !XmlConverter.IsWhitespace(charBuffer[num7]))
						{
							if (num7 == i)
							{
								break;
							}
							charBuffer[num6++] = charBuffer[num7++];
						}
						else
						{
							num7++;
						}
					}
					if (num6 == i)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(ex.Message, ex.InnerException));
					}
					i = num6;
					continue;
				}
				break;
			}
			return num5;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00008014 File Offset: 0x00006214
		public override string ReadContentAsString()
		{
			XmlBaseReader.XmlNode xmlNode = this.Node;
			if (xmlNode.IsAtomicValue)
			{
				string @string;
				if (this.value != null)
				{
					@string = this.value;
					if (xmlNode.AttributeText == null)
					{
						this.value = string.Empty;
					}
				}
				else
				{
					@string = xmlNode.Value.GetString();
					this.SkipValue(xmlNode);
					if (@string.Length > this.quotas.MaxStringContentLength)
					{
						XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, this.quotas.MaxStringContentLength);
					}
				}
				return @string;
			}
			return base.ReadContentAsString(this.quotas.MaxStringContentLength);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000080A0 File Offset: 0x000062A0
		public override bool ReadContentAsBoolean()
		{
			XmlBaseReader.XmlNode xmlNode = this.Node;
			if (this.value == null && xmlNode.IsAtomicValue)
			{
				bool flag = xmlNode.Value.ToBoolean();
				this.SkipValue(xmlNode);
				return flag;
			}
			return XmlConverter.ToBoolean(this.ReadContentAsString());
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000080E4 File Offset: 0x000062E4
		public override long ReadContentAsLong()
		{
			XmlBaseReader.XmlNode xmlNode = this.Node;
			if (this.value == null && xmlNode.IsAtomicValue)
			{
				long num = xmlNode.Value.ToLong();
				this.SkipValue(xmlNode);
				return num;
			}
			return XmlConverter.ToInt64(this.ReadContentAsString());
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008128 File Offset: 0x00006328
		public override int ReadContentAsInt()
		{
			XmlBaseReader.XmlNode xmlNode = this.Node;
			if (this.value == null && xmlNode.IsAtomicValue)
			{
				int num = xmlNode.Value.ToInt();
				this.SkipValue(xmlNode);
				return num;
			}
			return XmlConverter.ToInt32(this.ReadContentAsString());
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000816C File Offset: 0x0000636C
		public override DateTime ReadContentAsDateTime()
		{
			XmlBaseReader.XmlNode xmlNode = this.Node;
			if (this.value == null && xmlNode.IsAtomicValue)
			{
				DateTime dateTime = xmlNode.Value.ToDateTime();
				this.SkipValue(xmlNode);
				return dateTime;
			}
			return XmlConverter.ToDateTime(this.ReadContentAsString());
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000081B0 File Offset: 0x000063B0
		public override double ReadContentAsDouble()
		{
			XmlBaseReader.XmlNode xmlNode = this.Node;
			if (this.value == null && xmlNode.IsAtomicValue)
			{
				double num = xmlNode.Value.ToDouble();
				this.SkipValue(xmlNode);
				return num;
			}
			return XmlConverter.ToDouble(this.ReadContentAsString());
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000081F4 File Offset: 0x000063F4
		public override float ReadContentAsFloat()
		{
			XmlBaseReader.XmlNode xmlNode = this.Node;
			if (this.value == null && xmlNode.IsAtomicValue)
			{
				float num = xmlNode.Value.ToSingle();
				this.SkipValue(xmlNode);
				return num;
			}
			return XmlConverter.ToSingle(this.ReadContentAsString());
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008238 File Offset: 0x00006438
		public override decimal ReadContentAsDecimal()
		{
			XmlBaseReader.XmlNode xmlNode = this.Node;
			if (this.value == null && xmlNode.IsAtomicValue)
			{
				decimal num = xmlNode.Value.ToDecimal();
				this.SkipValue(xmlNode);
				return num;
			}
			return XmlConverter.ToDecimal(this.ReadContentAsString());
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000827C File Offset: 0x0000647C
		public override UniqueId ReadContentAsUniqueId()
		{
			XmlBaseReader.XmlNode xmlNode = this.Node;
			if (this.value == null && xmlNode.IsAtomicValue)
			{
				UniqueId uniqueId = xmlNode.Value.ToUniqueId();
				this.SkipValue(xmlNode);
				return uniqueId;
			}
			return XmlConverter.ToUniqueId(this.ReadContentAsString());
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000082C0 File Offset: 0x000064C0
		public override TimeSpan ReadContentAsTimeSpan()
		{
			XmlBaseReader.XmlNode xmlNode = this.Node;
			if (this.value == null && xmlNode.IsAtomicValue)
			{
				TimeSpan timeSpan = xmlNode.Value.ToTimeSpan();
				this.SkipValue(xmlNode);
				return timeSpan;
			}
			return XmlConverter.ToTimeSpan(this.ReadContentAsString());
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008304 File Offset: 0x00006504
		public override Guid ReadContentAsGuid()
		{
			XmlBaseReader.XmlNode xmlNode = this.Node;
			if (this.value == null && xmlNode.IsAtomicValue)
			{
				Guid guid = xmlNode.Value.ToGuid();
				this.SkipValue(xmlNode);
				return guid;
			}
			return XmlConverter.ToGuid(this.ReadContentAsString());
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008348 File Offset: 0x00006548
		public override object ReadContentAsObject()
		{
			XmlBaseReader.XmlNode xmlNode = this.Node;
			if (this.value == null && xmlNode.IsAtomicValue)
			{
				object obj = xmlNode.Value.ToObject();
				this.SkipValue(xmlNode);
				return obj;
			}
			return this.ReadContentAsString();
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008388 File Offset: 0x00006588
		public override object ReadContentAs(Type type, IXmlNamespaceResolver namespaceResolver)
		{
			if (type == typeof(ulong))
			{
				if (this.value == null && this.node.IsAtomicValue)
				{
					ulong num = this.node.Value.ToULong();
					this.SkipValue(this.node);
					return num;
				}
				return XmlConverter.ToUInt64(this.ReadContentAsString());
			}
			else
			{
				if (type == typeof(bool))
				{
					return this.ReadContentAsBoolean();
				}
				if (type == typeof(int))
				{
					return this.ReadContentAsInt();
				}
				if (type == typeof(long))
				{
					return this.ReadContentAsLong();
				}
				if (type == typeof(float))
				{
					return this.ReadContentAsFloat();
				}
				if (type == typeof(double))
				{
					return this.ReadContentAsDouble();
				}
				if (type == typeof(decimal))
				{
					return this.ReadContentAsDecimal();
				}
				if (type == typeof(DateTime))
				{
					return this.ReadContentAsDateTime();
				}
				if (type == typeof(UniqueId))
				{
					return this.ReadContentAsUniqueId();
				}
				if (type == typeof(Guid))
				{
					return this.ReadContentAsGuid();
				}
				if (type == typeof(TimeSpan))
				{
					return this.ReadContentAsTimeSpan();
				}
				if (type == typeof(object))
				{
					return this.ReadContentAsObject();
				}
				return base.ReadContentAs(type, namespaceResolver);
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008537 File Offset: 0x00006737
		public override void ResolveEntity()
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("The reader cannot be advanced.")));
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008550 File Offset: 0x00006750
		public override void Skip()
		{
			if (this.node.ReadState != ReadState.Interactive)
			{
				return;
			}
			if ((this.node.NodeType == XmlNodeType.Element || this.MoveToElement()) && !this.IsEmptyElement)
			{
				int num = this.Depth;
				while (this.Read() && num < this.Depth)
				{
				}
				if (this.node.NodeType == XmlNodeType.EndElement)
				{
					this.Read();
					return;
				}
			}
			else
			{
				this.Read();
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600018E RID: 398 RVA: 0x000085C0 File Offset: 0x000067C0
		public override string Value
		{
			get
			{
				if (this.value == null)
				{
					this.value = this.node.ValueAsString;
				}
				return this.value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000085E4 File Offset: 0x000067E4
		public override Type ValueType
		{
			get
			{
				if (this.value == null && this.node.QNameType == XmlBaseReader.QNameType.Normal)
				{
					Type type = this.node.Value.ToType();
					if (this.node.IsAtomicValue)
					{
						return type;
					}
					if (type == typeof(byte[]))
					{
						return type;
					}
				}
				return typeof(string);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00008644 File Offset: 0x00006844
		public override string XmlLang
		{
			get
			{
				return this.nsMgr.XmlLang;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00008651 File Offset: 0x00006851
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.nsMgr.XmlSpace;
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000865E File Offset: 0x0000685E
		public override bool TryGetLocalNameAsDictionaryString(out XmlDictionaryString localName)
		{
			return this.node.TryGetLocalNameAsDictionaryString(out localName);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000866C File Offset: 0x0000686C
		public override bool TryGetNamespaceUriAsDictionaryString(out XmlDictionaryString localName)
		{
			return this.node.TryGetNamespaceUriAsDictionaryString(out localName);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000867A File Offset: 0x0000687A
		public override bool TryGetValueAsDictionaryString(out XmlDictionaryString value)
		{
			return this.node.TryGetValueAsDictionaryString(out value);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00008688 File Offset: 0x00006888
		public override short[] ReadInt16Array(string localName, string namespaceUri)
		{
			return Int16ArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000086A2 File Offset: 0x000068A2
		public override short[] ReadInt16Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return Int16ArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000086BC File Offset: 0x000068BC
		public override int[] ReadInt32Array(string localName, string namespaceUri)
		{
			return Int32ArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000086D6 File Offset: 0x000068D6
		public override int[] ReadInt32Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return Int32ArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000086F0 File Offset: 0x000068F0
		public override long[] ReadInt64Array(string localName, string namespaceUri)
		{
			return Int64ArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000870A File Offset: 0x0000690A
		public override long[] ReadInt64Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return Int64ArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008724 File Offset: 0x00006924
		public override float[] ReadSingleArray(string localName, string namespaceUri)
		{
			return SingleArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000873E File Offset: 0x0000693E
		public override float[] ReadSingleArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return SingleArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008758 File Offset: 0x00006958
		public override double[] ReadDoubleArray(string localName, string namespaceUri)
		{
			return DoubleArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008772 File Offset: 0x00006972
		public override double[] ReadDoubleArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return DoubleArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000878C File Offset: 0x0000698C
		public override decimal[] ReadDecimalArray(string localName, string namespaceUri)
		{
			return DecimalArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000087A6 File Offset: 0x000069A6
		public override decimal[] ReadDecimalArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return DecimalArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000087C0 File Offset: 0x000069C0
		public override DateTime[] ReadDateTimeArray(string localName, string namespaceUri)
		{
			return DateTimeArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000087DA File Offset: 0x000069DA
		public override DateTime[] ReadDateTimeArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return DateTimeArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000087F4 File Offset: 0x000069F4
		public override Guid[] ReadGuidArray(string localName, string namespaceUri)
		{
			return GuidArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000880E File Offset: 0x00006A0E
		public override Guid[] ReadGuidArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return GuidArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00008828 File Offset: 0x00006A28
		public override TimeSpan[] ReadTimeSpanArray(string localName, string namespaceUri)
		{
			return TimeSpanArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00008842 File Offset: 0x00006A42
		public override TimeSpan[] ReadTimeSpanArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			return TimeSpanArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, this.quotas.MaxArrayLength);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000885C File Offset: 0x00006A5C
		public string GetOpenElements()
		{
			string text = string.Empty;
			for (int i = this.depth; i > 0; i--)
			{
				string @string = this.elementNodes[i].LocalName.GetString();
				if (i != this.depth)
				{
					text += ", ";
				}
				text += @string;
			}
			return text;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000088B1 File Offset: 0x00006AB1
		private char[] GetCharBuffer(int count)
		{
			if (count > 1024)
			{
				return new char[count];
			}
			if (this.chars == null || this.chars.Length < count)
			{
				this.chars = new char[count];
			}
			return this.chars;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000088E8 File Offset: 0x00006AE8
		private void SignStartElement(XmlSigningNodeWriter writer)
		{
			int num;
			int num2;
			byte[] @string = this.node.Prefix.GetString(out num, out num2);
			int num3;
			int num4;
			byte[] string2 = this.node.LocalName.GetString(out num3, out num4);
			writer.WriteStartElement(@string, num, num2, string2, num3, num4);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00008930 File Offset: 0x00006B30
		private void SignAttribute(XmlSigningNodeWriter writer, XmlBaseReader.XmlAttributeNode attributeNode)
		{
			if (attributeNode.QNameType == XmlBaseReader.QNameType.Normal)
			{
				int num;
				int num2;
				byte[] @string = attributeNode.Prefix.GetString(out num, out num2);
				int num3;
				int num4;
				byte[] string2 = attributeNode.LocalName.GetString(out num3, out num4);
				writer.WriteStartAttribute(@string, num, num2, string2, num3, num4);
				attributeNode.Value.Sign(writer);
				writer.WriteEndAttribute();
				return;
			}
			int num5;
			int num6;
			byte[] string3 = attributeNode.Namespace.Prefix.GetString(out num5, out num6);
			int num7;
			int num8;
			byte[] string4 = attributeNode.Namespace.Uri.GetString(out num7, out num8);
			writer.WriteXmlnsAttribute(string3, num5, num6, string4, num7, num8);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000089C8 File Offset: 0x00006BC8
		private void SignEndElement(XmlSigningNodeWriter writer)
		{
			int num;
			int num2;
			byte[] @string = this.node.Prefix.GetString(out num, out num2);
			int num3;
			int num4;
			byte[] string2 = this.node.LocalName.GetString(out num3, out num4);
			writer.WriteEndElement(@string, num, num2, string2, num3, num4);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00008A10 File Offset: 0x00006C10
		private void SignNode(XmlSigningNodeWriter writer)
		{
			XmlNodeType nodeType = this.node.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.None:
				return;
			case XmlNodeType.Element:
			{
				this.SignStartElement(writer);
				for (int i = 0; i < this.attributeCount; i++)
				{
					this.SignAttribute(writer, this.attributeNodes[i]);
				}
				writer.WriteEndStartElement(this.node.IsEmptyElement);
				return;
			}
			case XmlNodeType.Attribute:
			case XmlNodeType.EntityReference:
			case XmlNodeType.Entity:
			case XmlNodeType.ProcessingInstruction:
				goto IL_00C6;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				break;
			case XmlNodeType.Comment:
				writer.WriteComment(this.node.Value.GetString());
				return;
			default:
				switch (nodeType)
				{
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					break;
				case XmlNodeType.EndElement:
					this.SignEndElement(writer);
					return;
				case XmlNodeType.EndEntity:
					goto IL_00C6;
				case XmlNodeType.XmlDeclaration:
					writer.WriteDeclaration();
					return;
				default:
					goto IL_00C6;
				}
				break;
			}
			this.node.Value.Sign(writer);
			return;
			IL_00C6:
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00008AEE File Offset: 0x00006CEE
		public override bool CanCanonicalize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00008AF1 File Offset: 0x00006CF1
		protected bool Signing
		{
			get
			{
				return this.signing;
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00008AF9 File Offset: 0x00006CF9
		protected void SignNode()
		{
			if (this.signing)
			{
				this.SignNode(this.signingWriter);
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00008B10 File Offset: 0x00006D10
		public override void StartCanonicalization(Stream stream, bool includeComments, string[] inclusivePrefixes)
		{
			if (this.signing)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("XML canonicalization started")));
			}
			if (this.signingWriter == null)
			{
				this.signingWriter = this.CreateSigningNodeWriter();
			}
			this.signingWriter.SetOutput(XmlNodeWriter.Null, stream, includeComments, inclusivePrefixes);
			this.nsMgr.Sign(this.signingWriter);
			this.signing = true;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00008B79 File Offset: 0x00006D79
		public override void EndCanonicalization()
		{
			if (!this.signing)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("XML canonicalization was not started.")));
			}
			this.signingWriter.Flush();
			this.signingWriter.Close();
			this.signing = false;
		}

		// Token: 0x060001B2 RID: 434
		protected abstract XmlSigningNodeWriter CreateSigningNodeWriter();

		// Token: 0x04000097 RID: 151
		private XmlBufferReader bufferReader;

		// Token: 0x04000098 RID: 152
		private XmlBaseReader.XmlNode node;

		// Token: 0x04000099 RID: 153
		private XmlBaseReader.NamespaceManager nsMgr;

		// Token: 0x0400009A RID: 154
		private XmlBaseReader.XmlElementNode[] elementNodes;

		// Token: 0x0400009B RID: 155
		private XmlBaseReader.XmlAttributeNode[] attributeNodes;

		// Token: 0x0400009C RID: 156
		private XmlBaseReader.XmlAtomicTextNode atomicTextNode;

		// Token: 0x0400009D RID: 157
		private int depth;

		// Token: 0x0400009E RID: 158
		private int attributeCount;

		// Token: 0x0400009F RID: 159
		private int attributeStart;

		// Token: 0x040000A0 RID: 160
		private XmlDictionaryReaderQuotas quotas;

		// Token: 0x040000A1 RID: 161
		private XmlNameTable nameTable;

		// Token: 0x040000A2 RID: 162
		private XmlBaseReader.XmlDeclarationNode declarationNode;

		// Token: 0x040000A3 RID: 163
		private XmlBaseReader.XmlComplexTextNode complexTextNode;

		// Token: 0x040000A4 RID: 164
		private XmlBaseReader.XmlWhitespaceTextNode whitespaceTextNode;

		// Token: 0x040000A5 RID: 165
		private XmlBaseReader.XmlCDataNode cdataNode;

		// Token: 0x040000A6 RID: 166
		private XmlBaseReader.XmlCommentNode commentNode;

		// Token: 0x040000A7 RID: 167
		private XmlBaseReader.XmlElementNode rootElementNode;

		// Token: 0x040000A8 RID: 168
		private int attributeIndex;

		// Token: 0x040000A9 RID: 169
		private char[] chars;

		// Token: 0x040000AA RID: 170
		private string prefix;

		// Token: 0x040000AB RID: 171
		private string localName;

		// Token: 0x040000AC RID: 172
		private string ns;

		// Token: 0x040000AD RID: 173
		private string value;

		// Token: 0x040000AE RID: 174
		private int trailCharCount;

		// Token: 0x040000AF RID: 175
		private int trailByteCount;

		// Token: 0x040000B0 RID: 176
		private char[] trailChars;

		// Token: 0x040000B1 RID: 177
		private byte[] trailBytes;

		// Token: 0x040000B2 RID: 178
		private bool rootElement;

		// Token: 0x040000B3 RID: 179
		private bool readingElement;

		// Token: 0x040000B4 RID: 180
		private XmlSigningNodeWriter signingWriter;

		// Token: 0x040000B5 RID: 181
		private bool signing;

		// Token: 0x040000B6 RID: 182
		private XmlBaseReader.AttributeSorter attributeSorter;

		// Token: 0x040000B7 RID: 183
		private static XmlBaseReader.XmlInitialNode initialNode = new XmlBaseReader.XmlInitialNode(XmlBufferReader.Empty);

		// Token: 0x040000B8 RID: 184
		private static XmlBaseReader.XmlEndOfFileNode endOfFileNode = new XmlBaseReader.XmlEndOfFileNode(XmlBufferReader.Empty);

		// Token: 0x040000B9 RID: 185
		private static XmlBaseReader.XmlClosedNode closedNode = new XmlBaseReader.XmlClosedNode(XmlBufferReader.Empty);

		// Token: 0x040000BA RID: 186
		private static BinHexEncoding binhexEncoding;

		// Token: 0x040000BB RID: 187
		private static Base64Encoding base64Encoding;

		// Token: 0x040000BC RID: 188
		private const string xmlns = "xmlns";

		// Token: 0x040000BD RID: 189
		private const string xml = "xml";

		// Token: 0x040000BE RID: 190
		private const string xmlnsNamespace = "http://www.w3.org/2000/xmlns/";

		// Token: 0x040000BF RID: 191
		private const string xmlNamespace = "http://www.w3.org/XML/1998/namespace";

		// Token: 0x0200012F RID: 303
		protected enum QNameType
		{
			// Token: 0x040008C6 RID: 2246
			Normal,
			// Token: 0x040008C7 RID: 2247
			Xmlns
		}

		// Token: 0x02000130 RID: 304
		protected class XmlNode
		{
			// Token: 0x0600120B RID: 4619 RVA: 0x0004B4C4 File Offset: 0x000496C4
			protected XmlNode(XmlNodeType nodeType, PrefixHandle prefix, StringHandle localName, ValueHandle value, XmlBaseReader.XmlNode.XmlNodeFlags nodeFlags, ReadState readState, XmlBaseReader.XmlAttributeTextNode attributeTextNode, int depthDelta)
			{
				this.nodeType = nodeType;
				this.prefix = prefix;
				this.localName = localName;
				this.value = value;
				this.ns = XmlBaseReader.NamespaceManager.EmptyNamespace;
				this.hasValue = (nodeFlags & XmlBaseReader.XmlNode.XmlNodeFlags.HasValue) > XmlBaseReader.XmlNode.XmlNodeFlags.None;
				this.canGetAttribute = (nodeFlags & XmlBaseReader.XmlNode.XmlNodeFlags.CanGetAttribute) > XmlBaseReader.XmlNode.XmlNodeFlags.None;
				this.canMoveToElement = (nodeFlags & XmlBaseReader.XmlNode.XmlNodeFlags.CanMoveToElement) > XmlBaseReader.XmlNode.XmlNodeFlags.None;
				this.isAtomicValue = (nodeFlags & XmlBaseReader.XmlNode.XmlNodeFlags.AtomicValue) > XmlBaseReader.XmlNode.XmlNodeFlags.None;
				this.skipValue = (nodeFlags & XmlBaseReader.XmlNode.XmlNodeFlags.SkipValue) > XmlBaseReader.XmlNode.XmlNodeFlags.None;
				this.hasContent = (nodeFlags & XmlBaseReader.XmlNode.XmlNodeFlags.HasContent) > XmlBaseReader.XmlNode.XmlNodeFlags.None;
				this.readState = readState;
				this.attributeTextNode = attributeTextNode;
				this.exitScope = nodeType == XmlNodeType.EndElement;
				this.depthDelta = depthDelta;
				this.isEmptyElement = false;
				this.quoteChar = '"';
				this.qnameType = XmlBaseReader.QNameType.Normal;
			}

			// Token: 0x1700038F RID: 911
			// (get) Token: 0x0600120C RID: 4620 RVA: 0x0004B588 File Offset: 0x00049788
			public bool HasValue
			{
				get
				{
					return this.hasValue;
				}
			}

			// Token: 0x17000390 RID: 912
			// (get) Token: 0x0600120D RID: 4621 RVA: 0x0004B590 File Offset: 0x00049790
			public ReadState ReadState
			{
				get
				{
					return this.readState;
				}
			}

			// Token: 0x17000391 RID: 913
			// (get) Token: 0x0600120E RID: 4622 RVA: 0x0004B598 File Offset: 0x00049798
			public StringHandle LocalName
			{
				get
				{
					return this.localName;
				}
			}

			// Token: 0x17000392 RID: 914
			// (get) Token: 0x0600120F RID: 4623 RVA: 0x0004B5A0 File Offset: 0x000497A0
			public PrefixHandle Prefix
			{
				get
				{
					return this.prefix;
				}
			}

			// Token: 0x17000393 RID: 915
			// (get) Token: 0x06001210 RID: 4624 RVA: 0x0004B5A8 File Offset: 0x000497A8
			public bool CanGetAttribute
			{
				get
				{
					return this.canGetAttribute;
				}
			}

			// Token: 0x17000394 RID: 916
			// (get) Token: 0x06001211 RID: 4625 RVA: 0x0004B5B0 File Offset: 0x000497B0
			public bool CanMoveToElement
			{
				get
				{
					return this.canMoveToElement;
				}
			}

			// Token: 0x17000395 RID: 917
			// (get) Token: 0x06001212 RID: 4626 RVA: 0x0004B5B8 File Offset: 0x000497B8
			public XmlBaseReader.XmlAttributeTextNode AttributeText
			{
				get
				{
					return this.attributeTextNode;
				}
			}

			// Token: 0x17000396 RID: 918
			// (get) Token: 0x06001213 RID: 4627 RVA: 0x0004B5C0 File Offset: 0x000497C0
			public bool SkipValue
			{
				get
				{
					return this.skipValue;
				}
			}

			// Token: 0x17000397 RID: 919
			// (get) Token: 0x06001214 RID: 4628 RVA: 0x0004B5C8 File Offset: 0x000497C8
			public ValueHandle Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x17000398 RID: 920
			// (get) Token: 0x06001215 RID: 4629 RVA: 0x0004B5D0 File Offset: 0x000497D0
			public int DepthDelta
			{
				get
				{
					return this.depthDelta;
				}
			}

			// Token: 0x17000399 RID: 921
			// (get) Token: 0x06001216 RID: 4630 RVA: 0x0004B5D8 File Offset: 0x000497D8
			public bool HasContent
			{
				get
				{
					return this.hasContent;
				}
			}

			// Token: 0x1700039A RID: 922
			// (get) Token: 0x06001217 RID: 4631 RVA: 0x0004B5E0 File Offset: 0x000497E0
			// (set) Token: 0x06001218 RID: 4632 RVA: 0x0004B5E8 File Offset: 0x000497E8
			public XmlNodeType NodeType
			{
				get
				{
					return this.nodeType;
				}
				set
				{
					this.nodeType = value;
				}
			}

			// Token: 0x1700039B RID: 923
			// (get) Token: 0x06001219 RID: 4633 RVA: 0x0004B5F1 File Offset: 0x000497F1
			// (set) Token: 0x0600121A RID: 4634 RVA: 0x0004B5F9 File Offset: 0x000497F9
			public XmlBaseReader.QNameType QNameType
			{
				get
				{
					return this.qnameType;
				}
				set
				{
					this.qnameType = value;
				}
			}

			// Token: 0x1700039C RID: 924
			// (get) Token: 0x0600121B RID: 4635 RVA: 0x0004B602 File Offset: 0x00049802
			// (set) Token: 0x0600121C RID: 4636 RVA: 0x0004B60A File Offset: 0x0004980A
			public XmlBaseReader.Namespace Namespace
			{
				get
				{
					return this.ns;
				}
				set
				{
					this.ns = value;
				}
			}

			// Token: 0x1700039D RID: 925
			// (get) Token: 0x0600121D RID: 4637 RVA: 0x0004B613 File Offset: 0x00049813
			// (set) Token: 0x0600121E RID: 4638 RVA: 0x0004B61B File Offset: 0x0004981B
			public bool IsAtomicValue
			{
				get
				{
					return this.isAtomicValue;
				}
				set
				{
					this.isAtomicValue = value;
				}
			}

			// Token: 0x1700039E RID: 926
			// (get) Token: 0x0600121F RID: 4639 RVA: 0x0004B624 File Offset: 0x00049824
			// (set) Token: 0x06001220 RID: 4640 RVA: 0x0004B62C File Offset: 0x0004982C
			public bool ExitScope
			{
				get
				{
					return this.exitScope;
				}
				set
				{
					this.exitScope = value;
				}
			}

			// Token: 0x1700039F RID: 927
			// (get) Token: 0x06001221 RID: 4641 RVA: 0x0004B635 File Offset: 0x00049835
			// (set) Token: 0x06001222 RID: 4642 RVA: 0x0004B63D File Offset: 0x0004983D
			public bool IsEmptyElement
			{
				get
				{
					return this.isEmptyElement;
				}
				set
				{
					this.isEmptyElement = value;
				}
			}

			// Token: 0x170003A0 RID: 928
			// (get) Token: 0x06001223 RID: 4643 RVA: 0x0004B646 File Offset: 0x00049846
			// (set) Token: 0x06001224 RID: 4644 RVA: 0x0004B64E File Offset: 0x0004984E
			public char QuoteChar
			{
				get
				{
					return this.quoteChar;
				}
				set
				{
					this.quoteChar = value;
				}
			}

			// Token: 0x06001225 RID: 4645 RVA: 0x0004B657 File Offset: 0x00049857
			public bool IsLocalName(string localName)
			{
				if (this.qnameType == XmlBaseReader.QNameType.Normal)
				{
					return this.LocalName == localName;
				}
				return this.Namespace.Prefix == localName;
			}

			// Token: 0x06001226 RID: 4646 RVA: 0x0004B67F File Offset: 0x0004987F
			public bool IsLocalName(XmlDictionaryString localName)
			{
				if (this.qnameType == XmlBaseReader.QNameType.Normal)
				{
					return this.LocalName == localName;
				}
				return this.Namespace.Prefix == localName;
			}

			// Token: 0x06001227 RID: 4647 RVA: 0x0004B6A7 File Offset: 0x000498A7
			public bool IsNamespaceUri(string ns)
			{
				if (this.qnameType == XmlBaseReader.QNameType.Normal)
				{
					return this.Namespace.IsUri(ns);
				}
				return ns == "http://www.w3.org/2000/xmlns/";
			}

			// Token: 0x06001228 RID: 4648 RVA: 0x0004B6C9 File Offset: 0x000498C9
			public bool IsNamespaceUri(XmlDictionaryString ns)
			{
				if (this.qnameType == XmlBaseReader.QNameType.Normal)
				{
					return this.Namespace.IsUri(ns);
				}
				return ns.Value == "http://www.w3.org/2000/xmlns/";
			}

			// Token: 0x06001229 RID: 4649 RVA: 0x0004B6F0 File Offset: 0x000498F0
			public bool IsLocalNameAndNamespaceUri(string localName, string ns)
			{
				if (this.qnameType == XmlBaseReader.QNameType.Normal)
				{
					return this.LocalName == localName && this.Namespace.IsUri(ns);
				}
				return this.Namespace.Prefix == localName && ns == "http://www.w3.org/2000/xmlns/";
			}

			// Token: 0x0600122A RID: 4650 RVA: 0x0004B744 File Offset: 0x00049944
			public bool IsLocalNameAndNamespaceUri(XmlDictionaryString localName, XmlDictionaryString ns)
			{
				if (this.qnameType == XmlBaseReader.QNameType.Normal)
				{
					return this.LocalName == localName && this.Namespace.IsUri(ns);
				}
				return this.Namespace.Prefix == localName && ns.Value == "http://www.w3.org/2000/xmlns/";
			}

			// Token: 0x0600122B RID: 4651 RVA: 0x0004B79C File Offset: 0x0004999C
			public bool IsPrefixAndLocalName(string prefix, string localName)
			{
				if (this.qnameType == XmlBaseReader.QNameType.Normal)
				{
					return this.Prefix == prefix && this.LocalName == localName;
				}
				return prefix == "xmlns" && this.Namespace.Prefix == localName;
			}

			// Token: 0x0600122C RID: 4652 RVA: 0x0004B7EE File Offset: 0x000499EE
			public bool TryGetLocalNameAsDictionaryString(out XmlDictionaryString localName)
			{
				if (this.qnameType == XmlBaseReader.QNameType.Normal)
				{
					return this.LocalName.TryGetDictionaryString(out localName);
				}
				localName = null;
				return false;
			}

			// Token: 0x0600122D RID: 4653 RVA: 0x0004B809 File Offset: 0x00049A09
			public bool TryGetNamespaceUriAsDictionaryString(out XmlDictionaryString ns)
			{
				if (this.qnameType == XmlBaseReader.QNameType.Normal)
				{
					return this.Namespace.Uri.TryGetDictionaryString(out ns);
				}
				ns = null;
				return false;
			}

			// Token: 0x0600122E RID: 4654 RVA: 0x0004B829 File Offset: 0x00049A29
			public bool TryGetValueAsDictionaryString(out XmlDictionaryString value)
			{
				if (this.qnameType == XmlBaseReader.QNameType.Normal)
				{
					return this.Value.TryGetDictionaryString(out value);
				}
				value = null;
				return false;
			}

			// Token: 0x170003A1 RID: 929
			// (get) Token: 0x0600122F RID: 4655 RVA: 0x0004B844 File Offset: 0x00049A44
			public string ValueAsString
			{
				get
				{
					if (this.qnameType == XmlBaseReader.QNameType.Normal)
					{
						return this.Value.GetString();
					}
					return this.Namespace.Uri.GetString();
				}
			}

			// Token: 0x040008C8 RID: 2248
			private XmlNodeType nodeType;

			// Token: 0x040008C9 RID: 2249
			private PrefixHandle prefix;

			// Token: 0x040008CA RID: 2250
			private StringHandle localName;

			// Token: 0x040008CB RID: 2251
			private ValueHandle value;

			// Token: 0x040008CC RID: 2252
			private XmlBaseReader.Namespace ns;

			// Token: 0x040008CD RID: 2253
			private bool hasValue;

			// Token: 0x040008CE RID: 2254
			private bool canGetAttribute;

			// Token: 0x040008CF RID: 2255
			private bool canMoveToElement;

			// Token: 0x040008D0 RID: 2256
			private ReadState readState;

			// Token: 0x040008D1 RID: 2257
			private XmlBaseReader.XmlAttributeTextNode attributeTextNode;

			// Token: 0x040008D2 RID: 2258
			private bool exitScope;

			// Token: 0x040008D3 RID: 2259
			private int depthDelta;

			// Token: 0x040008D4 RID: 2260
			private bool isAtomicValue;

			// Token: 0x040008D5 RID: 2261
			private bool skipValue;

			// Token: 0x040008D6 RID: 2262
			private XmlBaseReader.QNameType qnameType;

			// Token: 0x040008D7 RID: 2263
			private bool hasContent;

			// Token: 0x040008D8 RID: 2264
			private bool isEmptyElement;

			// Token: 0x040008D9 RID: 2265
			private char quoteChar;

			// Token: 0x020001A5 RID: 421
			protected enum XmlNodeFlags
			{
				// Token: 0x04000A78 RID: 2680
				None,
				// Token: 0x04000A79 RID: 2681
				CanGetAttribute,
				// Token: 0x04000A7A RID: 2682
				CanMoveToElement,
				// Token: 0x04000A7B RID: 2683
				HasValue = 4,
				// Token: 0x04000A7C RID: 2684
				AtomicValue = 8,
				// Token: 0x04000A7D RID: 2685
				SkipValue = 16,
				// Token: 0x04000A7E RID: 2686
				HasContent = 32
			}
		}

		// Token: 0x02000131 RID: 305
		protected class XmlElementNode : XmlBaseReader.XmlNode
		{
			// Token: 0x06001230 RID: 4656 RVA: 0x0004B86A File Offset: 0x00049A6A
			public XmlElementNode(XmlBufferReader bufferReader)
				: this(new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader))
			{
			}

			// Token: 0x06001231 RID: 4657 RVA: 0x0004B884 File Offset: 0x00049A84
			private XmlElementNode(PrefixHandle prefix, StringHandle localName, ValueHandle value)
				: base(XmlNodeType.Element, prefix, localName, value, (XmlBaseReader.XmlNode.XmlNodeFlags)33, ReadState.Interactive, null, -1)
			{
				this.endElementNode = new XmlBaseReader.XmlEndElementNode(prefix, localName, value);
			}

			// Token: 0x170003A2 RID: 930
			// (get) Token: 0x06001232 RID: 4658 RVA: 0x0004B8AE File Offset: 0x00049AAE
			public XmlBaseReader.XmlEndElementNode EndElement
			{
				get
				{
					return this.endElementNode;
				}
			}

			// Token: 0x170003A3 RID: 931
			// (get) Token: 0x06001233 RID: 4659 RVA: 0x0004B8B6 File Offset: 0x00049AB6
			// (set) Token: 0x06001234 RID: 4660 RVA: 0x0004B8BE File Offset: 0x00049ABE
			public int BufferOffset
			{
				get
				{
					return this.bufferOffset;
				}
				set
				{
					this.bufferOffset = value;
				}
			}

			// Token: 0x040008DA RID: 2266
			private XmlBaseReader.XmlEndElementNode endElementNode;

			// Token: 0x040008DB RID: 2267
			private int bufferOffset;

			// Token: 0x040008DC RID: 2268
			public int NameOffset;

			// Token: 0x040008DD RID: 2269
			public int NameLength;
		}

		// Token: 0x02000132 RID: 306
		protected class XmlAttributeNode : XmlBaseReader.XmlNode
		{
			// Token: 0x06001235 RID: 4661 RVA: 0x0004B8C7 File Offset: 0x00049AC7
			public XmlAttributeNode(XmlBufferReader bufferReader)
				: this(new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader))
			{
			}

			// Token: 0x06001236 RID: 4662 RVA: 0x0004B8E4 File Offset: 0x00049AE4
			private XmlAttributeNode(PrefixHandle prefix, StringHandle localName, ValueHandle value)
				: base(XmlNodeType.Attribute, prefix, localName, value, (XmlBaseReader.XmlNode.XmlNodeFlags)15, ReadState.Interactive, new XmlBaseReader.XmlAttributeTextNode(prefix, localName, value), 0)
			{
			}
		}

		// Token: 0x02000133 RID: 307
		protected class XmlEndElementNode : XmlBaseReader.XmlNode
		{
			// Token: 0x06001237 RID: 4663 RVA: 0x0004B908 File Offset: 0x00049B08
			public XmlEndElementNode(PrefixHandle prefix, StringHandle localName, ValueHandle value)
				: base(XmlNodeType.EndElement, prefix, localName, value, XmlBaseReader.XmlNode.XmlNodeFlags.HasContent, ReadState.Interactive, null, -1)
			{
			}
		}

		// Token: 0x02000134 RID: 308
		protected class XmlTextNode : XmlBaseReader.XmlNode
		{
			// Token: 0x06001238 RID: 4664 RVA: 0x0004B928 File Offset: 0x00049B28
			protected XmlTextNode(XmlNodeType nodeType, PrefixHandle prefix, StringHandle localName, ValueHandle value, XmlBaseReader.XmlNode.XmlNodeFlags nodeFlags, ReadState readState, XmlBaseReader.XmlAttributeTextNode attributeTextNode, int depthDelta)
				: base(nodeType, prefix, localName, value, nodeFlags, readState, attributeTextNode, depthDelta)
			{
			}
		}

		// Token: 0x02000135 RID: 309
		protected class XmlAtomicTextNode : XmlBaseReader.XmlTextNode
		{
			// Token: 0x06001239 RID: 4665 RVA: 0x0004B948 File Offset: 0x00049B48
			public XmlAtomicTextNode(XmlBufferReader bufferReader)
				: base(XmlNodeType.Text, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), (XmlBaseReader.XmlNode.XmlNodeFlags)60, ReadState.Interactive, null, 0)
			{
			}
		}

		// Token: 0x02000136 RID: 310
		protected class XmlComplexTextNode : XmlBaseReader.XmlTextNode
		{
			// Token: 0x0600123A RID: 4666 RVA: 0x0004B974 File Offset: 0x00049B74
			public XmlComplexTextNode(XmlBufferReader bufferReader)
				: base(XmlNodeType.Text, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), (XmlBaseReader.XmlNode.XmlNodeFlags)36, ReadState.Interactive, null, 0)
			{
			}
		}

		// Token: 0x02000137 RID: 311
		protected class XmlWhitespaceTextNode : XmlBaseReader.XmlTextNode
		{
			// Token: 0x0600123B RID: 4667 RVA: 0x0004B9A0 File Offset: 0x00049BA0
			public XmlWhitespaceTextNode(XmlBufferReader bufferReader)
				: base(XmlNodeType.Whitespace, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlBaseReader.XmlNode.XmlNodeFlags.HasValue, ReadState.Interactive, null, 0)
			{
			}
		}

		// Token: 0x02000138 RID: 312
		protected class XmlCDataNode : XmlBaseReader.XmlTextNode
		{
			// Token: 0x0600123C RID: 4668 RVA: 0x0004B9CC File Offset: 0x00049BCC
			public XmlCDataNode(XmlBufferReader bufferReader)
				: base(XmlNodeType.CDATA, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), (XmlBaseReader.XmlNode.XmlNodeFlags)36, ReadState.Interactive, null, 0)
			{
			}
		}

		// Token: 0x02000139 RID: 313
		protected class XmlAttributeTextNode : XmlBaseReader.XmlTextNode
		{
			// Token: 0x0600123D RID: 4669 RVA: 0x0004B9F8 File Offset: 0x00049BF8
			public XmlAttributeTextNode(PrefixHandle prefix, StringHandle localName, ValueHandle value)
				: base(XmlNodeType.Text, prefix, localName, value, (XmlBaseReader.XmlNode.XmlNodeFlags)47, ReadState.Interactive, null, 1)
			{
			}
		}

		// Token: 0x0200013A RID: 314
		protected class XmlInitialNode : XmlBaseReader.XmlNode
		{
			// Token: 0x0600123E RID: 4670 RVA: 0x0004BA14 File Offset: 0x00049C14
			public XmlInitialNode(XmlBufferReader bufferReader)
				: base(XmlNodeType.None, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlBaseReader.XmlNode.XmlNodeFlags.None, ReadState.Initial, null, 0)
			{
			}
		}

		// Token: 0x0200013B RID: 315
		protected class XmlDeclarationNode : XmlBaseReader.XmlNode
		{
			// Token: 0x0600123F RID: 4671 RVA: 0x0004BA40 File Offset: 0x00049C40
			public XmlDeclarationNode(XmlBufferReader bufferReader)
				: base(XmlNodeType.XmlDeclaration, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlBaseReader.XmlNode.XmlNodeFlags.CanGetAttribute, ReadState.Interactive, null, 0)
			{
			}
		}

		// Token: 0x0200013C RID: 316
		protected class XmlCommentNode : XmlBaseReader.XmlNode
		{
			// Token: 0x06001240 RID: 4672 RVA: 0x0004BA6C File Offset: 0x00049C6C
			public XmlCommentNode(XmlBufferReader bufferReader)
				: base(XmlNodeType.Comment, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlBaseReader.XmlNode.XmlNodeFlags.HasValue, ReadState.Interactive, null, 0)
			{
			}
		}

		// Token: 0x0200013D RID: 317
		protected class XmlEndOfFileNode : XmlBaseReader.XmlNode
		{
			// Token: 0x06001241 RID: 4673 RVA: 0x0004BA98 File Offset: 0x00049C98
			public XmlEndOfFileNode(XmlBufferReader bufferReader)
				: base(XmlNodeType.None, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlBaseReader.XmlNode.XmlNodeFlags.None, ReadState.EndOfFile, null, 0)
			{
			}
		}

		// Token: 0x0200013E RID: 318
		protected class XmlClosedNode : XmlBaseReader.XmlNode
		{
			// Token: 0x06001242 RID: 4674 RVA: 0x0004BAC4 File Offset: 0x00049CC4
			public XmlClosedNode(XmlBufferReader bufferReader)
				: base(XmlNodeType.None, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlBaseReader.XmlNode.XmlNodeFlags.None, ReadState.Closed, null, 0)
			{
			}
		}

		// Token: 0x0200013F RID: 319
		private class AttributeSorter : IComparer
		{
			// Token: 0x06001243 RID: 4675 RVA: 0x0004BAEE File Offset: 0x00049CEE
			public bool Sort(XmlBaseReader.XmlAttributeNode[] attributeNodes, int attributeCount)
			{
				this.attributeIndex1 = -1;
				this.attributeIndex2 = -1;
				this.attributeNodes = attributeNodes;
				this.attributeCount = attributeCount;
				bool flag = this.Sort();
				this.attributeNodes = null;
				this.attributeCount = 0;
				return flag;
			}

			// Token: 0x06001244 RID: 4676 RVA: 0x0004BB20 File Offset: 0x00049D20
			public void GetIndeces(out int attributeIndex1, out int attributeIndex2)
			{
				attributeIndex1 = this.attributeIndex1;
				attributeIndex2 = this.attributeIndex2;
			}

			// Token: 0x06001245 RID: 4677 RVA: 0x0004BB32 File Offset: 0x00049D32
			public void Close()
			{
				if (this.indeces != null && this.indeces.Length > 32)
				{
					this.indeces = null;
				}
			}

			// Token: 0x06001246 RID: 4678 RVA: 0x0004BB50 File Offset: 0x00049D50
			private bool Sort()
			{
				if (this.indeces != null && this.indeces.Length == this.attributeCount && this.IsSorted())
				{
					return true;
				}
				object[] array = new object[this.attributeCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = i;
				}
				this.indeces = array;
				Array.Sort(this.indeces, 0, this.attributeCount, this);
				return this.IsSorted();
			}

			// Token: 0x06001247 RID: 4679 RVA: 0x0004BBC4 File Offset: 0x00049DC4
			private bool IsSorted()
			{
				for (int i = 0; i < this.indeces.Length - 1; i++)
				{
					if (this.Compare(this.indeces[i], this.indeces[i + 1]) >= 0)
					{
						this.attributeIndex1 = (int)this.indeces[i];
						this.attributeIndex2 = (int)this.indeces[i + 1];
						return false;
					}
				}
				return true;
			}

			// Token: 0x06001248 RID: 4680 RVA: 0x0004BC2C File Offset: 0x00049E2C
			public int Compare(object obj1, object obj2)
			{
				int num = (int)obj1;
				int num2 = (int)obj2;
				XmlBaseReader.XmlAttributeNode xmlAttributeNode = this.attributeNodes[num];
				XmlBaseReader.XmlAttributeNode xmlAttributeNode2 = this.attributeNodes[num2];
				int num3 = this.CompareQNameType(xmlAttributeNode.QNameType, xmlAttributeNode2.QNameType);
				if (num3 == 0)
				{
					if (xmlAttributeNode.QNameType == XmlBaseReader.QNameType.Normal)
					{
						num3 = xmlAttributeNode.LocalName.CompareTo(xmlAttributeNode2.LocalName);
						if (num3 == 0)
						{
							num3 = xmlAttributeNode.Namespace.Uri.CompareTo(xmlAttributeNode2.Namespace.Uri);
						}
					}
					else
					{
						num3 = xmlAttributeNode.Namespace.Prefix.CompareTo(xmlAttributeNode2.Namespace.Prefix);
					}
				}
				return num3;
			}

			// Token: 0x06001249 RID: 4681 RVA: 0x0004BCCE File Offset: 0x00049ECE
			public int CompareQNameType(XmlBaseReader.QNameType type1, XmlBaseReader.QNameType type2)
			{
				return type1 - type2;
			}

			// Token: 0x040008DE RID: 2270
			private object[] indeces;

			// Token: 0x040008DF RID: 2271
			private XmlBaseReader.XmlAttributeNode[] attributeNodes;

			// Token: 0x040008E0 RID: 2272
			private int attributeCount;

			// Token: 0x040008E1 RID: 2273
			private int attributeIndex1;

			// Token: 0x040008E2 RID: 2274
			private int attributeIndex2;
		}

		// Token: 0x02000140 RID: 320
		private class NamespaceManager
		{
			// Token: 0x0600124B RID: 4683 RVA: 0x0004BCDC File Offset: 0x00049EDC
			public NamespaceManager(XmlBufferReader bufferReader)
			{
				this.bufferReader = bufferReader;
				this.shortPrefixUri = new XmlBaseReader.Namespace[28];
				this.shortPrefixUri[0] = XmlBaseReader.NamespaceManager.emptyNamespace;
				this.namespaces = null;
				this.nsCount = 0;
				this.attributes = null;
				this.attributeCount = 0;
				this.space = XmlSpace.None;
				this.lang = string.Empty;
				this.depth = 0;
			}

			// Token: 0x0600124C RID: 4684 RVA: 0x0004BD48 File Offset: 0x00049F48
			public void Close()
			{
				if (this.namespaces != null && this.namespaces.Length > 32)
				{
					this.namespaces = null;
				}
				if (this.attributes != null && this.attributes.Length > 4)
				{
					this.attributes = null;
				}
				this.lang = string.Empty;
			}

			// Token: 0x170003A4 RID: 932
			// (get) Token: 0x0600124D RID: 4685 RVA: 0x0004BD98 File Offset: 0x00049F98
			public static XmlBaseReader.Namespace XmlNamespace
			{
				get
				{
					if (XmlBaseReader.NamespaceManager.xmlNamespace == null)
					{
						byte[] array = new byte[]
						{
							120, 109, 108, 104, 116, 116, 112, 58, 47, 47,
							119, 119, 119, 46, 119, 51, 46, 111, 114, 103,
							47, 88, 77, 76, 47, 49, 57, 57, 56, 47,
							110, 97, 109, 101, 115, 112, 97, 99, 101
						};
						XmlBaseReader.Namespace @namespace = new XmlBaseReader.Namespace(new XmlBufferReader(array));
						@namespace.Prefix.SetValue(0, 3);
						@namespace.Uri.SetValue(3, array.Length - 3);
						XmlBaseReader.NamespaceManager.xmlNamespace = @namespace;
					}
					return XmlBaseReader.NamespaceManager.xmlNamespace;
				}
			}

			// Token: 0x170003A5 RID: 933
			// (get) Token: 0x0600124E RID: 4686 RVA: 0x0004BDF2 File Offset: 0x00049FF2
			public static XmlBaseReader.Namespace EmptyNamespace
			{
				get
				{
					return XmlBaseReader.NamespaceManager.emptyNamespace;
				}
			}

			// Token: 0x170003A6 RID: 934
			// (get) Token: 0x0600124F RID: 4687 RVA: 0x0004BDF9 File Offset: 0x00049FF9
			public string XmlLang
			{
				get
				{
					return this.lang;
				}
			}

			// Token: 0x170003A7 RID: 935
			// (get) Token: 0x06001250 RID: 4688 RVA: 0x0004BE01 File Offset: 0x0004A001
			public XmlSpace XmlSpace
			{
				get
				{
					return this.space;
				}
			}

			// Token: 0x06001251 RID: 4689 RVA: 0x0004BE0C File Offset: 0x0004A00C
			public void Clear()
			{
				if (this.nsCount != 0)
				{
					if (this.shortPrefixUri != null)
					{
						for (int i = 0; i < this.shortPrefixUri.Length; i++)
						{
							this.shortPrefixUri[i] = null;
						}
					}
					this.shortPrefixUri[0] = XmlBaseReader.NamespaceManager.emptyNamespace;
					this.nsCount = 0;
				}
				this.attributeCount = 0;
				this.space = XmlSpace.None;
				this.lang = string.Empty;
				this.depth = 0;
			}

			// Token: 0x06001252 RID: 4690 RVA: 0x0004BE79 File Offset: 0x0004A079
			public void EnterScope()
			{
				this.depth++;
			}

			// Token: 0x06001253 RID: 4691 RVA: 0x0004BE8C File Offset: 0x0004A08C
			public void ExitScope()
			{
				while (this.nsCount > 0)
				{
					XmlBaseReader.Namespace @namespace = this.namespaces[this.nsCount - 1];
					if (@namespace.Depth != this.depth)
					{
						IL_009A:
						while (this.attributeCount > 0)
						{
							XmlBaseReader.NamespaceManager.XmlAttribute xmlAttribute = this.attributes[this.attributeCount - 1];
							if (xmlAttribute.Depth != this.depth)
							{
								break;
							}
							this.space = xmlAttribute.XmlSpace;
							this.lang = xmlAttribute.XmlLang;
							this.attributeCount--;
						}
						this.depth--;
						return;
					}
					PrefixHandleType prefixHandleType;
					if (@namespace.Prefix.TryGetShortPrefix(out prefixHandleType))
					{
						this.shortPrefixUri[(int)prefixHandleType] = @namespace.OuterUri;
					}
					this.nsCount--;
				}
				goto IL_009A;
			}

			// Token: 0x06001254 RID: 4692 RVA: 0x0004BF4C File Offset: 0x0004A14C
			public void Sign(XmlSigningNodeWriter writer)
			{
				for (int i = 0; i < this.nsCount; i++)
				{
					PrefixHandle prefix = this.namespaces[i].Prefix;
					bool flag = false;
					for (int j = i + 1; j < this.nsCount; j++)
					{
						if (object.Equals(prefix, this.namespaces[j].Prefix))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						int num;
						int num2;
						byte[] @string = prefix.GetString(out num, out num2);
						int num3;
						int num4;
						byte[] string2 = this.namespaces[i].Uri.GetString(out num3, out num4);
						writer.WriteXmlnsAttribute(@string, num, num2, string2, num3, num4);
					}
				}
			}

			// Token: 0x06001255 RID: 4693 RVA: 0x0004BFE2 File Offset: 0x0004A1E2
			public void AddLangAttribute(string lang)
			{
				this.AddAttribute();
				this.lang = lang;
			}

			// Token: 0x06001256 RID: 4694 RVA: 0x0004BFF1 File Offset: 0x0004A1F1
			public void AddSpaceAttribute(XmlSpace space)
			{
				this.AddAttribute();
				this.space = space;
			}

			// Token: 0x06001257 RID: 4695 RVA: 0x0004C000 File Offset: 0x0004A200
			private void AddAttribute()
			{
				if (this.attributes == null)
				{
					this.attributes = new XmlBaseReader.NamespaceManager.XmlAttribute[1];
				}
				else if (this.attributes.Length == this.attributeCount)
				{
					XmlBaseReader.NamespaceManager.XmlAttribute[] array = new XmlBaseReader.NamespaceManager.XmlAttribute[this.attributeCount * 2];
					Array.Copy(this.attributes, array, this.attributeCount);
					this.attributes = array;
				}
				XmlBaseReader.NamespaceManager.XmlAttribute xmlAttribute = this.attributes[this.attributeCount];
				if (xmlAttribute == null)
				{
					xmlAttribute = new XmlBaseReader.NamespaceManager.XmlAttribute();
					this.attributes[this.attributeCount] = xmlAttribute;
				}
				xmlAttribute.XmlLang = this.lang;
				xmlAttribute.XmlSpace = this.space;
				xmlAttribute.Depth = this.depth;
				this.attributeCount++;
			}

			// Token: 0x06001258 RID: 4696 RVA: 0x0004C0B4 File Offset: 0x0004A2B4
			public void Register(XmlBaseReader.Namespace nameSpace)
			{
				PrefixHandleType prefixHandleType;
				if (nameSpace.Prefix.TryGetShortPrefix(out prefixHandleType))
				{
					nameSpace.OuterUri = this.shortPrefixUri[(int)prefixHandleType];
					this.shortPrefixUri[(int)prefixHandleType] = nameSpace;
					return;
				}
				nameSpace.OuterUri = null;
			}

			// Token: 0x06001259 RID: 4697 RVA: 0x0004C0F0 File Offset: 0x0004A2F0
			public XmlBaseReader.Namespace AddNamespace()
			{
				if (this.namespaces == null)
				{
					this.namespaces = new XmlBaseReader.Namespace[4];
				}
				else if (this.namespaces.Length == this.nsCount)
				{
					XmlBaseReader.Namespace[] array = new XmlBaseReader.Namespace[this.nsCount * 2];
					Array.Copy(this.namespaces, array, this.nsCount);
					this.namespaces = array;
				}
				XmlBaseReader.Namespace @namespace = this.namespaces[this.nsCount];
				if (@namespace == null)
				{
					@namespace = new XmlBaseReader.Namespace(this.bufferReader);
					this.namespaces[this.nsCount] = @namespace;
				}
				@namespace.Clear();
				@namespace.Depth = this.depth;
				this.nsCount++;
				return @namespace;
			}

			// Token: 0x0600125A RID: 4698 RVA: 0x0004C196 File Offset: 0x0004A396
			public XmlBaseReader.Namespace LookupNamespace(PrefixHandleType prefix)
			{
				return this.shortPrefixUri[(int)prefix];
			}

			// Token: 0x0600125B RID: 4699 RVA: 0x0004C1A0 File Offset: 0x0004A3A0
			public XmlBaseReader.Namespace LookupNamespace(PrefixHandle prefix)
			{
				PrefixHandleType prefixHandleType;
				if (prefix.TryGetShortPrefix(out prefixHandleType))
				{
					return this.LookupNamespace(prefixHandleType);
				}
				for (int i = this.nsCount - 1; i >= 0; i--)
				{
					XmlBaseReader.Namespace @namespace = this.namespaces[i];
					if (@namespace.Prefix == prefix)
					{
						return @namespace;
					}
				}
				if (prefix.IsXml)
				{
					return XmlBaseReader.NamespaceManager.XmlNamespace;
				}
				return null;
			}

			// Token: 0x0600125C RID: 4700 RVA: 0x0004C1FC File Offset: 0x0004A3FC
			public XmlBaseReader.Namespace LookupNamespace(string prefix)
			{
				PrefixHandleType prefixHandleType;
				if (this.TryGetShortPrefix(prefix, out prefixHandleType))
				{
					return this.LookupNamespace(prefixHandleType);
				}
				for (int i = this.nsCount - 1; i >= 0; i--)
				{
					XmlBaseReader.Namespace @namespace = this.namespaces[i];
					if (@namespace.Prefix == prefix)
					{
						return @namespace;
					}
				}
				if (prefix == "xml")
				{
					return XmlBaseReader.NamespaceManager.XmlNamespace;
				}
				return null;
			}

			// Token: 0x0600125D RID: 4701 RVA: 0x0004C25C File Offset: 0x0004A45C
			private bool TryGetShortPrefix(string s, out PrefixHandleType shortPrefix)
			{
				int length = s.Length;
				if (length == 0)
				{
					shortPrefix = PrefixHandleType.Empty;
					return true;
				}
				if (length == 1)
				{
					char c = s[0];
					if (c >= 'a' && c <= 'z')
					{
						shortPrefix = PrefixHandle.GetAlphaPrefix((int)(c - 'a'));
						return true;
					}
				}
				shortPrefix = PrefixHandleType.Empty;
				return false;
			}

			// Token: 0x040008E3 RID: 2275
			private XmlBufferReader bufferReader;

			// Token: 0x040008E4 RID: 2276
			private XmlBaseReader.Namespace[] namespaces;

			// Token: 0x040008E5 RID: 2277
			private int nsCount;

			// Token: 0x040008E6 RID: 2278
			private int depth;

			// Token: 0x040008E7 RID: 2279
			private XmlBaseReader.Namespace[] shortPrefixUri;

			// Token: 0x040008E8 RID: 2280
			private static XmlBaseReader.Namespace emptyNamespace = new XmlBaseReader.Namespace(XmlBufferReader.Empty);

			// Token: 0x040008E9 RID: 2281
			private static XmlBaseReader.Namespace xmlNamespace;

			// Token: 0x040008EA RID: 2282
			private XmlBaseReader.NamespaceManager.XmlAttribute[] attributes;

			// Token: 0x040008EB RID: 2283
			private int attributeCount;

			// Token: 0x040008EC RID: 2284
			private XmlSpace space;

			// Token: 0x040008ED RID: 2285
			private string lang;

			// Token: 0x020001A6 RID: 422
			private class XmlAttribute
			{
				// Token: 0x1700046E RID: 1134
				// (get) Token: 0x0600153E RID: 5438 RVA: 0x000553DA File Offset: 0x000535DA
				// (set) Token: 0x0600153F RID: 5439 RVA: 0x000553E2 File Offset: 0x000535E2
				public int Depth
				{
					get
					{
						return this.depth;
					}
					set
					{
						this.depth = value;
					}
				}

				// Token: 0x1700046F RID: 1135
				// (get) Token: 0x06001540 RID: 5440 RVA: 0x000553EB File Offset: 0x000535EB
				// (set) Token: 0x06001541 RID: 5441 RVA: 0x000553F3 File Offset: 0x000535F3
				public string XmlLang
				{
					get
					{
						return this.lang;
					}
					set
					{
						this.lang = value;
					}
				}

				// Token: 0x17000470 RID: 1136
				// (get) Token: 0x06001542 RID: 5442 RVA: 0x000553FC File Offset: 0x000535FC
				// (set) Token: 0x06001543 RID: 5443 RVA: 0x00055404 File Offset: 0x00053604
				public XmlSpace XmlSpace
				{
					get
					{
						return this.space;
					}
					set
					{
						this.space = value;
					}
				}

				// Token: 0x04000A7F RID: 2687
				private XmlSpace space;

				// Token: 0x04000A80 RID: 2688
				private string lang;

				// Token: 0x04000A81 RID: 2689
				private int depth;
			}
		}

		// Token: 0x02000141 RID: 321
		protected class Namespace
		{
			// Token: 0x0600125F RID: 4703 RVA: 0x0004C2B0 File Offset: 0x0004A4B0
			public Namespace(XmlBufferReader bufferReader)
			{
				this.prefix = new PrefixHandle(bufferReader);
				this.uri = new StringHandle(bufferReader);
				this.outerUri = null;
				this.uriString = null;
			}

			// Token: 0x06001260 RID: 4704 RVA: 0x0004C2DE File Offset: 0x0004A4DE
			public void Clear()
			{
				this.uriString = null;
			}

			// Token: 0x170003A8 RID: 936
			// (get) Token: 0x06001261 RID: 4705 RVA: 0x0004C2E7 File Offset: 0x0004A4E7
			// (set) Token: 0x06001262 RID: 4706 RVA: 0x0004C2EF File Offset: 0x0004A4EF
			public int Depth
			{
				get
				{
					return this.depth;
				}
				set
				{
					this.depth = value;
				}
			}

			// Token: 0x170003A9 RID: 937
			// (get) Token: 0x06001263 RID: 4707 RVA: 0x0004C2F8 File Offset: 0x0004A4F8
			public PrefixHandle Prefix
			{
				get
				{
					return this.prefix;
				}
			}

			// Token: 0x06001264 RID: 4708 RVA: 0x0004C300 File Offset: 0x0004A500
			public bool IsUri(string s)
			{
				if (s == this.uriString)
				{
					return true;
				}
				if (this.uri == s)
				{
					this.uriString = s;
					return true;
				}
				return false;
			}

			// Token: 0x06001265 RID: 4709 RVA: 0x0004C325 File Offset: 0x0004A525
			public bool IsUri(XmlDictionaryString s)
			{
				if (s.Value == this.uriString)
				{
					return true;
				}
				if (this.uri == s)
				{
					this.uriString = s.Value;
					return true;
				}
				return false;
			}

			// Token: 0x170003AA RID: 938
			// (get) Token: 0x06001266 RID: 4710 RVA: 0x0004C354 File Offset: 0x0004A554
			public StringHandle Uri
			{
				get
				{
					return this.uri;
				}
			}

			// Token: 0x170003AB RID: 939
			// (get) Token: 0x06001267 RID: 4711 RVA: 0x0004C35C File Offset: 0x0004A55C
			// (set) Token: 0x06001268 RID: 4712 RVA: 0x0004C364 File Offset: 0x0004A564
			public XmlBaseReader.Namespace OuterUri
			{
				get
				{
					return this.outerUri;
				}
				set
				{
					this.outerUri = value;
				}
			}

			// Token: 0x040008EE RID: 2286
			private PrefixHandle prefix;

			// Token: 0x040008EF RID: 2287
			private StringHandle uri;

			// Token: 0x040008F0 RID: 2288
			private int depth;

			// Token: 0x040008F1 RID: 2289
			private XmlBaseReader.Namespace outerUri;

			// Token: 0x040008F2 RID: 2290
			private string uriString;
		}

		// Token: 0x02000142 RID: 322
		private class QuotaNameTable : XmlNameTable
		{
			// Token: 0x06001269 RID: 4713 RVA: 0x0004C36D File Offset: 0x0004A56D
			public QuotaNameTable(XmlDictionaryReader reader, int maxCharCount)
			{
				this.reader = reader;
				this.nameTable = new NameTable();
				this.maxCharCount = maxCharCount;
				this.charCount = 0;
			}

			// Token: 0x0600126A RID: 4714 RVA: 0x0004C395 File Offset: 0x0004A595
			public override string Get(char[] chars, int offset, int count)
			{
				return this.nameTable.Get(chars, offset, count);
			}

			// Token: 0x0600126B RID: 4715 RVA: 0x0004C3A5 File Offset: 0x0004A5A5
			public override string Get(string value)
			{
				return this.nameTable.Get(value);
			}

			// Token: 0x0600126C RID: 4716 RVA: 0x0004C3B3 File Offset: 0x0004A5B3
			private void Add(int charCount)
			{
				if (charCount > this.maxCharCount - this.charCount)
				{
					XmlExceptionHelper.ThrowMaxNameTableCharCountExceeded(this.reader, this.maxCharCount);
				}
				this.charCount += charCount;
			}

			// Token: 0x0600126D RID: 4717 RVA: 0x0004C3E4 File Offset: 0x0004A5E4
			public override string Add(char[] chars, int offset, int count)
			{
				string text = this.nameTable.Get(chars, offset, count);
				if (text != null)
				{
					return text;
				}
				this.Add(count);
				return this.nameTable.Add(chars, offset, count);
			}

			// Token: 0x0600126E RID: 4718 RVA: 0x0004C41C File Offset: 0x0004A61C
			public override string Add(string value)
			{
				string text = this.nameTable.Get(value);
				if (text != null)
				{
					return text;
				}
				this.Add(value.Length);
				return this.nameTable.Add(value);
			}

			// Token: 0x040008F3 RID: 2291
			private XmlDictionaryReader reader;

			// Token: 0x040008F4 RID: 2292
			private XmlNameTable nameTable;

			// Token: 0x040008F5 RID: 2293
			private int maxCharCount;

			// Token: 0x040008F6 RID: 2294
			private int charCount;
		}
	}
}
