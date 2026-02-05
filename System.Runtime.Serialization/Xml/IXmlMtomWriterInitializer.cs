using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200004A RID: 74
	public interface IXmlMtomWriterInitializer
	{
		// Token: 0x06000550 RID: 1360
		void SetOutput(Stream stream, Encoding encoding, int maxSizeInBytes, string startInfo, string boundary, string startUri, bool writeMessageHeaders, bool ownsStream);
	}
}
