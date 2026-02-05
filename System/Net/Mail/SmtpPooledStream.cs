using System;

namespace System.Net.Mail
{
	// Token: 0x02000297 RID: 663
	internal class SmtpPooledStream : PooledStream
	{
		// Token: 0x0600189C RID: 6300 RVA: 0x0007D00F File Offset: 0x0007B20F
		internal SmtpPooledStream(ConnectionPool connectionPool, TimeSpan lifetime, bool checkLifetime)
			: base(connectionPool, lifetime, checkLifetime)
		{
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x0007D01C File Offset: 0x0007B21C
		protected override void Dispose(bool disposing)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, "SmtpPooledStream::Dispose #" + ValidationHelper.HashString(this));
			}
			if (disposing && base.NetworkStream.Connected)
			{
				this.Write(SmtpCommands.Quit, 0, SmtpCommands.Quit.Length);
				this.Flush();
				byte[] array = new byte[80];
				int num = this.Read(array, 0, 80);
			}
			base.Dispose(disposing);
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, "SmtpPooledStream::Dispose #" + ValidationHelper.HashString(this));
			}
		}

		// Token: 0x04001884 RID: 6276
		internal bool previouslyUsed;

		// Token: 0x04001885 RID: 6277
		internal bool dsnEnabled;

		// Token: 0x04001886 RID: 6278
		internal bool serverSupportsEai;

		// Token: 0x04001887 RID: 6279
		internal ICredentialsByHost creds;

		// Token: 0x04001888 RID: 6280
		private const int safeBufferLength = 80;
	}
}
