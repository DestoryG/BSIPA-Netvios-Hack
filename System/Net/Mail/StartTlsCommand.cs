using System;

namespace System.Net.Mail
{
	// Token: 0x02000283 RID: 643
	internal static class StartTlsCommand
	{
		// Token: 0x06001814 RID: 6164 RVA: 0x0007ABB8 File Offset: 0x00078DB8
		internal static IAsyncResult BeginSend(SmtpConnection conn, AsyncCallback callback, object state)
		{
			StartTlsCommand.PrepareCommand(conn);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x0007ABC8 File Offset: 0x00078DC8
		private static void CheckResponse(SmtpStatusCode statusCode, string response)
		{
			if (statusCode == SmtpStatusCode.ServiceReady)
			{
				return;
			}
			if (statusCode != SmtpStatusCode.ClientNotPermitted)
			{
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), response);
			}
			throw new SmtpException(statusCode, response, true);
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x0007AC00 File Offset: 0x00078E00
		internal static void EndSend(IAsyncResult result)
		{
			string text;
			SmtpStatusCode smtpStatusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out text);
			StartTlsCommand.CheckResponse(smtpStatusCode, text);
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x0007AC22 File Offset: 0x00078E22
		private static void PrepareCommand(SmtpConnection conn)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.StartTls);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x0007AC5C File Offset: 0x00078E5C
		internal static void Send(SmtpConnection conn)
		{
			StartTlsCommand.PrepareCommand(conn);
			string text;
			SmtpStatusCode smtpStatusCode = CheckCommand.Send(conn, out text);
			StartTlsCommand.CheckResponse(smtpStatusCode, text);
		}
	}
}
