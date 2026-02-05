using System;

namespace System.Xml
{
	// Token: 0x0200000D RID: 13
	internal class Int64ArrayHelperWithDictionaryString : ArrayHelper<XmlDictionaryString, long>
	{
		// Token: 0x0600002E RID: 46 RVA: 0x000023AD File Offset: 0x000005AD
		protected override int ReadArray(XmlDictionaryReader reader, XmlDictionaryString localName, XmlDictionaryString namespaceUri, long[] array, int offset, int count)
		{
			return reader.ReadArray(localName, namespaceUri, array, offset, count);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000023BD File Offset: 0x000005BD
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, long[] array, int offset, int count)
		{
			writer.WriteArray(prefix, localName, namespaceUri, array, offset, count);
		}

		// Token: 0x0400000A RID: 10
		public static readonly Int64ArrayHelperWithDictionaryString Instance = new Int64ArrayHelperWithDictionaryString();
	}
}
