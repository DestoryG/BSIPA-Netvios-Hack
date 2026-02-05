using System;

namespace System.Xml
{
	// Token: 0x02000018 RID: 24
	internal class TimeSpanArrayHelperWithString : ArrayHelper<string, TimeSpan>
	{
		// Token: 0x0600005A RID: 90 RVA: 0x000025FF File Offset: 0x000007FF
		protected override int ReadArray(XmlDictionaryReader reader, string localName, string namespaceUri, TimeSpan[] array, int offset, int count)
		{
			return reader.ReadArray(localName, namespaceUri, array, offset, count);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000260F File Offset: 0x0000080F
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, string localName, string namespaceUri, TimeSpan[] array, int offset, int count)
		{
			writer.WriteArray(prefix, localName, namespaceUri, array, offset, count);
		}

		// Token: 0x04000015 RID: 21
		public static readonly TimeSpanArrayHelperWithString Instance = new TimeSpanArrayHelperWithString();
	}
}
