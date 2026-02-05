using System;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000286 RID: 646
	internal static class SmtpCommands
	{
		// Token: 0x0400181B RID: 6171
		internal static readonly byte[] Auth = Encoding.ASCII.GetBytes("AUTH ");

		// Token: 0x0400181C RID: 6172
		internal static readonly byte[] CRLF = Encoding.ASCII.GetBytes("\r\n");

		// Token: 0x0400181D RID: 6173
		internal static readonly byte[] Data = Encoding.ASCII.GetBytes("DATA\r\n");

		// Token: 0x0400181E RID: 6174
		internal static readonly byte[] DataStop = Encoding.ASCII.GetBytes("\r\n.\r\n");

		// Token: 0x0400181F RID: 6175
		internal static readonly byte[] EHello = Encoding.ASCII.GetBytes("EHLO ");

		// Token: 0x04001820 RID: 6176
		internal static readonly byte[] Expand = Encoding.ASCII.GetBytes("EXPN ");

		// Token: 0x04001821 RID: 6177
		internal static readonly byte[] Hello = Encoding.ASCII.GetBytes("HELO ");

		// Token: 0x04001822 RID: 6178
		internal static readonly byte[] Help = Encoding.ASCII.GetBytes("HELP");

		// Token: 0x04001823 RID: 6179
		internal static readonly byte[] Mail = Encoding.ASCII.GetBytes("MAIL FROM:");

		// Token: 0x04001824 RID: 6180
		internal static readonly byte[] Noop = Encoding.ASCII.GetBytes("NOOP\r\n");

		// Token: 0x04001825 RID: 6181
		internal static readonly byte[] Quit = Encoding.ASCII.GetBytes("QUIT\r\n");

		// Token: 0x04001826 RID: 6182
		internal static readonly byte[] Recipient = Encoding.ASCII.GetBytes("RCPT TO:");

		// Token: 0x04001827 RID: 6183
		internal static readonly byte[] Reset = Encoding.ASCII.GetBytes("RSET\r\n");

		// Token: 0x04001828 RID: 6184
		internal static readonly byte[] Send = Encoding.ASCII.GetBytes("SEND FROM:");

		// Token: 0x04001829 RID: 6185
		internal static readonly byte[] SendAndMail = Encoding.ASCII.GetBytes("SAML FROM:");

		// Token: 0x0400182A RID: 6186
		internal static readonly byte[] SendOrMail = Encoding.ASCII.GetBytes("SOML FROM:");

		// Token: 0x0400182B RID: 6187
		internal static readonly byte[] Turn = Encoding.ASCII.GetBytes("TURN\r\n");

		// Token: 0x0400182C RID: 6188
		internal static readonly byte[] Verify = Encoding.ASCII.GetBytes("VRFY ");

		// Token: 0x0400182D RID: 6189
		internal static readonly byte[] StartTls = Encoding.ASCII.GetBytes("STARTTLS");
	}
}
