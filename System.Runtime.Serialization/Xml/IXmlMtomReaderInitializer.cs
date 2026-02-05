using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200003B RID: 59
	public interface IXmlMtomReaderInitializer
	{
		// Token: 0x0600049B RID: 1179
		void SetInput(byte[] buffer, int offset, int count, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose onClose);

		// Token: 0x0600049C RID: 1180
		void SetInput(Stream stream, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose onClose);
	}
}
