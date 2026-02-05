using System;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000EE RID: 238
	internal class XmlSerializableWriter : XmlWriter
	{
		// Token: 0x06000E79 RID: 3705 RVA: 0x0003BFD4 File Offset: 0x0003A1D4
		internal void BeginWrite(XmlWriter xmlWriter, object obj)
		{
			this.depth = 0;
			this.xmlWriter = xmlWriter;
			this.obj = obj;
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0003BFEC File Offset: 0x0003A1EC
		internal void EndWrite()
		{
			if (this.depth != 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("IXmlSerializable.WriteXml method of type '{0}' did not close all open tags. Verify that the IXmlSerializable implementation is correct.", new object[] { (this.obj == null) ? string.Empty : DataContract.GetClrTypeFullName(this.obj.GetType()) })));
			}
			this.obj = null;
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0003C045 File Offset: 0x0003A245
		public override void WriteStartDocument()
		{
			if (this.WriteState == WriteState.Start)
			{
				this.xmlWriter.WriteStartDocument();
			}
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x0003C05A File Offset: 0x0003A25A
		public override void WriteEndDocument()
		{
			this.xmlWriter.WriteEndDocument();
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0003C067 File Offset: 0x0003A267
		public override void WriteStartDocument(bool standalone)
		{
			if (this.WriteState == WriteState.Start)
			{
				this.xmlWriter.WriteStartDocument(standalone);
			}
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x0003C07D File Offset: 0x0003A27D
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0003C07F File Offset: 0x0003A27F
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.xmlWriter.WriteStartElement(prefix, localName, ns);
			this.depth++;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x0003C0A0 File Offset: 0x0003A2A0
		public override void WriteEndElement()
		{
			if (this.depth == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("IXmlSerializable.WriteXml method of type '{0}' attempted to close too many tags.  Verify that the IXmlSerializable implementation is correct.", new object[] { (this.obj == null) ? string.Empty : DataContract.GetClrTypeFullName(this.obj.GetType()) })));
			}
			this.xmlWriter.WriteEndElement();
			this.depth--;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0003C10C File Offset: 0x0003A30C
		public override void WriteFullEndElement()
		{
			if (this.depth == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("IXmlSerializable.WriteXml method of type '{0}' attempted to close too many tags.  Verify that the IXmlSerializable implementation is correct.", new object[] { (this.obj == null) ? string.Empty : DataContract.GetClrTypeFullName(this.obj.GetType()) })));
			}
			this.xmlWriter.WriteFullEndElement();
			this.depth--;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x0003C177 File Offset: 0x0003A377
		public override void Close()
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("This method cannot be called from IXmlSerializable implementations.")));
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0003C18D File Offset: 0x0003A38D
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.xmlWriter.WriteStartAttribute(prefix, localName, ns);
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0003C19D File Offset: 0x0003A39D
		public override void WriteEndAttribute()
		{
			this.xmlWriter.WriteEndAttribute();
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x0003C1AA File Offset: 0x0003A3AA
		public override void WriteCData(string text)
		{
			this.xmlWriter.WriteCData(text);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x0003C1B8 File Offset: 0x0003A3B8
		public override void WriteComment(string text)
		{
			this.xmlWriter.WriteComment(text);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0003C1C6 File Offset: 0x0003A3C6
		public override void WriteProcessingInstruction(string name, string text)
		{
			this.xmlWriter.WriteProcessingInstruction(name, text);
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x0003C1D5 File Offset: 0x0003A3D5
		public override void WriteEntityRef(string name)
		{
			this.xmlWriter.WriteEntityRef(name);
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0003C1E3 File Offset: 0x0003A3E3
		public override void WriteCharEntity(char ch)
		{
			this.xmlWriter.WriteCharEntity(ch);
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x0003C1F1 File Offset: 0x0003A3F1
		public override void WriteWhitespace(string ws)
		{
			this.xmlWriter.WriteWhitespace(ws);
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x0003C1FF File Offset: 0x0003A3FF
		public override void WriteString(string text)
		{
			this.xmlWriter.WriteString(text);
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0003C20D File Offset: 0x0003A40D
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.xmlWriter.WriteSurrogateCharEntity(lowChar, highChar);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0003C21C File Offset: 0x0003A41C
		public override void WriteChars(char[] buffer, int index, int count)
		{
			this.xmlWriter.WriteChars(buffer, index, count);
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0003C22C File Offset: 0x0003A42C
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this.xmlWriter.WriteRaw(buffer, index, count);
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0003C23C File Offset: 0x0003A43C
		public override void WriteRaw(string data)
		{
			this.xmlWriter.WriteRaw(data);
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0003C24A File Offset: 0x0003A44A
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			this.xmlWriter.WriteBase64(buffer, index, count);
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x0003C25A File Offset: 0x0003A45A
		public override void WriteBinHex(byte[] buffer, int index, int count)
		{
			this.xmlWriter.WriteBinHex(buffer, index, count);
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x0003C26A File Offset: 0x0003A46A
		public override WriteState WriteState
		{
			get
			{
				return this.xmlWriter.WriteState;
			}
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x0003C277 File Offset: 0x0003A477
		public override void Flush()
		{
			this.xmlWriter.Flush();
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0003C284 File Offset: 0x0003A484
		public override void WriteName(string name)
		{
			this.xmlWriter.WriteName(name);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0003C292 File Offset: 0x0003A492
		public override void WriteQualifiedName(string localName, string ns)
		{
			this.xmlWriter.WriteQualifiedName(localName, ns);
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x0003C2A1 File Offset: 0x0003A4A1
		public override string LookupPrefix(string ns)
		{
			return this.xmlWriter.LookupPrefix(ns);
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x0003C2AF File Offset: 0x0003A4AF
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.xmlWriter.XmlSpace;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x0003C2BC File Offset: 0x0003A4BC
		public override string XmlLang
		{
			get
			{
				return this.xmlWriter.XmlLang;
			}
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0003C2C9 File Offset: 0x0003A4C9
		public override void WriteNmToken(string name)
		{
			this.xmlWriter.WriteNmToken(name);
		}

		// Token: 0x04000586 RID: 1414
		private XmlWriter xmlWriter;

		// Token: 0x04000587 RID: 1415
		private int depth;

		// Token: 0x04000588 RID: 1416
		private object obj;
	}
}
