using System;

namespace System.Net
{
	// Token: 0x020000F0 RID: 240
	internal class FtpWebRequestCreator : IWebRequestCreate
	{
		// Token: 0x06000865 RID: 2149 RVA: 0x0002F26D File Offset: 0x0002D46D
		internal FtpWebRequestCreator()
		{
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0002F275 File Offset: 0x0002D475
		public WebRequest Create(Uri uri)
		{
			return new FtpWebRequest(uri);
		}
	}
}
