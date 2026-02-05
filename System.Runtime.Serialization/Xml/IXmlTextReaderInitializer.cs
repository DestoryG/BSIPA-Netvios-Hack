using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000057 RID: 87
	public interface IXmlTextReaderInitializer
	{
		// Token: 0x06000642 RID: 1602
		void SetInput(byte[] buffer, int offset, int count, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose);

		// Token: 0x06000643 RID: 1603
		void SetInput(Stream stream, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose);
	}
}
