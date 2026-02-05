using System;

namespace System.Xml
{
	// Token: 0x02000017 RID: 23
	internal class GuidArrayHelperWithDictionaryString : ArrayHelper<XmlDictionaryString, Guid>
	{
		// Token: 0x06000056 RID: 86 RVA: 0x000025C9 File Offset: 0x000007C9
		protected override int ReadArray(XmlDictionaryReader reader, XmlDictionaryString localName, XmlDictionaryString namespaceUri, Guid[] array, int offset, int count)
		{
			return reader.ReadArray(localName, namespaceUri, array, offset, count);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000025D9 File Offset: 0x000007D9
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, Guid[] array, int offset, int count)
		{
			writer.WriteArray(prefix, localName, namespaceUri, array, offset, count);
		}

		// Token: 0x04000014 RID: 20
		public static readonly GuidArrayHelperWithDictionaryString Instance = new GuidArrayHelperWithDictionaryString();
	}
}
