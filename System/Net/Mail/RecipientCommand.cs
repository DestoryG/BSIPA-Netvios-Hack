using System;

namespace System.Net.Mail
{
	// Token: 0x02000285 RID: 645
	internal static class RecipientCommand
	{
		// Token: 0x0600181E RID: 6174 RVA: 0x0007AD8A File Offset: 0x00078F8A
		internal static IAsyncResult BeginSend(SmtpConnection conn, string to, AsyncCallback callback, object state)
		{
			RecipientCommand.PrepareCommand(conn, to);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x0007AD9C File Offset: 0x00078F9C
		private static bool CheckResponse(SmtpStatusCode statusCode, string response)
		{
			if (statusCode <= SmtpStatusCode.MailboxBusy)
			{
				if (statusCode - SmtpStatusCode.Ok <= 1)
				{
					return true;
				}
				if (statusCode != SmtpStatusCode.MailboxBusy)
				{
					goto IL_0034;
				}
			}
			else if (statusCode != SmtpStatusCode.InsufficientStorage && statusCode - SmtpStatusCode.MailboxUnavailable > 3)
			{
				goto IL_0034;
			}
			return false;
			IL_0034:
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), response);
			}
			throw new SmtpException(statusCode, response, true);
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x0007AE00 File Offset: 0x00079000
		internal static bool EndSend(IAsyncResult result, out string response)
		{
			SmtpStatusCode smtpStatusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out response);
			return RecipientCommand.CheckResponse(smtpStatusCode, response);
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x0007AE24 File Offset: 0x00079024
		private static void PrepareCommand(SmtpConnection conn, string to)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.Recipient);
			conn.BufferBuilder.Append(to, true);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x0007AE78 File Offset: 0x00079078
		internal static bool Send(SmtpConnection conn, string to, out string response)
		{
			RecipientCommand.PrepareCommand(conn, to);
			SmtpStatusCode smtpStatusCode = CheckCommand.Send(conn, out response);
			return RecipientCommand.CheckResponse(smtpStatusCode, response);
		}
	}
}
