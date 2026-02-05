using System;

namespace System.Net.Mail
{
	// Token: 0x02000287 RID: 647
	internal struct LineInfo
	{
		// Token: 0x06001824 RID: 6180 RVA: 0x0007B025 File Offset: 0x00079225
		internal LineInfo(SmtpStatusCode statusCode, string line)
		{
			this.statusCode = statusCode;
			this.line = line;
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001825 RID: 6181 RVA: 0x0007B035 File Offset: 0x00079235
		internal string Line
		{
			get
			{
				return this.line;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001826 RID: 6182 RVA: 0x0007B03D File Offset: 0x0007923D
		internal SmtpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
		}

		// Token: 0x0400182E RID: 6190
		private string line;

		// Token: 0x0400182F RID: 6191
		private SmtpStatusCode statusCode;
	}
}
