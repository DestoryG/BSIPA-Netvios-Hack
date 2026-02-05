using System;
using System.IO;

namespace System.Xml
{
	// Token: 0x0200002D RID: 45
	public interface IXmlBinaryWriterInitializer
	{
		// Token: 0x06000276 RID: 630
		void SetOutput(Stream stream, IXmlDictionary dictionary, XmlBinaryWriterSession session, bool ownsStream);
	}
}
