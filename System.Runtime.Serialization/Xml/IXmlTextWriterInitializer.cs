using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000059 RID: 89
	public interface IXmlTextWriterInitializer
	{
		// Token: 0x06000665 RID: 1637
		void SetOutput(Stream stream, Encoding encoding, bool ownsStream);
	}
}
