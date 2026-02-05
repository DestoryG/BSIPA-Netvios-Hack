using System;

namespace System.Net.Mail
{
	// Token: 0x02000284 RID: 644
	internal static class MailCommand
	{
		// Token: 0x06001819 RID: 6169 RVA: 0x0007AC7F File Offset: 0x00078E7F
		internal static IAsyncResult BeginSend(SmtpConnection conn, byte[] command, MailAddress from, bool allowUnicode, AsyncCallback callback, object state)
		{
			MailCommand.PrepareCommand(conn, command, from, allowUnicode);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x0007AC94 File Offset: 0x00078E94
		private static void CheckResponse(SmtpStatusCode statusCode, string response)
		{
			if (statusCode == SmtpStatusCode.Ok)
			{
				return;
			}
			if (statusCode - SmtpStatusCode.LocalErrorInProcessing > 1 && statusCode != SmtpStatusCode.ExceededStorageAllocation)
			{
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), response);
			}
			throw new SmtpException(statusCode, response, true);
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x0007ACD4 File Offset: 0x00078ED4
		internal static void EndSend(IAsyncResult result)
		{
			string text;
			SmtpStatusCode smtpStatusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out text);
			MailCommand.CheckResponse(smtpStatusCode, text);
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x0007ACF8 File Offset: 0x00078EF8
		private static void PrepareCommand(SmtpConnection conn, byte[] command, MailAddress from, bool allowUnicode)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(command);
			string smtpAddress = from.GetSmtpAddress(allowUnicode);
			conn.BufferBuilder.Append(smtpAddress, allowUnicode);
			if (allowUnicode)
			{
				conn.BufferBuilder.Append(" BODY=8BITMIME SMTPUTF8");
			}
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x0007AD64 File Offset: 0x00078F64
		internal static void Send(SmtpConnection conn, byte[] command, MailAddress from, bool allowUnicode)
		{
			MailCommand.PrepareCommand(conn, command, from, allowUnicode);
			string text;
			SmtpStatusCode smtpStatusCode = CheckCommand.Send(conn, out text);
			MailCommand.CheckResponse(smtpStatusCode, text);
		}
	}
}
