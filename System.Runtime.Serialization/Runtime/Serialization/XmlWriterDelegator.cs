using System;
using System.Globalization;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000EF RID: 239
	internal class XmlWriterDelegator
	{
		// Token: 0x06000E9B RID: 3739 RVA: 0x0003C2DF File Offset: 0x0003A4DF
		public XmlWriterDelegator(XmlWriter writer)
		{
			XmlObjectSerializer.CheckNull(writer, "writer");
			this.writer = writer;
			this.dictionaryWriter = writer as XmlDictionaryWriter;
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000E9C RID: 3740 RVA: 0x0003C305 File Offset: 0x0003A505
		internal XmlWriter Writer
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0003C30D File Offset: 0x0003A50D
		internal void Flush()
		{
			this.writer.Flush();
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0003C31A File Offset: 0x0003A51A
		internal string LookupPrefix(string ns)
		{
			return this.writer.LookupPrefix(ns);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0003C328 File Offset: 0x0003A528
		private void WriteEndAttribute()
		{
			this.writer.WriteEndAttribute();
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0003C335 File Offset: 0x0003A535
		public void WriteEndElement()
		{
			this.writer.WriteEndElement();
			this.depth--;
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0003C350 File Offset: 0x0003A550
		internal void WriteRaw(char[] buffer, int index, int count)
		{
			this.writer.WriteRaw(buffer, index, count);
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0003C360 File Offset: 0x0003A560
		internal void WriteRaw(string data)
		{
			this.writer.WriteRaw(data);
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0003C36E File Offset: 0x0003A56E
		internal void WriteXmlnsAttribute(XmlDictionaryString ns)
		{
			if (this.dictionaryWriter != null)
			{
				if (ns != null)
				{
					this.dictionaryWriter.WriteXmlnsAttribute(null, ns);
					return;
				}
			}
			else
			{
				this.WriteXmlnsAttribute(ns.Value);
			}
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0003C398 File Offset: 0x0003A598
		internal void WriteXmlnsAttribute(string ns)
		{
			if (ns != null)
			{
				if (ns.Length == 0)
				{
					this.writer.WriteAttributeString("xmlns", string.Empty, null, ns);
					return;
				}
				if (this.dictionaryWriter != null)
				{
					this.dictionaryWriter.WriteXmlnsAttribute(null, ns);
					return;
				}
				if (this.writer.LookupPrefix(ns) == null)
				{
					string text = string.Format(CultureInfo.InvariantCulture, "d{0}p{1}", this.depth, this.prefixes);
					this.prefixes++;
					this.writer.WriteAttributeString("xmlns", text, null, ns);
				}
			}
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0003C438 File Offset: 0x0003A638
		internal void WriteXmlnsAttribute(string prefix, XmlDictionaryString ns)
		{
			if (this.dictionaryWriter != null)
			{
				this.dictionaryWriter.WriteXmlnsAttribute(prefix, ns);
				return;
			}
			this.writer.WriteAttributeString("xmlns", prefix, null, ns.Value);
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0003C468 File Offset: 0x0003A668
		private void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.writer.WriteStartAttribute(prefix, localName, ns);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0003C478 File Offset: 0x0003A678
		private void WriteStartAttribute(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			if (this.dictionaryWriter != null)
			{
				this.dictionaryWriter.WriteStartAttribute(prefix, localName, namespaceUri);
				return;
			}
			this.writer.WriteStartAttribute(prefix, (localName == null) ? null : localName.Value, (namespaceUri == null) ? null : namespaceUri.Value);
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0003C4B5 File Offset: 0x0003A6B5
		internal void WriteAttributeString(string prefix, string localName, string ns, string value)
		{
			this.WriteStartAttribute(prefix, localName, ns);
			this.WriteAttributeStringValue(value);
			this.WriteEndAttribute();
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0003C4CE File Offset: 0x0003A6CE
		internal void WriteAttributeString(string prefix, XmlDictionaryString attrName, XmlDictionaryString attrNs, string value)
		{
			this.WriteStartAttribute(prefix, attrName, attrNs);
			this.WriteAttributeStringValue(value);
			this.WriteEndAttribute();
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0003C4E7 File Offset: 0x0003A6E7
		private void WriteAttributeStringValue(string value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0003C4F5 File Offset: 0x0003A6F5
		internal void WriteAttributeString(string prefix, XmlDictionaryString attrName, XmlDictionaryString attrNs, XmlDictionaryString value)
		{
			this.WriteStartAttribute(prefix, attrName, attrNs);
			this.WriteAttributeStringValue(value);
			this.WriteEndAttribute();
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0003C50E File Offset: 0x0003A70E
		private void WriteAttributeStringValue(XmlDictionaryString value)
		{
			if (this.dictionaryWriter == null)
			{
				this.writer.WriteString(value.Value);
				return;
			}
			this.dictionaryWriter.WriteString(value);
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0003C536 File Offset: 0x0003A736
		internal void WriteAttributeInt(string prefix, XmlDictionaryString attrName, XmlDictionaryString attrNs, int value)
		{
			this.WriteStartAttribute(prefix, attrName, attrNs);
			this.WriteAttributeIntValue(value);
			this.WriteEndAttribute();
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0003C54F File Offset: 0x0003A74F
		private void WriteAttributeIntValue(int value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0003C55D File Offset: 0x0003A75D
		internal void WriteAttributeBool(string prefix, XmlDictionaryString attrName, XmlDictionaryString attrNs, bool value)
		{
			this.WriteStartAttribute(prefix, attrName, attrNs);
			this.WriteAttributeBoolValue(value);
			this.WriteEndAttribute();
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0003C576 File Offset: 0x0003A776
		private void WriteAttributeBoolValue(bool value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0003C584 File Offset: 0x0003A784
		internal void WriteAttributeQualifiedName(string attrPrefix, XmlDictionaryString attrName, XmlDictionaryString attrNs, string name, string ns)
		{
			this.WriteXmlnsAttribute(ns);
			this.WriteStartAttribute(attrPrefix, attrName, attrNs);
			this.WriteAttributeQualifiedNameValue(name, ns);
			this.WriteEndAttribute();
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0003C5A7 File Offset: 0x0003A7A7
		private void WriteAttributeQualifiedNameValue(string name, string ns)
		{
			this.writer.WriteQualifiedName(name, ns);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0003C5B6 File Offset: 0x0003A7B6
		internal void WriteAttributeQualifiedName(string attrPrefix, XmlDictionaryString attrName, XmlDictionaryString attrNs, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteXmlnsAttribute(ns);
			this.WriteStartAttribute(attrPrefix, attrName, attrNs);
			this.WriteAttributeQualifiedNameValue(name, ns);
			this.WriteEndAttribute();
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0003C5D9 File Offset: 0x0003A7D9
		private void WriteAttributeQualifiedNameValue(XmlDictionaryString name, XmlDictionaryString ns)
		{
			if (this.dictionaryWriter == null)
			{
				this.writer.WriteQualifiedName(name.Value, ns.Value);
				return;
			}
			this.dictionaryWriter.WriteQualifiedName(name, ns);
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0003C608 File Offset: 0x0003A808
		internal void WriteStartElement(string localName, string ns)
		{
			this.WriteStartElement(null, localName, ns);
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0003C613 File Offset: 0x0003A813
		internal virtual void WriteStartElement(string prefix, string localName, string ns)
		{
			this.writer.WriteStartElement(prefix, localName, ns);
			this.depth++;
			this.prefixes = 1;
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0003C638 File Offset: 0x0003A838
		public void WriteStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			this.WriteStartElement(null, localName, namespaceUri);
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0003C644 File Offset: 0x0003A844
		internal void WriteStartElement(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			if (this.dictionaryWriter != null)
			{
				this.dictionaryWriter.WriteStartElement(prefix, localName, namespaceUri);
			}
			else
			{
				this.writer.WriteStartElement(prefix, (localName == null) ? null : localName.Value, (namespaceUri == null) ? null : namespaceUri.Value);
			}
			this.depth++;
			this.prefixes = 1;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0003C6A2 File Offset: 0x0003A8A2
		internal void WriteStartElementPrimitive(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			if (this.dictionaryWriter != null)
			{
				this.dictionaryWriter.WriteStartElement(null, localName, namespaceUri);
				return;
			}
			this.writer.WriteStartElement(null, (localName == null) ? null : localName.Value, (namespaceUri == null) ? null : namespaceUri.Value);
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0003C6DF File Offset: 0x0003A8DF
		internal void WriteEndElementPrimitive()
		{
			this.writer.WriteEndElement();
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000EBB RID: 3771 RVA: 0x0003C6EC File Offset: 0x0003A8EC
		internal WriteState WriteState
		{
			get
			{
				return this.writer.WriteState;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x0003C6F9 File Offset: 0x0003A8F9
		internal string XmlLang
		{
			get
			{
				return this.writer.XmlLang;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x0003C706 File Offset: 0x0003A906
		internal XmlSpace XmlSpace
		{
			get
			{
				return this.writer.XmlSpace;
			}
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0003C713 File Offset: 0x0003A913
		public void WriteNamespaceDecl(XmlDictionaryString ns)
		{
			this.WriteXmlnsAttribute(ns);
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x0003C71C File Offset: 0x0003A91C
		private Exception CreateInvalidPrimitiveTypeException(Type type)
		{
			return new InvalidDataContractException(SR.GetString("Type '{0}' is not a valid serializable type.", new object[] { DataContract.GetClrTypeFullName(type) }));
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0003C73C File Offset: 0x0003A93C
		internal void WriteAnyType(object value)
		{
			this.WriteAnyType(value, value.GetType());
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0003C74C File Offset: 0x0003A94C
		internal void WriteAnyType(object value, Type valueType)
		{
			bool flag = true;
			switch (Type.GetTypeCode(valueType))
			{
			case TypeCode.Boolean:
				this.WriteBoolean((bool)value);
				goto IL_01F5;
			case TypeCode.Char:
				this.WriteChar((char)value);
				goto IL_01F5;
			case TypeCode.SByte:
				this.WriteSignedByte((sbyte)value);
				goto IL_01F5;
			case TypeCode.Byte:
				this.WriteUnsignedByte((byte)value);
				goto IL_01F5;
			case TypeCode.Int16:
				this.WriteShort((short)value);
				goto IL_01F5;
			case TypeCode.UInt16:
				this.WriteUnsignedShort((ushort)value);
				goto IL_01F5;
			case TypeCode.Int32:
				this.WriteInt((int)value);
				goto IL_01F5;
			case TypeCode.UInt32:
				this.WriteUnsignedInt((uint)value);
				goto IL_01F5;
			case TypeCode.Int64:
				this.WriteLong((long)value);
				goto IL_01F5;
			case TypeCode.UInt64:
				this.WriteUnsignedLong((ulong)value);
				goto IL_01F5;
			case TypeCode.Single:
				this.WriteFloat((float)value);
				goto IL_01F5;
			case TypeCode.Double:
				this.WriteDouble((double)value);
				goto IL_01F5;
			case TypeCode.Decimal:
				this.WriteDecimal((decimal)value);
				goto IL_01F5;
			case TypeCode.DateTime:
				this.WriteDateTime((DateTime)value);
				goto IL_01F5;
			case TypeCode.String:
				this.WriteString((string)value);
				goto IL_01F5;
			}
			if (valueType == Globals.TypeOfByteArray)
			{
				this.WriteBase64((byte[])value);
			}
			else if (!(valueType == Globals.TypeOfObject))
			{
				if (valueType == Globals.TypeOfTimeSpan)
				{
					this.WriteTimeSpan((TimeSpan)value);
				}
				else if (valueType == Globals.TypeOfGuid)
				{
					this.WriteGuid((Guid)value);
				}
				else if (valueType == Globals.TypeOfUri)
				{
					this.WriteUri((Uri)value);
				}
				else if (valueType == Globals.TypeOfXmlQualifiedName)
				{
					this.WriteQName((XmlQualifiedName)value);
				}
				else
				{
					flag = false;
				}
			}
			IL_01F5:
			if (!flag)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(this.CreateInvalidPrimitiveTypeException(valueType));
			}
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0003C960 File Offset: 0x0003AB60
		internal void WriteExtensionData(IDataNode dataNode)
		{
			bool flag = true;
			Type dataType = dataNode.DataType;
			switch (Type.GetTypeCode(dataType))
			{
			case TypeCode.Boolean:
				this.WriteBoolean(((DataNode<bool>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.Char:
				this.WriteChar(((DataNode<char>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.SByte:
				this.WriteSignedByte(((DataNode<sbyte>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.Byte:
				this.WriteUnsignedByte(((DataNode<byte>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.Int16:
				this.WriteShort(((DataNode<short>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.UInt16:
				this.WriteUnsignedShort(((DataNode<ushort>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.Int32:
				this.WriteInt(((DataNode<int>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.UInt32:
				this.WriteUnsignedInt(((DataNode<uint>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.Int64:
				this.WriteLong(((DataNode<long>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.UInt64:
				this.WriteUnsignedLong(((DataNode<ulong>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.Single:
				this.WriteFloat(((DataNode<float>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.Double:
				this.WriteDouble(((DataNode<double>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.Decimal:
				this.WriteDecimal(((DataNode<decimal>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.DateTime:
				this.WriteDateTime(((DataNode<DateTime>)dataNode).GetValue());
				goto IL_027C;
			case TypeCode.String:
				this.WriteString(((DataNode<string>)dataNode).GetValue());
				goto IL_027C;
			}
			if (dataType == Globals.TypeOfByteArray)
			{
				this.WriteBase64(((DataNode<byte[]>)dataNode).GetValue());
			}
			else if (dataType == Globals.TypeOfObject)
			{
				object value = dataNode.Value;
				if (value != null)
				{
					this.WriteAnyType(value);
				}
			}
			else if (dataType == Globals.TypeOfTimeSpan)
			{
				this.WriteTimeSpan(((DataNode<TimeSpan>)dataNode).GetValue());
			}
			else if (dataType == Globals.TypeOfGuid)
			{
				this.WriteGuid(((DataNode<Guid>)dataNode).GetValue());
			}
			else if (dataType == Globals.TypeOfUri)
			{
				this.WriteUri(((DataNode<Uri>)dataNode).GetValue());
			}
			else if (dataType == Globals.TypeOfXmlQualifiedName)
			{
				this.WriteQName(((DataNode<XmlQualifiedName>)dataNode).GetValue());
			}
			else
			{
				flag = false;
			}
			IL_027C:
			if (!flag)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(this.CreateInvalidPrimitiveTypeException(dataType));
			}
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0003CBF9 File Offset: 0x0003ADF9
		internal void WriteString(string value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0003CC07 File Offset: 0x0003AE07
		internal virtual void WriteBoolean(bool value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0003CC15 File Offset: 0x0003AE15
		public void WriteBoolean(bool value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteBoolean(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0003CC2C File Offset: 0x0003AE2C
		internal virtual void WriteDateTime(DateTime value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0003CC3A File Offset: 0x0003AE3A
		public void WriteDateTime(DateTime value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteDateTime(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0003CC51 File Offset: 0x0003AE51
		internal virtual void WriteDecimal(decimal value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0003CC5F File Offset: 0x0003AE5F
		public void WriteDecimal(decimal value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteDecimal(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0003CC76 File Offset: 0x0003AE76
		internal virtual void WriteDouble(double value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0003CC84 File Offset: 0x0003AE84
		public void WriteDouble(double value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteDouble(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0003CC9B File Offset: 0x0003AE9B
		internal virtual void WriteInt(int value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0003CCA9 File Offset: 0x0003AEA9
		public void WriteInt(int value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteInt(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0003CCC0 File Offset: 0x0003AEC0
		internal virtual void WriteLong(long value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0003CCCE File Offset: 0x0003AECE
		public void WriteLong(long value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteLong(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0003CCE5 File Offset: 0x0003AEE5
		internal virtual void WriteFloat(float value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0003CCF3 File Offset: 0x0003AEF3
		public void WriteFloat(float value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteFloat(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0003CD0A File Offset: 0x0003AF0A
		internal virtual void WriteBase64(byte[] bytes)
		{
			if (bytes == null)
			{
				return;
			}
			this.writer.WriteBase64(bytes, 0, bytes.Length);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0003CD20 File Offset: 0x0003AF20
		internal virtual void WriteShort(short value)
		{
			this.writer.WriteValue((int)value);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0003CD2E File Offset: 0x0003AF2E
		public void WriteShort(short value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteShort(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0003CD45 File Offset: 0x0003AF45
		internal virtual void WriteUnsignedByte(byte value)
		{
			this.writer.WriteValue((int)value);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0003CD53 File Offset: 0x0003AF53
		public void WriteUnsignedByte(byte value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteUnsignedByte(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0003CD6A File Offset: 0x0003AF6A
		internal virtual void WriteSignedByte(sbyte value)
		{
			this.writer.WriteValue((int)value);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0003CD78 File Offset: 0x0003AF78
		public void WriteSignedByte(sbyte value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteSignedByte(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0003CD8F File Offset: 0x0003AF8F
		internal virtual void WriteUnsignedInt(uint value)
		{
			this.writer.WriteValue((long)((ulong)value));
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0003CD9E File Offset: 0x0003AF9E
		public void WriteUnsignedInt(uint value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteUnsignedInt(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0003CDB5 File Offset: 0x0003AFB5
		internal virtual void WriteUnsignedLong(ulong value)
		{
			this.writer.WriteRaw(XmlConvert.ToString(value));
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0003CDC8 File Offset: 0x0003AFC8
		public void WriteUnsignedLong(ulong value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteUnsignedLong(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0003CDDF File Offset: 0x0003AFDF
		internal virtual void WriteUnsignedShort(ushort value)
		{
			this.writer.WriteValue((int)value);
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0003CDED File Offset: 0x0003AFED
		public void WriteUnsignedShort(ushort value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteUnsignedShort(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0003CE04 File Offset: 0x0003B004
		internal virtual void WriteChar(char value)
		{
			this.writer.WriteValue((int)value);
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0003CE12 File Offset: 0x0003B012
		public void WriteChar(char value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteChar(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0003CE29 File Offset: 0x0003B029
		internal void WriteTimeSpan(TimeSpan value)
		{
			this.writer.WriteRaw(XmlConvert.ToString(value));
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0003CE3C File Offset: 0x0003B03C
		public void WriteTimeSpan(TimeSpan value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteTimeSpan(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0003CE53 File Offset: 0x0003B053
		internal void WriteGuid(Guid value)
		{
			this.writer.WriteRaw(value.ToString());
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0003CE6D File Offset: 0x0003B06D
		public void WriteGuid(Guid value, XmlDictionaryString name, XmlDictionaryString ns)
		{
			this.WriteStartElementPrimitive(name, ns);
			this.WriteGuid(value);
			this.WriteEndElementPrimitive();
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0003CE84 File Offset: 0x0003B084
		internal void WriteUri(Uri value)
		{
			this.writer.WriteString(value.GetComponents(UriComponents.SerializationInfoString, UriFormat.UriEscaped));
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0003CE9D File Offset: 0x0003B09D
		internal virtual void WriteQName(XmlQualifiedName value)
		{
			if (value != XmlQualifiedName.Empty)
			{
				this.WriteXmlnsAttribute(value.Namespace);
				this.WriteQualifiedName(value.Name, value.Namespace);
			}
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0003CECA File Offset: 0x0003B0CA
		internal void WriteQualifiedName(string localName, string ns)
		{
			this.writer.WriteQualifiedName(localName, ns);
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0003CED9 File Offset: 0x0003B0D9
		internal void WriteQualifiedName(XmlDictionaryString localName, XmlDictionaryString ns)
		{
			if (this.dictionaryWriter == null)
			{
				this.writer.WriteQualifiedName(localName.Value, ns.Value);
				return;
			}
			this.dictionaryWriter.WriteQualifiedName(localName, ns);
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0003CF08 File Offset: 0x0003B108
		public void WriteBooleanArray(bool[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			if (this.dictionaryWriter == null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					this.WriteBoolean(value[i], itemName, itemNamespace);
				}
				return;
			}
			this.dictionaryWriter.WriteArray(null, itemName, itemNamespace, value, 0, value.Length);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0003CF4C File Offset: 0x0003B14C
		public void WriteDateTimeArray(DateTime[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			if (this.dictionaryWriter == null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					this.WriteDateTime(value[i], itemName, itemNamespace);
				}
				return;
			}
			this.dictionaryWriter.WriteArray(null, itemName, itemNamespace, value, 0, value.Length);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0003CF94 File Offset: 0x0003B194
		public void WriteDecimalArray(decimal[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			if (this.dictionaryWriter == null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					this.WriteDecimal(value[i], itemName, itemNamespace);
				}
				return;
			}
			this.dictionaryWriter.WriteArray(null, itemName, itemNamespace, value, 0, value.Length);
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0003CFDC File Offset: 0x0003B1DC
		public void WriteInt32Array(int[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			if (this.dictionaryWriter == null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					this.WriteInt(value[i], itemName, itemNamespace);
				}
				return;
			}
			this.dictionaryWriter.WriteArray(null, itemName, itemNamespace, value, 0, value.Length);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0003D020 File Offset: 0x0003B220
		public void WriteInt64Array(long[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			if (this.dictionaryWriter == null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					this.WriteLong(value[i], itemName, itemNamespace);
				}
				return;
			}
			this.dictionaryWriter.WriteArray(null, itemName, itemNamespace, value, 0, value.Length);
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0003D064 File Offset: 0x0003B264
		public void WriteSingleArray(float[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			if (this.dictionaryWriter == null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					this.WriteFloat(value[i], itemName, itemNamespace);
				}
				return;
			}
			this.dictionaryWriter.WriteArray(null, itemName, itemNamespace, value, 0, value.Length);
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0003D0A8 File Offset: 0x0003B2A8
		public void WriteDoubleArray(double[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			if (this.dictionaryWriter == null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					this.WriteDouble(value[i], itemName, itemNamespace);
				}
				return;
			}
			this.dictionaryWriter.WriteArray(null, itemName, itemNamespace, value, 0, value.Length);
		}

		// Token: 0x04000589 RID: 1417
		protected XmlWriter writer;

		// Token: 0x0400058A RID: 1418
		protected XmlDictionaryWriter dictionaryWriter;

		// Token: 0x0400058B RID: 1419
		internal int depth;

		// Token: 0x0400058C RID: 1420
		private int prefixes;

		// Token: 0x0400058D RID: 1421
		private const int CharChunkSize = 76;

		// Token: 0x0400058E RID: 1422
		private const int ByteChunkSize = 57;
	}
}
