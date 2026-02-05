using System;

namespace System.Net.Mail
{
	// Token: 0x02000280 RID: 640
	internal static class DataStopCommand
	{
		// Token: 0x06001807 RID: 6151 RVA: 0x0007A8FC File Offset: 0x00078AFC
		private static void CheckResponse(SmtpStatusCode statusCode, string serverResponse)
		{
			if (statusCode <= SmtpStatusCode.InsufficientStorage)
			{
				if (statusCode == SmtpStatusCode.Ok)
				{
					return;
				}
				if (statusCode - SmtpStatusCode.LocalErrorInProcessing > 1)
				{
				}
			}
			else if (statusCode != SmtpStatusCode.ExceededStorageAllocation && statusCode != SmtpStatusCode.TransactionFailed)
			{
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), serverResponse);
			}
			throw new SmtpException(statusCode, serverResponse, true);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x0007A959 File Offset: 0x00078B59
		private static void PrepareCommand(SmtpConnection conn)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.DataStop);
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x0007A984 File Offset: 0x00078B84
		internal static void Send(SmtpConnection conn)
		{
			DataStopCommand.PrepareCommand(conn);
			string text;
			SmtpStatusCode smtpStatusCode = CheckCommand.Send(conn, out text);
			DataStopCommand.CheckResponse(smtpStatusCode, text);
		}
	}
}
