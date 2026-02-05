using System;
using System.Globalization;
using System.IO;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000028 RID: 40
	internal abstract class XmlBaseWriter : XmlDictionaryWriter, IFragmentCapableXmlDictionaryWriter
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x00008BE4 File Offset: 0x00006DE4
		protected XmlBaseWriter()
		{
			this.nsMgr = new XmlBaseWriter.NamespaceManager();
			this.writeState = WriteState.Start;
			this.documentState = XmlBaseWriter.DocumentState.None;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00008C08 File Offset: 0x00006E08
		protected void SetOutput(XmlStreamNodeWriter writer)
		{
			this.inList = false;
			this.writer = writer;
			this.nodeWriter = writer;
			this.writeState = WriteState.Start;
			this.documentState = XmlBaseWriter.DocumentState.None;
			this.nsMgr.Clear();
			if (this.depth != 0)
			{
				this.elements = null;
				this.depth = 0;
			}
			this.attributeLocalName = null;
			this.attributeValue = null;
			this.oldWriter = null;
			this.oldStream = null;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00008C75 File Offset: 0x00006E75
		public override void Flush()
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.writer.Flush();
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00008C90 File Offset: 0x00006E90
		public override void Close()
		{
			if (this.IsClosed)
			{
				return;
			}
			try
			{
				this.FinishDocument();
				this.AutoComplete(WriteState.Closed);
				this.writer.Flush();
			}
			finally
			{
				this.nsMgr.Close();
				if (this.depth != 0)
				{
					this.elements = null;
					this.depth = 0;
				}
				this.attributeValue = null;
				this.attributeLocalName = null;
				this.nodeWriter.Close();
				if (this.signingWriter != null)
				{
					this.signingWriter.Close();
				}
				if (this.textFragmentWriter != null)
				{
					this.textFragmentWriter.Close();
				}
				this.oldWriter = null;
				this.oldStream = null;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00008D40 File Offset: 0x00006F40
		protected bool IsClosed
		{
			get
			{
				return this.writeState == WriteState.Closed;
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008D4B File Offset: 0x00006F4B
		protected void ThrowClosed()
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("The XmlWriter is closed.")));
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00008D61 File Offset: 0x00006F61
		private static BinHexEncoding BinHexEncoding
		{
			get
			{
				if (XmlBaseWriter.binhexEncoding == null)
				{
					XmlBaseWriter.binhexEncoding = new BinHexEncoding();
				}
				return XmlBaseWriter.binhexEncoding;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00008D79 File Offset: 0x00006F79
		public override string XmlLang
		{
			get
			{
				return this.nsMgr.XmlLang;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00008D86 File Offset: 0x00006F86
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.nsMgr.XmlSpace;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00008D93 File Offset: 0x00006F93
		public override WriteState WriteState
		{
			get
			{
				return this.writeState;
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00008D9C File Offset: 0x00006F9C
		public override void WriteXmlnsAttribute(string prefix, string ns)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (ns == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("ns");
			}
			if (this.writeState != WriteState.Element)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", new object[]
				{
					"WriteXmlnsAttribute",
					this.WriteState.ToString()
				})));
			}
			if (prefix == null)
			{
				prefix = this.nsMgr.LookupPrefix(ns);
				if (prefix == null)
				{
					this.GeneratePrefix(ns, null);
					return;
				}
			}
			else
			{
				this.nsMgr.AddNamespaceIfNotDeclared(prefix, ns, null);
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00008E34 File Offset: 0x00007034
		public override void WriteXmlnsAttribute(string prefix, XmlDictionaryString ns)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (ns == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("ns");
			}
			if (this.writeState != WriteState.Element)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", new object[]
				{
					"WriteXmlnsAttribute",
					this.WriteState.ToString()
				})));
			}
			if (prefix == null)
			{
				prefix = this.nsMgr.LookupPrefix(ns.Value);
				if (prefix == null)
				{
					this.GeneratePrefix(ns.Value, ns);
					return;
				}
			}
			else
			{
				this.nsMgr.AddNamespaceIfNotDeclared(prefix, ns.Value, ns);
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008EDC File Offset: 0x000070DC
		private void StartAttribute(ref string prefix, string localName, string ns, XmlDictionaryString xNs)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (this.writeState == WriteState.Attribute)
			{
				this.WriteEndAttribute();
			}
			if (localName == null || (localName.Length == 0 && prefix != "xmlns"))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
			}
			if (this.writeState != WriteState.Element)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", new object[]
				{
					"WriteStartAttribute",
					this.WriteState.ToString()
				})));
			}
			if (prefix == null)
			{
				if (ns == "http://www.w3.org/2000/xmlns/" && localName != "xmlns")
				{
					prefix = "xmlns";
				}
				else if (ns == "http://www.w3.org/XML/1998/namespace")
				{
					prefix = "xml";
				}
				else
				{
					prefix = string.Empty;
				}
			}
			if (prefix.Length == 0 && localName == "xmlns")
			{
				prefix = "xmlns";
				localName = string.Empty;
			}
			this.isXmlnsAttribute = false;
			this.isXmlAttribute = false;
			if (prefix == "xml")
			{
				if (ns != null && ns != "http://www.w3.org/XML/1998/namespace")
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The prefix '{0}' is bound to the namespace '{1}' and cannot be changed to '{2}'.", new object[] { "xml", "http://www.w3.org/XML/1998/namespace", ns }), "ns"));
				}
				this.isXmlAttribute = true;
				this.attributeValue = string.Empty;
				this.attributeLocalName = localName;
			}
			else if (prefix == "xmlns")
			{
				if (ns != null && ns != "http://www.w3.org/2000/xmlns/")
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The prefix '{0}' is bound to the namespace '{1}' and cannot be changed to '{2}'.", new object[] { "xmlns", "http://www.w3.org/2000/xmlns/", ns }), "ns"));
				}
				this.isXmlnsAttribute = true;
				this.attributeValue = string.Empty;
				this.attributeLocalName = localName;
			}
			else if (ns == null)
			{
				if (prefix.Length == 0)
				{
					ns = string.Empty;
				}
				else
				{
					ns = this.nsMgr.LookupNamespace(prefix);
					if (ns == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The prefix '{0}' is not defined.", new object[] { prefix }), "prefix"));
					}
				}
			}
			else if (ns.Length == 0)
			{
				if (prefix.Length != 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The empty namespace requires a null or empty prefix."), "prefix"));
				}
			}
			else if (prefix.Length == 0)
			{
				prefix = this.nsMgr.LookupAttributePrefix(ns);
				if (prefix == null)
				{
					if (ns.Length == "http://www.w3.org/2000/xmlns/".Length && ns == "http://www.w3.org/2000/xmlns/")
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The namespace '{1}' can only be bound to the prefix '{0}'.", new object[] { "xmlns", ns })));
					}
					if (ns.Length == "http://www.w3.org/XML/1998/namespace".Length && ns == "http://www.w3.org/XML/1998/namespace")
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The namespace '{1}' can only be bound to the prefix '{0}'.", new object[] { "xml", ns })));
					}
					prefix = this.GeneratePrefix(ns, xNs);
				}
			}
			else
			{
				this.nsMgr.AddNamespaceIfNotDeclared(prefix, ns, xNs);
			}
			this.writeState = WriteState.Attribute;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00009219 File Offset: 0x00007419
		public override void WriteStartAttribute(string prefix, string localName, string namespaceUri)
		{
			this.StartAttribute(ref prefix, localName, namespaceUri, null);
			if (!this.isXmlnsAttribute)
			{
				this.writer.WriteStartAttribute(prefix, localName);
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000923B File Offset: 0x0000743B
		public override void WriteStartAttribute(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			this.StartAttribute(ref prefix, (localName != null) ? localName.Value : null, (namespaceUri != null) ? namespaceUri.Value : null, namespaceUri);
			if (!this.isXmlnsAttribute)
			{
				this.writer.WriteStartAttribute(prefix, localName);
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00009274 File Offset: 0x00007474
		public override void WriteEndAttribute()
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (this.writeState != WriteState.Attribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", new object[]
				{
					"WriteEndAttribute",
					this.WriteState.ToString()
				})));
			}
			this.FlushBase64();
			try
			{
				if (this.isXmlAttribute)
				{
					if (this.attributeLocalName == "lang")
					{
						this.nsMgr.AddLangAttribute(this.attributeValue);
					}
					else if (this.attributeLocalName == "space")
					{
						if (this.attributeValue == "preserve")
						{
							this.nsMgr.AddSpaceAttribute(XmlSpace.Preserve);
						}
						else
						{
							if (!(this.attributeValue == "default"))
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("'{0}' is not a valid xml:space value. Valid values are 'default' and 'preserve'.", new object[] { this.attributeValue })));
							}
							this.nsMgr.AddSpaceAttribute(XmlSpace.Default);
						}
					}
					this.isXmlAttribute = false;
					this.attributeLocalName = null;
					this.attributeValue = null;
				}
				if (this.isXmlnsAttribute)
				{
					this.nsMgr.AddNamespaceIfNotDeclared(this.attributeLocalName, this.attributeValue, null);
					this.isXmlnsAttribute = false;
					this.attributeLocalName = null;
					this.attributeValue = null;
				}
				else
				{
					this.writer.WriteEndAttribute();
				}
			}
			finally
			{
				this.writeState = WriteState.Element;
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000093F0 File Offset: 0x000075F0
		public override void WriteComment(string text)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (this.writeState == WriteState.Attribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", new object[]
				{
					"WriteComment",
					this.WriteState.ToString()
				})));
			}
			if (text == null)
			{
				text = string.Empty;
			}
			else if (text.IndexOf("--", StringComparison.Ordinal) != -1 || (text.Length > 0 && text[text.Length - 1] == '-'))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("XML comments cannot contain '--' or end with '-'."), "text"));
			}
			this.StartComment();
			this.FlushBase64();
			this.writer.WriteComment(text);
			this.EndComment();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000094BC File Offset: 0x000076BC
		public override void WriteFullEndElement()
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (this.writeState == WriteState.Attribute)
			{
				this.WriteEndAttribute();
			}
			if (this.writeState != WriteState.Element && this.writeState != WriteState.Content)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", new object[]
				{
					"WriteFullEndElement",
					this.WriteState.ToString()
				})));
			}
			this.AutoComplete(WriteState.Content);
			this.WriteEndElement();
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00009540 File Offset: 0x00007740
		public override void WriteCData(string text)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (this.writeState == WriteState.Attribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", new object[]
				{
					"WriteCData",
					this.WriteState.ToString()
				})));
			}
			if (text == null)
			{
				text = string.Empty;
			}
			if (text.Length > 0)
			{
				this.StartContent();
				this.FlushBase64();
				this.writer.WriteCData(text);
				this.EndContent();
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000095CF File Offset: 0x000077CF
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("This XmlWriter implementation does not support the '{0}' method.", new object[] { "WriteDocType" })));
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000095F4 File Offset: 0x000077F4
		private void StartElement(ref string prefix, string localName, string ns, XmlDictionaryString xNs)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (this.documentState == XmlBaseWriter.DocumentState.Epilog)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("Only one root element is permitted per document.")));
			}
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
			}
			if (localName.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The empty string is not a valid local name."), "localName"));
			}
			if (this.writeState == WriteState.Attribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", new object[]
				{
					"WriteStartElement",
					this.WriteState.ToString()
				})));
			}
			this.FlushBase64();
			this.AutoComplete(WriteState.Element);
			XmlBaseWriter.Element element = this.EnterScope();
			if (ns == null)
			{
				if (prefix == null)
				{
					prefix = string.Empty;
				}
				ns = this.nsMgr.LookupNamespace(prefix);
				if (ns == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The prefix '{0}' is not defined.", new object[] { prefix }), "prefix"));
				}
			}
			else if (prefix == null)
			{
				prefix = this.nsMgr.LookupPrefix(ns);
				if (prefix == null)
				{
					prefix = string.Empty;
					this.nsMgr.AddNamespace(string.Empty, ns, xNs);
				}
			}
			else
			{
				this.nsMgr.AddNamespaceIfNotDeclared(prefix, ns, xNs);
			}
			element.Prefix = prefix;
			element.LocalName = localName;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000974F File Offset: 0x0000794F
		public override void WriteStartElement(string prefix, string localName, string namespaceUri)
		{
			this.StartElement(ref prefix, localName, namespaceUri, null);
			this.writer.WriteStartElement(prefix, localName);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00009769 File Offset: 0x00007969
		public override void WriteStartElement(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			this.StartElement(ref prefix, (localName != null) ? localName.Value : null, (namespaceUri != null) ? namespaceUri.Value : null, namespaceUri);
			this.writer.WriteStartElement(prefix, localName);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000979C File Offset: 0x0000799C
		public override void WriteEndElement()
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (this.depth == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("Cannot call '{0}' while Depth is '{1}'.", new object[]
				{
					"WriteEndElement",
					this.depth.ToString(CultureInfo.InvariantCulture)
				})));
			}
			if (this.writeState == WriteState.Attribute)
			{
				this.WriteEndAttribute();
			}
			this.FlushBase64();
			if (this.writeState == WriteState.Element)
			{
				this.nsMgr.DeclareNamespaces(this.writer);
				this.writer.WriteEndStartElement(true);
			}
			else
			{
				XmlBaseWriter.Element element = this.elements[this.depth];
				this.writer.WriteEndElement(element.Prefix, element.LocalName);
			}
			this.ExitScope();
			this.writeState = WriteState.Content;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00009864 File Offset: 0x00007A64
		private XmlBaseWriter.Element EnterScope()
		{
			this.nsMgr.EnterScope();
			this.depth++;
			if (this.elements == null)
			{
				this.elements = new XmlBaseWriter.Element[4];
			}
			else if (this.elements.Length == this.depth)
			{
				XmlBaseWriter.Element[] array = new XmlBaseWriter.Element[this.depth * 2];
				Array.Copy(this.elements, array, this.depth);
				this.elements = array;
			}
			XmlBaseWriter.Element element = this.elements[this.depth];
			if (element == null)
			{
				element = new XmlBaseWriter.Element();
				this.elements[this.depth] = element;
			}
			return element;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00009900 File Offset: 0x00007B00
		private void ExitScope()
		{
			this.elements[this.depth].Clear();
			this.depth--;
			if (this.depth == 0 && this.documentState == XmlBaseWriter.DocumentState.Document)
			{
				this.documentState = XmlBaseWriter.DocumentState.Epilog;
			}
			this.nsMgr.ExitScope();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00009950 File Offset: 0x00007B50
		protected void FlushElement()
		{
			if (this.writeState == WriteState.Element)
			{
				this.AutoComplete(WriteState.Content);
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00009962 File Offset: 0x00007B62
		protected void StartComment()
		{
			this.FlushElement();
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000996A File Offset: 0x00007B6A
		protected void EndComment()
		{
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000996C File Offset: 0x00007B6C
		protected void StartContent()
		{
			this.FlushElement();
			if (this.depth == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("Text cannot be written outside the root element.")));
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00009991 File Offset: 0x00007B91
		protected void StartContent(char ch)
		{
			this.FlushElement();
			if (this.depth == 0)
			{
				this.VerifyWhitespace(ch);
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000099A8 File Offset: 0x00007BA8
		protected void StartContent(string s)
		{
			this.FlushElement();
			if (this.depth == 0)
			{
				this.VerifyWhitespace(s);
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000099BF File Offset: 0x00007BBF
		protected void StartContent(char[] chars, int offset, int count)
		{
			this.FlushElement();
			if (this.depth == 0)
			{
				this.VerifyWhitespace(chars, offset, count);
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000099D8 File Offset: 0x00007BD8
		private void VerifyWhitespace(char ch)
		{
			if (!this.IsWhitespace(ch))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("Text cannot be written outside the root element.")));
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000099F8 File Offset: 0x00007BF8
		private void VerifyWhitespace(string s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!this.IsWhitespace(s[i]))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("Text cannot be written outside the root element.")));
				}
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00009A3C File Offset: 0x00007C3C
		private void VerifyWhitespace(char[] chars, int offset, int count)
		{
			for (int i = 0; i < count; i++)
			{
				if (!this.IsWhitespace(chars[offset + i]))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("Text cannot be written outside the root element.")));
				}
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00009A77 File Offset: 0x00007C77
		private bool IsWhitespace(char ch)
		{
			return ch == ' ' || ch == '\n' || ch == '\r' || ch == 't';
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009A8F File Offset: 0x00007C8F
		protected void EndContent()
		{
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00009A91 File Offset: 0x00007C91
		private void AutoComplete(WriteState writeState)
		{
			if (this.writeState == WriteState.Element)
			{
				this.EndStartElement();
			}
			this.writeState = writeState;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00009AA9 File Offset: 0x00007CA9
		private void EndStartElement()
		{
			this.nsMgr.DeclareNamespaces(this.writer);
			this.writer.WriteEndStartElement(false);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00009AC8 File Offset: 0x00007CC8
		public override string LookupPrefix(string ns)
		{
			if (ns == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("ns"));
			}
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			return this.nsMgr.LookupPrefix(ns);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009AF7 File Offset: 0x00007CF7
		internal string LookupNamespace(string prefix)
		{
			if (prefix == null)
			{
				return null;
			}
			return this.nsMgr.LookupNamespace(prefix);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00009B0C File Offset: 0x00007D0C
		private string GetQualifiedNamePrefix(string namespaceUri, XmlDictionaryString xNs)
		{
			string text = this.nsMgr.LookupPrefix(namespaceUri);
			if (text == null)
			{
				if (this.writeState != WriteState.Attribute)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The namespace '{0}' is not defined.", new object[] { namespaceUri }), "namespaceUri"));
				}
				text = this.GeneratePrefix(namespaceUri, xNs);
			}
			return text;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00009B60 File Offset: 0x00007D60
		public override void WriteQualifiedName(string localName, string namespaceUri)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
			}
			if (localName.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The empty string is not a valid local name."), "localName"));
			}
			if (namespaceUri == null)
			{
				namespaceUri = string.Empty;
			}
			string qualifiedNamePrefix = this.GetQualifiedNamePrefix(namespaceUri, null);
			if (qualifiedNamePrefix.Length != 0)
			{
				this.WriteString(qualifiedNamePrefix);
				this.WriteString(":");
			}
			this.WriteString(localName);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009BE4 File Offset: 0x00007DE4
		public override void WriteQualifiedName(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
			}
			if (localName.Value.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The empty string is not a valid local name."), "localName"));
			}
			if (namespaceUri == null)
			{
				namespaceUri = XmlDictionaryString.Empty;
			}
			string qualifiedNamePrefix = this.GetQualifiedNamePrefix(namespaceUri.Value, namespaceUri);
			this.FlushBase64();
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(qualifiedNamePrefix + ":" + namespaceUri.Value);
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.writer.WriteQualifiedName(qualifiedNamePrefix, localName);
				this.EndContent();
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009C98 File Offset: 0x00007E98
		public override void WriteStartDocument()
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (this.writeState != WriteState.Start)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", new object[]
				{
					"WriteStartDocument",
					this.WriteState.ToString()
				})));
			}
			this.writeState = WriteState.Prolog;
			this.documentState = XmlBaseWriter.DocumentState.Document;
			this.writer.WriteDeclaration();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009D0E File Offset: 0x00007F0E
		public override void WriteStartDocument(bool standalone)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.WriteStartDocument();
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009D24 File Offset: 0x00007F24
		public override void WriteProcessingInstruction(string name, string text)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (name != "xml")
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("Processing instructions (other than the XML declaration) and DTDs are not supported."), "name"));
			}
			if (this.writeState != WriteState.Start)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("XML declaration can only be written at the beginning of the document.")));
			}
			this.writer.WriteDeclaration();
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009D8E File Offset: 0x00007F8E
		private void FinishDocument()
		{
			if (this.writeState == WriteState.Attribute)
			{
				this.WriteEndAttribute();
			}
			while (this.depth > 0)
			{
				this.WriteEndElement();
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00009DB0 File Offset: 0x00007FB0
		public override void WriteEndDocument()
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (this.writeState == WriteState.Start || this.writeState == WriteState.Prolog)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("The document does not have a root element.")));
			}
			this.FinishDocument();
			this.writeState = WriteState.Start;
			this.documentState = XmlBaseWriter.DocumentState.End;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00009E05 File Offset: 0x00008005
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x00009E12 File Offset: 0x00008012
		protected int NamespaceBoundary
		{
			get
			{
				return this.nsMgr.NamespaceBoundary;
			}
			set
			{
				this.nsMgr.NamespaceBoundary = value;
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00009E20 File Offset: 0x00008020
		public override void WriteEntityRef(string name)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("This XmlWriter implementation does not support the '{0}' method.", new object[] { "WriteEntityRef" })));
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00009E44 File Offset: 0x00008044
		public override void WriteName(string name)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.WriteString(name);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00009E5B File Offset: 0x0000805B
		public override void WriteNmToken(string name)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("This XmlWriter implementation does not support the '{0}' method.", new object[] { "WriteNmToken" })));
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009E80 File Offset: 0x00008080
		public override void WriteWhitespace(string whitespace)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (whitespace == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("whitespace");
			}
			foreach (char c in whitespace)
			{
				if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("Only white space characters can be written with this method."), "whitespace"));
				}
			}
			this.WriteString(whitespace);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00009EF8 File Offset: 0x000080F8
		public override void WriteString(string value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (value == null)
			{
				value = string.Empty;
			}
			if (value.Length > 0 || this.inList)
			{
				this.FlushBase64();
				if (this.attributeValue != null)
				{
					this.WriteAttributeText(value);
				}
				if (!this.isXmlnsAttribute)
				{
					this.StartContent(value);
					this.writer.WriteEscapedText(value);
					this.EndContent();
				}
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00009F64 File Offset: 0x00008164
		public override void WriteString(XmlDictionaryString value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
			}
			if (value.Value.Length > 0)
			{
				this.FlushBase64();
				if (this.attributeValue != null)
				{
					this.WriteAttributeText(value.Value);
				}
				if (!this.isXmlnsAttribute)
				{
					this.StartContent(value.Value);
					this.writer.WriteEscapedText(value);
					this.EndContent();
				}
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009FDC File Offset: 0x000081DC
		public override void WriteChars(char[] chars, int offset, int count)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (chars == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > chars.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { chars.Length - offset })));
			}
			if (count > 0)
			{
				this.FlushBase64();
				if (this.attributeValue != null)
				{
					this.WriteAttributeText(new string(chars, offset, count));
				}
				if (!this.isXmlnsAttribute)
				{
					this.StartContent(chars, offset, count);
					this.writer.WriteEscapedText(chars, offset, count);
					this.EndContent();
				}
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000A0C0 File Offset: 0x000082C0
		public override void WriteRaw(string value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (value == null)
			{
				value = string.Empty;
			}
			if (value.Length > 0)
			{
				this.FlushBase64();
				if (this.attributeValue != null)
				{
					this.WriteAttributeText(value);
				}
				if (!this.isXmlnsAttribute)
				{
					this.StartContent(value);
					this.writer.WriteText(value);
					this.EndContent();
				}
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000A124 File Offset: 0x00008324
		public override void WriteRaw(char[] chars, int offset, int count)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (chars == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > chars.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { chars.Length - offset })));
			}
			if (count > 0)
			{
				this.FlushBase64();
				if (this.attributeValue != null)
				{
					this.WriteAttributeText(new string(chars, offset, count));
				}
				if (!this.isXmlnsAttribute)
				{
					this.StartContent(chars, offset, count);
					this.writer.WriteText(chars, offset, count);
					this.EndContent();
				}
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000A208 File Offset: 0x00008408
		public override void WriteCharEntity(char ch)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (ch >= '\ud800' && ch <= '\udfff')
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The surrogate pair is invalid. Missing a low surrogate character."), "ch"));
			}
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(ch.ToString());
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent(ch);
				this.FlushBase64();
				this.writer.WriteCharEntity((int)ch);
				this.EndContent();
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000A28C File Offset: 0x0000848C
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			SurrogateChar surrogateChar = new SurrogateChar(lowChar, highChar);
			if (this.attributeValue != null)
			{
				char[] array = new char[] { highChar, lowChar };
				this.WriteAttributeText(new string(array));
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.FlushBase64();
				this.writer.WriteCharEntity(surrogateChar.Char);
				this.EndContent();
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000A300 File Offset: 0x00008500
		public override void WriteValue(object value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
			}
			if (value is object[])
			{
				this.WriteValue((object[])value);
				return;
			}
			if (value is Array)
			{
				this.WriteValue((Array)value);
				return;
			}
			if (value is IStreamProvider)
			{
				this.WriteValue((IStreamProvider)value);
				return;
			}
			this.WritePrimitiveValue(value);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000A374 File Offset: 0x00008574
		protected void WritePrimitiveValue(object value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
			}
			if (value is ulong)
			{
				this.WriteValue((ulong)value);
				return;
			}
			if (value is string)
			{
				this.WriteValue((string)value);
				return;
			}
			if (value is int)
			{
				this.WriteValue((int)value);
				return;
			}
			if (value is long)
			{
				this.WriteValue((long)value);
				return;
			}
			if (value is bool)
			{
				this.WriteValue((bool)value);
				return;
			}
			if (value is double)
			{
				this.WriteValue((double)value);
				return;
			}
			if (value is DateTime)
			{
				this.WriteValue((DateTime)value);
				return;
			}
			if (value is float)
			{
				this.WriteValue((float)value);
				return;
			}
			if (value is decimal)
			{
				this.WriteValue((decimal)value);
				return;
			}
			if (value is XmlDictionaryString)
			{
				this.WriteValue((XmlDictionaryString)value);
				return;
			}
			if (value is UniqueId)
			{
				this.WriteValue((UniqueId)value);
				return;
			}
			if (value is Guid)
			{
				this.WriteValue((Guid)value);
				return;
			}
			if (value is TimeSpan)
			{
				this.WriteValue((TimeSpan)value);
				return;
			}
			if (value.GetType().IsArray)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("Nested arrays are not supported."), "value"));
			}
			base.WriteValue(value);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000A4E1 File Offset: 0x000086E1
		public override void WriteValue(string value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.WriteString(value);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000A4F8 File Offset: 0x000086F8
		public override void WriteValue(int value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.FlushBase64();
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(XmlConverter.ToString(value));
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.writer.WriteInt32Text(value);
				this.EndContent();
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000A550 File Offset: 0x00008750
		public override void WriteValue(long value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.FlushBase64();
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(XmlConverter.ToString(value));
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.writer.WriteInt64Text(value);
				this.EndContent();
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000A5A8 File Offset: 0x000087A8
		private void WriteValue(ulong value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.FlushBase64();
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(XmlConverter.ToString(value));
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.writer.WriteUInt64Text(value);
				this.EndContent();
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000A600 File Offset: 0x00008800
		public override void WriteValue(bool value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.FlushBase64();
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(XmlConverter.ToString(value));
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.writer.WriteBoolText(value);
				this.EndContent();
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000A658 File Offset: 0x00008858
		public override void WriteValue(decimal value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.FlushBase64();
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(XmlConverter.ToString(value));
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.writer.WriteDecimalText(value);
				this.EndContent();
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000A6B0 File Offset: 0x000088B0
		public override void WriteValue(float value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.FlushBase64();
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(XmlConverter.ToString(value));
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.writer.WriteFloatText(value);
				this.EndContent();
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000A708 File Offset: 0x00008908
		public override void WriteValue(double value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.FlushBase64();
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(XmlConverter.ToString(value));
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.writer.WriteDoubleText(value);
				this.EndContent();
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000A75D File Offset: 0x0000895D
		public override void WriteValue(XmlDictionaryString value)
		{
			this.WriteString(value);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000A768 File Offset: 0x00008968
		public override void WriteValue(DateTime value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.FlushBase64();
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(XmlConverter.ToString(value));
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.writer.WriteDateTimeText(value);
				this.EndContent();
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A7C0 File Offset: 0x000089C0
		public override void WriteValue(UniqueId value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
			}
			this.FlushBase64();
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(XmlConverter.ToString(value));
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.writer.WriteUniqueIdText(value);
				this.EndContent();
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A82C File Offset: 0x00008A2C
		public override void WriteValue(Guid value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.FlushBase64();
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(XmlConverter.ToString(value));
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.writer.WriteGuidText(value);
				this.EndContent();
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A884 File Offset: 0x00008A84
		public override void WriteValue(TimeSpan value)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.FlushBase64();
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(XmlConverter.ToString(value));
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.writer.WriteTimeSpanText(value);
				this.EndContent();
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A8DC File Offset: 0x00008ADC
		public override void WriteBase64(byte[] buffer, int offset, int count)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.EnsureBufferBounds(buffer, offset, count);
			if (count > 0)
			{
				if (this.trailByteCount > 0)
				{
					while (this.trailByteCount < 3 && count > 0)
					{
						byte[] array = this.trailBytes;
						int num = this.trailByteCount;
						this.trailByteCount = num + 1;
						array[num] = buffer[offset++];
						count--;
					}
				}
				int num2 = this.trailByteCount + count;
				int num3 = num2 - num2 % 3;
				if (this.trailBytes == null)
				{
					this.trailBytes = new byte[3];
				}
				if (num3 >= 3)
				{
					if (this.attributeValue != null)
					{
						this.WriteAttributeText(XmlConverter.Base64Encoding.GetString(this.trailBytes, 0, this.trailByteCount));
						this.WriteAttributeText(XmlConverter.Base64Encoding.GetString(buffer, offset, num3 - this.trailByteCount));
					}
					if (!this.isXmlnsAttribute)
					{
						this.StartContent();
						this.writer.WriteBase64Text(this.trailBytes, this.trailByteCount, buffer, offset, num3 - this.trailByteCount);
						this.EndContent();
					}
					this.trailByteCount = num2 - num3;
					if (this.trailByteCount > 0)
					{
						int num4 = offset + count - this.trailByteCount;
						for (int i = 0; i < this.trailByteCount; i++)
						{
							this.trailBytes[i] = buffer[num4++];
						}
						return;
					}
				}
				else
				{
					Buffer.BlockCopy(buffer, offset, this.trailBytes, this.trailByteCount, count);
					this.trailByteCount += count;
				}
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000AA47 File Offset: 0x00008C47
		internal override IAsyncResult BeginWriteBase64(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.EnsureBufferBounds(buffer, offset, count);
			return new XmlBaseWriter.WriteBase64AsyncResult(buffer, offset, count, this, callback, state);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000AA6D File Offset: 0x00008C6D
		internal override void EndWriteBase64(IAsyncResult result)
		{
			XmlBaseWriter.WriteBase64AsyncResult.End(result);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000AA75 File Offset: 0x00008C75
		internal override AsyncCompletionResult WriteBase64Async(AsyncEventArgs<XmlWriteBase64AsyncArguments> state)
		{
			if (this.nodeWriterAsyncHelper == null)
			{
				this.nodeWriterAsyncHelper = new XmlBaseWriter.XmlBaseWriterNodeWriterAsyncHelper(this);
			}
			this.nodeWriterAsyncHelper.SetArguments(state);
			if (this.nodeWriterAsyncHelper.StartAsync() == 1)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000AAA8 File Offset: 0x00008CA8
		public override void WriteBinHex(byte[] buffer, int offset, int count)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			this.EnsureBufferBounds(buffer, offset, count);
			this.WriteRaw(XmlBaseWriter.BinHexEncoding.GetString(buffer, offset, count));
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000AAD4 File Offset: 0x00008CD4
		public override bool CanCanonicalize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000AAD7 File Offset: 0x00008CD7
		protected bool Signing
		{
			get
			{
				return this.writer == this.signingWriter;
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000AAE8 File Offset: 0x00008CE8
		public override void StartCanonicalization(Stream stream, bool includeComments, string[] inclusivePrefixes)
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (this.Signing)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("XML canonicalization started")));
			}
			this.FlushElement();
			if (this.signingWriter == null)
			{
				this.signingWriter = this.CreateSigningNodeWriter();
			}
			this.signingWriter.SetOutput(this.writer, stream, includeComments, inclusivePrefixes);
			this.writer = this.signingWriter;
			this.SignScope(this.signingWriter.CanonicalWriter);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000AB6C File Offset: 0x00008D6C
		public override void EndCanonicalization()
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (!this.Signing)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("XML canonicalization was not started.")));
			}
			this.signingWriter.Flush();
			this.writer = this.signingWriter.NodeWriter;
		}

		// Token: 0x0600020B RID: 523
		protected abstract XmlSigningNodeWriter CreateSigningNodeWriter();

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000ABC0 File Offset: 0x00008DC0
		public virtual bool CanFragment
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000ABC4 File Offset: 0x00008DC4
		public void StartFragment(Stream stream, bool generateSelfContainedTextFragment)
		{
			if (!this.CanFragment)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
			}
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("stream"));
			}
			if (this.oldStream != null || this.oldWriter != null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
			}
			if (this.WriteState == WriteState.Attribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", new object[]
				{
					"StartFragment",
					this.WriteState.ToString()
				})));
			}
			this.FlushElement();
			this.writer.Flush();
			this.oldNamespaceBoundary = this.NamespaceBoundary;
			XmlStreamNodeWriter xmlStreamNodeWriter = null;
			if (generateSelfContainedTextFragment)
			{
				this.NamespaceBoundary = this.depth + 1;
				if (this.textFragmentWriter == null)
				{
					this.textFragmentWriter = new XmlUTF8NodeWriter();
				}
				this.textFragmentWriter.SetOutput(stream, false, Encoding.UTF8);
				xmlStreamNodeWriter = this.textFragmentWriter;
			}
			if (this.Signing)
			{
				if (xmlStreamNodeWriter != null)
				{
					this.oldWriter = this.signingWriter.NodeWriter;
					this.signingWriter.NodeWriter = xmlStreamNodeWriter;
					return;
				}
				this.oldStream = ((XmlStreamNodeWriter)this.signingWriter.NodeWriter).Stream;
				((XmlStreamNodeWriter)this.signingWriter.NodeWriter).Stream = stream;
				return;
			}
			else
			{
				if (xmlStreamNodeWriter != null)
				{
					this.oldWriter = this.writer;
					this.writer = xmlStreamNodeWriter;
					return;
				}
				this.oldStream = this.nodeWriter.Stream;
				this.nodeWriter.Stream = stream;
				return;
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000AD50 File Offset: 0x00008F50
		public void EndFragment()
		{
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (this.oldStream == null && this.oldWriter == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
			}
			if (this.WriteState == WriteState.Attribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", new object[]
				{
					"EndFragment",
					this.WriteState.ToString()
				})));
			}
			this.FlushElement();
			this.writer.Flush();
			if (this.Signing)
			{
				if (this.oldWriter != null)
				{
					this.signingWriter.NodeWriter = this.oldWriter;
				}
				else
				{
					((XmlStreamNodeWriter)this.signingWriter.NodeWriter).Stream = this.oldStream;
				}
			}
			else if (this.oldWriter != null)
			{
				this.writer = this.oldWriter;
			}
			else
			{
				this.nodeWriter.Stream = this.oldStream;
			}
			this.NamespaceBoundary = this.oldNamespaceBoundary;
			this.oldWriter = null;
			this.oldStream = null;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000AE5C File Offset: 0x0000905C
		public void WriteFragment(byte[] buffer, int offset, int count)
		{
			if (!this.CanFragment)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
			}
			if (this.IsClosed)
			{
				this.ThrowClosed();
			}
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { buffer.Length - offset })));
			}
			if (this.WriteState == WriteState.Attribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", new object[]
				{
					"WriteFragment",
					this.WriteState.ToString()
				})));
			}
			if (this.writer != this.nodeWriter)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
			}
			this.FlushElement();
			this.FlushBase64();
			this.nodeWriter.Flush();
			this.nodeWriter.Stream.Write(buffer, offset, count);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000AF94 File Offset: 0x00009194
		private void FlushBase64()
		{
			if (this.trailByteCount > 0)
			{
				this.FlushTrailBytes();
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000AFA8 File Offset: 0x000091A8
		private void FlushTrailBytes()
		{
			if (this.attributeValue != null)
			{
				this.WriteAttributeText(XmlConverter.Base64Encoding.GetString(this.trailBytes, 0, this.trailByteCount));
			}
			if (!this.isXmlnsAttribute)
			{
				this.StartContent();
				this.writer.WriteBase64Text(this.trailBytes, this.trailByteCount, this.trailBytes, 0, 0);
				this.EndContent();
			}
			this.trailByteCount = 0;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000B014 File Offset: 0x00009214
		private void WriteValue(object[] array)
		{
			this.FlushBase64();
			this.StartContent();
			this.writer.WriteStartListText();
			this.inList = true;
			for (int i = 0; i < array.Length; i++)
			{
				if (i != 0)
				{
					this.writer.WriteListSeparator();
				}
				this.WritePrimitiveValue(array[i]);
			}
			this.inList = false;
			this.writer.WriteEndListText();
			this.EndContent();
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000B07C File Offset: 0x0000927C
		private void WriteValue(Array array)
		{
			this.FlushBase64();
			this.StartContent();
			this.writer.WriteStartListText();
			this.inList = true;
			for (int i = 0; i < array.Length; i++)
			{
				if (i != 0)
				{
					this.writer.WriteListSeparator();
				}
				this.WritePrimitiveValue(array.GetValue(i));
			}
			this.inList = false;
			this.writer.WriteEndListText();
			this.EndContent();
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000B0EC File Offset: 0x000092EC
		protected void StartArray(int count)
		{
			this.FlushBase64();
			if (this.documentState == XmlBaseWriter.DocumentState.Epilog)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("Only one root element is permitted per document.")));
			}
			if (this.documentState == XmlBaseWriter.DocumentState.Document && count > 1 && this.depth == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("Only one root element is permitted per document.")));
			}
			if (this.writeState == WriteState.Attribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", new object[]
				{
					"WriteStartElement",
					this.WriteState.ToString()
				})));
			}
			this.AutoComplete(WriteState.Content);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000B191 File Offset: 0x00009391
		protected void EndArray()
		{
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000B194 File Offset: 0x00009394
		private void EnsureBufferBounds(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { buffer.Length - offset })));
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000B220 File Offset: 0x00009420
		private string GeneratePrefix(string ns, XmlDictionaryString xNs)
		{
			if (this.writeState != WriteState.Element && this.writeState != WriteState.Attribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("A prefix cannot be defined while WriteState is '{0}'.", new object[] { this.WriteState.ToString() })));
			}
			string text = this.nsMgr.AddNamespace(ns, xNs);
			if (text != null)
			{
				return text;
			}
			do
			{
				XmlBaseWriter.Element element = this.elements[this.depth];
				int prefixId = element.PrefixId;
				element.PrefixId = prefixId + 1;
				int num = prefixId;
				text = "d" + this.depth.ToString(CultureInfo.InvariantCulture) + "p" + num.ToString(CultureInfo.InvariantCulture);
			}
			while (this.nsMgr.LookupNamespace(text) != null);
			this.nsMgr.AddNamespace(text, ns, xNs);
			return text;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000B2EB File Offset: 0x000094EB
		protected void SignScope(XmlCanonicalWriter signingWriter)
		{
			this.nsMgr.Sign(signingWriter);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000B2F9 File Offset: 0x000094F9
		private void WriteAttributeText(string value)
		{
			if (this.attributeValue.Length == 0)
			{
				this.attributeValue = value;
				return;
			}
			this.attributeValue += value;
		}

		// Token: 0x040000C0 RID: 192
		private XmlNodeWriter writer;

		// Token: 0x040000C1 RID: 193
		private XmlBaseWriter.NamespaceManager nsMgr;

		// Token: 0x040000C2 RID: 194
		private XmlBaseWriter.Element[] elements;

		// Token: 0x040000C3 RID: 195
		private int depth;

		// Token: 0x040000C4 RID: 196
		private string attributeLocalName;

		// Token: 0x040000C5 RID: 197
		private string attributeValue;

		// Token: 0x040000C6 RID: 198
		private bool isXmlAttribute;

		// Token: 0x040000C7 RID: 199
		private bool isXmlnsAttribute;

		// Token: 0x040000C8 RID: 200
		private WriteState writeState;

		// Token: 0x040000C9 RID: 201
		private XmlBaseWriter.DocumentState documentState;

		// Token: 0x040000CA RID: 202
		private byte[] trailBytes;

		// Token: 0x040000CB RID: 203
		private int trailByteCount;

		// Token: 0x040000CC RID: 204
		private XmlStreamNodeWriter nodeWriter;

		// Token: 0x040000CD RID: 205
		private XmlSigningNodeWriter signingWriter;

		// Token: 0x040000CE RID: 206
		private XmlUTF8NodeWriter textFragmentWriter;

		// Token: 0x040000CF RID: 207
		private XmlNodeWriter oldWriter;

		// Token: 0x040000D0 RID: 208
		private Stream oldStream;

		// Token: 0x040000D1 RID: 209
		private int oldNamespaceBoundary;

		// Token: 0x040000D2 RID: 210
		private bool inList;

		// Token: 0x040000D3 RID: 211
		private const string xmlnsNamespace = "http://www.w3.org/2000/xmlns/";

		// Token: 0x040000D4 RID: 212
		private const string xmlNamespace = "http://www.w3.org/XML/1998/namespace";

		// Token: 0x040000D5 RID: 213
		private static BinHexEncoding binhexEncoding;

		// Token: 0x040000D6 RID: 214
		private static string[] prefixes = new string[]
		{
			"a", "b", "c", "d", "e", "f", "g", "h", "i", "j",
			"k", "l", "m", "n", "o", "p", "q", "r", "s", "t",
			"u", "v", "w", "x", "y", "z"
		};

		// Token: 0x040000D7 RID: 215
		private XmlBaseWriter.XmlBaseWriterNodeWriterAsyncHelper nodeWriterAsyncHelper;

		// Token: 0x02000143 RID: 323
		private class WriteBase64AsyncResult : AsyncResult
		{
			// Token: 0x0600126F RID: 4719 RVA: 0x0004C454 File Offset: 0x0004A654
			public WriteBase64AsyncResult(byte[] buffer, int offset, int count, XmlBaseWriter writer, AsyncCallback callback, object state)
				: base(callback, state)
			{
				this.writer = writer;
				this.buffer = buffer;
				this.offset = offset;
				this.count = count;
				bool flag = true;
				if (this.count > 0)
				{
					if (writer.trailByteCount > 0)
					{
						while (writer.trailByteCount < 3 && this.count > 0)
						{
							byte[] trailBytes = writer.trailBytes;
							int trailByteCount = writer.trailByteCount;
							writer.trailByteCount = trailByteCount + 1;
							int num = trailByteCount;
							trailByteCount = this.offset;
							this.offset = trailByteCount + 1;
							trailBytes[num] = buffer[trailByteCount];
							this.count--;
						}
					}
					this.totalByteCount = writer.trailByteCount + this.count;
					this.actualByteCount = this.totalByteCount - this.totalByteCount % 3;
					if (writer.trailBytes == null)
					{
						writer.trailBytes = new byte[3];
					}
					if (this.actualByteCount >= 3)
					{
						if (writer.attributeValue != null)
						{
							writer.WriteAttributeText(XmlConverter.Base64Encoding.GetString(writer.trailBytes, 0, writer.trailByteCount));
							writer.WriteAttributeText(XmlConverter.Base64Encoding.GetString(buffer, this.offset, this.actualByteCount - writer.trailByteCount));
						}
						flag = this.HandleWriteBase64Text(null);
					}
					else
					{
						Buffer.BlockCopy(buffer, this.offset, writer.trailBytes, writer.trailByteCount, this.count);
						writer.trailByteCount += this.count;
					}
				}
				if (flag)
				{
					base.Complete(true);
				}
			}

			// Token: 0x06001270 RID: 4720 RVA: 0x0004C5CF File Offset: 0x0004A7CF
			private static bool OnComplete(IAsyncResult result)
			{
				return ((XmlBaseWriter.WriteBase64AsyncResult)result.AsyncState).HandleWriteBase64Text(result);
			}

			// Token: 0x06001271 RID: 4721 RVA: 0x0004C5E4 File Offset: 0x0004A7E4
			private bool HandleWriteBase64Text(IAsyncResult result)
			{
				if (!this.writer.isXmlnsAttribute)
				{
					if (result == null)
					{
						this.writer.StartContent();
						result = this.writer.writer.BeginWriteBase64Text(this.writer.trailBytes, this.writer.trailByteCount, this.buffer, this.offset, this.actualByteCount - this.writer.trailByteCount, base.PrepareAsyncCompletion(XmlBaseWriter.WriteBase64AsyncResult.onComplete), this);
						if (!result.CompletedSynchronously)
						{
							return false;
						}
					}
					this.writer.writer.EndWriteBase64Text(result);
					this.writer.EndContent();
				}
				this.writer.trailByteCount = this.totalByteCount - this.actualByteCount;
				if (this.writer.trailByteCount > 0)
				{
					int num = this.offset + this.count - this.writer.trailByteCount;
					for (int i = 0; i < this.writer.trailByteCount; i++)
					{
						this.writer.trailBytes[i] = this.buffer[num++];
					}
				}
				return true;
			}

			// Token: 0x06001272 RID: 4722 RVA: 0x0004C6F7 File Offset: 0x0004A8F7
			public static void End(IAsyncResult result)
			{
				AsyncResult.End<XmlBaseWriter.WriteBase64AsyncResult>(result);
			}

			// Token: 0x040008F7 RID: 2295
			private static AsyncResult.AsyncCompletion onComplete = new AsyncResult.AsyncCompletion(XmlBaseWriter.WriteBase64AsyncResult.OnComplete);

			// Token: 0x040008F8 RID: 2296
			private XmlBaseWriter writer;

			// Token: 0x040008F9 RID: 2297
			private byte[] buffer;

			// Token: 0x040008FA RID: 2298
			private int offset;

			// Token: 0x040008FB RID: 2299
			private int count;

			// Token: 0x040008FC RID: 2300
			private int actualByteCount;

			// Token: 0x040008FD RID: 2301
			private int totalByteCount;
		}

		// Token: 0x02000144 RID: 324
		private class Element
		{
			// Token: 0x170003AC RID: 940
			// (get) Token: 0x06001274 RID: 4724 RVA: 0x0004C713 File Offset: 0x0004A913
			// (set) Token: 0x06001275 RID: 4725 RVA: 0x0004C71B File Offset: 0x0004A91B
			public string Prefix
			{
				get
				{
					return this.prefix;
				}
				set
				{
					this.prefix = value;
				}
			}

			// Token: 0x170003AD RID: 941
			// (get) Token: 0x06001276 RID: 4726 RVA: 0x0004C724 File Offset: 0x0004A924
			// (set) Token: 0x06001277 RID: 4727 RVA: 0x0004C72C File Offset: 0x0004A92C
			public string LocalName
			{
				get
				{
					return this.localName;
				}
				set
				{
					this.localName = value;
				}
			}

			// Token: 0x170003AE RID: 942
			// (get) Token: 0x06001278 RID: 4728 RVA: 0x0004C735 File Offset: 0x0004A935
			// (set) Token: 0x06001279 RID: 4729 RVA: 0x0004C73D File Offset: 0x0004A93D
			public int PrefixId
			{
				get
				{
					return this.prefixId;
				}
				set
				{
					this.prefixId = value;
				}
			}

			// Token: 0x0600127A RID: 4730 RVA: 0x0004C746 File Offset: 0x0004A946
			public void Clear()
			{
				this.prefix = null;
				this.localName = null;
				this.prefixId = 0;
			}

			// Token: 0x040008FE RID: 2302
			private string prefix;

			// Token: 0x040008FF RID: 2303
			private string localName;

			// Token: 0x04000900 RID: 2304
			private int prefixId;
		}

		// Token: 0x02000145 RID: 325
		private enum DocumentState : byte
		{
			// Token: 0x04000902 RID: 2306
			None,
			// Token: 0x04000903 RID: 2307
			Document,
			// Token: 0x04000904 RID: 2308
			Epilog,
			// Token: 0x04000905 RID: 2309
			End
		}

		// Token: 0x02000146 RID: 326
		private class NamespaceManager
		{
			// Token: 0x0600127C RID: 4732 RVA: 0x0004C768 File Offset: 0x0004A968
			public NamespaceManager()
			{
				this.defaultNamespace = new XmlBaseWriter.NamespaceManager.Namespace();
				this.defaultNamespace.Depth = 0;
				this.defaultNamespace.Prefix = string.Empty;
				this.defaultNamespace.Uri = string.Empty;
				this.defaultNamespace.UriDictionaryString = null;
			}

			// Token: 0x170003AF RID: 943
			// (get) Token: 0x0600127D RID: 4733 RVA: 0x0004C7BE File Offset: 0x0004A9BE
			public string XmlLang
			{
				get
				{
					return this.lang;
				}
			}

			// Token: 0x170003B0 RID: 944
			// (get) Token: 0x0600127E RID: 4734 RVA: 0x0004C7C6 File Offset: 0x0004A9C6
			public XmlSpace XmlSpace
			{
				get
				{
					return this.space;
				}
			}

			// Token: 0x0600127F RID: 4735 RVA: 0x0004C7D0 File Offset: 0x0004A9D0
			public void Clear()
			{
				if (this.namespaces == null)
				{
					this.namespaces = new XmlBaseWriter.NamespaceManager.Namespace[4];
					this.namespaces[0] = this.defaultNamespace;
				}
				this.nsCount = 1;
				this.nsTop = 0;
				this.depth = 0;
				this.attributeCount = 0;
				this.space = XmlSpace.None;
				this.lang = null;
				this.lastNameSpace = null;
				this.namespaceBoundary = 0;
			}

			// Token: 0x170003B1 RID: 945
			// (get) Token: 0x06001280 RID: 4736 RVA: 0x0004C837 File Offset: 0x0004AA37
			// (set) Token: 0x06001281 RID: 4737 RVA: 0x0004C840 File Offset: 0x0004AA40
			public int NamespaceBoundary
			{
				get
				{
					return this.namespaceBoundary;
				}
				set
				{
					int num = 0;
					while (num < this.nsCount && this.namespaces[num].Depth < value)
					{
						num++;
					}
					this.nsTop = num;
					this.namespaceBoundary = value;
					this.lastNameSpace = null;
				}
			}

			// Token: 0x06001282 RID: 4738 RVA: 0x0004C884 File Offset: 0x0004AA84
			public void Close()
			{
				if (this.depth == 0)
				{
					if (this.namespaces != null && this.namespaces.Length > 32)
					{
						this.namespaces = null;
					}
					if (this.attributes != null && this.attributes.Length > 4)
					{
						this.attributes = null;
					}
				}
				else
				{
					this.namespaces = null;
					this.attributes = null;
				}
				this.lang = null;
			}

			// Token: 0x06001283 RID: 4739 RVA: 0x0004C8E8 File Offset: 0x0004AAE8
			public void DeclareNamespaces(XmlNodeWriter writer)
			{
				for (int i = this.nsCount; i > 0; i--)
				{
					if (this.namespaces[i - 1].Depth != this.depth)
					{
						IL_0065:
						while (i < this.nsCount)
						{
							XmlBaseWriter.NamespaceManager.Namespace @namespace = this.namespaces[i];
							if (@namespace.UriDictionaryString != null)
							{
								writer.WriteXmlnsAttribute(@namespace.Prefix, @namespace.UriDictionaryString);
							}
							else
							{
								writer.WriteXmlnsAttribute(@namespace.Prefix, @namespace.Uri);
							}
							i++;
						}
						return;
					}
				}
				goto IL_0065;
			}

			// Token: 0x06001284 RID: 4740 RVA: 0x0004C963 File Offset: 0x0004AB63
			public void EnterScope()
			{
				this.depth++;
			}

			// Token: 0x06001285 RID: 4741 RVA: 0x0004C974 File Offset: 0x0004AB74
			public void ExitScope()
			{
				while (this.nsCount > 0)
				{
					XmlBaseWriter.NamespaceManager.Namespace @namespace = this.namespaces[this.nsCount - 1];
					if (@namespace.Depth != this.depth)
					{
						IL_0099:
						while (this.attributeCount > 0)
						{
							XmlBaseWriter.NamespaceManager.XmlAttribute xmlAttribute = this.attributes[this.attributeCount - 1];
							if (xmlAttribute.Depth != this.depth)
							{
								break;
							}
							this.space = xmlAttribute.XmlSpace;
							this.lang = xmlAttribute.XmlLang;
							xmlAttribute.Clear();
							this.attributeCount--;
						}
						this.depth--;
						return;
					}
					if (this.lastNameSpace == @namespace)
					{
						this.lastNameSpace = null;
					}
					@namespace.Clear();
					this.nsCount--;
				}
				goto IL_0099;
			}

			// Token: 0x06001286 RID: 4742 RVA: 0x0004CA31 File Offset: 0x0004AC31
			public void AddLangAttribute(string lang)
			{
				this.AddAttribute();
				this.lang = lang;
			}

			// Token: 0x06001287 RID: 4743 RVA: 0x0004CA40 File Offset: 0x0004AC40
			public void AddSpaceAttribute(XmlSpace space)
			{
				this.AddAttribute();
				this.space = space;
			}

			// Token: 0x06001288 RID: 4744 RVA: 0x0004CA50 File Offset: 0x0004AC50
			private void AddAttribute()
			{
				if (this.attributes == null)
				{
					this.attributes = new XmlBaseWriter.NamespaceManager.XmlAttribute[1];
				}
				else if (this.attributes.Length == this.attributeCount)
				{
					XmlBaseWriter.NamespaceManager.XmlAttribute[] array = new XmlBaseWriter.NamespaceManager.XmlAttribute[this.attributeCount * 2];
					Array.Copy(this.attributes, array, this.attributeCount);
					this.attributes = array;
				}
				XmlBaseWriter.NamespaceManager.XmlAttribute xmlAttribute = this.attributes[this.attributeCount];
				if (xmlAttribute == null)
				{
					xmlAttribute = new XmlBaseWriter.NamespaceManager.XmlAttribute();
					this.attributes[this.attributeCount] = xmlAttribute;
				}
				xmlAttribute.XmlLang = this.lang;
				xmlAttribute.XmlSpace = this.space;
				xmlAttribute.Depth = this.depth;
				this.attributeCount++;
			}

			// Token: 0x06001289 RID: 4745 RVA: 0x0004CB04 File Offset: 0x0004AD04
			public string AddNamespace(string uri, XmlDictionaryString uriDictionaryString)
			{
				if (uri.Length == 0)
				{
					this.AddNamespaceIfNotDeclared(string.Empty, uri, uriDictionaryString);
					return string.Empty;
				}
				for (int i = 0; i < XmlBaseWriter.prefixes.Length; i++)
				{
					string text = XmlBaseWriter.prefixes[i];
					bool flag = false;
					for (int j = this.nsCount - 1; j >= this.nsTop; j--)
					{
						if (this.namespaces[j].Prefix == text)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.AddNamespace(text, uri, uriDictionaryString);
						return text;
					}
				}
				return null;
			}

			// Token: 0x0600128A RID: 4746 RVA: 0x0004CB88 File Offset: 0x0004AD88
			public void AddNamespaceIfNotDeclared(string prefix, string uri, XmlDictionaryString uriDictionaryString)
			{
				if (this.LookupNamespace(prefix) != uri)
				{
					this.AddNamespace(prefix, uri, uriDictionaryString);
				}
			}

			// Token: 0x0600128B RID: 4747 RVA: 0x0004CBA4 File Offset: 0x0004ADA4
			public void AddNamespace(string prefix, string uri, XmlDictionaryString uriDictionaryString)
			{
				if (prefix.Length >= 3 && ((int)prefix[0] & -33) == 88 && ((int)prefix[1] & -33) == 77 && ((int)prefix[2] & -33) == 76)
				{
					if (prefix == "xml" && uri == "http://www.w3.org/XML/1998/namespace")
					{
						return;
					}
					if (prefix == "xmlns" && uri == "http://www.w3.org/2000/xmlns/")
					{
						return;
					}
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("Prefixes beginning with \"xml\" (regardless of casing) are reserved for use by XML."), "prefix"));
				}
				else
				{
					int i = this.nsCount - 1;
					XmlBaseWriter.NamespaceManager.Namespace @namespace;
					while (i >= 0)
					{
						@namespace = this.namespaces[i];
						if (@namespace.Depth != this.depth)
						{
							break;
						}
						if (@namespace.Prefix == prefix)
						{
							if (@namespace.Uri == uri)
							{
								return;
							}
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The prefix '{0}' is bound to the namespace '{1}' and cannot be changed to '{2}'.", new object[] { prefix, @namespace.Uri, uri }), "prefix"));
						}
						else
						{
							i--;
						}
					}
					if (prefix.Length != 0 && uri.Length == 0)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The empty namespace requires a null or empty prefix."), "prefix"));
					}
					if (uri.Length == "http://www.w3.org/2000/xmlns/".Length && uri == "http://www.w3.org/2000/xmlns/")
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The namespace '{1}' can only be bound to the prefix '{0}'.", new object[] { "xmlns", uri })));
					}
					if (uri.Length == "http://www.w3.org/XML/1998/namespace".Length && uri[18] == 'X' && uri == "http://www.w3.org/XML/1998/namespace")
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("The namespace '{1}' can only be bound to the prefix '{0}'.", new object[] { "xml", uri })));
					}
					if (this.namespaces.Length == this.nsCount)
					{
						XmlBaseWriter.NamespaceManager.Namespace[] array = new XmlBaseWriter.NamespaceManager.Namespace[this.nsCount * 2];
						Array.Copy(this.namespaces, array, this.nsCount);
						this.namespaces = array;
					}
					@namespace = this.namespaces[this.nsCount];
					if (@namespace == null)
					{
						@namespace = new XmlBaseWriter.NamespaceManager.Namespace();
						this.namespaces[this.nsCount] = @namespace;
					}
					@namespace.Depth = this.depth;
					@namespace.Prefix = prefix;
					@namespace.Uri = uri;
					@namespace.UriDictionaryString = uriDictionaryString;
					this.nsCount++;
					this.lastNameSpace = null;
					return;
				}
			}

			// Token: 0x0600128C RID: 4748 RVA: 0x0004CE00 File Offset: 0x0004B000
			public string LookupPrefix(string ns)
			{
				if (this.lastNameSpace != null && this.lastNameSpace.Uri == ns)
				{
					return this.lastNameSpace.Prefix;
				}
				int num = this.nsCount;
				for (int i = num - 1; i >= this.nsTop; i--)
				{
					XmlBaseWriter.NamespaceManager.Namespace @namespace = this.namespaces[i];
					if (@namespace.Uri == ns)
					{
						string prefix = @namespace.Prefix;
						bool flag = false;
						for (int j = i + 1; j < num; j++)
						{
							if (this.namespaces[j].Prefix == prefix)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.lastNameSpace = @namespace;
							return prefix;
						}
					}
				}
				for (int k = num - 1; k >= this.nsTop; k--)
				{
					XmlBaseWriter.NamespaceManager.Namespace namespace2 = this.namespaces[k];
					if (namespace2.Uri == ns)
					{
						string prefix2 = namespace2.Prefix;
						bool flag2 = false;
						for (int l = k + 1; l < num; l++)
						{
							if (this.namespaces[l].Prefix == prefix2)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							this.lastNameSpace = namespace2;
							return prefix2;
						}
					}
				}
				if (ns.Length == 0)
				{
					bool flag3 = true;
					for (int m = num - 1; m >= this.nsTop; m--)
					{
						if (this.namespaces[m].Prefix.Length == 0)
						{
							flag3 = false;
							break;
						}
					}
					if (flag3)
					{
						return string.Empty;
					}
				}
				if (ns == "http://www.w3.org/2000/xmlns/")
				{
					return "xmlns";
				}
				if (ns == "http://www.w3.org/XML/1998/namespace")
				{
					return "xml";
				}
				return null;
			}

			// Token: 0x0600128D RID: 4749 RVA: 0x0004CF8C File Offset: 0x0004B18C
			public string LookupAttributePrefix(string ns)
			{
				if (this.lastNameSpace != null && this.lastNameSpace.Uri == ns && this.lastNameSpace.Prefix.Length != 0)
				{
					return this.lastNameSpace.Prefix;
				}
				int num = this.nsCount;
				for (int i = num - 1; i >= this.nsTop; i--)
				{
					XmlBaseWriter.NamespaceManager.Namespace @namespace = this.namespaces[i];
					if (@namespace.Uri == ns)
					{
						string prefix = @namespace.Prefix;
						if (prefix.Length != 0)
						{
							bool flag = false;
							for (int j = i + 1; j < num; j++)
							{
								if (this.namespaces[j].Prefix == prefix)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								this.lastNameSpace = @namespace;
								return prefix;
							}
						}
					}
				}
				for (int k = num - 1; k >= this.nsTop; k--)
				{
					XmlBaseWriter.NamespaceManager.Namespace namespace2 = this.namespaces[k];
					if (namespace2.Uri == ns)
					{
						string prefix2 = namespace2.Prefix;
						if (prefix2.Length != 0)
						{
							bool flag2 = false;
							for (int l = k + 1; l < num; l++)
							{
								if (this.namespaces[l].Prefix == prefix2)
								{
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								this.lastNameSpace = namespace2;
								return prefix2;
							}
						}
					}
				}
				if (ns.Length == 0)
				{
					return string.Empty;
				}
				return null;
			}

			// Token: 0x0600128E RID: 4750 RVA: 0x0004D0E0 File Offset: 0x0004B2E0
			public string LookupNamespace(string prefix)
			{
				int num = this.nsCount;
				if (prefix.Length == 0)
				{
					for (int i = num - 1; i >= this.nsTop; i--)
					{
						XmlBaseWriter.NamespaceManager.Namespace @namespace = this.namespaces[i];
						if (@namespace.Prefix.Length == 0)
						{
							return @namespace.Uri;
						}
					}
					return string.Empty;
				}
				if (prefix.Length == 1)
				{
					char c = prefix[0];
					for (int j = num - 1; j >= this.nsTop; j--)
					{
						XmlBaseWriter.NamespaceManager.Namespace namespace2 = this.namespaces[j];
						if (namespace2.PrefixChar == c)
						{
							return namespace2.Uri;
						}
					}
					return null;
				}
				for (int k = num - 1; k >= this.nsTop; k--)
				{
					XmlBaseWriter.NamespaceManager.Namespace namespace3 = this.namespaces[k];
					if (namespace3.Prefix == prefix)
					{
						return namespace3.Uri;
					}
				}
				if (prefix == "xmlns")
				{
					return "http://www.w3.org/2000/xmlns/";
				}
				if (prefix == "xml")
				{
					return "http://www.w3.org/XML/1998/namespace";
				}
				return null;
			}

			// Token: 0x0600128F RID: 4751 RVA: 0x0004D1DC File Offset: 0x0004B3DC
			public void Sign(XmlCanonicalWriter signingWriter)
			{
				int num = this.nsCount;
				for (int i = 1; i < num; i++)
				{
					XmlBaseWriter.NamespaceManager.Namespace @namespace = this.namespaces[i];
					bool flag = false;
					int num2 = i + 1;
					while (num2 < num && !flag)
					{
						flag = @namespace.Prefix == this.namespaces[num2].Prefix;
						num2++;
					}
					if (!flag)
					{
						signingWriter.WriteXmlnsAttribute(@namespace.Prefix, @namespace.Uri);
					}
				}
			}

			// Token: 0x04000906 RID: 2310
			private XmlBaseWriter.NamespaceManager.Namespace[] namespaces;

			// Token: 0x04000907 RID: 2311
			private XmlBaseWriter.NamespaceManager.Namespace lastNameSpace;

			// Token: 0x04000908 RID: 2312
			private int nsCount;

			// Token: 0x04000909 RID: 2313
			private int depth;

			// Token: 0x0400090A RID: 2314
			private XmlBaseWriter.NamespaceManager.XmlAttribute[] attributes;

			// Token: 0x0400090B RID: 2315
			private int attributeCount;

			// Token: 0x0400090C RID: 2316
			private XmlSpace space;

			// Token: 0x0400090D RID: 2317
			private string lang;

			// Token: 0x0400090E RID: 2318
			private int namespaceBoundary;

			// Token: 0x0400090F RID: 2319
			private int nsTop;

			// Token: 0x04000910 RID: 2320
			private XmlBaseWriter.NamespaceManager.Namespace defaultNamespace;

			// Token: 0x020001A7 RID: 423
			private class XmlAttribute
			{
				// Token: 0x17000471 RID: 1137
				// (get) Token: 0x06001545 RID: 5445 RVA: 0x00055415 File Offset: 0x00053615
				// (set) Token: 0x06001546 RID: 5446 RVA: 0x0005541D File Offset: 0x0005361D
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

				// Token: 0x17000472 RID: 1138
				// (get) Token: 0x06001547 RID: 5447 RVA: 0x00055426 File Offset: 0x00053626
				// (set) Token: 0x06001548 RID: 5448 RVA: 0x0005542E File Offset: 0x0005362E
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

				// Token: 0x17000473 RID: 1139
				// (get) Token: 0x06001549 RID: 5449 RVA: 0x00055437 File Offset: 0x00053637
				// (set) Token: 0x0600154A RID: 5450 RVA: 0x0005543F File Offset: 0x0005363F
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

				// Token: 0x0600154B RID: 5451 RVA: 0x00055448 File Offset: 0x00053648
				public void Clear()
				{
					this.lang = null;
				}

				// Token: 0x04000A82 RID: 2690
				private XmlSpace space;

				// Token: 0x04000A83 RID: 2691
				private string lang;

				// Token: 0x04000A84 RID: 2692
				private int depth;
			}

			// Token: 0x020001A8 RID: 424
			private class Namespace
			{
				// Token: 0x0600154D RID: 5453 RVA: 0x00055459 File Offset: 0x00053659
				public void Clear()
				{
					this.prefix = null;
					this.prefixChar = '\0';
					this.ns = null;
					this.xNs = null;
					this.depth = 0;
				}

				// Token: 0x17000474 RID: 1140
				// (get) Token: 0x0600154E RID: 5454 RVA: 0x0005547E File Offset: 0x0005367E
				// (set) Token: 0x0600154F RID: 5455 RVA: 0x00055486 File Offset: 0x00053686
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

				// Token: 0x17000475 RID: 1141
				// (get) Token: 0x06001550 RID: 5456 RVA: 0x0005548F File Offset: 0x0005368F
				public char PrefixChar
				{
					get
					{
						return this.prefixChar;
					}
				}

				// Token: 0x17000476 RID: 1142
				// (get) Token: 0x06001551 RID: 5457 RVA: 0x00055497 File Offset: 0x00053697
				// (set) Token: 0x06001552 RID: 5458 RVA: 0x0005549F File Offset: 0x0005369F
				public string Prefix
				{
					get
					{
						return this.prefix;
					}
					set
					{
						if (value.Length == 1)
						{
							this.prefixChar = value[0];
						}
						else
						{
							this.prefixChar = '\0';
						}
						this.prefix = value;
					}
				}

				// Token: 0x17000477 RID: 1143
				// (get) Token: 0x06001553 RID: 5459 RVA: 0x000554C7 File Offset: 0x000536C7
				// (set) Token: 0x06001554 RID: 5460 RVA: 0x000554CF File Offset: 0x000536CF
				public string Uri
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

				// Token: 0x17000478 RID: 1144
				// (get) Token: 0x06001555 RID: 5461 RVA: 0x000554D8 File Offset: 0x000536D8
				// (set) Token: 0x06001556 RID: 5462 RVA: 0x000554E0 File Offset: 0x000536E0
				public XmlDictionaryString UriDictionaryString
				{
					get
					{
						return this.xNs;
					}
					set
					{
						this.xNs = value;
					}
				}

				// Token: 0x04000A85 RID: 2693
				private string prefix;

				// Token: 0x04000A86 RID: 2694
				private string ns;

				// Token: 0x04000A87 RID: 2695
				private XmlDictionaryString xNs;

				// Token: 0x04000A88 RID: 2696
				private int depth;

				// Token: 0x04000A89 RID: 2697
				private char prefixChar;
			}
		}

		// Token: 0x02000147 RID: 327
		private class XmlBaseWriterNodeWriterAsyncHelper
		{
			// Token: 0x06001290 RID: 4752 RVA: 0x0004D24B File Offset: 0x0004B44B
			public XmlBaseWriterNodeWriterAsyncHelper(XmlBaseWriter writer)
			{
				this.writer = writer;
			}

			// Token: 0x06001291 RID: 4753 RVA: 0x0004D25A File Offset: 0x0004B45A
			public void SetArguments(AsyncEventArgs<XmlWriteBase64AsyncArguments> inputState)
			{
				this.inputState = inputState;
				this.buffer = inputState.Arguments.Buffer;
				this.offset = inputState.Arguments.Offset;
				this.count = inputState.Arguments.Count;
			}

			// Token: 0x06001292 RID: 4754 RVA: 0x0004D298 File Offset: 0x0004B498
			public AsyncCompletionResult StartAsync()
			{
				bool flag = true;
				if (this.count > 0)
				{
					if (this.writer.trailByteCount > 0)
					{
						while (this.writer.trailByteCount < 3 && this.count > 0)
						{
							byte[] trailBytes = this.writer.trailBytes;
							XmlBaseWriter xmlBaseWriter = this.writer;
							int trailByteCount = xmlBaseWriter.trailByteCount;
							xmlBaseWriter.trailByteCount = trailByteCount + 1;
							int num = trailByteCount;
							byte[] array = this.buffer;
							trailByteCount = this.offset;
							this.offset = trailByteCount + 1;
							trailBytes[num] = array[trailByteCount];
							this.count--;
						}
					}
					this.totalByteCount = this.writer.trailByteCount + this.count;
					this.actualByteCount = this.totalByteCount - this.totalByteCount % 3;
					if (this.writer.trailBytes == null)
					{
						this.writer.trailBytes = new byte[3];
					}
					if (this.actualByteCount >= 3)
					{
						if (this.writer.attributeValue != null)
						{
							this.writer.WriteAttributeText(XmlConverter.Base64Encoding.GetString(this.writer.trailBytes, 0, this.writer.trailByteCount));
							this.writer.WriteAttributeText(XmlConverter.Base64Encoding.GetString(this.buffer, this.offset, this.actualByteCount - this.writer.trailByteCount));
						}
						flag = this.HandleWriteBase64Text(false);
					}
					else
					{
						Buffer.BlockCopy(this.buffer, this.offset, this.writer.trailBytes, this.writer.trailByteCount, this.count);
						this.writer.trailByteCount += this.count;
					}
				}
				if (flag)
				{
					this.Clear();
					return 1;
				}
				return 0;
			}

			// Token: 0x06001293 RID: 4755 RVA: 0x0004D440 File Offset: 0x0004B640
			private static void OnWriteComplete(IAsyncEventArgs asyncEventArgs)
			{
				bool flag = false;
				Exception ex = null;
				XmlBaseWriter.XmlBaseWriterNodeWriterAsyncHelper xmlBaseWriterNodeWriterAsyncHelper = (XmlBaseWriter.XmlBaseWriterNodeWriterAsyncHelper)asyncEventArgs.AsyncState;
				AsyncEventArgs<XmlWriteBase64AsyncArguments> asyncEventArgs2 = xmlBaseWriterNodeWriterAsyncHelper.inputState;
				try
				{
					if (asyncEventArgs.Exception != null)
					{
						ex = asyncEventArgs.Exception;
						flag = true;
					}
					else
					{
						flag = xmlBaseWriterNodeWriterAsyncHelper.HandleWriteBase64Text(true);
					}
				}
				catch (Exception ex2)
				{
					if (Fx.IsFatal(ex2))
					{
						throw;
					}
					ex = ex2;
					flag = true;
				}
				if (flag)
				{
					xmlBaseWriterNodeWriterAsyncHelper.Clear();
					asyncEventArgs2.Complete(false, ex);
				}
			}

			// Token: 0x06001294 RID: 4756 RVA: 0x0004D4B4 File Offset: 0x0004B6B4
			private bool HandleWriteBase64Text(bool isAsyncCallback)
			{
				if (!this.writer.isXmlnsAttribute)
				{
					if (!isAsyncCallback)
					{
						if (this.nodeWriterAsyncState == null)
						{
							this.nodeWriterAsyncState = new AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs>();
							this.nodeWriterArgs = new XmlNodeWriterWriteBase64TextArgs();
						}
						if (XmlBaseWriter.XmlBaseWriterNodeWriterAsyncHelper.onWriteComplete == null)
						{
							XmlBaseWriter.XmlBaseWriterNodeWriterAsyncHelper.onWriteComplete = new AsyncEventArgsCallback(XmlBaseWriter.XmlBaseWriterNodeWriterAsyncHelper.OnWriteComplete);
						}
						this.writer.StartContent();
						this.nodeWriterArgs.TrailBuffer = this.writer.trailBytes;
						this.nodeWriterArgs.TrailCount = this.writer.trailByteCount;
						this.nodeWriterArgs.Buffer = this.buffer;
						this.nodeWriterArgs.Offset = this.offset;
						this.nodeWriterArgs.Count = this.actualByteCount - this.writer.trailByteCount;
						this.nodeWriterAsyncState.Set(XmlBaseWriter.XmlBaseWriterNodeWriterAsyncHelper.onWriteComplete, this.nodeWriterArgs, this);
						if (this.writer.writer.WriteBase64TextAsync(this.nodeWriterAsyncState) != 1)
						{
							return false;
						}
						this.nodeWriterAsyncState.Complete(true);
					}
					this.writer.EndContent();
				}
				this.writer.trailByteCount = this.totalByteCount - this.actualByteCount;
				if (this.writer.trailByteCount > 0)
				{
					int num = this.offset + this.count - this.writer.trailByteCount;
					for (int i = 0; i < this.writer.trailByteCount; i++)
					{
						this.writer.trailBytes[i] = this.buffer[num++];
					}
				}
				return true;
			}

			// Token: 0x06001295 RID: 4757 RVA: 0x0004D63C File Offset: 0x0004B83C
			private void Clear()
			{
				this.inputState = null;
				this.buffer = null;
				this.offset = 0;
				this.count = 0;
				this.actualByteCount = 0;
				this.totalByteCount = 0;
			}

			// Token: 0x04000911 RID: 2321
			private static AsyncEventArgsCallback onWriteComplete;

			// Token: 0x04000912 RID: 2322
			private XmlBaseWriter writer;

			// Token: 0x04000913 RID: 2323
			private byte[] buffer;

			// Token: 0x04000914 RID: 2324
			private int offset;

			// Token: 0x04000915 RID: 2325
			private int count;

			// Token: 0x04000916 RID: 2326
			private int actualByteCount;

			// Token: 0x04000917 RID: 2327
			private int totalByteCount;

			// Token: 0x04000918 RID: 2328
			private AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs> nodeWriterAsyncState;

			// Token: 0x04000919 RID: 2329
			private XmlNodeWriterWriteBase64TextArgs nodeWriterArgs;

			// Token: 0x0400091A RID: 2330
			private AsyncEventArgs<XmlWriteBase64AsyncArguments> inputState;
		}
	}
}
