using System;

namespace System.Xml
{
	// Token: 0x0200000F RID: 15
	internal class SingleArrayHelperWithDictionaryString : ArrayHelper<XmlDictionaryString, float>
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00002419 File Offset: 0x00000619
		protected override int ReadArray(XmlDictionaryReader reader, XmlDictionaryString localName, XmlDictionaryString namespaceUri, float[] array, int offset, int count)
		{
			return reader.ReadArray(localName, namespaceUri, array, offset, count);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002429 File Offset: 0x00000629
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, float[] array, int offset, int count)
		{
			writer.WriteArray(prefix, localName, namespaceUri, array, offset, count);
		}

		// Token: 0x0400000C RID: 12
		public static readonly SingleArrayHelperWithDictionaryString Instance = new SingleArrayHelperWithDictionaryString();
	}
}
