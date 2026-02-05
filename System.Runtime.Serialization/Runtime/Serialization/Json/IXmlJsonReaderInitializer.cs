using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x020000FE RID: 254
	[TypeForwardedFrom("System.ServiceModel.Web, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
	public interface IXmlJsonReaderInitializer
	{
		// Token: 0x06000FCF RID: 4047
		void SetInput(byte[] buffer, int offset, int count, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose);

		// Token: 0x06000FD0 RID: 4048
		void SetInput(Stream stream, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose);
	}
}
