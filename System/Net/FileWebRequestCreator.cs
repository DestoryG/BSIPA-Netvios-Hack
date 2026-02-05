using System;

namespace System.Net
{
	// Token: 0x020000E7 RID: 231
	internal class FileWebRequestCreator : IWebRequestCreate
	{
		// Token: 0x060007F2 RID: 2034 RVA: 0x0002BFD5 File Offset: 0x0002A1D5
		internal FileWebRequestCreator()
		{
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0002BFDD File Offset: 0x0002A1DD
		public WebRequest Create(Uri uri)
		{
			return new FileWebRequest(uri);
		}
	}
}
