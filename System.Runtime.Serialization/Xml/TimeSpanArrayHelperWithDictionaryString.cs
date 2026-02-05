using System;

namespace System.Xml
{
	// Token: 0x02000019 RID: 25
	internal class TimeSpanArrayHelperWithDictionaryString : ArrayHelper<XmlDictionaryString, TimeSpan>
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00002635 File Offset: 0x00000835
		protected override int ReadArray(XmlDictionaryReader reader, XmlDictionaryString localName, XmlDictionaryString namespaceUri, TimeSpan[] array, int offset, int count)
		{
			return reader.ReadArray(localName, namespaceUri, array, offset, count);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002645 File Offset: 0x00000845
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, TimeSpan[] array, int offset, int count)
		{
			writer.WriteArray(prefix, localName, namespaceUri, array, offset, count);
		}

		// Token: 0x04000016 RID: 22
		public static readonly TimeSpanArrayHelperWithDictionaryString Instance = new TimeSpanArrayHelperWithDictionaryString();
	}
}
