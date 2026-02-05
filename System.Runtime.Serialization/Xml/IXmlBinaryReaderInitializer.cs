using System;
using System.IO;

namespace System.Xml
{
	// Token: 0x0200002A RID: 42
	public interface IXmlBinaryReaderInitializer
	{
		// Token: 0x0600021B RID: 539
		void SetInput(byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session, OnXmlDictionaryReaderClose onClose);

		// Token: 0x0600021C RID: 540
		void SetInput(Stream stream, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session, OnXmlDictionaryReaderClose onClose);
	}
}
