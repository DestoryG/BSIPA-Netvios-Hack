using System;

namespace System.Xml
{
	// Token: 0x0200000E RID: 14
	internal class SingleArrayHelperWithString : ArrayHelper<string, float>
	{
		// Token: 0x06000032 RID: 50 RVA: 0x000023E3 File Offset: 0x000005E3
		protected override int ReadArray(XmlDictionaryReader reader, string localName, string namespaceUri, float[] array, int offset, int count)
		{
			return reader.ReadArray(localName, namespaceUri, array, offset, count);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000023F3 File Offset: 0x000005F3
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, string localName, string namespaceUri, float[] array, int offset, int count)
		{
			writer.WriteArray(prefix, localName, namespaceUri, array, offset, count);
		}

		// Token: 0x0400000B RID: 11
		public static readonly SingleArrayHelperWithString Instance = new SingleArrayHelperWithString();
	}
}
