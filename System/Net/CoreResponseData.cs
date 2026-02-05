using System;
using System.IO;

namespace System.Net
{
	// Token: 0x0200013A RID: 314
	internal class CoreResponseData
	{
		// Token: 0x06000B4F RID: 2895 RVA: 0x0003DF40 File Offset: 0x0003C140
		internal CoreResponseData Clone()
		{
			return new CoreResponseData
			{
				m_StatusCode = this.m_StatusCode,
				m_StatusDescription = this.m_StatusDescription,
				m_IsVersionHttp11 = this.m_IsVersionHttp11,
				m_ContentLength = this.m_ContentLength,
				m_ResponseHeaders = this.m_ResponseHeaders,
				m_ConnectStream = this.m_ConnectStream
			};
		}

		// Token: 0x04001074 RID: 4212
		public HttpStatusCode m_StatusCode;

		// Token: 0x04001075 RID: 4213
		public string m_StatusDescription;

		// Token: 0x04001076 RID: 4214
		public bool m_IsVersionHttp11;

		// Token: 0x04001077 RID: 4215
		public long m_ContentLength;

		// Token: 0x04001078 RID: 4216
		public WebHeaderCollection m_ResponseHeaders;

		// Token: 0x04001079 RID: 4217
		public Stream m_ConnectStream;
	}
}
