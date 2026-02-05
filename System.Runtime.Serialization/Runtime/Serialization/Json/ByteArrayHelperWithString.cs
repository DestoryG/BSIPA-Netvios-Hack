using System;
using System.Globalization;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x020000FB RID: 251
	internal class ByteArrayHelperWithString : ArrayHelper<string, byte>
	{
		// Token: 0x06000F78 RID: 3960 RVA: 0x000404D0 File Offset: 0x0003E6D0
		internal void WriteArray(XmlWriter writer, byte[] array, int offset, int count)
		{
			XmlJsonReader.CheckArray(array, offset, count);
			writer.WriteAttributeString(string.Empty, "type", string.Empty, "array");
			for (int i = 0; i < count; i++)
			{
				writer.WriteStartElement("item", string.Empty);
				writer.WriteAttributeString(string.Empty, "type", string.Empty, "number");
				writer.WriteValue((int)array[offset + i]);
				writer.WriteEndElement();
			}
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00040548 File Offset: 0x0003E748
		protected override int ReadArray(XmlDictionaryReader reader, string localName, string namespaceUri, byte[] array, int offset, int count)
		{
			XmlJsonReader.CheckArray(array, offset, count);
			int num = 0;
			while (num < count && reader.IsStartElement("item", string.Empty))
			{
				array[offset + num] = this.ToByte(reader.ReadElementContentAsInt());
				num++;
			}
			return num;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00040593 File Offset: 0x0003E793
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, string localName, string namespaceUri, byte[] array, int offset, int count)
		{
			this.WriteArray(writer, array, offset, count);
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x000405A2 File Offset: 0x0003E7A2
		private void ThrowConversionException(string value, string type)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("The value '{0}' cannot be parsed as the type '{1}'.", new object[] { value, type })));
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x000405C6 File Offset: 0x0003E7C6
		private byte ToByte(int value)
		{
			if (value < 0 || value > 255)
			{
				this.ThrowConversionException(value.ToString(NumberFormatInfo.CurrentInfo), "Byte");
			}
			return (byte)value;
		}

		// Token: 0x040007C3 RID: 1987
		public static readonly ByteArrayHelperWithString Instance = new ByteArrayHelperWithString();
	}
}
