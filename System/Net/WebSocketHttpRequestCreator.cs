using System;
using System.Net.WebSockets;

namespace System.Net
{
	// Token: 0x02000139 RID: 313
	internal class WebSocketHttpRequestCreator : IWebRequestCreate
	{
		// Token: 0x06000B4D RID: 2893 RVA: 0x0003DECD File Offset: 0x0003C0CD
		public WebSocketHttpRequestCreator(bool usingHttps)
		{
			this.m_httpScheme = (usingHttps ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0003DEEC File Offset: 0x0003C0EC
		public WebRequest Create(Uri Uri)
		{
			HttpWebRequest httpWebRequest = new HttpWebRequest(new UriBuilder(Uri)
			{
				Scheme = this.m_httpScheme
			}.Uri, null, true, "WebSocket" + Guid.NewGuid().ToString());
			WebSocketHelpers.PrepareWebRequest(ref httpWebRequest);
			return httpWebRequest;
		}

		// Token: 0x04001073 RID: 4211
		private string m_httpScheme;
	}
}
