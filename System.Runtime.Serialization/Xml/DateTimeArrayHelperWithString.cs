using System;

namespace System.Xml
{
	// Token: 0x02000014 RID: 20
	internal class DateTimeArrayHelperWithString : ArrayHelper<string, DateTime>
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00002527 File Offset: 0x00000727
		protected override int ReadArray(XmlDictionaryReader reader, string localName, string namespaceUri, DateTime[] array, int offset, int count)
		{
			return reader.ReadArray(localName, namespaceUri, array, offset, count);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002537 File Offset: 0x00000737
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, string localName, string namespaceUri, DateTime[] array, int offset, int count)
		{
			writer.WriteArray(prefix, localName, namespaceUri, array, offset, count);
		}

		// Token: 0x04000011 RID: 17
		public static readonly DateTimeArrayHelperWithString Instance = new DateTimeArrayHelperWithString();
	}
}
