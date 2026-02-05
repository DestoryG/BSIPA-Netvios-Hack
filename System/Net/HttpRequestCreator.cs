using System;

namespace System.Net
{
	// Token: 0x02000138 RID: 312
	internal class HttpRequestCreator : IWebRequestCreate
	{
		// Token: 0x06000B4B RID: 2891 RVA: 0x0003DEBC File Offset: 0x0003C0BC
		public WebRequest Create(Uri Uri)
		{
			return new HttpWebRequest(Uri, null);
		}
	}
}
