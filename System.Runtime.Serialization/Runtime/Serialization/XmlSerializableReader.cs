using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Runtime.Serialization
{
	// Token: 0x020000EC RID: 236
	internal class XmlSerializableReader : XmlReader, IXmlLineInfo, IXmlTextParser
	{
		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000E2F RID: 3631 RVA: 0x0003B89B File Offset: 0x00039A9B
		private XmlReader InnerReader
		{
			get
			{
				return this.innerReader;
			}
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0003B8A4 File Offset: 0x00039AA4
		internal void BeginRead(XmlReaderDelegator xmlReader)
		{
			if (xmlReader.NodeType != XmlNodeType.Element)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializerReadContext.CreateUnexpectedStateException(XmlNodeType.Element, xmlReader));
			}
			this.xmlReader = xmlReader;
			this.startDepth = xmlReader.Depth;
			this.innerReader = xmlReader.UnderlyingReader;
			this.isRootEmptyElement = this.InnerReader.IsEmptyElement;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0003B8F8 File Offset: 0x00039AF8
		internal void EndRead()
		{
			if (this.isRootEmptyElement)
			{
				this.xmlReader.Read();
				return;
			}
			if (this.xmlReader.IsStartElement() && this.xmlReader.Depth == this.startDepth)
			{
				this.xmlReader.Read();
			}
			while (this.xmlReader.Depth > this.startDepth)
			{
				if (!this.xmlReader.Read())
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializerReadContext.CreateUnexpectedStateException(XmlNodeType.EndElement, this.xmlReader));
				}
			}
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0003B97C File Offset: 0x00039B7C
		public override bool Read()
		{
			XmlReader xmlReader = this.InnerReader;
			return (xmlReader.Depth != this.startDepth || (xmlReader.NodeType != XmlNodeType.EndElement && (xmlReader.NodeType != XmlNodeType.Element || !xmlReader.IsEmptyElement))) && xmlReader.Read();
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0003B9C1 File Offset: 0x00039BC1
		public override void Close()
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("This method cannot be called from IXmlSerializable implementations.")));
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x0003B9D7 File Offset: 0x00039BD7
		public override XmlReaderSettings Settings
		{
			get
			{
				return this.InnerReader.Settings;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x0003B9E4 File Offset: 0x00039BE4
		public override XmlNodeType NodeType
		{
			get
			{
				return this.InnerReader.NodeType;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x0003B9F1 File Offset: 0x00039BF1
		public override string Name
		{
			get
			{
				return this.InnerReader.Name;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000E37 RID: 3639 RVA: 0x0003B9FE File Offset: 0x00039BFE
		public override string LocalName
		{
			get
			{
				return this.InnerReader.LocalName;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x0003BA0B File Offset: 0x00039C0B
		public override string NamespaceURI
		{
			get
			{
				return this.InnerReader.NamespaceURI;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000E39 RID: 3641 RVA: 0x0003BA18 File Offset: 0x00039C18
		public override string Prefix
		{
			get
			{
				return this.InnerReader.Prefix;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x0003BA25 File Offset: 0x00039C25
		public override bool HasValue
		{
			get
			{
				return this.InnerReader.HasValue;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x0003BA32 File Offset: 0x00039C32
		public override string Value
		{
			get
			{
				return this.InnerReader.Value;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x0003BA3F File Offset: 0x00039C3F
		public override int Depth
		{
			get
			{
				return this.InnerReader.Depth;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000E3D RID: 3645 RVA: 0x0003BA4C File Offset: 0x00039C4C
		public override string BaseURI
		{
			get
			{
				return this.InnerReader.BaseURI;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x0003BA59 File Offset: 0x00039C59
		public override bool IsEmptyElement
		{
			get
			{
				return this.InnerReader.IsEmptyElement;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000E3F RID: 3647 RVA: 0x0003BA66 File Offset: 0x00039C66
		public override bool IsDefault
		{
			get
			{
				return this.InnerReader.IsDefault;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x0003BA73 File Offset: 0x00039C73
		public override char QuoteChar
		{
			get
			{
				return this.InnerReader.QuoteChar;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x0003BA80 File Offset: 0x00039C80
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.InnerReader.XmlSpace;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x0003BA8D File Offset: 0x00039C8D
		public override string XmlLang
		{
			get
			{
				return this.InnerReader.XmlLang;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x0003BA9A File Offset: 0x00039C9A
		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return this.InnerReader.SchemaInfo;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x0003BAA7 File Offset: 0x00039CA7
		public override Type ValueType
		{
			get
			{
				return this.InnerReader.ValueType;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x0003BAB4 File Offset: 0x00039CB4
		public override int AttributeCount
		{
			get
			{
				return this.InnerReader.AttributeCount;
			}
		}

		// Token: 0x1700030A RID: 778
		public override string this[int i]
		{
			get
			{
				return this.InnerReader[i];
			}
		}

		// Token: 0x1700030B RID: 779
		public override string this[string name]
		{
			get
			{
				return this.InnerReader[name];
			}
		}

		// Token: 0x1700030C RID: 780
		public override string this[string name, string namespaceURI]
		{
			get
			{
				return this.InnerReader[name, namespaceURI];
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x0003BAEC File Offset: 0x00039CEC
		public override bool EOF
		{
			get
			{
				return this.InnerReader.EOF;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x0003BAF9 File Offset: 0x00039CF9
		public override ReadState ReadState
		{
			get
			{
				return this.InnerReader.ReadState;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x0003BB06 File Offset: 0x00039D06
		public override XmlNameTable NameTable
		{
			get
			{
				return this.InnerReader.NameTable;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x0003BB13 File Offset: 0x00039D13
		public override bool CanResolveEntity
		{
			get
			{
				return this.InnerReader.CanResolveEntity;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x0003BB20 File Offset: 0x00039D20
		public override bool CanReadBinaryContent
		{
			get
			{
				return this.InnerReader.CanReadBinaryContent;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x0003BB2D File Offset: 0x00039D2D
		public override bool CanReadValueChunk
		{
			get
			{
				return this.InnerReader.CanReadValueChunk;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x0003BB3A File Offset: 0x00039D3A
		public override bool HasAttributes
		{
			get
			{
				return this.InnerReader.HasAttributes;
			}
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0003BB47 File Offset: 0x00039D47
		public override string GetAttribute(string name)
		{
			return this.InnerReader.GetAttribute(name);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0003BB55 File Offset: 0x00039D55
		public override string GetAttribute(string name, string namespaceURI)
		{
			return this.InnerReader.GetAttribute(name, namespaceURI);
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0003BB64 File Offset: 0x00039D64
		public override string GetAttribute(int i)
		{
			return this.InnerReader.GetAttribute(i);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0003BB72 File Offset: 0x00039D72
		public override bool MoveToAttribute(string name)
		{
			return this.InnerReader.MoveToAttribute(name);
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0003BB80 File Offset: 0x00039D80
		public override bool MoveToAttribute(string name, string ns)
		{
			return this.InnerReader.MoveToAttribute(name, ns);
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x0003BB8F File Offset: 0x00039D8F
		public override void MoveToAttribute(int i)
		{
			this.InnerReader.MoveToAttribute(i);
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0003BB9D File Offset: 0x00039D9D
		public override bool MoveToFirstAttribute()
		{
			return this.InnerReader.MoveToFirstAttribute();
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0003BBAA File Offset: 0x00039DAA
		public override bool MoveToNextAttribute()
		{
			return this.InnerReader.MoveToNextAttribute();
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0003BBB7 File Offset: 0x00039DB7
		public override bool MoveToElement()
		{
			return this.InnerReader.MoveToElement();
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0003BBC4 File Offset: 0x00039DC4
		public override string LookupNamespace(string prefix)
		{
			return this.InnerReader.LookupNamespace(prefix);
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0003BBD2 File Offset: 0x00039DD2
		public override bool ReadAttributeValue()
		{
			return this.InnerReader.ReadAttributeValue();
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0003BBDF File Offset: 0x00039DDF
		public override void ResolveEntity()
		{
			this.InnerReader.ResolveEntity();
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0003BBEC File Offset: 0x00039DEC
		public override bool IsStartElement()
		{
			return this.InnerReader.IsStartElement();
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0003BBF9 File Offset: 0x00039DF9
		public override bool IsStartElement(string name)
		{
			return this.InnerReader.IsStartElement(name);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0003BC07 File Offset: 0x00039E07
		public override bool IsStartElement(string localname, string ns)
		{
			return this.InnerReader.IsStartElement(localname, ns);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0003BC16 File Offset: 0x00039E16
		public override XmlNodeType MoveToContent()
		{
			return this.InnerReader.MoveToContent();
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0003BC23 File Offset: 0x00039E23
		public override object ReadContentAsObject()
		{
			return this.InnerReader.ReadContentAsObject();
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0003BC30 File Offset: 0x00039E30
		public override bool ReadContentAsBoolean()
		{
			return this.InnerReader.ReadContentAsBoolean();
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0003BC3D File Offset: 0x00039E3D
		public override DateTime ReadContentAsDateTime()
		{
			return this.InnerReader.ReadContentAsDateTime();
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x0003BC4A File Offset: 0x00039E4A
		public override double ReadContentAsDouble()
		{
			return this.InnerReader.ReadContentAsDouble();
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0003BC57 File Offset: 0x00039E57
		public override int ReadContentAsInt()
		{
			return this.InnerReader.ReadContentAsInt();
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0003BC64 File Offset: 0x00039E64
		public override long ReadContentAsLong()
		{
			return this.InnerReader.ReadContentAsLong();
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0003BC71 File Offset: 0x00039E71
		public override string ReadContentAsString()
		{
			return this.InnerReader.ReadContentAsString();
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0003BC7E File Offset: 0x00039E7E
		public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			return this.InnerReader.ReadContentAs(returnType, namespaceResolver);
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0003BC8D File Offset: 0x00039E8D
		public override int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			return this.InnerReader.ReadContentAsBase64(buffer, index, count);
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0003BC9D File Offset: 0x00039E9D
		public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			return this.InnerReader.ReadContentAsBinHex(buffer, index, count);
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0003BCAD File Offset: 0x00039EAD
		public override int ReadValueChunk(char[] buffer, int index, int count)
		{
			return this.InnerReader.ReadValueChunk(buffer, index, count);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0003BCBD File Offset: 0x00039EBD
		public override string ReadString()
		{
			return this.InnerReader.ReadString();
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x0003BCCC File Offset: 0x00039ECC
		// (set) Token: 0x06000E6D RID: 3693 RVA: 0x0003BCFC File Offset: 0x00039EFC
		bool IXmlTextParser.Normalized
		{
			get
			{
				IXmlTextParser xmlTextParser = this.InnerReader as IXmlTextParser;
				if (xmlTextParser != null)
				{
					return xmlTextParser.Normalized;
				}
				return this.xmlReader.Normalized;
			}
			set
			{
				IXmlTextParser xmlTextParser = this.InnerReader as IXmlTextParser;
				if (xmlTextParser == null)
				{
					this.xmlReader.Normalized = value;
					return;
				}
				xmlTextParser.Normalized = value;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x0003BD2C File Offset: 0x00039F2C
		// (set) Token: 0x06000E6F RID: 3695 RVA: 0x0003BD5C File Offset: 0x00039F5C
		WhitespaceHandling IXmlTextParser.WhitespaceHandling
		{
			get
			{
				IXmlTextParser xmlTextParser = this.InnerReader as IXmlTextParser;
				if (xmlTextParser != null)
				{
					return xmlTextParser.WhitespaceHandling;
				}
				return this.xmlReader.WhitespaceHandling;
			}
			set
			{
				IXmlTextParser xmlTextParser = this.InnerReader as IXmlTextParser;
				if (xmlTextParser == null)
				{
					this.xmlReader.WhitespaceHandling = value;
					return;
				}
				xmlTextParser.WhitespaceHandling = value;
			}
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x0003BD8C File Offset: 0x00039F8C
		bool IXmlLineInfo.HasLineInfo()
		{
			IXmlLineInfo xmlLineInfo = this.InnerReader as IXmlLineInfo;
			if (xmlLineInfo != null)
			{
				return xmlLineInfo.HasLineInfo();
			}
			return this.xmlReader.HasLineInfo();
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0003BDBC File Offset: 0x00039FBC
		int IXmlLineInfo.LineNumber
		{
			get
			{
				IXmlLineInfo xmlLineInfo = this.InnerReader as IXmlLineInfo;
				if (xmlLineInfo != null)
				{
					return xmlLineInfo.LineNumber;
				}
				return this.xmlReader.LineNumber;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0003BDEC File Offset: 0x00039FEC
		int IXmlLineInfo.LinePosition
		{
			get
			{
				IXmlLineInfo xmlLineInfo = this.InnerReader as IXmlLineInfo;
				if (xmlLineInfo != null)
				{
					return xmlLineInfo.LinePosition;
				}
				return this.xmlReader.LinePosition;
			}
		}

		// Token: 0x0400057F RID: 1407
		private XmlReaderDelegator xmlReader;

		// Token: 0x04000580 RID: 1408
		private int startDepth;

		// Token: 0x04000581 RID: 1409
		private bool isRootEmptyElement;

		// Token: 0x04000582 RID: 1410
		private XmlReader innerReader;
	}
}
