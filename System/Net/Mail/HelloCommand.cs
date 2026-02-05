using System;

namespace System.Net.Mail
{
	// Token: 0x02000282 RID: 642
	internal static class HelloCommand
	{
		// Token: 0x0600180F RID: 6159 RVA: 0x0007AADD File Offset: 0x00078CDD
		internal static IAsyncResult BeginSend(SmtpConnection conn, string domain, AsyncCallback callback, object state)
		{
			HelloCommand.PrepareCommand(conn, domain);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x0007AAEE File Offset: 0x00078CEE
		private static void CheckResponse(SmtpStatusCode statusCode, string serverResponse)
		{
			if (statusCode == SmtpStatusCode.Ok)
			{
				return;
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), serverResponse);
			}
			throw new SmtpException(statusCode, serverResponse, true);
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x0007AB1C File Offset: 0x00078D1C
		internal static void EndSend(IAsyncResult result)
		{
			string text;
			SmtpStatusCode smtpStatusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out text);
			HelloCommand.CheckResponse(smtpStatusCode, text);
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x0007AB40 File Offset: 0x00078D40
		private static void PrepareCommand(SmtpConnection conn, string domain)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.Hello);
			conn.BufferBuilder.Append(domain);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x0007AB94 File Offset: 0x00078D94
		internal static void Send(SmtpConnection conn, string domain)
		{
			HelloCommand.PrepareCommand(conn, domain);
			string text;
			SmtpStatusCode smtpStatusCode = CheckCommand.Send(conn, out text);
			HelloCommand.CheckResponse(smtpStatusCode, text);
		}
	}
}
