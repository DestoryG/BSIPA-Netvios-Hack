using System;

namespace System.Net.Mail
{
	// Token: 0x0200027F RID: 639
	internal static class DataCommand
	{
		// Token: 0x06001802 RID: 6146 RVA: 0x0007A83D File Offset: 0x00078A3D
		internal static IAsyncResult BeginSend(SmtpConnection conn, AsyncCallback callback, object state)
		{
			DataCommand.PrepareCommand(conn);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x0007A84D File Offset: 0x00078A4D
		private static void CheckResponse(SmtpStatusCode statusCode, string serverResponse)
		{
			if (statusCode == SmtpStatusCode.StartMailInput)
			{
				return;
			}
			if (statusCode != SmtpStatusCode.LocalErrorInProcessing && statusCode != SmtpStatusCode.TransactionFailed)
			{
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), serverResponse);
			}
			throw new SmtpException(statusCode, serverResponse, true);
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x0007A88C File Offset: 0x00078A8C
		internal static void EndSend(IAsyncResult result)
		{
			string text;
			SmtpStatusCode smtpStatusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out text);
			DataCommand.CheckResponse(smtpStatusCode, text);
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x0007A8AE File Offset: 0x00078AAE
		private static void PrepareCommand(SmtpConnection conn)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.Data);
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x0007A8D8 File Offset: 0x00078AD8
		internal static void Send(SmtpConnection conn)
		{
			DataCommand.PrepareCommand(conn);
			string text;
			SmtpStatusCode smtpStatusCode = CheckCommand.Send(conn, out text);
			DataCommand.CheckResponse(smtpStatusCode, text);
		}
	}
}
