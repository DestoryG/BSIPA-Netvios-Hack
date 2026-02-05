using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x0200008C RID: 140
	internal class ExtensionDataReader : XmlReader
	{
		// Token: 0x060009C1 RID: 2497 RVA: 0x0002A800 File Offset: 0x00028A00
		[SecuritySafeCritical]
		static ExtensionDataReader()
		{
			ExtensionDataReader.AddPrefix("i", "http://www.w3.org/2001/XMLSchema-instance");
			ExtensionDataReader.AddPrefix("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			ExtensionDataReader.AddPrefix(string.Empty, string.Empty);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0002A84E File Offset: 0x00028A4E
		internal ExtensionDataReader(XmlObjectSerializerReadContext context)
		{
			this.attributeIndex = -1;
			this.context = context;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0002A870 File Offset: 0x00028A70
		internal void SetDeserializedValue(object obj)
		{
			IDataNode dataNode = ((this.deserializedDataNodes == null || this.deserializedDataNodes.Count == 0) ? null : this.deserializedDataNodes.Dequeue());
			if (dataNode != null && !(obj is IDataNode))
			{
				dataNode.Value = obj;
				dataNode.IsFinalValue = true;
			}
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0002A8BA File Offset: 0x00028ABA
		internal IDataNode GetCurrentNode()
		{
			IDataNode dataNode = this.element.dataNode;
			this.Skip();
			return dataNode;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0002A8CD File Offset: 0x00028ACD
		internal void SetDataNode(IDataNode dataNode, string name, string ns)
		{
			this.SetNextElement(dataNode, name, ns, null);
			this.element = this.nextElement;
			this.nextElement = null;
			this.SetElement();
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0002A8F4 File Offset: 0x00028AF4
		internal void Reset()
		{
			this.localName = null;
			this.ns = null;
			this.prefix = null;
			this.value = null;
			this.attributeCount = 0;
			this.attributeIndex = -1;
			this.depth = 0;
			this.element = null;
			this.nextElement = null;
			this.elements = null;
			this.deserializedDataNodes = null;
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x0002A94E File Offset: 0x00028B4E
		private bool IsXmlDataNode
		{
			get
			{
				return this.internalNodeType == ExtensionDataReader.ExtensionDataNodeType.Xml;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0002A959 File Offset: 0x00028B59
		public override XmlNodeType NodeType
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return this.nodeType;
				}
				return this.xmlNodeReader.NodeType;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x0002A975 File Offset: 0x00028B75
		public override string LocalName
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return this.localName;
				}
				return this.xmlNodeReader.LocalName;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x0002A991 File Offset: 0x00028B91
		public override string NamespaceURI
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return this.ns;
				}
				return this.xmlNodeReader.NamespaceURI;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x0002A9AD File Offset: 0x00028BAD
		public override string Prefix
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return this.prefix;
				}
				return this.xmlNodeReader.Prefix;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0002A9C9 File Offset: 0x00028BC9
		public override string Value
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return this.value;
				}
				return this.xmlNodeReader.Value;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060009CD RID: 2509 RVA: 0x0002A9E5 File Offset: 0x00028BE5
		public override int Depth
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return this.depth;
				}
				return this.xmlNodeReader.Depth;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x0002AA01 File Offset: 0x00028C01
		public override int AttributeCount
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return this.attributeCount;
				}
				return this.xmlNodeReader.AttributeCount;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x0002AA1D File Offset: 0x00028C1D
		public override bool EOF
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return this.readState == ReadState.EndOfFile;
				}
				return this.xmlNodeReader.EOF;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x0002AA3C File Offset: 0x00028C3C
		public override ReadState ReadState
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return this.readState;
				}
				return this.xmlNodeReader.ReadState;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x0002AA58 File Offset: 0x00028C58
		public override bool IsEmptyElement
		{
			get
			{
				return this.IsXmlDataNode && this.xmlNodeReader.IsEmptyElement;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x0002AA6F File Offset: 0x00028C6F
		public override bool IsDefault
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return base.IsDefault;
				}
				return this.xmlNodeReader.IsDefault;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x0002AA8B File Offset: 0x00028C8B
		public override char QuoteChar
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return base.QuoteChar;
				}
				return this.xmlNodeReader.QuoteChar;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x0002AAA7 File Offset: 0x00028CA7
		public override XmlSpace XmlSpace
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return base.XmlSpace;
				}
				return this.xmlNodeReader.XmlSpace;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x0002AAC3 File Offset: 0x00028CC3
		public override string XmlLang
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return base.XmlLang;
				}
				return this.xmlNodeReader.XmlLang;
			}
		}

		// Token: 0x17000196 RID: 406
		public override string this[int i]
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return this.GetAttribute(i);
				}
				return this.xmlNodeReader[i];
			}
		}

		// Token: 0x17000197 RID: 407
		public override string this[string name]
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return this.GetAttribute(name);
				}
				return this.xmlNodeReader[name];
			}
		}

		// Token: 0x17000198 RID: 408
		public override string this[string name, string namespaceURI]
		{
			get
			{
				if (!this.IsXmlDataNode)
				{
					return this.GetAttribute(name, namespaceURI);
				}
				return this.xmlNodeReader[name, namespaceURI];
			}
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0002AB3B File Offset: 0x00028D3B
		public override bool MoveToFirstAttribute()
		{
			if (this.IsXmlDataNode)
			{
				return this.xmlNodeReader.MoveToFirstAttribute();
			}
			if (this.attributeCount == 0)
			{
				return false;
			}
			this.MoveToAttribute(0);
			return true;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0002AB63 File Offset: 0x00028D63
		public override bool MoveToNextAttribute()
		{
			if (this.IsXmlDataNode)
			{
				return this.xmlNodeReader.MoveToNextAttribute();
			}
			if (this.attributeIndex + 1 >= this.attributeCount)
			{
				return false;
			}
			this.MoveToAttribute(this.attributeIndex + 1);
			return true;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0002AB9C File Offset: 0x00028D9C
		public override void MoveToAttribute(int index)
		{
			if (this.IsXmlDataNode)
			{
				this.xmlNodeReader.MoveToAttribute(index);
				return;
			}
			if (index < 0 || index >= this.attributeCount)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid XML while deserializing extension data.")));
			}
			this.nodeType = XmlNodeType.Attribute;
			AttributeData attributeData = this.element.attributes[index];
			this.localName = attributeData.localName;
			this.ns = attributeData.ns;
			this.prefix = attributeData.prefix;
			this.value = attributeData.value;
			this.attributeIndex = index;
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0002AC2C File Offset: 0x00028E2C
		public override string GetAttribute(string name, string namespaceURI)
		{
			if (this.IsXmlDataNode)
			{
				return this.xmlNodeReader.GetAttribute(name, namespaceURI);
			}
			for (int i = 0; i < this.element.attributeCount; i++)
			{
				AttributeData attributeData = this.element.attributes[i];
				if (attributeData.localName == name && attributeData.ns == namespaceURI)
				{
					return attributeData.value;
				}
			}
			return null;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0002AC98 File Offset: 0x00028E98
		public override bool MoveToAttribute(string name, string namespaceURI)
		{
			if (this.IsXmlDataNode)
			{
				return this.xmlNodeReader.MoveToAttribute(name, this.ns);
			}
			for (int i = 0; i < this.element.attributeCount; i++)
			{
				AttributeData attributeData = this.element.attributes[i];
				if (attributeData.localName == name && attributeData.ns == namespaceURI)
				{
					this.MoveToAttribute(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0002AD0A File Offset: 0x00028F0A
		public override bool MoveToElement()
		{
			if (this.IsXmlDataNode)
			{
				return this.xmlNodeReader.MoveToElement();
			}
			if (this.nodeType != XmlNodeType.Attribute)
			{
				return false;
			}
			this.SetElement();
			return true;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0002AD34 File Offset: 0x00028F34
		private void SetElement()
		{
			this.nodeType = XmlNodeType.Element;
			this.localName = this.element.localName;
			this.ns = this.element.ns;
			this.prefix = this.element.prefix;
			this.value = string.Empty;
			this.attributeCount = this.element.attributeCount;
			this.attributeIndex = -1;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0002ADA0 File Offset: 0x00028FA0
		[SecuritySafeCritical]
		public override string LookupNamespace(string prefix)
		{
			if (this.IsXmlDataNode)
			{
				return this.xmlNodeReader.LookupNamespace(prefix);
			}
			string text;
			if (!ExtensionDataReader.prefixToNsTable.TryGetValue(prefix, out text))
			{
				return null;
			}
			return text;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0002ADD4 File Offset: 0x00028FD4
		public override void Skip()
		{
			if (this.IsXmlDataNode)
			{
				this.xmlNodeReader.Skip();
				return;
			}
			if (this.ReadState != ReadState.Interactive)
			{
				return;
			}
			this.MoveToElement();
			if (this.IsElementNode(this.internalNodeType))
			{
				int num = 1;
				while (num != 0)
				{
					if (!this.Read())
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid XML while deserializing extension data.")));
					}
					if (this.IsElementNode(this.internalNodeType))
					{
						num++;
					}
					else if (this.internalNodeType == ExtensionDataReader.ExtensionDataNodeType.EndElement)
					{
						this.ReadEndElement();
						num--;
					}
				}
				return;
			}
			this.Read();
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0002AE67 File Offset: 0x00029067
		private bool IsElementNode(ExtensionDataReader.ExtensionDataNodeType nodeType)
		{
			return nodeType == ExtensionDataReader.ExtensionDataNodeType.Element || nodeType == ExtensionDataReader.ExtensionDataNodeType.ReferencedElement || nodeType == ExtensionDataReader.ExtensionDataNodeType.NullElement;
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0002AE77 File Offset: 0x00029077
		public override void Close()
		{
			if (this.IsXmlDataNode)
			{
				this.xmlNodeReader.Close();
				return;
			}
			this.Reset();
			this.readState = ReadState.Closed;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0002AE9C File Offset: 0x0002909C
		public override bool Read()
		{
			if (this.nodeType == XmlNodeType.Attribute && this.MoveToNextAttribute())
			{
				return true;
			}
			this.MoveNext(this.element.dataNode);
			switch (this.internalNodeType)
			{
			case ExtensionDataReader.ExtensionDataNodeType.None:
				if (this.depth != 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid XML while deserializing extension data.")));
				}
				this.nodeType = XmlNodeType.None;
				this.prefix = string.Empty;
				this.ns = string.Empty;
				this.localName = string.Empty;
				this.value = string.Empty;
				this.attributeCount = 0;
				this.readState = ReadState.EndOfFile;
				return false;
			case ExtensionDataReader.ExtensionDataNodeType.Element:
			case ExtensionDataReader.ExtensionDataNodeType.ReferencedElement:
			case ExtensionDataReader.ExtensionDataNodeType.NullElement:
				this.PushElement();
				this.SetElement();
				break;
			case ExtensionDataReader.ExtensionDataNodeType.EndElement:
				this.nodeType = XmlNodeType.EndElement;
				this.prefix = string.Empty;
				this.ns = string.Empty;
				this.localName = string.Empty;
				this.value = string.Empty;
				this.attributeCount = 0;
				this.attributeIndex = -1;
				this.PopElement();
				break;
			case ExtensionDataReader.ExtensionDataNodeType.Text:
				this.nodeType = XmlNodeType.Text;
				this.prefix = string.Empty;
				this.ns = string.Empty;
				this.localName = string.Empty;
				this.attributeCount = 0;
				this.attributeIndex = -1;
				break;
			case ExtensionDataReader.ExtensionDataNodeType.Xml:
				break;
			default:
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("Invalid state in extension data reader.")));
			}
			this.readState = ReadState.Interactive;
			return true;
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0002B00E File Offset: 0x0002920E
		public override string Name
		{
			get
			{
				if (this.IsXmlDataNode)
				{
					return this.xmlNodeReader.Name;
				}
				return string.Empty;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0002B029 File Offset: 0x00029229
		public override bool HasValue
		{
			get
			{
				return this.IsXmlDataNode && this.xmlNodeReader.HasValue;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0002B040 File Offset: 0x00029240
		public override string BaseURI
		{
			get
			{
				if (this.IsXmlDataNode)
				{
					return this.xmlNodeReader.BaseURI;
				}
				return string.Empty;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0002B05B File Offset: 0x0002925B
		public override XmlNameTable NameTable
		{
			get
			{
				if (this.IsXmlDataNode)
				{
					return this.xmlNodeReader.NameTable;
				}
				return null;
			}
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0002B072 File Offset: 0x00029272
		public override string GetAttribute(string name)
		{
			if (this.IsXmlDataNode)
			{
				return this.xmlNodeReader.GetAttribute(name);
			}
			return null;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0002B08A File Offset: 0x0002928A
		public override string GetAttribute(int i)
		{
			if (this.IsXmlDataNode)
			{
				return this.xmlNodeReader.GetAttribute(i);
			}
			return null;
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0002B0A2 File Offset: 0x000292A2
		public override bool MoveToAttribute(string name)
		{
			return this.IsXmlDataNode && this.xmlNodeReader.MoveToAttribute(name);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0002B0BA File Offset: 0x000292BA
		public override void ResolveEntity()
		{
			if (this.IsXmlDataNode)
			{
				this.xmlNodeReader.ResolveEntity();
			}
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0002B0CF File Offset: 0x000292CF
		public override bool ReadAttributeValue()
		{
			return this.IsXmlDataNode && this.xmlNodeReader.ReadAttributeValue();
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0002B0E8 File Offset: 0x000292E8
		private void MoveNext(IDataNode dataNode)
		{
			ExtensionDataReader.ExtensionDataNodeType extensionDataNodeType = this.internalNodeType;
			if (extensionDataNodeType == ExtensionDataReader.ExtensionDataNodeType.Text || extensionDataNodeType - ExtensionDataReader.ExtensionDataNodeType.ReferencedElement <= 1)
			{
				this.internalNodeType = ExtensionDataReader.ExtensionDataNodeType.EndElement;
				return;
			}
			Type dataType = dataNode.DataType;
			if (dataType == Globals.TypeOfClassDataNode)
			{
				this.MoveNextInClass((ClassDataNode)dataNode);
				return;
			}
			if (dataType == Globals.TypeOfCollectionDataNode)
			{
				this.MoveNextInCollection((CollectionDataNode)dataNode);
				return;
			}
			if (dataType == Globals.TypeOfISerializableDataNode)
			{
				this.MoveNextInISerializable((ISerializableDataNode)dataNode);
				return;
			}
			if (dataType == Globals.TypeOfXmlDataNode)
			{
				this.MoveNextInXml((XmlDataNode)dataNode);
				return;
			}
			if (dataNode.Value != null)
			{
				this.MoveToDeserializedObject(dataNode);
				return;
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("Invalid state in extension data reader.")));
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0002B1A4 File Offset: 0x000293A4
		private void SetNextElement(IDataNode node, string name, string ns, string prefix)
		{
			this.internalNodeType = ExtensionDataReader.ExtensionDataNodeType.Element;
			this.nextElement = this.GetNextElement();
			this.nextElement.localName = name;
			this.nextElement.ns = ns;
			this.nextElement.prefix = prefix;
			if (node == null)
			{
				this.nextElement.attributeCount = 0;
				this.nextElement.AddAttribute("i", "http://www.w3.org/2001/XMLSchema-instance", "nil", "true");
				this.internalNodeType = ExtensionDataReader.ExtensionDataNodeType.NullElement;
				return;
			}
			if (!this.CheckIfNodeHandled(node))
			{
				this.AddDeserializedDataNode(node);
				node.GetData(this.nextElement);
				if (node is XmlDataNode)
				{
					this.MoveNextInXml((XmlDataNode)node);
				}
			}
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0002B250 File Offset: 0x00029450
		private void AddDeserializedDataNode(IDataNode node)
		{
			if (node.Id != Globals.NewObjectId && (node.Value == null || !node.IsFinalValue))
			{
				if (this.deserializedDataNodes == null)
				{
					this.deserializedDataNodes = new Queue<IDataNode>();
				}
				this.deserializedDataNodes.Enqueue(node);
			}
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0002B2A0 File Offset: 0x000294A0
		private bool CheckIfNodeHandled(IDataNode node)
		{
			bool flag = false;
			if (node.Id != Globals.NewObjectId)
			{
				flag = this.cache[node] != null;
				if (flag)
				{
					if (this.nextElement == null)
					{
						this.nextElement = this.GetNextElement();
					}
					this.nextElement.attributeCount = 0;
					this.nextElement.AddAttribute("z", "http://schemas.microsoft.com/2003/10/Serialization/", "Ref", node.Id.ToString(NumberFormatInfo.InvariantInfo));
					this.nextElement.AddAttribute("i", "http://www.w3.org/2001/XMLSchema-instance", "nil", "true");
					this.internalNodeType = ExtensionDataReader.ExtensionDataNodeType.ReferencedElement;
				}
				else
				{
					this.cache.Add(node, node);
				}
			}
			return flag;
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0002B358 File Offset: 0x00029558
		private void MoveNextInClass(ClassDataNode dataNode)
		{
			if (dataNode.Members != null && this.element.childElementIndex < dataNode.Members.Count)
			{
				if (this.element.childElementIndex == 0)
				{
					this.context.IncrementItemCount(-dataNode.Members.Count);
				}
				IList<ExtensionDataMember> members = dataNode.Members;
				ElementData elementData = this.element;
				int childElementIndex = elementData.childElementIndex;
				elementData.childElementIndex = childElementIndex + 1;
				ExtensionDataMember extensionDataMember = members[childElementIndex];
				this.SetNextElement(extensionDataMember.Value, extensionDataMember.Name, extensionDataMember.Namespace, ExtensionDataReader.GetPrefix(extensionDataMember.Namespace));
				return;
			}
			this.internalNodeType = ExtensionDataReader.ExtensionDataNodeType.EndElement;
			this.element.childElementIndex = 0;
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0002B408 File Offset: 0x00029608
		private void MoveNextInCollection(CollectionDataNode dataNode)
		{
			if (dataNode.Items != null && this.element.childElementIndex < dataNode.Items.Count)
			{
				if (this.element.childElementIndex == 0)
				{
					this.context.IncrementItemCount(-dataNode.Items.Count);
				}
				IList<IDataNode> items = dataNode.Items;
				ElementData elementData = this.element;
				int childElementIndex = elementData.childElementIndex;
				elementData.childElementIndex = childElementIndex + 1;
				IDataNode dataNode2 = items[childElementIndex];
				this.SetNextElement(dataNode2, dataNode.ItemName, dataNode.ItemNamespace, ExtensionDataReader.GetPrefix(dataNode.ItemNamespace));
				return;
			}
			this.internalNodeType = ExtensionDataReader.ExtensionDataNodeType.EndElement;
			this.element.childElementIndex = 0;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0002B4B0 File Offset: 0x000296B0
		private void MoveNextInISerializable(ISerializableDataNode dataNode)
		{
			if (dataNode.Members != null && this.element.childElementIndex < dataNode.Members.Count)
			{
				if (this.element.childElementIndex == 0)
				{
					this.context.IncrementItemCount(-dataNode.Members.Count);
				}
				IList<ISerializableDataMember> members = dataNode.Members;
				ElementData elementData = this.element;
				int childElementIndex = elementData.childElementIndex;
				elementData.childElementIndex = childElementIndex + 1;
				ISerializableDataMember serializableDataMember = members[childElementIndex];
				this.SetNextElement(serializableDataMember.Value, serializableDataMember.Name, string.Empty, string.Empty);
				return;
			}
			this.internalNodeType = ExtensionDataReader.ExtensionDataNodeType.EndElement;
			this.element.childElementIndex = 0;
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0002B554 File Offset: 0x00029754
		private void MoveNextInXml(XmlDataNode dataNode)
		{
			if (this.IsXmlDataNode)
			{
				this.xmlNodeReader.Read();
				if (this.xmlNodeReader.Depth == 0)
				{
					this.internalNodeType = ExtensionDataReader.ExtensionDataNodeType.EndElement;
					this.xmlNodeReader = null;
					return;
				}
			}
			else
			{
				this.internalNodeType = ExtensionDataReader.ExtensionDataNodeType.Xml;
				if (this.element == null)
				{
					this.element = this.nextElement;
				}
				else
				{
					this.PushElement();
				}
				XmlNode xmlNode = XmlObjectSerializerReadContext.CreateWrapperXmlElement(dataNode.OwnerDocument, dataNode.XmlAttributes, dataNode.XmlChildNodes, this.element.prefix, this.element.localName, this.element.ns);
				for (int i = 0; i < this.element.attributeCount; i++)
				{
					AttributeData attributeData = this.element.attributes[i];
					XmlAttribute xmlAttribute = dataNode.OwnerDocument.CreateAttribute(attributeData.prefix, attributeData.localName, attributeData.ns);
					xmlAttribute.Value = attributeData.value;
					xmlNode.Attributes.Append(xmlAttribute);
				}
				this.xmlNodeReader = new XmlNodeReader(xmlNode);
				this.xmlNodeReader.Read();
			}
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0002B664 File Offset: 0x00029864
		private void MoveToDeserializedObject(IDataNode dataNode)
		{
			Type type = dataNode.DataType;
			bool flag = true;
			if (type == Globals.TypeOfObject)
			{
				type = dataNode.Value.GetType();
				if (type == Globals.TypeOfObject)
				{
					this.internalNodeType = ExtensionDataReader.ExtensionDataNodeType.EndElement;
					return;
				}
				flag = false;
			}
			if (this.MoveToText(type, dataNode, flag))
			{
				return;
			}
			if (dataNode.IsFinalValue)
			{
				this.internalNodeType = ExtensionDataReader.ExtensionDataNodeType.EndElement;
				return;
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid data node for '{0}' type.", new object[] { DataContract.GetClrTypeFullName(type) })));
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0002B6EC File Offset: 0x000298EC
		private bool MoveToText(Type type, IDataNode dataNode, bool isTypedNode)
		{
			bool flag = true;
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
				this.value = XmlConvert.ToString(isTypedNode ? ((DataNode<bool>)dataNode).GetValue() : ((bool)dataNode.Value));
				goto IL_03E7;
			case TypeCode.Char:
				this.value = XmlConvert.ToString((int)(isTypedNode ? ((DataNode<char>)dataNode).GetValue() : ((char)dataNode.Value)));
				goto IL_03E7;
			case TypeCode.SByte:
				this.value = XmlConvert.ToString(isTypedNode ? ((DataNode<sbyte>)dataNode).GetValue() : ((sbyte)dataNode.Value));
				goto IL_03E7;
			case TypeCode.Byte:
				this.value = XmlConvert.ToString(isTypedNode ? ((DataNode<byte>)dataNode).GetValue() : ((byte)dataNode.Value));
				goto IL_03E7;
			case TypeCode.Int16:
				this.value = XmlConvert.ToString(isTypedNode ? ((DataNode<short>)dataNode).GetValue() : ((short)dataNode.Value));
				goto IL_03E7;
			case TypeCode.UInt16:
				this.value = XmlConvert.ToString(isTypedNode ? ((DataNode<ushort>)dataNode).GetValue() : ((ushort)dataNode.Value));
				goto IL_03E7;
			case TypeCode.Int32:
				this.value = XmlConvert.ToString(isTypedNode ? ((DataNode<int>)dataNode).GetValue() : ((int)dataNode.Value));
				goto IL_03E7;
			case TypeCode.UInt32:
				this.value = XmlConvert.ToString(isTypedNode ? ((DataNode<uint>)dataNode).GetValue() : ((uint)dataNode.Value));
				goto IL_03E7;
			case TypeCode.Int64:
				this.value = XmlConvert.ToString(isTypedNode ? ((DataNode<long>)dataNode).GetValue() : ((long)dataNode.Value));
				goto IL_03E7;
			case TypeCode.UInt64:
				this.value = XmlConvert.ToString(isTypedNode ? ((DataNode<ulong>)dataNode).GetValue() : ((ulong)dataNode.Value));
				goto IL_03E7;
			case TypeCode.Single:
				this.value = XmlConvert.ToString(isTypedNode ? ((DataNode<float>)dataNode).GetValue() : ((float)dataNode.Value));
				goto IL_03E7;
			case TypeCode.Double:
				this.value = XmlConvert.ToString(isTypedNode ? ((DataNode<double>)dataNode).GetValue() : ((double)dataNode.Value));
				goto IL_03E7;
			case TypeCode.Decimal:
				this.value = XmlConvert.ToString(isTypedNode ? ((DataNode<decimal>)dataNode).GetValue() : ((decimal)dataNode.Value));
				goto IL_03E7;
			case TypeCode.DateTime:
				this.value = (isTypedNode ? ((DataNode<DateTime>)dataNode).GetValue() : ((DateTime)dataNode.Value)).ToString("yyyy-MM-ddTHH:mm:ss.fffffffK", DateTimeFormatInfo.InvariantInfo);
				goto IL_03E7;
			case TypeCode.String:
				this.value = (isTypedNode ? ((DataNode<string>)dataNode).GetValue() : ((string)dataNode.Value));
				goto IL_03E7;
			}
			if (type == Globals.TypeOfByteArray)
			{
				byte[] array = (isTypedNode ? ((DataNode<byte[]>)dataNode).GetValue() : ((byte[])dataNode.Value));
				this.value = ((array == null) ? string.Empty : Convert.ToBase64String(array));
			}
			else if (type == Globals.TypeOfTimeSpan)
			{
				this.value = XmlConvert.ToString(isTypedNode ? ((DataNode<TimeSpan>)dataNode).GetValue() : ((TimeSpan)dataNode.Value));
			}
			else if (type == Globals.TypeOfGuid)
			{
				this.value = (isTypedNode ? ((DataNode<Guid>)dataNode).GetValue() : ((Guid)dataNode.Value)).ToString();
			}
			else if (type == Globals.TypeOfUri)
			{
				Uri uri = (isTypedNode ? ((DataNode<Uri>)dataNode).GetValue() : ((Uri)dataNode.Value));
				this.value = uri.GetComponents(UriComponents.SerializationInfoString, UriFormat.UriEscaped);
			}
			else
			{
				flag = false;
			}
			IL_03E7:
			if (flag)
			{
				this.internalNodeType = ExtensionDataReader.ExtensionDataNodeType.Text;
			}
			return flag;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0002BAEC File Offset: 0x00029CEC
		private void PushElement()
		{
			this.GrowElementsIfNeeded();
			ElementData[] array = this.elements;
			int num = this.depth;
			this.depth = num + 1;
			array[num] = this.element;
			if (this.nextElement == null)
			{
				this.element = this.GetNextElement();
				return;
			}
			this.element = this.nextElement;
			this.nextElement = null;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0002BB48 File Offset: 0x00029D48
		private void PopElement()
		{
			this.prefix = this.element.prefix;
			this.localName = this.element.localName;
			this.ns = this.element.ns;
			if (this.depth == 0)
			{
				return;
			}
			this.depth--;
			if (this.elements != null)
			{
				this.element = this.elements[this.depth];
			}
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0002BBBC File Offset: 0x00029DBC
		private void GrowElementsIfNeeded()
		{
			if (this.elements == null)
			{
				this.elements = new ElementData[8];
				return;
			}
			if (this.elements.Length == this.depth)
			{
				ElementData[] array = new ElementData[this.elements.Length * 2];
				Array.Copy(this.elements, 0, array, 0, this.elements.Length);
				this.elements = array;
			}
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0002BC1C File Offset: 0x00029E1C
		private ElementData GetNextElement()
		{
			int num = this.depth + 1;
			if (this.elements != null && this.elements.Length > num && this.elements[num] != null)
			{
				return this.elements[num];
			}
			return new ElementData();
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0002BC60 File Offset: 0x00029E60
		[SecuritySafeCritical]
		internal static string GetPrefix(string ns)
		{
			ns = ns ?? string.Empty;
			string text;
			if (!ExtensionDataReader.nsToPrefixTable.TryGetValue(ns, out text))
			{
				Dictionary<string, string> dictionary = ExtensionDataReader.nsToPrefixTable;
				lock (dictionary)
				{
					if (!ExtensionDataReader.nsToPrefixTable.TryGetValue(ns, out text))
					{
						text = ((ns == null || ns.Length == 0) ? string.Empty : ("p" + ExtensionDataReader.nsToPrefixTable.Count));
						ExtensionDataReader.AddPrefix(text, ns);
					}
				}
			}
			return text;
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0002BCF8 File Offset: 0x00029EF8
		[SecuritySafeCritical]
		private static void AddPrefix(string prefix, string ns)
		{
			ExtensionDataReader.nsToPrefixTable.Add(ns, prefix);
			ExtensionDataReader.prefixToNsTable.Add(prefix, ns);
		}

		// Token: 0x040003B4 RID: 948
		private Hashtable cache = new Hashtable();

		// Token: 0x040003B5 RID: 949
		private ElementData[] elements;

		// Token: 0x040003B6 RID: 950
		private ElementData element;

		// Token: 0x040003B7 RID: 951
		private ElementData nextElement;

		// Token: 0x040003B8 RID: 952
		private ReadState readState;

		// Token: 0x040003B9 RID: 953
		private ExtensionDataReader.ExtensionDataNodeType internalNodeType;

		// Token: 0x040003BA RID: 954
		private XmlNodeType nodeType;

		// Token: 0x040003BB RID: 955
		private int depth;

		// Token: 0x040003BC RID: 956
		private string localName;

		// Token: 0x040003BD RID: 957
		private string ns;

		// Token: 0x040003BE RID: 958
		private string prefix;

		// Token: 0x040003BF RID: 959
		private string value;

		// Token: 0x040003C0 RID: 960
		private int attributeCount;

		// Token: 0x040003C1 RID: 961
		private int attributeIndex;

		// Token: 0x040003C2 RID: 962
		private XmlNodeReader xmlNodeReader;

		// Token: 0x040003C3 RID: 963
		private Queue<IDataNode> deserializedDataNodes;

		// Token: 0x040003C4 RID: 964
		private XmlObjectSerializerReadContext context;

		// Token: 0x040003C5 RID: 965
		[SecurityCritical]
		private static Dictionary<string, string> nsToPrefixTable = new Dictionary<string, string>();

		// Token: 0x040003C6 RID: 966
		[SecurityCritical]
		private static Dictionary<string, string> prefixToNsTable = new Dictionary<string, string>();

		// Token: 0x02000174 RID: 372
		private enum ExtensionDataNodeType
		{
			// Token: 0x04000A19 RID: 2585
			None,
			// Token: 0x04000A1A RID: 2586
			Element,
			// Token: 0x04000A1B RID: 2587
			EndElement,
			// Token: 0x04000A1C RID: 2588
			Text,
			// Token: 0x04000A1D RID: 2589
			Xml,
			// Token: 0x04000A1E RID: 2590
			ReferencedElement,
			// Token: 0x04000A1F RID: 2591
			NullElement
		}
	}
}
