using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security;

namespace System.Xml
{
	// Token: 0x0200002F RID: 47
	internal class XmlBinaryWriter : XmlBaseWriter, IXmlBinaryWriterInitializer
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x0000E970 File Offset: 0x0000CB70
		public void SetOutput(Stream stream, IXmlDictionary dictionary, XmlBinaryWriterSession session, bool ownsStream)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("stream"));
			}
			if (this.writer == null)
			{
				this.writer = new XmlBinaryNodeWriter();
			}
			this.writer.SetOutput(stream, dictionary, session, ownsStream);
			base.SetOutput(this.writer);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000E9BF File Offset: 0x0000CBBF
		protected override XmlSigningNodeWriter CreateSigningNodeWriter()
		{
			return new XmlSigningNodeWriter(false);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000E9C8 File Offset: 0x0000CBC8
		protected override void WriteTextNode(XmlDictionaryReader reader, bool attribute)
		{
			Type valueType = reader.ValueType;
			if (valueType == typeof(string))
			{
				XmlDictionaryString xmlDictionaryString;
				if (reader.TryGetValueAsDictionaryString(out xmlDictionaryString))
				{
					this.WriteString(xmlDictionaryString);
				}
				else if (reader.CanReadValueChunk)
				{
					if (this.chars == null)
					{
						this.chars = new char[256];
					}
					int num;
					while ((num = reader.ReadValueChunk(this.chars, 0, this.chars.Length)) > 0)
					{
						this.WriteChars(this.chars, 0, num);
					}
				}
				else
				{
					this.WriteString(reader.Value);
				}
				if (!attribute)
				{
					reader.Read();
					return;
				}
			}
			else if (valueType == typeof(byte[]))
			{
				if (reader.CanReadBinaryContent)
				{
					if (this.bytes == null)
					{
						this.bytes = new byte[384];
					}
					int num2;
					while ((num2 = reader.ReadValueAsBase64(this.bytes, 0, this.bytes.Length)) > 0)
					{
						this.WriteBase64(this.bytes, 0, num2);
					}
				}
				else
				{
					this.WriteString(reader.Value);
				}
				if (!attribute)
				{
					reader.Read();
					return;
				}
			}
			else
			{
				if (valueType == typeof(int))
				{
					this.WriteValue(reader.ReadContentAsInt());
					return;
				}
				if (valueType == typeof(long))
				{
					this.WriteValue(reader.ReadContentAsLong());
					return;
				}
				if (valueType == typeof(bool))
				{
					this.WriteValue(reader.ReadContentAsBoolean());
					return;
				}
				if (valueType == typeof(double))
				{
					this.WriteValue(reader.ReadContentAsDouble());
					return;
				}
				if (valueType == typeof(DateTime))
				{
					this.WriteValue(reader.ReadContentAsDateTime());
					return;
				}
				if (valueType == typeof(float))
				{
					this.WriteValue(reader.ReadContentAsFloat());
					return;
				}
				if (valueType == typeof(decimal))
				{
					this.WriteValue(reader.ReadContentAsDecimal());
					return;
				}
				if (valueType == typeof(UniqueId))
				{
					this.WriteValue(reader.ReadContentAsUniqueId());
					return;
				}
				if (valueType == typeof(Guid))
				{
					this.WriteValue(reader.ReadContentAsGuid());
					return;
				}
				if (valueType == typeof(TimeSpan))
				{
					this.WriteValue(reader.ReadContentAsTimeSpan());
					return;
				}
				this.WriteValue(reader.ReadContentAsObject());
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000EC21 File Offset: 0x0000CE21
		private void WriteStartArray(string prefix, string localName, string namespaceUri, int count)
		{
			base.StartArray(count);
			this.writer.WriteArrayNode();
			this.WriteStartElement(prefix, localName, namespaceUri);
			this.WriteEndElement();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000EC45 File Offset: 0x0000CE45
		private void WriteStartArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, int count)
		{
			base.StartArray(count);
			this.writer.WriteArrayNode();
			this.WriteStartElement(prefix, localName, namespaceUri);
			this.WriteEndElement();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000EC69 File Offset: 0x0000CE69
		private void WriteEndArray()
		{
			base.EndArray();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000EC71 File Offset: 0x0000CE71
		[SecurityCritical]
		private unsafe void UnsafeWriteArray(string prefix, string localName, string namespaceUri, XmlBinaryNodeType nodeType, int count, byte* array, byte* arrayMax)
		{
			this.WriteStartArray(prefix, localName, namespaceUri, count);
			this.writer.UnsafeWriteArray(nodeType, count, array, arrayMax);
			this.WriteEndArray();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000EC97 File Offset: 0x0000CE97
		[SecurityCritical]
		private unsafe void UnsafeWriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, XmlBinaryNodeType nodeType, int count, byte* array, byte* arrayMax)
		{
			this.WriteStartArray(prefix, localName, namespaceUri, count);
			this.writer.UnsafeWriteArray(nodeType, count, array, arrayMax);
			this.WriteEndArray();
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000ECC0 File Offset: 0x0000CEC0
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

		// Token: 0x060002C0 RID: 704 RVA: 0x0000ED90 File Offset: 0x0000CF90
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, bool[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (bool* ptr = &array[offset])
				{
					bool* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.BoolTextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + count));
				}
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000EDEC File Offset: 0x0000CFEC
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, bool[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (bool* ptr = &array[offset])
				{
					bool* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.BoolTextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + count));
				}
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000EE48 File Offset: 0x0000D048
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, short[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (short* ptr = &array[offset])
				{
					short* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.Int16TextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + count));
				}
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000EEA8 File Offset: 0x0000D0A8
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, short[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (short* ptr = &array[offset])
				{
					short* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.Int16TextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + count));
				}
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000EF08 File Offset: 0x0000D108
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, int[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (int* ptr = &array[offset])
				{
					int* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.Int32TextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + count));
				}
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000EF68 File Offset: 0x0000D168
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, int[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (int* ptr = &array[offset])
				{
					int* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.Int32TextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + count));
				}
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000EFC8 File Offset: 0x0000D1C8
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, long[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (long* ptr = &array[offset])
				{
					long* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.Int64TextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + count));
				}
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000F028 File Offset: 0x0000D228
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, long[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (long* ptr = &array[offset])
				{
					long* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.Int64TextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + count));
				}
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000F088 File Offset: 0x0000D288
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, float[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (float* ptr = &array[offset])
				{
					float* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.FloatTextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + count));
				}
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000F0E8 File Offset: 0x0000D2E8
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, float[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (float* ptr = &array[offset])
				{
					float* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.FloatTextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + count));
				}
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000F148 File Offset: 0x0000D348
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, double[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (double* ptr = &array[offset])
				{
					double* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.DoubleTextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + count));
				}
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000F1A8 File Offset: 0x0000D3A8
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, double[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (double* ptr = &array[offset])
				{
					double* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.DoubleTextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + count));
				}
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000F208 File Offset: 0x0000D408
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, string localName, string namespaceUri, decimal[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (decimal* ptr = &array[offset])
				{
					decimal* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.DecimalTextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + (IntPtr)count * 16 / (IntPtr)sizeof(decimal)));
				}
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000F268 File Offset: 0x0000D468
		[SecuritySafeCritical]
		public unsafe override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, decimal[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				fixed (decimal* ptr = &array[offset])
				{
					decimal* ptr2 = ptr;
					this.UnsafeWriteArray(prefix, localName, namespaceUri, XmlBinaryNodeType.DecimalTextWithEndElement, count, (byte*)ptr2, (byte*)(ptr2 + (IntPtr)count * 16 / (IntPtr)sizeof(decimal)));
				}
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000F2C8 File Offset: 0x0000D4C8
		public override void WriteArray(string prefix, string localName, string namespaceUri, DateTime[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				this.WriteStartArray(prefix, localName, namespaceUri, count);
				this.writer.WriteDateTimeArray(array, offset, count);
				this.WriteEndArray();
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000F320 File Offset: 0x0000D520
		public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, DateTime[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				this.WriteStartArray(prefix, localName, namespaceUri, count);
				this.writer.WriteDateTimeArray(array, offset, count);
				this.WriteEndArray();
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000F378 File Offset: 0x0000D578
		public override void WriteArray(string prefix, string localName, string namespaceUri, Guid[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				this.WriteStartArray(prefix, localName, namespaceUri, count);
				this.writer.WriteGuidArray(array, offset, count);
				this.WriteEndArray();
			}
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000F3D0 File Offset: 0x0000D5D0
		public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, Guid[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				this.WriteStartArray(prefix, localName, namespaceUri, count);
				this.writer.WriteGuidArray(array, offset, count);
				this.WriteEndArray();
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000F428 File Offset: 0x0000D628
		public override void WriteArray(string prefix, string localName, string namespaceUri, TimeSpan[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				this.WriteStartArray(prefix, localName, namespaceUri, count);
				this.writer.WriteTimeSpanArray(array, offset, count);
				this.WriteEndArray();
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000F480 File Offset: 0x0000D680
		public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, TimeSpan[] array, int offset, int count)
		{
			if (base.Signing)
			{
				base.WriteArray(prefix, localName, namespaceUri, array, offset, count);
				return;
			}
			this.CheckArray(array, offset, count);
			if (count > 0)
			{
				this.WriteStartArray(prefix, localName, namespaceUri, count);
				this.writer.WriteTimeSpanArray(array, offset, count);
				this.WriteEndArray();
			}
		}

		// Token: 0x040001A6 RID: 422
		private XmlBinaryNodeWriter writer;

		// Token: 0x040001A7 RID: 423
		private char[] chars;

		// Token: 0x040001A8 RID: 424
		private byte[] bytes;
	}
}
