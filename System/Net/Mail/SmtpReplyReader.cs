using System;

namespace System.Net.Mail
{
	// Token: 0x02000293 RID: 659
	internal class SmtpReplyReader
	{
		// Token: 0x06001886 RID: 6278 RVA: 0x0007CA05 File Offset: 0x0007AC05
		internal SmtpReplyReader(SmtpReplyReaderFactory reader)
		{
			this.reader = reader;
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x0007CA14 File Offset: 0x0007AC14
		internal IAsyncResult BeginReadLines(AsyncCallback callback, object state)
		{
			return this.reader.BeginReadLines(this, callback, state);
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x0007CA24 File Offset: 0x0007AC24
		internal IAsyncResult BeginReadLine(AsyncCallback callback, object state)
		{
			return this.reader.BeginReadLine(this, callback, state);
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x0007CA34 File Offset: 0x0007AC34
		public void Close()
		{
			this.reader.Close(this);
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x0007CA42 File Offset: 0x0007AC42
		internal LineInfo[] EndReadLines(IAsyncResult result)
		{
			return this.reader.EndReadLines(result);
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x0007CA50 File Offset: 0x0007AC50
		internal LineInfo EndReadLine(IAsyncResult result)
		{
			return this.reader.EndReadLine(result);
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x0007CA5E File Offset: 0x0007AC5E
		internal LineInfo[] ReadLines()
		{
			return this.reader.ReadLines(this);
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x0007CA6C File Offset: 0x0007AC6C
		internal LineInfo ReadLine()
		{
			return this.reader.ReadLine(this);
		}

		// Token: 0x0400185D RID: 6237
		private SmtpReplyReaderFactory reader;
	}
}
