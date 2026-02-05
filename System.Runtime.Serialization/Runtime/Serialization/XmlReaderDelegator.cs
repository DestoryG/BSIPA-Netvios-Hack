using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace System.Runtime.Serialization
{
	// Token: 0x020000EB RID: 235
	internal class XmlReaderDelegator
	{
		// Token: 0x06000DC3 RID: 3523 RVA: 0x0003A346 File Offset: 0x00038546
		public XmlReaderDelegator(XmlReader reader)
		{
			XmlObjectSerializer.CheckNull(reader, "reader");
			this.reader = reader;
			this.dictionaryReader = reader as XmlDictionaryReader;
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x0003A36C File Offset: 0x0003856C
		internal XmlReader UnderlyingReader
		{
			get
			{
				return this.reader;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x0003A374 File Offset: 0x00038574
		internal ExtensionDataReader UnderlyingExtensionDataReader
		{
			get
			{
				return this.reader as ExtensionDataReader;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x0003A381 File Offset: 0x00038581
		internal int AttributeCount
		{
			get
			{
				if (!this.isEndOfEmptyElement)
				{
					return this.reader.AttributeCount;
				}
				return 0;
			}
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0003A398 File Offset: 0x00038598
		internal string GetAttribute(string name)
		{
			if (!this.isEndOfEmptyElement)
			{
				return this.reader.GetAttribute(name);
			}
			return null;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0003A3B0 File Offset: 0x000385B0
		internal string GetAttribute(string name, string namespaceUri)
		{
			if (!this.isEndOfEmptyElement)
			{
				return this.reader.GetAttribute(name, namespaceUri);
			}
			return null;
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0003A3C9 File Offset: 0x000385C9
		internal string GetAttribute(int i)
		{
			if (this.isEndOfEmptyElement)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("i", SR.GetString("Only Element nodes have attributes.")));
			}
			return this.reader.GetAttribute(i);
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x0003A3F9 File Offset: 0x000385F9
		internal bool IsEmptyElement
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0003A3FC File Offset: 0x000385FC
		internal bool IsNamespaceURI(string ns)
		{
			if (this.dictionaryReader == null)
			{
				return ns == this.reader.NamespaceURI;
			}
			return this.dictionaryReader.IsNamespaceUri(ns);
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0003A424 File Offset: 0x00038624
		internal bool IsLocalName(string localName)
		{
			if (this.dictionaryReader == null)
			{
				return localName == this.reader.LocalName;
			}
			return this.dictionaryReader.IsLocalName(localName);
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0003A44C File Offset: 0x0003864C
		internal bool IsNamespaceUri(XmlDictionaryString ns)
		{
			if (this.dictionaryReader == null)
			{
				return ns.Value == this.reader.NamespaceURI;
			}
			return this.dictionaryReader.IsNamespaceUri(ns);
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0003A479 File Offset: 0x00038679
		internal bool IsLocalName(XmlDictionaryString localName)
		{
			if (this.dictionaryReader == null)
			{
				return localName.Value == this.reader.LocalName;
			}
			return this.dictionaryReader.IsLocalName(localName);
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0003A4A8 File Offset: 0x000386A8
		internal int IndexOfLocalName(XmlDictionaryString[] localNames, XmlDictionaryString ns)
		{
			if (this.dictionaryReader != null)
			{
				return this.dictionaryReader.IndexOfLocalName(localNames, ns);
			}
			if (this.reader.NamespaceURI == ns.Value)
			{
				string localName = this.LocalName;
				for (int i = 0; i < localNames.Length; i++)
				{
					if (localName == localNames[i].Value)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0003A50B File Offset: 0x0003870B
		public bool IsStartElement()
		{
			return !this.isEndOfEmptyElement && this.reader.IsStartElement();
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0003A522 File Offset: 0x00038722
		internal bool IsStartElement(string localname, string ns)
		{
			return !this.isEndOfEmptyElement && this.reader.IsStartElement(localname, ns);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0003A53C File Offset: 0x0003873C
		public bool IsStartElement(XmlDictionaryString localname, XmlDictionaryString ns)
		{
			if (this.dictionaryReader == null)
			{
				return !this.isEndOfEmptyElement && this.reader.IsStartElement(localname.Value, ns.Value);
			}
			return !this.isEndOfEmptyElement && this.dictionaryReader.IsStartElement(localname, ns);
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0003A58A File Offset: 0x0003878A
		internal bool MoveToAttribute(string name)
		{
			return !this.isEndOfEmptyElement && this.reader.MoveToAttribute(name);
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0003A5A2 File Offset: 0x000387A2
		internal bool MoveToAttribute(string name, string ns)
		{
			return !this.isEndOfEmptyElement && this.reader.MoveToAttribute(name, ns);
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0003A5BB File Offset: 0x000387BB
		internal void MoveToAttribute(int i)
		{
			if (this.isEndOfEmptyElement)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("i", SR.GetString("Only Element nodes have attributes.")));
			}
			this.reader.MoveToAttribute(i);
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0003A5EB File Offset: 0x000387EB
		internal bool MoveToElement()
		{
			return !this.isEndOfEmptyElement && this.reader.MoveToElement();
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0003A602 File Offset: 0x00038802
		internal bool MoveToFirstAttribute()
		{
			return !this.isEndOfEmptyElement && this.reader.MoveToFirstAttribute();
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0003A619 File Offset: 0x00038819
		internal bool MoveToNextAttribute()
		{
			return !this.isEndOfEmptyElement && this.reader.MoveToNextAttribute();
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x0003A630 File Offset: 0x00038830
		public XmlNodeType NodeType
		{
			get
			{
				if (!this.isEndOfEmptyElement)
				{
					return this.reader.NodeType;
				}
				return XmlNodeType.EndElement;
			}
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0003A648 File Offset: 0x00038848
		internal bool Read()
		{
			this.reader.MoveToElement();
			if (!this.reader.IsEmptyElement)
			{
				return this.reader.Read();
			}
			if (this.isEndOfEmptyElement)
			{
				this.isEndOfEmptyElement = false;
				return this.reader.Read();
			}
			this.isEndOfEmptyElement = true;
			return true;
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0003A69D File Offset: 0x0003889D
		internal XmlNodeType MoveToContent()
		{
			if (this.isEndOfEmptyElement)
			{
				return XmlNodeType.EndElement;
			}
			return this.reader.MoveToContent();
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0003A6B5 File Offset: 0x000388B5
		internal bool ReadAttributeValue()
		{
			return !this.isEndOfEmptyElement && this.reader.ReadAttributeValue();
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0003A6CC File Offset: 0x000388CC
		public void ReadEndElement()
		{
			if (this.isEndOfEmptyElement)
			{
				this.Read();
				return;
			}
			this.reader.ReadEndElement();
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0003A6E9 File Offset: 0x000388E9
		private Exception CreateInvalidPrimitiveTypeException(Type type)
		{
			return new InvalidDataContractException(SR.GetString(type.IsInterface ? "Interface type '{0}' cannot be created. Consider replacing with a non-interface serializable type." : "Type '{0}' is not a valid serializable type.", new object[] { DataContract.GetClrTypeFullName(type) }));
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0003A718 File Offset: 0x00038918
		public object ReadElementContentAsAnyType(Type valueType)
		{
			this.Read();
			object obj = this.ReadContentAsAnyType(valueType);
			this.ReadEndElement();
			return obj;
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0003A730 File Offset: 0x00038930
		internal object ReadContentAsAnyType(Type valueType)
		{
			switch (Type.GetTypeCode(valueType))
			{
			case TypeCode.Boolean:
				return this.ReadContentAsBoolean();
			case TypeCode.Char:
				return this.ReadContentAsChar();
			case TypeCode.SByte:
				return this.ReadContentAsSignedByte();
			case TypeCode.Byte:
				return this.ReadContentAsUnsignedByte();
			case TypeCode.Int16:
				return this.ReadContentAsShort();
			case TypeCode.UInt16:
				return this.ReadContentAsUnsignedShort();
			case TypeCode.Int32:
				return this.ReadContentAsInt();
			case TypeCode.UInt32:
				return this.ReadContentAsUnsignedInt();
			case TypeCode.Int64:
				return this.ReadContentAsLong();
			case TypeCode.UInt64:
				return this.ReadContentAsUnsignedLong();
			case TypeCode.Single:
				return this.ReadContentAsSingle();
			case TypeCode.Double:
				return this.ReadContentAsDouble();
			case TypeCode.Decimal:
				return this.ReadContentAsDecimal();
			case TypeCode.DateTime:
				return this.ReadContentAsDateTime();
			case TypeCode.String:
				return this.ReadContentAsString();
			}
			if (valueType == Globals.TypeOfByteArray)
			{
				return this.ReadContentAsBase64();
			}
			if (valueType == Globals.TypeOfObject)
			{
				return new object();
			}
			if (valueType == Globals.TypeOfTimeSpan)
			{
				return this.ReadContentAsTimeSpan();
			}
			if (valueType == Globals.TypeOfGuid)
			{
				return this.ReadContentAsGuid();
			}
			if (valueType == Globals.TypeOfUri)
			{
				return this.ReadContentAsUri();
			}
			if (valueType == Globals.TypeOfXmlQualifiedName)
			{
				return this.ReadContentAsQName();
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(this.CreateInvalidPrimitiveTypeException(valueType));
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0003A8D8 File Offset: 0x00038AD8
		internal IDataNode ReadExtensionData(Type valueType)
		{
			switch (Type.GetTypeCode(valueType))
			{
			case TypeCode.Boolean:
				return new DataNode<bool>(this.ReadContentAsBoolean());
			case TypeCode.Char:
				return new DataNode<char>(this.ReadContentAsChar());
			case TypeCode.SByte:
				return new DataNode<sbyte>(this.ReadContentAsSignedByte());
			case TypeCode.Byte:
				return new DataNode<byte>(this.ReadContentAsUnsignedByte());
			case TypeCode.Int16:
				return new DataNode<short>(this.ReadContentAsShort());
			case TypeCode.UInt16:
				return new DataNode<ushort>(this.ReadContentAsUnsignedShort());
			case TypeCode.Int32:
				return new DataNode<int>(this.ReadContentAsInt());
			case TypeCode.UInt32:
				return new DataNode<uint>(this.ReadContentAsUnsignedInt());
			case TypeCode.Int64:
				return new DataNode<long>(this.ReadContentAsLong());
			case TypeCode.UInt64:
				return new DataNode<ulong>(this.ReadContentAsUnsignedLong());
			case TypeCode.Single:
				return new DataNode<float>(this.ReadContentAsSingle());
			case TypeCode.Double:
				return new DataNode<double>(this.ReadContentAsDouble());
			case TypeCode.Decimal:
				return new DataNode<decimal>(this.ReadContentAsDecimal());
			case TypeCode.DateTime:
				return new DataNode<DateTime>(this.ReadContentAsDateTime());
			case TypeCode.String:
				return new DataNode<string>(this.ReadContentAsString());
			}
			if (valueType == Globals.TypeOfByteArray)
			{
				return new DataNode<byte[]>(this.ReadContentAsBase64());
			}
			if (valueType == Globals.TypeOfObject)
			{
				return new DataNode<object>(new object());
			}
			if (valueType == Globals.TypeOfTimeSpan)
			{
				return new DataNode<TimeSpan>(this.ReadContentAsTimeSpan());
			}
			if (valueType == Globals.TypeOfGuid)
			{
				return new DataNode<Guid>(this.ReadContentAsGuid());
			}
			if (valueType == Globals.TypeOfUri)
			{
				return new DataNode<Uri>(this.ReadContentAsUri());
			}
			if (valueType == Globals.TypeOfXmlQualifiedName)
			{
				return new DataNode<XmlQualifiedName>(this.ReadContentAsQName());
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(this.CreateInvalidPrimitiveTypeException(valueType));
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0003AA98 File Offset: 0x00038C98
		private void ThrowConversionException(string value, string type)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(XmlObjectSerializer.TryAddLineInfo(this, SR.GetString("The value '{0}' cannot be parsed as the type '{1}'.", new object[] { value, type }))));
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0003AAC2 File Offset: 0x00038CC2
		private void ThrowNotAtElement()
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Start element expected. Found {0}.", new object[] { "EndElement" })));
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0003AAE6 File Offset: 0x00038CE6
		internal virtual char ReadElementContentAsChar()
		{
			return this.ToChar(this.ReadElementContentAsInt());
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0003AAF4 File Offset: 0x00038CF4
		internal virtual char ReadContentAsChar()
		{
			return this.ToChar(this.ReadContentAsInt());
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0003AB02 File Offset: 0x00038D02
		private char ToChar(int value)
		{
			if (value < 0 || value > 65535)
			{
				this.ThrowConversionException(value.ToString(NumberFormatInfo.CurrentInfo), "Char");
			}
			return (char)value;
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0003AB29 File Offset: 0x00038D29
		public string ReadElementContentAsString()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowNotAtElement();
			}
			return this.reader.ReadElementContentAsString();
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0003AB44 File Offset: 0x00038D44
		internal string ReadContentAsString()
		{
			if (!this.isEndOfEmptyElement)
			{
				return this.reader.ReadContentAsString();
			}
			return string.Empty;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0003AB5F File Offset: 0x00038D5F
		public bool ReadElementContentAsBoolean()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowNotAtElement();
			}
			return this.reader.ReadElementContentAsBoolean();
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0003AB7A File Offset: 0x00038D7A
		internal bool ReadContentAsBoolean()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowConversionException(string.Empty, "Boolean");
			}
			return this.reader.ReadContentAsBoolean();
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0003AB9F File Offset: 0x00038D9F
		public float ReadElementContentAsFloat()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowNotAtElement();
			}
			return this.reader.ReadElementContentAsFloat();
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0003ABBA File Offset: 0x00038DBA
		internal float ReadContentAsSingle()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowConversionException(string.Empty, "Float");
			}
			return this.reader.ReadContentAsFloat();
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0003ABDF File Offset: 0x00038DDF
		public double ReadElementContentAsDouble()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowNotAtElement();
			}
			return this.reader.ReadElementContentAsDouble();
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0003ABFA File Offset: 0x00038DFA
		internal double ReadContentAsDouble()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowConversionException(string.Empty, "Double");
			}
			return this.reader.ReadContentAsDouble();
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0003AC1F File Offset: 0x00038E1F
		public decimal ReadElementContentAsDecimal()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowNotAtElement();
			}
			return this.reader.ReadElementContentAsDecimal();
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0003AC3A File Offset: 0x00038E3A
		internal decimal ReadContentAsDecimal()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowConversionException(string.Empty, "Decimal");
			}
			return this.reader.ReadContentAsDecimal();
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0003AC5F File Offset: 0x00038E5F
		internal virtual byte[] ReadElementContentAsBase64()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowNotAtElement();
			}
			if (this.dictionaryReader == null)
			{
				return this.ReadContentAsBase64(this.reader.ReadElementContentAsString());
			}
			return this.dictionaryReader.ReadElementContentAsBase64();
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0003AC94 File Offset: 0x00038E94
		internal virtual byte[] ReadContentAsBase64()
		{
			if (this.isEndOfEmptyElement)
			{
				return new byte[0];
			}
			if (this.dictionaryReader == null)
			{
				return this.ReadContentAsBase64(this.reader.ReadContentAsString());
			}
			return this.dictionaryReader.ReadContentAsBase64();
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0003ACCC File Offset: 0x00038ECC
		internal byte[] ReadContentAsBase64(string str)
		{
			if (str == null)
			{
				return null;
			}
			str = str.Trim();
			if (str.Length == 0)
			{
				return new byte[0];
			}
			byte[] array;
			try
			{
				array = Convert.FromBase64String(str);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(str, "byte[]", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(str, "byte[]", ex2));
			}
			return array;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0003AD44 File Offset: 0x00038F44
		internal virtual DateTime ReadElementContentAsDateTime()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowNotAtElement();
			}
			return this.reader.ReadElementContentAsDateTime();
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0003AD5F File Offset: 0x00038F5F
		internal virtual DateTime ReadContentAsDateTime()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowConversionException(string.Empty, "DateTime");
			}
			return this.reader.ReadContentAsDateTime();
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0003AD84 File Offset: 0x00038F84
		public int ReadElementContentAsInt()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowNotAtElement();
			}
			return this.reader.ReadElementContentAsInt();
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0003AD9F File Offset: 0x00038F9F
		internal int ReadContentAsInt()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowConversionException(string.Empty, "Int32");
			}
			return this.reader.ReadContentAsInt();
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0003ADC4 File Offset: 0x00038FC4
		public long ReadElementContentAsLong()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowNotAtElement();
			}
			return this.reader.ReadElementContentAsLong();
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0003ADDF File Offset: 0x00038FDF
		internal long ReadContentAsLong()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowConversionException(string.Empty, "Int64");
			}
			return this.reader.ReadContentAsLong();
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0003AE04 File Offset: 0x00039004
		public short ReadElementContentAsShort()
		{
			return this.ToShort(this.ReadElementContentAsInt());
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0003AE12 File Offset: 0x00039012
		internal short ReadContentAsShort()
		{
			return this.ToShort(this.ReadContentAsInt());
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0003AE20 File Offset: 0x00039020
		private short ToShort(int value)
		{
			if (value < -32768 || value > 32767)
			{
				this.ThrowConversionException(value.ToString(NumberFormatInfo.CurrentInfo), "Int16");
			}
			return (short)value;
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0003AE4B File Offset: 0x0003904B
		public byte ReadElementContentAsUnsignedByte()
		{
			return this.ToByte(this.ReadElementContentAsInt());
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0003AE59 File Offset: 0x00039059
		internal byte ReadContentAsUnsignedByte()
		{
			return this.ToByte(this.ReadContentAsInt());
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0003AE67 File Offset: 0x00039067
		private byte ToByte(int value)
		{
			if (value < 0 || value > 255)
			{
				this.ThrowConversionException(value.ToString(NumberFormatInfo.CurrentInfo), "Byte");
			}
			return (byte)value;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0003AE8E File Offset: 0x0003908E
		public sbyte ReadElementContentAsSignedByte()
		{
			return this.ToSByte(this.ReadElementContentAsInt());
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0003AE9C File Offset: 0x0003909C
		internal sbyte ReadContentAsSignedByte()
		{
			return this.ToSByte(this.ReadContentAsInt());
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0003AEAA File Offset: 0x000390AA
		private sbyte ToSByte(int value)
		{
			if (value < -128 || value > 127)
			{
				this.ThrowConversionException(value.ToString(NumberFormatInfo.CurrentInfo), "SByte");
			}
			return (sbyte)value;
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0003AECF File Offset: 0x000390CF
		public uint ReadElementContentAsUnsignedInt()
		{
			return this.ToUInt32(this.ReadElementContentAsLong());
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0003AEDD File Offset: 0x000390DD
		internal uint ReadContentAsUnsignedInt()
		{
			return this.ToUInt32(this.ReadContentAsLong());
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0003AEEB File Offset: 0x000390EB
		private uint ToUInt32(long value)
		{
			if (value < 0L || value > (long)((ulong)(-1)))
			{
				this.ThrowConversionException(value.ToString(NumberFormatInfo.CurrentInfo), "UInt32");
			}
			return (uint)value;
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0003AF10 File Offset: 0x00039110
		internal virtual ulong ReadElementContentAsUnsignedLong()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowNotAtElement();
			}
			string text = this.reader.ReadElementContentAsString();
			if (text == null || text.Length == 0)
			{
				this.ThrowConversionException(string.Empty, "UInt64");
			}
			return XmlConverter.ToUInt64(text);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0003AF58 File Offset: 0x00039158
		internal virtual ulong ReadContentAsUnsignedLong()
		{
			string text = this.reader.ReadContentAsString();
			if (text == null || text.Length == 0)
			{
				this.ThrowConversionException(string.Empty, "UInt64");
			}
			return XmlConverter.ToUInt64(text);
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0003AF92 File Offset: 0x00039192
		public ushort ReadElementContentAsUnsignedShort()
		{
			return this.ToUInt16(this.ReadElementContentAsInt());
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0003AFA0 File Offset: 0x000391A0
		internal ushort ReadContentAsUnsignedShort()
		{
			return this.ToUInt16(this.ReadContentAsInt());
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0003AFAE File Offset: 0x000391AE
		private ushort ToUInt16(int value)
		{
			if (value < 0 || value > 65535)
			{
				this.ThrowConversionException(value.ToString(NumberFormatInfo.CurrentInfo), "UInt16");
			}
			return (ushort)value;
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0003AFD5 File Offset: 0x000391D5
		public TimeSpan ReadElementContentAsTimeSpan()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowNotAtElement();
			}
			return XmlConverter.ToTimeSpan(this.reader.ReadElementContentAsString());
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0003AFF5 File Offset: 0x000391F5
		internal TimeSpan ReadContentAsTimeSpan()
		{
			return XmlConverter.ToTimeSpan(this.reader.ReadContentAsString());
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0003B008 File Offset: 0x00039208
		public Guid ReadElementContentAsGuid()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowNotAtElement();
			}
			string text = this.reader.ReadElementContentAsString();
			Guid guid;
			try
			{
				guid = Guid.Parse(text);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "Guid", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "Guid", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "Guid", ex3));
			}
			return guid;
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0003B09C File Offset: 0x0003929C
		internal Guid ReadContentAsGuid()
		{
			string text = this.reader.ReadContentAsString();
			Guid guid;
			try
			{
				guid = Guid.Parse(text);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "Guid", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "Guid", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "Guid", ex3));
			}
			return guid;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0003B124 File Offset: 0x00039324
		public Uri ReadElementContentAsUri()
		{
			if (this.isEndOfEmptyElement)
			{
				this.ThrowNotAtElement();
			}
			string text = this.ReadElementContentAsString();
			Uri uri;
			try
			{
				uri = new Uri(text, UriKind.RelativeOrAbsolute);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "Uri", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "Uri", ex2));
			}
			return uri;
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0003B194 File Offset: 0x00039394
		internal Uri ReadContentAsUri()
		{
			string text = this.ReadContentAsString();
			Uri uri;
			try
			{
				uri = new Uri(text, UriKind.RelativeOrAbsolute);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "Uri", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "Uri", ex2));
			}
			return uri;
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0003B1F8 File Offset: 0x000393F8
		public XmlQualifiedName ReadElementContentAsQName()
		{
			this.Read();
			XmlQualifiedName xmlQualifiedName = this.ReadContentAsQName();
			this.ReadEndElement();
			return xmlQualifiedName;
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0003B20D File Offset: 0x0003940D
		internal virtual XmlQualifiedName ReadContentAsQName()
		{
			return this.ParseQualifiedName(this.ReadContentAsString());
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0003B21C File Offset: 0x0003941C
		private XmlQualifiedName ParseQualifiedName(string str)
		{
			string empty;
			string text;
			if (str == null || str.Length == 0)
			{
				text = (empty = string.Empty);
			}
			else
			{
				string text2;
				XmlObjectSerializerReadContext.ParseQualifiedName(str, this, out empty, out text, out text2);
			}
			return new XmlQualifiedName(empty, text);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0003B252 File Offset: 0x00039452
		private void CheckExpectedArrayLength(XmlObjectSerializerReadContext context, int arrayLength)
		{
			context.IncrementItemCount(arrayLength);
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0003B25B File Offset: 0x0003945B
		protected int GetArrayLengthQuota(XmlObjectSerializerReadContext context)
		{
			if (this.dictionaryReader.Quotas == null)
			{
				return context.RemainingItemCount;
			}
			return Math.Min(context.RemainingItemCount, this.dictionaryReader.Quotas.MaxArrayLength);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0003B28C File Offset: 0x0003948C
		private void CheckActualArrayLength(int expectedLength, int actualLength, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			if (expectedLength != actualLength)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Array length '{0}' provided by Size attribute is not equal to the number of array elements '{1}' from namespace '{2}' found.", new object[] { expectedLength, itemName.Value, itemNamespace.Value })));
			}
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0003B2CC File Offset: 0x000394CC
		internal bool TryReadBooleanArray(XmlObjectSerializerReadContext context, XmlDictionaryString itemName, XmlDictionaryString itemNamespace, int arrayLength, out bool[] array)
		{
			if (this.dictionaryReader == null)
			{
				array = null;
				return false;
			}
			if (arrayLength != -1)
			{
				this.CheckExpectedArrayLength(context, arrayLength);
				array = new bool[arrayLength];
				int num = 0;
				int num2;
				while ((num2 = this.dictionaryReader.ReadArray(itemName, itemNamespace, array, num, arrayLength - num)) > 0)
				{
					num += num2;
				}
				this.CheckActualArrayLength(arrayLength, num, itemName, itemNamespace);
			}
			else
			{
				array = BooleanArrayHelperWithDictionaryString.Instance.ReadArray(this.dictionaryReader, itemName, itemNamespace, this.GetArrayLengthQuota(context));
				context.IncrementItemCount(array.Length);
			}
			return true;
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0003B358 File Offset: 0x00039558
		internal bool TryReadDateTimeArray(XmlObjectSerializerReadContext context, XmlDictionaryString itemName, XmlDictionaryString itemNamespace, int arrayLength, out DateTime[] array)
		{
			if (this.dictionaryReader == null)
			{
				array = null;
				return false;
			}
			if (arrayLength != -1)
			{
				this.CheckExpectedArrayLength(context, arrayLength);
				array = new DateTime[arrayLength];
				int num = 0;
				int num2;
				while ((num2 = this.dictionaryReader.ReadArray(itemName, itemNamespace, array, num, arrayLength - num)) > 0)
				{
					num += num2;
				}
				this.CheckActualArrayLength(arrayLength, num, itemName, itemNamespace);
			}
			else
			{
				array = DateTimeArrayHelperWithDictionaryString.Instance.ReadArray(this.dictionaryReader, itemName, itemNamespace, this.GetArrayLengthQuota(context));
				context.IncrementItemCount(array.Length);
			}
			return true;
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0003B3E4 File Offset: 0x000395E4
		internal bool TryReadDecimalArray(XmlObjectSerializerReadContext context, XmlDictionaryString itemName, XmlDictionaryString itemNamespace, int arrayLength, out decimal[] array)
		{
			if (this.dictionaryReader == null)
			{
				array = null;
				return false;
			}
			if (arrayLength != -1)
			{
				this.CheckExpectedArrayLength(context, arrayLength);
				array = new decimal[arrayLength];
				int num = 0;
				int num2;
				while ((num2 = this.dictionaryReader.ReadArray(itemName, itemNamespace, array, num, arrayLength - num)) > 0)
				{
					num += num2;
				}
				this.CheckActualArrayLength(arrayLength, num, itemName, itemNamespace);
			}
			else
			{
				array = DecimalArrayHelperWithDictionaryString.Instance.ReadArray(this.dictionaryReader, itemName, itemNamespace, this.GetArrayLengthQuota(context));
				context.IncrementItemCount(array.Length);
			}
			return true;
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0003B470 File Offset: 0x00039670
		internal bool TryReadInt32Array(XmlObjectSerializerReadContext context, XmlDictionaryString itemName, XmlDictionaryString itemNamespace, int arrayLength, out int[] array)
		{
			if (this.dictionaryReader == null)
			{
				array = null;
				return false;
			}
			if (arrayLength != -1)
			{
				this.CheckExpectedArrayLength(context, arrayLength);
				array = new int[arrayLength];
				int num = 0;
				int num2;
				while ((num2 = this.dictionaryReader.ReadArray(itemName, itemNamespace, array, num, arrayLength - num)) > 0)
				{
					num += num2;
				}
				this.CheckActualArrayLength(arrayLength, num, itemName, itemNamespace);
			}
			else
			{
				array = Int32ArrayHelperWithDictionaryString.Instance.ReadArray(this.dictionaryReader, itemName, itemNamespace, this.GetArrayLengthQuota(context));
				context.IncrementItemCount(array.Length);
			}
			return true;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0003B4FC File Offset: 0x000396FC
		internal bool TryReadInt64Array(XmlObjectSerializerReadContext context, XmlDictionaryString itemName, XmlDictionaryString itemNamespace, int arrayLength, out long[] array)
		{
			if (this.dictionaryReader == null)
			{
				array = null;
				return false;
			}
			if (arrayLength != -1)
			{
				this.CheckExpectedArrayLength(context, arrayLength);
				array = new long[arrayLength];
				int num = 0;
				int num2;
				while ((num2 = this.dictionaryReader.ReadArray(itemName, itemNamespace, array, num, arrayLength - num)) > 0)
				{
					num += num2;
				}
				this.CheckActualArrayLength(arrayLength, num, itemName, itemNamespace);
			}
			else
			{
				array = Int64ArrayHelperWithDictionaryString.Instance.ReadArray(this.dictionaryReader, itemName, itemNamespace, this.GetArrayLengthQuota(context));
				context.IncrementItemCount(array.Length);
			}
			return true;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0003B588 File Offset: 0x00039788
		internal bool TryReadSingleArray(XmlObjectSerializerReadContext context, XmlDictionaryString itemName, XmlDictionaryString itemNamespace, int arrayLength, out float[] array)
		{
			if (this.dictionaryReader == null)
			{
				array = null;
				return false;
			}
			if (arrayLength != -1)
			{
				this.CheckExpectedArrayLength(context, arrayLength);
				array = new float[arrayLength];
				int num = 0;
				int num2;
				while ((num2 = this.dictionaryReader.ReadArray(itemName, itemNamespace, array, num, arrayLength - num)) > 0)
				{
					num += num2;
				}
				this.CheckActualArrayLength(arrayLength, num, itemName, itemNamespace);
			}
			else
			{
				array = SingleArrayHelperWithDictionaryString.Instance.ReadArray(this.dictionaryReader, itemName, itemNamespace, this.GetArrayLengthQuota(context));
				context.IncrementItemCount(array.Length);
			}
			return true;
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0003B614 File Offset: 0x00039814
		internal bool TryReadDoubleArray(XmlObjectSerializerReadContext context, XmlDictionaryString itemName, XmlDictionaryString itemNamespace, int arrayLength, out double[] array)
		{
			if (this.dictionaryReader == null)
			{
				array = null;
				return false;
			}
			if (arrayLength != -1)
			{
				this.CheckExpectedArrayLength(context, arrayLength);
				array = new double[arrayLength];
				int num = 0;
				int num2;
				while ((num2 = this.dictionaryReader.ReadArray(itemName, itemNamespace, array, num, arrayLength - num)) > 0)
				{
					num += num2;
				}
				this.CheckActualArrayLength(arrayLength, num, itemName, itemNamespace);
			}
			else
			{
				array = DoubleArrayHelperWithDictionaryString.Instance.ReadArray(this.dictionaryReader, itemName, itemNamespace, this.GetArrayLengthQuota(context));
				context.IncrementItemCount(array.Length);
			}
			return true;
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0003B6A0 File Offset: 0x000398A0
		internal IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			if (!(this.reader is IXmlNamespaceResolver))
			{
				return null;
			}
			return ((IXmlNamespaceResolver)this.reader).GetNamespacesInScope(scope);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0003B6C4 File Offset: 0x000398C4
		internal bool HasLineInfo()
		{
			IXmlLineInfo xmlLineInfo = this.reader as IXmlLineInfo;
			return xmlLineInfo != null && xmlLineInfo.HasLineInfo();
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x0003B6E8 File Offset: 0x000398E8
		internal int LineNumber
		{
			get
			{
				IXmlLineInfo xmlLineInfo = this.reader as IXmlLineInfo;
				if (xmlLineInfo != null)
				{
					return xmlLineInfo.LineNumber;
				}
				return 0;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000E21 RID: 3617 RVA: 0x0003B70C File Offset: 0x0003990C
		internal int LinePosition
		{
			get
			{
				IXmlLineInfo xmlLineInfo = this.reader as IXmlLineInfo;
				if (xmlLineInfo != null)
				{
					return xmlLineInfo.LinePosition;
				}
				return 0;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x0003B730 File Offset: 0x00039930
		// (set) Token: 0x06000E23 RID: 3619 RVA: 0x0003B76C File Offset: 0x0003996C
		internal bool Normalized
		{
			get
			{
				XmlTextReader xmlTextReader = this.reader as XmlTextReader;
				if (xmlTextReader == null)
				{
					IXmlTextParser xmlTextParser = this.reader as IXmlTextParser;
					return xmlTextParser != null && xmlTextParser.Normalized;
				}
				return xmlTextReader.Normalization;
			}
			set
			{
				XmlTextReader xmlTextReader = this.reader as XmlTextReader;
				if (xmlTextReader == null)
				{
					IXmlTextParser xmlTextParser = this.reader as IXmlTextParser;
					if (xmlTextParser != null)
					{
						xmlTextParser.Normalized = value;
						return;
					}
				}
				else
				{
					xmlTextReader.Normalization = value;
				}
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x0003B7A8 File Offset: 0x000399A8
		// (set) Token: 0x06000E25 RID: 3621 RVA: 0x0003B7E4 File Offset: 0x000399E4
		internal WhitespaceHandling WhitespaceHandling
		{
			get
			{
				XmlTextReader xmlTextReader = this.reader as XmlTextReader;
				if (xmlTextReader != null)
				{
					return xmlTextReader.WhitespaceHandling;
				}
				IXmlTextParser xmlTextParser = this.reader as IXmlTextParser;
				if (xmlTextParser != null)
				{
					return xmlTextParser.WhitespaceHandling;
				}
				return WhitespaceHandling.None;
			}
			set
			{
				XmlTextReader xmlTextReader = this.reader as XmlTextReader;
				if (xmlTextReader == null)
				{
					IXmlTextParser xmlTextParser = this.reader as IXmlTextParser;
					if (xmlTextParser != null)
					{
						xmlTextParser.WhitespaceHandling = value;
						return;
					}
				}
				else
				{
					xmlTextReader.WhitespaceHandling = value;
				}
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x0003B81E File Offset: 0x00039A1E
		internal string Name
		{
			get
			{
				return this.reader.Name;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000E27 RID: 3623 RVA: 0x0003B82B File Offset: 0x00039A2B
		public string LocalName
		{
			get
			{
				return this.reader.LocalName;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000E28 RID: 3624 RVA: 0x0003B838 File Offset: 0x00039A38
		internal string NamespaceURI
		{
			get
			{
				return this.reader.NamespaceURI;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000E29 RID: 3625 RVA: 0x0003B845 File Offset: 0x00039A45
		internal string Value
		{
			get
			{
				return this.reader.Value;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x0003B852 File Offset: 0x00039A52
		internal Type ValueType
		{
			get
			{
				return this.reader.ValueType;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000E2B RID: 3627 RVA: 0x0003B85F File Offset: 0x00039A5F
		internal int Depth
		{
			get
			{
				return this.reader.Depth;
			}
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0003B86C File Offset: 0x00039A6C
		internal string LookupNamespace(string prefix)
		{
			return this.reader.LookupNamespace(prefix);
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000E2D RID: 3629 RVA: 0x0003B87A File Offset: 0x00039A7A
		internal bool EOF
		{
			get
			{
				return this.reader.EOF;
			}
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0003B887 File Offset: 0x00039A87
		internal void Skip()
		{
			this.reader.Skip();
			this.isEndOfEmptyElement = false;
		}

		// Token: 0x0400057C RID: 1404
		protected XmlReader reader;

		// Token: 0x0400057D RID: 1405
		protected XmlDictionaryReader dictionaryReader;

		// Token: 0x0400057E RID: 1406
		protected bool isEndOfEmptyElement;
	}
}
