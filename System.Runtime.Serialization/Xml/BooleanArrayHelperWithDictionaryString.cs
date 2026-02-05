using System;

namespace System.Xml
{
	// Token: 0x02000007 RID: 7
	internal class BooleanArrayHelperWithDictionaryString : ArrayHelper<XmlDictionaryString, bool>
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002269 File Offset: 0x00000469
		protected override int ReadArray(XmlDictionaryReader reader, XmlDictionaryString localName, XmlDictionaryString namespaceUri, bool[] array, int offset, int count)
		{
			return reader.ReadArray(localName, namespaceUri, array, offset, count);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002279 File Offset: 0x00000479
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, bool[] array, int offset, int count)
		{
			writer.WriteArray(prefix, localName, namespaceUri, array, offset, count);
		}

		// Token: 0x04000004 RID: 4
		public static readonly BooleanArrayHelperWithDictionaryString Instance = new BooleanArrayHelperWithDictionaryString();
	}
}
