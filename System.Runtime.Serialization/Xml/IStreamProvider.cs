using System;
using System.IO;

namespace System.Xml
{
	// Token: 0x0200001C RID: 28
	public interface IStreamProvider
	{
		// Token: 0x0600008F RID: 143
		Stream GetStream();

		// Token: 0x06000090 RID: 144
		void ReleaseStream(Stream stream);
	}
}
