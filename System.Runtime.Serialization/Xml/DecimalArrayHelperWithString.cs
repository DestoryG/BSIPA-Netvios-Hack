using System;

namespace System.Xml
{
	// Token: 0x02000012 RID: 18
	internal class DecimalArrayHelperWithString : ArrayHelper<string, decimal>
	{
		// Token: 0x06000042 RID: 66 RVA: 0x000024BB File Offset: 0x000006BB
		protected override int ReadArray(XmlDictionaryReader reader, string localName, string namespaceUri, decimal[] array, int offset, int count)
		{
			return reader.ReadArray(localName, namespaceUri, array, offset, count);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000024CB File Offset: 0x000006CB
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, string localName, string namespaceUri, decimal[] array, int offset, int count)
		{
			writer.WriteArray(prefix, localName, namespaceUri, array, offset, count);
		}

		// Token: 0x0400000F RID: 15
		public static readonly DecimalArrayHelperWithString Instance = new DecimalArrayHelperWithString();
	}
}
