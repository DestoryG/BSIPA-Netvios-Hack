using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000032 RID: 50
	internal sealed class XmlCanonicalWriter
	{
		// Token: 0x0600033D RID: 829 RVA: 0x000111C0 File Offset: 0x0000F3C0
		public void SetOutput(Stream stream, bool includeComments, string[] inclusivePrefixes)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
			}
			if (this.writer == null)
			{
				this.writer = new XmlUTF8NodeWriter(XmlCanonicalWriter.isEscapedAttributeChar, XmlCanonicalWriter.isEscapedElementChar);
			}
			this.writer.SetOutput(stream, false, null);
			if (this.elementStream == null)
			{
				this.elementStream = new MemoryStream();
			}
			if (this.elementWriter == null)
			{
				this.elementWriter = new XmlUTF8NodeWriter(XmlCanonicalWriter.isEscapedAttributeChar, XmlCanonicalWriter.isEscapedElementChar);
			}
			this.elementWriter.SetOutput(this.elementStream, false, null);
			if (this.xmlnsAttributes == null)
			{
				this.xmlnsAttributeCount = 0;
				this.xmlnsOffset = 0;
				this.WriteXmlnsAttribute("xml", "http://www.w3.org/XML/1998/namespace");
				this.WriteXmlnsAttribute("xmlns", "http://www.w3.org/2000/xmlns/");
				this.WriteXmlnsAttribute(string.Empty, string.Empty);
				this.xmlnsStartOffset = this.xmlnsOffset;
				for (int i = 0; i < 3; i++)
				{
					this.xmlnsAttributes[i].referred = true;
				}
			}
			else
			{
				this.xmlnsAttributeCount = 3;
				this.xmlnsOffset = this.xmlnsStartOffset;
			}
			this.depth = 0;
			this.inStartElement = false;
			this.includeComments = includeComments;
			this.inclusivePrefixes = null;
			if (inclusivePrefixes != null)
			{
				this.inclusivePrefixes = new string[inclusivePrefixes.Length];
				for (int j = 0; j < inclusivePrefixes.Length; j++)
				{
					if (inclusivePrefixes[j] == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument(global::System.Runtime.Serialization.SR.GetString("The inclusive namespace prefix collection cannot contain null as one of the items."));
					}
					this.inclusivePrefixes[j] = inclusivePrefixes[j];
				}
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00011329 File Offset: 0x0000F529
		public void Flush()
		{
			this.ThrowIfClosed();
			this.writer.Flush();
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0001133C File Offset: 0x0000F53C
		public void Close()
		{
			if (this.writer != null)
			{
				this.writer.Close();
			}
			if (this.elementWriter != null)
			{
				this.elementWriter.Close();
			}
			if (this.elementStream != null && this.elementStream.Length > 512L)
			{
				this.elementStream = null;
			}
			this.elementBuffer = null;
			if (this.scopes != null && this.scopes.Length > 16)
			{
				this.scopes = null;
			}
			if (this.attributes != null && this.attributes.Length > 16)
			{
				this.attributes = null;
			}
			if (this.xmlnsBuffer != null && this.xmlnsBuffer.Length > 1024)
			{
				this.xmlnsAttributes = null;
				this.xmlnsBuffer = null;
			}
			this.inclusivePrefixes = null;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x000113FA File Offset: 0x0000F5FA
		public void WriteDeclaration()
		{
		}

		// Token: 0x06000341 RID: 833 RVA: 0x000113FC File Offset: 0x0000F5FC
		public void WriteComment(string value)
		{
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
			}
			this.ThrowIfClosed();
			if (this.includeComments)
			{
				this.writer.WriteComment(value);
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00011428 File Offset: 0x0000F628
		private void StartElement()
		{
			if (this.scopes == null)
			{
				this.scopes = new XmlCanonicalWriter.Scope[4];
			}
			else if (this.depth == this.scopes.Length)
			{
				XmlCanonicalWriter.Scope[] array = new XmlCanonicalWriter.Scope[this.depth * 2];
				Array.Copy(this.scopes, array, this.depth);
				this.scopes = array;
			}
			this.scopes[this.depth].xmlnsAttributeCount = this.xmlnsAttributeCount;
			this.scopes[this.depth].xmlnsOffset = this.xmlnsOffset;
			this.depth++;
			this.inStartElement = true;
			this.attributeCount = 0;
			this.elementStream.Position = 0L;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x000114E4 File Offset: 0x0000F6E4
		private void EndElement()
		{
			this.depth--;
			this.xmlnsAttributeCount = this.scopes[this.depth].xmlnsAttributeCount;
			this.xmlnsOffset = this.scopes[this.depth].xmlnsOffset;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00011538 File Offset: 0x0000F738
		public void WriteStartElement(string prefix, string localName)
		{
			if (prefix == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("prefix");
			}
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
			}
			this.ThrowIfClosed();
			bool flag = this.depth == 0;
			this.StartElement();
			this.element.prefixOffset = this.elementWriter.Position + 1;
			this.element.prefixLength = Encoding.UTF8.GetByteCount(prefix);
			this.element.localNameOffset = this.element.prefixOffset + this.element.prefixLength + ((this.element.prefixLength != 0) ? 1 : 0);
			this.element.localNameLength = Encoding.UTF8.GetByteCount(localName);
			this.elementWriter.WriteStartElement(prefix, localName);
			if (flag && this.inclusivePrefixes != null)
			{
				for (int i = 0; i < this.scopes[0].xmlnsAttributeCount; i++)
				{
					if (this.IsInclusivePrefix(ref this.xmlnsAttributes[i]))
					{
						XmlCanonicalWriter.XmlnsAttribute xmlnsAttribute = this.xmlnsAttributes[i];
						this.AddXmlnsAttribute(ref xmlnsAttribute);
					}
				}
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0001164C File Offset: 0x0000F84C
		public void WriteStartElement(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
		{
			if (prefixBuffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("prefixBuffer"));
			}
			if (prefixOffset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixOffset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (prefixOffset > prefixBuffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixOffset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { prefixBuffer.Length })));
			}
			if (prefixLength < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixLength", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (prefixLength > prefixBuffer.Length - prefixOffset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixLength", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { prefixBuffer.Length - prefixOffset })));
			}
			if (localNameBuffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localNameBuffer"));
			}
			if (localNameOffset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameOffset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (localNameOffset > localNameBuffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameOffset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { localNameBuffer.Length })));
			}
			if (localNameLength < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameLength", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (localNameLength > localNameBuffer.Length - localNameOffset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameLength", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { localNameBuffer.Length - localNameOffset })));
			}
			this.ThrowIfClosed();
			bool flag = this.depth == 0;
			this.StartElement();
			this.element.prefixOffset = this.elementWriter.Position + 1;
			this.element.prefixLength = prefixLength;
			this.element.localNameOffset = this.element.prefixOffset + prefixLength + ((prefixLength != 0) ? 1 : 0);
			this.element.localNameLength = localNameLength;
			this.elementWriter.WriteStartElement(prefixBuffer, prefixOffset, prefixLength, localNameBuffer, localNameOffset, localNameLength);
			if (flag && this.inclusivePrefixes != null)
			{
				for (int i = 0; i < this.scopes[0].xmlnsAttributeCount; i++)
				{
					if (this.IsInclusivePrefix(ref this.xmlnsAttributes[i]))
					{
						XmlCanonicalWriter.XmlnsAttribute xmlnsAttribute = this.xmlnsAttributes[i];
						this.AddXmlnsAttribute(ref xmlnsAttribute);
					}
				}
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00011898 File Offset: 0x0000FA98
		private bool IsInclusivePrefix(ref XmlCanonicalWriter.XmlnsAttribute xmlnsAttribute)
		{
			for (int i = 0; i < this.inclusivePrefixes.Length; i++)
			{
				if (this.inclusivePrefixes[i].Length == xmlnsAttribute.prefixLength && string.Compare(Encoding.UTF8.GetString(this.xmlnsBuffer, xmlnsAttribute.prefixOffset, xmlnsAttribute.prefixLength), this.inclusivePrefixes[i], StringComparison.Ordinal) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000118FC File Offset: 0x0000FAFC
		public void WriteEndStartElement(bool isEmpty)
		{
			this.ThrowIfClosed();
			this.elementWriter.Flush();
			this.elementBuffer = this.elementStream.GetBuffer();
			this.inStartElement = false;
			this.ResolvePrefixes();
			this.writer.WriteStartElement(this.elementBuffer, this.element.prefixOffset, this.element.prefixLength, this.elementBuffer, this.element.localNameOffset, this.element.localNameLength);
			for (int i = this.scopes[this.depth - 1].xmlnsAttributeCount; i < this.xmlnsAttributeCount; i++)
			{
				int j = i - 1;
				bool flag = false;
				while (j >= 0)
				{
					if (this.Equals(this.xmlnsBuffer, this.xmlnsAttributes[i].prefixOffset, this.xmlnsAttributes[i].prefixLength, this.xmlnsBuffer, this.xmlnsAttributes[j].prefixOffset, this.xmlnsAttributes[j].prefixLength))
					{
						if (!this.Equals(this.xmlnsBuffer, this.xmlnsAttributes[i].nsOffset, this.xmlnsAttributes[i].nsLength, this.xmlnsBuffer, this.xmlnsAttributes[j].nsOffset, this.xmlnsAttributes[j].nsLength))
						{
							break;
						}
						if (this.xmlnsAttributes[j].referred)
						{
							flag = true;
							break;
						}
					}
					j--;
				}
				if (!flag)
				{
					this.WriteXmlnsAttribute(ref this.xmlnsAttributes[i]);
				}
			}
			if (this.attributeCount > 0)
			{
				if (this.attributeCount > 1)
				{
					this.SortAttributes();
				}
				for (int k = 0; k < this.attributeCount; k++)
				{
					this.writer.WriteText(this.elementBuffer, this.attributes[k].offset, this.attributes[k].length);
				}
			}
			this.writer.WriteEndStartElement(false);
			if (isEmpty)
			{
				this.writer.WriteEndElement(this.elementBuffer, this.element.prefixOffset, this.element.prefixLength, this.elementBuffer, this.element.localNameOffset, this.element.localNameLength);
				this.EndElement();
			}
			this.elementBuffer = null;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00011B56 File Offset: 0x0000FD56
		public void WriteEndElement(string prefix, string localName)
		{
			if (prefix == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("prefix");
			}
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
			}
			this.ThrowIfClosed();
			this.writer.WriteEndElement(prefix, localName);
			this.EndElement();
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00011B90 File Offset: 0x0000FD90
		private void EnsureXmlnsBuffer(int byteCount)
		{
			if (this.xmlnsBuffer == null)
			{
				this.xmlnsBuffer = new byte[Math.Max(byteCount, 128)];
				return;
			}
			if (this.xmlnsOffset + byteCount > this.xmlnsBuffer.Length)
			{
				byte[] array = new byte[Math.Max(this.xmlnsOffset + byteCount, this.xmlnsBuffer.Length * 2)];
				Buffer.BlockCopy(this.xmlnsBuffer, 0, array, 0, this.xmlnsOffset);
				this.xmlnsBuffer = array;
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00011C08 File Offset: 0x0000FE08
		public void WriteXmlnsAttribute(string prefix, string ns)
		{
			if (prefix == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("prefix");
			}
			if (ns == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("ns");
			}
			this.ThrowIfClosed();
			if (prefix.Length > 2147483647 - ns.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("ns", global::System.Runtime.Serialization.SR.GetString("The combined length of the prefix and namespace must not be greater than {0}.", new object[] { 715827882 })));
			}
			int num = prefix.Length + ns.Length;
			if (num > 715827882)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("ns", global::System.Runtime.Serialization.SR.GetString("The combined length of the prefix and namespace must not be greater than {0}.", new object[] { 715827882 })));
			}
			this.EnsureXmlnsBuffer(num * 3);
			XmlCanonicalWriter.XmlnsAttribute xmlnsAttribute;
			xmlnsAttribute.prefixOffset = this.xmlnsOffset;
			xmlnsAttribute.prefixLength = Encoding.UTF8.GetBytes(prefix, 0, prefix.Length, this.xmlnsBuffer, this.xmlnsOffset);
			this.xmlnsOffset += xmlnsAttribute.prefixLength;
			xmlnsAttribute.nsOffset = this.xmlnsOffset;
			xmlnsAttribute.nsLength = Encoding.UTF8.GetBytes(ns, 0, ns.Length, this.xmlnsBuffer, this.xmlnsOffset);
			this.xmlnsOffset += xmlnsAttribute.nsLength;
			xmlnsAttribute.referred = false;
			this.AddXmlnsAttribute(ref xmlnsAttribute);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00011D60 File Offset: 0x0000FF60
		public void WriteXmlnsAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] nsBuffer, int nsOffset, int nsLength)
		{
			if (prefixBuffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("prefixBuffer"));
			}
			if (prefixOffset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixOffset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (prefixOffset > prefixBuffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixOffset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { prefixBuffer.Length })));
			}
			if (prefixLength < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixLength", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (prefixLength > prefixBuffer.Length - prefixOffset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixLength", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { prefixBuffer.Length - prefixOffset })));
			}
			if (nsBuffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("nsBuffer"));
			}
			if (nsOffset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("nsOffset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (nsOffset > nsBuffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("nsOffset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { nsBuffer.Length })));
			}
			if (nsLength < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("nsLength", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (nsLength > nsBuffer.Length - nsOffset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("nsLength", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { nsBuffer.Length - nsOffset })));
			}
			this.ThrowIfClosed();
			if (prefixLength > 2147483647 - nsLength)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("nsLength", global::System.Runtime.Serialization.SR.GetString("The combined length of the prefix and namespace must not be greater than {0}.", new object[] { int.MaxValue })));
			}
			this.EnsureXmlnsBuffer(prefixLength + nsLength);
			XmlCanonicalWriter.XmlnsAttribute xmlnsAttribute;
			xmlnsAttribute.prefixOffset = this.xmlnsOffset;
			xmlnsAttribute.prefixLength = prefixLength;
			Buffer.BlockCopy(prefixBuffer, prefixOffset, this.xmlnsBuffer, this.xmlnsOffset, prefixLength);
			this.xmlnsOffset += prefixLength;
			xmlnsAttribute.nsOffset = this.xmlnsOffset;
			xmlnsAttribute.nsLength = nsLength;
			Buffer.BlockCopy(nsBuffer, nsOffset, this.xmlnsBuffer, this.xmlnsOffset, nsLength);
			this.xmlnsOffset += nsLength;
			xmlnsAttribute.referred = false;
			this.AddXmlnsAttribute(ref xmlnsAttribute);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00011FB0 File Offset: 0x000101B0
		public void WriteStartAttribute(string prefix, string localName)
		{
			if (prefix == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("prefix");
			}
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
			}
			this.ThrowIfClosed();
			this.attribute.offset = this.elementWriter.Position;
			this.attribute.length = 0;
			this.attribute.prefixOffset = this.attribute.offset + 1;
			this.attribute.prefixLength = Encoding.UTF8.GetByteCount(prefix);
			this.attribute.localNameOffset = this.attribute.prefixOffset + this.attribute.prefixLength + ((this.attribute.prefixLength != 0) ? 1 : 0);
			this.attribute.localNameLength = Encoding.UTF8.GetByteCount(localName);
			this.attribute.nsOffset = 0;
			this.attribute.nsLength = 0;
			this.elementWriter.WriteStartAttribute(prefix, localName);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x000120A0 File Offset: 0x000102A0
		public void WriteStartAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
		{
			if (prefixBuffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("prefixBuffer"));
			}
			if (prefixOffset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixOffset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (prefixOffset > prefixBuffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixOffset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { prefixBuffer.Length })));
			}
			if (prefixLength < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixLength", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (prefixLength > prefixBuffer.Length - prefixOffset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("prefixLength", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { prefixBuffer.Length - prefixOffset })));
			}
			if (localNameBuffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localNameBuffer"));
			}
			if (localNameOffset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameOffset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (localNameOffset > localNameBuffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameOffset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { localNameBuffer.Length })));
			}
			if (localNameLength < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameLength", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (localNameLength > localNameBuffer.Length - localNameOffset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("localNameLength", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { localNameBuffer.Length - localNameOffset })));
			}
			this.ThrowIfClosed();
			this.attribute.offset = this.elementWriter.Position;
			this.attribute.length = 0;
			this.attribute.prefixOffset = this.attribute.offset + 1;
			this.attribute.prefixLength = prefixLength;
			this.attribute.localNameOffset = this.attribute.prefixOffset + prefixLength + ((prefixLength != 0) ? 1 : 0);
			this.attribute.localNameLength = localNameLength;
			this.attribute.nsOffset = 0;
			this.attribute.nsLength = 0;
			this.elementWriter.WriteStartAttribute(prefixBuffer, prefixOffset, prefixLength, localNameBuffer, localNameOffset, localNameLength);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x000122C8 File Offset: 0x000104C8
		public void WriteEndAttribute()
		{
			this.ThrowIfClosed();
			this.elementWriter.WriteEndAttribute();
			this.attribute.length = this.elementWriter.Position - this.attribute.offset;
			this.AddAttribute(ref this.attribute);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00012314 File Offset: 0x00010514
		public void WriteCharEntity(int ch)
		{
			this.ThrowIfClosed();
			if (ch <= 65535)
			{
				char[] array = new char[] { (char)ch };
				this.WriteEscapedText(array, 0, 1);
				return;
			}
			this.WriteText(ch);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0001234C File Offset: 0x0001054C
		public void WriteEscapedText(string value)
		{
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
			}
			this.ThrowIfClosed();
			if (this.depth > 0)
			{
				if (this.inStartElement)
				{
					this.elementWriter.WriteEscapedText(value);
					return;
				}
				this.writer.WriteEscapedText(value);
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0001238C File Offset: 0x0001058C
		public void WriteEscapedText(byte[] chars, int offset, int count)
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
			this.ThrowIfClosed();
			if (this.depth > 0)
			{
				if (this.inStartElement)
				{
					this.elementWriter.WriteEscapedText(chars, offset, count);
					return;
				}
				this.writer.WriteEscapedText(chars, offset, count);
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00012482 File Offset: 0x00010682
		public void WriteEscapedText(char[] chars, int offset, int count)
		{
			this.ThrowIfClosed();
			if (this.depth > 0)
			{
				if (this.inStartElement)
				{
					this.elementWriter.WriteEscapedText(chars, offset, count);
					return;
				}
				this.writer.WriteEscapedText(chars, offset, count);
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000124B8 File Offset: 0x000106B8
		public void WriteText(int ch)
		{
			this.ThrowIfClosed();
			if (this.inStartElement)
			{
				this.elementWriter.WriteText(ch);
				return;
			}
			this.writer.WriteText(ch);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000124E4 File Offset: 0x000106E4
		public void WriteText(byte[] chars, int offset, int count)
		{
			this.ThrowIfClosed();
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
			if (this.inStartElement)
			{
				this.elementWriter.WriteText(chars, offset, count);
				return;
			}
			this.writer.WriteText(chars, offset, count);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x000125D1 File Offset: 0x000107D1
		public void WriteText(string value)
		{
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
			}
			if (value.Length > 0)
			{
				if (this.inStartElement)
				{
					this.elementWriter.WriteText(value);
					return;
				}
				this.writer.WriteText(value);
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00012610 File Offset: 0x00010810
		public void WriteText(char[] chars, int offset, int count)
		{
			this.ThrowIfClosed();
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
			if (this.inStartElement)
			{
				this.elementWriter.WriteText(chars, offset, count);
				return;
			}
			this.writer.WriteText(chars, offset, count);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000126FD File Offset: 0x000108FD
		private void ThrowIfClosed()
		{
			if (this.writer == null)
			{
				this.ThrowClosed();
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0001270D File Offset: 0x0001090D
		private void ThrowClosed()
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ObjectDisposedException(base.GetType().ToString()));
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00012724 File Offset: 0x00010924
		private void WriteXmlnsAttribute(ref XmlCanonicalWriter.XmlnsAttribute xmlnsAttribute)
		{
			if (xmlnsAttribute.referred)
			{
				this.writer.WriteXmlnsAttribute(this.xmlnsBuffer, xmlnsAttribute.prefixOffset, xmlnsAttribute.prefixLength, this.xmlnsBuffer, xmlnsAttribute.nsOffset, xmlnsAttribute.nsLength);
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00012760 File Offset: 0x00010960
		private void SortAttributes()
		{
			if (this.attributeCount < 16)
			{
				for (int i = 0; i < this.attributeCount - 1; i++)
				{
					int num = i;
					for (int j = i + 1; j < this.attributeCount; j++)
					{
						if (this.Compare(ref this.attributes[j], ref this.attributes[num]) < 0)
						{
							num = j;
						}
					}
					if (num != i)
					{
						XmlCanonicalWriter.Attribute attribute = this.attributes[i];
						this.attributes[i] = this.attributes[num];
						this.attributes[num] = attribute;
					}
				}
				return;
			}
			new XmlCanonicalWriter.AttributeSorter(this).Sort();
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00012808 File Offset: 0x00010A08
		private void AddAttribute(ref XmlCanonicalWriter.Attribute attribute)
		{
			if (this.attributes == null)
			{
				this.attributes = new XmlCanonicalWriter.Attribute[4];
			}
			else if (this.attributeCount == this.attributes.Length)
			{
				XmlCanonicalWriter.Attribute[] array = new XmlCanonicalWriter.Attribute[this.attributeCount * 2];
				Array.Copy(this.attributes, array, this.attributeCount);
				this.attributes = array;
			}
			this.attributes[this.attributeCount] = attribute;
			this.attributeCount++;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00012888 File Offset: 0x00010A88
		private void AddXmlnsAttribute(ref XmlCanonicalWriter.XmlnsAttribute xmlnsAttribute)
		{
			if (this.xmlnsAttributes == null)
			{
				this.xmlnsAttributes = new XmlCanonicalWriter.XmlnsAttribute[4];
			}
			else if (this.xmlnsAttributes.Length == this.xmlnsAttributeCount)
			{
				XmlCanonicalWriter.XmlnsAttribute[] array = new XmlCanonicalWriter.XmlnsAttribute[this.xmlnsAttributeCount * 2];
				Array.Copy(this.xmlnsAttributes, array, this.xmlnsAttributeCount);
				this.xmlnsAttributes = array;
			}
			if (this.depth > 0 && this.inclusivePrefixes != null && this.IsInclusivePrefix(ref xmlnsAttribute))
			{
				xmlnsAttribute.referred = true;
			}
			if (this.depth == 0)
			{
				XmlCanonicalWriter.XmlnsAttribute[] array2 = this.xmlnsAttributes;
				int num = this.xmlnsAttributeCount;
				this.xmlnsAttributeCount = num + 1;
				array2[num] = xmlnsAttribute;
				return;
			}
			int i = this.scopes[this.depth - 1].xmlnsAttributeCount;
			bool flag = true;
			while (i < this.xmlnsAttributeCount)
			{
				int num2 = this.Compare(ref xmlnsAttribute, ref this.xmlnsAttributes[i]);
				if (num2 > 0)
				{
					i++;
				}
				else
				{
					if (num2 == 0)
					{
						this.xmlnsAttributes[i] = xmlnsAttribute;
						flag = false;
						break;
					}
					break;
				}
			}
			if (flag)
			{
				Array.Copy(this.xmlnsAttributes, i, this.xmlnsAttributes, i + 1, this.xmlnsAttributeCount - i);
				this.xmlnsAttributes[i] = xmlnsAttribute;
				this.xmlnsAttributeCount++;
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x000129D0 File Offset: 0x00010BD0
		private void ResolvePrefix(int prefixOffset, int prefixLength, out int nsOffset, out int nsLength)
		{
			int num = this.scopes[this.depth - 1].xmlnsAttributeCount;
			int num2 = this.xmlnsAttributeCount - 1;
			while (!this.Equals(this.elementBuffer, prefixOffset, prefixLength, this.xmlnsBuffer, this.xmlnsAttributes[num2].prefixOffset, this.xmlnsAttributes[num2].prefixLength))
			{
				num2--;
			}
			nsOffset = this.xmlnsAttributes[num2].nsOffset;
			nsLength = this.xmlnsAttributes[num2].nsLength;
			if (num2 < num)
			{
				if (!this.xmlnsAttributes[num2].referred)
				{
					XmlCanonicalWriter.XmlnsAttribute xmlnsAttribute = this.xmlnsAttributes[num2];
					xmlnsAttribute.referred = true;
					this.AddXmlnsAttribute(ref xmlnsAttribute);
					return;
				}
			}
			else
			{
				this.xmlnsAttributes[num2].referred = true;
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00012AAB File Offset: 0x00010CAB
		private void ResolvePrefix(ref XmlCanonicalWriter.Attribute attribute)
		{
			if (attribute.prefixLength != 0)
			{
				this.ResolvePrefix(attribute.prefixOffset, attribute.prefixLength, out attribute.nsOffset, out attribute.nsLength);
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00012AD4 File Offset: 0x00010CD4
		private void ResolvePrefixes()
		{
			int num;
			int num2;
			this.ResolvePrefix(this.element.prefixOffset, this.element.prefixLength, out num, out num2);
			for (int i = 0; i < this.attributeCount; i++)
			{
				this.ResolvePrefix(ref this.attributes[i]);
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00012B24 File Offset: 0x00010D24
		private int Compare(ref XmlCanonicalWriter.XmlnsAttribute xmlnsAttribute1, ref XmlCanonicalWriter.XmlnsAttribute xmlnsAttribute2)
		{
			return this.Compare(this.xmlnsBuffer, xmlnsAttribute1.prefixOffset, xmlnsAttribute1.prefixLength, xmlnsAttribute2.prefixOffset, xmlnsAttribute2.prefixLength);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00012B4C File Offset: 0x00010D4C
		private int Compare(ref XmlCanonicalWriter.Attribute attribute1, ref XmlCanonicalWriter.Attribute attribute2)
		{
			int num = this.Compare(this.xmlnsBuffer, attribute1.nsOffset, attribute1.nsLength, attribute2.nsOffset, attribute2.nsLength);
			if (num == 0)
			{
				num = this.Compare(this.elementBuffer, attribute1.localNameOffset, attribute1.localNameLength, attribute2.localNameOffset, attribute2.localNameLength);
			}
			return num;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00012BA7 File Offset: 0x00010DA7
		private int Compare(byte[] buffer, int offset1, int length1, int offset2, int length2)
		{
			if (offset1 == offset2)
			{
				return length1 - length2;
			}
			return this.Compare(buffer, offset1, length1, buffer, offset2, length2);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00012BC4 File Offset: 0x00010DC4
		private int Compare(byte[] buffer1, int offset1, int length1, byte[] buffer2, int offset2, int length2)
		{
			int num = Math.Min(length1, length2);
			int num2 = 0;
			int num3 = 0;
			while (num3 < num && num2 == 0)
			{
				num2 = (int)(buffer1[offset1 + num3] - buffer2[offset2 + num3]);
				num3++;
			}
			if (num2 == 0)
			{
				num2 = length1 - length2;
			}
			return num2;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00012C04 File Offset: 0x00010E04
		private bool Equals(byte[] buffer1, int offset1, int length1, byte[] buffer2, int offset2, int length2)
		{
			if (length1 != length2)
			{
				return false;
			}
			for (int i = 0; i < length1; i++)
			{
				if (buffer1[offset1 + i] != buffer2[offset2 + i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040001BD RID: 445
		private XmlUTF8NodeWriter writer;

		// Token: 0x040001BE RID: 446
		private MemoryStream elementStream;

		// Token: 0x040001BF RID: 447
		private byte[] elementBuffer;

		// Token: 0x040001C0 RID: 448
		private XmlUTF8NodeWriter elementWriter;

		// Token: 0x040001C1 RID: 449
		private bool inStartElement;

		// Token: 0x040001C2 RID: 450
		private int depth;

		// Token: 0x040001C3 RID: 451
		private XmlCanonicalWriter.Scope[] scopes;

		// Token: 0x040001C4 RID: 452
		private int xmlnsAttributeCount;

		// Token: 0x040001C5 RID: 453
		private XmlCanonicalWriter.XmlnsAttribute[] xmlnsAttributes;

		// Token: 0x040001C6 RID: 454
		private int attributeCount;

		// Token: 0x040001C7 RID: 455
		private XmlCanonicalWriter.Attribute[] attributes;

		// Token: 0x040001C8 RID: 456
		private XmlCanonicalWriter.Attribute attribute;

		// Token: 0x040001C9 RID: 457
		private XmlCanonicalWriter.Element element;

		// Token: 0x040001CA RID: 458
		private byte[] xmlnsBuffer;

		// Token: 0x040001CB RID: 459
		private int xmlnsOffset;

		// Token: 0x040001CC RID: 460
		private const int maxBytesPerChar = 3;

		// Token: 0x040001CD RID: 461
		private int xmlnsStartOffset;

		// Token: 0x040001CE RID: 462
		private bool includeComments;

		// Token: 0x040001CF RID: 463
		private string[] inclusivePrefixes;

		// Token: 0x040001D0 RID: 464
		private const string xmlnsNamespace = "http://www.w3.org/2000/xmlns/";

		// Token: 0x040001D1 RID: 465
		private static readonly bool[] isEscapedAttributeChar = new bool[]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true, true, true, true, true, true, true, true, true,
			true, true, true, true, true, true, true, true, true, true,
			true, true, false, false, true, false, false, false, true, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, false
		};

		// Token: 0x040001D2 RID: 466
		private static readonly bool[] isEscapedElementChar = new bool[]
		{
			true, true, true, true, true, true, true, true, true, false,
			false, true, true, true, true, true, true, true, true, true,
			true, true, true, true, true, true, true, true, true, true,
			true, true, false, false, false, false, false, false, true, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, true, false
		};

		// Token: 0x0200014C RID: 332
		private class AttributeSorter : IComparer
		{
			// Token: 0x060012A4 RID: 4772 RVA: 0x0004DBBC File Offset: 0x0004BDBC
			public AttributeSorter(XmlCanonicalWriter writer)
			{
				this.writer = writer;
			}

			// Token: 0x060012A5 RID: 4773 RVA: 0x0004DBCC File Offset: 0x0004BDCC
			public void Sort()
			{
				object[] array = new object[this.writer.attributeCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = i;
				}
				Array.Sort(array, this);
				XmlCanonicalWriter.Attribute[] array2 = new XmlCanonicalWriter.Attribute[this.writer.attributes.Length];
				for (int j = 0; j < array.Length; j++)
				{
					array2[j] = this.writer.attributes[(int)array[j]];
				}
				this.writer.attributes = array2;
			}

			// Token: 0x060012A6 RID: 4774 RVA: 0x0004DC54 File Offset: 0x0004BE54
			public int Compare(object obj1, object obj2)
			{
				int num = (int)obj1;
				int num2 = (int)obj2;
				return this.writer.Compare(ref this.writer.attributes[num], ref this.writer.attributes[num2]);
			}

			// Token: 0x04000927 RID: 2343
			private XmlCanonicalWriter writer;
		}

		// Token: 0x0200014D RID: 333
		private struct Scope
		{
			// Token: 0x04000928 RID: 2344
			public int xmlnsAttributeCount;

			// Token: 0x04000929 RID: 2345
			public int xmlnsOffset;
		}

		// Token: 0x0200014E RID: 334
		private struct Element
		{
			// Token: 0x0400092A RID: 2346
			public int prefixOffset;

			// Token: 0x0400092B RID: 2347
			public int prefixLength;

			// Token: 0x0400092C RID: 2348
			public int localNameOffset;

			// Token: 0x0400092D RID: 2349
			public int localNameLength;
		}

		// Token: 0x0200014F RID: 335
		private struct Attribute
		{
			// Token: 0x0400092E RID: 2350
			public int prefixOffset;

			// Token: 0x0400092F RID: 2351
			public int prefixLength;

			// Token: 0x04000930 RID: 2352
			public int localNameOffset;

			// Token: 0x04000931 RID: 2353
			public int localNameLength;

			// Token: 0x04000932 RID: 2354
			public int nsOffset;

			// Token: 0x04000933 RID: 2355
			public int nsLength;

			// Token: 0x04000934 RID: 2356
			public int offset;

			// Token: 0x04000935 RID: 2357
			public int length;
		}

		// Token: 0x02000150 RID: 336
		private struct XmlnsAttribute
		{
			// Token: 0x04000936 RID: 2358
			public int prefixOffset;

			// Token: 0x04000937 RID: 2359
			public int prefixLength;

			// Token: 0x04000938 RID: 2360
			public int nsOffset;

			// Token: 0x04000939 RID: 2361
			public int nsLength;

			// Token: 0x0400093A RID: 2362
			public bool referred;
		}
	}
}
